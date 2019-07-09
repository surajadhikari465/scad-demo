using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebSupport.DataAccess;
using WebSupport.EsbProducerFactory;

namespace WebSupport.Tests.EsbProducerFactory
{
    [TestClass]
    public class PriceRefreshEsbProducerFactoryTests
    {
        private PriceRefreshEsbProducerFactory factory;

        [TestInitialize]
        public void Initialize()
        {
            factory = new PriceRefreshEsbProducerFactory();
        }

        [TestMethod]
        public void CreateEsbProducer_Irma_ReturnIrmaEsbProducer()
        {
            //When
            var producer = factory.CreateEsbProducer(PriceRefreshConstants.IRMA, "FL");

            //Then
            Assert.IsInstanceOfType(producer, typeof(NonJndiEsbProducer));
            Assert.AreEqual("WFMSB1.Mammoth.Retail.JustInTimeIrma.MammothPrice.FL.Queue.V1", producer.Settings.QueueName);
        }

        [TestMethod]
        public void CreateEsbProducer_R10_ReturnR10EsbProducer()
        {
            //When
            var producer = factory.CreateEsbProducer(PriceRefreshConstants.R10, null);

            //Then
            Assert.IsInstanceOfType(producer, typeof(NonJndiEsbProducer));
        }
    }
}
