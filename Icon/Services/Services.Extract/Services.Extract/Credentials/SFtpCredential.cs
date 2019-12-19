namespace Services.Extract.Credentials
{
    public class SFtpCredential
    {
        public SFtpCredential(string host,string username, string password)
        {
            Host = host;
            Username = username;
            Password = password;
        }

        public string Host { get; set;}
        public string Username { get; set; }
        public string Password { get; set; }
    }
}