using RegionalEventController.Controller.Processors;
using Icon.Logging;
using Icon.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RegionalEventController.Controller
{
    public class RegionalControllerBase
    {
        private INewItemProcessor newItemProcessor;

        public RegionalControllerBase(INewItemProcessor newItemProcessor)
        {
            this.newItemProcessor = newItemProcessor;
        }

        public void Start()
        {
            newItemProcessor.Run();
        }

    }
}
