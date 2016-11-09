using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GlobalEventController.DataAccess.Commands
{
    public class AddOrUpdateBrandCommand
    {
        public int? IconBrandId { get; set; }
        public string BrandName { get; set; }
        public string Region { get; set; }

        // output parameter
        public int BrandId { get; set; }
    }
}
