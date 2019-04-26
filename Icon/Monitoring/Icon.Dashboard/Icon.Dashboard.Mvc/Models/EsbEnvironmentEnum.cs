using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Icon.Dashboard.Mvc.Models
{
    public enum EsbEnvironmentEnum
    {
        Undefined,
        DEV,
        DEV_DUP,
        TEST,
        TEST_DUP,
        QA_Func,
        QA_DUP,
        QA_Perf,
        PRD
    }
}