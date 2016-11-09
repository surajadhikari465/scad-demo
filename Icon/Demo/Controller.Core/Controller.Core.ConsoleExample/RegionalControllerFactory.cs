using Controller.Core.Factories;
using SimpleInjector;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Controller.Core.ConsoleExample
{
    public class RegionalControllerFactory : IGenericFactory<RegionalController, string>
    {
        Container container;

        public RegionalControllerFactory(Container container)
        {
            this.container = container;
        }

        public RegionalController Create()
        {
            throw new NotImplementedException();
        }

        public RegionalController Create(string region)
        {
            RegionalController controller = new RegionalController();

            return controller;
        }
    }
}
