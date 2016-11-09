using Irma.Framework;
using PushController.Common.Models;
using PushController.Controller.PosDataConverters;
using PushController.Controller.PosDataStagingServices;
using System;
using System.Collections.Generic;

namespace PushController.Controller.PosDataGenerators
{
    public class IrmaPushDataGenerator : IPosDataGenerator<IrmaPushModel>
    {
        private IPosDataConverter<IrmaPushModel> irmaPushDataConverter;
        private IPosDataStagingService<IrmaPushModel> irmaPushStagingService;

        public IrmaPushDataGenerator(
            IPosDataConverter<IrmaPushModel> irmaPushDataConverter,
            IPosDataStagingService<IrmaPushModel> irmaPushStagingService)
        {
            this.irmaPushDataConverter = irmaPushDataConverter;
            this.irmaPushStagingService = irmaPushStagingService;
        }

        public List<IrmaPushModel> ConvertPosData(List<IConPOSPushPublish> posDataReadyToConvert)
        {
            return irmaPushDataConverter.ConvertPosData(posDataReadyToConvert);
        }

        public void StagePosData(List<IrmaPushModel> posDataToStage)
        {
            try
            {
                irmaPushStagingService.StagePosDataBulk(posDataToStage);
            }
            catch (Exception)
            {
                StagePosDataRowByRow(posDataToStage);
            }
        }

        private void StagePosDataRowByRow(List<IrmaPushModel> posDataToStage)
        {
            irmaPushStagingService.StagePosDataRowByRow(posDataToStage);
        }
    }
}
