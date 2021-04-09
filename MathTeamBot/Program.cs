using System;
using System.Threading;
using Telegram.Bot;
using Telegram.Bot.Types.Enums;

namespace MathTeamBot
{
    class Program
    {
        public static TelegramBotClient bot;
        
        static void Main(string[] args)
        {
            bot = new TelegramBotClient(Settings.TOKEN);
        }

        public static void Start()
        {
            Console.WriteLine($"[log] Bot is running..");
            
            bot.StartReceiving();
            bot.OnMessage += Handler.OnMessage;
            bot.OnCallbackQuery += Handler.OnCallBack;
            Thread.Sleep(Int32.MaxValue);
        }

        public static void Stop()
        {
            Console.WriteLine($"[log] Bot is stoping..");
            bot.StopReceiving();
        }
    }
}