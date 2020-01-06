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
using Icon.Web.DataAccess.Commands;

namespace Icon.Web.Mvc.Controllers
{
    public class ContactController : Controller
    {
        private ILogger logger;
        private IconWebAppSettings settings;
        private IDonutCacheManager cacheManager;
        private IQueryHandler<GetContactsParameters, List<ContactModel>> getContactsQuery;
        private IQueryHandler<GetHierarchyClassByIdParameters, HierarchyClass> getHierarchyClassQuery;
        private IQueryHandler<GetContactTypesParameters, List<ContactTypeModel>> getContactTypesQuery;
        private ICommandHandler<AddUpdateContactCommand> handlerAddUpdateContact;
        private ICommandHandler<AddUpdateContactTypeCommand> handlerAddUpdateContactType;
        private ICommandHandler<DeleteContactTypeCommand> handlerDeleteContactType;

        public ContactController(
            ILogger logger,
            IQueryHandler<GetContactsParameters, List<ContactModel>> getContactsQuery,
            IQueryHandler<GetContactTypesParameters, List<ContactTypeModel>> getContactTypesQuery,
            IQueryHandler<GetHierarchyClassByIdParameters, HierarchyClass> getHierarchyClassQuery,
            ICommandHandler<AddUpdateContactCommand> handlerAddUpdateContact,
            ICommandHandler<AddUpdateContactTypeCommand> handlerAddUpdateContactType,
            ICommandHandler<DeleteContactTypeCommand> handlerDeleteContactType,
            IconWebAppSettings settings,
            IDonutCacheManager cacheManager)
        {
            this.logger = logger;
            this.settings = settings;
            this.cacheManager = cacheManager;
            this.handlerAddUpdateContact = handlerAddUpdateContact;
            this.handlerAddUpdateContactType = handlerAddUpdateContactType;
            this.handlerDeleteContactType = handlerDeleteContactType;
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
                TempData["Message"] = "Contact view is currently disabled.";
                return RedirectToAction("Disabled", "Contact");
            }
            
            var viewModel = GetHierarchyClass(hierarchyClassId);

            if(viewModel.HierarchyId != Hierarchies.Brands && viewModel.HierarchyId != Hierarchies.Manufacturer)
            {
                TempData["Message"] = $"Selected hierarchy class does not support Contact Management. HierarchyClassId = {hierarchyClassId}";
                return RedirectToAction("Disabled", "Contact");
            }

            ViewBag.UserWriteAccess = GetWriteAccess();
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

        [GridDataSourceAction]
        public ActionResult ContactTypeAll()
        {
            var viewModels = GetContactTypes(true);
            return View(viewModels.AsQueryable());
        }

         // GET: Contact/Create
        [WriteAccessAuthorize]
        public ActionResult Manage(int hierarchyClassId, int contactId)
        {
            var hierarchy = GetHierarchyClass(hierarchyClassId);
            if(hierarchy.HierarchyId != Hierarchies.Brands && hierarchy.HierarchyId != Hierarchies.Manufacturer)
            {
                TempData["Message"] = $"Selected hierarchy class does not support Contact Management. HierarchyClassId = {hierarchyClassId}";;
                return RedirectToAction("Disabled", "Contact");
            }

            ContactViewModel viewModel = contactId <= 0
                ? EmptyViewModel(hierarchyClassId)
                : GetContacts(hierarchyClassId).Where(x => x.ContactId == contactId).FirstOrDefault();

            if(viewModel == null)
            {
              TempData["Message"] = $"Mismatched HierarchyClassId <{hierarchyClassId}> and ContactId <{contactId}>.";
              return RedirectToAction("Disabled", "Contact");
            }

            ViewBag.UserWriteAccess = GetWriteAccess();
            viewModel.ContactTypes = GetContactTypes();

            return View(viewModel);
        }
        
        // POST: Brand/Create
        [HttpPost]
        [WriteAccessAuthorize]
        public ActionResult Manage(ContactViewModel viewModel)
        {
            var isOK = ModelState.IsValid;
            ViewBag.UserWriteAccess = GetWriteAccess();
            
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

                if(viewModel.ContactTypeId <= 0 || String.IsNullOrEmpty(viewModel.ContactName) || String.IsNullOrEmpty(viewModel.Email) || !emailRegex.IsMatch(viewModel.Email))
                {
                    isOK = false;
                    ViewData["ErrorMessage"] = "Contact Type, Name and valid Email are required.";
                }
            }

            if(isOK) //Check if the same Contact already exists
            {
                var contact = GetContacts(viewModel.HierarchyClassId)
                        .Where(x => x.ContactId != viewModel.ContactId && x.ContactTypeId == viewModel.ContactTypeId && String.Compare(x.Email, viewModel.Email, StringComparison.CurrentCultureIgnoreCase) == 0)
                        .FirstOrDefault();

                    if(contact != null)
                    {
                        isOK = false;
                        ViewData["ErrorMessage"] = $"Contact with the same Contact Type and Email already exists. Existing Contact Name is {contact.ContactName}";   
                    }
            }

