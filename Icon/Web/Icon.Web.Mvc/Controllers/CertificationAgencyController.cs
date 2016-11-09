using Icon.Common.DataAccess;
using Icon.Framework;
using Icon.Logging;
using Icon.Web.Common;
using Icon.Web.DataAccess.Infrastructure;
using Icon.Web.DataAccess.Managers;
using Icon.Web.DataAccess.Models;
using Icon.Web.DataAccess.Queries;
using Icon.Web.Extensions;
using Icon.Web.Mvc.Attributes;
using Icon.Web.Mvc.Models;
using Infragistics.Web.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Web.Mvc;

namespace Icon.Web.Mvc.Controllers
{
    public class CertificationAgencyController : Controller
    {
        private ILogger logger;
        private IQueryHandler<GetCertificationAgenciesParameters, List<CertificationAgencyModel>> getCertificationAgenciesQuery;
        private IQueryHandler<GetHierarchyClassByIdParameters, HierarchyClass> getHierarchyClassQuery;
        private IManagerHandler<AddCertificationAgencyManager> addCertificationAgencyManagerHandler;
        private IManagerHandler<UpdateCertificationAgencyManager> updateCertificationAgencyManagerHandler;
        private IManagerHandler<DeleteHierarchyClassManager> deleteHierarchyClass;
        //private IExcelExporterService excelExporterService;

        public CertificationAgencyController(
            ILogger logger,
            IQueryHandler<GetCertificationAgenciesParameters, List<CertificationAgencyModel>> getCertificationAgenciesQuery,
            IQueryHandler<GetHierarchyClassByIdParameters, HierarchyClass> getHierarchyClassQuery,
            IManagerHandler<AddCertificationAgencyManager> addCertificationAgencyManagerHandler,
            IManagerHandler<UpdateCertificationAgencyManager> updateCertificationAgencyManagerHandler,
            IManagerHandler<DeleteHierarchyClassManager> deleteHierarchyClass)
        {
            this.logger = logger;
            this.getCertificationAgenciesQuery = getCertificationAgenciesQuery;
            this.getHierarchyClassQuery = getHierarchyClassQuery;
            this.addCertificationAgencyManagerHandler = addCertificationAgencyManagerHandler;
            this.updateCertificationAgencyManagerHandler = updateCertificationAgencyManagerHandler;
            this.deleteHierarchyClass = deleteHierarchyClass;
            //this.excelExporterService = excelExporterService;
        }

        // GET: CertificationAgency
        public ActionResult Index()
        {
            return View();
        }

        // GET: CertificationAgency/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: CertificationAgency/Create
        [HttpPost]
        [WriteAccessAuthorize]
        public ActionResult Create(CertificationAgencyViewModel viewModel)
        {
            // Ignore model state errors for this property until all hierarchy logic has been separated and this property can be safely removed.
            if (ModelState.ContainsKey("HierarchyClassName"))
            {
                ModelState["HierarchyClassName"].Errors.Clear();
            }

            if (!ModelState.IsValid)
            {
                return View(viewModel);
            }

            string newAgencyName = viewModel.AgencyName.Trim();
            //string newBrandAbbreviation = String.IsNullOrEmpty(viewModel.BrandAbbreviation) ? null : viewModel.BrandAbbreviation.Trim();
            bool newGlutenFree = viewModel.GlutenFree;
 
            var newAgency = new HierarchyClass
            {
                hierarchyClassName = newAgencyName
            };

            var manager = new AddCertificationAgencyManager
            {
                Agency = newAgency,
                GlutenFree = Convert.ToInt16(viewModel.GlutenFree).ToString(),
                Kosher = Convert.ToInt16(viewModel.Kosher).ToString(),
                NonGMO = Convert.ToInt16(viewModel.NonGMO).ToString(),
                Organic = Convert.ToInt16(viewModel.Organic).ToString(),
                Vegan = Convert.ToInt16(viewModel.Vegan).ToString()
            };

            try
            {
                addCertificationAgencyManagerHandler.Execute(manager);

                string successMessage = String.Format("Successfully added certification agency {0}.", newAgencyName);

                ViewData["SuccessMessage"] = successMessage;

                ModelState.Clear();
                return View();
            }
            catch (CommandException ex)
            {
                var exceptionLogger = new ExceptionLogger(logger);
                exceptionLogger.LogException(ex, this.GetType(), MethodBase.GetCurrentMethod());
                ViewData["ErrorMessage"] = ex.Message;

                return View();
            }
        }

        // GET: CertificationAgency/Edit/5
        public ActionResult Edit(int hierarchyClassId)
        {
            var viewModel = BuildCertificationAgencyViewModel(hierarchyClassId);

            return View(viewModel);
        }

