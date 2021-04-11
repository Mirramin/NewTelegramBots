using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
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
            
            Console.WriteLine($"[log] Bot is running..");
            
            Settings.Admins.Add(Settings.OWNER);
            bot.StartReceiving();
            bot.OnUpdate += Handler.OnUpdate;
            bot.OnMessage += Handler.OnMessage;
            bot.OnCallbackQuery += Handler.OnCallBack;
            Thread.Sleep(Int32.MaxValue);
        }
        
        public static void Stop()
        {
            Console.WriteLine($"[log] Bot is stoping..");
            bot.StopReceiving();
        }
        
        public static async Task<bool> CheckAdminsRoots(long chatId)
        {
            var admins = await Program.bot.GetChatAdministratorsAsync(chatId);
            var bot = admins.FirstOrDefault(x => x.User.Id == Program.bot.BotId);

            if (bot == null) return false;

            return bot.CanDeleteMessages == true &&
                   bot.CanPinMessages == true &&
                   bot.CanRestrictMembers == true
                ? true
                : false;
        }
    }
}