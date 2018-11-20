using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.ServiceProcess;
using System.Text;
using System.Threading.Tasks;

namespace KitBuilder.Esb.LocaleListener.Service
{
    public partial class KitBuilderLocaleListenerWindowsService : ServiceBase
    {
        KitBuilderLocaleListener listener;

        public KitBuilderLocaleListenerWindowsService()
        {
            InitializeComponent();
        }

        protected override void OnStart(string[] args)
        {
            listener = SimpleInjectorInitializer.InitializeContainer().GetInstance<KitBuilderLocaleListener>();
            listener.Run();
        }

        protected override void OnStop()
        {
            listener.Close();
        }
    }
}
