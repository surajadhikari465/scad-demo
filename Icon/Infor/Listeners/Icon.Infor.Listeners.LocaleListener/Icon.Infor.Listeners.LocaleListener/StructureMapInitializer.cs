using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using StructureMap;
using Icon.Infor.Listeners.LocaleListener.MessageParsers;
using Icon.Esb.MessageParsers;
using Icon.Logging;
using Icon.Infor.Listeners.LocaleListener.Models;

namespace Icon.Infor.Listeners.LocaleListener
{
    public static class StructureMapInitializer
    {
        public static Container InitializeContainer()
        {
            var container = new Container(c =>
            {
                c.For(typeof(ILogger<>)).Use(typeof( NLogLogger<>));
               // c.For(typeof(IMessageParser<List<LocaleModel>>).use LocaleMessageParser>();

            });

            return container;
        }
    }
}
