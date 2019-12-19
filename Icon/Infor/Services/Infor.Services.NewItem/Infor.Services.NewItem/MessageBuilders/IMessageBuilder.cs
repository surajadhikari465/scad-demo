using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.NewItem.MessageBuilders
{
    public interface IMessageBuilder<TModel>
    {
        string BuildMessage(TModel model);
    }
}
