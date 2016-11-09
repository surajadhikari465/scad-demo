namespace Icon.Dashboard.DataFileAccess.Services
{
    using Icon.Dashboard.DataFileAccess.Models;
    using System.ComponentModel.Composition;
    using System.Xml.Linq;

    [Export(typeof(ApplicationFactory))]
    public class ScheduledTaskFactory : ApplicationFactory
    {
        public override IApplication GetApplication(XElement applicationElement)
        {
            var scheduledTask = new ScheduledTask();

            base.SetApplicationProperties(scheduledTask, applicationElement);
            base.LoadAppSettings(scheduledTask);

            scheduledTask.FindAndCreateInstance();

            return scheduledTask;
        }
    }
}