using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebSupport.DataAccess.Models
{
    /// <summary>
    /// Purchase Order Archived Message.
    /// </summary>
    public class PurchaseOrderArchivedMessage
    {
        /// <summary>
        /// Gets or sets the Purchase Order ID.
        /// </summary>
        public int PurchaseOrderId { get; set; }

        /// <summary>
        /// Gets or sets the Message Archive ID.
        /// </summary>
        public int ArchiveID { get; set; }

        /// <summary>
        /// Gets or sets the status
        /// </summary>
        public string Status { get; set; }
    }
}
