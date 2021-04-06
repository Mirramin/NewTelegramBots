using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TelegramBotMafia.Interfaces;

namespace TelegramBotMafia.Models
{
    public class Chat : IChat
    {
        public Chat(long id)
        {
            this.Id = id;
        }
        
        public long Id { get; set; }
        public List<int> Garbage { get; set; }
        public int MsgAboutStartGameId { get; set; }
        
        // setting room
        public int startTime { get; set; }
        public int countPlayers { get; set; }
        public int voteTime { get; set; }
        public int nigthTime { get; set; }
        public int minCounterPeople { get; set; }
        
        
        public async Task<bool> CheckAdminsRoot()
        {
            var admins = await Program.bot.GetChatAdministratorsAsync(this.Id);
            var bot = admins.FirstOrDefault(x => x.User.Id == Program.bot.BotId);

            if (bot == null) return false;

            return bot.CanDeleteMessages == true &&
                   bot.CanPinMessages == true &&
                   bot.CanRestrictMembers == true
                ? true
                : false;
        }

        public async void DeleteGarbage()
        {
            foreach (var msgId in Garbage)
            {
                try
                {
                    await Program.bot.DeleteMessageAsync(this.Id, msgId);
                }
                catch (Exception e)
                {
                    Console.WriteLine($"[{DateTime.Now.TimeOfDay}] {e}");
                }
            }
        }
        
        // Зберігає усі чати
        public static List<IChat> Chats = new List<IChat>();
        
        // Зберігає усі активні чати, де може проводитись гра
        public static List<IChat> ActivChats = new List<IChat>();
        
        // Повертає чат або null у випадку, коли чату з таким id не існує
        public static IChat GetChatOrNull(long id)
        {
            return Chats.FirstOrDefault(x => x.Id == id);
        }
        
    }
}