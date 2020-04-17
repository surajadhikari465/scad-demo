﻿namespace Services.Extract.Credentials
{
    public class CredentialsCacheManager : ICredentialsCacheManager
    {
        private IS3CredentialsCache S3Credentials;
        private ISFtpCredentialsCache SFtpCredentials;
        private IEsbCredentialsCache EsbCredentials;

        public IS3CredentialsCache S3CredentialsCache { get => S3Credentials; set => S3Credentials = value; }
        public ISFtpCredentialsCache SFtpCredentialsCache { get => SFtpCredentials; set => SFtpCredentials = value; }
        public IEsbCredentialsCache EsbCredentialsCache { get => EsbCredentials; set => EsbCredentials = value; }


        public CredentialsCacheManager(IS3CredentialsCache s3CredentialsCache, ISFtpCredentialsCache sFtpCredentialsCache, IEsbCredentialsCache esbCredentialsCache)
        {
            S3Credentials = s3CredentialsCache;
            SFtpCredentials = sFtpCredentialsCache;
            EsbCredentialsCache = esbCredentialsCache;
        }

    }
}