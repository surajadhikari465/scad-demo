using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.ServiceProcess;
using System.Text;
using OOSCommon;

namespace OOSCollectorService
{
    public partial class CollectorService : ServiceBase
    {
        public OOSCommon.OOSCollector.OOSCollectorWorkflow oosCollectorWorkflow { get; set; }

        public CollectorService()
        {
            InitializeComponent();
            oosCollectorWorkflow = new OOSCommon.OOSCollector.OOSCollectorWorkflow();
            oosCollectorWorkflow.InitializeComponent();
        }

        protected override void OnStart(string[] args)
        {
            oosCollectorWorkflow.OnStart(args);
        }

        protected override void OnStop()
        {
            oosCollectorWorkflow.OnStop();
        }

    }
}
