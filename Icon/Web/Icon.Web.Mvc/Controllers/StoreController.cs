using Icon.Common.DataAccess;
using Icon.Framework;
using Icon.Logging;
using Icon.Web.Common;
using Icon.Web.DataAccess.Infrastructure;
using Icon.Web.DataAccess.Managers;
using Icon.Web.DataAccess.Models;
using Icon.Web.DataAccess.Queries;
using Icon.Web.Mvc.Attributes;
using Icon.Web.Mvc.Models;
using Infragistics.Web.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Reflection;
using System.Web;
using System.Web.Mvc;

namespace Icon.Web.Controllers
{

    public class StoreController : Controller
    {
        private ILogger logger;
        private IQueryHandler<GetLocaleParameters, List<Locale>> getLocaleQuery;
        private IQueryHandler<GetLocalesByChainParameters, List<Locale>> getLocalesByChainQuery;
        private IGenericQuery genericQuery;
        private IManagerHandler<UpdateLocaleManager> updateLocaleManager;
        private IManagerHandler<UpdateVenueManager> updateVenueManager;
        private IManagerHandler<AddLocaleManager> addLocaleManager;
        private List<Territory> territories;
        private List<Country> countries;
        private List<Timezone> timeZones;
        private List<Agency> eWicAgencies;
        private List<Currency> currencies;
        private List<LocaleSubType> localeSubTypes;

        public StoreController(
            ILogger logger,
            IQueryHandler<GetLocaleParameters, List<Locale>> getLocaleQuery,
            IQueryHandler<GetLocalesByChainParameters, List<Locale>> getLocalesByChainQuery,
            IGenericQuery genericQuery,
            IManagerHandler<UpdateLocaleManager> updateLocaleCommand,
            IManagerHandler<UpdateVenueManager> updateVenueManager,
            IManagerHandler<AddLocaleManager> addLocaleCommand)
        {
            this.logger = logger;
            this.getLocaleQuery = getLocaleQuery;
            this.getLocalesByChainQuery = getLocalesByChainQuery;
            this.genericQuery = genericQuery;
            this.updateLocaleManager = updateLocaleCommand;
            this.updateVenueManager = updateVenueManager;
            this.addLocaleManager = addLocaleCommand;

            this.territories = genericQuery.GetAll<Territory>();
            this.countries = genericQuery.GetAll<Country>();
            this.timeZones = genericQuery.GetAll<Timezone>();
            this.eWicAgencies = genericQuery.GetAll<Agency>();
            this.currencies = genericQuery.GetAll<Currency>();
            this.localeSubTypes = genericQuery.GetAll<LocaleSubType>();
        }

        [OutputCache(NoStore = true, Duration = 0, VaryByParam = "*")]
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Search(string chainName)
        {
            var locales = getLocalesByChainQuery.Search(new GetLocalesByChainParameters
            {
                ChainName = chainName
            });

            var localeViewModels = locales.Select(locale => new LocaleGridRowViewModel(locale)).ToList();
            return View(CreateChain(false, localeViewModels));
        }

        private LocaleGridViewModel RetrieveLocales(bool includeDeleted)
        {
            GetLocaleParameters searchRegionParameters = new GetLocaleParameters();
            List<LocaleGridRowViewModel> locales = getLocaleQuery.Search(searchRegionParameters).Select(locale => new LocaleGridRowViewModel(locale)).ToList();

            return CreateChain(includeDeleted, locales);
        }

