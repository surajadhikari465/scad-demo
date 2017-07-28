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
        public IEnumerable<LocaleModel> chains { get; set; }
        public IEnumerable<LocaleModel> regions { get; set; }
        public IEnumerable<LocaleModel> metros { get; set; }
        public IEnumerable<LocaleModel> stores { get; set; }
    }
}
