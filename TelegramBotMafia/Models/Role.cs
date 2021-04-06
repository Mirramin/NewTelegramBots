namespace TelegramBotMafia.Models
{
    public abstract class Role
    {
        public string name { get; set; }
        public int priority { get; set; }
        
        
        
        
    }

    public class People : Role
    {
        public People()
        {
            base.name = "Мирний";
            base.priority = 3;
        }
    }
    
    
    public class Mafia : Role
    {
        public Mafia()
        {
            base.name = "Мафія";
            base.priority = 3;
        }
    }
    
    public class Doctor : Role
    {
        public Doctor()
        {
            base.name = "Лікар";
            base.priority = 3;
        }
    }
}