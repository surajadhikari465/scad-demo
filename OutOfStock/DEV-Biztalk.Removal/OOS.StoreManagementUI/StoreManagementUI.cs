using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using OOS.Model;
using StructureMap;

namespace OOS.StoreManagementUI
{
    static class StoreManagementUi
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            StructureMapBootstrapper.Bootstrap();
           
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(ObjectFactory.GetInstance<StoreManagementTasks>());
        }
    }

}
