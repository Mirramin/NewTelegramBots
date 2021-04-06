using System.Collections.Generic;
using Telegram.Bot.Types;

namespace TelegramBotMafia.Interfaces
{
    public interface IGame
    {
        // Чат в якому буде проводитись гра
        public IChat room { get; set; }
        
        // Список гравців
        public List<User> players { get; set; }
        
        // Повернути список гравців як рядок
        public string PlayersToString();
        
        // Добавити нового гравця до гри
        public void AddUser(User user);
        
        
    }
}