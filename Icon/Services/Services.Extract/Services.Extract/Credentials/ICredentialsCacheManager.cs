namespace Services.Extract.Credentials
{
    public interface ICredentialsCacheManager
    {
        IS3CredentialsCache S3CredentialsCache { get; set; }
        ISFtpCredentialsCache SFtpCredentialsCache { get; set; }
        IEsbCredentialsCache EsbCredentialsCache { get; set; }
        IActiveMqCredentialsCache ActiveMqCredentialsCache { get; set; }
    }
}