        private LocaleGridViewModel CreateChain(bool includeDeleted, List<LocaleGridRowViewModel> locales)
        {
            LocaleGridViewModel model = new LocaleGridViewModel();
            LocaleGridRowViewModel chainLocale = new LocaleGridRowViewModel();

            model.ChainLocale = chainLocale;
            chainLocale.Locales = GetLocales(locales, null, includeDeleted);

            model.ChainLocale = PopulateChildAttribute(model.ChainLocale);

            model.Countries = this.countries.Select(c => new CountryViewModel { CountryId = c.countryID, CountryCode = c.countryCode, CountryName = c.countryName });
            model.Territories = this.territories.Select(t => new TerritoryViewModel { TerritoryId = t.territoryID, TerritoryCode = t.territoryCode, TerritoryName = t.territoryName });
            model.TimeZones = this.timeZones.Select(t => new TimeZoneViewModel { TimeZoneId = t.timezoneID, TimeZoneCode = t.timezoneCode, TimeZoneName = t.timezoneName });
            model.EwicAgencies = this.eWicAgencies;
            model.StorePosTypes = StorePosTypes.AsDictionary.Values.ToList();
            model.Currencies = this.currencies.Select(c => new CurrencyViewModel { CurrencyTypeID = c.currencyTypeID, CurrencyTypeCode = c.currencyTypeCode, CurrencyTypeDesc = c.currencyTypeDesc, IssuingEntity = c.issuingEntity, NumericCode = c.numericCode, MinorUnit = c.minorUnit, Symbol = c.symbol });
            model.LocaleSubTypes = this.localeSubTypes.Select(l => new LocaleSubTypeViewModel { LocaleSubTypeID = l.localeSubTypeID, LocaleTypeID = l.localeTypeID, LocaleSubTypeCode = l.localSubTypeCode, LocaleSubTypeDescription = l.localeSubTypeDesc });
            return model;
        }

        private List<LocaleGridRowViewModel> GetLocales(List<LocaleGridRowViewModel> allLocales, LocaleGridRowViewModel parentLocale, bool includeDeleted)
        {
            int? parentLocaleId = null;
            string parentLocaleName = String.Empty;

            if (parentLocale != null)
            {
                parentLocaleId = parentLocale.LocaleId;
                parentLocaleName = parentLocale.LocaleName;
            }

            var childLocales = allLocales.Where(l => l.ParentLocaleId == parentLocaleId).OrderBy(l => l.LocaleName);

            List<LocaleGridRowViewModel> storeHierarchy = new List<LocaleGridRowViewModel>();

            foreach (var locale in childLocales)
            {
                LocaleGridRowViewModel gridViewModel = new LocaleGridRowViewModel
                {
                    LocaleId = locale.LocaleId,
                    LocaleName = locale.LocaleName,
                    ParentLocaleId = locale.ParentLocaleId,
                    ParentLocaleName = parentLocaleName,
                    OwnerOrgPartyId = locale.OwnerOrgPartyId,
                    LocaleTypeId = locale.LocaleTypeId,
                    LocaleTypeDesc = locale.LocaleTypeDesc,
                    OpenDate = locale.OpenDate,
                    CloseDate = locale.CloseDate,
                    RegionAbbreviation = locale.RegionAbbreviation,
                    BusinessUnitId = locale.BusinessUnitId,
                    LocaleAddLink = locale.LocaleAddLink,
                    Locales = GetLocales(allLocales, locale, includeDeleted),
                    AddressID = locale.AddressID,
                    AddressLine1 = locale.AddressLine1,
                    AddressLine2 = locale.AddressLine2,
                    AddressLine3 = locale.AddressLine3,
                    City = locale.City,
                    PostalCode = locale.PostalCode,
                    County = locale.County,
                    TerritoryId = locale.TerritoryId,
                    CountryId = locale.CountryId,
                    TimeZoneId = locale.TimeZoneId,
                    Latitude = locale.Latitude,
                    Longitude = locale.Longitude,
                    TerritoryCode = locale.TerritoryId == 0 ? String.Empty : territories.FirstOrDefault(t => t.territoryID == locale.TerritoryId).territoryCode,
                    CountryCode = locale.CountryId == 0 ? String.Empty : countries.FirstOrDefault(c => c.countryID == locale.CountryId).countryCode,
                    TimeZoneCode = (locale.TimeZoneId == 0 || locale.TimeZoneId == null) ? String.Empty : timeZones.FirstOrDefault(tz => tz.timezoneID == locale.TimeZoneId).timezoneCode,
                    ContactPerson = locale.ContactPerson,
                    PhoneNumber = locale.PhoneNumber,
                    StoreAbbreviation = locale.StoreAbbreviation,
                    EwicAgencyId = locale.EwicAgencyId,
                    Fax = locale.Fax,
                    IrmaStoreId = locale.IrmaStoreId,
                    StorePosType = locale.StorePosType,
                    CurrencyCode = locale.CurrencyCode,
                    VenueCode = locale.VenueCode,
                    VenueOccupant = locale.VenueOccupant,
                    LocaleSubType = locale.LocaleSubType,
                    LocaleSubTypeId = localeSubTypes.Where(l => l.localeSubTypeDesc == locale.LocaleSubType).Select(ls => ls.localeSubTypeID).FirstOrDefault()
                };

                storeHierarchy.Add(gridViewModel);
            }

            return storeHierarchy;
        }

