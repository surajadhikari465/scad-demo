using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GlobalEventController.DataAccess.Commands
{
    public class BrandDeleteCommand
    {
        [Flags]
        public enum BrandDeleteResult
        {
            NothingDeleted = 0,
            ValidatedBrandDeleted = 1,
            ItemBrandDeleted = 2,
            ValidatedAndItemBrandsDeleted = ValidatedBrandDeleted | ItemBrandDeleted,
            ItemBrandAssociatedWithItems = 4,
            ValidatedBrandDeletedButItemBrandAssociatedWithItems = ValidatedBrandDeleted | ItemBrandAssociatedWithItems,
        }

        public int? IconBrandId { get; set; }
        public string Region { get; set; }
        public BrandDeleteResult Result { get; set; }
    }
}
