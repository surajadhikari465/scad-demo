using Icon.Framework;
using System.Collections.Generic;

namespace PushController.Controller.MessageBuilders
{
    public interface IMessageBuilder<T>
    {
        List<T> BuildMessages(List<IRMAPush> posDataReadyForEsb);
    }
}
