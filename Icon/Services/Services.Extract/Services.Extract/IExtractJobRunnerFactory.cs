using Icon.Logging;
using OpsgenieAlert;
using Services.Extract.Credentials;

namespace Services.Extract
{
    public interface IExtractJobRunnerFactory
    {
        IExtractJobRunner Create(string jobName, ILogger<ExtractJobRunner> logger, IOpsgenieAlert opsGenieAlert, ICredentialsCacheManager credentialsCacheManager, IFileDestinationCache fileDestinationCache);
    }
}