namespace BulkItemUploadProcessor.Service.Interfaces
{
    public interface IServiceConfiguration
    {
        int TimerInterval { get; set; }
        string IconConnectionString { get; set; }
    }
}