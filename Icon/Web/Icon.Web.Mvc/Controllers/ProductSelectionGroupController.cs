using Icon.Common.DataAccess;
using Icon.Framework;
using Icon.Logging;
using Icon.Web.Common;
using Icon.Web.DataAccess.Infrastructure;
using Icon.Web.DataAccess.Managers;
using Icon.Web.DataAccess.Queries;
using Icon.Web.Mvc.Models;
using Infragistics.Web.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Web.Mvc;

namespace Icon.Web.Mvc.Controllers
{
    [Authorize(Roles = "WFM\\IRMA.Applications, WFM\\IRMA.Developers")]
    public class ProductSelectionGroupController : Controller
    {
        private ILogger logger;
        private IQueryHandler<GetProductSelectionGroupsParameters, List<ProductSelectionGroup>> getProductSelectionGroupsQueryHandler;
        private IQueryHandler<GetProductSelectionGroupTypesParameters, List<ProductSelectionGroupType>> getProductSelectionGroupTypesQueryHandler;
        private IQueryHandler<GetTraitsParameters, List<Trait>> getTraitsQueryHandler;
        private IQueryHandler<GetHierarchyParameters, List<Hierarchy>> getHierarchiesQueryHandler;
        private IManagerHandler<AddProductSelectionGroupManager> addProductSelectionGroupManagerHandler;
        private IManagerHandler<UpdateProductSelectionGroupManager> updateProductSelectionGroupManagerHandler;

        public ProductSelectionGroupController(
            ILogger logger,
            IQueryHandler<GetProductSelectionGroupsParameters, List<ProductSelectionGroup>> getProductSelectionGroupsQueryHandler,
            IQueryHandler<GetProductSelectionGroupTypesParameters, List<ProductSelectionGroupType>> getProductSelectionGroupTypesQueryHandler,
            IQueryHandler<GetTraitsParameters, List<Trait>> getTraitsQueryHandler,
            IQueryHandler<GetHierarchyParameters, List<Hierarchy>> getHierarchiesQueryHandler,
            IManagerHandler<AddProductSelectionGroupManager> addProductSelectionGroupManagerHandler,
            IManagerHandler<UpdateProductSelectionGroupManager> updateProductSelectionGroupManagerHandler)
        {
            this.logger = logger;
            this.getProductSelectionGroupsQueryHandler = getProductSelectionGroupsQueryHandler;
            this.getProductSelectionGroupTypesQueryHandler = getProductSelectionGroupTypesQueryHandler;
            this.getTraitsQueryHandler = getTraitsQueryHandler;
            this.getHierarchiesQueryHandler = getHierarchiesQueryHandler;
            this.addProductSelectionGroupManagerHandler = addProductSelectionGroupManagerHandler;
            this.updateProductSelectionGroupManagerHandler = updateProductSelectionGroupManagerHandler;
        }

        public ActionResult Index()
        {
            IEnumerable<ProductSelectionGroupViewModel> productSelectionGroups = getProductSelectionGroupsQueryHandler
                .Search(new GetProductSelectionGroupsParameters())
                .Select(p => new ProductSelectionGroupViewModel
                {
                    ProductSelectionGroupId = p.ProductSelectionGroupId,
                    ProductSelectionGroupName = p.ProductSelectionGroupName,
                    ProductSelectionGroupTypeId = p.ProductSelectionGroupTypeId,
                    TraitId = p.TraitId,
                    TraitValue = p.TraitValue,
                    MerchandiseHierarchyClassId = p.MerchandiseHierarchyClassId
                });

            List<ProductSelectionGroupType> productSelectionGroupTypes = getProductSelectionGroupTypesQueryHandler.Search(new GetProductSelectionGroupTypesParameters());
            List<Trait> traits = getTraitsQueryHandler.Search(new GetTraitsParameters { IncludeNavigation = true });
            List<Hierarchy> merchandiseHierarchy = getHierarchiesQueryHandler.Search(new GetHierarchyParameters { HierarchyId = Hierarchies.Merchandise, IncludeNavigation = true });

            var viewModel = new ProductSelectionGroupGridViewModel
            {
                ProductSelectionGroups = productSelectionGroups.ToList(),
                ProductSelectionGroupTypes = productSelectionGroupTypes.Select(type => new ProductSelectionGroupTypeViewModel(type)).ToList(),
                Traits = traits.Select(trait => new TraitViewModel(trait)).ToList(),
                MerchandiseHierarchyClasses = merchandiseHierarchy.First(h => h.hierarchyID == Hierarchies.Merchandise).HierarchyClass
                    .Where(hc => hc.hierarchyLevel == HierarchyLevels.SubBrick)
                    .Select(merch => new HierarchyClassViewModel
                    {
                        HierarchyClassId = merch.hierarchyClassID,
                        HierarchyClassName = merch.hierarchyClassName + "|" + merch.hierarchyClassID,
                        HierarchyId = merch.hierarchyID,
                        HierarchyLevel = merch.hierarchyLevel,
                        HierarchyParentClassId = merch.hierarchyParentClassID
                    })
                    .OrderBy(hc => hc.HierarchyClassName)
                    .ToList()
            };

            return View("Index", viewModel);
        }

