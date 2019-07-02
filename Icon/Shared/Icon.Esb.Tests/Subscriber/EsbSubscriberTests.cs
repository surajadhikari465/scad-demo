using Icon.Esb.Subscriber;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;

namespace Icon.Esb.Tests.Subscriber
{
    [TestClass]
    public class EsbSubscriberTests
    {
        [TestMethod]
        [ExpectedException(typeof(NullReferenceException))]
        public void BeginListening_OpenConnectionHasNotBeenCalled_ShouldThrowAnException()
        {
            //Given
            EsbSubscriber subscriber = new EsbSubscriber(new EsbConnectionSettings());

            //When
            subscriber.BeginListening();
        }
    }
}
