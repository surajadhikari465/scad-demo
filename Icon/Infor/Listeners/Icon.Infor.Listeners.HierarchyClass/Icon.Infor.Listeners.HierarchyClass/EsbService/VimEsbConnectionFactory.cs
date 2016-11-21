using Icon.Esb.Factory;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Icon.Esb;
using Icon.Esb.Producer;
using Icon.Esb.Subscriber;

namespace Icon.Infor.Listeners.HierarchyClass.EsbService
{
    public class VimEsbConnectionFactory : IEsbConnectionFactory
    {
        public EsbConnectionSettings Settings { get; set; }

        public VimEsbConnectionFactory(VimEsbConnectionSettings settings)
        {
            Settings = settings;
        }

        public IEsbProducer CreateProducer(EsbConnectionSettings settings)
        {
            return new VimEsbProducer { Settings = settings };
        }

        public IEsbProducer CreateProducer(bool openConnection = true)
        {
            var producer = new VimEsbProducer { Settings = Settings };
            producer.OpenConnection();
            return producer;
        }

        public IEsbSubscriber CreateSubscriber()
        {
            throw new NotImplementedException(typeof(VimEsbConnectionFactory).Name + " does not create a subscriber. It is only used to create producers.");
        }

        public IEsbSubscriber CreateSubscriber(EsbConnectionSettings settings)
        {
            throw new NotImplementedException(typeof(VimEsbConnectionFactory).Name + " does not create a subscriber. It is only used to create producers.");
        }
    }
}