        [HttpGet]
        [OutputCache(Duration = 20)]
        public ActionResult Create()
        {
            var viewModel = BuildProductSelectionGroupCreateViewModel();
            return View(viewModel);
        }

        [HttpPost]
        public ActionResult Create(ProductSelectionGroupCreateViewModel viewModel)
        {
            if (!ModelState.IsValid)
            {
                BuildProductSelectionGroupCreateViewModel(viewModel);
                return View("Create", viewModel);
            }

            string error = CheckModelValidation(viewModel);
            if (!String.IsNullOrEmpty(error))
            {
                ViewData["Error"] = error;
                BuildProductSelectionGroupCreateViewModel(viewModel);
                return View("Create", viewModel);
            }

            try
            {
                var addPsgManager = new AddProductSelectionGroupManager
                {
                    ProductSelectionGroupName = viewModel.ProductSelectionGroupName,
                    ProductSelectionGroupTypeId = viewModel.SelectedProductSelectionGroupTypeId,
                    TraitId = viewModel.SelectedTraitId,
                    TraitValue = viewModel.TraitValue,
                    MerchandiseHierarchyClassId = viewModel.SelectedMerchandiseHierarchyClassId
                };

                addProductSelectionGroupManagerHandler.Execute(addPsgManager);
            }
            catch (Exception ex)
            {
                ViewData["Error"] = ex.Message;
                BuildProductSelectionGroupCreateViewModel(viewModel);
                return View(viewModel);
            }

            ViewData["Message"] = "Successfully created the Product Selection Group.";
            return RedirectToAction("Index");
        }

        private ProductSelectionGroupCreateViewModel BuildProductSelectionGroupCreateViewModel(ProductSelectionGroupCreateViewModel viewModel = null)
        {
            if (viewModel == null)
            {
                viewModel = new ProductSelectionGroupCreateViewModel();
            }

            List<ProductSelectionGroupType> productSelectionGroupTypes = getProductSelectionGroupTypesQueryHandler
                .Search(new GetProductSelectionGroupTypesParameters());

            List<Trait> traits = getTraitsQueryHandler
                .Search(new GetTraitsParameters { IncludeNavigation = true });

            List<Hierarchy> merchandiseHierarchy = getHierarchiesQueryHandler
                .Search(new GetHierarchyParameters { HierarchyId = Hierarchies.Merchandise, IncludeNavigation = true });

            List<HierarchyClassViewModel> merchandiseSubBricks = merchandiseHierarchy.Single().HierarchyClass
                .Where(hc => hc.hierarchyLevel == HierarchyLevels.SubBrick)
                .Select(subBrick => new HierarchyClassViewModel
                {
                    HierarchyClassId = subBrick.hierarchyClassID,
                    HierarchyClassName = subBrick.hierarchyClassName + "|" + subBrick.hierarchyClassID,
                    HierarchyLevel = subBrick.hierarchyLevel,
                    HierarchyId = subBrick.hierarchyID,
                    HierarchyParentClassId = subBrick.hierarchyParentClassID
                })
                .OrderBy(vm => vm.HierarchyClassLineage)
                .ToList();

            viewModel.ProductSelectionGroupTypes = new SelectList(productSelectionGroupTypes, "ProductSelectionGroupTypeId", "ProductSelectionGroupTypeName");
            viewModel.Traits = new SelectList(traits, "traitID", "traitDesc");
            viewModel.MerchandiseHierarchyClasses = new SelectList(merchandiseSubBricks, "hierarchyClassID", "HierarchyClassName");

            return viewModel;
        }

