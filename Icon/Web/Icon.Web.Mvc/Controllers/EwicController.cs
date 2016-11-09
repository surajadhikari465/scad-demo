using Icon.Common.DataAccess;
using Icon.Framework;
using Icon.Logging;
using Icon.Web.Common;
using Icon.Web.DataAccess.Commands;
using Icon.Web.DataAccess.Infrastructure;
using Icon.Web.DataAccess.Managers;
using Icon.Web.DataAccess.Models;
using Icon.Web.DataAccess.Queries;
using Icon.Web.Mvc.Attributes;
using Icon.Web.Mvc.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Web.Mvc;

namespace Icon.Web.Mvc.Controllers
{
    public class EwicController : Controller
    {
        private ILogger logger;
        private IQueryHandler<GetEwicExclusionsParameters, List<EwicExclusionModel>> getEwicExclusionsQueryHandler;
        private IQueryHandler<GetAplScanCodesParameters, List<EwicAplScanCodeModel>> getAplScanCodesQueryHandler;
        private IQueryHandler<GetEwicMappingsParameters, List<EwicMappingModel>> getEwicMappingsQueryHandler;
        private IQueryHandler<GetItemTraitByTraitCodeParameters, ItemTrait> getItemTraitQueryHandler;
        private IManagerHandler<AddEwicExclusionManager> addEwicExclusionManagerHandler;
        private IManagerHandler<AddEwicMappingManager> addEwicMappingManagerHandler;
        private IManagerHandler<RemoveEwicExclusionManager> removeEwicExclusionManagerHandler;
        private IManagerHandler<RemoveEwicMappingManager> removeEwicMappingManagerHandler;

        public EwicController(
            ILogger logger,
            IQueryHandler<GetEwicExclusionsParameters, List<EwicExclusionModel>> getEwicExclusionsQueryHandler,
            IQueryHandler<GetAplScanCodesParameters, List<EwicAplScanCodeModel>> getAplScanCodesQueryHandler,
            IQueryHandler<GetEwicMappingsParameters, List<EwicMappingModel>> getEwicMappingsQueryHandler,
            IQueryHandler<GetItemTraitByTraitCodeParameters, ItemTrait> getItemTraitQueryHandler,
            IManagerHandler<AddEwicExclusionManager> addEwicExclusionManagerHandler,
            IManagerHandler<RemoveEwicExclusionManager> removeEwicExclusionManagerHandler,
            IManagerHandler<AddEwicMappingManager> addEwicMappingManagerHandler,
            IManagerHandler<RemoveEwicMappingManager> removeEwicMappingManagerHandler)
        {
            this.logger = logger;
            this.getEwicExclusionsQueryHandler = getEwicExclusionsQueryHandler;
            this.getAplScanCodesQueryHandler = getAplScanCodesQueryHandler;
            this.getEwicMappingsQueryHandler = getEwicMappingsQueryHandler;
            this.getItemTraitQueryHandler = getItemTraitQueryHandler;
            this.addEwicExclusionManagerHandler = addEwicExclusionManagerHandler;
            this.removeEwicExclusionManagerHandler = removeEwicExclusionManagerHandler;
            this.addEwicMappingManagerHandler = addEwicMappingManagerHandler;
            this.removeEwicMappingManagerHandler = removeEwicMappingManagerHandler;
        }

        // GET: Ewic/Exclusions
        public ActionResult Exclusions()
        {
            var viewModel = new EwicExclusionViewModel();

            return View(BuildExclusionViewModel(viewModel));
        }

        // POST: Ewic/AddExclusion
        [WriteAccessAuthorize]
        [HttpPost]
        public ActionResult AddExclusion(EwicExclusionViewModel viewModel)
        {
            if (!ModelState.IsValid)
            {
                return View("Exclusions", BuildExclusionViewModel(viewModel));
            }

            var parameters = new GetItemTraitByTraitCodeParameters
            {
                ScanCode = viewModel.NewExclusion,
                TraitCode = TraitCodes.ProductDescription
            };

            string productDescription;
            try
            {
                productDescription = getItemTraitQueryHandler.Search(parameters).traitValue;
            }
            catch (Exception ex)
            {
                var exceptionLogger = new ExceptionLogger(logger);
                exceptionLogger.LogException(ex, this.GetType(), MethodBase.GetCurrentMethod());
                ViewData["ErrorMessage"] = String.Format("Missing required trait information for scan code {0}.", viewModel.NewExclusion);
                return View("Exclusions", BuildExclusionViewModel(viewModel));
            }

            var manager = new AddEwicExclusionManager
            {
                ScanCode = viewModel.NewExclusion,
                ProductDescription = productDescription
            };

            try
            {
                addEwicExclusionManagerHandler.Execute(manager);
                ViewData["SuccessMessage"] = "The exclusion was added successfully and transmitted to R10.";
            }
            catch (Exception ex)
            {
                var exceptionLogger = new ExceptionLogger(logger);
                exceptionLogger.LogException(ex, this.GetType(), MethodBase.GetCurrentMethod());
                ViewData["ErrorMessage"] = ex.Message;
            }

            return View("Exclusions", BuildExclusionViewModel(viewModel));
        }

