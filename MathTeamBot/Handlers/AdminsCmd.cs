using System;
using System.Threading;
using System.Threading.Tasks;
using MathTeamBot.Models;
using Telegram.Bot.Args;
using Telegram.Bot.Types;

namespace MathTeamBot.Handlers
{
    public class AdminsCmd
    {
        public static async Task AdminsHandlers(object sender, CallbackQueryEventArgs e, string[] Commands)
        {
            if (Commands[0] == "Post")
            {
                var post = Post.GetPost(Convert.ToInt32(Commands[2]));
                if (post == null) return;
                

                    switch (Commands[1])
                {
                    // Публікація поста
                    case "Public":
                        var msg = Program.bot.SendPhotoAsync(Settings.Chanels[0], post.photo, post.text,
                            replyMarkup: Keyboards.GenerateMarkup(post.buttons)).Result;
                        await Program.bot.DeleteMessageAsync(post.owner.id, post.mainMsgId);
                        await Program.bot.DeleteMessageAsync(post.owner.id, post.msgPostId);
                        await Program.bot.SendTextMessageAsync(post.owner.id, "Пост надіслано на канал!",
                            replyMarkup: Keyboards.PostWasSended(msg.MessageId, post.id));
                        break;
                    // Відмінити відправку поста на канал та повернутись до редагування
                    case "CancelSending":
                        await Program.bot.DeleteMessageAsync(Settings.Chanels[0], Convert.ToInt32(Commands[3]));
                        post.msgPostId = Program.bot.SendPhotoAsync(post.owner.id, post.photo, post.text,
                            replyMarkup: Keyboards.GenerateMarkup(post.buttons)).Result.MessageId;
                        Thread.Sleep(2000);
                        post.mainMsgId = Program.bot.EditMessageTextAsync(post.owner.id, post.mainMsgId,
                            "Виберіть потрібну дію:", replyMarkup: Keyboards.CreatePost(post.id)).Result.MessageId;
                        break;
                    case "Done":
                        await Program.bot.DeleteMessageAsync(post.owner.id, e.CallbackQuery.Message.MessageId);
                        await Program.bot.AnswerCallbackQueryAsync(e.CallbackQuery.Id, "Ви молодчинка! Так тримати!");
                        Post.List.RemoveAll(x => x.id == Convert.ToInt32(Commands[2]));
                        post.step = 10;
                        await Program.bot.SendTextMessageAsync(post.owner.id, "Меню адміністратора:",
                            replyMarkup: Keyboards.AdminsMenu());
                        break;
                    // Обробка хендлера для додавання нової клавіші
                    case "AddButton":
                        post.step = 3;
                        await Program.bot.EditMessageTextAsync(
                            post.owner.id,
                            post.mainMsgId,
                            "Введіть заголовок вашої кнопки:", replyMarkup: Keyboards.PostBackToMenu(post.id));
                        break;
                    // Розпочати процедуру видалення клавіші
                    case "StartDeleteButton":
                        await Program.bot.EditMessageTextAsync(
                            post.owner.id,
                            post.mainMsgId,
                            "Виберіть клавішу, яку потрібно видалити:",
                            replyMarkup: Keyboards.GenerateMarkup(post));
                        break;
                    // Видалити клавішу по її ідентифікатору
                    case "DeleteButton":
                        post.buttons.RemoveAll(x => x.id == Convert.ToInt32(Commands[2]));
                        await Program.bot.EditMessageReplyMarkupAsync(post.owner.id, post.msgPostId,
                            replyMarkup: Keyboards.GenerateMarkup(post.buttons));
                        await Program.bot.EditMessageReplyMarkupAsync(post.owner.id, post.mainMsgId,
                            replyMarkup: Keyboards.GenerateMarkup(post));
                        await Program.bot.AnswerCallbackQueryAsync(e.CallbackQuery.Id, "Клавіша успішно видалена!");
                        break;
                    // Редагувати текст поста
                    case "EditText":
                        post.step = 5;
                        await Program.bot.EditMessageTextAsync(post.owner.id, post.mainMsgId,
                            "Введіть новий текст:", replyMarkup: Keyboards.PostBackToMenu(post.id));
                        break;
                    // Змінити фото поста
                    case "EditPhoto":
                        post.step = 6;
                        await Program.bot.EditMessageTextAsync(post.owner.id, post.mainMsgId,
                            "Надішліть мені нову картинку для цього поста",
                            replyMarkup: Keyboards.PostBackToMenu(post.id));
                        break;
                    // Відминити створення нового поста
                    case "CancelCreate":
                        
                        Post.List.RemoveAll(x => x.id == Convert.ToInt32(Commands[2]));
                        post.step = 10;
                        await Program.bot.SendTextMessageAsync(post.owner.id, "Меню адміністратора:",
                            replyMarkup: Keyboards.AdminsMenu());
                        await Program.bot.DeleteMessageAsync(post.owner.id, post.mainMsgId);
                        await Program.bot.DeleteMessageAsync(post.owner.id, post.msgPostId);
                        break;
                    // Повернення до набору клавіш головного меню
                    case "BackToMenu":
                        post.step = 2;
                        await Program.bot.EditMessageTextAsync(post.owner.id, post.mainMsgId,
                            "Виберіть потрібну дію:", replyMarkup: Keyboards.CreatePost(post.id));
                        break;
                }
            }
        }
    }
}