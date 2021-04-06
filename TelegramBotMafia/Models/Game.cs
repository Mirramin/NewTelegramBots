using System.Collections.Generic;
using System.Linq;
using Telegram.Bot.Types;
using TelegramBotMafia.Interfaces;

namespace TelegramBotMafia.Models
{
    public class Game : IGame
    {
        public Game(IChat chat)
        {
            room = chat;
            players = new List<User>();
        }
        
        
        public IChat room { get; set; }
        public List<User> players { get; set; }


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
                    Text.StartGame + PlayersToString());
            }
            else
            {
                await Program.bot.SendTextMessageAsync(user.Id,
                    "Розбійник, Ви уже в грі! Очікуйте початку!");
            }
        }


        public static List<IGame> Games = new List<IGame>();

        public static IGame GetGameOrNull(long roomId)
        {
            return Games.FirstOrDefault(x => x.room.Id == roomId);
        }
    }
}