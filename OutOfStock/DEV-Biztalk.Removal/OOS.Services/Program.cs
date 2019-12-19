using System;
using System.Linq;
using OOS.Services.DAL;
using NLog;

namespace OOS.Services
{
    class Program
    {
        static void Main(string[] args)
        {

            var reportOnly = false;
            Console.WriteLine(args.Length);
            Console.WriteLine(string.Join(",", args));

            if (args.Length == 0) reportOnly = false;
            if (args.Length > 0 && args.Contains("Report")) reportOnly = true;



            var repo = new OosRepository();
            var vim = new VimRepo();

            var updater = new StoreUpdater(repo, vim);
            updater.Compare();

            if (!reportOnly)
                updater.UpdateDatabase();

            if (reportOnly)
                updater.GenerateReport();
        }
    }
}
