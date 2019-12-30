using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Reflection;
using System.Text.RegularExpressions;
using System.Security.Cryptography;
using Icon.Common.DataAccess;
using Icon.Framework;
using Icon.Logging;
using Icon.Web.Common;
using Icon.Web.DataAccess.Infrastructure;
using Icon.Web.DataAccess.Managers;
using Icon.Web.DataAccess.Models;
using Icon.Web.DataAccess.Queries;
using Icon.Web.Mvc.Extensions;
using Icon.Web.Mvc.Attributes;
using Icon.Web.Mvc.Excel;
using Icon.Web.Mvc.Exporters;
using Icon.Web.Mvc.Models;
using Infragistics.Web.Mvc;
using Icon.Web.Mvc.Utility;
using Icon.Web.DataAccess.Extensions;

namespace Icon.Web.Mvc.Controllers
{
    public class ContactController : Controller
    {
        private ILogger logger;
        private IconWebAppSettings settings;
        private IDonutCacheManager cacheManager;
        private IManagerHandler<AddUpdateContactManager> contactManagerHandler;
        private IQueryHandler<GetContactsParameters, List<ContactModel>> getContactsQuery;
        private IQueryHandler<GetHierarchyClassByIdParameters, HierarchyClass> getHierarchyClassQuery;
        private IQueryHandler<GetContactTypesParameters, List<ContactTypeModel>> getContactTypesQuery;

        public ContactController(
            ILogger logger,
            IQueryHandler<GetContactsParameters, List<ContactModel>> getContactsQuery,
            IQueryHandler<GetContactTypesParameters, List<ContactTypeModel>> getContactTypesQuery,
            IQueryHandler<GetHierarchyClassByIdParameters, HierarchyClass> getHierarchyClassQuery,
            IManagerHandler<AddUpdateContactManager> contactManagerHandler,
            IconWebAppSettings settings,
            IDonutCacheManager cacheManager)
        {
            this.logger = logger;
            this.settings = settings;
            this.cacheManager = cacheManager;
            this.contactManagerHandler = contactManagerHandler;
            this.getContactsQuery = getContactsQuery;
            this.getContactTypesQuery = getContactTypesQuery;
            this.getHierarchyClassQuery = getHierarchyClassQuery;
        }

        // GET: Contact
        public ActionResult Index()
        {
            return View();
        }

        // GET: Contact/Contact/5
        [WriteAccessAuthorize]
        public ActionResult Contact(int hierarchyClassId)
        {
            if(!this.settings.IsContactViewEnabled)
            {
                //return View();
                return RedirectToAction("Disabled", "Contact");
            }

            ViewBag.UserWriteAccess = GetWriteAccess();
            var viewModel = GetHierarchyClass(hierarchyClassId);
            return View(viewModel);
        }

        public ActionResult Disabled()
        {
            return View();
        }

        [GridDataSourceAction]
        public ActionResult ContactAll(int hierarchyClassId)
        {
            var viewModels = GetContacts(hierarchyClassId);
            return View(viewModels.AsQueryable());
        }

         // GET: Contact/Create
        [WriteAccessAuthorize]
        public ActionResult Manage(int hierarchyClassId, int contactId)
        {
            ContactViewModel viewModel = contactId <= 0
                ? null
                : GetContacts(hierarchyClassId).Where(x => x.ContactId == contactId).FirstOrDefault();

            if(viewModel == null)
            { 
              viewModel = EmptyViewModel(hierarchyClassId);
            }

            ViewBag.UserWriteAccess = GetWriteAccess();
            if(viewModel.ContactTypes == null)
            {
                viewModel.ContactTypes = GetContactTypes();
            }


            return View(viewModel);
        }
        
