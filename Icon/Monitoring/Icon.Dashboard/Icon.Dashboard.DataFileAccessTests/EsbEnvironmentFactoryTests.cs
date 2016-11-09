namespace Icon.Dashboard.DataFileAccess.Tests
{
    using DataFileAccess.Constants;
    using DataFileAccess.Services;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using System.Linq;
    using System.Xml.Linq;

    [TestClass]
    public class EsbEnvironmentFactoryTests
    {
        private XDocument config;
        private EsbEnvironmentFactory testFactory;

        [TestInitialize]
        public void Initialize()
        {
            this.config = XDocument.Load("IconApplication.xml");
            this.testFactory = new EsbEnvironmentFactory();
        }

        [TestMethod]
        public void WhenValidConfig_ThenGetEsbEnvironment_ShouldReturnValidEsbEnvironment()
        {
            // Given
            var firstApplicationElement = this.config.Root.Element(EsbEnvironmentSchema.EsbEnvironments).Elements().First();

            // When
            var env = this.testFactory.GetEsbEnvironment(firstApplicationElement);

            // Then
            Assert.AreEqual("ENV_A", env.Name);
            Assert.AreEqual("ssl://cerd1617.wfm.pvt:17293", env.ServerUrl);
            Assert.AreEqual("cerd1617.wfm.pvt", env.TargetHostName);
            Assert.AreEqual("jmsUser1", env.JmsUsername);
            Assert.AreEqual("secret1234", env.JmsPassword);
            Assert.AreEqual("jndiUserX", env.JndiUsername);
            Assert.AreEqual("topSecret99", env.JndiPassword);
            Assert.IsNotNull(env.Applications);
            Assert.AreEqual(2, env.Applications.Count());
        }
    }
}