using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using WFM.ScanProductClient.BtsServiceRef;

namespace WFM.ScanProductClient
{
    class Program
    {
        static void Main(string[] args)
        {
            //ScanProductDeployedBuildMachine();
            ScanProductUpcsFromFile();
        }   

        static private void ScanProductDeployedBuildMachine()
        {
            var client = new BTS_OOS_Orch_ScanProduct_ScanProductAsyncClient();
            var sp = new ScanProductsByStoreAbbreviationRequest();
            sp.ScanProductsByStoreAbbreviation = new ScanProductsByStoreAbbreviation
                                                    {
                                                        scanDate = DateTime.Now,
                                                        scanDateSpecified = true,
                                                        storeAbbrev = "BCA",
                                                        regionAbbrev = "FL",
                                                        upcs = new[] { "0005342300066" }
                                                    };

            client.ClientCredentials.Windows.AllowedImpersonationLevel = System.Security.Principal.TokenImpersonationLevel.Anonymous;
            client.ScanProductsByStoreAbbreviation(sp.ScanProductsByStoreAbbreviation);
        }

        static private void ScanProductUpcsFromFile()
        {
            var upload = OOS.Model.UploadMessage.From(Properties.Resources.upload);
            var upcs = upload.Scans;
            var scanDate = upload.ScanDate;
            string storeAbbrev = upload.StoreAbbreviation;
            string regionAbbrev = upload.RegionAbbreviation;

            var client = new BTS_OOS_Orch_ScanProduct_ScanProductAsyncClient();
            var sp = new ScanProductsByStoreAbbreviationRequest();
            sp.ScanProductsByStoreAbbreviation = new ScanProductsByStoreAbbreviation
            {
                scanDate = scanDate,
                scanDateSpecified = true,
                storeAbbrev = storeAbbrev,
                regionAbbrev = regionAbbrev,
                upcs = upcs.ToArray()
            };

            client.ClientCredentials.Windows.AllowedImpersonationLevel = System.Security.Principal.TokenImpersonationLevel.Anonymous;
            client.ScanProductsByStoreAbbreviation(sp.ScanProductsByStoreAbbreviation);


        }


    }
}
