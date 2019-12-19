using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace OOSCommon.Import
{
    public class PipeImport : IOOSImportReported
    {
        public const string signaturePipe = "|upc"; // lower case required here

        public OOSCommon.IOOSLog logging { get; set; }
        public OOSCommon.DataContext.STORE store { get; set; }
        public DateTime? scanDate { get; set; }
        public bool isValidationMode { get; set; }

        public int recordCount { get; set; }
        public int lineCount { get; set; }

        public OOSCommon.VIM.IVIMRepository vimRepository { get; set; }
        public string oosEFConnectionString { get; set; }
        public OOSCommon.Movement.IMovementRepository movementRepository { get; set; }

        public enum eHeader : int { None = 0, UPC = 1, ScanDate = 2, StoreAbbreviation = 3, StorePS_BU = 4 }
        public Dictionary<string, eHeader> headerColumnTitles = new Dictionary<string, eHeader>
        {
            {"upc", eHeader.UPC},
            {"upc number", eHeader.UPC}
        };

        public OOSUpdateReported outputData
        {
            get
            {
                if (_outputData == null)
                    _outputData = new OOSUpdateReported(store, isValidationMode, logging, vimRepository, oosEFConnectionString, movementRepository);
                return _outputData;
            }
            set { _outputData = value; }
        } OOSUpdateReported _outputData = null;

        public PipeImport(DateTime scanDate,
            OOSCommon.DataContext.STORE store, bool isValidationMode,
            OOSCommon.IOOSLog logging,
            OOSCommon.VIM.IVIMRepository vimRepository, string oosEFConnectionString,
            OOSCommon.Movement.IMovementRepository movementRepository)
        {
            this.scanDate = scanDate;
            this.store = store;
            this.isValidationMode = isValidationMode;
            this.logging = logging;
            this.vimRepository = vimRepository;
            this.oosEFConnectionString = oosEFConnectionString;
            this.movementRepository = movementRepository;

            this.recordCount = 0;
            this.lineCount = 0;
        }

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
                            if (line.ToLower().Contains(signaturePipe))
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
            this.recordCount = 0;
            this.lineCount = 0;
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
                    while (ImportNextBatch(filePath, fileStream))
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
        protected bool ImportNextBatch(string filePath, StreamReader fileStream)
        {
            bool isOk = scanDate.HasValue;
            int? columnUPC = null;
            string[] fields = null;
            if (isOk)
            {
                // Get heading line
                isOk = false;
                while (!fileStream.EndOfStream)
                {
                    string line = fileStream.ReadLine();
                    ++this.lineCount;
                    if (line.Length > 1 && line[0] == '|')
                    {
                        fields = line.Split(new char[] { '|' });
                        isOk = true;
                        break;
                    }
                }
            }
            // Get heading positions
            if (isOk)
            {
                for (int ix = 0; ix < fields.Length; ++ix)
                {
                    string fieldLower = fields[ix].ToLower();
                    if (headerColumnTitles.Keys.Contains(fieldLower))
                    {
                        switch (headerColumnTitles[fieldLower])
                        {
                            case eHeader.UPC:
                                columnUPC = ix;
                                break;
                        }
                    }
                }
                isOk = columnUPC.HasValue;
            }
            if (isOk && outputData.BeginBatch(scanDate.Value))
            {
                var upcs = GetUPCs(fileStream, columnUPC, filePath);
                outputData.WriteUPCs(upcs);
            }
            return isOk;
        }

        private List<string> GetUPCs(StreamReader fileStream, int? columnUPC, string filePath)
        {
            string[] fields;
            List<string> upcs = new List<string>();
            while (!fileStream.EndOfStream)
            {
                string line = fileStream.ReadLine();
                ++this.lineCount;
                if (line.Length > 1 && line[0] == '|')
                {
                    ++this.recordCount;
                    fields = line.Split(new char[] { '|' });
                    if (columnUPC.Value < fields.Length)
                    {
                        string upc = fields[columnUPC.Value];

                        logging.Info(string.Format("Raw Scan data: UPC='{0}'", upc));

                        if (UpcSpecification.IsSatisfiedBy(upc))
                        {
                            upcs.Add(UpcSpecification.FormValid(new List<string>{upc}).First());
                            continue;
                        }
                        logging.Warn("Problem with upc. Error=" + UpcSpecification.GetUpcCheckValue(upc) + ", UPC=\"" + upc + "\", file=\'" + filePath + "\"line=" + lineCount);
                    }
                }
            }
            return upcs;
        }

        public DateTime? DateFromFilename(string filePath)
        {
            DateTime? result = null;
            string fileName = System.IO.Path.GetFileName(filePath);
            // Get date from filename ... Try <anything else><mmddyy>['.' <anything else>]
            {
                int useLength = fileName.IndexOf('.');
                if (useLength < 0)
                    useLength = fileName.Length;
                if (useLength >= 6)
                {
                    int month = 1;
                    if (int.TryParse(fileName.Substring(useLength - 6 + 0, 2), out month))
                    {
                        int day = 1;
                        if (int.TryParse(fileName.Substring(useLength - 6 + 2, 2), out day))
                        {
                            int year = 1;
                            if (int.TryParse(fileName.Substring(useLength - 6 + 4, 2), out year))
                                result = new DateTime(year + 2000, month, day); 
                        }
                    }
                }
            }
            return result;
        }

    }

}
