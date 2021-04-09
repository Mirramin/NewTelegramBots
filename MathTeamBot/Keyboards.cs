using Telegram.Bot.Types.ReplyMarkups;

namespace MathTeamBot
{
    public abstract class Keyboards
    {
        public static InlineKeyboardMarkup AddNewChanelMessage(long chanelId)
        {
            return new InlineKeyboardMarkup(new[]
            {
                new[]
                {
                    InlineKeyboardButton.WithCallbackData("Підтвердити", $"AddNewChanel:{chanelId}"),
                    InlineKeyboardButton.WithCallbackData("Вийти з цього каналу", $"LeaveFromChanel:{chanelId}"),
                }
            });
        }
    }
}