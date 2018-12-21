using Icon.Dashboard.DataFileGenerator;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Icon.Dashboard.DataFileGeneratorConsole
{
    /// <summary>
    /// console app for reading service data from remote server(s) and, with the help
    ///  of a couple of database calls, builds the xml data file for Icon Dashboard
    /// </summary>
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Icon Dashboard Data File Generator_____________________________");
            var startTime = DateTime.Now;
            Console.WriteLine($"[{startTime}] beginning...");

            bool interactiveMode = false;
            string outputFilePath = $"DashboardDataFile_{startTime}.xml";

            // check for cmd line arguments, otherwise use config
            if (args.Length > 0)
            {
                Console.WriteLine($"CommandLine arg[0] (InteractiveConsoleMode) = {args[0]}");
                Console.WriteLine($"CommandLine arg[1] (XmlOutputPath) = {args[1]}");

                if (!bool.TryParse(args[0], out interactiveMode))
                {
                    Console.WriteLine("expected bool (true/false) value for first command-line argument (InteractiveConsoleMode)");
                    return;
                }
                if (args.Length > 1)
                {
                    outputFilePath = args[1];
                }
            }
            else
            {
                interactiveMode = Convert.ToBoolean(ConfigurationManager.AppSettings["InteractiveConsole"]);
                outputFilePath = ConfigurationManager.AppSettings["XmlOutputPath"];
                Console.WriteLine($"Config (InteractiveConsole) = {interactiveMode}");
                Console.WriteLine($"Config (XmlOutputPath) = {outputFilePath}");
            }
            var dataFileGenerator = new DataFileGeneratorService(outputFilePath);
            var dashboardDataFile = dataFileGenerator.GenerateDashboardXmlDataFile();

            var endTime = DateTime.Now;
            Console.WriteLine($"[{endTime}] ({(endTime - startTime):hh\\:mm\\:ss} elapsed)");

            if (interactiveMode)
            {
                Console.WriteLine("Press <enter> to close_________________________________________");
                Console.ReadLine();
            }
            else
            {
                Console.WriteLine("Icon Dashboard Data File Generator Completed.__________________");
            }
        }
    }
}
