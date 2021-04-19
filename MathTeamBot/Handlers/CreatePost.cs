using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using MathTeamBot.Interfaces;
using MathTeamBot.Models;
using Telegram.Bot.Args;
using Telegram.Bot.Types.Enums;
using Telegram.Bot.Types.InputFiles;

namespace MathTeamBot.Handlers
{
    public static class CreatePost
    { 
        public static async Task Create(long userId)
        {
            IPost post = new Post(1);
            Post.List.Add(post);
            int step = 1;

            var add = new ManualResetEvent(false);
             EventHandler<MessageEventArgs> addPost = (object sender, MessageEventArgs e) =>
             {
                 if (userId != e.Message.From.Id) return;
                 if (userId != e.Message.Chat.Id) return;

                 if (post.step == 1) // Ввід поста
                 {
                     if (e.Message.Photo == null)
                     {
                         Program.bot.DeleteMessageAsync(userId, e.Message.MessageId);
                         Program.bot.EditMessageTextAsync(userId, post.mainMsgId,
                             "Я очікував на фото та текст :( Спробуйте ще раз, у Вас все вийде!");
                         return;
                     } else if (e.Message.Caption == null)
                     {
                         Program.bot.DeleteMessageAsync(userId, e.Message.MessageId);
                         Program.bot.EditMessageTextAsync(userId, post.mainMsgId,
                             "Я очікував на фото та текст :( Спробуйте ще раз, у Вас все вийде!");
                         return;
                     }
                     
                     
                     post.text = e.Message.Caption; 
                     post.photo = e.Message.Photo[0].FileId;
                     post.owner.id = userId;
                     post.owner.tgname = e.Message.From.Username;
                     post.owner.name = e.Message.From.FirstName + " " + e.Message.From.LastName;


                     Program.bot.DeleteMessageAsync(userId, e.Message.MessageId);
                     Program.bot.DeleteMessageAsync(userId, post.mainMsgId);
                     post.msgPostId = Program.bot.SendPhotoAsync(userId, new InputOnlineFile(post.photo), post.text).Result.MessageId;
                     Program.bot.SendChatActionAsync(userId, ChatAction.Typing);
                     Thread.Sleep(3000);
                     var text = e.Message.Caption.Split(" ");
                     var links = (from str in text where str.Contains("http") select str).ToList();
                     if (links.Count == 0)
                     {
                         post.mainMsgId = Program.bot.SendTextMessageAsync(userId, "Виберіть потрібну дію:",
                             replyMarkup: Keyboards.CreatePost(post.id)).Result.MessageId; 
                     }

                     // Добавити аналізатор лінків
                 }
                 else if (post.step == 2) // Порожня обробка (Стан, коли бот не очікує введення жодної інформації)
                 {
                     Program.bot.DeleteMessageAsync(userId, e.Message.MessageId);
                 }
                 else if (post.step == 3) // Додавання кнопки
                 { 
                     post.step++;
                     post.button = new Post.Button(post.buttons) {name = e.Message.Text};

                     Program.bot.DeleteMessageAsync(userId, e.Message.MessageId);
                     Program.bot.EditMessageTextAsync(post.owner.id, post.mainMsgId,
                         "Введіть посилання на яке буде перенаправлено користувача після натискання на цю клавішу");
                 }
                 else if (post.step == 4) // Додавання кнопки
                 { 
                     post.step = 2;
                     post.button.url = e.Message.Text;
                     post.buttons.Add(post.button);

                     Program.bot.DeleteMessageAsync(userId, e.Message.MessageId);
                     Program.bot.EditMessageTextAsync(post.owner.id, post.mainMsgId,
                         "Кнопка успішно добавлена! \nВиберіть наступну дію:",
                         replyMarkup: Keyboards.CreatePost(post.id));
                     Program.bot.EditMessageReplyMarkupAsync(userId, post.msgPostId,
                         Keyboards.GenerateMarkup(post.buttons));
                 }
                 else if (post.step == 5) // Редагувати текст поста
                 {
                     post.step = 2;
                     post.text = e.Message.Text;

                     Program.bot.DeleteMessageAsync(userId, e.Message.MessageId);
                     Program.bot.EditMessageCaptionAsync(userId, post.msgPostId, post.text,
                         replyMarkup: Keyboards.GenerateMarkup(post.buttons));
                     Program.bot.EditMessageTextAsync(userId, post.mainMsgId, "Виберіть потрібну дію:",
                         replyMarkup: Keyboards.CreatePost(post.id));
                 }
                 else if (post.step == 6) // Змінити картинку поста
                 {
                     Program.bot.DeleteMessageAsync(userId, e.Message.MessageId);

                     if (e.Message.Photo != null)
                     {
                         post.step = 2;
                         post.photo = e.Message.Photo[0].FileId;
                         Program.bot.DeleteMessageAsync(userId, post.mainMsgId);
                         Program.bot.DeleteMessageAsync(userId, post.msgPostId);
                         post.msgPostId = Program.bot.SendPhotoAsync(userId, new InputOnlineFile(post.photo), post.text,
                             replyMarkup: Keyboards.GenerateMarkup(post.buttons)).Result.MessageId;
                         Program.bot.SendChatActionAsync(userId, ChatAction.Typing);
                         Thread.Sleep(3000);
                         post.mainMsgId = Program.bot.SendTextMessageAsync(userId,
                             "Виберіть потрібну дію:", replyMarkup: Keyboards.CreatePost(post.id)).Result.MessageId;
                     }
                     else
                     {
                         Program.bot.EditMessageTextAsync(userId, post.mainMsgId,
                             "Я очікував на фото :( Спробуйте ще раз, у Вас все вийде!", 
                             replyMarkup: Keyboards.PostBackToMenu(post.id));
                     }
                 }
             };

          post.mainMsgId = Program.bot.SendTextMessageAsync(userId, "Надішліть ваш пост.").Result.MessageId;
          await Program.bot.SendChatActionAsync(userId, ChatAction.Typing);
          
          Program.bot.OnMessage += addPost;
          add.WaitOne();
          Program.bot.OnMessage -= addPost;
      }
    } 
    
}