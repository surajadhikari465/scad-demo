namespace Icon.Web.Mvc.Utility
{
    public interface IDonutCacheManager
    {
        void ClearCacheForController(string controllerName);
    }
}