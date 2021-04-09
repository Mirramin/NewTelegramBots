using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;
using TelegramBotMafia.Interfaces;

namespace TelegramBotMafia.Models
{
    public class Game : IGame
    {
        public Game(IChat chat)
        {
            room = chat;
            players = new List<User>();
            ConnectedUsersId = new List<long>();
            roles = new List<IRole>();
        }
        
        
        public IChat room { get; set; }
        public List<User> players { get; set; }
        public List<IRole> roles { get; set; }
        public List<long> ConnectedUsersId { get; set; }
        
        

        public string PlayersToString()
        {
            string result = "";
            foreach (var player in players)
            {
                result += ", @" + player.Username;
            }

            return result;
        }

        public async void AddUser(User user)
        {
            if (players.FirstOrDefault(x => x.Id == user.Id) == null)
            {
                await Program.bot.SendTextMessageAsync(user.Id,
                    "Ви успішно приєднались до гри! \nОчікуйте на свою роль!");
                
                players.Add(user);
                await Program.bot.EditMessageTextAsync(room.Id, room.MsgAboutStartGameId,
                    Text.StartGame + PlayersToString(), replyMarkup: Keyboards.StartGame());
            }
            else
            {
                await Program.bot.SendTextMessageAsync(user.Id,
                    "Розбійник, Ви уже в грі! Очікуйте початку!");
            }
        }

        public async Task CloseConnectionsAndStartGame(int time)
        {
            
            Thread.Sleep((time-30)*1000);
            var msg = await Program.bot.SendTextMessageAsync(room.Id, Text.RemsgAboutStartGame);
            Thread.Sleep(30 * 1000);
            await Program.bot.DeleteMessageAsync(room.Id, room.MsgAboutStartGameId);
            await Program.bot.DeleteMessageAsync(room.Id, msg.MessageId);

            if (players.Count < 3)
            {
                await Program.bot.SendTextMessageAsync(room.Id, "Недостатньо гравців для початку гри!");
                Games.Remove(this);
                room.InGame = false;
                return;
            }
            
            StartGame();
        }

        private async void StartGame()
        {
            await Program.bot.SendTextMessageAsync(room.Id, "Гра почалась!");
            GiveRoles();
            await Program.bot.SendChatActionAsync(room.Id, ChatAction.Typing);
            Thread.Sleep(5000);
            await Program.bot.SendTextMessageAsync(room.Id, "Місто засинає.. Прокидається Мафія.");
            
        }

        private async void StartNigthVoting()
        {
            
        }
        
        private async void GiveRoles()
        {
            int num;
            
            int countPlayers = players.Count;
            for (int i = 0; i < room.minCounterPeople; i++)
            {
                num = new Random().Next(0, players.Count-1);
                roles.Add(new People(players[num]));
                await Program.bot.SendTextMessageAsync(players[num].Id, "Твоя роль - мирний \n\n" + Text.AboutPeople);
                players.RemoveAt(num);
            }
            
            
            
            num = new Random().Next(0, players.Count-1);
            roles.Add(new Don(players[num]));
            await Program.bot.SendTextMessageAsync(players[num].Id, "Твоя роль - Дон \n\n" + Text.AboutDon);
            players.RemoveAt(num);
            
            for (int i = 0; i < countPlayers / room.mafiaToPeople - 1; i++)
            {
                num = new Random().Next(0, players.Count-1);
                roles.Add(new Mafia(players[num]));
                await Program.bot.SendTextMessageAsync(players[num].Id, "Твоя роль - Мафія \n\n" + Text.AboutMafia);
                players.RemoveAt(num);
            }

            if (players.Count > 0)
            {
                num = new Random().Next(0, players.Count-1);
                roles.Add(new Doctor(players[num]));
                await Program.bot.SendTextMessageAsync(players[num].Id, "Твоя роль - Доктор \n\n" + Text.AboutDoc);
                players.RemoveAt(num);
            }
            
            if (players.Count > 0)
            {
                num = new Random().Next(0, players.Count-1);
                roles.Add(new Com(players[num]));
                await Program.bot.SendTextMessageAsync(players[num].Id, "Твоя роль - Комісар \n\n" + Text.AboutCom);
                players.RemoveAt(num);
            }
        }
        
        public static List<IGame> Games = new List<IGame>();

        public static IGame GetGameOrNull(long roomId)
        {
            return Games.FirstOrDefault(x => x.room.Id == roomId);
        }

        public static IGame GetGameWithUserId(long userId)
        {
            return Games.FirstOrDefault(x => x.ConnectedUsersId.FirstOrDefault(y => y == userId) == userId);
        }
    }
}