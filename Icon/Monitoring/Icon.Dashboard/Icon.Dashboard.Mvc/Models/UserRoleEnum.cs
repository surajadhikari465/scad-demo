﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Icon.Dashboard.Mvc.Models
{
    /// <summary>
    /// Enumeration of authorized roles within the Dashboard application,
    ///   corresponding with AD security groups
    /// </summary>
    [Flags]
    public enum UserRoleEnum
    {
        Unauthorized = 0x0,
        ReadOnly = 0x1,
        EditingPrivileges = ReadOnly | 0x2
    }
}