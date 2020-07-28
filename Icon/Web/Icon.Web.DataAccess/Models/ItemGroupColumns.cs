using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Icon.Web.DataAccess.Models
{
    /// <summary>
    /// List of Columns available to Item Group queries.
    /// </summary>
    public enum ItemGroupColumns
    {
        /// <summary>
        /// ItemGroupId (SkuID, priceLine ID).
        /// </summary>
        ItemGroupId = 1,

        /// <summary>
        /// ItemGroupTypeId.
        /// </summary>
        ItemGroupTypeId = 2,

        /// <summary>
        /// SKUDescription.
        /// </summary>
        SKUDescription = 3,

        /// <summary>
        /// PriceLineDescription.
        /// </summary>
        PriceLineDescription = 4,

        /// <summary>
        /// PriceLineSize.
        /// </summary>
        PriceLineSize = 5,

        /// <summary>
        /// PriceLineUOM.
        /// </summary>
        PriceLineUOM = 6,

        /// <summary>
        /// ScanCode.
        /// </summary>
        ScanCode = 7,

        /// <summary>
        /// ItemCount.
        /// </summary>
        ItemCount = 8,
    }
}
