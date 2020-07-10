﻿using Icon.Common.DataAccess;
using Icon.Web.DataAccess.Models;
using System.Collections.Generic;

namespace Icon.Web.DataAccess.Queries
{
    public class GetItemGroupParameters : IQuery<IEnumerable<ItemGroupModel>>
    {
        /// <summary>
        /// Gets or sets Item Group Type Id to query
        /// </summary>
        public ItemGroupTypeId ItemGroupTypeId { get; set; }
    }
}
