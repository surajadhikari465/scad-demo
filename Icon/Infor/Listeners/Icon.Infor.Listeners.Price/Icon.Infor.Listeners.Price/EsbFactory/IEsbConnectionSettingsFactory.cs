using Icon.Esb;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Icon.Infor.Listeners.Price.EsbFactory
{
    public interface IEsbConnectionSettingsFactory
    {
        EsbConnectionSettings CreateConnectionSettings(Type t);
    }
}
