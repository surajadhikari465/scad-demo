using Icon.Framework;
using System.Collections.Generic;

namespace PushController.Controller.MessageGenerators
{
    public interface IMessageGenerator<T>
    {
        List<T> BuildMessages(List<IRMAPush> posDataReadyForEsb);
        void SaveMessages(List<T> messagesToSave);
    }
}
