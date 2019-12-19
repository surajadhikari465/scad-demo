using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace OOSCommon.Import
{
    public class TagneticsImport : IOOSImportReported
    {
        public OOSCommon.IOOSLog logging { get; set; }
        public OOSCommon.DataContext.STORE store { get; set; }
        public DateTime? scanDate { get; set; }
        public bool isValidationMode { get; set; }

        public int recordCount { get; set; }
        public int itemCount { get; set; }

        public OOSCommon.VIM.IVIMRepository vimRepository { get; set; }
        public string oosEFConnectionString { get; set; }
        public OOSCommon.Movement.IMovementRepository movementRepository { get; set; }

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

        public TagneticsImport(DateTime? scanDate, OOSCommon.DataContext.STORE store,
            bool isValidationMode, OOSCommon.IOOSLog logging,
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
        }

        public OOSImportIsMyFormat IsMyFormat(string filePath)
        {
            // TODO: Implement
            return OOSImportIsMyFormat.NotSupported;
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
        protected bool ImportNextBatch(string filePath, System.IO.StreamReader fileStream)
        {
            bool isOk = false;
            this.recordCount = 1;
            this.itemCount = 0;
            if (scanDate.HasValue)
            {
                if (outputData.BeginBatch(scanDate.Value))
                {
                    // Get upc's
                    List<string> upcs = new List<string>();
                    while (!fileStream.EndOfStream)
                    {
                        ++this.recordCount;
                        ++this.itemCount;
                        string upc = fileStream.ReadLine().Trim();

                        logging.Info(string.Format("Raw Scan data: UPC='{0}'", upc));

                        if (UpcSpecification.IsSatisfiedBy(upc))
                        {
                            upcs.Add(UpcSpecification.FormValid(new List<string>{upc}).First());
                            continue;
                        }
                        logging.Warn("Problem with upc. Error=" + UpcSpecification.GetUpcCheckValue(upc) + ", UPC=\"" + upc + "\", file=\'" + filePath + "\"line=" + itemCount);
                    }
                    outputData.WriteUPCs(upcs);
                }
            }
            return isOk;
        }

    }

}
