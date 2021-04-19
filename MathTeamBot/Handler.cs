using System;
using System.Linq;
using MathTeamBot.Handlers;
using Telegram.Bot.Args;
using Telegram.Bot.Types.Enums;

namespace MathTeamBot
{
    public class Handler
    {
        public static async void OnMessage(object sender, MessageEventArgs e)
        {
            // Вітка для обробки повідомлень із приватних чатів
            if (e.Message.Chat.Type == ChatType.Private)
            {
                if (Settings.Admins.Contains(e.Message.From.Id))
                {
                    switch (e.Message.Text)
                    {
                        case "!adm":
                            await Program.bot.SendTextMessageAsync(e.Message.From.Id, "Меню адміністратора:",
                                replyMarkup: Keyboards.AdminsMenu());
                            break;
                        
                        case "!post":
                            await Program.bot.DeleteMessageAsync(e.Message.From.Id, e.Message.MessageId);
                            await CreatePost.Create(e.Message.From.Id);
                            break;
                    }
                }
                else
                {
                    await Program.bot.SendTextMessageAsync(e.Message.From.Id, "Було, є і буде так: кращим з кращих є МАТФАК ");
                }
            }
            // Вітка для обробки повідомлень із груп та супергруп
            else
            {
                // При додаванні бота в якийсь чат 
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
                
                // Перевіряє права адміністратора
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
                            DataBase.DB.DeleteMainChat(Settings.MainChat);
                            Settings.MainChat = e.Message.Chat.Id;
                            DataBase.DB.AddChat(Settings.MainChat, true);
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
                                DataBase.DB.AddMembers("admins", e.Message.ReplyToMessage.From.Id);
                                Settings.Moders.Add(e.Message.ReplyToMessage.From.Id);
                                DataBase.DB.AddMembers("moders", e.Message.ReplyToMessage.From.Id);
                                await Program.bot.SendTextMessageAsync(
                                    e.Message.Chat.Id,
                                    $"Користувачу @{e.Message.ReplyToMessage.From.Username} надано права адміністратора"
                                );
                            }
                            break;
                    }
                }
                // Обробка повідомлень від адміністраторів
                else if (Settings.Admins.Contains(e.Message.From.Id))
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
                        // Добавити нового профорга
                        case "/newmoder":
                            // Якщо до цього повідомлення прикріплено повідомлення від іншого користувача
                            if (e.Message.ReplyToMessage != null)
                            {
                                if (Settings.Moders.FirstOrDefault(x => x == e.Message.ReplyToMessage.From.Id) != 0)
                                {
                                    await Program.bot.SendTextMessageAsync(
                                        e.Message.Chat.Id,
                                        $"У користувача @{e.Message.ReplyToMessage.From.Username} уже є права профорга"
                                    );
                                    return;
                                }

                                Settings.Moders.Add(e.Message.ReplyToMessage.From.Id);
                                DataBase.DB.AddMembers("moders", e.Message.ReplyToMessage.From.Id);
                                await Program.bot.SendTextMessageAsync(
                                    Settings.MainChat,
                                    $"Користувачу @{e.Message.ReplyToMessage.From.Username} надано права профорга",
                                    replyMarkup: Keyboards.CancelNewModer(e.Message.Chat.Id, e.Message.From.Id, e.Message.From.Username)
                                );
                                await Program.bot.SendTextMessageAsync(
                                    e.Message.Chat.Id,
                                    $"Користувачу @{e.Message.ReplyToMessage.From.Username} надано права профорга"
                                );
                            }
                            // Якщо не прикріплено - надати можливість приєднатись усім учасникам чату
                            else
                            {
                                await Program.bot.SendTextMessageAsync(
                                    e.Message.Chat.Id,
                                    "Для отримання прав профорга - натисність \"Отримати\"!",
                                    replyMarkup: Keyboards.GiveMeModersRoot()
                                );
                                await Program.bot.SendTextMessageAsync(
                                    Settings.MainChat,
                                    $"Адміністратор @{e.Message.From.Username} ініціював видачу прав профорга!"
                                );
                            }
                            break;
                    }
                }
                // Обробка повідомлень від модераторів (профоргів)
                else if (Settings.Moders.Contains(e.Message.From.Id))
                {
                    
                } 
                // Фільтрація повідомлень в чатах
                
            }
        }

        public static async void OnCallBack(object sender, CallbackQueryEventArgs e)
        {
            // Розбиває запит на частини команди, щоб розпарсити її покомпонентно
            var Commands = e.CallbackQuery.Data.Split(":");

            // Якщо команда була відправлена із головного чату
            if (e.CallbackQuery.Message.Chat.Id == Settings.MainChat)
            {
                switch (Commands[0])
                {
                    // Обробробляє добавлення каналу в список каналів, з яких проводиться розсилка
                    case "AddNewChanel":
                        Settings.Chanels.Add(Convert.ToInt64(Commands[1]));
                        DataBase.DB.AddChanel(Convert.ToInt64(Commands[1]));
                        
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
                    // Переслати повідомлення у всі чати
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
                    // Не пересилати повідомлення із каналу у всі чати
                    case "DontSendToAllChats":
                        await Program.bot.DeleteMessageAsync(e.CallbackQuery.Message.Chat.Id,
                            e.CallbackQuery.Message.MessageId);
                        await Program.bot.SendTextMessageAsync(
                            e.CallbackQuery.Message.Chat.Id,
                            "Розсилку відмінено!");
                        break;
                    // Відмінити видачу прав модератора (профорга)
                    case "CancelNewModer":
                        Settings.Moders.Remove(Convert.ToInt64(Commands[2]));
                        DataBase.DB.DeleteModerRoot(Convert.ToInt64(Commands[2]));
                        await Program.bot.EditMessageTextAsync(
                            e.CallbackQuery.Message.Chat.Id,
                            e.CallbackQuery.Message.MessageId,
                            "Відмінено!"
                        );

                        string msg = $"Адміністратор зняв з @{Commands[3]} права профорга.";

                        await Program.bot.SendTextMessageAsync(
                            Convert.ToInt64(Commands[1]),
                            msg
                        );
                        break;
                    
                }
            } 
            else if (Settings.Admins.Contains(e.CallbackQuery.Message.Chat.Id))
            {
                // Запустити процес створення поста
                if (e.CallbackQuery.Data == "StartCreatingPost")
                {
                    await Program.bot.DeleteMessageAsync(e.CallbackQuery.Message.Chat.Id,
                        e.CallbackQuery.Message.MessageId);
                    await CreatePost.Create(e.CallbackQuery.From.Id);
                } 
                else if (e.CallbackQuery.Data == "AdminsHelp")
                {
                    await Program.bot.AnswerCallbackQueryAsync(e.CallbackQuery.Id, "В розробці!");
                }
                else
                {
                    await AdminsCmd.AdminsHandlers(sender, e, Commands);   
                }
            }
            
            switch (Commands[0])
            {
                // Перевіряє, що боту надані права адміністратора
                case "CheckAdminsRoots":
                    if (Program.CheckAdminsRoots(e.CallbackQuery.Message.Chat.Id).Result)
                    {
                        await Program.bot.AnswerCallbackQueryAsync(
                            e.CallbackQuery.Id, "Я отримав права адміністратора!");
                        await Program.bot.EditMessageTextAsync(
                            e.CallbackQuery.Message.Chat.Id,
                            e.CallbackQuery.Message.MessageId,
                            "Добавити розсилку повідомлень в цю групу із каналу t.me/mathfamily?",
                            replyMarkup: Keyboards.AddChatForSendMessage(e.CallbackQuery.Message.Chat.Id)
                        );
                    }
                    else
                    {
                        await Program.bot.AnswerCallbackQueryAsync(
                            e.CallbackQuery.Id, "Права не надані!", showAlert: true);
                    }
                    break;
                // Добавляє чат для розсилки повідомлень
                case "AddChatForSendMessage":
                    if (Settings.Moders.FirstOrDefault(x => x == e.CallbackQuery.From.Id) != 0)
                    {
                        Settings.Chats.Add(Convert.ToInt64(Commands[1]));
                        DataBase.DB.AddChat(Convert.ToInt64(Commands[1]));
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
                // Не добавляти чат для розсилки та вийти з чату
                case "DontAddChatForSendMessage":

                    if (Settings.Moders.FirstOrDefault(x => x == e.CallbackQuery.From.Id) != 0)
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
                // Надати права модератора (профорга)
                case "GiveMeModersRoot":
                    if (Settings.Moders.FirstOrDefault(x => x == e.CallbackQuery.From.Id) != 0)
                    {
                        await Program.bot.AnswerCallbackQueryAsync(e.CallbackQuery.Id,
                            "У вас уже є права профорга!", true);
                        return;
                    }

                    Settings.Moders.Add(e.CallbackQuery.From.Id);
                    DataBase.DB.AddMembers("moders", e.CallbackQuery.From.Id);
                    await Program.bot.SendTextMessageAsync(
                        Settings.MainChat,
                        $"Користувачу @{e.CallbackQuery.From.Username} надано права профорга",
                        replyMarkup: Keyboards.CancelNewModer(e.CallbackQuery.Message.Chat.Id, e.CallbackQuery.From.Id, e.CallbackQuery.From.Username)
                    );
                    await Program.bot.SendTextMessageAsync(
                        e.CallbackQuery.Message.Chat.Id,
                        $"Користувачу @{e.CallbackQuery.From.Username} надано права профорга"
                    );
                    break;
            }
            
        }

        public static async void OnUpdate(object sender, UpdateEventArgs e)
        {
            // Вітка для обробки повідомлень із каналів
            if (e.Update.Type == UpdateType.ChannelPost)
            {

                // Вітка обробки повідомлень в каналах, звідки буде проводитись розсилка
                if (Settings.Chanels.Contains(e.Update.ChannelPost.Chat.Id))
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