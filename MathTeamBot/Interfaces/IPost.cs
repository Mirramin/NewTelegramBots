using System.Collections.Generic;
using MathTeamBot.Models;


namespace MathTeamBot.Interfaces
{
    public interface IPost
    {
        // Ідентифікатор цього поста
        public int id { get; set; }
        // Телеграм-ідентифікатор повідомлення із постом
        public int msgPostId { get; set; }
        // Телеграм-ідентифікатор повідомлення із споском дій
        public int mainMsgId { get; set; }
        // Крок створення (Для хендлера)
        public int step { get; set; }
        // Телеграм-ідентифікатори повідомлень, які потрібно видалити
        public List<int> garbage { get; set; }
        // Текст цього поста
        public string text { get; set; } 
        // Фото цього поста
        public string photo { get; set; }
        // Список клавіш із силками
        public List<Post.Button> buttons { get; set; }
        // Екземпляр клавіші
        public Post.Button button { get; set; }
        // Інформація про автора цього поста
        public Post.Owner owner { get; set; }
        
    }
}