using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Reflection;
using System.Text.RegularExpressions;
using Icon.Common.DataAccess;
using Icon.Framework;
using Icon.Logging;
using Icon.Web.Common;
using Icon.Web.DataAccess.Models;
using Icon.Web.DataAccess.Queries;
using Icon.Web.Mvc.Extensions;
using Icon.Web.Mvc.Attributes;
using Icon.Web.Mvc.Exporters;
using Icon.Web.Mvc.Models;
using Infragistics.Web.Mvc;
using Icon.Web.Mvc.Utility;
using System.IO;
using System.Data;
using Icon.Web.DataAccess.Commands;
using Infragistics.Documents.Excel;
using Newtonsoft.Json;

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
        private IQueryHandler<GetContactEligibleHCParameters, HashSet<int>> getContactEligibleHCQuery;
        private ICommandHandler<AddUpdateContactCommand> handlerAddUpdateContact;
        private ICommandHandler<AddUpdateContactTypeCommand> handlerAddUpdateContactType;
        private ICommandHandler<DeleteContactCommand> handlerDeleteContact;
        private ICommandHandler<DeleteContactTypeCommand> handlerDeleteContactType;
        private IExcelExporterService excelExporterService;
        private IQueryHandler<GetBulkContactUploadErrorsPrameters, List<BulkUploadErrorModel>> getBulkUploadErrorsQueryHandler;
        private IQueryHandler<GetBulkContactUploadStatusParameters, List<BulkContactUploadStatusModel>> getBulkUploadStatusQueryHandler;
        private IQueryHandler<GetBulkContactUploadByIdParameters, BulkItemUploadStatusModel> getBulkUploadByIdQueryHandler;
        private ICommandHandler<BulkContactUploadCommand> bulkUploadCommandHandler;
        private Regex emailRegex = new Regex(@"^([\w-]+\.)*?[\w-]+@[\w-]+\.([\w-]+\.)*?[\w]+$", RegexOptions.Compiled);

        const string sngl_space = " ";
        Regex rgxSpace = new Regex(@"\s+", RegexOptions.Compiled);
        Regex rgxNRT = new Regex(@"[\n\r\t]", RegexOptions.Compiled); //New Line & Tab
        public enum UploadStatus { None, InProcess, ValidationFailed, Validated, LoadFailed, Loaded, OK}

        class UploadInfo
        {
            public enum StatusCode { None, InProcess, ValidationFailed, Validated, LoadFailed, Loaded, OK}
            
            public StatusCode Code { get; set; }
            public int TotatlRecords { get; set; }
            public string ErrorMessage { get; set; }
            public Dictionary<string, int> ValidationCounts { get; set; }
        }

        public ContactController(
            ILogger logger,
            IQueryHandler<GetContactsParameters, List<ContactModel>> getContactsQuery,
            IQueryHandler<GetContactTypesParameters, List<ContactTypeModel>> getContactTypesQuery,
            IQueryHandler<GetHierarchyClassByIdParameters, HierarchyClass> getHierarchyClassQuery,
            ICommandHandler<AddUpdateContactCommand> handlerAddUpdateContact,
            ICommandHandler<AddUpdateContactTypeCommand> handlerAddUpdateContactType,
            ICommandHandler<DeleteContactCommand> handlerDeleteContact,
            ICommandHandler<DeleteContactTypeCommand> handlerDeleteContactType,
            IQueryHandler<GetContactEligibleHCParameters, HashSet<int>> getContactEligibleHCQuery,
            IconWebAppSettings settings,
            IDonutCacheManager cacheManager,
            IExcelExporterService excelExporterService,
            IQueryHandler<GetBulkContactUploadByIdParameters, BulkItemUploadStatusModel> getBulkUploadByIdQueryHandler,
            IQueryHandler<GetBulkContactUploadStatusParameters, List<BulkContactUploadStatusModel>> getBulkUploadStatusQueryHandler,
            IQueryHandler<GetBulkContactUploadErrorsPrameters, List<BulkUploadErrorModel>> getBulkUploadErrorsQueryHandler,
            ICommandHandler<BulkContactUploadCommand> bulkUploadCommandHandler)
        {
            this.logger = logger;
            this.settings = settings;
            this.cacheManager = cacheManager;
            this.handlerAddUpdateContact = handlerAddUpdateContact;
            this.handlerAddUpdateContactType = handlerAddUpdateContactType;
            this.handlerDeleteContact = handlerDeleteContact;
            this.handlerDeleteContactType = handlerDeleteContactType;
            this.getContactsQuery = getContactsQuery;
            this.getContactTypesQuery = getContactTypesQuery;
            this.getHierarchyClassQuery = getHierarchyClassQuery;
            this.excelExporterService = excelExporterService;
            this.getBulkUploadStatusQueryHandler = getBulkUploadStatusQueryHandler;
            this.getBulkUploadErrorsQueryHandler = getBulkUploadErrorsQueryHandler;
            this.getBulkUploadByIdQueryHandler = getBulkUploadByIdQueryHandler;
            this.bulkUploadCommandHandler = bulkUploadCommandHandler;
            this.getContactEligibleHCQuery = getContactEligibleHCQuery;
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
            if (!this.settings.IsContactViewEnabled)
            {
                TempData["Message"] = "Contact view is currently disabled.";
                return RedirectToAction("FYI", "Contact");
            }

            var viewModel = GetHierarchyClass(hierarchyClassId);

            if (viewModel.HierarchyId != Hierarchies.Brands && viewModel.HierarchyId != Hierarchies.Manufacturer)
            {
                TempData["Message"] = $"Selected hierarchy class does not support Contact Management. HierarchyClassId = {hierarchyClassId}";
                return RedirectToAction("FYI", "Contact");
            }

            ViewBag.UserWriteAccess = GetWriteAccess();
            return View(viewModel);
        }

        public ActionResult FYI()
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
            if (hierarchy.HierarchyId != Hierarchies.Brands && hierarchy.HierarchyId != Hierarchies.Manufacturer)
            {
                TempData["Message"] = $"Selected hierarchy class does not support Contact Management. HierarchyClassId = {hierarchyClassId}"; ;
                return RedirectToAction("Disabled", "Contact");
            }

            ContactViewModel viewModel = contactId <= 0
                ? EmptyViewModel(hierarchyClassId)
                : GetContacts(hierarchyClassId).Where(x => x.ContactId == contactId).FirstOrDefault();

            if (viewModel == null)
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

            if (GetWriteAccess() != Enums.WriteAccess.Full)
            {
                isOK = false;
                ViewData["ErrorMessage"] = "You don't have write privileges to add or update Contact.";
            }

            if (isOK)
            {
                var regEx = new Regex(@"\s+", RegexOptions.Compiled);
                viewModel.ContactName = viewModel.ContactName == null ? String.Empty : regEx.Replace(viewModel.ContactName.Trim().Replace('\t', '\0'), " ");
                viewModel.Email = viewModel.Email == null ? String.Empty : viewModel.Email.Replace(" ", String.Empty);

                if (viewModel.ContactTypeId <= 0 || String.IsNullOrEmpty(viewModel.ContactName) || String.IsNullOrEmpty(viewModel.Email) || !this.emailRegex.IsMatch(viewModel.Email))
                {
                    isOK = false;
                    ViewData["ErrorMessage"] = "Contact Type, Name and valid Email are required.";
                }
            }

            if (isOK) //Check if the same Contact already exists
            {
                var contact = GetContacts(viewModel.HierarchyClassId)
                        .Where(x => x.ContactId != viewModel.ContactId && x.ContactTypeId == viewModel.ContactTypeId && String.Compare(x.Email, viewModel.Email, StringComparison.CurrentCultureIgnoreCase) == 0)
                        .FirstOrDefault();

                if (contact != null)
                {
                    isOK = false;
                    ViewData["ErrorMessage"] = $"Contact with the same Contact Type and Email already exists. Existing Contact Name is {contact.ContactName}";
                }
            }

            if (!isOK)
            {
                viewModel.ContactTypes = GetContactTypes();
                return View(viewModel);
            }

			var command = new AddUpdateContactCommand()
			{
				UserName = User.Identity.Name,
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

        [HttpGet]
        public ActionResult BulkUploadStatus(int rowCount)
        {
            var data = this.getBulkUploadStatusQueryHandler.Search(new GetBulkContactUploadStatusParameters() { RowCount = rowCount });
            return Json(data, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        [WriteAccessAuthorize]
        public ActionResult BulkUpload()
        {
            BulkUploadViewModel bulkUploadViewModel = new BulkUploadViewModel();
            ViewData["BulkUploadType"] = "Contact";
            return View(bulkUploadViewModel);
        }

        [HttpGet]
        public ActionResult BulkUploadErrors(int Id)
        {
            var model = getBulkUploadByIdQueryHandler.Search(new GetBulkContactUploadByIdParameters { BulkContactUploadId = Id });

            return View(model);
        }

        [HttpGet]
        public ActionResult GetBulkUploadErrors(int Id)
        {
            var parameters = new GetBulkContactUploadErrorsPrameters() { BulkContactUploadId = Id };
            var data = this.getBulkUploadErrorsQueryHandler.Search(parameters);
            return Json(data, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public ActionResult DownloadRefFile(string fileName)
        {
            var cache = System.Runtime.Caching.MemoryCache.Default;
            if(cache.Contains(fileName))
            { 
                var data = cache[fileName] as byte[];
                SendForDownload(new MemoryStream(data), fileName);
                return Json("OK", JsonRequestBehavior.AllowGet);
            }
            else
            {
                TempData["Message"] = $"Requested file not found: {fileName}.";
                return RedirectToAction("FYI", "Contact");
            }
        }

        [HttpPost]
        [WriteAccessAuthorize]
        public ActionResult UploadFiles()
        {
            //To prevent IIS from hijacking custom response or add the line below to web config file in <system.webServer> section
            //<httpErrors errorMode="DetailedLocalOnly" existingResponse="PassThrough"/>
            Response.TrySkipIisCustomErrors = true; 

            if(Request.Files == null || Request.Files.Count == 0)
            {
                Response.StatusCode = (int)System.Net.HttpStatusCode.BadRequest;
                Response.StatusDescription = "No file(s) selected";
                return Json(JsonConvert.SerializeObject(new { message = Response.StatusDescription }), JsonRequestBehavior.AllowGet); 
            }
            else
            {
                var uploadedFileType = Request.Form["fileType"];
                var uploadedFileName = string.Empty;
                char[] trimChar = new char[]{'.', ' '};

                try
                {
                    //  Get all files from Request object  
                    HttpFileCollectionBase files = Request.Files;
                    for(int i = 0; i < files.Count; i++)
                    {
                        var uploadedFile = files[i];

                        if (uploadedFile == null)
                        {
                            Response.StatusDescription = "No file selected";
                            return Json(JsonConvert.SerializeObject(new { message = Response.StatusDescription }), JsonRequestBehavior.AllowGet);
                        }

                        // Checking for Internet Explorer  
                        if (Request.Browser.Browser.ToUpper() == "IE" || Request.Browser.Browser.ToUpper() == "INTERNETEXPLORER")
                        {
                            string[] testfiles = uploadedFile.FileName.Split(new char[] { '\\' });
                            uploadedFileName = testfiles[testfiles.Length - 1];
                        }
                        else
                        {
                            uploadedFileName = uploadedFile.FileName;
                        }

                        var binaryReader = new BinaryReader(uploadedFile.InputStream);
                        var strm = new MemoryStream();
                        strm.Write(binaryReader.ReadBytes(uploadedFile.ContentLength), 0, uploadedFile.ContentLength);
                        strm.Position = 0;
                        var info = ProcessFile(strm);

                        if(info.Code != UploadInfo.StatusCode.Loaded)
                        { 
                            var refFileName = String.Format("Ref_{0}_{1}", DateTime.Now.ToString("yyyyMMdd_hhmmss"), uploadedFileName);
                            var cache = System.Runtime.Caching.MemoryCache.Default;
                            cache.Add(refFileName, strm.ToArray(), new System.Runtime.Caching.CacheItemPolicy(){ AbsoluteExpiration = DateTime.Now.AddMinutes(2) });

                            Response.StatusCode = (int)System.Net.HttpStatusCode.ExpectationFailed;
                            Response.StatusDescription = info.Code == UploadInfo.StatusCode.ValidationFailed 
                                ? "File validaition failed. See validaition error(s) with failed records count below."
                                : $"File process failed. {info.ErrorMessage}";
                            var obj = JsonConvert.SerializeObject( new { fileName = refFileName, validation = info.ValidationCounts.Select(x => new {key = x.Key.Trim(trimChar), value = x.Value}).ToArray() });
                            return Json(obj, JsonRequestBehavior.AllowGet);
                        }
                        else
                        { 
                            try
                            {
                                Response.StatusCode = (int)System.Net.HttpStatusCode.OK;

                                bulkUploadCommandHandler.Execute(new BulkContactUploadCommand()
                                {
                                    BulkContactUploadModel = new BulkContactUploadModel
                                    {
                                        FileName = uploadedFileName,
                                        FileContent = strm.ToArray(),
                                        UploadedBy = User.Identity.Name,
                                        TotalRecords = info.TotatlRecords
                                    }
                                });

                                Response.StatusDescription ="File processed successfully.";
                            }
                            catch (Exception ex)
                            {
                                Response.StatusDescription = $"File processed successfully but failed to store uploaded file: {rgxSpace.Replace(rgxNRT.Replace(ex.Message, sngl_space), sngl_space).Trim()}";
                            }

                            return Json(Response.StatusDescription, JsonRequestBehavior.AllowGet);
                        }
                    }
                }
                catch (Exception ex)
                {
                    Response.StatusCode = (int)System.Net.HttpStatusCode.ExpectationFailed;
                    Response.StatusDescription = ex.Message;
                    return Json(JsonConvert.SerializeObject(new { message = Response.StatusDescription }), JsonRequestBehavior.AllowGet);
                }
            }

            return null;
        }

        private UploadInfo ProcessFile(Stream inputStream)
        {
            var Info = new UploadInfo() { Code = UploadInfo.StatusCode.InProcess };

            var flds = new Icon.Web.Mvc.Excel.Field[]
                {
                    new Icon.Web.Mvc.Excel.Field("ContactId", typeof(int), 0, true),
                    new Icon.Web.Mvc.Excel.Field("HierarchyClassName", typeof(string), null, true),
                    new Icon.Web.Mvc.Excel.Field("Email", typeof(string), null, true, emailRegex, true),
                    new Icon.Web.Mvc.Excel.Field("ContactName", typeof(string), null, false),
                    new Icon.Web.Mvc.Excel.Field("ContactType", typeof(string), null, false),
                    new Icon.Web.Mvc.Excel.Field("Title", typeof(string), null, false),
                    new Icon.Web.Mvc.Excel.Field("AddressLine1", typeof(string), null, false),
                    new Icon.Web.Mvc.Excel.Field("AddressLine2", typeof(string), null, false),
                    new Icon.Web.Mvc.Excel.Field("City", typeof(string), null, false),
                    new Icon.Web.Mvc.Excel.Field("State", typeof(string), null, false),
                    new Icon.Web.Mvc.Excel.Field("ZipCode", typeof(string), null, false),
                    new Icon.Web.Mvc.Excel.Field("Country", typeof(string), null, false),
                    new Icon.Web.Mvc.Excel.Field("PhoneNumber1", typeof(string), null, false),
                    new Icon.Web.Mvc.Excel.Field("PhoneNumber2", typeof(string), null, false),
                    new Icon.Web.Mvc.Excel.Field("WebsiteURL", typeof(string), null, false, null, true),
                };

            try
            {
                int id;
                bool isInvalid = false;
                string linkMessage = null;
                var contactList = new List<ContactModel>();
                var links = new DocumentFormat.OpenXml.Spreadsheet.Hyperlinks();
                const string max15 = "{0} exceeding max length of 15. ";
                const string max30 = "{0} exceeding max length of 30. ";
                const string max255 = "{0} exceeding max length of 255. ";
                const string invalidEmail = "Invalid Email. ";
                const string invalidHierarchy = "Invalid Hierarchy. ";
                const string invalidType = "Invalid Contact Type. ";
                const string missingName = "Contact Name is missing. ";

                
                var contactTypes = GetContactTypes().ToDictionary(x => x.ContactTypeName, x => x.ContactTypeId);
                var eligableIds = this.getContactEligibleHCQuery.Search(new GetContactEligibleHCParameters()).ToHashSet();

                int GetHCID(Icon.Web.Mvc.Excel.ExcelReader rdr)
                {
                    try 
	                {	        
                        int hcId;
                        const char pipe = '|';
                        
                        return rdr["HierarchyClassName"] == null
                            ? 0
                            : int.TryParse(rdr["HierarchyClassName"].ToString().Split(pipe).Last().Trim(), out hcId) && eligableIds.Contains(hcId)
                                ? hcId
                                : 0; 
	                }
	                catch
                    {
                        return 0;
	                }
                }

                using(var rdr = new Icon.Web.Mvc.Excel.ExcelReader(inputStream, "Contact", flds))
                {
                    if(rdr.IsEmpty) throw new Exception("Contact worksheet is empty.");

                    while(rdr.Read())
                    {
                        linkMessage = String.Empty;
                        
                        var contact = new ContactModel()
                        {
                            AddressLine1 = rdr["AddressLine1"]?.ToString(),
                            AddressLine2 = rdr["AddressLine2"]?.ToString(),
                            City = rdr["City"]?.ToString(),
                            ContactId = (int)rdr["ContactId"],
                            ContactTypeId = rdr["ContactType"] == null ? 0 : contactTypes.TryGetValue(rdr["ContactType"].ToString(), out id) ? id : 0,
                            Country = rdr["Country"]?.ToString(),
                            ContactName = rdr["ContactName"]?.ToString(),
                            Email = rdr["Email"]?.ToString(),
                            HierarchyClassId = GetHCID(rdr),
                            PhoneNumber1 = rdr["PhoneNumber1"]?.ToString(),
                            PhoneNumber2 = rdr["PhoneNumber2"]?.ToString(),
                            State = rdr["State"]?.ToString(),
                            Title = rdr["Title"]?.ToString(),
                            WebsiteURL = rdr["WebsiteURL"]?.ToString(),
                            ZipCode = rdr["ZipCode"]?.ToString()
                        };

                        linkMessage = String.Format("{0}{1}{2}{3}{4}{5}{6}{7}{8}{9}{10}{11}{12}{13}",
                            contact.ContactTypeId <= 0 ? invalidType : String.Empty,
                            contact.HierarchyClassId <= 0 ? invalidHierarchy : String.Empty,
                            String.IsNullOrEmpty(contact.Email) ? invalidEmail : String.Empty,
                            String.IsNullOrEmpty(contact.ContactName) ? missingName : (contact.ContactName.Length > 255 ? String.Format(max255, "Contact Name") : String.Empty),
                            contact.AddressLine1 != null && contact.AddressLine1.Length > 255 ? String.Format(max255, "Address Line 1") : String.Empty,
                            contact.AddressLine2 != null && contact.AddressLine2.Length > 255 ? String.Format(max255, "Address Line 2") : String.Empty,
                            contact.City != null && contact.City.Length > 255 ? String.Format(max255, "City") : String.Empty,
                            contact.Country != null && contact.Country.Length > 255 ? String.Format(max255, "Country") : String.Empty,
                            contact.PhoneNumber1 != null && contact.PhoneNumber1.Length > 30 ? String.Format(max30, "Phone Number 1") : String.Empty,
                            contact.PhoneNumber2 != null && contact.PhoneNumber2.Length > 30 ? String.Format(max30, "Phone Number 2") : String.Empty,
                            contact.State != null && contact.State.Length > 255 ? String.Format(max255, "State") : String.Empty,
                            contact.Title != null && contact.Title.Length > 255 ? String.Format(max255, "Title") : String.Empty,
                            contact.WebsiteURL != null && contact.WebsiteURL.Length > 255 ? String.Format(max255, "Website URL") : String.Empty,
                            contact.ZipCode != null && contact.ZipCode.Length > 15 ? String.Format(max15, "Zip Code") : String.Empty
                        ).Trim();

                        if(!String.IsNullOrEmpty(linkMessage))
                        {
                            links.Append(new DocumentFormat.OpenXml.Spreadsheet.Hyperlink(){ Reference = $"A{links.Count() + 2}", Location = $"Contact!A{rdr.RowIndex}", Display = $"Ref ID: {rdr.RowIndex}", Tooltip = linkMessage });

                            if(!isInvalid)
                            {
                                isInvalid = true;
                                contactList.Clear();
                            }

                            contactList.Add(contact); //Keep invalid contacts only if at least one is found
                        }
                        else
                        {
                            if(!isInvalid)
                            {
                                contactList.Add(contact); //Keep valid contacts only
                            }
                        }
                    }

                    Info.TotatlRecords = rdr.RecordsAffected;
                    if(links.Any())
                    {
                        rdr.SetErrorLinks(links);
                    }
                }
                
                Info.ValidationCounts = new List<KeyValuePair<string, int>>()
                {
                    new KeyValuePair<string, int>(String.Format(max255, "Address Line 1"), contactList.Where(x => x.AddressLine1 != null && x.AddressLine1.Length > 255).Count()),
                    new KeyValuePair<string, int>(String.Format(max255, "Address Line 2"), contactList.Where(x => x.AddressLine2 != null && x.AddressLine2.Length > 255).Count()),
                    new KeyValuePair<string, int>(String.Format(max255, "City"), contactList.Where(x => x.City != null && x.City.Length > 255).Count()),
                    new KeyValuePair<string, int>(String.Format(max255, "Contact Name"), contactList.Where(x => x.ContactName != null && x.ContactName.Length > 255).Count()),
                    new KeyValuePair<string, int>(String.Format(max255, "Country"), contactList.Where(x => x.Country != null && x.Country.Length > 255).Count()),
                    new KeyValuePair<string, int>(invalidType, contactList.Where(x => x.ContactTypeId <= 0).Count()),
                    new KeyValuePair<string, int>(invalidEmail, contactList.Where(x => x.Email == null).Count()),
                    new KeyValuePair<string, int>(invalidHierarchy, contactList.Where(x => x.HierarchyClassId <= 0).Count()),
                    new KeyValuePair<string, int>(String.Format(max30, "Phone Number 1"), contactList.Where(x => x.PhoneNumber1 != null && x.PhoneNumber1.Length > 30).Count()),
                    new KeyValuePair<string, int>(String.Format(max30, "Phone Number 2"), contactList.Where(x => x.PhoneNumber2 != null && x.PhoneNumber2.Length > 30).Count()),
                    new KeyValuePair<string, int>(String.Format(max255, "State"), contactList.Where(x => x.State != null && x.State.Length > 255).Count()),
                    new KeyValuePair<string, int>(String.Format(max255, "Title"), contactList.Where(x => x.Title != null && x.Title.Length > 255).Count()),
                    new KeyValuePair<string, int>(String.Format(max255, "Website URL"), contactList.Where(x => x.WebsiteURL != null && x.WebsiteURL.Length > 255).Count()),
                    new KeyValuePair<string, int>(String.Format(max15, "Zip Code"), contactList.Where(x => x.ZipCode != null && x.ZipCode.Length > 15).Count())
                }
                .Where(x => x.Value > 0).ToDictionary(x => x.Key, x => x.Value);

                Info.Code = isInvalid ? UploadInfo.StatusCode.ValidationFailed : UploadInfo.StatusCode.Validated;
               
                if(Info.Code == UploadInfo.StatusCode.Validated)
                {
                    try
                    {
                        AddUpdateContact(contactList);
                        Info.Code = UploadInfo.StatusCode.Loaded;
                    }
                    catch (Exception ex)
                    {
                        Info.ErrorMessage = rgxSpace.Replace(rgxNRT.Replace(ex.Message, sngl_space), sngl_space).Trim();
                        Info.Code = UploadInfo.StatusCode.LoadFailed;
                    }
                }

                return Info;
            }
            catch
            {
                throw;
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
                var regEx = new Regex(@"\s+", RegexOptions.Compiled);
                contactTypeName = contactTypeName == null ? string.Empty : regEx.Replace(contactTypeName.Trim(), " ");

                if (String.IsNullOrEmpty(contactTypeName))
                {
                    Response.StatusCode = (int)System.Net.HttpStatusCode.BadRequest;
                    return Json("Contact Type Name is not specified.");
                }

                if (GetWriteAccess() != Enums.WriteAccess.Full)
                {
                    Response.StatusCode = (int)System.Net.HttpStatusCode.Forbidden;
                    return Json("You don't have write privileges to add or update Contact Type.");
                }

                if (GetContactTypes(true).Any(x => x.ContactTypeId != contactTypeId && String.Compare(x.ContactTypeName.Replace(" ", String.Empty), contactTypeName.Replace(" ", String.Empty), true) == 0))
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
        public ActionResult DeleteContact(int contactId)
        {
            try
            {
                if (GetWriteAccess() != Enums.WriteAccess.Full)
                {
                    Response.StatusCode = (int)System.Net.HttpStatusCode.Forbidden;
                    return Json("You don’t have sufficient privileges to complete selected action.");
                }

				this.handlerDeleteContact.Execute(new DeleteContactCommand() { UserName = User.Identity.Name, ContactIds = new List<int>() { contactId } });
				Response.StatusCode = (int)System.Net.HttpStatusCode.OK;
				return Json(new { success = true, responseText = "Contact has been deleted." }, JsonRequestBehavior.AllowGet);
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
                if (GetWriteAccess() != Enums.WriteAccess.Full)
                {
                    Response.StatusCode = (int)System.Net.HttpStatusCode.Forbidden;
                    return Json("You don’t have sufficient privileges to complete selected action.");
                }

                this.handlerDeleteContactType.Execute(new DeleteContactTypeCommand() { ContactTypeIds = new List<int>() { contactTypeId } });
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
            var hierarchy = getHierarchyClassQuery.Search(new GetHierarchyClassByIdParameters() { HierarchyClassId = hierarchyClassId });
            return new HierarchyClassViewModel(hierarchy);
        }

        private List<ContactViewModel> GetContacts(int hierarchyClassId)
        {
            return getContactsQuery.Search(new GetContactsParameters() { HierarchyClassId = hierarchyClassId }).ToViewModels();
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

        public void Export(int hierarchyClassId=0)
        {
            var contacts = getContactsQuery.Search(new GetContactsParameters() { HierarchyClassId = hierarchyClassId }).OrderBy(a => a.HierarchyName).ThenBy(a => a.HierarchyClassName)
                .Select(c => new Dictionary<string,object>()
                {
                    {"Hierarchy" , c.HierarchyName} ,
                    { "HierarchyClassName", string.Format("{0} | {1}", c.HierarchyClassName, c.HierarchyClassId)},
                    { "ContactId" , c.ContactId},
                    { "ContactType" , c.ContactTypeName},
                    { "ContactName", c.ContactName},
                    { "Email", c.Email},
                    {"Title" , c.Title},
                    {"AddressLine1" , c.AddressLine1},
                    {"AddressLine2" , c.AddressLine2},
                    {"City" , c.City},
                    {"State", c.State},
                    {"ZipCode", c.ZipCode},
                    {"Country" , c.Country},
                    {"PhoneNumber1" , c.PhoneNumber1},
                    {"PhoneNumber2" , c.PhoneNumber2},
                    {"WebsiteURL", c.WebsiteURL}
                }).ToList();
            var newContactTemplateExporter = excelExporterService.GetContactBlankTemplateExporter();

            newContactTemplateExporter.Export(contacts);
            SendForDownload(newContactTemplateExporter.ExportModel.ExcelWorkbook, newContactTemplateExporter.ExportModel.ExcelWorkbook.CurrentFormat, "Contact", "IconImportTemplate_");
        }

        [HttpGet]
        public void ContactTemplateNewExporter()
        {
            var newContactTemplateExporter = excelExporterService.GetContactBlankTemplateExporter();
            
            newContactTemplateExporter.Export();
            SendForDownload(newContactTemplateExporter.ExportModel.ExcelWorkbook, newContactTemplateExporter.ExportModel.ExcelWorkbook.CurrentFormat, "Contact", "IconImportTemplate_");
        }

        private void SendForDownload(Workbook document, WorkbookFormat excelFormat, string source, string filePrefix)
        {
            string documentFileNameRoot = $"{filePrefix}_{source}_{DateTime.Now.ToString("yyyyMMddHHmmss")}.xlsx";

            Response.Clear();
            Response.AppendHeader("content-disposition", "attachment; filename=" + documentFileNameRoot);
            Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
            Response.SetCookie(new HttpCookie("fileDownload", "true") { Path = "/" });
            document.SetCurrentFormat(excelFormat);
            document.Save(Response.OutputStream);
            Response.End();
        }

        private void SendForDownload(Stream fileStream, string name)
        {
            Response.Clear();
            Response.AppendHeader("content-disposition", "attachment; filename=" + name);
            Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
            Response.SetCookie(new HttpCookie("fileDownload", "true") { Path = "/" });
            fileStream.CopyTo(Response.OutputStream);
            Response.End();
        }

        private void AddUpdateContact(List<ContactModel> contactList)
        {
            var command = new AddUpdateContactCommand()
			    {
			        UserName = User.Identity.Name,
			        Contacts = contactList
			    };

            this.handlerAddUpdateContact.Execute(command);  
        }
    }
}