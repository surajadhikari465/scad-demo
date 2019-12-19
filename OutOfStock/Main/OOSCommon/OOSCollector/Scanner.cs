using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using OOSCommon.DataContext;

namespace OOSCommon.OOSCollector
{
    public class Scanner : IScanner
    {
        public string uploadedBasePath { get; set; }
        public string reportedOOSPostImportMoveToPath { get; set; }
        public bool reportedOOSPostImportDelete { get; set; }
        public string regionPrefix { get; set; }
        public string storePrefix { get; set; }
        public string oosConnectionString { get; set; }
        public string oosEFConnectionString { get; set; }
        public bool isValidation { get; set; }
        public OOSCommon.IOOSLog log { get; set; }

        public List<OOSCommon.DataContext.REGION> regions { get; set; }
        public int ixRegion { get; set; }
        public List<OOSCommon.DataContext.STORE> stores { get; set; }
        public int ixStore { get; set; }
        public List<string> files { get; set; }
        public int ixFile { get; set; }

        public static HashSet<string> reservedWords = new HashSet<string> { "CON", "PRN", "AUX", "NUL", "COM1", "COM2", "COM3", "COM4", "COM5", "COM6", "COM7", "COM8", "COM9", "LPT1", "LPT2", "LPT3", "LPT4", "LPT5", "LPT6", "LPT7", "LPT8", "LPT9" };

        public OOSCommon.DataContext.IOOSEntities db
        {
            get
            {
                if (_db == null)
                    _db = new OOSCommon.DataContext.OOSEntities(oosEFConnectionString);
                return _db;
            }
            set { _db = value; }
        } OOSCommon.DataContext.IOOSEntities _db = null;

        public Scanner(string uploadedBasePath, string reportedOOSPostImportMoveToPath, bool reportedOOSPostImportDelete,
            string regionPrefix, string storePrefix, string oosConnectionString, string oosEFConnectionString,
            bool isValidation,
            OOSCommon.IOOSLog log)
        {
            this.uploadedBasePath = uploadedBasePath;
            this.reportedOOSPostImportMoveToPath = reportedOOSPostImportMoveToPath;
            this.reportedOOSPostImportDelete = reportedOOSPostImportDelete;
            this.regionPrefix = regionPrefix;
            this.storePrefix = storePrefix;
            this.oosConnectionString = oosConnectionString;
            this.oosEFConnectionString = oosEFConnectionString;
            this.isValidation = isValidation;
            this.log = log;

            this.regions = null;
            this.ixRegion = -1;
            this.stores = null;
            this.ixStore = -1;
            this.files = null;
            this.ixFile = -1;
        }

        public string GetRegionFirst()
        {
            if (regions == null)
                regions = (from r in db.REGION orderby r.REGION_ABBR select r).ToList();
            ixRegion = -1;
            ixStore = -1;
            stores = null;
            files = null;
            ixFile = -1;
            return GetRegionNext();
        }

        public string GetRegionNext()
        {
            string result = string.Empty;
            ixStore = -1;
            stores = null;
            if (regions != null && (ixRegion + 1) < regions.Count)
            {
                ++ixRegion;
                result = GetRegion();
            }
            return result;
        }

        public string GetRegion()
        {
            string result = string.Empty;
            if (regions != null && ixRegion >= 0 && ixRegion < regions.Count)
                result = regions[ixRegion].REGION_ABBR.ToUpper();
            return result;
        }

        public OOSCommon.DataContext.STORE GetStoreFirst(string regionAbbreviation)
        {
            if (stores == null)
            {
                stores = GetOpenStores(regionAbbreviation);
            }
            ixStore = -1;
            files = null;
            ixFile = -1;
            return GetStoreNext();
        }


        private List<STORE> GetOpenStores(string regionAbbreviation)
        {
            var openStores = (from s in db.STORE
                              join r in db.REGION on s.REGION_ID equals r.ID
                              join ss in db.STATUS on s.STATUS_ID equals ss.ID
                              where r.REGION_ABBR.Equals(regionAbbreviation, StringComparison.OrdinalIgnoreCase)
                              && !ss.STATUS1.Equals("Closed", StringComparison.OrdinalIgnoreCase)
                              orderby s.STORE_ABBREVIATION
                              select s).ToList();
            return openStores;
        }



        public OOSCommon.DataContext.STORE GetStoreNext()
        {
            OOSCommon.DataContext.STORE result = null;
            if (stores != null && (ixStore + 1) < stores.Count)
            {
                ++ixStore;
                result = GetStore();
            }
            return result;
        }

        public OOSCommon.DataContext.STORE GetStore()
        {
            OOSCommon.DataContext.STORE result = null;
            if (stores != null && ixStore >= 0 && ixStore < stores.Count)
                result = stores[ixStore];
            return result;
        }

