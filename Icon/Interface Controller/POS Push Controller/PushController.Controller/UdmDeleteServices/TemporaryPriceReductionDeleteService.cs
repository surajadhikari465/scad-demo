using PushController.Common.Models;
using PushController.DataAccess.Commands;
using PushController.DataAccess.Interfaces;
using System.Collections.Generic;

namespace PushController.Controller.UdmDeleteServices
{
    public class TemporaryPriceReductionDeleteService : IUdmDeleteService<TemporaryPriceReductionModel>
    {
        private ICommandHandler<DeleteTemporaryPriceReductionsCommand> deleteTemporaryPriceReductionsCommandHandler;

        public TemporaryPriceReductionDeleteService(ICommandHandler<DeleteTemporaryPriceReductionsCommand> deleteTemporaryPriceReductionsCommandHandler)
        {
            this.deleteTemporaryPriceReductionsCommandHandler = deleteTemporaryPriceReductionsCommandHandler;
        }

        public void DeleteEntitiesBulk(List<TemporaryPriceReductionModel> temporaryPriceReductions)
        {
            var command = new DeleteTemporaryPriceReductionsCommand
            {
                TemporaryPriceReductions = temporaryPriceReductions
            };

            deleteTemporaryPriceReductionsCommandHandler.Execute(command);
        }

        public void DeleteEntitiesRowByRow(List<TemporaryPriceReductionModel> temporaryPriceReductions)
        {
            foreach (var tpr in temporaryPriceReductions)
            {
                deleteTemporaryPriceReductionsCommandHandler.Execute(new DeleteTemporaryPriceReductionsCommand 
                {
                    TemporaryPriceReductions = new List<TemporaryPriceReductionModel> { tpr }
                });
            }
        }
    }
}
