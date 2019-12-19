using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using OOSCommon;

namespace OOS.Model
{
    public class ScanOutOfStockItemService : IScanOutOfStockItemService
    {
        private IOOSLog logger;
        private IEventCreatorFactory creatorFactory;
        private IRegionRepository regionRepository;

        public ScanOutOfStockItemService(ILogService logService, IEventCreatorFactory creatorFactory, IRegionRepository regionRepository)
        {
            this.logger = logService.GetLogger();
            this.creatorFactory = creatorFactory;
            this.regionRepository = regionRepository;
        }

        public string[] Validate(string[] upcs)
        {
            return upcs.Where(upc => !Valid(upc)).ToArray();
        }

        public bool Validate(string upc)
        {
            string[] invalidUPCs = Validate(new[] {upc});
            return (invalidUPCs.Length == 0);
        }


        public bool CreateEventFor(string storeName, string upc, DateTime scanDate)
        {
            return CreateEventsFor(storeName, new[] { upc }, scanDate);
        }

        public bool CreateEventsFor(string storeName, string[] upcs, DateTime scanDate)
        {
            try
            {
                return CreateEvents(storeName, upcs, scanDate);
            }
            catch(Exception exception)
            {
                var msg = string.Format("CreateEventsFor(): Failed, Message='{0}', StackTrace={1}", exception.Message, exception.StackTrace);
                logger.Error(msg);
                return false;
            }
        }

        

        private bool CreateEvents(string storeName, IEnumerable<string> upcs, DateTime scanDate)
        {
            var eventCreator = creatorFactory.ForStore(storeName);
            LogRawScanData(upcs);

            eventCreator.BeginBatch(scanDate);
            return eventCreator.WriteUPCs(upcs.ToList());
        }

        private void LogRawScanData(IEnumerable<string> upcs)
        {
            foreach (var upc in upcs)
            {
                logger.Info(string.Format("Raw Scan data: UPC='{0}'", upc));
            }
        }

        private bool Valid(string upc)
        {
            if (UpcSpecification.IsSatisfiedBy(upc)) return true;

            logger.Info(string.Format("UPC='{0}' not valid, Error='{1}'", upc, UpcSpecification.GetUpcCheckValue(upc)));
            return false;
        }

        public string[] RegionAbbreviations()
        {
            try
            {
                return regionRepository.RegionAbbreviations().OrderBy(p => p).ToArray();
            }
            catch (Exception exception)
            {
                var msg = string.Format("RegionAbbreviations(): Failed, Message='{0}', StackTrace={1}", exception.Message, exception.StackTrace);
                logger.Error(msg);
                return new string[]{};
            }
        }

        public string[] RegionNames()
        {
            try
            {
                return regionRepository.RegionNames().OrderBy(p => p).ToArray();
            }
            catch (Exception exception)
            {
                var msg = string.Format("RegionNames(): Failed, Message='{0}', StackTrace={1}", exception.Message, exception.StackTrace);
                logger.Error(msg);
                return new string[] { };                               
            }
        }

        public string[] StoreAbbreviationsFor(string regionAbbrev)
        {
            try
            {
                var region = regionRepository.ForAbbrev(regionAbbrev);
                return region == null ? new string[] { } : region.OpenStores().Select(s => s.Abbrev).OrderBy(p => p).ToArray();
            }
            catch (Exception exception)
            {
                var msg = string.Format("StoreAbbreviationsFor(): Failed, Message='{0}', StackTrace={1}", exception.Message, exception.StackTrace);
                logger.Error(msg);
                return new string[] { };                               
            }
        }

        public string[] StoreNamesFor(string regionName)
        {
            try
            {
                var region = regionRepository.ForName(regionName);
                return region == null ? new string[] { } : region.OpenStores().Select(s => s.Name).OrderBy(p => p).ToArray();
            }
            catch (Exception exception)
            {
                var msg = string.Format("StoreNamesFor(): Failed, Message='{0}', StackTrace={1}", exception.Message, exception.StackTrace);
                logger.Error(msg);
                return new string[] { };                               
            }
        }

        public void ScanProducts(string regionName, string storeName, string[] upcs, DateTime scanDate)
        {
            var eventCreator = creatorFactory.ForStoreInRegion(regionName, storeName);
            LogRawScanData(upcs);

            eventCreator.BeginBatch(scanDate);
            eventCreator.WriteUPCs(upcs.ToList());
        }

        public void ScanProductsByStoreAbbreviation(string regionAbbrev, string storeAbbrev, string[] upcs, DateTime scanDate)
        {
            ValidateStoreInRegion(regionAbbrev, storeAbbrev);
            
            var eventCreator = creatorFactory.ForStoreByAbbreviation(storeAbbrev);
            LogRawScanData(upcs);

            var success = eventCreator.BeginBatch(scanDate);
            if (success)
                eventCreator.WriteUPCs(upcs.ToList());
        }

        private void ValidateStoreInRegion(string regionAbbrev, string storeAbbrev)
        {
            var region = regionRepository.ForAbbrev(regionAbbrev);
            AssertRegionValid(region, regionAbbrev);
            var store = region.OpenStores().FirstOrDefault(p => p.Abbrev.Equals(storeAbbrev, StringComparison.OrdinalIgnoreCase));
            AssertStoreValid(store, storeAbbrev);          
        }

        private static void AssertRegionValid(Region region, string regionAbbrev)
        {
            if (region == null)
            {
                throw new InvalidStoreException(string.Format("Invalid Region Abbreviation='{0}'", regionAbbrev));
            }
        }

        private static void AssertStoreValid(Store store, string storeAbbrev)
        {
            if (store == null)
            {
                throw new InvalidStoreException(string.Format("Invalid Store Abbreviation='{0}' in Region", storeAbbrev));
            }
        }
    }
}
