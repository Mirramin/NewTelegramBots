using System;
using System.Collections.Generic;
using System.Linq;
using Telegram.Bot.Args;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;
using Telegram.Bot.Types.InlineQueryResults;
using TelegramBotMafia.Handlers;
using TelegramBotMafia.Interfaces;
using TelegramBotMafia.Models;
using Chat = TelegramBotMafia.Models.Chat;
using Game = TelegramBotMafia.Models.Game;

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
                        var chat = new Chat(e.Message.Chat.Id);
                        Chat.Chats.Add(chat);
                        chat.Garbage.Add(e.Message.MessageId);
                        
                        chat.Garbage.Add(Program.bot.SendTextMessageAsync(e.Message.Chat.Id, Text.StartInChat,
                            replyMarkup: Keyboards.CheckAdminsRoot()).Result.MessageId );
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
                            if (chat.CheckAdminsRoot().Result && !chat.InGame)
                            {
                                // Start game
                                var msg = await Program.bot.SendTextMessageAsync(chat.Id, Text.StartGame,
                                    replyMarkup: Keyboards.StartGame());

                                chat.MsgAboutStartGameId = msg.MessageId;
                                chat.InGame = true;
                                var game = new Game(chat);
                                Game.Games.Add(game);
                                await game.CloseConnectionsAndStartGame(chat.startTime);
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
                switch (e.Message.Text)
                {
                    case "/start connect":
                        var game = Game.GetGameWithUserId(e.Message.From.Id);
                        if (game != null)
                        {
                            game.AddUser(e.Message.From);
                        }
                        else
                        {
                            await Program.bot.SendTextMessageAsync(e.Message.From.Id,
                                "Я ще занадто молодий бот, щоб розпізнати цю команду(");
                        }
                        break;
                }
            }
        }
        
        public async static void OnCallbackQuery(object sender, CallbackQueryEventArgs e)
        {
            IGame game = Game.GetGameWithUserId(e.CallbackQuery.From.Id);
            
            
            if (game != null && game.room.InGame)
            {
                var role = game.roles.FirstOrDefault(x => x.user.Id == e.CallbackQuery.From.Id);
                if (role == null) return;
                
                if(role is Mafia)
                {
                    
                }
            }
            
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
                        chat.DeleteGarbage();
                    }
                    else
                    {
                        await Program.bot.AnswerCallbackQueryAsync(
                            e.CallbackQuery.Id, "Права не надані!", showAlert: true);
                    }


                    break;
                case "ConnectToGame":
                    if (game == null) return;

                    await Program.bot.AnswerCallbackQueryAsync(e.CallbackQuery.Id,
                        url: "https://t.me/CHNUEventBot?start=connect");
                    
                    game.ConnectedUsersId.Add(e.CallbackQuery.From.Id);
                    break;
            }
        }
    }
}