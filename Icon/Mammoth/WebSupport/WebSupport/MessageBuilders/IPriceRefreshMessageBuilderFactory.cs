using Esb.Core.MessageBuilders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebSupport.DataAccess.Models;
using WebSupport.Models;

namespace WebSupport.MessageBuilders
{
    public interface IPriceRefreshMessageBuilderFactory
    {
        IMessageBuilder<List<GpmPrice>> CreateMessageBuilder(string system);
    }
}
