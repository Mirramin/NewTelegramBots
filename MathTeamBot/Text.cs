namespace MathTeamBot
{
    public abstract class Text
    {
        public static string AddInNewChanelMessage(string name, string tname)
        {
            return "<h1>Увага!</h1> " +
                   $"<h4>Мене добавили в новий канал <b>{name}(@{tname})</b></h4>" +
                   "<p>Включити розсилку повідомлень із цього чату?</p>";
        }

        public static string SendMessageIntoAllChats(string tname)
        {
            return "<h1>Увага!</h1>" +
                   $"<h4>На каналі @{tname} вийшов новий пост.</h4>" +
                   "Переслати його у всі чати?";
        }
        
        public static string AddChatForSendMessage()
        {
            return "<h1>Привіт!</h1>" +
                   $"<h4>Виберіть потрібну вам дію!</h4>";
        }
    }
}