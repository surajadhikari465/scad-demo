using AutoMapper;
using Icon.Common.DataAccess;
using Icon.Logging;
using Icon.Web.Common;
using Icon.Web.DataAccess.Commands;
using Icon.Web.DataAccess.Models;
using Icon.Web.Mvc.Models;
using Infragistics.Web.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Reflection;
using System.Web.Mvc;

namespace Icon.Web.Mvc.Controllers
{
    /// <summary>
    /// FeatureFlag Controller
    /// </summary>
    public class FeatureFlagController : Controller
    {
        private IQueryHandler<EmptyQueryParameters<IEnumerable<FeatureFlagModel>>,IEnumerable<FeatureFlagModel>> getFeatureFlagsQuery;
        private ICommandHandler<AddFeatureFlagCommand> addFeatureFlagCommandHandler;
        private ICommandHandler<UpdateFeatureFlagCommand> updateFeatureFlagCommandHandler;
        private IMapper mapper;
        private ILogger logger;

        /// <summary>
        /// Intialize an instance of FeatureFlagController
        /// </summary>
        public FeatureFlagController(
            IQueryHandler<EmptyQueryParameters<IEnumerable<FeatureFlagModel>>,IEnumerable<FeatureFlagModel>> getFeatureFlagsQuery,
            ICommandHandler<AddFeatureFlagCommand> addFeatureFlagCommandHandler,
            ICommandHandler<UpdateFeatureFlagCommand> updateFeatureFlagCommandHandler,
            IMapper mapper,
            ILogger logger)
        {
            this.getFeatureFlagsQuery = getFeatureFlagsQuery;
            this.mapper = mapper;
            this.addFeatureFlagCommandHandler = addFeatureFlagCommandHandler;
            this.updateFeatureFlagCommandHandler = updateFeatureFlagCommandHandler;
            this.logger = logger;
        }

        /// <summary>
        /// GET: FeatureFlag
        /// </summary>
        /// <returns>View</returns>
        public ActionResult Index()
        {
            FeatureFlagGridViewModel model = new FeatureFlagGridViewModel();
            model.FeatureFlags = getFeatureFlagsQuery
                                    .Search(null)
                                    .Select(ff => mapper.Map<FeatureFlagViewModel>(ff));
            return View(model);
        }

        [HttpPost]
        // Processes the Update for each row in the Infragistics igGrid
        public ActionResult SaveChangesInGrid(string ig_transactions)
        {
            if(ig_transactions == null)
            {
                logger.Error("Invalid Input: ig_transactions is null");
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest, "ig_transactions is null");
            }

            ViewData["GenerateCompactJSONResponse"] = false;

            GridModel gridModel = new GridModel();
            List<Transaction<FeatureFlagViewModel>> transactions = gridModel.LoadTransactions<FeatureFlagViewModel>(ig_transactions);

            if (transactions == null)
            {
                logger.Error("Invalid Input: transactions is null");
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest, "transactions is null");
            }

            JsonResult result = new JsonResult();
            Dictionary<string, bool> response = new Dictionary<string, bool>();

            foreach (var featureFlag in transactions)
            {
                if (featureFlag.row == null)
                {
                    logger.Error("Invalid Input: featureFlag.row is null");
                    return new HttpStatusCodeResult(HttpStatusCode.BadRequest, "featureFlag.row is null");
                }

                if (featureFlag.type == "newrow")
                {
                    try
                    {
                        var addFeatureFlagCommand = new AddFeatureFlagCommand
                        {
                            FeatureFlag = mapper.Map<FeatureFlagModel>(featureFlag.row)
                        };
                        addFeatureFlagCommand.FeatureFlag.LastModifiedBy = this.User.Identity.Name;
                        this.addFeatureFlagCommandHandler.Execute(addFeatureFlagCommand);
                        response.Add("Success", true);
                    }
                    catch (Exception exception)
                    {
                        var exceptionLogger = new ExceptionLogger(logger);
                        exceptionLogger.LogException(exception, this.GetType(), MethodBase.GetCurrentMethod());
                        response.Add("Success", false);
                    }
                }
                else
                {
                    try
                    {
                        var updateFeatureFlagCommand = new UpdateFeatureFlagCommand
                        {
                            FeatureFlag = mapper.Map<FeatureFlagModel>(featureFlag.row)
                        };
                        updateFeatureFlagCommand.FeatureFlag.LastModifiedBy = this.User.Identity.Name;
                        this.updateFeatureFlagCommandHandler.Execute(updateFeatureFlagCommand);
                        response.Add("Success", true);
                    }
                    catch (Exception exception)
                    {
                        var exceptionLogger = new ExceptionLogger(logger);
                        exceptionLogger.LogException(exception, this.GetType(), MethodBase.GetCurrentMethod());
                        response.Add("Success", false);
                    }

                }
            }

            result.Data = response;
            return result;
        }

    }
}
