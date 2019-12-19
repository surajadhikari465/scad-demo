using System;
using System.Collections.Generic;
using System.Linq;
using OOSCommon;
using OOSCommon.DataContext;
using OOSCommon.Import;

namespace OOS.Model
{
    public class KnownUploadRepository : IKnownUploadRepository
    {
        private List<IKnownUpload> items = new List<IKnownUpload>();
        private ICreateDisposableEntities entityFactory;

        public KnownUploadRepository(ICreateDisposableEntities entityFactory)
        {
            this.entityFactory = entityFactory;
        }

        public void Insert(IKnownUpload upload)
        {
            if (For(upload) == null)
            {
                items.Add(upload);
                Save(upload);
            }
        }


        private IKnownUpload For(IKnownUpload upload)
        {
            if (!items.Any(p => (KnownUpload)p == (KnownUpload)upload))
            {
                var searched = Search(upload);
                if (searched != null)
                {
                    items.Add(searched);
                }
            }
            return items.FirstOrDefault(p => (KnownUpload)p == (KnownUpload)upload);
        }

        private IKnownUpload Search(IKnownUpload upload)
        {
            using (var dbContext = entityFactory.New())
            {
                var header =
                    (from c in dbContext.KNOWN_OOS_HEADER where c.CREATED_DATE == upload.UploadDate select c).
                        FirstOrDefault();
                if (header == null) return null;
                var knownUpload = new KnownUpload(upload.UploadDate);
                var itemData = header.KNOWN_OOS_DETAIL;
                foreach (var item in itemData)
                {
                    var reasonCode = item.REASON_ID.ToString();
                    var startDate = item.START_DATE.HasValue ? item.START_DATE.Value.ToString() : DateTime.MinValue.ToString();
                    knownUpload.AddItem(new OOSKnownItemData(item.UPC, reasonCode, startDate, item.VIN, 
                                            item.ProductStatus, item.ExpirationDate.HasValue ? item.ExpirationDate.Value : DateTime.MaxValue));
                }
                var vendorMaps = header.KNOWN_OOS_MAP;
                foreach (var map in vendorMaps)
                {
                    knownUpload.AddVendorRegion(new OOSKnownVendorRegionMap(map.VENDOR_KEY+"-"+map.REGION.REGION_ABBR, map.REGION.REGION_ABBR, map.VENDOR_KEY));
                }
                return knownUpload;
            }
        }

        private void Save(IKnownUpload upload)
        {
            using (var context = entityFactory.New())
            {
                var known = new KNOWN_OOS_HEADER{ CREATED_DATE = upload.UploadDate, CREATED_BY = CreatedBy(), LAST_UPDATED_BY = "OOSUpload", LAST_UPDATED_DATE = DateTime.Now };
                foreach (var item in upload.ItemData)
                {
                    var psbu = (item.ps_bu.HasValue) ? item.ps_bu.Value.ToString() : null;
                    int? storeId = (psbu == null) ? 0 : (from c in context.STORE where c.PS_BU == psbu select c.ID).FirstOrDefault();
                    known.KNOWN_OOS_DETAIL.Add(new KNOWN_OOS_DETAIL
                                                   {
                                                       REASON_ID = (int) item.reason_code,
                                                       SOURCE_ID = (int) eSources.VENDOR_COST_FILE,
                                                       STORE_ID = (storeId > 0) ? storeId : null,
                                                       VIN = item.vin.ToString(),
                                                       UPC = item.name,
                                                       START_DATE = item.start_date,
                                                       END_DATE = item.end_date,
                                                       PS_TEAM = item.team_name,
                                                       PS_SUBTEAM = item.subteam_name,
                                                       CREATED_DATE = DateTime.Now,
                                                       CREATED_BY = CreatedBy(),
                                                       ProductStatus = item.ProductStatus,
                                                       ExpirationDate = item.ExpirationDate,
                                                   });
                }
                foreach (var map in upload.VendorRegionMap)
                {
                    var region = map.region;
                    var regionId = context.REGION.Where(r => r.REGION_ABBR.Equals(region, StringComparison.OrdinalIgnoreCase)).Select(r => r.ID).FirstOrDefault();
                    known.KNOWN_OOS_MAP.Add(new KNOWN_OOS_MAP
                                                {
                                                    REGION_ID = regionId,
                                                    VENDOR_KEY = map.vendor_key,
                                                    CREATED_DATE = DateTime.Now,
                                                    CREATED_BY = CreatedBy()
                                                });
                }
                context.KNOWN_OOS_HEADER.AddObject(known);
                context.SaveChanges();
            }
        }

        private string CreatedBy()
        {
            try
            {
                return Environment.UserDomainName + "/" + Environment.UserName;
            }
            catch (Exception)
            {
                return "OOSUpload";
            }
        }

        public IKnownUpload For(DateTime date)
        {
            var upload = new KnownUpload(date);
            return For(upload);
        }

