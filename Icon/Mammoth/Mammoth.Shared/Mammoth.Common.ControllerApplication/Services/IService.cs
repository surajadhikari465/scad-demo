using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mammoth.Common.ControllerApplication.Services
{
    public interface IService<T>
    {
        void Process(List<T> data);
    }
}
