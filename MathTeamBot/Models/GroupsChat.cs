using System.Collections.Generic;
using Telegram.Bot.Types;

namespace MathTeamBot.Models
{
    public class GroupsChat
    {


        public GroupsChat()
        {
            this.Id = StaticId;
            StaticId++;
            members = new List<User>();
            OnFilter = true;
            
        }


        public int Id { get; set; }
        public List<User> members { get; set; }
        
        // Settings
        public bool OnFilter { get; set; }
        public bool ReplyMessage { get; set; }

        private static int StaticId = 0;
    }
}