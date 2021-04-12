using System;
using System.Data;
using System.Threading.Tasks;
using Npgsql;

namespace MathTeamBot.DataBase
{
    public abstract class DB
    {
        // Створює підключення до бази
        private static NpgsqlConnection conn = new NpgsqlConnection(DBSettings.ConnectionString);

        public static void AddChat(long tgId, bool IsMain = false)
        {
            conn.Open();

            string script = $"INSERT INTO chats (telegram_id, is_main) VALUES ('{tgId}', '{IsMain}');";
            var cmd = new NpgsqlCommand(script, conn);
            cmd.ExecuteNonQuery();
            Console.WriteLine($"[{DateTime.Now.TimeOfDay}][DB] Добавлено чат.");
            conn.Close();
        }

        public static void DeleteMainChat(long tgId)
        {
            conn.Open();

            string script = $"DELETE FROM chats WHERE telegram_id = '{tgId}';";
            var cmd = new NpgsqlCommand(script, conn);
            cmd.ExecuteNonQuery();
            Console.WriteLine($"[{DateTime.Now.TimeOfDay}][DB] Видалено чат.");
            conn.Close();
        }

        public static void AddMembers(string role, long tgId)
        {
            conn.Open();
            
            string script = $"INSERT INTO {role} VALUES ('{tgId}');";
            var cmd = new NpgsqlCommand(script, conn);
            cmd.ExecuteNonQuery();
            Console.WriteLine($"[{DateTime.Now.TimeOfDay}][DB] Добавлено {role}.");
            conn.Close();
        }

        public static void DeleteModerRoot(long tgId)
        {
            conn.Open();
            
            string script = $"DELETE FROM moders WHERE telegram_id = '{tgId}';";
            var cmd = new NpgsqlCommand(script, conn);
            cmd.ExecuteNonQuery();
            Console.WriteLine($"[{DateTime.Now.TimeOfDay}][DB] Видалено модератора.");
            conn.Close();
        }

        public static void AddChanel(long tgId)
        {
            conn.Open();
            
            string script = $"INSERT INTO chanels VALUES ('{tgId}');";
            var cmd = new NpgsqlCommand(script, conn);
            cmd.ExecuteNonQuery();
            Console.WriteLine($"[{DateTime.Now.TimeOfDay}][DB] Добавлено канал.");
            conn.Close();
        }
        
        public static void DownloadDB()
        {
            int counter = 0;
            conn.Open();
            var cmd = new NpgsqlCommand("SELECT * FROM chats;", conn);
            var reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                counter++;
                if (reader.GetBoolean(1))
                {
                    Settings.MainChat = reader.GetInt64(0);
                    continue;
                }
                
                Settings.Chats.Add(reader.GetInt64(0));
            }

            Console.WriteLine($"Завантажено {counter} чатів.");
            counter = 0;
            
            cmd.Cancel();
            reader.Close();
            cmd = new NpgsqlCommand("SELECT * FROM admins;", conn);
            reader = cmd.ExecuteReader();
            
            while (reader.Read())
            {
                counter++;
                Settings.Admins.Add(reader.GetInt64(0));
            }
            
            Console.WriteLine($"Завантажено {counter} адмінів.");
            counter = 0;
            
            cmd.Cancel();
            reader.Close();
            cmd = new NpgsqlCommand("SELECT * FROM moders;", conn);
            reader = cmd.ExecuteReader();
            
            while (reader.Read())
            {
                counter++;
                Settings.Moders.Add(reader.GetInt64(0));
            }
            
            Console.WriteLine($"Завантажено {counter} профоргів.");
            counter = 0;
            
            cmd.Cancel();
            reader.Close();
            cmd = new NpgsqlCommand("SELECT * FROM chanels;", conn);
            reader = cmd.ExecuteReader();
            
            while (reader.Read())
            {
                counter++;
                Settings.Chanels.Add(reader.GetInt64(0));
            }
            
            Console.WriteLine($"Завантажено {counter} каналів.");
            
            conn.Close();
        }
    }
}