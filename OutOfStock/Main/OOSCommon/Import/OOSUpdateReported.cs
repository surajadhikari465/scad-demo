using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Data.EntityClient;
using System.Data.Objects;
using System.Data.SqlClient;
using System.Globalization;
using System.Linq;
using System.ServiceModel;
using System.Text;
using OOSCommon.Movement;
using ProductDataBoundedContext;
using SharedKernel;
using StructureMap;

namespace OOSCommon.Import
{
    public class OOSUpdateReported : IOOSUpdateReported
    {
        public OOSCommon.DataContext.STORE store { get; set; }
        public bool isValidationMode { get; set; }
        public OOSCommon.IOOSLog logging { get; set; }
        public DateTime dtScan { get; set; }
        public OOSCommon.DataContext.REPORT_HEADER reportHeader { get; set; }
        public int recordCount { get; set; }
        public int itemCount { get; set; }

        public OOSCommon.VIM.IVIMRepository vimRepository { get; set; }
        public OOSCommon.Movement.IMovementRepository movementRepository { get; set; }
        public string oosEFConnectionString { get; set; }

        const string createdByName = "OOSImport";

        protected OOSCommon.DataContext.OOSEntities db
        {
            get
            {
                if (_db == null)
                    _db = new OOSCommon.DataContext.OOSEntities(oosEFConnectionString);
                return _db;
            }
            set { _db = value; }
        } public OOSCommon.DataContext.OOSEntities _db = null;

        public OOSUpdateReported(OOSCommon.DataContext.STORE store, bool isValidationMode,
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
            this.reportHeader = null;
            this.dtScan = DateTime.Now;
            this.recordCount = 0;
            this.itemCount = 0;
        }

        /// <summary>
        /// Start a reported Out of Stock batch for a given store
        /// </summary>
        /// <param name="scanDate"></param>
        /// <returns></returns>
        public bool BeginBatch(DateTime scanDate)
        {
            logging.Trace(string.Format("BeginBatch(): Enter on scanDate='{0}'", scanDate));
            ValidateScanDate(scanDate);
            bool isOk = true;
            try
            {
                this.reportHeader = null;
                this.recordCount = 1;
                this.itemCount = 0;

                if (ReportHeaderExistsFor(scanDate))
                {
                    isOk = false;
                }
                else
                {
                    SetScanDateTime(scanDate);
                    CreateReportHeaderInDB();                    
                }
            }
            catch (Exception ex)
            {
                logging.Warn(string.Format("BeginBatch({0}) Exception: Message=\"{1}\"{2}", scanDate, ex.Message, 
                    (ex.InnerException == null ? string.Empty : ", Inner=\"" + ex.InnerException.Message + "\"" + ", Stack=" + ex.StackTrace)));
                RemoveReportHeaderFromDB();
                throw;
            }
            logging.Trace("BeginBatch(): Exit");
            return isOk;
        }

        private void ValidateScanDate(DateTime scanDate)
        {
            if (!ScanDateValid(scanDate))
                throw new InvalidScanDateException(string.Format("ScanDate is not valid: scanDate='{0}'", scanDate));
        }

        private bool ScanDateValid(DateTime scanDate)
        {
            try
            {
                new System.Data.SqlTypes.SqlDateTime(scanDate);
                return true;
            }
            catch (System.Data.SqlTypes.SqlTypeException) {} // no need to do anything, this is the expected out of range error
            return false;

        }

        private bool ReportHeaderExistsFor(DateTime scanDate)
        {
            var id = db.REPORT_HEADER.Where(rh => rh.STORE_ID == store.ID && rh.CREATED_DATE == scanDate).Select(rh => rh.ID).FirstOrDefault();
            if (id > 0)
            {
                logging.Warn("REPORT HEADER already exists: Id=" + id + ", Store Id=" + store.ID + ", Date=" + scanDate.ToString("yyyy/MM/dd HH:mm:ss"));
                return true;
            }
            return false;
        }

        private void SetScanDateTime(DateTime dateTime)
        {
            dtScan = dateTime;
        }

        private void CreateReportHeaderInDB()
        {
            this.reportHeader = new OOSCommon.DataContext.REPORT_HEADER();
            this.reportHeader.CREATED_BY = createdByName;
            try { reportHeader.CREATED_BY = Environment.UserDomainName + "/" + Environment.UserName; }
            catch (Exception) { }
            this.reportHeader.CREATED_DATE = this.dtScan;
            this.reportHeader.LAST_UPDATED_DATE = DateTime.Now;
            this.reportHeader.STORE_ID = store.ID;   // Should be but is not PS_BU
            SaveReportHeader();
        }

        private void SaveReportHeader()
        {
            if (isValidationMode || reportHeader == null) return;

            db.REPORT_HEADER.AddObject(reportHeader);
            db.SaveChanges();
        }

