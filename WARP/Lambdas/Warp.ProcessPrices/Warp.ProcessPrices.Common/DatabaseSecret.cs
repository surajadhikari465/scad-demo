namespace Warp.ProcessPrices.Common
{

    public class DatabaseSecret
    {
        public string password { get; set; }
        public string dbname { get; set; }
        public string engine { get; set; }
        public int port { get; set; }
        public string host { get; set; }
        public string username { get; set; }
    }
}