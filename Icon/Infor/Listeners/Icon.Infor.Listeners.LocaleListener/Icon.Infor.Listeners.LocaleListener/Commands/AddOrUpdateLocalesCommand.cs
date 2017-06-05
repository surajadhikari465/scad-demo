using Icon.Infor.Listeners.LocaleListener.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Icon.Infor.Listeners.LocaleListener.Commands
{
    public class AddOrUpdateLocalesCommand
    {
        public LocaleModel Locale { get; set; }
    }
}