        // POST: Ewic/RemoveExclusion
        [WriteAccessAuthorize]
        [HttpPost]
        public ActionResult RemoveExclusion(EwicExclusionViewModel viewModel)
        {
            // Ignore model state errors for the other form on the page.
            if (ModelState.ContainsKey("NewExclusion"))
            {
                ModelState["NewExclusion"].Errors.Clear();
            }

            if (!ModelState.IsValid)
            {
                return View("Exclusions", BuildExclusionViewModel(viewModel));
            }

            if (String.IsNullOrEmpty(viewModel.RemovedExclusionSelectedId))
            {
                ViewData["ErrorMessage"] = "Please select an exclusion to remove.";
                return View("Exclusions", BuildExclusionViewModel(viewModel));
            }

            var parameters = new GetItemTraitByTraitCodeParameters
            {
                ScanCode = viewModel.RemovedExclusionSelectedId,
                TraitCode = TraitCodes.ProductDescription
            };

            string productDescription;
            try
            {
                productDescription = getItemTraitQueryHandler.Search(parameters).traitValue;
            }
            catch (Exception ex)
            {
                var exceptionLogger = new ExceptionLogger(logger);
                exceptionLogger.LogException(ex, this.GetType(), MethodBase.GetCurrentMethod());
                ViewData["ErrorMessage"] = String.Format("Missing required trait information for scan code {0}.", viewModel.RemovedExclusionSelectedId);
                return View("Exclusions", BuildExclusionViewModel(viewModel));
            }

            var manager = new RemoveEwicExclusionManager
            {
                ScanCode = viewModel.RemovedExclusionSelectedId,
                ProductDescription = productDescription
            };

            try
            {
                removeEwicExclusionManagerHandler.Execute(manager);
                ViewData["SuccessMessage"] = "The exclusion was successfully removed and transmitted to R10.";
            }
            catch (Exception ex)
            {
                var exceptionLogger = new ExceptionLogger(logger);
                exceptionLogger.LogException(ex, this.GetType(), MethodBase.GetCurrentMethod());
                ViewData["ErrorMessage"] = ex.Message;
            }

            return View("Exclusions", BuildExclusionViewModel(viewModel));
        }

        // GET: Ewic/Mappings
        public ActionResult Mappings()
        {
            var viewModel = new EwicMappingSearchViewModel();

            return View(BuildMappingSearchViewModel(viewModel));
        }

        // GET: Ewic/MappingDetail
        public ActionResult MappingDetail(EwicMappingSearchViewModel searchViewModel)
        {
            if (!ModelState.IsValid)
            {
                return View("Mappings", BuildMappingSearchViewModel(searchViewModel));
            }

            var detailViewModel = new EwicMappingDetailViewModel();

            return View(BuildMappingDetailViewModel(detailViewModel, searchViewModel.AplScanCodeSelectedId));
        }

        // POST: Ewic/AddMapping
        [WriteAccessAuthorize]
        [HttpPost]
        public ActionResult AddMapping(EwicMappingDetailViewModel viewModel)
        {
            // Ignore model state errors for the other form on the page.
            if (ModelState.ContainsKey("RemovableMappedWfmScanCodesSelectedId"))
            {
                ModelState["RemovableMappedWfmScanCodesSelectedId"].Errors.Clear();
            }

            if (!ModelState.IsValid)
            {
                return View("MappingDetail", BuildMappingDetailViewModel(viewModel, viewModel.AplScanCode));
            }

            var parameters = new GetItemTraitByTraitCodeParameters
            {
                ScanCode = viewModel.NewMapping,
                TraitCode = TraitCodes.ProductDescription
            };

            string productDescription;
            try
            {
                productDescription = getItemTraitQueryHandler.Search(parameters).traitValue;
            }
            catch (Exception ex)
            {
                var exceptionLogger = new ExceptionLogger(logger);
                exceptionLogger.LogException(ex, this.GetType(), MethodBase.GetCurrentMethod());
                ViewData["ErrorMessage"] = String.Format("Missing required trait information for scan code {0}.", viewModel.NewMapping);
                return View("MappingDetail", BuildMappingDetailViewModel(viewModel, viewModel.AplScanCode));
            }

            var manager = new AddEwicMappingManager
            {
                AplScanCode = viewModel.AplScanCode,
                WfmScanCode = viewModel.NewMapping,
                ProductDescription = productDescription
            };

            try
            {
                addEwicMappingManagerHandler.Execute(manager);
                ViewData["SuccessMessage"] = "The mapping was added successfully and transmitted to R10.";
            }
            catch (Exception ex)
            {
                var exceptionLogger = new ExceptionLogger(logger);
                exceptionLogger.LogException(ex, this.GetType(), MethodBase.GetCurrentMethod());
                ViewData["ErrorMessage"] = ex.Message;
            }

            return View("MappingDetail", BuildMappingDetailViewModel(viewModel, viewModel.AplScanCode));
        }

