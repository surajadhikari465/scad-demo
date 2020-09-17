using Icon.Logging;
using OpsgenieAlert;
using Services.Extract.Credentials;

namespace Services.Extract
{
    public class ExtractJobRunnerFactory : IExtractJobRunnerFactory
    {
        public IExtractJobRunner Create(string jobName, ILogger<ExtractJobRunner> logger, IOpsgenieAlert opsGenieAlert, ICredentialsCacheManager credentialsCacheManager, IFileDestinationCache fileDestinationCache)
        {
            return new ExtractJobRunner(jobName, logger, opsGenieAlert, credentialsCacheManager, fileDestinationCache);
        }
    }
}