        // POST: CertificationAgency/Edit/
        [HttpPost]
        [WriteAccessAuthorize]
        public ActionResult Edit(CertificationAgencyViewModel viewModel)
        {
            // Ignore model state errors for this property until all hierarchy logic has been separated and this property can be safely removed.
            if (ModelState.ContainsKey("HierarchyClassName"))
            {
                ModelState["HierarchyClassName"].Errors.Clear();
            }

            if (!ModelState.IsValid)
            {
                return View(viewModel);
            }

            string updatedAgencyName = viewModel.AgencyName.Trim();
            string updatedGlutenFree = Convert.ToString(Convert.ToInt16(viewModel.GlutenFree));
            string updatedKosher = Convert.ToString(Convert.ToInt16(viewModel.Kosher));
            string updatedNonGMO = Convert.ToString(Convert.ToInt16(viewModel.NonGMO));
            string updatedOrganic = Convert.ToString(Convert.ToInt16(viewModel.Organic));
            string updatedVegan = Convert.ToString(Convert.ToInt16(viewModel.Vegan));
            string defaultOrganic = Convert.ToString(Convert.ToInt16(viewModel.DefaultOrganic));

            var updatedAgency = new HierarchyClass
            {
                hierarchyID = viewModel.HierarchyId,
                hierarchyClassID = viewModel.HierarchyClassId,
                hierarchyClassName = updatedAgencyName,
                hierarchyLevel = viewModel.HierarchyLevel
            };

            var manager = new UpdateCertificationAgencyManager
            {
                Agency = updatedAgency,
                GlutenFree = updatedGlutenFree,
                Kosher = updatedKosher,
                NonGMO = updatedNonGMO,
                Organic = updatedOrganic,
                Vegan = updatedVegan,
                DefaultOrganic = defaultOrganic
            };

            try
            {
                updateCertificationAgencyManagerHandler.Execute(manager);
                ViewData["SuccessMessage"] = "Agency update was successful.";
            }
            catch (CommandException ex)
            {
                var exceptionLogger = new ExceptionLogger(logger);
                exceptionLogger.LogException(ex, this.GetType(), MethodBase.GetCurrentMethod());
                ViewData["ErrorMessage"] = ex.Message;
            }

            return View(viewModel);
        }

        // GET: /CertificationAgency/Delete/{HierarchyClassID}
        [HttpGet]
        public ActionResult Delete(int hierarchyClassId)
        {
            if (hierarchyClassId < 1)
            {
                return RedirectToAction("Index");
            }

            GetHierarchyClassByIdParameters hierarchyClassParameters = new GetHierarchyClassByIdParameters { HierarchyClassId = hierarchyClassId };
            HierarchyClass hierarchyClass = getHierarchyClassQuery.Search(hierarchyClassParameters);

            if (HierarchyClassAccessor.IsDefaultCertificationAgency(hierarchyClass))
            {
                return RedirectToAction("Index");
            }

            CertificationAgencyViewModel viewModel = new CertificationAgencyViewModel()
            {
                HierarchyClassId = hierarchyClass.hierarchyClassID,
                AgencyName = hierarchyClass.hierarchyClassName,
                GlutenFree = String.IsNullOrEmpty(HierarchyClassAccessor.GetGlutenFreeTrait(hierarchyClass)) ? false : true,
                Kosher = String.IsNullOrEmpty(HierarchyClassAccessor.GetKosherTrait(hierarchyClass)) ? false : true,
                NonGMO = String.IsNullOrEmpty(HierarchyClassAccessor.GetNonGMOTrait(hierarchyClass)) ? false : true,
                Organic = String.IsNullOrEmpty(HierarchyClassAccessor.GetOrganicTrait(hierarchyClass)) ? false : true,
                Vegan = String.IsNullOrEmpty(HierarchyClassAccessor.GetVeganTrait(hierarchyClass)) ? false : true
            };

            return View(viewModel);
        }

