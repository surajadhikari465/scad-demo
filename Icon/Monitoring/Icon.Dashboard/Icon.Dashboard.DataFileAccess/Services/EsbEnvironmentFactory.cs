namespace Icon.Dashboard.DataFileAccess.Services
{
    using Icon.Dashboard.DataFileAccess.Models;
    using System.ComponentModel.Composition;
    using System.Xml.Linq;
    using System;

    [Export(typeof(EsbEnvironmentFactory))]
    public class EsbEnvironmentFactory : EsbEnvironmentFactoryBase
    {
        public override IEsbEnvironment GetEsbEnvironment(XElement esbEnvironmentElement)
        {
            var esbEnvironment = new EsbEnvironment();
            base.SetEsbEnvironmentProperties(esbEnvironment, esbEnvironmentElement);
            return esbEnvironment;
        }
    }
}