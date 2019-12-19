using DevTrends.MvcDonutCaching;
using Icon.Common.DataAccess;
using Icon.Framework;
using Icon.Logging;
using Icon.Web.Common;
using Icon.Web.DataAccess.Commands;
using Icon.Web.DataAccess.Models;
using Icon.Web.DataAccess.Queries;
using Icon.Web.Mvc.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Web.Mvc;

namespace Icon.Web.Controllers
{
    [RedirectFilterAttribute]
    public class PluMappingController : Controller
    {
        private ILogger logger;
        private IQueryHandler<GetPluMappingParameters, List<Item>> pluQuery;
        private IQueryHandler<GetItemByIdParameters, Item> getItemQuery;
        private IQueryHandler<GetPluRemappingsParameters, List<BulkImportPluRemapModel>> remapQuery;
        private ICommandHandler<UpdatePluCommand> updatePluCommand;

        public PluMappingController(
            ILogger logger,
            IQueryHandler<GetPluMappingParameters, List<Item>> pluQuery,
            IQueryHandler<GetItemByIdParameters, Item> getItemQuery,
            IQueryHandler<GetPluRemappingsParameters, List<BulkImportPluRemapModel>> remapQuery,
            ICommandHandler<UpdatePluCommand> updatePluCommand)
        {
            this.logger = logger;
            this.pluQuery = pluQuery;
            this.getItemQuery = getItemQuery;
            this.remapQuery = remapQuery;
            this.updatePluCommand = updatePluCommand;
        }

        //
        // GET: /PluMapping/
        public ActionResult Index()
        {
            return View(new PluSearchViewModel());
        }

        //
        // GET: /PluMapping/Search
        [DonutOutputCache(NoStore = true, Duration = 0, VaryByParam = "*")]
        public ActionResult Search(PluSearchViewModel viewModel)
        {
            if (!ModelState.IsValid)
            {
                Response.StatusCode = (int)System.Net.HttpStatusCode.BadRequest;
                return PartialView("_PluMapSearchOptionsPartial", viewModel);
            }

            GetPluMappingParameters searchParameters = new GetPluMappingParameters
            {
                NationalPlu = viewModel.NationalPlu,
                RegionalPlu = viewModel.RegionalPlu,
                BrandName = viewModel.BrandName,
                PluDescription = viewModel.PluDescription
            };

            List<Item> pluList = pluQuery.Search(searchParameters);

            // To make it easier to work in the View, project the Item objects to PluMappingModel objects.
            viewModel.PluMapping = pluList.Select(item => new PluMappingViewModel(item)).ToList();

            // Hold results in Session cache for Export
            Session["grid_search_results"] = viewModel.PluMapping;

            return PartialView("_PluMapSearchResultsPartial", viewModel);
        }

        //
        // GET: /PluMapping/Edit/5
        public ActionResult Edit(int id)
        {
            if (!ModelState.IsValid)
            {
                Response.StatusCode = (int)System.Net.HttpStatusCode.BadRequest;
                return RedirectToAction("Index");
            }

            GetItemByIdParameters parameter = new GetItemByIdParameters
            {
                ItemId = id
            };

            var item = getItemQuery.Search(parameter);
            PluMappingViewModel pluViewModel = new PluMappingViewModel(item);

            return View(pluViewModel);
        }

