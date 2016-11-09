using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mammoth.Common
{
    public interface IRegionalControllerApplicationSettings
    {
        List<string> Regions { get; set; }
        string CurrentRegion { get; set; }
    }
}