        // POST: Brand/Create
        [HttpPost]
        [WriteAccessAuthorize]
        public ActionResult Manage(ContactViewModel viewModel)
        {
            var isOK = ModelState.IsValid;
            
            if(GetWriteAccess() != Enums.WriteAccess.Full)
            {
                isOK = false;
                ViewData["ErrorMessage"] = "You don't have write privileges to add or update Contact.";
            }

            if(isOK)
            {
                var regEx = new Regex( @"\s+", RegexOptions.Compiled);
                var emailRegex = new Regex(@"^([\w-]+\.)*?[\w-]+@[\w-]+\.([\w-]+\.)*?[\w]+$", RegexOptions.Compiled);
                viewModel.ContactName = viewModel.ContactName == null ? String.Empty : regEx.Replace(viewModel.ContactName.Trim().Replace('\t', '\0'), " ");
                viewModel.Email = viewModel.Email == null ? String.Empty : viewModel.Email.Replace(" ", String.Empty);

                if(String.IsNullOrEmpty(viewModel.ContactName) || String.IsNullOrEmpty(viewModel.Email) || !emailRegex.IsMatch(viewModel.Email))
                {
                    isOK = false;
                    ViewData["ErrorMessage"] = "Contact Name and valid Email are requred.";
                }
            }

            if(!isOK)
            {
                viewModel.ContactTypes =  GetContactTypes();
                ViewBag.UserWriteAccess = GetWriteAccess();
                return View(viewModel);
            }

            var manager = new AddUpdateContactManager
            {
                Contacts = new List<ContactModel>()
                    {
                        new ContactModel()
                        {
                            AddressLine1 = viewModel.AddressLine1,
                            AddressLine2 = viewModel.AddressLine2,
                            City = viewModel.City,
                            ContactId = viewModel.ContactId,
                            ContactTypeId = viewModel.ContactTypeId,
                            Country = viewModel.Country,
                            ContactName = viewModel.ContactName,
                            Email = viewModel.Email,
                            HierarchyClassId = viewModel.HierarchyClassId,
                            PhoneNumber1 = viewModel.PhoneNumber1,
                            PhoneNumber2 = viewModel.PhoneNumber2,
                            State = viewModel.State,
                            Title = viewModel.Title,
                            WebsiteURL = viewModel.WebsiteURL,
                            ZipCode = viewModel.ZipCode
                        }
                    }
            };

            try
            {
                this.contactManagerHandler.Execute(manager);
                ModelState.Clear();
                ViewData["SuccessMessage"] = $"Contact {viewModel.ContactName} has been successfully added";
                return RedirectToAction("Contact", "Contact", new { hierarchyClassId = viewModel.HierarchyClassId });   
                //return View(EmptyViewModel(viewModel.HierarchyClassId));
            }
            catch (CommandException ex)
            {
                var exLogger = new ExceptionLogger(logger);
                exLogger.LogException(ex, this.GetType(), MethodBase.GetCurrentMethod());
                ViewData["ErrorMessage"] = ex.Message;
                viewModel.ContactTypes = GetContactTypes();
                ViewBag.UserWriteAccess = GetWriteAccess();
                return View(viewModel);
            }
        }

        private ContactViewModel EmptyViewModel(int hierarchyClassId)
        {
            var hierarchy = GetHierarchyClass(hierarchyClassId);

            var viewModel = new ContactViewModel()
                {
                    ContactId = 0,
                    HierarchyClassId = hierarchyClassId,
                    ContactName = "New Contact"
                };

            viewModel.HierarchyName = hierarchy.HierarchyLevelName;
            viewModel.HierarchyClassName = hierarchy.HierarchyClassName;
            ViewBag.UserWriteAccess = GetWriteAccess();
            viewModel.ContactTypes = GetContactTypes();
            return viewModel;
        }

        private HierarchyClassViewModel GetHierarchyClass(int hierarchyClassId)
        {
            var hierarchy = getHierarchyClassQuery.Search(new GetHierarchyClassByIdParameters(){ HierarchyClassId = hierarchyClassId });
            return new HierarchyClassViewModel(hierarchy);
        }

        private List<ContactViewModel> GetContacts(int hierarchyClassId)
        {
            return getContactsQuery.Search(new GetContactsParameters(){ HierarchyClassId = hierarchyClassId }).ToViewModels();
        }

        private List<ContactTypeViewModel> GetContactTypes()
        {
            return getContactTypesQuery.Search(new GetContactTypesParameters()).ToViewModels();
        }

        private Enums.WriteAccess GetWriteAccess()
        {
            bool hasWriteAccess = this.settings.WriteAccessGroups.Split(',').Any(x => this.HttpContext.User.IsInRole(x.Trim()));
            return hasWriteAccess ? Enums.WriteAccess.Full : Enums.WriteAccess.None;
        }
    }
}