        //
        // POST: /PluMapping/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(PluMappingViewModel viewModel)
        {
            if (!ModelState.IsValid)
            {
                return View(viewModel);
            }

            viewModel = TrimLeadingZeros(viewModel);
            
            // Check if all regional PLU lengths match the type of the national PLU.
            string validateLengths = ValidatePluRowByNationalPluLength(viewModel);
            if (validateLengths != null)
            {
                string badLengthRegion = validateLengths.TrimEnd('P', 'L', 'U').ToUpper();
                ViewData["UpdateFailed"] = String.Format(
                    "All PLU updates failed: PLU {0} does not match the national PLU length or type.", badLengthRegion);
                return View(viewModel);
            }

            // Check if any PLUs are already mapped by utilizing PLU Bulk Import Remapping Query.
            List<BulkImportPluModel> remapList = new List<BulkImportPluModel>();
            BulkImportPluModel pluModel = new BulkImportPluModel
            {
                NationalPlu = viewModel.NationalPlu,
                flPLU = viewModel.flPLU,
                maPLU = viewModel.maPLU,
                mwPLU = viewModel.mwPLU,
                naPLU = viewModel.naPLU,
                ncPLU = viewModel.ncPLU,
                nePLU = viewModel.nePLU,
                pnPLU = viewModel.pnPLU,
                rmPLU = viewModel.rmPLU,
                soPLU = viewModel.soPLU,
                spPLU = viewModel.spPLU,
                swPLU = viewModel.swPLU,
                ukPLU = viewModel.ukPLU
            };

            remapList.Add(pluModel);
            GetPluRemappingsParameters remapParameters = new GetPluRemappingsParameters { ImportedItems = remapList };
            List<BulkImportPluRemapModel> alreadyMappedPlus = remapQuery.Search(remapParameters);

            if (alreadyMappedPlus.Count > 0 && alreadyMappedPlus[0].CurrentNationalPlu != alreadyMappedPlus[0].NewNationalPlu)
            {
                string failedRegion = alreadyMappedPlus[0].Region.TrimEnd('P', 'L', 'U').ToUpper();
                string mappedPlu = alreadyMappedPlus[0].RegionalPlu;
                string currentNationalPlu = alreadyMappedPlus[0].CurrentNationalPlu;

                ViewData["UpdateFailed"] = String.Format(
                    "All PLU updates failed: PLU {0} is already mapped to national PLU {1} for the {2} region.", mappedPlu, currentNationalPlu, failedRegion);
                
                return View(viewModel);
            }

            UpdatePluCommand command = new UpdatePluCommand
            {
                Plu = new PLUMap
                {
                    itemID = viewModel.ItemId,
                    flPLU = viewModel.flPLU,
                    maPLU = viewModel.maPLU,
                    mwPLU = viewModel.mwPLU,
                    naPLU = viewModel.naPLU,
                    ncPLU = viewModel.ncPLU,
                    nePLU = viewModel.nePLU,
                    pnPLU = viewModel.pnPLU,
                    rmPLU = viewModel.rmPLU,
                    soPLU = viewModel.soPLU,
                    spPLU = viewModel.spPLU,
                    swPLU = viewModel.swPLU,
                    ukPLU = viewModel.ukPLU
                }
            };

            try
            {
                updatePluCommand.Execute(command);
                ViewData["UpdateSuccess"] = "Update successful.";
                return View(viewModel);
            }
            catch (CommandException exception)
            {
                var exceptionLogger = new ExceptionLogger(logger);
                exceptionLogger.LogException(exception, this.GetType(), MethodBase.GetCurrentMethod());
                ViewData["UpdateFailed"] = "An unexpected error occurred, and no mappings have been saved.  Please try again.";
                return View(viewModel);
            }
        }

        /// <summary>
        /// Check to see if the length of the regional PLUs match the National PLU length
        /// </summary>
        /// <param name="plu"></param>
        /// <returns>The Property Name of the region that does not match lengths</returns>
        private string ValidatePluRowByNationalPluLength(PluMappingViewModel plu)
        {
            int nationalPluLength = plu.NationalPlu.Length;
            Type pluType = plu.GetType();
            PropertyInfo[] pluProperties = pluType.GetProperties();

            for (int i = 0; i < pluProperties.Count(); i++)
            {
                if (pluProperties[i].Name.Contains("PLU") && pluProperties[i].GetValue(plu) != null)
                {
                    if (nationalPluLength <= 6)
                    {
                        if (pluProperties[i].GetValue(plu).ToString().Length > 6)
                        {
                            return pluProperties[i].Name;
                        }
                    }

                    if (nationalPluLength == 11)
                    {
                        if (pluProperties[i].GetValue(plu).ToString().Length != 11)
                        {
                            return pluProperties[i].Name;
                        }
                    }
                }
            }

            return null;
        }

        private PluMappingViewModel TrimLeadingZeros(PluMappingViewModel viewModel)
        {
            viewModel.flPLU = String.IsNullOrEmpty(viewModel.flPLU) ? null : viewModel.flPLU.TrimStart('0');
            viewModel.maPLU = String.IsNullOrEmpty(viewModel.maPLU) ? null : viewModel.maPLU.TrimStart('0');
            viewModel.mwPLU = String.IsNullOrEmpty(viewModel.mwPLU) ? null : viewModel.mwPLU.TrimStart('0');
            viewModel.naPLU = String.IsNullOrEmpty(viewModel.naPLU) ? null : viewModel.naPLU.TrimStart('0');
            viewModel.ncPLU = String.IsNullOrEmpty(viewModel.ncPLU) ? null : viewModel.ncPLU.TrimStart('0');
            viewModel.nePLU = String.IsNullOrEmpty(viewModel.nePLU) ? null : viewModel.nePLU.TrimStart('0');
            viewModel.pnPLU = String.IsNullOrEmpty(viewModel.pnPLU) ? null : viewModel.pnPLU.TrimStart('0');
            viewModel.rmPLU = String.IsNullOrEmpty(viewModel.rmPLU) ? null : viewModel.rmPLU.TrimStart('0');
            viewModel.soPLU = String.IsNullOrEmpty(viewModel.soPLU) ? null : viewModel.soPLU.TrimStart('0');
            viewModel.spPLU = String.IsNullOrEmpty(viewModel.spPLU) ? null : viewModel.spPLU.TrimStart('0');
            viewModel.swPLU = String.IsNullOrEmpty(viewModel.swPLU) ? null : viewModel.swPLU.TrimStart('0');
            viewModel.ukPLU = String.IsNullOrEmpty(viewModel.ukPLU) ? null : viewModel.ukPLU.TrimStart('0');
            
            return viewModel;
        }
    }
}
