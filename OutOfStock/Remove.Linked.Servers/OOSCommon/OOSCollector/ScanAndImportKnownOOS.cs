using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Text.RegularExpressions;
using OOSCommon;

namespace OOSCommon.OOSCollector
{
    public class ScanAndImportKnownOOS
    {
        public OOSCommon.IOOSLog log { get; set; }
        public bool isValidationMode { get; set; }
        public OOSCommon.VIM.IVIMRepository vimRepository { get; set; }
        public OOSCommon.Import.IOOSImportKnown importKnown { get; set; }
        public OOSCommon.Import.IOOSUpdateKnown updateKnown { get; set; }
        public string oosEFConnectionString { get; set; }
        public string ftpUrlUNFI { get; set; }
        public string knownOOSPostImportMoveToPath { get; set; }
        public bool knownOOSPostImportDelete { get; set; }

        public ScanAndImportKnownOOS(OOSCommon.IOOSLog log, bool isValidationMode,
            OOSCommon.VIM.IVIMRepository vimRepository,
            OOSCommon.Import.IOOSImportKnown importKnown,
            OOSCommon.Import.IOOSUpdateKnown updateKnown,
            string oosEFConnectionString,
            string ftpUrlUNFI, 
            string knownOOSPostImportMoveToPath, bool knownOOSPostImportDelete)
        {
            this.log = log;
            this.isValidationMode = isValidationMode;
            this.vimRepository = vimRepository;
            this.importKnown = importKnown;
            this.updateKnown = updateKnown;
            this.oosEFConnectionString = oosEFConnectionString;
            this.ftpUrlUNFI = ftpUrlUNFI;
            this.knownOOSPostImportMoveToPath = knownOOSPostImportMoveToPath;
            this.knownOOSPostImportDelete = knownOOSPostImportDelete;
        }

        public void DoScanAndImport()
        {
            log.Trace("Enter");
            // Get directory
            List<FtpDirectoryContent> ftpDirectoryContent = GetFtpDirectory(ftpUrlUNFI);
            // import files
            foreach (FtpDirectoryContent ftpEntry in ftpDirectoryContent)
            {
                log.Info("Known OOS file: " + ftpEntry.fileName);

                // Prefer date in filename of present
                DateTime eventDate = ftpEntry.createDate;
                {
                    DateTime? eventDateFromFileName = Utility.FindDateInString(ftpEntry.fileName);
                    if (eventDateFromFileName.HasValue)
                        eventDate = eventDateFromFileName.Value;
                }

                string fileFtpUri = ftpUrlUNFI + ftpEntry.fileName;

                // Import known oos data
                try
                {
                    // Import file content
                    string fileContent = GetFtpFileContent(fileFtpUri);
                    bool isOk = !string.IsNullOrWhiteSpace(fileContent);
                    if (isOk)
                        isOk = importKnown.GetKnownOOSFromContent(fileContent);
                    if (isOk)
                        isOk = updateKnown.BeginBatch(eventDate, importKnown);
                    if (isOk)
                        isOk = updateKnown.WriteKnownOOS(importKnown);
                }
                catch (Exception ex)
                {
                    log.Warn("Error Loading known oos file: File=\"" + ftpEntry.fileName + "\", Error=" +
                        ex.Message + (ex.InnerException == null ? string.Empty :
                        ", Inner=\"" + ex.InnerException.Message + "\""));
                }

                // Dispose of file
                GetFtpFileDisposition(fileFtpUri, ftpEntry.fileName);
            }
            log.Trace("Exit");
        }

