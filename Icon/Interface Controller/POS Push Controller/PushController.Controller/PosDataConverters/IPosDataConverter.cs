using Irma.Framework;
using System.Collections.Generic;

namespace PushController.Controller.PosDataConverters
{
    public interface IPosDataConverter<T>
    {
        List<T> ConvertPosData(List<IConPOSPushPublish> posDataReadyToConvert);
    }
}
