using System;
using System.Collections.Generic;
using System.Web;
using System.Web.Mvc;
using System.IO;
using OutOfStock.Models;


namespace OutOfStock.Controllers
{
    public class UploadController : Controller
    {
        private readonly IUserLoginManager _loginManager;
        public const string UploadFolder = "~/App_Data/UploadedFiles/";
        public static HashSet<string> SupportedExtensions = new HashSet<string> { ".xls", ".xlsx", ".txt", ".csv" };

        public UploadController(IUserLoginManager loginManager)
        {
            _loginManager = loginManager;
        }

        public ActionResult Index()
        {

            var username = OOSUser.GetUserName();
            var userLocation = OOSUser.GetUsersLocation(username);

            _loginManager.RecordLogin(OOSUser.GetUserName(), userLocation.Region, userLocation.Store);

            ActionResult result;
            if (!OOSUser.HasValidLocationInformation)
                result = View("~/Views/Shared/InvalidLocationInformation.cshtml");
            else
            {
                if (!OOSUser.isRegionalBuyer)
                    result = new RedirectResult("~/Home/Index");
                else
                    result = View();
            }
            return result;
        }

        [HttpPost]
        public ActionResult ReadExcelSheet(HttpPostedFileBase file)
        {
            ActionResult result;
            var msg = string.Empty;
            try
            {
                var serverFilename = string.Empty;
                var userFileName = string.Empty;
                var errorMessage = string.Empty;
                var sheetErrors = new List<string>();
                var isError = file == null || file.ContentLength <= 0 || string.IsNullOrWhiteSpace(file.FileName);
                if (isError)
                    errorMessage = "Please choose a file to upload.";
                else
                {
                    string ext = Path.GetExtension(file.FileName);
                    isError = !SupportedExtensions.Contains(ext);
                    if (isError)
                        errorMessage = $"Unsupported file extension: {ext}";
                    else
                    {
                        userFileName = Path.GetFileName(file.FileName);
                        serverFilename = HttpContext.Server.MapPath(UploadFolder) +
                            Guid.NewGuid() + ext;
                        try
                        {
                            file.SaveAs(serverFilename);
                        }
                        catch (Exception ex)
                        {
                            MvcApplication.oosLog.Warn(ex.Message + ", Stack=" + ex.StackTrace);
                            isError = true;
                            errorMessage = $"Internal error uploading file. ({ex.Message})";
                        }
                    }
                }
                int recordsUpdated = 0;
                if (!isError)
                {

                    if (string.IsNullOrEmpty(OOSUser.userRegion))
                    {
                        msg = "";
                        userFileName = "";
                        errorMessage = "Unable to determine the region the user belongs to.";
                        MvcApplication.oosLog.Warn(errorMessage);
                    }
                    else if (OOSUser.userRegion.ToUpper() == "CE")
                    {
                        msg = "";
                        userFileName = "";
                        errorMessage = "Only regional users can upload data for their region.";
                        MvcApplication.oosLog.Warn(errorMessage);
                    }

                    else
                    {
                        try
                        {
                            
                            sheetErrors = KnownOOSUploadModel.ReadExcelFile(serverFilename,out recordsUpdated);
                        }
                        catch (Exception ex)
                        {
                            MvcApplication.oosLog.Warn(ex.Message + ", Stack=" + ex.StackTrace);
                            errorMessage = "There was an error while interpreting the file";
                        }
                    }
                }
                if (!string.IsNullOrWhiteSpace(serverFilename))
                {
                    for (int retry = 0; retry < 10; ++retry)
                    {
                        try
                        {
                            System.IO.File.Delete(serverFilename);
                            retry = 10;
                        }
                        catch (Exception)
                        {
                            System.Threading.Thread.Sleep(200);
                        }
                    }
                }
                ViewBag.sheetErrors = sheetErrors;
                
                if ( ! string.IsNullOrEmpty(userFileName))
                {
                    ViewBag.MessageClass = "list_good";
                    msg= $"[{userFileName}] was uploaded. ";
                }

                if (sheetErrors.Count > 0)
                {
                    ViewBag.MessageClass = "list_bad";
                    msg = $"[{userFileName}] was uploaded but some rows had errors. ";
                }

                if (errorMessage.Length > 0)
                    msg +=  errorMessage;
                else
                {

                    if ( recordsUpdated == 1)
                    {
                        msg += $"{recordsUpdated} record was saved.";
                    }
                    else
                    {
                        msg += $"{recordsUpdated} records were saved.";
                    }
                }

                ViewBag.Message = msg;
                result = View("Index");
            }
            catch (Exception ex)
            {
                MvcApplication.oosLog.Warn(ex.Message + ", Stack=" + ex.StackTrace);
                ViewBag.Message = "Sorry, an internal error has occurred.";
                result = View("Index");
            }
            
            return result;
        }

    }
}
