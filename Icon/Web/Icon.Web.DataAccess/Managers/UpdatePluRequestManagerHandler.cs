using Icon.Common.DataAccess;
using Icon.Framework;
using Icon.Web.Common;
using Icon.Web.DataAccess.Commands;
using Icon.Web.DataAccess.Infrastructure;
using Icon.Web.DataAccess.Queries;
using System;
using System.Data.Entity.Validation;
using Icon.Common.Validators;

namespace Icon.Web.DataAccess.Managers
{
    public class UpdatePluRequestManagerHandler : IManagerHandler<UpdatePluRequestManager>
    {
        private ICommandHandler<UpdatePluRequestCommand> updatePluRequestCommandHandler;
        private ICommandHandler<AddPluRequestChangeHistoryCommand> addPluRequestChangeHistoryCommandHandler;
        private IQueryHandler<GetPluRequestByIdParameters, PLURequest> getPluRequestByIdQuery;
        private IObjectValidator<UpdatePluRequestManager> updatePluRequestManager;

        public UpdatePluRequestManagerHandler(
            ICommandHandler<UpdatePluRequestCommand> updatePluRequestCommandHandler,
            ICommandHandler<AddPluRequestChangeHistoryCommand> addPluRequestChangeHistoryCommandHandler,
            IQueryHandler<GetPluRequestByIdParameters, PLURequest> getPluRequestByIdQuery,
            IObjectValidator<UpdatePluRequestManager> updatePluRequestManager)
        {
            this.updatePluRequestCommandHandler = updatePluRequestCommandHandler;
            this.addPluRequestChangeHistoryCommandHandler = addPluRequestChangeHistoryCommandHandler;
            this.getPluRequestByIdQuery = getPluRequestByIdQuery;
            this.updatePluRequestManager = updatePluRequestManager;
        }

        public void Execute(UpdatePluRequestManager data)
        {
            ObjectValidationResult validationResult = updatePluRequestManager.Validate(data);

            if (!validationResult.IsValid)
            {
                throw new ArgumentException(String.Format("Cannot update PLU Request. Error details: {0}", validationResult.Error));
            }

           try
            {
                var PluRequest = getPluRequestByIdQuery.Search(new GetPluRequestByIdParameters() { PluRequestId = data.PluRequestId });

                bool statusChanged = PluRequest.requestStatus != data.RequestStatus;
                var statusChangeNotes = PluRequest.requestStatus + " To " + data.RequestStatus;
                var updatePluRequestCommand = new UpdatePluRequestCommand
                 {
                     PLURequest = new PLURequest()
                     {
                         pluRequestID = data.PluRequestId,
                         nationalPLU = data.NationalPlu,
                         scanCodeTypeID = data.PluType == ScanCodeTypes.PosPlu.ToString() ? ScanCodeTypes.PosPlu : ScanCodeTypes.ScalePlu,
                         brandName = data.BrandName,
                         itemDescription = data.ProductDescription,
                         posDescription = data.PosDescription,
                         retailSize = data.RetailSize,
                         retailUom = data.RetailUom,
                         merchandiseClassID = data.MerchandiseHierarchyClassId,
                         nationalClassID = data.NationalHierarchyClassId,
                         lastModifiedUser = data.UserName,
                         lastModifiedDate = DateTime.Now,
                         requestStatus = data.RequestStatus,
                         packageUnit = data.PackageUnit,
                         FinancialClassID = data.FinancialHierarchyClassId
                     }
                 };

                updatePluRequestCommandHandler.Execute(updatePluRequestCommand);

                if (!string.IsNullOrEmpty(data.Notes))
                {

                    var addPluRequestChangeHistoryCommand = new AddPluRequestChangeHistoryCommand()
                    {
                        PLURequestChangeHistory = new PLURequestChangeHistory()
                        {
                            pluRequestID = updatePluRequestCommand.PLURequest.pluRequestID,
                            pluRequestChange = data.Notes,
                            pluRequestChangeTypeID = PLURequestChangeTypes.Notes,
                            insertedDate = DateTime.Now,
                            insertedUser = data.UserName
                        }
                    };
                    addPluRequestChangeHistoryCommandHandler.Execute(addPluRequestChangeHistoryCommand);
                }

                if (statusChanged)
                {

                    var addPluRequestChangeHistoryCommand = new AddPluRequestChangeHistoryCommand()
                    {
                        PLURequestChangeHistory = new PLURequestChangeHistory()
                        {
                            pluRequestID = updatePluRequestCommand.PLURequest.pluRequestID,
                            pluRequestChange = statusChangeNotes,
                            pluRequestChangeTypeID = PLURequestChangeTypes.StatusChange,
                            insertedDate = DateTime.Now,
                            insertedUser = data.UserName
                        }
                    };
                    addPluRequestChangeHistoryCommandHandler.Execute(addPluRequestChangeHistoryCommand);
                }

            }
            catch (DbEntityValidationException exception)
            {
                throw new CommandException(String.Format("There was a database validation error when updating national PLU {0} request.", data.NationalPlu), exception);
            }
            catch (InvalidOperationException exception)
            {
                throw new CommandException(exception.Message, exception);
            }
            catch (Exception exception)
            {
                throw new CommandException(String.Format("There was an error while updating updating national PLU {0} request: {1}", data.NationalPlu, exception.Message), exception);
            }
        }        
    }
}
