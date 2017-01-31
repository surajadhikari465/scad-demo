namespace Icon.Dashboard.DataFileAccess.Services
{
    using Icon.Dashboard.DataFileAccess.Models;
    using System;
    using System.ComponentModel.Composition;
    using System.Xml.Linq;

    [Export(typeof(ApplicationFactory))]
    public class WindowsServiceFactory : ApplicationFactory
    {
        public override IApplication GetApplication(XElement applicationElement)
        {
            try
            {
                var winService = new WindowsService();

                base.SetApplicationProperties(winService, applicationElement);
                base.LoadAppSettings(winService);

                winService.FindAndCreateInstance();

                return winService;
            }
            catch (Exception ex)
            {
                //for debugging
                string msg = ex.Message;
                throw;
            }
        }
    }
}