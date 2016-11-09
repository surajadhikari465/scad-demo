using ApplicationDashboard.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceProcess;
using System.Threading.Tasks;

namespace ApplicationDashboard.Core
{
    public class ApplicationContext
    {
        private IEnumerable<Application> applications = new List<Application>
        {
            new Application(1, "IconAPIController-Hierarchy", "API Controller - Hierarchy", "CEWD6592"),
            new Application(2, "IconAPIController-ItemLocale", "API Controller - ItemLocale", "CEWD6592"),
            new Application(3, "IconAPIController-Locale", "API Controller - Locale", "CEWD6592"),
            new Application(4, "IconAPIController-Price", "API Controller - Price", "CEWD6592"),
            new Application(5, "IconAPIController-ProductSelectionGroup", "API Controller - Product Selection Group", "CEWD6592"),
            new Application(12, "IconAPIController-Product", "API Controller - Product", "CEWD6592"),
            new Application(6, "IconAPIController-ItemLocale", "Icon Global Event Controller Service", "CEWD6592"),
            new Application(7, "IconAPIController-ItemLocale", "Icon Regional Event Controller", "CEWD6592"),
            new Application(8, "IconAPIController-ItemLocale", "Icon POS Push Controller", "CEWD6592"),
            new Application(9, "IconAPIController-ItemLocale", "Icon R10 Listener", "CEWD6592"),
            new Application(10, "IconAPIController-ItemLocale", "Icon CCH Tax Listener", "CEWD6592"),
            new Application(11, "IconAPIController-ItemLocale", "Icon Item Movement Listener", "CEWD6592"),
        };

        public IEnumerable<Application> Applications
        {
            get
            {
                return applications;
            }
        }

        public Application GetApplication(int id, bool loadStatus = true)
        {
            var application = Applications.First(a => a.Id == id);

            if(loadStatus)
            {
                application.Status = GetStatus(application);
            }

            return application;
        }

        public string GetStatus(Application application)
        {
            return new ServiceController(application.Name, application.MachineName).Status.ToString();
        }
    }
}
