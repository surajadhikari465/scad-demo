using Icon.Common.DataAccess;
using Icon.Web.Common.Validators;
using Icon.Web.DataAccess.Infrastructure;
using Icon.Web.DataAccess.Managers;
using Icon.Web.DataAccess.Models;
using Icon.Web.DataAccess.Queries;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Icon.Web.Mvc.Validators.Managers
{
    public class AddEwicMappingManagerValidator : IObjectValidator<AddEwicMappingManager>
    {
        private IQueryHandler<GetItemsByBulkScanCodeSearchParameters, List<ItemSearchModel>> wfmScanCodeExistsQuery;
        private IQueryHandler<GetAplScanCodesParameters, List<EwicAplScanCodeModel>> aplScanCodeExistsQuery;
        private IQueryHandler<GetCurrentEwicMappingParameters, string> getCurrentEwicMappingQuery;

        public AddEwicMappingManagerValidator(
            IQueryHandler<GetItemsByBulkScanCodeSearchParameters, List<ItemSearchModel>> wfmScanCodeExistsQuery,
            IQueryHandler<GetAplScanCodesParameters, List<EwicAplScanCodeModel>> aplScanCodeExistsQuery,
            IQueryHandler<GetCurrentEwicMappingParameters, string> getCurrentEwicMappingQuery)
        {
            this.wfmScanCodeExistsQuery = wfmScanCodeExistsQuery;
            this.aplScanCodeExistsQuery = aplScanCodeExistsQuery;
            this.getCurrentEwicMappingQuery = getCurrentEwicMappingQuery;
        }

        public ObjectValidationResult Validate(AddEwicMappingManager manager)
        {
            // The mapping must exist as a loaded or validated scan code in Icon.
            var wfmScanCodeExistsParameters = new GetItemsByBulkScanCodeSearchParameters { ScanCodes = new List<string> { manager.WfmScanCode } };

            bool scanCodeExists = wfmScanCodeExistsQuery.Search(wfmScanCodeExistsParameters).Count > 0;

            if (!scanCodeExists)
            {
                return ObjectValidationResult.InvalidResult(String.Format("Scan Code {0} does not exist in Icon.", manager.WfmScanCode));
            }

            // The APL Scan Code must exist in the APL.
            var aplScanCodeExistsParameters = new GetAplScanCodesParameters();

            bool aplScanCodeExists = aplScanCodeExistsQuery.Search(aplScanCodeExistsParameters).Any(apl => apl.ScanCode == manager.AplScanCode);

            if (!aplScanCodeExists)
            {
                return ObjectValidationResult.InvalidResult(String.Format("Scan Code {0} does not exist in the APL.", manager.AplScanCode));
            }

            // The WFM scan code cannot already be mapped to another APL scan code.
            var getCurrentMappingParameters = new GetCurrentEwicMappingParameters
            {
                AplScanCode = manager.AplScanCode,
                WfmScanCode = manager.WfmScanCode
            };

            string currentMapping = getCurrentEwicMappingQuery.Search(getCurrentMappingParameters);

            if (!String.IsNullOrEmpty(currentMapping))
            {
                return ObjectValidationResult.InvalidResult(String.Format("WFM Scan Code {0} is already mapped to APL scan code {1}.",
                    manager.WfmScanCode, currentMapping));
            }

            return ObjectValidationResult.ValidResult();
        }
    }
}