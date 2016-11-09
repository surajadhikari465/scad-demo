using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GlobalEventController.DataAccess.DataServices
{
    public interface IDataService<TService>
    {
        void Process(TService service);
    }
}
