using System.Linq;
using Telegram.Bot.Args;
using Telegram.Bot.Types.Enums;
using TelegramBotMafia.Interfaces;
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
                else if(e.Message.LeftChatMember != null)
                {
                    Chat.Chats.Remove(Chat.GetChatOrNull(e.Message.Chat.Id));
                }
                else
                {
                    var chat = Chat.GetChatOrNull(e.Message.Chat.Id);

                    if (e.Message.Text == "/createChat")
                    {
                        Chat.Chats.Add(new Chat(e.Message.Chat.Id));
                        await Program.bot.SendTextMessageAsync(e.Message.Chat.Id, "Done");
                    }
                    
                    if (chat == null) // Створює новий екземпляр групи, якщо її не існувало раніше
                    {
                        return;
                    }

                    if (!chat.CheckAdminsRoot().Result)
                    {
                        await Program.bot.SendTextMessageAsync(chat.Id, Text.StartInChat,
                            replyMarkup: Keyboards.CheckAdminsRoot());
                        return;
                    }
                    
                    switch (e.Message.Text)
                    {
                        case "/game": // Start game
                            if (chat.CheckAdminsRoot().Result)
                            {
                                // Start game
                                var msg = await Program.bot.SendTextMessageAsync(chat.Id, Text.StartGame,
                                    replyMarkup: Keyboards.StartGame());

                                chat.MsgAboutStartGameId = msg.MessageId;
                                Game.Games.Add(new Game(chat));
                                
                            }
                            
                            break;
                        case "/setting": // Settings of game
                            if (
                                Program.bot.GetChatAdministratorsAsync(chat.Id).Result.FirstOrDefault(
                                    x => x.User.Id == e.Message.From.Id) != null)
                            {
                                await Program.bot.SendTextMessageAsync(
                                    e.Message.From.Id,
                                    "Виберіть, які налаштування ви б хотіли змінити:",
                                    replyMarkup: Keyboards.Settings()
                                );
                            }
                            else
                            {
                                await Program.bot.SendTextMessageAsync(e.Message.From.Id, "Ви не адміністрітор чату!");
                            }
                            break;
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
                        await Program.bot.AnswerCallbackQueryAsync(e.CallbackQuery.Id, "Права не надані!");
                    }


                    break;
                case "ConnectToGame":
                    IGame game = Game.GetGameOrNull(e.CallbackQuery.Message.Chat.Id);
                    if (game == null) return;
                    
                    
                    
                    game.AddUser(e.CallbackQuery.From);
                    
                    break;
            }
        }
    }
}