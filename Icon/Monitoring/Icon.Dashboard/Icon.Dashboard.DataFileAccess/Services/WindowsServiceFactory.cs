namespace Icon.Dashboard.DataFileAccess.Services
{
    using Icon.Dashboard.DataFileAccess.Models;
    using System.ComponentModel.Composition;
    using System.Xml.Linq;

    [Export(typeof(ApplicationFactory))]
    public class WindowsServiceFactory : ApplicationFactory
    {
        public override IApplication GetApplication(XElement applicationElement)
        {
            var winService = new WindowsService();

            base.SetApplicationProperties(winService, applicationElement);
            base.LoadAppSettings(winService);

            winService.FindAndCreateInstance();

            return winService;
        }
    }
}