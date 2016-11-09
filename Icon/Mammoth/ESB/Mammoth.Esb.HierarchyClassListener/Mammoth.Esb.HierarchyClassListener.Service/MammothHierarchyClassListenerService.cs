using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.ServiceProcess;
using System.Text;
using System.Threading.Tasks;

namespace Mammoth.Esb.HierarchyClassListener.Service
{
    public partial class MammothHierarchyClassListenerService : ServiceBase
    {
        private MammothHierarchyClassListener listener;

        public MammothHierarchyClassListenerService()
        {
            InitializeComponent();
        }

        protected override void OnStart(string[] args)
        {
            listener = SimpleInjectorInitializer.Initialize().GetInstance<MammothHierarchyClassListener>();
            listener.Run();
        }

        protected override void OnStop()
        {
            listener.Close();
        }
    }
}
