using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;

using WholeFoods.Common.IRMALib;
using WholeFoods.Common.IRMALib.Dates;
using System.Configuration;

namespace IRMALibTest
{
    [TestClass]
    public class IRMALibTestBase
    {
        protected const string applicationGuid_UserAudit_TST = "916C99D7-C783-4ECD-B277-15C863C5864F";
        protected const string environmentGuid_TST = "20C5DDAC-659C-4B81-84F6-5F79CC390D10";
        protected const string connectionStringName_FL = "IRMA_Test_FL";
    
        protected string ConnectionString_FLD
        {
            get
            {
                return ConfigurationManager.ConnectionStrings[connectionStringName_FL].ConnectionString;
            }
        }

        protected Guid AppId
        {
            get
            {
                return new Guid(applicationGuid_UserAudit_TST);
            }
        }
        protected Guid EnvId
        {
            get
            {
                return new Guid(environmentGuid_TST);
            }
        }
    }
}
