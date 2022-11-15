using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InventoryProducer.Producer
{
    internal class Helper
    {
        internal static string ConvertToG29String(decimal? val)
        {
            return string.Format("{0:G29}", val);
        }
    }
}
