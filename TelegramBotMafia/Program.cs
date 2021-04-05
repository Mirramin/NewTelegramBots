using System;
using System.Threading;
using Telegram.Bot;

namespace TelegramBotMafia
{
    class Program
    {
        public static TelegramBotClient bot;
        
        
        static void Main(string[] args)
        {
            bot = new TelegramBotClient(Settings.TOKEN);

            Console.WriteLine($"[{DateTime.Now.TimeOfDay}] Bot is runnig!");
            
            bot.StartReceiving();
            bot.OnMessage += Handler.OnMessage;
            bot.OnCallbackQuery += Handler.OnCallbackQuery;
            Thread.Sleep(Int32.MaxValue);
        }
    }
}