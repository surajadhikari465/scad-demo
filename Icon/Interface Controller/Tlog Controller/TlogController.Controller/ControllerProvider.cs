using Icon.Framework;
using Icon.Logging;
using TlogController.Common;
using TlogController.Controller.ProcessorModules;
using TlogController.Controller.Processors;
using TlogController.DataAccess.BulkCommands;
using TlogController.DataAccess.Queries;

namespace TlogController.Controller
{
    public static class ControllerProvider
    {
        private static string instance = StartupOptions.Instance.ToString();
        private static IconContext iconContext = new IconContext();
        public static TlogControllerBase ComposeController()
        {
            var controllerLogger = new NLogLoggerInstance<TlogControllerBase>(instance);

            var tlogProcessor = BuildTlogProcessor();

            return new TlogControllerBase(controllerLogger, tlogProcessor);
        }

        private static ITlogProcessor BuildTlogProcessor()
        {
            return new TlogProcessor(
                new NLogLoggerInstance<TlogProcessor>(StartupOptions.Instance.ToString()),
                BuildIconTlogProcessorModule());
        }

        private static IIconTlogProcessorModule BuildIconTlogProcessorModule()
        {
            return new IconTlogProcessorModule(
                new NLogLoggerInstance<IconTlogProcessorModule>(StartupOptions.Instance.ToString()),
                new GetBusinessUnitToRegionCodeMappingQueryHandler(iconContext),
                new BulkUpdateItemMovementInProcessCommandHandler(iconContext),
                new BulkUpdateItemMovementCommandHandler(new NLogLoggerInstance<BulkUpdateItemMovementCommandHandler>(StartupOptions.Instance.ToString()), iconContext));
        }
    }
}
