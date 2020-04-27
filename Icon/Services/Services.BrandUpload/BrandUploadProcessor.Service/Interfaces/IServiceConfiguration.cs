using System.Collections.Generic;

namespace BrandUploadProcessor.Service.Interfaces
{
    public interface IServiceConfiguration
    {
        int TimerInterval { get; set; }
        string IconConnectionString { get; set; }

        List<string> BrandRefreshEventConfiguredRegions { get; set; }
    }
}