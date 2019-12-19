using Icon.Common.DataAccess;
using Icon.Common.Validators;
using Icon.Web.DataAccess.Managers;
using Icon.Web.DataAccess.Models;
using Icon.Web.DataAccess.Queries;
using System;
using System.Collections.Generic;

namespace Icon.Web.Mvc.Validators.Managers
{
    public class AddEwicExclusionManagerValidator : IObjectValidator<AddEwicExclusionManager>
    {
        private IQueryHandler<GetItemsByBulkScanCodeSearchParameters, List<ItemSearchModel>> scanCodeExistsQuery;

        public AddEwicExclusionManagerValidator(
            IQueryHandler<GetItemsByBulkScanCodeSearchParameters, List<ItemSearchModel>> scanCodeExistsQuery)
        {
            this.scanCodeExistsQuery = scanCodeExistsQuery;
        }

        public ObjectValidationResult Validate(AddEwicExclusionManager manager)
        {
            var parameters = new GetItemsByBulkScanCodeSearchParameters { ScanCodes = new List<string> { manager.ScanCode } };

            bool scanCodeExists = scanCodeExistsQuery.Search(parameters).Count > 0;

            if (!scanCodeExists)
            {
                return ObjectValidationResult.InvalidResult(String.Format("Scan Code {0} does not exist in Icon.", manager.ScanCode));
            }

            return ObjectValidationResult.ValidResult();
        }
    }
}