        private LocaleGridRowViewModel PopulateChildAttribute(LocaleGridRowViewModel storeModel)
        {
            if (storeModel.HasChildren)
            {
                storeModel.ChildLocaleTypeCode = storeModel.Locales.First().LocaleTypeId;
            }

            if (storeModel.LocaleTypeId == LocaleTypes.Metro)
            {
                storeModel.LocaleAddLink = String.Format("<a href=/Store/CreateStore?parentLocaleId={0}&parentLocaleName={1}>Add Store</a>", storeModel.LocaleId.ToString(), storeModel.LocaleName);
            }

            if (storeModel.LocaleTypeId == LocaleTypes.Store)
            {
                storeModel.LocaleAddLink = String.Format("<a href=/Venue/CreateVenue?parentLocaleId={0}&parentLocaleName={1}>Add Venue</a>", storeModel.LocaleId.ToString(), storeModel.LocaleName.Replace(" ", "&nbsp;"));
            }

            foreach (var locale in storeModel.Locales)
            {
                PopulateChildAttribute(locale);
            }

            return storeModel;
        }

        // GET: /Store/CreateStore
        [HttpGet]
        [WriteAccessAuthorize(GlobalDataTeamException = true)]
        public ActionResult CreateStore(int? parentLocaleId, string parentLocaleName)
        {
            if ((parentLocaleId == null || parentLocaleId < 1) || String.IsNullOrEmpty(parentLocaleName))
            {
                return RedirectToAction("Index");
            }

            GetLocaleParameters parameters = new GetLocaleParameters { LocaleId = parentLocaleId.Value };
            Locale parentLocale = getLocaleQuery.Search(parameters).Single();

            LocaleManagementViewModel viewModel = new LocaleManagementViewModel
            {
                ParentLocaleId = parentLocale.localeID,
                ParentLocaleName = parentLocale.localeName,
                LocaleTypeId = LocaleTypes.Store,
                OwnerOrgPartyId = parentLocale.ownerOrgPartyID
            };

            viewModel = PopulateViewModelDropDowns(viewModel);

            return View(viewModel);
        }

