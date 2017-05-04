using Esb.Core.MessageBuilders;
using Icon.Infor.Listeners.Price.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Icon.Infor.Listeners.Price.MessageBuilders
{
    public class PriceMessageBuilder : IMessageBuilder<IEnumerable<PriceModel>>
    {
        public string BuildMessage(IEnumerable<PriceModel> request)
        {
            throw new NotImplementedException();
        }
    }
}
