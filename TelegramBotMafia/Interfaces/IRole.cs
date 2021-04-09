using Telegram.Bot.Types;
using TelegramBotMafia.Models;

namespace TelegramBotMafia.Interfaces
{
    public interface IRole
    {
        public User user { get; set; }
        public string name { get; set; }
        public int priority { get; set; }

        public bool IsDead { get; set; }
    }
}