using Icon.Common.DataAccess;
using Icon.Framework;
using Icon.Logging;
using Icon.Web.Common;
using Icon.Web.DataAccess.Infrastructure;
using Icon.Web.DataAccess.Managers;
using Icon.Web.DataAccess.Queries;
using Icon.Web.Mvc.Attributes;
using Icon.Web.Mvc.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Web.Mvc;

namespace Icon.Web.Controllers
{
    public class VenueController : Controller
    {
        private ILogger logger;
        private IQueryHandler<GetLocaleParameters, List<Locale>> getLocaleQuery;
        private IQueryHandler<GetLocalesByChainParameters, List<Locale>> getLocalesByChainQuery;
        private IGenericQuery genericQuery;
        private IManagerHandler<AddVenueManager> addVenueManager;
        private List<LocaleSubType> localeSubTypes;

        public VenueController(
            ILogger logger,
            IQueryHandler<GetLocaleParameters, List<Locale>> getLocaleQuery,
            IQueryHandler<GetLocalesByChainParameters, List<Locale>> getLocalesByChainQuery,
            IGenericQuery genericQuery,
            IManagerHandler<AddVenueManager> addVenueManager)
        {
            this.logger = logger;
            this.getLocaleQuery = getLocaleQuery;
            this.getLocalesByChainQuery = getLocalesByChainQuery;
            this.genericQuery = genericQuery;
            this.addVenueManager = addVenueManager;
            this.localeSubTypes = genericQuery.GetAll<LocaleSubType>();
        }


        // GET: /Venue/CreateVenue
        [HttpGet]
        [WriteAccessAuthorize(GlobalDataTeamException = true)]
        public ActionResult CreateVenue(int? parentLocaleId, string parentLocaleName)
        {
            if ((parentLocaleId == null || parentLocaleId < 1) || String.IsNullOrEmpty(parentLocaleName))
            {
                return RedirectToAction("Index");
            }

            GetLocaleParameters parameters = new GetLocaleParameters { LocaleId = parentLocaleId.Value };
            Locale parentLocale = getLocaleQuery.Search(parameters).Single();

            VenueViewModel viewModel = new VenueViewModel
            {
                ParentLocaleId = parentLocale.localeID,
                ParentLocaleName = parentLocale.localeName,
                LocaleTypeId = LocaleTypes.Venue,
                OwnerOrgPartyId = parentLocale.ownerOrgPartyID
            };

            viewModel = PopulateViewModelDropDowns(viewModel);

            return View(viewModel);
        }

        // POST: /Venue/CreateVenue/
        [HttpPost]
        [WriteAccessAuthorize(GlobalDataTeamException = true)]
        [ValidateAntiForgeryToken]
        public ActionResult CreateVenue(VenueViewModel viewModel)
        {
            if (!ModelState.IsValid)
            {
                viewModel = PopulateViewModelDropDowns(viewModel);
                return View(viewModel);
            }
            if(viewModel.LocaleSubTypeId == 1
                && (viewModel.TouchPointGroupId ?? String.Empty).Trim().Length == 0)
            {
                    ModelState.AddModelError("TouchPointGroupId", "TouchPoint Group Id is Required for Hospitality subtype.");
                    viewModel = PopulateViewModelDropDowns(viewModel);
                    return View(viewModel);
            }

            AddVenueManager manager = new AddVenueManager
            {
                LocaleName = viewModel.LocaleName,
                ParentLocaleId = viewModel.ParentLocaleId,
                OpenDate = viewModel.OpenDate,
                VenueCode = viewModel.VenueCode,
                VenueOccupant = viewModel.VenueOccupant,
                LocaleSubTypeId = viewModel.LocaleSubTypeId,
                LocaleSubType = localeSubTypes.Where(ls => ls.localeSubTypeID == viewModel.LocaleSubTypeId).Select(s => s.localeSubTypeDesc).First(),
                UserName = User.Identity.Name,
                LocaleTypeId = LocaleTypes.Venue,
                OwnerOrgPartyId = viewModel.OwnerOrgPartyId,
                TouchPointGroupId = viewModel.TouchPointGroupId,
            };

            try
            {
                addVenueManager.Execute(manager);
            }
            catch (CommandException ex)
            {
                var exceptionLogger = new ExceptionLogger(logger);
                exceptionLogger.LogException(ex, this.GetType(), MethodBase.GetCurrentMethod());

                TempData["ErrorMessage"] = ex.Message;
                viewModel = PopulateViewModelDropDowns(viewModel);

                return View(viewModel);
            }

            TempData["SuccessMessage"] = "Venue created successfully.";

            return RedirectToAction("CreateVenue", new { parentLocaleId = viewModel.ParentLocaleId, parentLocaleName = viewModel.ParentLocaleName });
        }

        private VenueViewModel PopulateViewModelDropDowns(VenueViewModel viewModel)
        {
            viewModel.LocaleSubTypes = this.localeSubTypes.Select(c => new SelectListItem { Value = c.localeSubTypeID.ToString(), Text = c.localeSubTypeDesc });

            return viewModel;
        }
    }
}