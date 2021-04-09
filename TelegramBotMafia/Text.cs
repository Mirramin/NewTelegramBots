namespace TelegramBotMafia
{
    public class Text
    {
        public static readonly string StartInChat = "Привіт! " +
                                                    "\nЯ бот-ведучий для гри в мафію." +
                                                    "\n\nДля початку гри дай мені такі права адміністратора:" +
                                                    "\n- видаляти повідомлення" +
                                                    "\n- блокувати користувачів" +
                                                    "\n- закріплювати повідомлення";

        public static readonly string StartGame = "Розпочалась реєстрація на гру в мафію." +
                                                  "\nНатискай приєднатись!" +
                                                  "\nПриєднались: ";

        public static readonly string RemsgAboutStartGame = "Гра розпочнеться через 30 секунд! " +
                                                            "\nВстигни зареєструватись!";

        public static readonly string AboutPeople = "Мирний житель лох. (доповнити)";
        public static readonly string AboutCom = "Комісар лох. (доповнити)";
        public static readonly string AboutDoc = "Доктор лох. (доповнити)";
        public static readonly string AboutMafia = "Мафія лох. (доповнити)";
        public static readonly string AboutDon = "Дон лох. (доповнити)";
    }
}