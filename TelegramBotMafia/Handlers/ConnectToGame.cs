using System;
using System.Threading;
using Telegram.Bot.Args;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;
using TelegramBotMafia.Interfaces;

namespace TelegramBotMafia.Handlers
{
    public class ConnectToGame
    {
        public static async void Connect(User user, IGame game, string CBid)
        {
            int step = 1;
            var pr = new ManualResetEvent(false);

            EventHandler<MessageEventArgs> conn = (object sender, MessageEventArgs e) =>
            {
                if (e.Message.From.Id != user.Id) return;
                if(e.Message.Chat.Type != ChatType.Private) return;

                if (step == 1)
                {
                    if (e.Message.Text == "/start")
                    {
                        game.AddUser(user);
                    }
                }
                
            };
            Program.bot.OnMessage += conn;
            pr.WaitOne();
            Program.bot.OnMessage -= conn;
        }
    }
}