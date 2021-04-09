using System;
using System.Linq;
using Telegram.Bot.Args;
using Telegram.Bot.Types.Enums;

namespace MathTeamBot
{
    public abstract class Handler
    {
        public static async void OnMessage(object sender, MessageEventArgs e)
        {
            // Вітка для обробки повідомлень із каналів
            if (e.Message.Chat.Type == ChatType.Channel)
            {  
                // Вітка обробки повідомлень в каналах, звідки буде проводитись розсилка
                if (Settings.MajorChanels.Contains(e.Message.Chat.Id))
                {
                    // Пересилка цього повідомлення в чати
                    await Program.bot.SendTextMessageAsync(
                        Settings.MainChat,
                        Text.SendMessageIntoAllChats(e.Message.Chat.Username),
                        parseMode: ParseMode.Html,
                        replyMarkup: Keyboards.SendMessageIntoAllChats(e.Message.MessageId, e.Message.Chat.Id)
                    );
                }
                // Вітка обробки повідомлень із невідомих каналів
                else
                {
                    // Якщо бота добавили на новий канал
                    if (e.Message.NewChatMembers.FirstOrDefault(x => x.Id == Program.bot.BotId) != null)
                    {
                        await Program.bot.SendTextMessageAsync(
                            Settings.MainChat,
                            Text.AddInNewChanelMessage(e.Message.Chat.Title, e.Message.Chat.Username),
                            parseMode: ParseMode.Html,
                            replyMarkup: Keyboards.AddNewChanelMessage(e.Message.Chat.Id)
                        );
                    }
                }
            }
            // Вітка для обробки повідомлень із особистих повідомлень
            else if (e.Message.Chat.Type == ChatType.Private)
            {
                
                
                
            }
            // Вітка для обробки повідомлень із груп та супергруп
            else
            {
                // Обробка повідомлень від власника (Гл. адміністратора)
                if (e.Message.From.Id == Settings.OWNER)
                {
                    switch (e.Message.Text)
                    {
                        // Розпочати роботу бота
                        case "/bot start": 
                            Program.Start();
                            break;
                        // Зупинити роботу бота та зберегти все необхідне
                        case "/bot stop":
                            Program.Stop();
                            break;
                            
                    }
                } 
                // Обрабка повідомлень від адміністраторів
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
                        Settings.MajorChanels.Add(Convert.ToInt32(Commands[1]));
                        
                        Console.WriteLine(
                            $"[log] Додано новий канал для розсилки. " +
                            $"Додав - {e.CallbackQuery.From.Username}[{e.CallbackQuery.From.Id}]");
                        
                        await Program.bot.DeleteMessageAsync(
                            e.CallbackQuery.Message.Chat.Id,
                            e.CallbackQuery.Message.MessageId
                        );
                        
                        await Program.bot.SendTextMessageAsync(
                            e.CallbackQuery.Message.Chat.Id,
                            "<h4>[log]</h4> Додано новий канал для розсилки. " +
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
                            $"<h4>[log]</h4> Я вийшов із каналу id {Commands[1]}. " +
                            $"Приказ від - @{e.CallbackQuery.From.Username}[{e.CallbackQuery.From.Id}]",
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
                                Convert.ToInt32(Commands[2]),
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
                    case "AddChatForSendMessage":
                        Settings.Chats.Add(Convert.ToInt32(Commands[1]));
                        await Program.bot.SendTextMessageAsync(
                            e.CallbackQuery.Message.Chat.Id,
                            "Тепер ви завжди будете в курсі останніх новин!");
                        await Program.bot.DeleteMessageAsync(
                            e.CallbackQuery.Message.Chat.Id,
                            e.CallbackQuery.Message.MessageId);
                        break;
                    case "DontAddChatForSendMessage":
                        await Program.bot.SendTextMessageAsync(
                            e.CallbackQuery.Message.Chat.Id,
                            "Ну і ладно! Я образився!");
                        await Program.bot.DeleteMessageAsync(
                            e.CallbackQuery.Message.Chat.Id,
                            e.CallbackQuery.Message.MessageId);
                        await Program.bot.LeaveChatAsync(e.CallbackQuery.Message.Chat.Id);
                        break;
                }
            }
            
            
        }
        
    }
}