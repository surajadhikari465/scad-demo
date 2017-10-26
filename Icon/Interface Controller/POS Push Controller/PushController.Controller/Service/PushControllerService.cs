
namespace PushController.Controller.Service
{
    using DataAccess.Queries;
    using Icon.Common;
    using Icon.Common.DataAccess;
    using Icon.Logging;
    using InterfaceController.Common;
    using Irma.Framework;
    using PushController.Common;
    using System;
    using System.Configuration;
    using System.Timers;

    public class PushControllerService : IPushControllerService
    {
        private static NLogLogger<Program> logger = new NLogLogger<Program>();
        private Timer timer;
        private const string flagKey = "GlobalPriceManagement";
        private PushController.DataAccess.Interfaces.IQueryHandler<GetInstanceDataFlagValueByFlagKeyQuery, bool> getInstanceDataFlagsByFlagKeyQueryHandler = new GetInstanceDataFlagValueByFlagKeyQueryHandler();
  
        public PushControllerService()
        {
            int runInterval = AppSettingsAccessor.GetIntSetting("RunInterval");
            this.timer = new Timer(runInterval);
        }

        public void Start()
        {
            this.timer.Elapsed += RunService;
            this.timer.Start();
        }

        private void RunService(object sender, ElapsedEventArgs eventArgs)
        {
            this.timer.Stop();

            Cache.ClearAll();

            try
            {
                int controllerInstanceId = AppSettingsAccessor.GetIntSetting("ControllerInstanceId");

                if (controllerInstanceId < 1)
                {
                    logger.Error("Please provide an integer greater than zero to be used as the unique instance ID.");
                    return;
                }

                StartupOptions.Instance = controllerInstanceId;

                string[] configuredRegions = ConfigurationManager.AppSettings["RegionsToProcess"].Split(',');

                if (configuredRegions.Length == 0)
                {
                    logger.Error("No regions are configured for processing.");
                    return;
                }

                string[] regionsToCheckForGPM = ConfigurationManager.AppSettings["RegionsToCheckForGPM"].Split(',');

                if (regionsToCheckForGPM.Length > 0)
                {
                    foreach (string region in regionsToCheckForGPM)
                    {
                        string connectionString = ConnectionBuilder.GetConnection(region);
                        IrmaContext context = new IrmaContext(ConnectionBuilder.GetConnection(region));

                        var query = new GetInstanceDataFlagValueByFlagKeyQuery
                        {
                            FlagKey = flagKey,
                            StoreNo = null,
                            Context = context
                        };

                        if (!Cache.regionCodeToGPMInstanceDataFlag.ContainsKey(region))
                        {
                            Cache.regionCodeToGPMInstanceDataFlag.Add(region, getInstanceDataFlagsByFlagKeyQueryHandler.Execute(query));
                        }                     
                    }
                }

                StartupOptions.RegionsToProcess = configuredRegions;

                int maxRecordsToProcess;
                if (!Int32.TryParse(ConfigurationManager.AppSettings["MaxRecordsToProcess"], out maxRecordsToProcess))
                {
                    maxRecordsToProcess = 100;
                }

                StartupOptions.MaxRecordsToProcess = maxRecordsToProcess;
                StartupOptions.UseItemTypeInsteadOfNonMerchTrait = AppSettingsAccessor.GetBoolSetting("UseItemTypeInsteadOfNonMerchTrait");

                var posController = ControllerProvider.ComposeController();

                logger.Info("Starting POS Push Controller...");

                posController.Start();

                logger.Info("Shutting down POS Push Controller...");
            }
            catch (Exception)
            {
                throw;
            }

            this.timer.Start();
        }

        public void Stop()
        {
            this.timer.Stop();
        }
    }
}
