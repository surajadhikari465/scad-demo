using Icon.Framework;
using System.Collections.Generic;

namespace PushController.Controller.UdmEntityGenerators
{
    public interface IUdmEntityGenerator<T>
    {
        List<T> BuildEntities(List<IRMAPush> posDataReadyForUdm);
        void SaveEntities(List<T> entitiesToSave);
    }
}