        private bool WriteUPC(string upc, IRetailItem productData, decimal? movementUnits)
        {
            bool isOk = (this.reportHeader != null && this.store != null);
            try
            {
                if (isOk)
                {
                    OOSCommon.DataContext.REPORT_DETAIL reportDetail = new OOSCommon.DataContext.REPORT_DETAIL();
                    reportDetail.REPORT_HEADER_ID = this.reportHeader.ID;
                    reportDetail.UPC = upc;
                    reportDetail.NOTES = string.Empty;
                    try { reportDetail.CREATED_BY = Environment.UserDomainName + "/" + Environment.UserName; }
                    catch (Exception) { }
                    reportDetail.CREATED_DATE = DateTime.Now;
                    reportDetail.SCAN_DATE = dtScan;

                    // From VIM
                    reportDetail.VENDOR_KEY = productData.VendorKey;
                    reportDetail.VIN = productData.VendorItemNumber;
                    reportDetail.CASE_SIZE = productData.CaseSize;
                    reportDetail.PS_TEAM = productData.TeamName;
                    reportDetail.PS_SUBTEAM = productData.SubTeamName;
                    reportDetail.EFF_PRICE = Convert.ToDecimal(productData.Price);
                    reportDetail.EFF_PRICETYPE = productData.PriceType;
                    reportDetail.EFF_COST = Convert.ToDecimal(productData.Cost);

                    reportDetail.BRAND = productData.Brand;
                    reportDetail.BRAND_NAME = productData.BrandName;
                    reportDetail.LONG_DESCRIPTION = productData.LongDescription;
                    reportDetail.ITEM_SIZE = productData.Size;
                    reportDetail.ITEM_UOM = productData.UOM;
                    reportDetail.CATEGORY_NAME = productData.CategoryName;
                    reportDetail.CLASS_NAME = productData.ClassName;

                    // From another source
                    reportDetail.MOVEMENT = movementUnits;

                    if (!isValidationMode)
                    {
                        db.REPORT_DETAIL.AddObject(reportDetail);
                        db.SaveChanges();
                    }
                }
            }
            catch (Exception ex)
            {
                logging.Warn("Exception: Message=\"" + ex.Message + "\"" + (ex.InnerException == null ? string.Empty :
                    ", Inner=\"" + ex.InnerException.Message + "\"" + ", Stack=" + ex.StackTrace));
                isOk = false;
            }
            return isOk;
        }

        
        /// <summary>
        /// Add reported Out of Stock item to database
        /// </summary>
        /// <param name="record"></param>
        /// <returns></returns>
        public bool WriteUPCs(List<string> upcs)
        {
            if (!StateValid()) return false;
            
            logging.Debug("Movement data: Start, Count=" + upcs.Count +
                ", Date=" + dtScan.ToString("yyyy/MM/dd") + ", Store=" + store.PS_BU);
            DateTime dtStart = DateTime.Now;

            var dicMovement = GetMovementUnits(upcs, store.PS_BU, dtScan);
            var productDataMap = GetProductMapFor(upcs);
            
            logging.Debug("Movement data: Done, Count=" + upcs.Count + ", Resolved=" +
                dicMovement.Count + ", Elapsed seconds=" + DateTime.Now.Subtract(dtStart).TotalSeconds);

            ReportProductDataFor(upcs, productDataMap, dicMovement);
            return true;
        }

        private Dictionary<string, decimal?> GetMovementUnits(IEnumerable<string> upcs, string storeNumber, DateTime scanDate)
        {
            var msg = string.Empty;
            const int nTries = 5;
            for (var i = 0; i < nTries; i++)
            {
                try
                {
                    return movementRepository.GetMovementUnits(upcs.ToList(), storeNumber, scanDate);
                }
                catch (Exception ex)
                {
                    msg = new StringBuilder("Exception(").Append(i + 1).Append(" out of ").Append(nTries).Append(" tries").Append("): Message=\"").Append(ex.Message).Append("\"").Append(ex.InnerException == null ? string.Empty : ", Inner=\"" + ex.InnerException.Message + "\"").ToString();
                    logging.Error(msg);

                }
            }
            RemoveReportHeaderFromDB();
            throw new MovementDataReadException(msg);
        }

        private Dictionary<string, IRetailItem> GetProductMapFor(IEnumerable<string> upcs)
        {
            try
            {
                return GetProductMap(upcs);
            }
            catch(Exception exception)
            {
                logging.Error(string.Format("GetProductMapFor(): {0}, Stack Trace={1}", exception.Message, exception.StackTrace));
                RemoveReportHeaderFromDB();
                throw new ProductDataReadException(exception.Message);
            }
        }

        private Dictionary<string, IRetailItem> GetProductMap(IEnumerable<string> upcs)
        {
            var productDataService = ObjectFactory.GetInstance<ProductDataService>();
            var retailItems = productDataService.QueryProductData(upcs, store.STORE_ABBREVIATION);
            var productDataMap = retailItems.ToDictionary(p => p.UPC.Code, q => q);
            return productDataMap;
        }

        private bool StateValid()
        {
            return (reportHeader != null && store != null);
        }

        private int ReportProductDataFor(IEnumerable<string> upcs, Dictionary<string, IRetailItem> retailItemMap, Dictionary<string, decimal?> dicMovement)
        {
            var reported = 0;
            foreach (string upc in upcs)
            {
                IRetailItem retailItem = (retailItemMap.Keys.Contains(upc) ? retailItemMap[upc] : null);
                bool isOkInner = (retailItem != null);
                if (retailItem == null)
                    logging.Info("No VIM data: UPC='" + upc + "', STORE=" + store.STORE_ABBREVIATION + "/" + store.PS_BU);
                decimal? movementUnits = (dicMovement.Keys.Contains(upc) ? dicMovement[upc] : (decimal?)null);
                if (!movementUnits.HasValue)
                    logging.Info("No movement data: UPC='" + upc + "'");
                if (!isOkInner) continue;
                if (WriteUPC(upc, retailItem, movementUnits)) 
                    reported = reported + 1;
            }
            if (reported == 0)
            {
                RemoveReportHeaderFromDB();
                throw new NoProductDataForAnyScanException(string.Format("No product data saved for any of the scans at the store='{0}'", store.STORE_ABBREVIATION));
            }
            return reported;
        }

        
        private void RemoveReportHeaderFromDB()
        {
            if (isValidationMode || reportHeader == null) return;
            
            db.REPORT_HEADER.DeleteObject(reportHeader);
            db.SaveChanges();
            reportHeader = null;
        }


    }
}
