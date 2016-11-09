namespace WebSupport.Controllers
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Web.Mvc;

    using Icon.Common.DataAccess;
    using Icon.Logging;

    using WebSupport.DataAccess.Commands;
    using WebSupport.ViewModels;
    using DataAccess.Queries;

    public class EventCreatorController : Controller
    {
        #region Constants

        private const string CommandSuccessMessage = "Item events have been created successfully for reprocessing.";
        private const string FailedMessage = "An error has occured, please check the logs.";
        private const string InvalidScanCodesMessage = "The following scan codes do not exist: ";

        #endregion

        #region Fields

        private ILogger logger;
        private ICommandHandler<CreateEventsForRegionCommand> commandHandler;
        private IQueryHandler<GetExistingScanCodesParameters, List<string>> searchHandler;

        #endregion

        #region Ctors

        public EventCreatorController(
            ILogger logger, 
            ICommandHandler<CreateEventsForRegionCommand> commandHandler,
            IQueryHandler<GetExistingScanCodesParameters, List<string>> searchHandler)
        {
            this.logger = logger;
            this.commandHandler = commandHandler;
            this.searchHandler = searchHandler;
        }

        #endregion

        #region Public Methods

        // GET: EventCreator
        public ActionResult Index()
        {
            return View(new EventCreatorViewModel());
        }

        public ActionResult CreateEvents(EventCreatorViewModel viewModel)
        {
            if(!ModelState.IsValid)
            {
                return View("Index", viewModel);
            }

            var areScanCodesValid = this.ValidateScanCodesExist(viewModel);

            if(areScanCodesValid)
            {
                this.ExecuteCreateEvents(viewModel);
            }

            return View("Index", viewModel);
        }

        #endregion

        #region Private Methods

        private bool ValidateScanCodesExist(EventCreatorViewModel viewModel)
        {
            var queryResultMessage = string.Empty;
            bool areScanCodesValid = false;
            var scanCodes = viewModel.GetScanCodes().ToList();

            try
            {
                var queryResult = this.searchHandler.Search(new GetExistingScanCodesParameters
                {
                    Region = viewModel.SelectedRegion,
                    ScanCodes = scanCodes
                });

                areScanCodesValid = !queryResult.Any();
                if(!areScanCodesValid)
                {
                    queryResultMessage = InvalidScanCodesMessage + string.Join(", ", queryResult);
                }
            }
            catch(Exception e)
            {
                queryResultMessage = FailedMessage;
                this.logger.Error(e.ToString());
            }
            finally
            {
                ViewBag.CommandResultMessage = queryResultMessage;
            }

            if (!areScanCodesValid)
                ViewBag.Error = true;

            return areScanCodesValid;
        }

        private void ExecuteCreateEvents(EventCreatorViewModel viewModel)
        {
            var scanCodes = viewModel.GetScanCodes();
            var commandResult = CommandSuccessMessage;

            try
            {
                this.commandHandler.Execute(new CreateEventsForRegionCommand
                {
                    EventType = viewModel.SelectedEventType,
                    Region = viewModel.SelectedRegion,
                    ScanCodes = scanCodes
                });
            }
            catch (Exception e)
            {
                commandResult = FailedMessage;
                this.logger.Error(e.ToString());
                ViewBag.Error = true;
            }
            finally
            {
                ViewBag.CommandResultMessage = commandResult;
            }
        }

        #endregion
    }
}