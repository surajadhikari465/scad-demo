using System;

namespace Icon.Dashboard.RemoteServerAccess
{
    public class RemoteServerAccessService
    {
        var serviceName = "";
        var machineName = "";

        var serviceController = new ServiceController(serviceName, machineName);
    }
}


//namespace Icon.Dashboard.DataFileAccess.Services
//{
//    using Icon.Dashboard.DataFileAccess.Models;
//    using System;
//    using System.ComponentModel.Composition;
//    using System.Xml.Linq;

//    [Export(typeof(ApplicationFactory))]
//    public class WindowsServiceFactory : ApplicationFactory
//    {
//        public override IIconApplication GetApplication(XElement applicationElement)
//        {
//            var winService = new IconService();
//            try
//            {
//                base.SetApplicationProperties(winService, applicationElement);
//                base.LoadAppSettings(winService);

//                winService.FindAndCreateInstance();
//            }
//            catch (System.IO.FileNotFoundException fileNotFoundEx)
//            {
//                // unable to read the config file, so just eat this exception
//                //for debugging
//                var msg = fileNotFoundEx.Message;
//            }
//            return winService;
//        }
//    }
//}