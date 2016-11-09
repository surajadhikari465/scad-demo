using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GlobalEventController.DataAccess.Commands
{
    public class BrandDeleteCommand
    {
        public enum BrandDeleteResult
        {
            ItemBrandAndValidatedBrandDeleted = 1,
            ValidatedBrandDeleted = 2,
            NothingDeleted = 3
        }
        public int? IconBrandId { get; set; }
        public string Region { get; set; }
        public BrandDeleteResult Result { get; set; }
    }
}
