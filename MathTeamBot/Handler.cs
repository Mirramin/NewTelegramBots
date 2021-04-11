using System;
using System.Linq;
using Telegram.Bot.Args;
using Telegram.Bot.Types.Enums;

namespace MathTeamBot
{
    public class Handler
    {
        public static async void OnMessage(object sender, MessageEventArgs e)
        {
            
            
            // Вітка для обробки повідомлень із особистих повідомлень
            if (e.Message.Chat.Type == ChatType.Private)
            {

                await Program.bot.SendTextMessageAsync(e.Message.From.Id, "Було, є і буде так: кращим з кращих є МАТФАК ");

            }
            // Вітка для обробки повідомлень із груп та супергруп
            else
            {
                if (e.Message.NewChatMembers != null &&
                    e.Message.NewChatMembers.FirstOrDefault(x => x.Id == Program.bot.BotId) != null)
                {
                    await Program.bot.SendTextMessageAsync(
                        e.Message.Chat.Id,
                        Text.CheckAdminsRoots,
                        ParseMode.Html,
                        replyMarkup: Keyboards.CheckAdminsRoots()
                    );
                    return;
                }
                
                if (e.Message.LeftChatMember == null && !Program.CheckAdminsRoots(e.Message.Chat.Id).Result)
                {
                    await Program.bot.SendTextMessageAsync(
                        e.Message.Chat.Id,
                        Text.CheckAdminsRoots,
                        ParseMode.Html,
                        replyMarkup: Keyboards.CheckAdminsRoots()
                    );
                    return;
                }
                
                
                // Обробка повідомлень від власника (Гл. адміністратора)
                if (e.Message.From.Id == Settings.OWNER)
                {
                    switch (e.Message.Text)
                    {
                        // Зупинити роботу бота та зберегти все необхідне
                        case "/bot stop":
                            Program.Stop();
                            break;
                        case "/setchat":
                            Settings.MainChat = e.Message.Chat.Id;
                            await Program.bot.DeleteMessageAsync(e.Message.Chat.Id, e.Message.MessageId);
                            await Program.bot.SendTextMessageAsync(e.Message.Chat.Id, "Оновлено основний чат.");
                            break;
                        case "/start":
                            await Program.bot.SendTextMessageAsync(
                                e.Message.Chat.Id,
                                Text.AddChatForSendMessage(),
                                parseMode: ParseMode.Html,
                                replyMarkup: Keyboards.AddChatForSendMessage(e.Message.Chat.Id)
                            );
                            break;
                        case "/newadmin":
                            if (e.Message.ReplyToMessage != null)
                            {
                                Settings.Admins.Add(e.Message.ReplyToMessage.From.Id);
                                Settings.Moders.Add(e.Message.ReplyToMessage.From.Id);
                                await Program.bot.SendTextMessageAsync(
                                    e.Message.Chat.Id,
                                    $"Користувачу @{e.Message.ReplyToMessage.From.Username} надано права адміністратора"
                                );
                            }
                            break;
                    }
                } 
                
                // Обробка повідомлень від адміністраторів
                if (Settings.Admins.Contains(e.Message.From.Id))
                {
                    switch (e.Message.Text)
                    {
                        // Надіслати доступне меню дій для адміністраторів в чат
                        case "/start":
                            await Program.bot.SendTextMessageAsync(
                                e.Message.Chat.Id,
                                Text.AddChatForSendMessage(),
                                parseMode: ParseMode.Html,
                                replyMarkup: Keyboards.AddChatForSendMessage(e.Message.Chat.Id)
                            );
                            break;
                    }
                }
            }
        }

