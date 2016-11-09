using Icon.Logging;
using PushController.Controller.Processors;

namespace PushController.Controller
{
    public class PosPushControllerBase
    {
        private ILogger<PosPushControllerBase> logger;
        private IIrmaPosProcessor irmaPosProcessor;
        private IIconPosProcessor iconPosProcessor;

        public PosPushControllerBase(ILogger<PosPushControllerBase> logger, IIrmaPosProcessor irmaPosProcessor, IIconPosProcessor iconPosProcessor)
        {
            this.logger = logger;
            this.irmaPosProcessor = irmaPosProcessor;
            this.iconPosProcessor = iconPosProcessor;
        }

        public void Start()
        {
            irmaPosProcessor.StageIrmaPosData();
            iconPosProcessor.ProcessPosDataForEsb();
            iconPosProcessor.ProcessPosDataForUdm();
        }
    }
}
