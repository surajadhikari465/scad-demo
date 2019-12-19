﻿using Icon.Common.DataAccess;
using Icon.Framework;
using System.Collections.Generic;

namespace Icon.Web.DataAccess.Queries
{
    public class GetPluRequestChangeHistoryByIdParameters : IQuery<List<PLURequestChangeHistory>>
    {
        public int PluRequestId { get; set; }
    }
}
