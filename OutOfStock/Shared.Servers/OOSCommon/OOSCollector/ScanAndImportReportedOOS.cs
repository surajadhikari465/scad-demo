using System;
using System.Collections.Generic;

namespace OOSCommon.OOSCollector
{
    public class ScanAndImportReportedOOS
    {
        public IScanner scanner { get; set; }
        public OOSCommon.IOOSLog log { get; set; }
        public bool isValidationMode { get; set; }
        public OOSCommon.VIM.IVIMRepository vimRepository { get; set; }
        public string oosEFConnectionString { get; set; }
        public OOSCommon.Movement.IMovementRepository movementRepository { get; set; }

        public enum ImportFormats : int { WIMP, Tagnetics, Pipe }

        public Dictionary<ImportFormats, OOSCommon.Import.IOOSImportReported> dicImporters
        {
            get
            {
                if (_dicImporters == null)
                {
                    _dicImporters = new Dictionary<ImportFormats, OOSCommon.Import.IOOSImportReported>();
                    _dicImporters.Add(ImportFormats.WIMP,
                            new OOSCommon.Import.WIMPImport((OOSCommon.DataContext.STORE)null,
                                isValidationMode, log, vimRepository, oosEFConnectionString,
                                movementRepository)
                        );
                    _dicImporters.Add(ImportFormats.Tagnetics,
                            new OOSCommon.Import.TagneticsImport(DateTime.Now,
                                (OOSCommon.DataContext.STORE)null,
                                isValidationMode, log, vimRepository, oosEFConnectionString,
                                movementRepository)
                        );
                    _dicImporters.Add(ImportFormats.Pipe,
                            new OOSCommon.Import.PipeImport(DateTime.Now,
                                (OOSCommon.DataContext.STORE)null,
                                isValidationMode, log, vimRepository, oosEFConnectionString,
                                movementRepository)
                        );
                }
                return _dicImporters;
            }
            set { _dicImporters = value; }
        } Dictionary<ImportFormats, OOSCommon.Import.IOOSImportReported> _dicImporters = null;

        public ScanAndImportReportedOOS(IScanner scanner, OOSCommon.IOOSLog log, bool isValidationMode,
            OOSCommon.VIM.IVIMRepository vimRepository, string oosEFConnectionString,
            OOSCommon.Movement.IMovementRepository movementRepository)
        {
            this.scanner = scanner;
            this.log = log;
            this.isValidationMode = isValidationMode;
            this.vimRepository = vimRepository;
            this.oosEFConnectionString = oosEFConnectionString;
            this.movementRepository = movementRepository;
        }

        public void DoScanAndImport()
        {
            log.Trace("Enter");
            for (string region = scanner.GetRegionFirst();
                !string.IsNullOrWhiteSpace(region);
                region = scanner.GetRegionNext())
            {
                log.Trace("Region: " + region);
                for (OOSCommon.DataContext.STORE store = scanner.GetStoreFirst(region);
                    store != null;
                    store = scanner.GetStoreNext())
                {
                    string storeAbbreviation = store.STORE_ABBREVIATION.ToUpper();
                    log.Trace("Store: " + storeAbbreviation);

                    if (TurnOffStoreImportPolicy.ShouldTurnOff(storeAbbreviation))
                    {
                        log.Trace(string.Format("DoScanAndImport() {0} store not accepting reported OOS items from import", store.STORE_NAME));
                        continue;
                    }

                    for (string file = scanner.GetFileFirst(region, storeAbbreviation);
                        !string.IsNullOrWhiteSpace(file);
                        file = scanner.GetFileNext())
                    {
                        log.Trace("File: Name=\"" + file + "\", Start");
                        try
                        {
                            // Prefer date in filename of present
                            DateTime scanDate = System.IO.File.GetCreationTime(file);
                            {
                                DateTime? eventDateFromFileName =
                                    OOSCommon.Utility.FindDateInString(System.IO.Path.GetFileName(file));
                                if (eventDateFromFileName.HasValue)
                                    scanDate = eventDateFromFileName.Value;
                            }
                            // Check Format
                            ImportFormats? format = CheckFileFormat(file);

                            bool hasError = false;
                            // Import file
                            ImportFile(format, store, scanDate, file);
                            // Finish up by moving or deleting

                            log.Trace("File: Name=\"" + file + "\", Imported, Scan date=" +
                                scanDate.ToString("MM/dd/yy HH:mm") + ", Format=" +
                                (format.HasValue ? format.Value.ToString() : "?"));
                            scanner.SetFileDone(file, hasError, region, storeAbbreviation);
                        }
                        catch (Exception ex)
                        {
                            log.Warn("Exception: Message=\"" + ex.Message + "\"" + (ex.InnerException == null ? string.Empty :
                                ", Inner=\"" + ex.InnerException.Message + "\""));
                        }
                    }
                }
            }
            log.Trace("Exit");
        }


        public ImportFormats? CheckFileFormat(string filename)
        {
            ImportFormats? result = null;
            foreach (KeyValuePair<ImportFormats, OOSCommon.Import.IOOSImportReported> item in dicImporters)
            {
                OOSCommon.Import.OOSImportIsMyFormat formatMatch = item.Value.IsMyFormat(filename);
                switch (formatMatch)
                {
                    case OOSCommon.Import.OOSImportIsMyFormat.Yes:
                    case OOSCommon.Import.OOSImportIsMyFormat.Maybe:
                        result = item.Key;
                        break;
                    case OOSCommon.Import.OOSImportIsMyFormat.No:
                    case OOSCommon.Import.OOSImportIsMyFormat.NotDetermined:
                    case OOSCommon.Import.OOSImportIsMyFormat.NotSupported:
                        break;
                }
                if (formatMatch == OOSCommon.Import.OOSImportIsMyFormat.Yes)
                    break;
            }
            return result;
        }

        public void ImportFile(ImportFormats? format, OOSCommon.DataContext.STORE store, DateTime scanDate, string file)
        {
            log.Trace("Enter: File=\"" + file + "\"");
            OOSCommon.Import.IOOSImportReported importer = null;
            if (format.HasValue)
            {
                switch (format.Value)
                {
                    case ImportFormats.Pipe:
                        importer = new OOSCommon.Import.PipeImport(scanDate, store, isValidationMode, log,
                            vimRepository, oosEFConnectionString, movementRepository);
                        break;
                    case ImportFormats.Tagnetics:
                        importer = new OOSCommon.Import.TagneticsImport(scanDate, store, isValidationMode, log,
                            vimRepository, oosEFConnectionString, movementRepository);
                        break;
                    case ImportFormats.WIMP:
                        importer = new OOSCommon.Import.WIMPImport(store, isValidationMode, log,
                            vimRepository, oosEFConnectionString, movementRepository);
                        break;
                }
            }
            if (importer != null)
                importer.Import(file);
            log.Trace("Exit");
        }

    }
}
