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
                    InlineKeyboardButton.WithCallbackData("Включити", $"AddNewChanel:{chanelId}"),
                    InlineKeyboardButton.WithCallbackData("Вийти з цього каналу", $"LeaveFromChanel:{chanelId}"),
                }
            });
        }
        
        public static InlineKeyboardMarkup SendMessageIntoAllChats(long msgId, long chanelId)
        {
            return new InlineKeyboardMarkup(new[]
            {
                new[]
                {
                    InlineKeyboardButton.WithCallbackData("Надіслати", $"SendToAllChats:{msgId}:{chanelId}"),
                    InlineKeyboardButton.WithCallbackData("Не надсилати", $"DontSendToAllChats:{msgId}{chanelId}"),
                }
            });
        }

        public static InlineKeyboardMarkup AddChatForSendMessage(long chatId)
        {
            return new InlineKeyboardMarkup(new[]
            {
                new[]
                {
                    InlineKeyboardButton.WithCallbackData("Добавити", $"AddChatForSendMessage:{chatId}"),
                    InlineKeyboardButton.WithCallbackData("Відмінити", $"DontAddChatForSendMessage:{chatId}"),
                }
            });
        }
    }
}