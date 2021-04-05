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
    }
}