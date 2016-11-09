using Mammoth.Esb.LocaleListener.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mammoth.Esb.LocaleListener.Commands
{
    public class AddOrUpdateLocalesCommand
    {
        public List<LocaleModel> Locales { get; set; }
    }
}
