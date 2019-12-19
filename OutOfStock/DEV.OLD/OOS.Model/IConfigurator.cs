using OOSCommon;

namespace OOS.Model
{
    public interface IConfigurator : IConfigure
    {
        string GetSessionID();
        string TemporaryDownloadFilePath();
    }
}