        public static async void OnCallBack(object sender, CallbackQueryEventArgs e)
        {
            // Розбиває запит на частини команди, щоб розпарсити її покомпонентно
            var Commands = e.CallbackQuery.Data.Split(":");

            // Якщо команда складалась із двох компонентів
            if (e.CallbackQuery.Message.Chat.Id == Settings.MainChat)
            {
                switch (Commands[0])
                {
                    // Обробробляє добавлення каналу в список каналів, з яких проводиться розсилка
                    case "AddNewChanel":
                        Settings.MajorChanels.Add(Convert.ToInt64(Commands[1]));
                        
                        Console.WriteLine(
                            $"<b>[log] Додано новий канал для розсилки. </b> \n" +
                            $"Додав - {e.CallbackQuery.From.Username}[{e.CallbackQuery.From.Id}]");
                        
                        await Program.bot.DeleteMessageAsync(
                            e.CallbackQuery.Message.Chat.Id,
                            e.CallbackQuery.Message.MessageId
                        );
                        
                        await Program.bot.SendTextMessageAsync(
                            e.CallbackQuery.Message.Chat.Id,
                            "[log]<b> Додано новий канал для розсилки. </b> \n" +
                            $"Додав - @{e.CallbackQuery.From.Username}[{e.CallbackQuery.From.Id}]",
                            parseMode: ParseMode.Html
                        );
                        break;
                    // Обробляє команду виходу із каналу
                    case "LeaveFromChanel":
                        await Program.bot.LeaveChatAsync(Commands[1]);
                        
                        Console.WriteLine(
                            $"[log] Я вийшов із каналу id {Commands[1]}. " +
                            $"Приказ від - {e.CallbackQuery.From.Username}[{e.CallbackQuery.From.Id}]");
                        
                        await Program.bot.DeleteMessageAsync(
                            e.CallbackQuery.Message.Chat.Id,
                            e.CallbackQuery.Message.MessageId
                        );
                        
                        await Program.bot.SendTextMessageAsync(
                            e.CallbackQuery.Message.Chat.Id,
                            $"[log]<b> Я вийшов із каналу id {Commands[1]}.</b> \n" +
                            $"Приказ від - @{e.CallbackQuery.From.Username}[{e.CallbackQuery.From.Id}] ",
                            parseMode: ParseMode.Html
                        );
                        break;
                    case "SendToAllChats":
                        await Program.bot.DeleteMessageAsync(e.CallbackQuery.Message.Chat.Id,
                            e.CallbackQuery.Message.MessageId);
                        await Program.bot.SendTextMessageAsync(
                            e.CallbackQuery.Message.Chat.Id,
                            "Повідомлення розіслано!");

                        foreach (var chatid in Settings.Chats)
                        {
                            await Program.bot.ForwardMessageAsync(
                                chatid,
                                Convert.ToInt64(Commands[2]),
                                Convert.ToInt32(Commands[1])
                            );
                        }
                        break;
                    case "DontSendToAllChats":
                        await Program.bot.DeleteMessageAsync(e.CallbackQuery.Message.Chat.Id,
                            e.CallbackQuery.Message.MessageId);
                        await Program.bot.SendTextMessageAsync(
                            e.CallbackQuery.Message.Chat.Id,
                            "Розсилку відмінено!");
                        break;
                }
            }
            
            switch (Commands[0])
            {
                case "CheckAdminsRoots":
                    if (Program.CheckAdminsRoots(e.CallbackQuery.Message.Chat.Id).Result)
                    {
                        await Program.bot.DeleteMessageAsync(
                            e.CallbackQuery.Message.Chat.Id, e.CallbackQuery.Message.MessageId);
                        await Program.bot.AnswerCallbackQueryAsync(
                            e.CallbackQuery.Id, "Я отримав права адміністратора!");
                    }
                    else
                    {
                        await Program.bot.AnswerCallbackQueryAsync(
                            e.CallbackQuery.Id, "Права не надані!", showAlert: true);
                    }
                    break;
                case "AddChatForSendMessage":
                    if (Settings.Admins.FirstOrDefault(x => x == e.CallbackQuery.From.Id) != 0)
                    {
                        Settings.Chats.Add(Convert.ToInt64(Commands[1]));
                        await Program.bot.SendTextMessageAsync(
                            e.CallbackQuery.Message.Chat.Id,
                            "Тепер ви завжди будете в курсі останніх новин!");
                        await Program.bot.DeleteMessageAsync(
                            e.CallbackQuery.Message.Chat.Id,
                            e.CallbackQuery.Message.MessageId);
                    }
                    else
                    {
                        await Program.bot.AnswerCallbackQueryAsync(
                            e.CallbackQuery.Id, "Ця функція доступна тільки профоргам!", showAlert: true);
                    }
                    break;
                case "DontAddChatForSendMessage":

                    if (Settings.Admins.FirstOrDefault(x => x == e.CallbackQuery.From.Id) != 0)
                    {
                        await Program.bot.SendTextMessageAsync(
                            e.CallbackQuery.Message.Chat.Id,
                            "Ну і ладно! Я образився!");
                        await Program.bot.DeleteMessageAsync(
                            e.CallbackQuery.Message.Chat.Id,
                            e.CallbackQuery.Message.MessageId);
                        await Program.bot.LeaveChatAsync(e.CallbackQuery.Message.Chat.Id);
                    }
                    else
                    {
                        await Program.bot.AnswerCallbackQueryAsync(
                            e.CallbackQuery.Id, "Ця функція доступна тільки профоргам!", showAlert: true);
                    }
                    break;
            }
            
        }

        public static async void OnUpdate(object sender, UpdateEventArgs e)
        {
            // Вітка для обробки повідомлень із каналів
            if (e.Update.Type == UpdateType.ChannelPost)
            {

                // Вітка обробки повідомлень в каналах, звідки буде проводитись розсилка
                if (Settings.MajorChanels.Contains(e.Update.ChannelPost.Chat.Id))
                {
                    // Пересилка цього повідомлення в чати
                    await Program.bot.SendTextMessageAsync(
                        Settings.MainChat,
                        Text.SendMessageIntoAllChats(e.Update.ChannelPost.Chat.Username),
                        parseMode: ParseMode.Html,
                        replyMarkup: Keyboards.SendMessageIntoAllChats(e.Update.ChannelPost.MessageId, e.Update.ChannelPost.Chat.Id)
                    );
                }
                // Вітка обробки повідомлень із невідомих каналів
                else
                {
                    // Якщо бота добавили на новий канал
                    if (e.Update.ChannelPost.Text == "/start")
                    {
                        await Program.bot.DeleteMessageAsync(e.Update.ChannelPost.Chat.Id,
                            e.Update.ChannelPost.MessageId);
                        await Program.bot.SendTextMessageAsync(
                            Settings.MainChat,
                            Text.AddInNewChanelMessage(e.Update.ChannelPost.Chat.Title, e.Update.ChannelPost.Chat.Username),
                            parseMode: ParseMode.Html,
                            replyMarkup: Keyboards.AddNewChanelMessage(e.Update.ChannelPost.Chat.Id)
                        );
                    }
                }
            }
        }
    }
}