            if(!isOK)
            {
                viewModel.ContactTypes =  GetContactTypes();
                return View(viewModel);
            }

            var command = new AddUpdateContactCommand()
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
                this.handlerAddUpdateContact.Execute(command);
                ModelState.Clear();
                ViewData["SuccessMessage"] = String.Format("Contact {0} has been successfully {1}.", viewModel.ContactName, viewModel.ContactId > 0 ? "updated" : "added");
                //return RedirectToAction("Contact", "Contact", new { hierarchyClassId = viewModel.HierarchyClassId });   
                //return View(EmptyViewModel(viewModel.HierarchyClassId));
                viewModel.ContactTypes = GetContactTypes();
                return View(viewModel);
            }
            catch (CommandException ex)
            {
                var exLogger = new ExceptionLogger(logger);
                exLogger.LogException(ex, this.GetType(), MethodBase.GetCurrentMethod());
                ViewData["ErrorMessage"] = ex.Message;
                viewModel.ContactTypes = GetContactTypes();
                return View(viewModel);
            }
        }

        public ActionResult ManageType()
        {
            ViewBag.UserWriteAccess = GetWriteAccess();
            return View();
        }

        [HttpPost]
        [WriteAccessAuthorize]
        public ActionResult AddUpdateContactType(string contactTypeName, int contactTypeId = 0, bool isArchived = false)
        {
            try
            {
                var regEx = new Regex( @"\s+", RegexOptions.Compiled);
                contactTypeName = contactTypeName == null ? string.Empty : regEx.Replace(contactTypeName.Trim(), " ");

                if(String.IsNullOrEmpty(contactTypeName))
                {
                    Response.StatusCode = (int)System.Net.HttpStatusCode.BadRequest;
                    return Json("Contact Type Name is not specified.");
                }

                if(GetWriteAccess() != Enums.WriteAccess.Full)
                {
                    Response.StatusCode = (int)System.Net.HttpStatusCode.Forbidden;
                    return Json("You don't have write privileges to add or update Contact Type.");
                }

                if(GetContactTypes(true).Any(x => x.ContactTypeId != contactTypeId && String.Compare(x.ContactTypeName.Replace(" ", String.Empty), contactTypeName.Replace(" ", String.Empty), true) == 0))
                {
                    Response.StatusCode = (int)System.Net.HttpStatusCode.Conflict;
                    return Json("Contact Type already exists.");
                }

                var command = new AddUpdateContactTypeCommand()
                {
                    ContactTypes = new List<ContactTypeModel>()
                    {
                        new ContactTypeModel(){ ContactTypeId = contactTypeId, ContactTypeName = contactTypeName, Archived = isArchived }
                    }
                };

                this.handlerAddUpdateContactType.Execute(command);
                var contactType = GetContactTypes(true).Where(x => String.Compare(x.ContactTypeName, contactTypeName, true) == 0).Single();
                Response.StatusCode = (int)System.Net.HttpStatusCode.OK;
                return Json(new { success = true, responseText = "Contact Type has been successfully added.", ContactTypeId = contactType.ContactTypeId, ContactTypeName = contactType.ContactTypeName }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                Response.StatusCode = (int)System.Net.HttpStatusCode.ExpectationFailed;
                return Json(ex.Message);
            }
        }

        [HttpPost]
        [WriteAccessAuthorize]
        public ActionResult DeleteContactType(int contactTypeId)
        {
            try
            {
                if(GetWriteAccess() != Enums.WriteAccess.Full)
                {
                    Response.StatusCode = (int)System.Net.HttpStatusCode.Forbidden;
                    return Json("You don’t have sufficient privileges to complete selected action.");
                }

                this.handlerDeleteContactType.Execute(new DeleteContactTypeCommand(){ ContactTypeIds = new List<int>(){ contactTypeId } });
                Response.StatusCode = (int)System.Net.HttpStatusCode.OK;
                return Json(new { success = true, responseText = "Contact type(s) has been deleted." }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                Response.StatusCode = (int)System.Net.HttpStatusCode.ExpectationFailed;
                return Json(ex.Message);
            }
        }

        private ContactViewModel EmptyViewModel(int hierarchyClassId)
        {
            var hierarchy = GetHierarchyClass(hierarchyClassId);

            var viewModel = new ContactViewModel()
                {
                    ContactId = 0,
                    HierarchyClassId = hierarchyClassId,
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

        private List<ContactTypeViewModel> GetContactTypes(bool includeArchived = false)
        {
            return getContactTypesQuery.Search(new GetContactTypesParameters() { IncludeArchived = includeArchived }).ToViewModels();
        }

        private Enums.WriteAccess GetWriteAccess()
        {
            bool hasWriteAccess = this.settings.WriteAccessGroups.Split(',').Any(x => this.HttpContext.User.IsInRole(x.Trim()));
            return hasWriteAccess ? Enums.WriteAccess.Full : Enums.WriteAccess.None;
        }
    }
}