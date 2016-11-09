using InterfaceController.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GlobalEventController.DataAccess.Commands
{
    public class AddTaxClassCommand
    {
        public string TaxClassDescription { get; set; }
        public string TaxCode { get; set; }

        // 'output' parameter
        public int TaxClassId { get; set; }
    }
}
