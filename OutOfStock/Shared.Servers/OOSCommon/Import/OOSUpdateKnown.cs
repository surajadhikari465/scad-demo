using System;
using System.Linq;
//using Magnum;
using OOSCommon.DataContext;
using OOSCommon.VIM;

namespace OOSCommon.Import
{
    public class OOSUpdateKnown : IOOSUpdateKnown
    {
        private bool isValidationMode { get; set; }
        private OOSCommon.IOOSLog logging { get; set; }
        private OOSCommon.DataContext.KNOWN_OOS_HEADER knownOOSHeader { get; set; }
        private ICreateDisposableEntities entityFactory;

        private OOSCommon.VIM.IVIMRepository vimRepository { get; set; }
        private string oosEFConnectionString { get; set; }

        private const string createdByName = "OOSImport";

        public OOSUpdateKnown(bool validate, IOOSLog logger, ICreateDisposableEntities entityFactory, IVIMRepository vimRepository, string connectionString)
        {
            this.isValidationMode = validate;
            this.logging = logger;
            this.vimRepository = vimRepository;
            this.oosEFConnectionString = connectionString;
            this.knownOOSHeader = null;
            this.entityFactory = entityFactory;
        }

        public bool BeginBatch(DateTime uploadDate, IOOSImportKnown importKnown)
        {
            logging.Trace("Enter");
            knownOOSHeader = null;

            using (var dbContext = entityFactory.New())
            {
                int? id = dbContext.KNOWN_OOS_HEADER.Where(kh => kh.CREATED_DATE == uploadDate).Select(kh => kh.ID).FirstOrDefault();
                if (id > 0)
                {
                    logging.Warn("KNOWN OOS HEADER already exists: Id=" + id.Value + ", Date=" + uploadDate.ToString("yyyy/MM/dd HH:mm:ss"));
                    return false;
                }
            }

            using (var dbContext = entityFactory.New())
            {
                knownOOSHeader = DefaultHeader(uploadDate);
                if (!this.isValidationMode)
                {
                    dbContext.KNOWN_OOS_HEADER.AddObject(knownOOSHeader);
                    dbContext.SaveChanges();
                }
                foreach (OOSKnownVendorRegionMap item in importKnown.vendorRegionMap)
                {
                    OOSKnownVendorRegionMap item1 = item;
                    int? regionId = dbContext.REGION.Where(r => r.REGION_ABBR.Equals(item1.region, StringComparison.OrdinalIgnoreCase)).Select(r => r.ID).FirstOrDefault();

                    // TODO -- check for redundant entries
                    var kom = DefaultVendorMapFrom(regionId.Value, item.vendor_key);
                    if (!this.isValidationMode)
                    {
                        dbContext.KNOWN_OOS_MAP.AddObject(kom);
                        dbContext.SaveChanges();
                    }
                }
            }
            
            logging.Trace("Exit");
            return true;
        }

        private KNOWN_OOS_HEADER DefaultHeader(DateTime uploadDate)
        {
            return new KNOWN_OOS_HEADER
            {
                CREATED_BY = CreatedBy(),
                CREATED_DATE = uploadDate,
                LAST_UPDATED_BY = createdByName,
                LAST_UPDATED_DATE = DateTime.Now,
            };
        }

        private KNOWN_OOS_MAP DefaultVendorMapFrom(int regionId, string vendorKey)
        {
            return new KNOWN_OOS_MAP
            {
                REGION_ID = regionId,
                KNOWN_OOS_HEADER_ID = knownOOSHeader.ID,
                VENDOR_KEY = vendorKey,
                CREATED_DATE = DateTime.Now,
                CREATED_BY = CreatedBy()
            };
        }

        private string CreatedBy()
        {
            try
            {
                return Environment.UserDomainName + "/" + Environment.UserName;
            }
            catch (Exception)
            {
                return createdByName;
            }
        }

        // writes known oos data to database.
        public bool WriteKnownOOS(IOOSImportKnown importKnown)
        {
            // removed Magnum.dll dependency
            // Guard.AgainstNull(knownOOSHeader);
            if (knownOOSHeader == null) {  throw new ArgumentNullException();}

            bool isOk = true;
            
            using(var dbContext = entityFactory.New())
            {
                foreach (OOSKnownItemData itemData in importKnown.itemData)
                {
                    var kod = new DataContext.KNOWN_OOS_DETAIL
                    {
                        REASON_ID = (int)itemData.reason_code,
                        SOURCE_ID = (int)eSources.VENDOR_COST_FILE
                    };
                    if (itemData.ps_bu.HasValue)
                    {
                        OOSKnownItemData data = itemData;
                        kod.STORE_ID = dbContext.STORE.Where(s => s.PS_BU.Equals(data.ps_bu.Value.ToString())).Select(s => s.ID).FirstOrDefault();
                    }
                    else
                        kod.STORE_ID = null;
                    kod.VIN = itemData.vin.ToString();
                    kod.UPC = itemData.name;
                    kod.START_DATE = itemData.start_date;
                    kod.END_DATE = itemData.end_date;
                    kod.PS_TEAM = itemData.team_name;
                    kod.PS_SUBTEAM = itemData.subteam_name;
                    kod.KNOWN_OOS_HEADER_ID = knownOOSHeader.ID;
                    kod.CREATED_DATE = DateTime.Now;
                    kod.CREATED_BY = CreatedBy();
                    if (!this.isValidationMode)
                    {
                        dbContext.KNOWN_OOS_DETAIL.AddObject(kod);
                        dbContext.SaveChanges();
                    }
                }
                return isOk;
            }
        }

        public bool Upload(IKnownUpload uploadDoc)
        {
            //write (known_oos_header and known_oos_map) then write known_oos_detail
            return BeginBatch(uploadDoc) && WriteKnownOOS(uploadDoc);
        }

        private bool BeginBatch(IKnownUpload uploadDoc)
        {
            // writes known oos HEADER / MAP records to database
            var knownImport = new OOSImportKnownUNFI { vendorRegionMap = uploadDoc.VendorRegionMap.ToList() };
            return BeginBatch(uploadDoc.UploadDate, knownImport);
        }

        private bool WriteKnownOOS(IKnownUpload uploadDoc)
        {
            // writes known oos DETAIL to database.
            var knownImport = new OOSImportKnownUNFI { itemData = uploadDoc.ItemData.ToList() };
            return WriteKnownOOS(knownImport);
        }
    }
}
