using System;

namespace OOSCommon
{
    public class BasicConfigurator : IConfigure
    {
        private const string VIM_SERVICE_NAME = "VIM_SERVICE_NAME";
        private const string MOVEMENT_SERVICE_NAME = "MOVEMENT_SERVICE_NAME";
        private const string LOGGER_NAME = "NLogEventLog";
        private const string LOG_BASE_PATH = "NLogBasePath";
        private const string OOS_CONNECTION_STRING = "OOSConnectionString";
        private const string EF_CONNECTION_STRING = "OOSEntities";
        private const string VALIDATION_MODE = "ValidationMode";
        private const string OFFSHELF_UPLOAD_BC_ENDPOINT = "OffshelfUploadBoundedContextEndpoint";
        private const string KNOWN_UPLOAD_BC_ENDPOINT = "KnownUploadBoundedContextEndpoint";

        public string GetVIMServiceName()
        {
            return AppConfig.AppSettings[VIM_SERVICE_NAME];
        }

        public string GetMovementServiceName()
        {
            return AppConfig.AppSettings[MOVEMENT_SERVICE_NAME];
        }

        public string GetLoggerBasePath()
        {
            return AppConfig.AppSettings[LOG_BASE_PATH];
        }

        public string GetLoggerName()
        {
            return LOGGER_NAME;
        }

        public string GetOOSConnectionString()
        {
            return AppConfig.ConnectionStrings[OOS_CONNECTION_STRING].ConnectionString;
        }

        public string GetVimConnectionString()
        {
            return AppConfig.ConnectionStrings["VIM"].ConnectionString;
        }

        public string GetEFConnectionString()
        {
            return AppConfig.ConnectionStrings[EF_CONNECTION_STRING].ConnectionString;
        }

        public bool GetValidationMode()
        {
            return AppConfig.AppSettings[VALIDATION_MODE].Equals("true", StringComparison.OrdinalIgnoreCase); ;
        }

        public string GetOffshelfUploadBoundedContextEndpoint()
        {
            return AppConfig.AppSettings[OFFSHELF_UPLOAD_BC_ENDPOINT];
        }

        public string GetKnownUploadBoundedContextEndpoint()
        {
            return AppConfig.AppSettings[KNOWN_UPLOAD_BC_ENDPOINT];
        }
    }
}
