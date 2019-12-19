using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

[assembly: System.Runtime.CompilerServices.InternalsVisibleTo("OOSImport.Test")]
namespace OOSImport
{
    public class ImportBatch
    {
        public enum OOSFormat : int { None = 0, WIMP = 1, Tagnetics = 2, UPC = 3 }

        public string[] args { get; set; }
        public int currentArg { get; set; }
        public OOSCommon.IOOSLog logging { get; set; }
        public bool isValidationMode { get; set; }
        public OOSCommon.DataContext.STORE currentStore { get; set; }
        public OOSFormat currentFormat { get; set; }
        public DateTime? currentScanDate { get; set; }

        public OOSCommon.DataContext.OOSEntities db
        {
            get
            {
                if (_db == null)
                    _db = new OOSCommon.DataContext.OOSEntities(Program.oosEFConnectionString);
                return _db;
            }
            set { _db = value; }
        } public OOSCommon.DataContext.OOSEntities _db = null;

        public ImportBatch(string[] args, bool isValidationMode, OOSCommon.IOOSLog logging)
        {
            this.args = args;
            this.isValidationMode = isValidationMode;
            this.currentArg = 0;
            this.logging = logging;
            this.currentStore = null;
            this.currentFormat = OOSFormat.None;
            this.currentScanDate = (DateTime?)null;
        }

        public ImportBatch(string formatText, string dateText, string storeText, string fileName,
            bool isValidationMode, OOSCommon.IOOSLog logging)
        {
            List<string> args = new List<string>();
            args.Add("/f:" + formatText);
            args.Add("/d:" + dateText);
            args.Add("/s:" + storeText);
            args.Add(fileName);
            this.args = args.ToArray();
            this.isValidationMode = isValidationMode;
            this.currentArg = 0;
            this.logging = logging;
            this.currentStore = null;
            this.currentFormat = OOSFormat.None;
            this.currentScanDate = (DateTime?)null;
        }

        /// <summary>
        /// Advance through zero or more options
        /// </summary>
        private void InterpretOptions()
        {
            while (this.currentArg < this.args.Length && this.args[this.currentArg].StartsWith("/"))
            {
                string currentOption = this.args[this.currentArg++];
                string option = string.Empty;
                string value = string.Empty;
                if (currentOption.Length > 1)
                {
                    option = currentOption.Substring(1, 1);
                    int ix = currentOption.IndexOf(':');
                    if (ix > 0 && currentOption.Length > (ix + 1))
                        value = currentOption.Substring(ix + 1);
                }
                switch (option.ToLower())
                {
                    case "d":   // Format (validated on program start)
                        this.currentScanDate = (DateTime?)null;
                        if (value.Length > 0)
                        {
                            DateTime localValue = DateTime.Now;
                            if (DateTime.TryParse(value, out localValue))
                                this.currentScanDate = localValue;
                        }
                        this.logging.Log(NLog.LogLevel.Info, "Scan Date=" +
                            (this.currentScanDate.HasValue ? this.currentScanDate.Value.ToShortDateString() : "(not set)"));
                        break;
                    case "f":   // Format (validated on program start)
                        if (value.Length > 0)
                        {
                            switch (value.Substring(0, 1).ToLower())
                            {
                                case "t":
                                    this.currentFormat = OOSFormat.Tagnetics;
                                    break;
                                case "u":
                                    this.currentFormat = OOSFormat.UPC;
                                    break;
                                case "w":
                                    this.currentFormat = OOSFormat.WIMP;
                                    break;
                                default:
                                    this.currentFormat = OOSFormat.None;
                                    break;
                            }
                        }
                        this.logging.Log(NLog.LogLevel.Info, "Format=" + this.currentFormat);
                        break;
                    case "s":   // Store (validated on program start)
                        this.currentStore = null;
                        if (value.Length > 0)
                        {
                            int psBu = 0;
                            if (int.TryParse(value, out psBu))
                                this.currentStore = (from s in this.db.STORE where s.PS_BU == value select s)
                                    .FirstOrDefault();
                            else
                                this.currentStore =
                                    (from s in this.db.STORE
                                     where s.STORE_ABBREVIATION.Equals(value, StringComparison.OrdinalIgnoreCase)
                                     select s)
                                    .FirstOrDefault();
                        }
                        this.logging.Log(NLog.LogLevel.Info, "Store=" +
                            (this.currentStore == null ? "(not set)" :
                            this.currentStore.STORE_ABBREVIATION + "/" + this.currentStore.PS_BU));
                        break;
                }
            }
        }

        /// <summary>
        /// Import next file with current options and any options in the queue
        /// </summary>
        /// <returns></returns>
        public bool ImportNextFile()
        {
            InterpretOptions();
            if (currentArg < args.Length)
            {
                string currentFile = args[currentArg++];
                logging.Log(NLog.LogLevel.Info, "Import: format=" + this.currentFormat +
                    ", file='" + currentFile + "'");
                OOSCommon.Import.IOOSImportReported import = null;
                switch (this.currentFormat)
                {
                    case OOSFormat.WIMP:
                        if (this.currentStore != null && this.currentFormat != OOSFormat.None)
                            import = new OOSCommon.Import.WIMPImport(currentStore, isValidationMode, logging, Program.vimRepository, Program.oosEFConnectionString, Program.movementRepository);
                        break;
                    case OOSFormat.Tagnetics:
                        if (this.currentStore != null && this.currentFormat != OOSFormat.None && currentScanDate.HasValue)
                            import = new OOSCommon.Import.TagneticsImport(currentScanDate, currentStore, isValidationMode, logging, Program.vimRepository, Program.oosEFConnectionString, Program.movementRepository);
                        break;
                    case OOSFormat.UPC:
                        if (this.currentStore != null && this.currentFormat != OOSFormat.None && currentScanDate.HasValue)
                            import = new OOSCommon.Import.UPCImport(currentScanDate, currentStore, isValidationMode, logging, Program.vimRepository, Program.oosEFConnectionString, Program.movementRepository);
                        break;
                }
                if (import == null)
                    logging.Log(NLog.LogLevel.Error, "File not imported");
                else
                    import.Import(currentFile);
            }
            return currentArg < args.Length;
        }

    }
}
