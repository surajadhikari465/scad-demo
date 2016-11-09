using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using InterfaceController.Common;

namespace GlobalEventController.DataAccess.Commands
{
    public class UpdateTaxClassCommand
    {
        public string TaxClassDescription { get; set; }
        public string TaxCode { get; set; }

        // 'output' parameter
        public int TaxClassId { get; set; }
    }
}