        public void Modify(IKnownUpload upload)
        {
            if (For(upload) != null)
            {
                items.Remove(upload);
                items.Add(upload);
                Update(upload);
            }
        }

        private void Update(IKnownUpload upload)
        {
            using (var dbContext = entityFactory.New())
            {
                var searched =
                    (from c in dbContext.KNOWN_OOS_HEADER where c.CREATED_DATE == upload.UploadDate select c).
                        FirstOrDefault();
                if (searched == null) return;

                foreach (var detail in searched.KNOWN_OOS_DETAIL.ToList())
                {
                    dbContext.KNOWN_OOS_DETAIL.DeleteObject(detail);
                }
                foreach (var map in searched.KNOWN_OOS_MAP.ToList())
                {
                    dbContext.KNOWN_OOS_MAP.DeleteObject(map);
                }
                foreach (var item in upload.ItemData)
                {
                    var psbu = (item.ps_bu.HasValue) ? item.ps_bu.Value.ToString() : null;
                    int? storeId = (psbu == null) ? 0 : (from c in dbContext.STORE where c.PS_BU == psbu select c.ID).FirstOrDefault();
                    searched.KNOWN_OOS_DETAIL.Add(new KNOWN_OOS_DETAIL
                    {
                        REASON_ID = (int)item.reason_code,
                        SOURCE_ID = (int)eSources.VENDOR_COST_FILE,
                        STORE_ID = (storeId > 0) ? storeId : null,
                        VIN = item.vin.ToString(),
                        UPC = item.name,
                        START_DATE = item.start_date,
                        END_DATE = item.end_date,
                        PS_TEAM = item.team_name,
                        PS_SUBTEAM = item.subteam_name,
                        CREATED_DATE = DateTime.Now,
                        CREATED_BY = CreatedBy(),
                        ProductStatus = item.ProductStatus,
                        ExpirationDate = item.ExpirationDate,
                    });
                }
                foreach (var map in upload.VendorRegionMap)
                {
                    var region = map.region;
                    var regionId = dbContext.REGION.Where(r => r.REGION_ABBR.Equals(region, StringComparison.OrdinalIgnoreCase)).Select(r => r.ID).FirstOrDefault();
                    searched.KNOWN_OOS_MAP.Add(new KNOWN_OOS_MAP
                    {
                        REGION_ID = regionId,
                        VENDOR_KEY = map.vendor_key,
                        CREATED_DATE = DateTime.Now,
                        CREATED_BY = CreatedBy()
                    });
                }
                dbContext.SaveChanges();
            }
        }

        public void Remove(DateTime date)
        {
            var upload = new KnownUpload(date);
            if (For(upload) != null)
            {
                items.Remove(upload);
                Delete(upload);
            }
        }

        private void Delete(IKnownUpload upload)
        {
            using (var context = entityFactory.New())
            {
                var searched =
                    (from c in context.KNOWN_OOS_HEADER where c.CREATED_DATE == upload.UploadDate select c).
                        FirstOrDefault();
                if (searched == null) return;
                searched.KNOWN_OOS_DETAIL.ToList().RemoveAll(p => true);
                searched.KNOWN_OOS_MAP.ToList().RemoveAll(p => true);
                context.KNOWN_OOS_HEADER.DeleteObject(searched);
                context.SaveChanges();
            }
        }

        public void Reset()
        {
            items.Clear();
        }

        
        public bool ExistProductStatusProjection(ProductStatus projection)
        {
            using (var context = entityFactory.New())
            {
                //return (from d in context.KNOWN_OOS_DETAIL
                //                   from m in context.KNOWN_OOS_MAP
                //                   from h in context.KNOWN_OOS_HEADER
                //                   from r in context.REGION
                //                   where
                //                       d.KNOWN_OOS_HEADER_ID == h.ID && m.KNOWN_OOS_HEADER_ID == h.ID && m.REGION_ID == r.ID && 
                //                       projection.Region == r.REGION_ABBR && projection.VendorKey == m.VENDOR_KEY && projection.Vin == d.VIN && projection.Upc == d.UPC
                //                   select new {r.REGION_ABBR, m.VENDOR_KEY, d.VIN, d.UPC}).Count() > 0;


                return (from d in context.KNOWN_OOS_DETAIL
                        from m in context.KNOWN_OOS_MAP
                        from h in context.KNOWN_OOS_HEADER
                        from r in context.REGION
                        where
                            d.KNOWN_OOS_HEADER_ID == h.ID && m.KNOWN_OOS_HEADER_ID == h.ID && m.REGION_ID == r.ID &&
                            projection.Region == r.REGION_ABBR && 
                            projection.Upc == d.UPC
                        select new { r.REGION_ABBR, m.VENDOR_KEY, d.VIN, d.UPC }).Count() > 0;

                //projection.VendorKey == m.VENDOR_KEY && projection.Vin == d.VIN && 


            }
        }
    }
}