        public string GetFileFirst(string regionAbbreviation, string storeAbbreviation)
        {
            string result = string.Empty;
            files = null;
            ixFile = -1;
            if (string.IsNullOrWhiteSpace(uploadedBasePath))
                log.Warn("No uploaded base path");
            else if (!Path.IsPathRooted(uploadedBasePath))
                log.Warn("uploaded base path is not rooted: \"" + uploadedBasePath + "\"");
            else
            {
                string storeEffective = storePrefix + storeAbbreviation;
                if (reservedWords.Contains(storeEffective))
                    storeEffective += "_";
                string fullPath = CreatePathAsNeeded(uploadedBasePath,
                    regionPrefix + regionAbbreviation, storeEffective);
                files = Directory.EnumerateFiles(fullPath, "*", SearchOption.TopDirectoryOnly).ToList();
                result = GetFileNext();
            }
            return result;
        }

        public string GetFileNext()
        {
            string result = string.Empty;
            if (files != null && (ixFile + 1) < files.Count)
            {
                ++ixFile;
                result = GetFile();
            }
            return result;
        }

        public string GetFile()
        {
            string result = string.Empty;
            if (files != null && ixFile >= 0 && ixFile < files.Count)
                result = files[ixFile];
            return result;
        }

        public void SetFileDone(string uploadedFile, bool hasError, string regionAbbreviation, string storeAbbreviation)
        {
            if (reportedOOSPostImportDelete)
            {
                try
                {
                    if (!isValidation)
                        File.Delete(uploadedFile);
                }
                catch (Exception exx)
                {
                    log.Warn("Exception while deleting \"" + uploadedFile +
                        "\" after import. " + exx.Message);
                }
            }
            else if (!string.IsNullOrWhiteSpace(reportedOOSPostImportMoveToPath))
            {
                string effectivePath = reportedOOSPostImportMoveToPath;
                string importedFile = string.Empty;
                if (!Path.IsPathRooted(reportedOOSPostImportMoveToPath))
                {
                    effectivePath = Path.GetDirectoryName(uploadedFile);
                    effectivePath = Path.GetFullPath(effectivePath + reportedOOSPostImportMoveToPath);
                    if (!effectivePath.EndsWith("\\"))
                        effectivePath += "\\";
                    CreatePathAsNeeded(effectivePath);
                    importedFile = effectivePath + Path.GetFileName(uploadedFile);
                }
                else
                {
                    string storeEffective = storePrefix + storeAbbreviation;
                    if (reservedWords.Contains(storeEffective))
                        storeEffective += "_";
                    CreatePathAsNeeded(effectivePath,
                        regionPrefix + regionAbbreviation, storeEffective);
                    importedFile = uploadedFile.Replace(uploadedBasePath, effectivePath);
                }
                string importedFileFull = importedFile;
                for (int pass = 0; File.Exists(importedFileFull); ++pass)
                {
                    importedFileFull = importedFile + "." + pass.ToString();
                    if (hasError)
                        importedFileFull += ".error";
                }
                if (!isValidation)
                {
                    try
                    {
                        // File.Move sometimes leaves uploadedFile because it is busy.  This avoids the latency
                        System.Threading.Thread.Sleep(10);
                        File.Move(uploadedFile, importedFileFull);
                        // File.Move sometimes leaves uploadedFile because it is busy.  This takes care of thing if the sleep did not help.
                        if (File.Exists(uploadedFile))
                        {
                            try
                            {
                                File.Delete(uploadedFile);
                            }
                            catch (Exception ex)
                            {
                                log.Warn("Exception deleting file after move: File=\"" + uploadedFile +
                                    "\", Message=\"" + ex.Message + "\"" +
                                    (ex.InnerException == null ? string.Empty : ", InnerMessage=\"" + ex.InnerException.Message + "\"")
                                    );
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        log.Warn("Exception moving file to archive: Source=\"" + uploadedFile +
                            "\", Destination=\"" + importedFileFull +
                            "\", Message=\"" + ex.Message + "\"" +
                            (ex.InnerException == null ? string.Empty : ", InnerMessage=\"" + ex.InnerException.Message + "\"")
                            );
                    }
                }
            }
        }

        protected bool CreatePathAsNeeded(string fullPath)
        {
            bool exists = true;
            try
            {
                if (!Directory.Exists(fullPath))
                {
                    Directory.CreateDirectory(fullPath);
                    log.Info("Created directory \"" + fullPath + "\"");
                }
            }
            catch (Exception ex)
            {
                exists = false;
                log.Warn("Path check/create failed for \"" + fullPath +
                    "\" with exception, " + ex.Message + (ex.InnerException == null ? string.Empty :
                    ", Inner=\"" + ex.InnerException.Message + "\""));
            }
            return exists;
        }

        protected string CreatePathAsNeeded(string basePath, string regionAbbreviation, string storeAbbreviation)
        {
            string result = string.Empty;
            string fullPath = Path.GetFullPath(basePath);
            if (CreatePathAsNeeded(fullPath))
            {
                if (fullPath.Length > 0 && fullPath[fullPath.Length - 1] != '\\')
                    fullPath += "\\";
                fullPath += regionAbbreviation.ToUpper();
                if (CreatePathAsNeeded(fullPath))
                {
                    if (CreatePathAsNeeded(fullPath))
                    {
                        fullPath += "\\" + storeAbbreviation.ToUpper();
                        if (CreatePathAsNeeded(fullPath))
                            result = fullPath;
                    }
                }
            }
            return result;
        }

    }
}
