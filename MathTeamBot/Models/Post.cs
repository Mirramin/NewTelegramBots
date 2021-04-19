using System.Collections.Generic;
using System.Linq;
using MathTeamBot.Interfaces;


namespace MathTeamBot.Models
{
    public class Post : IPost
    {
        public class Button
        {
            public Button(List<Button> buttons)
            {
                // Перевірка на унікальність ідентифікатора
                if (buttons.FirstOrDefault(x => x.id == Counter) != null)
                {
                    Counter++;
                }
                
                id = Counter;
                Counter++;
            }
            
            public int id;
            public string name;
            public string url;

            public static int Counter = 0;
        }
        
        public class Owner
        {
            public long id;
            public string name;
            public string tgname;
        }
        
        public Post(int step)
        {
            this.step = step;
            if (GetPost(StaticPostId) != null) // Перевірка на унікальність ідентифікатора
            {
                StaticPostId++;
            }
            id = StaticPostId;
            StaticPostId++;
            garbage = new List<int>();
            buttons = new List<Button>();
            owner = new Owner();
        }
        
        
        public int id { get; set; }
        public int msgPostId { get; set; }
        public int mainMsgId { get; set; }
        public int step { get; set; }
        public List<int> garbage { get; set; }
        public string text { get; set; }
        public string photo { get; set; }
        public List<Button> buttons { get; set; }
        public Button button { get; set; }
        public Owner owner { get; set; }
        
        // Повертає екземпляр поста за його ідентифікатором
        public static IPost GetPost(int postId)
        {
            return List.FirstOrDefault(x => x.id == postId);
        }

        public static List<IPost> List = new List<IPost>();
        public static int StaticPostId = 0;
    }
}