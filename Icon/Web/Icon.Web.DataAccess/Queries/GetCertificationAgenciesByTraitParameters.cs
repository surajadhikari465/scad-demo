﻿using Icon.Common.DataAccess;
using Icon.Framework;
using System.Collections.Generic;

namespace Icon.Web.DataAccess.Queries
{
    public class GetCertificationAgenciesByTraitParameters : IQuery<List<HierarchyClass>>
    {
        public string AgencyTypeTraitCode { get; set; }
    }
}
