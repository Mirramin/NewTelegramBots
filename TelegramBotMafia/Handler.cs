using System.Linq;
using Telegram.Bot.Args;
using Telegram.Bot.Types.Enums;
using TelegramBotMafia.Models;

namespace TelegramBotMafia
{
    public class Handler
    {
        public static async void OnMessage(object sender, MessageEventArgs e)
        {
            // Вітка для обробки повідомлень в групі
            if (e.Message.Chat.Type == ChatType.Group || e.Message.Chat.Type == ChatType.Supergroup)
            {
                if (e.Message.NewChatMembers != null) // Реагує на додавання нових користувачів
                {
                    if (e.Message.NewChatMembers.FirstOrDefault(x => x.Id == Program.bot.BotId) != null)
                    {
                        await Program.bot.SendChatActionAsync(e.Message.Chat.Id, ChatAction.Typing);
                        // Connect to DB
                        
                        Chat.Chats.Add(new Chat(e.Message.Chat.Id));
                        
                         await Program.bot.SendTextMessageAsync(e.Message.Chat.Id, Text.StartInChat,
                            replyMarkup: Keyboards.CheckAdminsRoot());
                    }
                }
            }
            // Вітка для обробки приватних повідомлень
            else
            {
                
            }
        }
        
        public async static void OnCallbackQuery(object sender, CallbackQueryEventArgs e)
        {

            switch (e.CallbackQuery.Data)
            {
                case "CheckAdminsRoot":
                    var chat = Chat.GetChatOrNull(e.CallbackQuery.Message.Chat.Id);

                    if (chat == null) break;

                    if (chat.CheckAdminsRoot().Result)
                    {
                        await Program.bot.SendTextMessageAsync(chat.Id,
                            "Я отримав права! \nМожемо розпочинати гру - /game");
                        
                        Chat.ActivChats.Add(chat);
                    }
                    else
                    {
                        await Program.bot.SendTextMessageAsync(chat.Id, Text.StartInChat,
                            replyMarkup: Keyboards.CheckAdminsRoot());
                    }


                    break;
            }
        }
    }
}