        // POST: /Store/CreateStore/{Locale}
        [HttpPost]
        [WriteAccessAuthorize(GlobalDataTeamException = true)]
        [ValidateAntiForgeryToken]
        public ActionResult CreateStore(LocaleManagementViewModel viewModel)
        {
            if (!ModelState.IsValid)
            {
                viewModel = PopulateViewModelDropDowns(viewModel);
                return View(viewModel);
            }

            AddLocaleManager manager = new AddLocaleManager
            {
                LocaleName = viewModel.LocaleName,
                LocaleParentId = viewModel.ParentLocaleId,
                OpenDate = viewModel.OpenDate,
                OwnerOrgPartyId = viewModel.OwnerOrgPartyId,
                LocaleTypeID = viewModel.LocaleTypeId.HasValue ? viewModel.LocaleTypeId.Value : LocaleTypes.Store,
                BusinessUnit = viewModel.BusinessUnit,
                StoreAbbreviation = viewModel.StoreAbbreviation,
                PhoneNumber = viewModel.PhoneNumber,
                ContactPerson = viewModel.ContactPerson,
                AddressLine1 = viewModel.AddressLine1,
                AddressLine2 = viewModel.AddressLine2,
                AddressLine3 = viewModel.AddressLine3,
                City = viewModel.City,
                TerritoryId = viewModel.TerritoryId,
                PostalCode = viewModel.PostalCode,
                County = viewModel.County,
                TimeZoneId = viewModel.TimeZoneId,
                CountryId = viewModel.CountryId,
                Latitude = viewModel.Latitude,
                Longitude = viewModel.Longitude,
                EwicAgencyId = viewModel.EwicAgencyId,
                Fax = viewModel.Fax,
                IrmaStoreId = viewModel.IrmaStoreId,
                StorePosType = viewModel.SelectedStorePosType,
                UserName = User.Identity.Name
            };

            try
            {
                addLocaleManager.Execute(manager);
            }
            catch (CommandException ex)
            {
                var exceptionLogger = new ExceptionLogger(logger);
                exceptionLogger.LogException(ex, this.GetType(), MethodBase.GetCurrentMethod());

                ViewData["ErrorMessage"] = ex.Message;
                viewModel = PopulateViewModelDropDowns(viewModel);

                return View(viewModel);
            }

            return RedirectToAction("Index");
        }

        [WriteAccessAuthorize(IsJsonResult = true, SetStatusCode = true, GlobalDataTeamException = true)]
        [ValidateInput(false)]
        public ActionResult SaveChanges()
        {
            ViewData["GenerateCompactJSONResponse"] = false;
            Boolean updatingVenue = false;

            GridModel gridModel = new GridModel();

            List<Transaction<LocaleGridRowViewModel>> localeTransactions = gridModel.LoadTransactions<LocaleGridRowViewModel>(HttpContext.Request.Unvalidated.Form["ig_transactions"]);

            JsonResult result = new JsonResult();
            Dictionary<string, bool> response = new Dictionary<string, bool>();

            foreach (Transaction<LocaleGridRowViewModel> t in localeTransactions)
            {
                if (t.layoutKey != null)
                {
                    if (t.type == "row")
                    {
                        LocaleGridRowViewModel localeRow = (LocaleGridRowViewModel)t.row;

                        try
                        {
                            if (localeRow.LocaleTypeId == LocaleTypes.Store)
                            {
                                StoreModel storeModel = ConvertToStoreModel(localeRow);
                                UpdateLocaleManager command = new UpdateLocaleManager(storeModel);
                                updateLocaleManager.Execute(command);

                            }
                            else
                            {
                                updatingVenue = true;
                                VenueModel venueModel = ConvertToVenueModel(localeRow);
                                UpdateVenueManager command = new UpdateVenueManager(venueModel);
                                updateVenueManager.Execute(command);
                            }
                        }
                        catch (CommandException ex)
                        {
                            var exceptionLogger = new ExceptionLogger(logger);
                            exceptionLogger.LogException(ex, this.GetType(), MethodBase.GetCurrentMethod());

                            Response.StatusCode = (int)System.Net.HttpStatusCode.BadRequest;
                            if (updatingVenue)
                            {
                                ErrorModel errorModel = new ErrorModel();
                                errorModel.Error = ex.Message;
                                errorModel.IsUpdatingVenue = true;
                                return Json(errorModel, JsonRequestBehavior.AllowGet);

                            }
                            return result;
                        }
                    }

                }
            }

            response.Add("Success", true);
            result.Data = response;
            return result;
        }

