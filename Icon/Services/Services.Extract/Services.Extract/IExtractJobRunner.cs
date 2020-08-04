using Services.Extract.Models;

namespace Services.Extract
{
    public interface IExtractJobRunner
    {
        void Run(ExtractJobConfiguration configuration);
    }
}