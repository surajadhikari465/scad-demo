using System;
using System.Collections.Generic;
using System.Configuration;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace AmazonLoad.Runner
{
    partial class Program
    {
        static List<InstanceInfo> RunningProcesses = new List<InstanceInfo>();
        protected static void stopHandler(object sender, ConsoleCancelEventArgs args)
        {
            Console.WriteLine("Exiting...");
            foreach (var process in RunningProcesses)
            {
                if (!process.Instance.HasExited) process.Instance.Kill();
            }
            Process.GetCurrentProcess().Kill();
        }
        static void Main(string[] args)
        {
            Console.CancelKeyPress += new ConsoleCancelEventHandler(stopHandler);
            if (args.Count() != 4)
            {
                Console.WriteLine("Error! Please provide -StartGroup and -EndGroup attributes");
                return;
            }
            string executablePath = ConfigurationManager.AppSettings["ExecutablePath"].ToString();
            int maxInstance = int.Parse(ConfigurationManager.AppSettings["MaxInstance"]);

            int startGroup = -1;
            int endGroup = -1;

            //The following loop can handle reordering of arguments
            for (int i = 0; i < args.Length; i++)
            {
                if (args[i].ToLower() == "-startgroup")
                {
                    if (i + 1 < args.Length && !int.TryParse(args[i + 1], out startGroup))
                    {
                        Console.WriteLine($"Value: {args[i]} is not a valid StartGroup");
                        return;
                    }
                }
                if (args[i].ToLower() == "-endgroup")
                {
                    if (i + 1 < args.Length && !int.TryParse(args[i + 1], out endGroup))
                    {
                        Console.WriteLine($"Value: {args[i]} is not a valid EndGroup");
                        return;
                    }
                }
            }
            int maxGroupsToSingleInstance = (endGroup - startGroup) / maxInstance;

            PrintMetaData(executablePath, maxInstance + 1, maxGroupsToSingleInstance);

            int runningStart = startGroup;
            int runningEnd = runningStart + maxGroupsToSingleInstance;
            var startTime = DateTime.Now;
            while (runningEnd < endGroup)
            {
                StartAmazonLoad(runningStart, runningEnd, executablePath);

                runningStart = runningEnd;
                runningEnd += maxGroupsToSingleInstance;
            }

            //For last set of group ids remaining
            StartAmazonLoad(runningStart, endGroup, executablePath);
            Console.WriteLine($"Instances running: {RunningProcesses.Count()}");

            while (!AllProcessesCompleted(RunningProcesses))
            {
                Thread.Sleep(10000);
                PrintProcessInfo(RunningProcesses, executablePath, maxInstance, maxGroupsToSingleInstance);
            }
            Console.WriteLine("=======================");
            Console.WriteLine("=======================");
            Console.WriteLine($"Processing took: {(DateTime.Now - startTime).TotalMinutes}");
            Console.WriteLine($"Groups processed: {(endGroup - startGroup)}");
            Console.WriteLine($"Records processed: {(endGroup - startGroup) * 100}");
            Console.ReadLine();
        }
        private static void PrintMetaData(string executablePath, int maxInstance, int maxGroupsToSingleInstance)
        {
            Console.WriteLine($"Executable path: {executablePath}");
            Console.WriteLine($"Max Running instances: {maxInstance}");
            Console.WriteLine($"Currently Running instances: {RunningProcesses.Where(a => a.IsRunning).Count()}");
            Console.WriteLine($"Per instances group count: {maxGroupsToSingleInstance}");
        }
        private static void StartAmazonLoad(int runningStart, int runningEnd, string executablePath)
        {
            string amazonLoadArgs = $"-RunMode process -StartGroup {runningStart} -EndGroup {runningEnd} -KillAfterCompletion true";
            Console.WriteLine($"Starting Amazon Load with {amazonLoadArgs}");
            Process instance = new Process();
            instance.StartInfo.FileName = executablePath;
            instance.StartInfo.Arguments = amazonLoadArgs;
            instance.Start();
            RunningProcesses.Add(new InstanceInfo()
            {
                Instance = instance,
                StartGroup = runningStart,
                EndGroup = runningEnd
            });
        }
        private static bool AllProcessesCompleted(List<InstanceInfo> runningProcesses)
        {
            bool allSuccessful = true;
            foreach (var process in runningProcesses)
            {
                if (!process.Instance.HasExited) return false;
                allSuccessful = allSuccessful && process.Instance.ExitCode == 0;
            }
            Console.WriteLine($"All Successful: {allSuccessful}");
            return true;
        }
        private static void PrintProcessInfo(List<InstanceInfo> runningProcesses, string executablePath, int maxInstance, int maxGroupsToSingleInstance)
        {
            Console.Clear();
            PrintMetaData(executablePath, maxInstance + 1, maxGroupsToSingleInstance);
            Console.WriteLine("=============================");
            Console.WriteLine("Error? \t Start-End \t Status \t Running Time \t Exit Code");
            foreach (var process in runningProcesses)
            {
                if (process.Instance.HasExited)
                {
                    Console.WriteLine($"{process.Instance.ExitCode != 0} \t {process.StartGroup}-{process.EndGroup} \t Completed \t {Math.Round((process.Instance.ExitTime - process.Instance.StartTime).TotalMinutes, 3)} \t\t {process.Instance.ExitCode}");
                }
                else
                {
                    Console.WriteLine($"... \t {process.StartGroup}-{process.EndGroup} \t Running \t {Math.Round((DateTime.Now - process.Instance.StartTime).TotalMinutes, 3)}");
                }
            }
        }
    }
}
