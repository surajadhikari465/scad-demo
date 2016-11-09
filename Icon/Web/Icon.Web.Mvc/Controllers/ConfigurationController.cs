using Icon.Common.DataAccess;
using Icon.Framework;
using Icon.Logging;
using Icon.Web.Common;
using Icon.Web.DataAccess.Commands;
using Icon.Web.DataAccess.Infrastructure;
using Icon.Web.DataAccess.Models;
using Icon.Web.DataAccess.Queries;
using Icon.Web.Extensions;
using Icon.Web.Mvc.Models;
using Infragistics.Web.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Web.Mvc;

namespace Icon.Web.Mvc.Controllers
{
    public class ConfigurationController : Controller
    {
        private ILogger logger;
        private IQueryHandler<GetRegionalSettingsByRegionParameters, List<RegionalSettingsModel>> getRegionalSettingsQuery;
        private ICommandHandler<UpdateRegionalSettingsCommand> updateRegionalSettingHandler;
        private IQueryHandler<GetRegionsParameters, List<Regions>> getRegionsQuery;

        public ConfigurationController(
            ILogger logger,
            IQueryHandler<GetRegionalSettingsByRegionParameters, List<RegionalSettingsModel>> getRegionalSettingsQuery,
            ICommandHandler<UpdateRegionalSettingsCommand> updateRegionalSettingHandler,
            IQueryHandler<GetRegionsParameters, List<Regions>> getRegionsQuery)
        {
            this.logger = logger;
            this.getRegionalSettingsQuery = getRegionalSettingsQuery;
            this.updateRegionalSettingHandler = updateRegionalSettingHandler;
            this.getRegionsQuery = getRegionsQuery;
        }

        //
        // GET: /Configuration/
        public ActionResult Index()
        {
            var viewModel = new ConfigurationSearchViewModel
            {
                RegionList = GetRegionListSelectList(String.Empty)
            };

            return View(viewModel);
        }

        //
        // GET: /Configuration/Search/
        [OutputCache(NoStore = true, Duration = 0, VaryByParam = "*")]
        public PartialViewResult Search(ConfigurationSearchViewModel viewModel)
        {
            if (!ModelState.IsValid)
            {
                Response.StatusCode = (int)System.Net.HttpStatusCode.BadRequest;
                viewModel.RegionList = GetRegionListSelectList(String.Empty);
                return PartialView("_ConfigurationSearchOptionsPartial", viewModel);
            }

            List<RegionalSettingsModel> regionalSettingsListModel = getRegionalSettingsQuery.Search(new GetRegionalSettingsByRegionParameters() { RegionCode = viewModel.SelectedRegionCode });
            viewModel.RegionItemConfigurationList = GetRegionalSettingsforSection(regionalSettingsListModel, "Item");
            viewModel.RegionFinancialSubTeamConfigurationList = GetRegionalSettingsforSection(regionalSettingsListModel, "Financial");

            viewModel.RegionList = GetRegionListSelectList(String.Empty);
            return PartialView("_ConfigurationSearchResultsPartial", viewModel);
        }

        // Processes the Update for each row in the Infragistics igGrid
        public ActionResult SaveChangesInGrid()
        {
            ViewData["GenerateCompactJSONResponse"] = false;

            GridModel gridModel = new GridModel();
            List<Transaction<ConfigurationViewModel>> transactions = gridModel.LoadTransactions<ConfigurationViewModel>(HttpContext.Request.Form["ig_transactions"]);

            JsonResult result = new JsonResult();
            Dictionary<string, bool> response = new Dictionary<string, bool>();

            foreach (Transaction<ConfigurationViewModel> item in transactions)
            {
                var command = new UpdateRegionalSettingsCommand
                {
                    SettingValue = item.row.Value,
                    RegionalSettingId = item.row.RegionalSettingsId
                };
                
                try
                {
                    updateRegionalSettingHandler.Execute(command);
                }
                catch (CommandException exception)
                {
                    var exceptionLogger = new ExceptionLogger(logger);
                    exceptionLogger.LogException(exception, this.GetType(), MethodBase.GetCurrentMethod());
                    response.Add("Success", false);
                    result.Data = response;
                    return result;
                }
            }

            response.Add("Success", true);
            result.Data = response;
            return result;
        }

        private List<ConfigurationViewModel> GetRegionalSettingsforSection(List<RegionalSettingsModel> regionalSettingsList, string sectionName)
        {
            List<ConfigurationViewModel> regionalSettingsListClasses = regionalSettingsList.Where(rs => rs.SectionName.ToLower().Equals(sectionName.ToLower())).ToList().ToConfigurationViewModel();

            return regionalSettingsListClasses;
        }

        private SelectList GetRegionListSelectList(string selectedRegion)
        {
            var regions = GetRegionList().OrderBy(r => r);

            return new SelectList(regions, selectedRegion);
        }

        private List<string> GetRegionList()
        {
            return getRegionsQuery.Search(new GetRegionsParameters()).Select(r => r.RegionCode).ToList();
        }
    }
}