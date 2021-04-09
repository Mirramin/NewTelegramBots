using System.Collections.Generic;
using System.Threading.Tasks;

namespace TelegramBotMafia.Interfaces
{
    public interface IChat
    {
        // Телеграм ідентифікатор цього чату
        public long Id { get; set; }
        
        // Список ідентифікаторів повідомлень, які варто видалити
        public List<int> Garbage { get; set; }
        
        // id повідомлення про початок набору гравців на гру
        public int MsgAboutStartGameId { get; set; }
        
        // Перевіряє права бота в цьому чаті
        public Task<bool> CheckAdminsRoot();
        
        // Видаляє усі повідомлення із індентифікаторами з списку garbage
        public void DeleteGarbage();
        
        public bool InGame { get; set; }
        
        
        
        // settings room
        public int startTime { get; set; }
        public int countPlayers { get; set; }
        public int voteTime { get; set; }
        public int nigthTime { get; set; }
        public int minCounterPeople { get; set; }
        public int mafiaToPeople { get; set; }
    }
}