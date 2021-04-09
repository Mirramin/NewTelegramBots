using System.Collections.Generic;
using System.Linq;
using Telegram.Bot.Types;
using Telegram.Bot.Types.ReplyMarkups;
using TelegramBotMafia.Interfaces;

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
                }
            });
        }

        public static InlineKeyboardMarkup ChoiseMafia(List<User> users)
        {
            IEnumerable<InlineKeyboardButton> markup = new List<InlineKeyboardButton>();
            foreach (var user in users)
            {
                markup.Append(
                    InlineKeyboardButton.WithCallbackData($"{user.FirstName} {user.LastName}", $"{user.Id}")
                );
            }
            
            return new InlineKeyboardMarkup(markup);
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