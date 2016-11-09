using System.Collections.Generic;

namespace PushController.Controller.PosDataStagingServices
{
    public interface IPosDataStagingService<T>
    {
        void StagePosDataBulk(List<T> posDataReadyToStage);
        void StagePosDataRowByRow(List<T> posDataReadyToStage);
    }
}
