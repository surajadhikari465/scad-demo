using Controller.Core.Factories;
using Icon.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Controller.Core.ConsoleExample.Factories
{
    public class IconContextFactory : IGenericFactory<IconContext>
    {
        public IconContext Create()
        {
            return new IconContext();
        }
    }
}
