using Icon.Web.Common.BulkUpload;
using Icon.Web.Mvc.Attributes;
using Icon.Web.Mvc.Domain.BulkImport;
using Icon.Web.Mvc.Models;
using System;
using System.IO;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;

namespace Icon.Web.Mvc.Controllers
{
    public class BulkUploadController : Controller
    {
        private IBulkUploadService bulkUploadService;

        public BulkUploadController(IBulkUploadService bulkUploadService)
        {
            this.bulkUploadService = bulkUploadService;
        }

        [HttpGet]
        [WriteAccessAuthorize]
        public ActionResult Index(BulkUploadDataType bulkUploadDataType)
        {
            BulkUploadViewModel bulkUploadViewModel = new BulkUploadViewModel
            {
                BulkUploadDataType = bulkUploadDataType
            };
            return View(bulkUploadViewModel);
        }

        [HttpGet]
        public ActionResult BulkUploadStatus(BulkUploadDataType bulkUploadDataType, int rowCount)
        {
            //(BulkUploadDataType)Enum.Parse(typeof(BulkUploadDataType), bulkUploadDataType)
            var data = bulkUploadService.GetBulkUploads(bulkUploadDataType, rowCount)
                .Select(u => new BulkUploadStatusViewModel
                {
                    BulkUploadId = u.BulkUploadId,
                    FileModeType = u.FileModeType.ToString(),
                    FileName = u.FileName,
                    FileUploadTime = u.FileUploadTime,
                    Message = u.Message,
                    NumberOfRowsWithError = u.NumberOfRowsWithError,
                    PercentageProcessed = u.PercentageProcessed,
                    Status = u.Status,
                    UploadedBy = u.UploadedBy
                });
            return Json(data, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public ActionResult UploadErrors(BulkUploadDataType bulkUploadDataType, int id)
        {
            var model = bulkUploadService.GetBulkUpload(bulkUploadDataType, id);
            var viewModel = new BulkUploadStatusViewModel
            {
                BulkUploadId = model.BulkUploadId,
                FileModeType = model.FileModeType.ToString(),
                FileName = model.FileName,
                BulkUploadDataType = bulkUploadDataType.ToString(),
                FileUploadTime = model.FileUploadTime,
                Message = model.Message,
                NumberOfRowsWithError = model.NumberOfRowsWithError,
                PercentageProcessed = model.PercentageProcessed,
                Status = model.Status,
                UploadedBy = model.UploadedBy
            };
            return View(viewModel);
        }

        [HttpGet]
        public ActionResult GetBulkUploadErrors(BulkUploadDataType bulkUploadDataType, int id)
        {
            var data = bulkUploadService.GetBulkUploadErrors(bulkUploadDataType, id);
            return Json(data, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        [WriteAccessAuthorize]
        public ActionResult UploadFiles(BulkUploadDataType bulkUploadDataType)
        {
            // Checking no of files injected in Request object  
            if (Request.Files.Count > 0)
            {
                var uploadedFileName = string.Empty;
                var uploadedFileType = Request.Form["fileType"];
                try
                {
                    //  Get all files from Request object
                    HttpFileCollectionBase files = Request.Files;
                    for (int i = 0; i < files.Count; i++)
                    {
                        var uploadedFile = files[i];
                        if (uploadedFile == null)
                        {
                            var result = new BulkUploadResultModel { Result = "Error", Message = "No file selected" };
                            return Json(result);
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
                        var uploadedData = binaryReader.ReadBytes(uploadedFile.ContentLength);

                        try
                        {
                            bulkUploadService.BulkUpload(
                                bulkUploadDataType,
                                uploadedFileType == "UpdateExisting" ? BulkUploadActionType.Update : BulkUploadActionType.Add,
                                uploadedFileName,
                                uploadedData,
                                User.Identity.Name);
                            return RequestInfo($"File uploaded uploaded successfully and added to processing queue. You can monitor processing status in the grid.", HttpStatusCode.OK);
                        }
                        catch
                        {
                            throw;
                        }
                    }
                }
                catch (Exception ex)
                {
                    return RequestInfo($"Upload failed: {ex.Message}", HttpStatusCode.ExpectationFailed);
                }
            }
            else
            {
                return RequestInfo("No files have been selected", HttpStatusCode.BadRequest);
            }

            return null;
        }

        [HttpGet]
        public ActionResult BulkUploadErrorReport(BulkUploadDataType bulkUploadDataType, int id)
        {
            try
            {
                var model = bulkUploadService.GetBulkUploadErrorExport(bulkUploadDataType, id);

                using (var mem = new MemoryStream())
                {
                    try
                    {
                        mem.Write(model.BulkUploadModel.FileContent, 0, model.BulkUploadModel.FileContent.Length);

                        using (var rdr = new Icon.Web.Mvc.Excel.ExcelReader(mem))
                        {
                            var links = new DocumentFormat.OpenXml.Spreadsheet.Hyperlinks();
                            var rowid = 2;
                            foreach (var grp in model.bulkUploadErrorModels.OrderBy(a => a.RowId).GroupBy(a => a.RowId))
                            {
                                foreach (var val in grp)
                                {
                                    links.Append(new DocumentFormat.OpenXml.Spreadsheet.Hyperlink() { Reference = $"A{links.Count() + 2}", Location = $"Brands!A{rowid}", Display = $"Ref ID: {rowid}", Tooltip = val.Message });
                                }
                                rowid++;
                            }

                            var listId = model.bulkUploadErrorModels.Select(a => a.RowId).Distinct().ToList();
                            rdr.SetErrorLinks(links, "BrandsValidation", listId);
                            SendForDownloadBrand(mem, $"{Path.GetFileNameWithoutExtension(model.BulkUploadModel.FileName)}_Error.xlsx");
                        }
                    }
                    catch (Exception ex)
                    {
                        var result = new BulkUploadResultModel { Result = "Error", Message = $"Error occurred. Error details: {ex.Message}" };
                        return Json(result);
                    }
                }
            }
            catch (Exception ex)
            {
                var result = new BulkUploadResultModel { Result = "Error", Message = $"Error occurred. Error details: {ex.Message}" };
                return Json(result);
            }
            return Json("OK", JsonRequestBehavior.AllowGet);
        }

        private ActionResult RequestInfo(string errMessage, HttpStatusCode statusCode)
        {
            //To prevent IIS from hijacking custom response or add the line below to web config file in <system.webServer> section
            //<httpErrors errorMode="DetailedLocalOnly" existingResponse="PassThrough"/>
            Response.TrySkipIisCustomErrors = true;

            Response.StatusCode = (int)statusCode;
            Response.StatusDescription = errMessage;
            return Json(errMessage);
        }

        private void SendForDownloadBrand(Stream fileStream, string name)
        {
            fileStream.Position = 0;
            Response.Clear();
            Response.AppendHeader("content-disposition", "attachment; filename=" + name);
            Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
            Response.SetCookie(new HttpCookie("fileDownload", "true") { Path = "/" });
            fileStream.CopyTo(Response.OutputStream);
            Response.End();
        }
    }
}