using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Icon.Esb.Subscriber
{
    public class EsbMessageEventArgs : EventArgs
    {
        public IEsbMessage Message { get; set; }
    }
}
