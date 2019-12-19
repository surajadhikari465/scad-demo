using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;

using System.IO;
//using MassTransit;
using OOS.Model;
using OutOfStock.Models;


namespace OutOfStock.Controllers
{
    public class UploadController : Controller
    {
        public const string uploadFolder = "~/App_Data/UploadedFiles/";
        public static HashSet<string> supportedExtensions = 
            new HashSet<string>() { ".xls", ".xlsx", ".txt", ".csv" };

        private ProductStatusToKnownUploadCommandMapper productStatusToKnownUploadCommandMapper;
        //private IServiceBus bus;

        //public UploadController(ProductStatusToKnownUploadCommandMapper productStatusToKnownUploadCommandMapper)
        public UploadController()
        {
           // this.productStatusToKnownUploadCommandMapper = productStatusToKnownUploadCommandMapper;
            
        }

        public ActionResult Index()
        {
            OutOfStock.MvcApplication.oosLog.Trace("Enter");
            ActionResult result = null;
            if (!OOSUser.isRegionalBuyer)
                result = new RedirectResult("~/Home/Index");
            else
                result = View();
            OutOfStock.MvcApplication.oosLog.Trace("Exit");
            return result;
        }

        [HttpPost]
        public ActionResult ReadExcelSheet(HttpPostedFileBase file)
        {
            OutOfStock.MvcApplication.oosLog.Trace("Enter");
            ActionResult result = null;
            var msg = string.Empty;
            try
            {
                var serverFilename = string.Empty;
                var userFileName = string.Empty;
                var errorMessage = string.Empty;
                var sheetErrors = new List<string>();
                var isError = (file == null || file.ContentLength <= 0 || string.IsNullOrWhiteSpace(file.FileName));
                if (isError)
                    errorMessage = "Please choose a file to upload.";
                else
                {
                    string ext = Path.GetExtension(file.FileName);
                    isError = !supportedExtensions.Contains(ext);
                    if (isError)
                        errorMessage = string.Format("Unsupported file extension: {0}", ext);
                    else
                    {
                        userFileName = Path.GetFileName(file.FileName);
                        serverFilename = HttpContext.Server.MapPath(uploadFolder) +
                            Guid.NewGuid().ToString() + ext;
                        try
                        {
                            file.SaveAs(serverFilename);
                        }
                        catch (Exception ex)
                        {
                            OutOfStock.MvcApplication.oosLog.Warn(ex.Message + ", Stack=" + ex.StackTrace);
                            isError = true;
                            errorMessage = string.Format("Internal error uploading file. ({0})", ex.Message);
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
                        OutOfStock.MvcApplication.oosLog.Warn(errorMessage);
                    }
                    else if (OOSUser.userRegion.ToUpper() == "CE")
                    {
                        msg = "";
                        userFileName = "";
                        errorMessage = "Only regional users can upload data for their region.";
                        OutOfStock.MvcApplication.oosLog.Warn(errorMessage);
                    }

                    else
                    {
                        try
                        {
                            //sheetErrors = OutOfStock.Models.KnownOOSUploadModel.ReadExcelFile(serverFilename, productStatusToKnownUploadCommandMapper, bus);
                            sheetErrors = OutOfStock.Models.KnownOOSUploadModel.ReadExcelFile(serverFilename,
                                                                                              out recordsUpdated);
                        }
                        catch (Exception ex)
                        {
                            OutOfStock.MvcApplication.oosLog.Warn(ex.Message + ", Stack=" + ex.StackTrace);
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
                    msg= string.Format("[{0}] was uploaded. ", userFileName);
                }

                if (sheetErrors.Count > 0)
                {
                    ViewBag.MessageClass = "list_bad";
                    msg = string.Format("[{0}] was uploaded but some rows had errors. ", userFileName);
                }

                if (errorMessage.Length > 0)
                    msg +=  errorMessage;
                else
                {

                    if ( recordsUpdated == 1)
                    {
                        msg += string.Format("{0} record was saved.", recordsUpdated);
                    }
                    else
                    {
                        msg += string.Format("{0} records were saved.", recordsUpdated);
                    }
                }

                ViewBag.Message = msg;
                result = View("Index");
            }
            catch (Exception ex)
            {
                OutOfStock.MvcApplication.oosLog.Warn(ex.Message + ", Stack=" + ex.StackTrace);
                ViewBag.Message = "Sorry, an internal error has occurred.";
                result = View("Index");
            }
            OutOfStock.MvcApplication.oosLog.Trace("Exit");
            return result;
        }

    }
}
