using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace OOSCommon
{
    public interface IConfigure
    {
        string GetVIMServiceName();
        string GetMovementServiceName();

        string GetLoggerBasePath();
        string GetLoggerName();

        string GetOOSConnectionString();
        string GetEFConnectionString();

        bool GetValidationMode();
        string GetOffshelfUploadBoundedContextEndpoint();
        string GetKnownUploadBoundedContextEndpoint();
    }
}
