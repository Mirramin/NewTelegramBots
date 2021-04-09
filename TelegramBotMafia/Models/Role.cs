using Telegram.Bot.Types;
using TelegramBotMafia.Interfaces;

namespace TelegramBotMafia.Models
{
    public abstract class Role : IRole
    {
        public User user { get; set; }
        public string name { get; set; }
        public int priority { get; set; }

        public bool IsDead { get; set; }
    }

    public class People : Role
    {
        public People(User user)
        {
            base.user = user;
            base.name = "Мирний";
            base.priority = 3;
            IsDead = false;
        }
    }
    
    
    public class Mafia : Role
    {
        public Mafia(User user)
        {
            base.user = user;
            base.name = "Мафія";
            base.priority = 3;
            IsDead = false;
        }
    }

    public class Don : Mafia
    {
        public Don(User user) : base(user)
        {
            base.name = "Дон";
            base.priority = 3;
        }
    }
    
    public class Doctor : Role
    {
        public Doctor(User user)
        {
            base.user = user;
            base.name = "Лікар";
            base.priority = 3;
            IsDead = false;
        }
    }
    
    public class Com : Role
    {
        public Com(User user)
        {
            base.user = user;
            base.name = "Лікар";
            base.priority = 3;
            IsDead = false;
        }
    }
}