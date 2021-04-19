using System.Collections.Generic;
using MathTeamBot.Interfaces;
using MathTeamBot.Models;
using Telegram.Bot.Types;
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

        public static InlineKeyboardMarkup CheckAdminsRoots()
        {
            return new InlineKeyboardMarkup(new[]
            {
                new[]
                {
                    InlineKeyboardButton.WithCallbackData("Готово", "CheckAdminsRoots"),
                }
            });
        }

        public static InlineKeyboardMarkup CancelNewModer(long chatId, long userId, string userName)
        {
            return new InlineKeyboardMarkup(new[]
            {
                InlineKeyboardButton.WithCallbackData("Відмінити",
                    $"CancelNewModer:{chatId}:{userId}:{userName}"),
            });
        }

        public static InlineKeyboardMarkup GiveMeModersRoot()
        {
            return new InlineKeyboardMarkup(new[]
            {
                InlineKeyboardButton.WithCallbackData("Отримати",
                    $"GiveMeModersRoot"),
            });
        }

        public static InlineKeyboardMarkup CreatePost(int postId)
        {
            return new InlineKeyboardMarkup(new[]
            {
                new[]
                {
                    InlineKeyboardButton.WithCallbackData("Опублікувати пост", $"Post:Public:{postId}"),
                },
                new[]
                {
                    InlineKeyboardButton.WithCallbackData("Добавити кнопку", $"Post:AddButton:{postId}"),
                },
                new[]
                {
                    InlineKeyboardButton.WithCallbackData("Видалити кнопку", $"Post:StartDeleteButton:{postId}"),
                },
                new []
                {
                    InlineKeyboardButton.WithCallbackData("Редагувати текст", $"Post:EditText:{postId}"), 
                },
                new []
                {
                    InlineKeyboardButton.WithCallbackData("Змінити картинку", $"Post:EditPhoto:{postId}"),   
                },
                new[]
                {
                    InlineKeyboardButton.WithCallbackData("Відмінити", $"Post:CancelCreate:{postId}"),
                }
            });
        }

        public static InlineKeyboardMarkup GenerateMarkup(List<Post.Button> buttons)
        {
            List<InlineKeyboardButton[]> markup = new List<InlineKeyboardButton[]>();
            
            foreach (var btn in buttons)
            {
                markup.Add(new[] {InlineKeyboardButton.WithUrl(btn.name, btn.url)} );
            }

            return new InlineKeyboardMarkup(markup.ToArray());
        }

        public static InlineKeyboardMarkup GenerateMarkup(IPost post)
        {
            List<InlineKeyboardButton[]> markup = new List<InlineKeyboardButton[]>();
            
            foreach (var btn in post.buttons)
            {
                markup.Add(new[] {InlineKeyboardButton.WithCallbackData(btn.name, 
                    $"Post:DeleteButton:{btn.id}")} );
            }
            markup.Add(new[] {InlineKeyboardButton.WithCallbackData("Назад", 
                $"Post:BackToMenu:{post.id}")} );
            
            return new InlineKeyboardMarkup(markup.ToArray());
        }

        public static InlineKeyboardMarkup PostBackToMenu(int postId)
        {
            return new InlineKeyboardMarkup(new[]
            {
                new[]
                {
                    InlineKeyboardButton.WithCallbackData("Відмінити", $"Post:BackToMenu:{postId}")
                }
            });
        }

        public static InlineKeyboardMarkup PostWasSended(int msgId, int postId)
        {
            return new InlineKeyboardMarkup(new[]
            {
                new[]
                {
                    InlineKeyboardButton.WithCallbackData("Відмінити", $"Post:CancelSending:{postId}:{msgId}"),
                    InlineKeyboardButton.WithCallbackData("Все гуд", $"Post:Done:{postId}"),
                }
            });
        }

        public static InlineKeyboardMarkup AdminsMenu()
        {
            return new InlineKeyboardMarkup(new[]
            {
                new[]
                {
                    InlineKeyboardButton.WithCallbackData("Пост", "StartCreatingPost"),
                },
                new[]
                {
                    InlineKeyboardButton.WithCallbackData("Допомога", "AdminsHelp"),
                }
            });
        }
        
        
    }
}