using Irma.Framework;
using System.Collections.Generic;

namespace PushController.Controller.PosDataGenerators
{
    public interface IPosDataGenerator<T>
    {
        List<T> ConvertPosData(List<IConPOSPushPublish> posDataReadyToConvert);
        void StagePosData(List<T> posDataToStage);
    }
}