        // POST: /CertificationAgency/Delete/
        [HttpPost]
        [ValidateAntiForgeryToken]
        [WriteAccessAuthorize]
        public ActionResult Delete(CertificationAgencyViewModel viewModel)
        {
            GetHierarchyClassByIdParameters hierarchyClassParameters = new GetHierarchyClassByIdParameters { HierarchyClassId = viewModel.HierarchyClassId };
            HierarchyClass deletedHierarchyClass = getHierarchyClassQuery.Search(hierarchyClassParameters);

            // Make sure the node does not have any children.
            if (deletedHierarchyClass.ItemSignAttribute.Count > 0 || deletedHierarchyClass.ItemSignAttribute1.Count > 0 || deletedHierarchyClass.ItemSignAttribute2.Count > 0 ||
                deletedHierarchyClass.ItemSignAttribute3.Count > 0 || deletedHierarchyClass.ItemSignAttribute4.Count > 0)
            {
                viewModel = BuildCertificationAgencyViewModel(deletedHierarchyClass.hierarchyClassID);
                ViewData["ErrorMessage"] = "Error: This hierarchy class is assigned as at least one item's certification agency, so it cannot be deleted.";

                ModelState.Clear();
                return View(viewModel);
            }

            var command = new DeleteHierarchyClassManager
            {
                DeletedHierarchyClass = deletedHierarchyClass
            };

            try
            {
                deleteHierarchyClass.Execute(command);
            }
            catch (CommandException exception)
            {
                var exceptionLogger = new ExceptionLogger(logger);
                exceptionLogger.LogException(exception, this.GetType(), MethodBase.GetCurrentMethod());
                ViewData["ErrorMessage"] = "Error: There was a problem with applying this delete on the database.";
                viewModel = BuildCertificationAgencyViewModel(deletedHierarchyClass.hierarchyClassID);
                ModelState.Clear();

                return View(viewModel);
            }

            return RedirectToAction("Index");
        }

        [GridDataSourceAction]
        public ActionResult All()
        {
            List<CertificationAgencyModel> agencies = getCertificationAgenciesQuery.Search(new GetCertificationAgenciesParameters());
            var viewModels = agencies.ToViewModels().AsQueryable();
            return View(viewModels);
        }

        //public void Export()
        //{
        //    List<CertificationAgencyViewModel> agencies = getCertificationAgenciesQuery.Search(new GetCertificationAgenciesParameters())
        //        .ToViewModels()
        //        .Select(b => new BrandExportViewModel 
        //        {
        //            BrandId = b.HierarchyClassId.ToString(),
        //            BrandName = b.BrandName,
        //            BrandAbbreviation = b.BrandAbbreviation
        //        })
        //        .ToList();

        //    var brandExporter = excelExporterService.GetBrandExporter();
        //    brandExporter.ExportData = agencies;
        //    brandExporter.Export();

        //    ExcelHelper.SendForDownload(Response, brandExporter.ExportModel.ExcelWorkbook, brandExporter.ExportModel.ExcelWorkbook.CurrentFormat, "Brand");
        //}

        private CertificationAgencyViewModel BuildCertificationAgencyViewModel(int hierarchyClassId)
        {
            var parameters = new GetHierarchyClassByIdParameters
            {
                HierarchyClassId = hierarchyClassId
            };

            var agency = getHierarchyClassQuery.Search(parameters);

            string agencyName = agency.hierarchyClassName;
            bool glutenFree = false;
            bool kosher = false;
            bool nonGMO = false;
            bool organic = false;
            bool vegan = false;
            bool defaultOrganic = false;

            var glutenFreeTrait = agency.HierarchyClassTrait.SingleOrDefault(hct => hct.traitID == Traits.GlutenFree);
            var kosherTrait = agency.HierarchyClassTrait.SingleOrDefault(hct => hct.traitID == Traits.Kosher);
            var nonGMOTrait = agency.HierarchyClassTrait.SingleOrDefault(hct => hct.traitID == Traits.NonGmo);
            var organicTrait = agency.HierarchyClassTrait.SingleOrDefault(hct => hct.traitID == Traits.Organic);
            var veganTrait = agency.HierarchyClassTrait.SingleOrDefault(hct => hct.traitID == Traits.Vegan);
            var defaultOrganicTrait = agency.HierarchyClassTrait.SingleOrDefault(hct => hct.traitID == Traits.DefaultCertificationAgency && hct.traitValue == Constants.DefaultOrganicAgencyTraitValue);

            if (glutenFreeTrait != null)
            {
                glutenFree = Convert.ToBoolean(Convert.ToInt16(glutenFreeTrait.traitValue));
            }

            if (kosherTrait != null)
            {
                kosher = Convert.ToBoolean(Convert.ToInt16(kosherTrait.traitValue));
            }

            if (nonGMOTrait != null)
            {
                nonGMO = Convert.ToBoolean(Convert.ToInt16(nonGMOTrait.traitValue));
            }

            if (organicTrait != null)
            {
                organic = Convert.ToBoolean(Convert.ToInt16(organicTrait.traitValue));
            }

            if (veganTrait != null)
            {
                vegan = Convert.ToBoolean(Convert.ToInt16(veganTrait.traitValue));
            }

            if (defaultOrganicTrait != null)
            {
                defaultOrganic = true;
            }

            var viewModel = new CertificationAgencyViewModel
            {
                AgencyName = agency.hierarchyClassName,
                HierarchyId = agency.hierarchyID,
                HierarchyClassId = agency.hierarchyClassID,
                HierarchyLevel = HierarchyLevels.Parent,
                GlutenFree = glutenFree,
                Kosher = kosher,
                NonGMO = nonGMO,
                Organic = organic,
                Vegan = vegan,
                DefaultOrganic = defaultOrganic
            };

            return viewModel;
        }

