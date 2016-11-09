using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infor.Services.NewItem.Processor
{
    public interface INewItemProcessor
    {
        void ProcessNewItemEvents(int controllerInstanceId);
    }
}
