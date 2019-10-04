using System;

namespace Icon.Dashboard.Mvc.Enums
{
    /// <summary>
    /// Enumeration of authorized privilege levels within the Dashboard application,
    /// </summary>
    [Flags]
    public enum UserAuthorizationLevelEnum
    {
        None = 0x0,
        ReadOnly = 0x1,
        EditingPrivileges = ReadOnly | 0x2
    }
}