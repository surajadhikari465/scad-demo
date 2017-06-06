using Icon.Esb.Subscriber;
using Icon.Infor.Listeners.LocaleListener.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Icon.Infor.Listeners.LocaleListener.Commands
{
    public class ArchiveLocaleMessageCommand
    {
        public IEsbMessage Message { get; set; }
        public LocaleModel Locale { get; set; }
    }
}
