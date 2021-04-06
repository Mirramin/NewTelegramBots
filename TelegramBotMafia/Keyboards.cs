using Telegram.Bot.Types.ReplyMarkups;

namespace TelegramBotMafia
{
    public class Keyboards
    {
        public static InlineKeyboardMarkup CheckAdminsRoot()
        {
            return new InlineKeyboardMarkup(new[]
            {
                new[]
                {
                    InlineKeyboardButton.WithCallbackData("Готово", "CheckAdminsRoot"),
                }
            });
        }

        public static InlineKeyboardMarkup StartGame()
        {
            return new InlineKeyboardMarkup(new[]
            {
                new[]
                {
                    InlineKeyboardButton.WithCallbackData("Приєднатись", "ConnectToGame"),
                    InlineKeyboardButton.WithSwitchInlineQuery("test", "/start"),  
                    
                }
            });
        }

        public static InlineKeyboardMarkup Settings()
        {
            return new InlineKeyboardMarkup(new[]
            {
                new[]
                {
                    InlineKeyboardButton.WithCallbackData(""), 
                }
            });
        }
    }
}