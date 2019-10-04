﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Icon.Dashboard.CommonDatabaseAccess
{
    public enum QuerySortOrder
    {
        Unspecified = -1,
        // Rows are sorted in ascending order.
        Ascending = 0,
        // Rows are sorted in descending order.
        Descending = 1
    }
}
