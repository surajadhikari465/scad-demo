
namespace PushController.Controller.Service
{
    using DataAccess.Queries;
    using Icon.Common;
    using Icon.Common.DataAccess;
    using Icon.Logging;
    using InterfaceController.Common;
    using Irma.Framework;
    using PushController.Common;
    using PushController.Common.Models;
    using System;
    using System.Collections.Generic;
    using System.Configuration;
    using System.Linq;
    using System.Timers;

    public class PushControllerService : IPushControllerService
    {
        private static NLogLogger<Program> logger = new NLogLogger<Program>();
        private Timer timer;
        private const string flagKey = "GlobalPriceManagement";
        private PushController.DataAccess.Interfaces.IQueryHandler<GetInstanceDataFlagValueByFlagKeyQuery, bool> getInstanceDataFlagsByFlagKeyQueryHandler = new GetInstanceDataFlagValueByFlagKeyQueryHandler();
        private PushController.DataAccess.Interfaces.IQueryHandler<GetInstanceDataFlagStoreValuesQuery, IEnumerable<InstanceDataFlagStoreValues>> getInstanceDataFlagStoreValueList = new GetInstanceDataFlagStoreValuesQueryHandler();

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

                string[] regionsToCheckForGpm = ConfigurationManager.AppSettings["RegionsToCheckForGPM"].Split(',');

                if (regionsToCheckForGpm.Length > 0)
                {
                    foreach (string region in regionsToCheckForGpm)
                    {
                        try
                        {
                            string connectionString = ConnectionBuilder.GetConnection(region);
                            IrmaContext context = new IrmaContext(ConnectionBuilder.GetConnection(region));

                            // Populate the non-gpm store list
                            var getGpmInstanceDataFlagStoreQuery = new GetInstanceDataFlagStoreValuesQuery
                            {
                                FlagKey = flagKey,
                                Context = context
                            };

                            IEnumerable<InstanceDataFlagStoreValues> gpmFlagStoreValues = getInstanceDataFlagStoreValueList.Execute(getGpmInstanceDataFlagStoreQuery);
                            var nonGpmFlagValuesByStore = gpmFlagStoreValues
                                .Where(fsv => !fsv.FlagValue)
                                .Select(x => x.BusinessUnitId)
                                .Distinct();
                            Cache.nonGpmStores.UnionWith(nonGpmFlagValuesByStore);
                        }
                        catch (Exception error)
                        {
                            logger.Error("Not able to connect to Irma Database: " + region +". Error is:"+ error.ToString());
                            continue;
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
