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
        
        // Перевіряє права бота в цьому чаті
        public Task<bool> CheckAdminsRoot();
        
        // Видаляє усі повідомлення із індентифікаторами з списку garbage
        public void DeleteGarbage();
    }
}