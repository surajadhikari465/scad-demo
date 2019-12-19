namespace OutOfStock
{
    public interface IApplicationConfig
    {
        string GetValue(string key);
        void SetValue(string key, string value);
    }
}