namespace OutOfStock
{
    public interface IUserLoginManager
    {
        void RecordLogin(string username, string region, string store);
        void GetLocationOverrides(string username, out string region, out string store);
    }
}