namespace MathTeamBot
{
    public abstract class Text
    {
        public static string AddInNewChanelMessage(string name, string tname)
        {
            return "<b>Увага!</b> \n " +
                   $"Мене добавили в новий канал <b>{name}(@{tname}) </b>\n" +
                   "Включити розсилку повідомлень із цього чату?";
        }

        public static string SendMessageIntoAllChats(string tname)
        {
            return "<b>Увага! </b> \n" +
                   $"На каналі <b>@{tname}</b> вийшов новий пост. \n" +
                   "Переслати його у всі чати?";
        }
        
        public static string AddChatForSendMessage()
        {
            return "<b>Привіт!</b>" +
                   $"\nВиберіть потрібну вам дію!";
        }

        public static string CheckAdminsRoots = "<b>Для початку роботи дай мені такі права адміністратора:</b>" +
                                                "\n- видаляти повідомлення" +
                                                "\n- блокувати користувачів" +
                                                "\n- закріплювати повідомлення";

    }
}