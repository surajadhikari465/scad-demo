namespace Icon.Infor.LoadTests.ConsoleTester
{
    using ApiControllerTests;
    using GloCon;
    using Esb;
    using InforItemListener;
    using LoadTests;
    using ReCon;
    using System;
    using System.Collections.Generic;
    using System.Configuration;
    using System.Diagnostics;

    public class Program
    {
        private static int printStatusCounter = 0;

        public static void Main(string[] args)
        {
            Console.WriteLine("Enter application that you want to run load test for: ");

            var testApp = Console.ReadLine();

            switch (testApp)
            {
                case "GloCon/Item":
                    RunGloConTest("Item");
                    break;
                case "GloCon/Brand":
                    RunGloConTest("Brand");
                    break;
                case "ReCon":
                    RunReConTest();
                    break;
                case "APICon/Product":
                    RunApiConTest("Product");
                    break;
                case "APICon/Hierarchy":
                    RunApiConTest("Hierarchy");
                    break;
                case "APICon/Price":
                    RunApiConTest("Price");
                    break;
                case "APICon/ItemLocale":
                    RunApiConTest("ItemLocale");
                    break;
                case "ItemListener":
                    RunItemListener();
                    break;
            }

            Console.WriteLine("Running tests");

            Console.ReadLine();
        }

        private static void RunItemListener()
        {
            InforItemListenerTestConfiguration configuration = new InforItemListenerTestConfiguration
            {
                EntityCount = 10,
                ApplicationInstances = new List<ApplicationInstance>
                {
                    new ApplicationInstance { Server = @"vm-icon-test2" }
                },
                EmailRecipients = new List<string> { "blake.jones@wholefoods.com" },
                EsbConnectionSettings = EsbConnectionSettings.CreateSettingsFromConfig(),
                TestRunTime = TimeSpan.FromMinutes(int.Parse(ConfigurationManager.AppSettings["TestRunTimeInMinutes"])),
                PopulateTestDataInterval = TimeSpan.FromMinutes(int.Parse(ConfigurationManager.AppSettings["TestPopulateDataIntervalInMinutes"]))
            };

            LoadTestManager<InforItemListenerTestConfiguration, InforItemListenerTestStatus> manager =
                new LoadTestManager<InforItemListenerTestConfiguration, InforItemListenerTestStatus>(
                    new InforItemListenerTest(configuration),
                    configuration);

            manager.StatusUpdate += (s, args) =>
            {
                PrintTestStatus(args);
            };

            manager.TestFinished += (s, args) =>
            {
                PrintTestStatus(args);
            };

            manager.RunTest();
        }

        private static void RunApiConTest(string apiControllerType)
        {
            LoadTestConfiguration configuration = new LoadTestConfiguration
            {
                EntityCount = 1000,
                ApplicationInstances = new List<ApplicationInstance>
                {
                    new ApplicationInstance { Server = @"\\vm-icon-test1" }
                },
                EmailRecipients = new List<string> { "blake.jones@wholefoods.com" },
                TestRunTime = TimeSpan.FromMinutes(int.Parse(ConfigurationManager.AppSettings["TestRunTimeInMinutes"])),
                PopulateTestDataInterval = TimeSpan.FromMinutes(int.Parse(ConfigurationManager.AppSettings["TestPopulateDataIntervalInMinutes"]))
            };
            
            LoadTestManager<LoadTestConfiguration, LoadTestStatus> manager = null;
            switch (apiControllerType)
            {
                case "Product" :
                    manager =new LoadTestManager<LoadTestConfiguration, LoadTestStatus>(
                        new ApiControllerProductTest(configuration),
                        configuration);
                    break;

                case "Price":
                    manager = new LoadTestManager<LoadTestConfiguration, LoadTestStatus>(
                        new ApiControllerPriceTest(configuration),
                        configuration);
                    break;

                case "Hierarchy":
                    manager = new LoadTestManager<LoadTestConfiguration, LoadTestStatus>(
                        new ApiControllerHierarchyTest(configuration),
                        configuration);
                    break;

                case "ItemLocale":
                    manager = new LoadTestManager<LoadTestConfiguration, LoadTestStatus>(
                        new ApiControllerItemLocaleTest(configuration),
                        configuration);
                    break;
            }

            manager.StatusUpdate += (s, args) =>
            {
                PrintTestStatus(args);
            };

            manager.TestFinished += (s, args) =>
            {
                PrintTestStatus(args);
            };

            manager.RunTest();
        }

        private static void RunGloConTest(string eventType)
        {
            LoadTestConfiguration configuration = new LoadTestConfiguration
            {
                EntityCount = 1000,
                ApplicationInstances = new List<ApplicationInstance>
                {
                    new ApplicationInstance { Server = @"\\vm-icon-test1" }
                },
                EmailRecipients = new List<string> { "blake.jones@wholefoods.com", "min.zhao@wholefoods.com" },
                TestRunTime = TimeSpan.FromMinutes(int.Parse(ConfigurationManager.AppSettings["TestRunTimeInMinutes"])),
                PopulateTestDataInterval = TimeSpan.FromMinutes(int.Parse(ConfigurationManager.AppSettings["TestPopulateDataIntervalInMinutes"]))
            };

            LoadTestManager<LoadTestConfiguration, GloConTestStatus> manager = null;

            switch (eventType)
            { 
                case "Item":
                    manager =
                        new LoadTestManager<LoadTestConfiguration, GloConTestStatus>(
                        new GloConItemTest(configuration),
                    configuration);
                    break;
                case "Brand":
                    manager =
                        new LoadTestManager<LoadTestConfiguration, GloConTestStatus>(
                        new GloConBrandTest(configuration),
                    configuration);
                    break;
            }

            manager.StatusUpdate += (s, args) =>
            {
                PrintTestStatus(args);
            };

            manager.TestFinished += (s, args) =>
            {
                PrintTestStatus(args);
            };

            manager.RunTest();
        }

        private static void RunReConTest()
        {
            ReConTestConfiguration configuration = new ReConTestConfiguration
            {
                EntityCount = 5000,
                ApplicationInstances = new List<ApplicationInstance>
                {
                    new ApplicationInstance { Server = @"vm-icon-test1" }
                },
                EmailRecipients = new List<string> { "blake.jones@wholefoods.com", "min.zhao@wholefoods.com" }
            };

            LoadTestManager<ReConTestConfiguration, ReConTestStatus> manager =
                new LoadTestManager<ReConTestConfiguration, ReConTestStatus>(
                    new ReConItemLoadTest(configuration),
                    configuration);

            manager.StatusUpdate += (s, args) =>
            {
                PrintReConTestStatus(args);
            };

            manager.TestFinished += (s, args) =>
            {
                PrintReConTestStatus(args);
            };

            manager.RunTest();
        }

        private static void PrintTestStatus(LoadTestStatusEventArgs args)
        {
            var status = args.TestStatus;
            var properties = status.GetType().GetProperties();

            printStatusCounter++;

            if (status.Status == LoadTestStatusEnum.Completed || status.Status == LoadTestStatusEnum.Failed)
            {
                Console.WriteLine();

                foreach (var property in properties)
                {
                    if (property.PropertyType.IsGenericType && property.PropertyType.GetGenericTypeDefinition() == typeof(Dictionary<,>))
                    {
                        var dictionary = property.GetValue(status);
                        Console.WriteLine();
                    }
                    else
                    {
                        Console.WriteLine("     {0}: {1}", property.Name, property.GetValue(status));
                    }
                }
            }
        }

        private static void PrintReConTestStatus(LoadTestStatusEventArgs args) 
        {
            var status = args.TestStatus;

            printStatusCounter++;

            if (status.Status == LoadTestStatusEnum.Completed || status.Status == LoadTestStatusEnum.Failed || printStatusCounter % 30 == 0)
            {
                Console.WriteLine();
                Console.WriteLine("     Status {0}:", status.Status);
                Console.WriteLine();
                Console.WriteLine("     Elapsed Time in Seconds {0}:", status.ElapsedTime);

                Console.WriteLine();
                foreach (var region in (status as ReConTestStatus).RegionalEvents)
                {
                    Console.WriteLine("     Region {0}:", region.Key);
                    Console.WriteLine("     {0}: {1}", "Processed Events: ", region.Value.ProcessedEvents);
                    Console.WriteLine("     {0}: {1}", "Unprocessed Events: ", region.Value.UnProcessedEvents);
                    Console.WriteLine("     {0}: {1}", "Failed Events: ", region.Value.FailedEvents);
                }
            }
        }
    }
}