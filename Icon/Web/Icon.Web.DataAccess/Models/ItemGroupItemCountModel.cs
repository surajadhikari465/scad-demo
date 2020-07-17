using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Icon.Web.DataAccess.Models
{
    /// <summary>
    /// Sku/priceline Item-Count Model.
    /// </summary>
    public class ItemGroupItemCountModel
    {
        /// <summary>
        /// Gets or sets the SkuId
        /// </summary>
        public int ItemGroupId { get; set; }

        /// <summary>
        /// Gets or sets the Count Of Items
        /// </summary>
        public int CountOfItems { get; set; }
    }
}
