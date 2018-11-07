using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace IRMAUserAuditConsole
{
    public enum UserAuditFunctionEnum : int
    {
        None = 0,
        Error = 1,
        Import = 2,
        Export = 3,
        Backup = 4,
        Restore = 5
    }
}
