using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CustomAppConfig
{
    class Program
    {
        static void Main(string[] args)
        {
            //var a = RegisterCompaniesConfig.GetConfig();
            //var b = a.Companies["blah"];
            //foreach (var c in a.Companies)
            //{
            //    Console.WriteLine(c.ToString());
            //}
            EsbConnectionsConfig settings = EsbConnectionsConfig.GetConfig();
            Console.WriteLine(settings.Connections["r10"].Name);
            Console.WriteLine(settings.Connections["test"].Name);
        }
    }
}
