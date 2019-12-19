using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace OOSCommon.Import
{
    public class WIMPImport : IOOSImportReported
    {
        public OOSCommon.IOOSLog logging { get; set; }
        public OOSCommon.DataContext.STORE store { get; set; }
        public bool isValidationMode { get; set; }

        public int recordCount { get; set; }
        public int itemCount { get; set; }

        public OOSCommon.VIM.IVIMRepository vimRepository { get; set; }
        public string oosEFConnectionString { get; set; }
        public OOSCommon.Movement.IMovementRepository movementRepository;

        protected OOSUpdateReported outputData
        {
            get
            {
                if (_outputData == null)
                    _outputData = new OOSUpdateReported(store, isValidationMode, logging, vimRepository, oosEFConnectionString, movementRepository);
                return _outputData;
            }
            set { _outputData = value; }
        } OOSUpdateReported _outputData = null;

        public enum indexHeader : int { HR = 0, Batch = 1, Date = 12, Time = 14, StoreTeamOOS = 16, Last = 21 }
        public enum indexItem : int { R = 0, Batch = 1, UPC = 2, Last = 2 }
        public enum indexTerminator : int { T = 0, Batch = 1, RecordCountTotal = 2, RecordCountItem = 6, Last = 6 }

        // States an input type for ImportNextBatch
        protected enum importFileState : int { ScanningForHeader = 0, ReadingRecords = 1 };
        protected enum importFileRecordType : int { Unknown = 0, Header = 1, Record = 2, Terminator = 3 };

        public WIMPImport(OOSCommon.DataContext.STORE store, bool isValidationMode,
            OOSCommon.IOOSLog logging,
            OOSCommon.VIM.IVIMRepository vimRepository, string oosEFConnectionString,
            OOSCommon.Movement.IMovementRepository movementRepository)
        {
            this.store = store;
            this.isValidationMode = isValidationMode;
            this.logging = logging;
            this.vimRepository = vimRepository;
            this.oosEFConnectionString = oosEFConnectionString;
            this.movementRepository = movementRepository;
        }

        public const string signatureWIMP = "|HR|";

        public OOSImportIsMyFormat IsMyFormat(string filePath)
        {
            OOSImportIsMyFormat result = OOSImportIsMyFormat.No;
            System.IO.StreamReader fileStream = null;
            try
            {
                fileStream = new System.IO.StreamReader(filePath);
                if (fileStream != null)
                {
                    while (!fileStream.EndOfStream)
                    {
                        string line = fileStream.ReadLine();
                        if (line.Length > 1 && line[0] == '|')
                        {
                            if (line.Length >= signatureWIMP.Length && line.Substring(0, signatureWIMP.Length).Equals(signatureWIMP, StringComparison.OrdinalIgnoreCase))
                                result = OOSImportIsMyFormat.Yes;
                            break;
                        }
                    }
                }
            }
            finally
            {
                if (fileStream != null)
                    fileStream.Close();
            }
            return result;
        }

        /// <summary>
        /// Import all sets of Out of Stock information from the file
        /// </summary>
        /// <param name="filePath"></param>
        /// <returns></returns>
        public bool Import(string filePath)
        {
            bool isOk = false;
            System.IO.StreamReader fileStream = null;
            try
            {
                try { fileStream = new System.IO.StreamReader(filePath); }
                catch (Exception ex)
                {
                    logging.Warn("File \"" + filePath + "\" open failed with \"" + ex.Message + (ex.InnerException == null ? string.Empty :
                    ", Inner=\"" + ex.InnerException.Message + "\""));
                }
                if (fileStream != null)
                {
                    while (ImportNextBatch(fileStream))
                        ;
                }
            }
            finally
            {
                if (fileStream != null)
                    fileStream.Close();
            }
            return isOk;
        }

        /// <summary>
        /// Import the next batch of Out of Stock information from the stream
        /// </summary>
        /// <param name="fileStream"></param>
        /// <returns></returns>
        protected bool ImportNextBatch(System.IO.StreamReader fileStream)
        {
            bool isOk = false;
            int lineCurrent = 0;
            OOSUpdateReported wimpData = null;
            importFileState fileStateCurrent = importFileState.ScanningForHeader;
            List<string> upcs = new List<string>();
            while (!fileStream.EndOfStream)
            {
                string[] content = new string[] { };
                string line = fileStream.ReadLine();
                ++lineCurrent;
                if (!string.IsNullOrWhiteSpace(line))
                    content = PSA.ImportNextLine(line);
                importFileRecordType recordTypeCurrent = importFileRecordType.Unknown;
                if (content.Length > 0)
                {
                    switch (content[0].ToLower())
                    {
                        case "hr":
                            recordTypeCurrent = importFileRecordType.Header;
                            break;
                        case "r":
                            recordTypeCurrent = importFileRecordType.Record;
                            break;
                        case "t":
                            recordTypeCurrent = importFileRecordType.Terminator;
                            break;
                    }
                }
                switch (fileStateCurrent)
                {
                    case importFileState.ScanningForHeader:
                        switch (recordTypeCurrent)
                        {
                            case importFileRecordType.Header:
                                // Create a new header and go
                                wimpData = new OOSUpdateReported(store, isValidationMode, logging, vimRepository, oosEFConnectionString, movementRepository);
                                if (!ValidateHeader(content))
                                    isOk = false;
                                fileStateCurrent = importFileState.ReadingRecords;
                                break;
                            case importFileRecordType.Record:
                            case importFileRecordType.Terminator:
                                logging.Warn("Line " + lineCurrent + " has mis-placed records in file.");
                                break;
                        }
                        break;
                    case importFileState.ReadingRecords:
                        switch (recordTypeCurrent)
                        {
                            case importFileRecordType.Header:
                                // Complete prior section
                                Complete(ref upcs);
                                // Log a warning
                                logging.Warn("Header encountered while reading items on line " + lineCurrent + ".");
                                // Create a new header and go
                                wimpData = new OOSUpdateReported(store, isValidationMode, logging, vimRepository, oosEFConnectionString, movementRepository);
                                if (!ValidateHeader(content))
                                    isOk = false;
                                fileStateCurrent = importFileState.ReadingRecords;
                                break;
                            case importFileRecordType.Record:
                                // Add record to database
                                if (!ValidateItem(content, ref upcs))
                                    isOk = false;
                                break;
                            case importFileRecordType.Terminator:
                                // Complete prior section
                                if (!ValidateEndOfSection(content))
                                    isOk = false;
                                Complete(ref upcs);
                                wimpData = null;
                                fileStateCurrent = importFileState.ScanningForHeader;
                                break;
                            default:
                                // Warn about noise within records
                                logging.Warn("Line " + lineCurrent + " has noise within record.");
                                break;
                        }
                        break;
                }
            }
            return isOk;
        }

        /// <summary>
        /// Start a reported Out of Stock batch for a given store
        /// </summary>
        /// <param name="record"></param>
        /// <returns></returns>
        protected bool ValidateHeader(string[] record)
        {
            bool isOk = false;
            this.recordCount = 1;
            this.itemCount = 0;
            DateTime dtScan = DateTime.Now;
            // Require a full record with the right type
            if (record.Length >= (int)indexHeader.Last &&
                record[(int)indexHeader.HR].Equals("HR", StringComparison.OrdinalIgnoreCase))
            {
                isOk = DateTime.TryParse(
                    record[(int)indexHeader.Date] + " " + record[(int)indexHeader.Time], out dtScan);
            }
            if (isOk)
                outputData.BeginBatch(dtScan);
            logging.Info("Start of import batch: Ok=" + isOk.ToString());
            return isOk;
        }

        /// <summary>
        /// Add reported Out of Stock item to database
        /// </summary>
        /// <param name="record"></param>
        /// <returns></returns>
        protected bool ValidateItem(string[] record, ref List<string> upcs)
        {
            bool isOk = false;
            ++this.recordCount;
            ++this.itemCount;
            // Require a full record with the right type
            if (record.Length >= (int)indexItem.Last &&
                record[(int)indexItem.R].Equals("R", StringComparison.OrdinalIgnoreCase))
            {
                string upc = record[(int)indexItem.UPC];

                logging.Info(string.Format("Raw Scan data: UPC='{0}'", upc));

                isOk = (upc.Length == 13);
                if (isOk)
                {
                    // WIMP UPC to VIM UPC
                    upc = "0" + upc.Substring(0, upc.Length - 1);
                    upcs.Add(upc);
                    //outputData.WriteUPC(upc);
                }
            }
            return isOk;
        }

        /// <summary>
        /// Process terminator record
        /// </summary>
        /// <param name="record"></param>
        /// <returns></returns>
        protected bool ValidateEndOfSection(string[] record)
        {
            bool isOk = false;
            ++this.recordCount;
            // Require a full record with the right type
            if (record.Length >= (int)indexTerminator.Last &&
                record[(int)indexTerminator.T].Equals("T", StringComparison.OrdinalIgnoreCase))
            {
                isOk = true;
                // Nothing else to do
            }
            logging.Warn("End of import batch: Ok=" + isOk.ToString() +
                ", record count=" + this.recordCount + ", item count=" + this.itemCount);
            return isOk;
        }

        /// <summary>
        /// Complete a batch
        /// </summary>
        protected void Complete(ref List<string> upcs)
        {
            if (upcs.Count > 0)
            {
                outputData.WriteUPCs(upcs);
                upcs = new List<string>();
            }
        }

    }

}
