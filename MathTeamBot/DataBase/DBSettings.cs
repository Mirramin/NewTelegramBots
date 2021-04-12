namespace MathTeamBot.DataBase
{
    public class DBSettings
    {
        public const string Host = "ec2-34-233-0-64.compute-1.amazonaws.com";
        public const int Port = 5432;
        public const string Username = "xqwilmmpvkzdnr";
        public const string Password = "e35c9246c54141fc175f2f5c2a01485588e9ec3c47a8818a3a5a62072b6a9ecf";
        public const string Datebase = "d7b9r3uv70c1qo";
        public const bool Pooling = true;
        public const string SSLMode = "Require";
        public const bool TrueServerCertificate = true;

        public static readonly string ConnectionString = $"host={Host};" +
                                                         $"username={Username};" +
                                                         $"password={Password};" +
                                                         $"database={Datebase};" +
                                                         $"port={Port};" +
                                                         $"Sslmode=Require;" +
                                                         $"Trust Server Certificate=true";
    }
}