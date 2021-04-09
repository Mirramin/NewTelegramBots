namespace MathTeamBot
{
    public abstract class Text
    {
        public static string AddInNewChanelMessage(string name)
        {
            return "<h1>Увага!</h1> " +
                   $"<h4>Мене добавили в новий канал <b>{name}</b></h4>" +
                   "<p>Включити розсилку повідомлень із цього чату?</p>";
        }
        
        
    }
}