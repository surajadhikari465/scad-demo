using Icon.Framework;
using System.Collections.Generic;

namespace PushController.Controller.UdmEntityBuilders
{
    public interface IUdmEntityBuilder<T>
    {
        List<T> BuildEntities(List<IRMAPush> posDataReadyForUdm);
    }
}
