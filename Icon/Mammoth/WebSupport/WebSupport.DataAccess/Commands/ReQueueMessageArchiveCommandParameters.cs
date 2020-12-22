using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebSupport.DataAccess.Commands
{
    /// <summary>
    /// Re-Queue MessageArchive Command Parameters.
    /// </summary>
    public class ReQueueMessageArchiveCommandParameters
    {
        /// <summary>
        /// Gets or sets the region.
        /// </summary>
        public string Region { get; set; }

        /// <summary>
        /// Gets or sets the list of MessageArchive IDs.
        /// </summary>
        public int[] MessageArchiveIDs { get; set; }

        /// <summary>
        /// Gets or sets the username.
        /// </summary>
		public string UserName { get; set; }

        /// <summary>
        /// Gets or sets the Rows Affected.
        /// </summary>
        public int RowsAffected { get; set; }
    }
}