        private VenueModel ConvertToVenueModel(LocaleGridRowViewModel localeRow)
        {
            VenueModel venueModel = new VenueModel
            {
                LocaleId = localeRow.LocaleId,
                LocaleName = localeRow.LocaleName,
                ParentLocaleId = localeRow.ParentLocaleId,
                OpenDate = localeRow.OpenDate != null ? localeRow.OpenDate : (DateTime?)null,
                CloseDate = localeRow.CloseDate != null ? localeRow.CloseDate : (DateTime?)null,
                LocaleTypeId = localeRow.LocaleTypeId,
                LocaleSubType = localeSubTypes.Where(ls => ls.localeSubTypeID == localeRow.LocaleSubTypeId).Select(s => s.localeSubTypeDesc).First(),
                VenueCode = localeRow.VenueCode,
                VenueOccupant = localeRow.VenueOccupant,
                LocaleSubTypeId = localeRow.LocaleSubTypeId,
                UserName = User.Identity.Name
            };

            return venueModel;
        }

        private StoreModel ConvertToStoreModel(LocaleGridRowViewModel localeRow)
        {
            StoreModel storeModel = new StoreModel
            {
                LocaleId = localeRow.LocaleId,
                LocaleName = localeRow.LocaleName,
                ParentLocaleId = localeRow.ParentLocaleId,
                OpenDate = localeRow.OpenDate != null ? localeRow.OpenDate : (DateTime?)null,
                CloseDate = localeRow.CloseDate != null ? localeRow.CloseDate : (DateTime?)null,
                OwnerOrgPartyId = localeRow.OwnerOrgPartyId,
                LocaleTypeId = localeRow.LocaleTypeId,
                StoreAbbreviation = localeRow.StoreAbbreviation,
                BusinessUnitId = localeRow.BusinessUnitId,
                PhoneNumber = localeRow.PhoneNumber,
                Fax = localeRow.Fax,
                ContactPerson = localeRow.ContactPerson,
                AddressID = localeRow.AddressID,
                AddressLine1 = localeRow.AddressLine1,
                AddressLine2 = localeRow.AddressLine2,
                AddressLine3 = localeRow.AddressLine3,
                City = localeRow.City,
                PostalCode = localeRow.PostalCode,
                County = localeRow.County,
                CountryId = localeRow.CountryId.Value,
                TerritoryId = localeRow.TerritoryId.Value,
                TimeZoneId = localeRow.TimeZoneId.Value,
                Latitude = String.IsNullOrWhiteSpace(localeRow.Latitude) ? null : (decimal?)Decimal.Parse(localeRow.Latitude),
                Longitude = String.IsNullOrWhiteSpace(localeRow.Latitude) ? null : (decimal?)Decimal.Parse(localeRow.Longitude),
                EwicAgencyId = localeRow.EwicAgencyId,
                IrmaStoreId = localeRow.IrmaStoreId,
                StorePosType = localeRow.StorePosType,
                UserName = User.Identity.Name
            };
            return storeModel;
        }

        private LocaleManagementViewModel PopulateViewModelDropDowns(LocaleManagementViewModel viewModel)
        {
            viewModel.CountryList = this.countries.Select(c => new SelectListItem { Value = c.countryID.ToString(), Text = c.countryName });
            viewModel.TerritoryList = this.territories.Select(t => new SelectListItem { Value = t.territoryID.ToString(), Text = t.territoryName });
            viewModel.TimeZones = this.timeZones.Select(tz => new SelectListItem { Value = tz.timezoneID.ToString(), Text = tz.timezoneName });
            viewModel.StorePosTypes = StorePosTypes.AsDictionary.Values.Select(t => new SelectListItem { Value = t, Text = t });
            viewModel.StorePosTypes.First().Selected = true;
            viewModel.EwicAgencies = this.eWicAgencies.Select(a => new SelectListItem { Value = a.AgencyId, Text = a.AgencyId });
            viewModel.CurrencyTypes = this.currencies.Select(c => new SelectListItem { Value = c.currencyTypeID.ToString(), Text = c.currencyTypeCode });

            return viewModel;
        }
    }
}
