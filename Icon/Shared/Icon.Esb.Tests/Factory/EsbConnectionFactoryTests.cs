using Icon.Esb.Factory;
using Icon.Esb.Producer;
using Icon.Esb.Subscriber;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Icon.Esb.Tests.Factory
{
    [TestClass]
    public class EsbConnectionFactoryTests
    {
        [TestMethod]
        public void CreateSubscriber_ShouldReturnEsbSubscriber()
        {
            //Given
            EsbConnectionFactory factory = new EsbConnectionFactory();

            //When
            EsbSubscriber subscriber = factory.CreateSubscriber(new EsbConnectionSettings()) as EsbSubscriber;

            //Then
            Assert.IsNotNull(subscriber);
        }

        [TestMethod]
        public void CreateProducer_ShouldReturnEsbProducer()
        {

            //Given
            EsbConnectionFactory factory = new EsbConnectionFactory();

            //When
            EsbProducer producer = factory.CreateProducer(new EsbConnectionSettings()) as EsbProducer;

            //Then
            Assert.IsNotNull(producer);
        }
    }
}
