using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Icon.Configuration.Core.ConsoleTest
{
    class Program
    {
        static void Main(string[] args)
        {
            var a = ConfigurationManager.GetSection("customAppSettings") as ConfigurationSettingsSection;
            foreach (var item in a.AppSettings)
            {
                //Console.WriteLine(item.Key + " " + item.Value);
            }
            Console.ReadLine();
        }
    }
}
