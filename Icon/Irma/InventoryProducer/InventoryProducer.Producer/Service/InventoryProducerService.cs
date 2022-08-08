using InventoryProducer.Common;
using InventoryProducer.Producer.ProducerBuilders;
using Icon.Logging;
using System;
using System.Configuration;
using System.Diagnostics;
using System.Threading;
using System.Timers;
using Irma.Framework;

namespace InventoryProducer.Producer.Service
{
    class InventoryProducerService : IInventoryProducerService
    {
        private static InventoryLogger<Program> inventoryLogger = new InventoryLogger<Program>(new NLogLogger<Program>());
        private readonly System.Timers.Timer timer = null;
        private bool isServiceRunning = false;
        private readonly string producerType;
        private readonly int dayOfTheWeek = 0;
        private TimeSpan startTime;
        private TimeSpan endTime;
        private readonly int producerInstanceID;
        private readonly InventoryProducerBase inventoryProducerBase;

        private static string sSource;
        private static string sLog;
        private static string sEvent;


        public InventoryProducerService()
        {
            inventoryLogger.LogInfo("Initializing InventoryProducerService");
            try
            {
                int.TryParse(ConfigurationManager.AppSettings["ProducerInstanceId"], out producerInstanceID);
                producerType = ConfigurationManager.AppSettings["ProducerType"].ToString();
                int.TryParse(ConfigurationManager.AppSettings["MaintenanceDay"], out dayOfTheWeek);

                DateTime timeStamp;
                if (!DateTime.TryParse(ConfigurationManager.AppSettings["MaintenanceStartTime"], out timeStamp))
                    throw new ArgumentException("Invalid or missing MaintenanceStartTime configuration setting");

                startTime = new TimeSpan(timeStamp.Hour, timeStamp.Minute, 0);

                if (!DateTime.TryParse(ConfigurationManager.AppSettings["MaintenanceEndTime"], out timeStamp))
                    throw new ArgumentException("Invalid or missing MaintenanceEndTime configuration setting");

                endTime = new TimeSpan(timeStamp.Hour, timeStamp.Minute, 0);

                //Validate config settings
                if (producerInstanceID < 1)
                    throw new Exception("Please provide an integer greater than zero to be used as the unique instance ID.");
                if (producerInstanceID < 1 || string.IsNullOrEmpty(producerType))
                    throw new Exception("Both the producer type argument and the instance ID argument are required.");
                if (!StartupOptions.ValidArgs.Contains(producerType))
                    throw new Exception($"Invalid argument specified.  The valid arguments are: {string.Join(",", StartupOptions.ValidArgs)}");

                InventoryProducerSettings settings = InventoryProducerSettings.CreateFromConfig("IRMA", ProducerType.Instance);
                inventoryLogger = new InventoryLogger<Program>(
                    new NLogLoggerInstance<Program>(ProducerType.Instance.ToString()),
                    new IrmaDbContextFactory(),
                    settings
                    );
                inventoryProducerBase = ProducerProvider.GetProducer(settings);


                //Initilize timer if all settings have been validated
                int runInterval;
                int.TryParse(ConfigurationManager.AppSettings["RunInterval"], out runInterval);
                timer = new System.Timers.Timer(runInterval > 0 ? runInterval : 30000); //Use default interval == 30000 in case if config setting is missing or invalid
                inventoryLogger.LogInfo("InventoryProducerService Initialized");
            } catch (Exception e)
            {
                // thrown exceptions are not captured in log, so catching -> logging -> throwing
                inventoryLogger.LogError(e.ToString(), e.StackTrace);
                throw e;
            }
        }

        public void Start()
        {
            if (timer == null) return;
            timer.Elapsed += RunService;
            timer.Start();
        }

        private void RunService(object sender, ElapsedEventArgs e)
        {
            timer.Stop();
            var now = DateTime.Now;

            if (now.DayOfWeek == (DayOfWeek)dayOfTheWeek && now.TimeOfDay >= startTime && now.TimeOfDay <= endTime)
            {
                timer.Start();
                return;
            }

            ProducerType.Instance = producerInstanceID;


            try
            {
                isServiceRunning = true;
                inventoryLogger.LogInfo($"Starting Inventory Producer- Type: {ProducerType.Type} - Instance: {ProducerType.Instance}.");

                if (inventoryProducerBase != null)
                {
                    inventoryProducerBase.Execute();
                }

                // There's almost no delay between the last log entry in Execute() and this one, which sometimes causes the statements
                // to appear in the wrong order in the database.  This brief nap will add enough of a delay to prevent that.
                Thread.Sleep(100);

                inventoryLogger.LogInfo($"Shutting down Inventory Producer - Type: {ProducerType.Type} - Instance: {ProducerType.Instance}.");
            }
            catch (Exception ex)
            {
                switch (producerType)
                {
                    case "spoilage":
                        sSource = "Inventory Producer - Inventory Spoilage";
                        break;
                    default:
                        sSource = "Inventory Producer - Unknown Type";
                        break;
                }

                //Log to the Event Log
                try
                {
                    sLog = "Application";

                    sEvent = $"InventoryProducerService - RunService {ex.Message}";

                    if (ex.InnerException != null)
                        sEvent = $"{sEvent} -- {ex.InnerException.Message}";

                    if (!EventLog.SourceExists(sSource))
                        EventLog.CreateEventSource(sSource, sLog);

                    EventLog.WriteEntry(sSource, sEvent,
                    EventLogEntryType.Error);
                }
                catch (Exception eventLogException)
                {
                    inventoryLogger.LogError($"Unable to log to the Event Log. Inventory Producer type: {sSource}. Error: {ex.Message}", eventLogException.StackTrace);
                }
                finally
                {
                    inventoryLogger.LogError($"An unexpected error occurred. Inventory Producer type: {sSource}. Error: {ex.Message}", ex.StackTrace);
                }
            }
            finally
            {
                timer.Start();
                isServiceRunning = false;
            }
        }

        public void Stop()
        {
            if (timer == null) return;
            
            while (isServiceRunning)
            {
                inventoryLogger.LogInfo($"Waiting for service run to complete before stopping. Inventory Producer - Type: {ProducerType.Type} - Instance: {ProducerType.Instance}.");
                Thread.Sleep(30000);
            }
            timer.Stop();
            timer.Elapsed -= RunService;
        }
    }
}
