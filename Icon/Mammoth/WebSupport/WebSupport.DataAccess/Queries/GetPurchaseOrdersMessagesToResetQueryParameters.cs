using Icon.Common.DataAccess;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebSupport.DataAccess.Models;

namespace WebSupport.DataAccess.Queries
{
    /// <summary>
    /// GetPurchaseOrdersMessagesToReset Query Parameters
    /// </summary>
    public class GetPurchaseOrdersMessagesToResetQueryParameters : IQuery<IList<PurchaseOrderArchivedMessage>>
    {
        /// <summary>
        /// Gets or sets the region.
        /// </summary>
        public string Region { get; set; }

        /// <summary>
        /// Gets or sets the Max Records.
        /// </summary>
        public int MaxRecords { get; set; }

        /// <summary>
        /// Gets or sets the Purchase Order Id List.
        /// </summary>
        public int[] PurchaseOrderIdList { get; set; }
    }
}
