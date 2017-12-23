using Icon.Esb.Producer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebSupport.EsbProducerFactory
{
    public interface IPriceRefreshEsbProducerFactory
    {
        IEsbProducer CreateEsbProducer(string system, string region);
    }
}
