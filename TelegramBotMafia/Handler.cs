using System.Linq;
using Telegram.Bot.Args;
using Telegram.Bot.Types.Enums;

namespace TelegramBotMafia
{
    public class Handler
    {
        public static async void OnMessage(object sender, MessageEventArgs e)
        {
            // Вітка для обробки повідомлень в групі
            if (e.Message.Chat.Type == ChatType.Group)
            {
                if (e.Message.NewChatMembers != null) // Реагує на додавання нових користувачів
                {
                    if (e.Message.NewChatMembers.FirstOrDefault(x => x.Id == Program.bot.BotId) != null)
                    {
                        await Program.bot.SendChatActionAsync(e.Message.Chat.Id, ChatAction.Typing);
                        // Connect to DB

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
        
        public static void OnCallbackQuery(object sender, CallbackQueryEventArgs e)
        {
            
        }
    }
}