        protected void GetFtpFileDisposition(string fileFtpUri, string fileName)
        {
            if (!isValidationMode && (this.knownOOSPostImportDelete ||
                !string.IsNullOrWhiteSpace(this.knownOOSPostImportMoveToPath)))
            {
                try
                {
                    FtpWebRequest request = (FtpWebRequest)WebRequest.Create(fileFtpUri);
                    if (this.knownOOSPostImportDelete)
                        request.Method = WebRequestMethods.Ftp.DeleteFile;
                    else if (!string.IsNullOrWhiteSpace(this.knownOOSPostImportMoveToPath))
                    {
                        request.Method = WebRequestMethods.Ftp.Rename;
                        request.RenameTo = this.knownOOSPostImportMoveToPath.Replace('\\', '/');
                        if (!request.RenameTo.EndsWith("/"))
                            request.RenameTo += "/";
                        request.RenameTo += fileName;
                    }
                    FtpWebResponse response = (FtpWebResponse)request.GetResponse();
                    response.Close();
                }
                catch (Exception ex)
                {
                    log.Warn("Error disposing known oos file with FTP: File=\"" + fileFtpUri + "\", Error=" +
                        ex.Message + (ex.InnerException == null ? string.Empty :
                        ", Inner=\"" + ex.InnerException.Message + "\""));
                }
            }
        }

        protected string GetFtpFileContent(string fileFtpUri)
        {
            log.Trace("Enter: File=\"" + fileFtpUri + "\"");
            string fileContent = string.Empty;              // E.g. "Testing "
            string fileStatusDescription = string.Empty;    // E.g. "226 File send OK.\r\n"
            //string fileFtpUri = ftpUrlUNFI + ftpEntry.fileName;
            // Get file content
            try
            {
                // Credentials must be in the URL
                FtpWebRequest request = (FtpWebRequest)WebRequest.Create(fileFtpUri);
                request.Method = WebRequestMethods.Ftp.DownloadFile;
                {
                    FtpWebResponse response = (FtpWebResponse)request.GetResponse();
                    Stream responseStream = response.GetResponseStream();
                    {
                        StreamReader reader = new StreamReader(responseStream);
                        fileContent = reader.ReadToEnd();
                        reader.Close();
                    }
                    fileStatusDescription = response.StatusDescription;
                    response.Close();
                }
            }
            catch (Exception ex)
            {
                log.Warn("Error reading known oos file with FTP: File=\"" + fileFtpUri + "\", Error=" +
                    ex.Message + (ex.InnerException == null ? string.Empty :
                    ", Inner=\"" + ex.InnerException.Message + "\""));
            }
            log.Trace("Exit");
            return fileContent;
        }

        protected List<FtpDirectoryContent> GetFtpDirectory(string ftpUrl)
        {
            // Get directory
            List<FtpDirectoryContent> ftpDirectoryContent = null;
            try
            {
                // Credentials must be in the URL
                FtpWebRequest request = (FtpWebRequest)WebRequest.Create(ftpUrl);
                request.Method = WebRequestMethods.Ftp.ListDirectoryDetails;
                {
                    FtpWebResponse response = (FtpWebResponse)request.GetResponse();
                    Stream responseStream = response.GetResponseStream();
                    {
                        StreamReader reader = new StreamReader(responseStream);
                        string directoryContent = string.Empty;     // E.g. "-rw-r--r--    1 8466     8466            8 Jan 13 11:46 NexusDevTest.txt\r\ndrwxrwxr-x    2 8466     8466         4096 Jan 13 11:46 archive\r\n"
                        //string directoryContent = string.Empty;   // E.g. WebRequestMethods.Ftp.ListDirectory "NexusDevTest.txt\r\narchive\r\n"
                        directoryContent = reader.ReadToEnd();
                        reader.Close();
                        ftpDirectoryContent = FtpDirectoryContent.ParseLines(directoryContent, true);
                    }
                    string directoryStatusDescription = string.Empty;   // E.g. "226 Directory send OK.\r\n"
                    directoryStatusDescription = response.StatusDescription;
                    response.Close();
                }
            }
            catch (Exception ex)
            {
                log.Trace("Exception during FTP directory listing: " + ex.Message + (ex.InnerException == null ? string.Empty :
                    ", Inner=\"" + ex.InnerException.Message + "\""));
            }
            return ftpDirectoryContent;
        }
    }
}