        [HttpPost]
        public ActionResult SaveChangesInGrid()
        {
            ViewData["GenerateCompactJSONResponse"] = false;

            List<Transaction<ProductSelectionGroupViewModel>> transactions = new GridModel()
                .LoadTransactions<ProductSelectionGroupViewModel>(HttpContext.Request.Form["ig_transactions"]);

            if (!transactions.Any())
            {
                return Json(new { Success = false, Error = "No new values were specified." });
            }

            ProductSelectionGroupViewModel psg = transactions.First().row;

            // Custom Model Validation
            string errorMessage = CheckModelValidation(psg);
            if (!String.IsNullOrEmpty(errorMessage))
            {
                return Json(new { Success = false, Error = errorMessage });
            }

            UpdateProductSelectionGroupManager manager = new UpdateProductSelectionGroupManager
            {
                ProductSelectionGroupId = psg.ProductSelectionGroupId,
                ProductSelectionGroupName = psg.ProductSelectionGroupName,
                ProductSelectionGroupTypeId = psg.ProductSelectionGroupTypeId,
                TraitValue = String.IsNullOrEmpty(psg.TraitValue) ? null : psg.TraitValue,
                TraitId = psg.TraitId,
                MerchandiseHierarchyClassId = psg.MerchandiseHierarchyClassId
            };

            try
            {
                updateProductSelectionGroupManagerHandler.Execute(manager);
            }
            catch (Exception ex)
            {
                var exceptionLogger = new ExceptionLogger(logger);
                exceptionLogger.LogException(ex, this.GetType(), MethodBase.GetCurrentMethod());

                return Json(new { Success = false, Error = ex.Message });
            }

            return Json(new { Success = true });
        }

        private string CheckModelValidation(ProductSelectionGroupCreateViewModel viewModel)
        {
            string errorMessage = String.Empty;

            // A PSG cannot have a Selected Trait with TraiValue combo and a MerchandiseHierarchyClassId
            if (viewModel.SelectedMerchandiseHierarchyClassId.HasValue && (viewModel.SelectedTraitId.HasValue || !String.IsNullOrEmpty(viewModel.TraitValue)))
            {
                errorMessage = "There cannot be a Merchandise Sub Brick assigned to the PSG if there is a Trait and Trait Value assigned.";
            }

            if (viewModel.SelectedMerchandiseHierarchyClassId.HasValue)
            {
                bool psgToMerchandiseAssociationAlreadyExists = getProductSelectionGroupsQueryHandler.Search(new GetProductSelectionGroupsParameters())
                    .Any(psg => psg.MerchandiseHierarchyClassId != null
                        && psg.MerchandiseHierarchyClassId.Value == viewModel.SelectedMerchandiseHierarchyClassId.Value);

                if (psgToMerchandiseAssociationAlreadyExists)
                {
                    errorMessage = "A PSG is already associated to the selected Merchandise Hierarchy Class.";
                }
            }
            else
            {
                // A Trait and TraitValue need to jointly have values, e.g. when one has a value, the other must also have a value
                if (viewModel.SelectedTraitId.HasValue && String.IsNullOrEmpty(viewModel.TraitValue))
                {
                    errorMessage = "The Trait Value must have a value for the selected Trait.";
                }

                if (!viewModel.SelectedTraitId.HasValue && !String.IsNullOrEmpty(viewModel.TraitValue))
                {
                    errorMessage = "A Trait must be selected for the specific Trait Value.";
                }
            }

            return errorMessage;
        }

        private string CheckModelValidation(ProductSelectionGroupViewModel viewModel)
        {
            string errorMessage = String.Empty;

            // A PSG cannot have a Selected Trait with TraiValue combo and a MerchandiseHierarchyClassId
            if (viewModel.MerchandiseHierarchyClassId.HasValue && (viewModel.TraitId.HasValue || !String.IsNullOrEmpty(viewModel.TraitValue)))
            {
                errorMessage = "There cannot be a Merchandise Sub Brick assigned to the PSG if there is a Trait and Trait Value assigned.";
            }

            if (viewModel.MerchandiseHierarchyClassId.HasValue)
            {
                var psgExistsWithSameMerch = getProductSelectionGroupsQueryHandler.Search(new GetProductSelectionGroupsParameters())
                    .Any(psg => psg.MerchandiseHierarchyClassId != null
                        && psg.ProductSelectionGroupId != viewModel.ProductSelectionGroupId
                        && psg.MerchandiseHierarchyClassId.Value == viewModel.MerchandiseHierarchyClassId.Value);
                if (psgExistsWithSameMerch)
                {
                    errorMessage = "A PSG is already associated to the selected Merchandise Hierarchy Class.";
                }
            }
            else
            {
                // A Trait and TraitValue need to jointly have values, e.g. when one has a value, the other must also have a value
                if (viewModel.TraitId.HasValue && String.IsNullOrEmpty(viewModel.TraitValue))
                {
                    errorMessage = "The Trait Value must have a value for the selected Trait.";
                }

                if (!viewModel.TraitId.HasValue && !String.IsNullOrEmpty(viewModel.TraitValue))
                {
                    errorMessage = "A Trait must be selected for the specific Trait Value.";
                }
            }

            return errorMessage;
        }
    }
}