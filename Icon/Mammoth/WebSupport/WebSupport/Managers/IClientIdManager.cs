namespace WebSupport.Managers
{
    public interface IClientIdManager
    {
    
        string GetClientId();
        void Initialize(string appName);
    }
}