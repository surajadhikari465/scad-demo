using Controller.Core.Filters;
using System;

namespace Controller.Core.ConsoleExample
{
    public class RegionalController
    {
        private IFilter<RegionalControllerPipelineParameters> pipeline;
        private RegionalControllerSettings settings;

        public RegionalController()
        {

        }

        public RegionalController(
            RegionalControllerSettings settings,
            IFilter<RegionalControllerPipelineParameters> pipeline)
        {
            this.settings = settings;
            this.pipeline = pipeline;
        }

        public void Run()
        {
            pipeline.Execute(new RegionalControllerPipelineParameters());
        }
    }
}