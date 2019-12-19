using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Rhino.Mocks;

namespace OOS.Model.UnitTests
{
    public class MockConfigurator
    {
        private const string OOS_CONNECTION_STRING = "Data Source=OOSDbTest;Initial Catalog=OOS;Integrated Security=True";
        private const string OOS_EF_CONNECTION_STRING = "metadata=res://*/DataContext.OOS.csdl|res://*/DataContext.OOS.ssdl|res://*/DataContext.OOS.msl;provider=System.Data.SqlClient;provider connection string=\"data source=OOSDbTest;initial catalog=OOS;integrated security=True;multipleactiveresultsets=True;App=EntityFramework\"";

        private MockConfigurator() {}

        public static IConfigurator New()
        {
            return GetConfigurator();
        }

        private static IConfigurator GetConfigurator()
        {
            var config = MockRepository.GenerateStub<IConfigurator>();
            config.Expect(p => p.GetEFConnectionString()).Return(OOS_EF_CONNECTION_STRING);
            config.Expect(p => p.GetSessionID()).Return("(no session)");
            config.Expect(p => p.GetLoggerBasePath()).Return("~/.");
            config.Expect(p => p.GetLoggerName()).Return("NLogEventLog");
            config.Expect(p => p.GetMovementServiceName()).Return("STELLA_Dev");
            config.Expect(p => p.GetOOSConnectionString()).Return(OOS_CONNECTION_STRING);
            config.Expect(p => p.GetValidationMode()).Return(true);
            config.Expect(p => p.GetVIMServiceName()).Return("VIM_DQ");
            config.Expect(p => p.TemporaryDownloadFilePath()).Return("./OutOfStock");
            return config;
        }
    }
}
