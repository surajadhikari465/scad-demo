using AutoMapper;
using Icon.Framework;
using Icon.Web.Common;
using Icon.Web.DataAccess.Commands;
using Icon.Web.DataAccess.Infrastructure;
using Icon.Web.DataAccess.Queries;
using System;
using System.Data.Entity;
using System.Linq;
using Icon.Web.DataAccess.Models;
using System.Collections.Generic;
using Icon.Web.Common.Validators;
using Icon.Common.DataAccess;

namespace Icon.Web.DataAccess.Managers
{
    public class AddPluRequestManagerHandler : IManagerHandler<AddPluRequestManager>
    {
        private ICommandHandler<AddPluRequestCommand> addPluRequestCommandHandler;
        private ICommandHandler<AddPluRequestChangeHistoryCommand> addPluRequestChangeHistoryCommandHandler;
        private IQueryHandler<GetHierarchyClassByNameParameters, HierarchyClass> getHierarchyClassByNameQuery;
        private IObjectValidator<AddPluRequestManager> addPluRequestManager;

        public AddPluRequestManagerHandler(ICommandHandler<AddPluRequestCommand> addPluRequestCommandHandler,
            IQueryHandler<GetHierarchyClassByNameParameters, HierarchyClass> getHierarchyClassByNameQuery,
            ICommandHandler<AddPluRequestChangeHistoryCommand> addPluRequestChangeHistoryCommandHandler,
             IQueryHandler<GetTaxClassByTaxRomanceParameters, HierarchyClass> getTaxClassByTaxRomanceQuery,
            IObjectValidator<AddPluRequestManager> addPluRequestManager)
        {

            this.addPluRequestCommandHandler = addPluRequestCommandHandler;
            this.addPluRequestChangeHistoryCommandHandler = addPluRequestChangeHistoryCommandHandler;
            this.getHierarchyClassByNameQuery = getHierarchyClassByNameQuery;
            this.addPluRequestManager = addPluRequestManager;
        }

        public void Execute(AddPluRequestManager data)
        {
            SetFinancialId(data);
            ObjectValidationResult validationResult = addPluRequestManager.Validate(data);

            if (!validationResult.IsValid)
            {
                throw new ArgumentException(String.Format("Cannot add PLU Request. Error details: {0}", validationResult.Error));
            }
            
            try
            {               
                var addPluRequestCommand = new AddPluRequestCommand
                {
                    PLURequest = new PLURequest()
                    {
                        nationalPLU = data.NationalPlu,
                        scanCodeTypeID = data.PluType == ScanCodeTypes.PosPlu.ToString() ? ScanCodeTypes.PosPlu : ScanCodeTypes.ScalePlu,
                        brandName = data.BrandName,
                        itemDescription = data.ProductDescription,
                        posDescription = data.PosDescription,
                        packageUnit = data.PackageUnit,
                        retailSize = data.RetailSize,
                        retailUom = data.RetailUom,
                        FinancialClassID = data.FinancialHierarchyClassId.HasValue? data.FinancialHierarchyClassId.Value : -1,
                        nationalClassID = data.NationalHierarchyClassId,
                        lastModifiedUser = data.UserName,
                        requesterName = data.UserName,
                        requestStatus = PluRequestStatus.New.ToString(),
                        requestedDate = DateTime.Now,
                        lastModifiedDate = DateTime.Now
                    }
                };


                addPluRequestCommandHandler.Execute(addPluRequestCommand);
                if (!string.IsNullOrEmpty(data.Notes))
                {

                    var addPluRequestChangeHistoryCommand = new AddPluRequestChangeHistoryCommand()
                    {
                        PLURequestChangeHistory = new PLURequestChangeHistory()
                        {
                            pluRequestID = addPluRequestCommand.PLURequest.pluRequestID,
                            pluRequestChange = data.Notes,
                            pluRequestChangeTypeID = PLURequestChangeTypes.Notes,
                            insertedDate = DateTime.Now,
                            insertedUser = data.UserName
                        }
                    };
                    addPluRequestChangeHistoryCommandHandler.Execute(addPluRequestChangeHistoryCommand);
                }
            }
            catch (Exception ex)
            {
                throw new CommandException(String.Format("There was an error adding PLU request {0}", ex.Message), ex);
            }
        }

        private void SetFinancialId(AddPluRequestManager data)
        {
            if (!data.FinancialHierarchyClassId.HasValue && !String.IsNullOrWhiteSpace(data.FinancialHierarchyClassName))
            {
                var merchandiseHierarchyClass = getHierarchyClassByNameQuery.Search(new GetHierarchyClassByNameParameters
                {
                    HierarchyClassName = data.FinancialHierarchyClassName,
                    HierarchyName = HierarchyNames.Merchandise,
                    HierarchyLevel = HierarchyLevels.SubBrick
                });

                if (merchandiseHierarchyClass != null)
                {
                    data.FinancialHierarchyClassId = merchandiseHierarchyClass.hierarchyClassID;
                }
                else
                {
                    throw new ArgumentException((String.Format("Sub team Hierarchy Class {0} does not exist.", data.FinancialHierarchyClassName)));
                }
            }            
        }
    }
}
