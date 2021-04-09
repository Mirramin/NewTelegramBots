using System.Collections.Generic;
using System.Threading.Tasks;
using Telegram.Bot.Types;

namespace TelegramBotMafia.Interfaces
{
    public interface IGame
    {
        // Чат в якому буде проводитись гра
        public IChat room { get; set; }
        
        // Список гравців
        public List<User> players { get; set; }
        
        public List<IRole> roles { get; set; }
        
        // Список ідентифікаторів гравців, які хочуть приєднатись до гри
        public List<long> ConnectedUsersId { get; set; }
        
        // Чекає time-секунд та розпочинає гру
        public Task CloseConnectionsAndStartGame(int time);
        
        // Повернути список гравців як рядок
        public string PlayersToString();
        
        // Добавити нового гравця до гри
        public void AddUser(User user);
        
        
    }
}