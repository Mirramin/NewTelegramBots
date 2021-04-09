using System.Collections.Generic;

namespace MathTeamBot
{
    public abstract class Settings
    {
        public static readonly string TOKEN = "";
        public static readonly long OWNER = 0;
        public static long MainChat = 0;
        public static List<long> Chats = new List<long>();
        public static List<long> Admins = new List<long>();
        
        
        public static List<long> MajorChanels = new List<long>();
        
    }
}