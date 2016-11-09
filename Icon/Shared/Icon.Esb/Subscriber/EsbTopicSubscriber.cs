using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Icon.Esb.Subscriber
{
    public class EsbTopicSubscriber : EsbSubscriber
    {
        public EsbTopicSubscriber(EsbConnectionSettings settings) : base(settings) { }

        protected override void CreateDestination()
        {
            destination = session.CreateTopic(Settings.QueueName);
        }
    }
}
