using Icon.Logging;
using TlogController.Controller.Processors;

namespace TlogController.Controller
{
    public class TlogControllerBase
    {
        private ILogger<TlogControllerBase> logger;
        private ITlogProcessor tlogProcessor;

        public TlogControllerBase(ILogger<TlogControllerBase> logger, ITlogProcessor tlogProcessor)
        {
            this.logger = logger;
            this.tlogProcessor = tlogProcessor;
        }

        public void Start()
        {
            tlogProcessor.Run();
        }
    }
}
