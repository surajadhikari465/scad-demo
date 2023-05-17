namespace Services.Extract.Credentials
{
    public class CredentialsCacheManager : ICredentialsCacheManager
    {
        private IS3CredentialsCache S3Credentials;
        private ISFtpCredentialsCache SFtpCredentials;
        private IActiveMqCredentialsCache ActiveMqCredentials;

        public IS3CredentialsCache S3CredentialsCache { get => S3Credentials; set => S3Credentials = value; }
        public ISFtpCredentialsCache SFtpCredentialsCache { get => SFtpCredentials; set => SFtpCredentials = value; }
        public IActiveMqCredentialsCache ActiveMqCredentialsCache { get => ActiveMqCredentials; set => ActiveMqCredentials = value; }


        public CredentialsCacheManager(IS3CredentialsCache s3CredentialsCache, ISFtpCredentialsCache sFtpCredentialsCache, IActiveMqCredentialsCache activeMqCredentialsCache)
        {
            S3Credentials = s3CredentialsCache;
            SFtpCredentials = sFtpCredentialsCache;
            ActiveMqCredentialsCache = activeMqCredentialsCache;
        }

    }
}