        public JsonResult GetAnimalWelfareRatings()
        {
            return Json(AnimalWelfareRatings.AsDictionary.Select(kvp => new { id = kvp.Key, name = kvp.Value }).ToArray(), JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetMilkTypes()
        {
            return Json(MilkTypes.AsDictionary.Select(kvp => new { id = kvp.Key, name = kvp.Value }).ToArray(), JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetEcoScaleRatings()
        {
            return Json(EcoScaleRatings.AsDictionary.Select(kvp => new { id = kvp.Key, name = kvp.Value }).ToArray(), JsonRequestBehavior.AllowGet);
        }
                
        public JsonResult GetSeafoodFreshOrFrozenTypes()
        {
            return Json(SeafoodFreshOrFrozenTypes.AsDictionary.OrderBy(kvp => kvp.Key).Select(kvp => new { id = kvp.Key, name = kvp.Value }).ToArray(), JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetSeafoodCatchTypes()
        {
            return Json(SeafoodCatchTypes.AsDictionary.Select(kvp => new { id = kvp.Key, name = kvp.Value }).ToArray(), JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetGlutenFreeAgencies()
        {
            var agencies = getCertificationAgenciesQuery.Search(new GetCertificationAgenciesParameters())
                .Where(hc => hc.GlutenFree == "1")
                .Select(hc => new { id = hc.HierarchyClassId, name = hc.HierarchyClassName })
                .ToArray();
            return Json(agencies, JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetKosherAgencies()
        {
            var agencies = getCertificationAgenciesQuery.Search(new GetCertificationAgenciesParameters())
                .Where(hc => hc.Kosher == "1")
                .Select(hc => new { id = hc.HierarchyClassId, name = hc.HierarchyClassName })
                .ToArray();
            return Json(agencies, JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetNonGmoAgencies()
        {
            var agencies = getCertificationAgenciesQuery.Search(new GetCertificationAgenciesParameters())
                .Where(hc => hc.NonGMO == "1")
                .Select(hc => new { id = hc.HierarchyClassId, name = hc.HierarchyClassName })
                .ToArray();
            return Json(agencies, JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetOrganicAgencies()
        {
            var agencies = getCertificationAgenciesQuery.Search(new GetCertificationAgenciesParameters())
                .Where(hc => hc.Organic == "1")
                .Select(hc => new { id = hc.HierarchyClassId, name = hc.HierarchyClassName })
                .ToArray();
            return Json(agencies, JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetVeganAgencies()
        {
            var agencies = getCertificationAgenciesQuery.Search(new GetCertificationAgenciesParameters())
                .Where(hc => hc.Vegan == "1")
                .Select(hc => new { id = hc.HierarchyClassId, name = hc.HierarchyClassName })
                .ToArray();
            return Json(agencies, JsonRequestBehavior.AllowGet);
        }

        public JsonResult CertificationAgencyAutocomplete(int certificationAgencyTraitId, string term)
        {
            var certificationAgencies = getCertificationAgenciesQuery.Search(new GetCertificationAgenciesParameters())
                .Where(hc => hc.HierarchyClassName.ToLower().Contains(term.ToLower()));

            switch(certificationAgencyTraitId)
            {
                case Traits.GlutenFree: certificationAgencies = certificationAgencies.Where(hc => hc.GlutenFree == "1");
                    break;
                case Traits.Kosher: certificationAgencies = certificationAgencies.Where(hc => hc.Kosher == "1");
                    break;                    
                case Traits.Organic: certificationAgencies = certificationAgencies.Where(hc => hc.Organic == "1");
                    break;
                case Traits.NonGmo: certificationAgencies = certificationAgencies.Where(hc => hc.NonGMO == "1");
                    break;
                case Traits.Vegan: certificationAgencies = certificationAgencies.Where(hc => hc.Vegan == "1");
                    break;
            }

            return Json(certificationAgencies.Select(hc => new { value = hc.HierarchyClassName }).ToArray(), JsonRequestBehavior.AllowGet);
        }
    }
}