using RegionalEventController.Controller.Processors;

namespace RegionalEventController.Controller
{
    public class PrepareControllerBase
    {
        private INewItemProcessor iconReferenceProcessor;

        public PrepareControllerBase(INewItemProcessor iconReferenceProcessor)
        {
            this.iconReferenceProcessor = iconReferenceProcessor;
        }

        public void Start()
        {
            iconReferenceProcessor.Run();
        }

    }
}

