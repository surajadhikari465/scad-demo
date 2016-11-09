using Icon.Esb.Producer;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Icon.Esb.Tests.Producer
{
    [TestClass]
    public class EsbProducerTests
    {
        [TestMethod]
        [ExpectedException(typeof(NullReferenceException))]
        public void Send_OpenConnectionIsNotCalled_ThrowsException()
        {
            //Given
            EsbProducer producer = new EsbProducer(new EsbConnectionSettings());

            //When
            producer.Send("Test message");
        }
    }
}
