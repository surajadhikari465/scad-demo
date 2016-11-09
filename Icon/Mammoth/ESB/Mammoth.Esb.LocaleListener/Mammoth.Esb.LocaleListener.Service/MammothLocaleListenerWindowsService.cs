using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.ServiceProcess;
using System.Text;
using System.Threading.Tasks;

namespace Mammoth.Esb.LocaleListener.Service
{
    public partial class MammothLocaleListenerWindowsService : ServiceBase
    {
        MammothLocaleListener listener;

        public MammothLocaleListenerWindowsService()
        {
            InitializeComponent();
        }

        protected override void OnStart(string[] args)
        {
            listener = SimpleInjectorInitializer.InitializeContainer().GetInstance<MammothLocaleListener>();
            listener.Run();
        }

        protected override void OnStop()
        {
            listener.Close();
        }
    }
}