        // POST: Ewic/RemoveMapping
        [WriteAccessAuthorize]
        [HttpPost]
        public ActionResult RemoveMapping(EwicMappingDetailViewModel viewModel)
        {
            // Ignore model state errors for the other form on the page.
            if (ModelState.ContainsKey("NewMapping"))
            {
                ModelState["NewMapping"].Errors.Clear();
            }

            if (!ModelState.IsValid)
            {
                return View("MappingDetail", BuildMappingDetailViewModel(viewModel, viewModel.AplScanCode));
            }

            var parameters = new GetItemTraitByTraitCodeParameters
            {
                ScanCode = viewModel.RemovableMappedWfmScanCodesSelectedId,
                TraitCode = TraitCodes.ProductDescription
            };

            string productDescription;
            try
            {
                productDescription = getItemTraitQueryHandler.Search(parameters).traitValue;
            }
            catch (Exception ex)
            {
                var exceptionLogger = new ExceptionLogger(logger);
                exceptionLogger.LogException(ex, this.GetType(), MethodBase.GetCurrentMethod());
                ViewData["ErrorMessage"] = String.Format("Missing required trait information for scan code {0}.", viewModel.RemovableMappedWfmScanCodesSelectedId);
                return View("MappingDetail", BuildMappingDetailViewModel(viewModel, viewModel.AplScanCode));
            }

            var manager = new RemoveEwicMappingManager
            {
                AplScanCode = viewModel.AplScanCode,
                WfmScanCode = viewModel.RemovableMappedWfmScanCodesSelectedId,
                ProductDescription = productDescription
            };

            try
            {
                removeEwicMappingManagerHandler.Execute(manager);
                ViewData["SuccessMessage"] = "The mapping was successfully removed and transmitted to R10.";
            }
            catch (Exception ex)
            {
                var exceptionLogger = new ExceptionLogger(logger);
                exceptionLogger.LogException(ex, this.GetType(), MethodBase.GetCurrentMethod());
                ViewData["ErrorMessage"] = ex.Message;
            }

            return View("MappingDetail", BuildMappingDetailViewModel(viewModel, viewModel.AplScanCode));
        }

        private EwicExclusionViewModel BuildExclusionViewModel(EwicExclusionViewModel viewModel)
        {
            var exclusions = getEwicExclusionsQueryHandler.Search(new GetEwicExclusionsParameters()).OrderBy(e => e.ScanCode).ToList();

            viewModel.CurrentExclusions = exclusions;
            viewModel.RemovableEwicExclusions = exclusions.Select(e => new SelectListItem { Value = e.ScanCode, Text = e.ScanCode });

            return viewModel;
        }

        private EwicMappingSearchViewModel BuildMappingSearchViewModel(EwicMappingSearchViewModel viewModel)
        {
            var availableMappingTargets = getAplScanCodesQueryHandler.Search(new GetAplScanCodesParameters()).OrderBy(q => q.ScanCode).ToList();

            viewModel.AplScanCodes = availableMappingTargets.Select(m => new SelectListItem { Value = m.ScanCode, Text = m.ScanCode + " " + m.ItemDescription });

            return viewModel;
        }

        private EwicMappingDetailViewModel BuildMappingDetailViewModel(EwicMappingDetailViewModel viewModel, string aplScanCode)
        {
            var parameters = new GetEwicMappingsParameters { AplScanCode = aplScanCode };
            var mappedScanCodes = getEwicMappingsQueryHandler.Search(parameters).OrderBy(m => m.ScanCode).ToList();

            viewModel.AplScanCode = aplScanCode;
            viewModel.CurrentMappings = mappedScanCodes;
            viewModel.RemovableMappedWfmScanCodes = mappedScanCodes.Select(m => new SelectListItem { Value = m.ScanCode, Text = m.ScanCode + " " + m.ProductDescription });

            return viewModel;
        }
        
        public bool ValidateAplScanCode(string aplScanCode)
        {
            var availableMappingTargets = getAplScanCodesQueryHandler.Search(new GetAplScanCodesParameters()).OrderBy(q => q.ScanCode).ToList();

            return availableMappingTargets.Any(m => m.ScanCode.Equals(aplScanCode));
        }
    }
}