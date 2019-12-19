using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using OOSCommon;

namespace OOS.Model
{
    public class ScanOutOfStockItemService : IScanOutOfStockItemService
    {
        private readonly IOOSLog _logger;
        private readonly IEventCreatorFactory _creatorFactory;
        private readonly IRegionRepository _regionRepository;

        public ScanOutOfStockItemService(ILogService logService, IEventCreatorFactory creatorFactory, IRegionRepository regionRepository)
        {
            _logger = logService.GetLogger();
            _creatorFactory = creatorFactory;
            _regionRepository = regionRepository;
        }

        public string[] Validate(string[] upcs)
        {
            return upcs.Where(upc => !Valid(upc)).ToArray();
        }

        public bool Validate(string upc)
        {
            var invalidUpcs = Validate(new[] {upc});
            return (invalidUpcs.Length == 0);
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
                var msg =
                    $"CreateEventsFor(): Failed, Message='{exception.Message}', StackTrace={exception.StackTrace}";
                _logger.Error(msg);
                return false;
            }
        }

        

        private bool CreateEvents(string storeName, IEnumerable<string> upcs, DateTime scanDate)
        {
            var eventCreator = _creatorFactory.ForStore(storeName);
            LogRawScanData(upcs);

            eventCreator.BeginBatch(scanDate);
            return eventCreator.WriteUPCs(upcs.ToList());
        }

        private void LogRawScanData(IEnumerable<string> upcs)
        {
            foreach (var upc in upcs)
            {
                _logger.Info($"Raw Scan data: UPC='{upc}'");
            }
        }

        private bool Valid(string upc)
        {
            if (UpcSpecification.IsSatisfiedBy(upc)) return true;

            _logger.Info($"UPC='{upc}' not valid, Error='{UpcSpecification.GetUpcCheckValue(upc)}'");
            return false;
        }

        public string[] RegionAbbreviations()
        {
            try
            {
                return _regionRepository.RegionAbbreviations().OrderBy(p => p).ToArray();
            }
            catch (Exception exception)
            {
                var msg =
                    $"RegionAbbreviations(): Failed, Message='{exception.Message}', StackTrace={exception.StackTrace}";
                _logger.Error(msg);
                return new string[]{};
            }
        }

        public string[] RegionNames()
        {
            try
            {
                return _regionRepository.RegionNames().OrderBy(p => p).ToArray();
            }
            catch (Exception exception)
            {
                var msg = $"RegionNames(): Failed, Message='{exception.Message}', StackTrace={exception.StackTrace}";
                _logger.Error(msg);
                return new string[] { };                               
            }
        }

        public string[] StoreAbbreviationsFor(string regionAbbrev)
        {
            try
            {
                if (regionAbbrev == "UK") regionAbbrev = "EU";
                var region = _regionRepository.ForAbbrev(regionAbbrev);
                return region?.OpenStores().Select(s => s.Abbrev).OrderBy(p => p).ToArray() ?? new string[] { };
            }
            catch (Exception exception)
            {
                var msg =
                    $"StoreAbbreviationsFor(): Failed, Message='{exception.Message}', StackTrace={exception.StackTrace}";
                _logger.Error(msg);
                return new string[] { };                               
            }
        }

        public string[] StoreNamesFor(string regionName)
        {
            try
            {
                var region = _regionRepository.ForName(regionName);
                return region?.OpenStores().Select(s => s.Name).OrderBy(p => p).ToArray() ?? new string[] { };
            }
            catch (Exception exception)
            {
                var msg = $"StoreNamesFor(): Failed, Message='{exception.Message}', StackTrace={exception.StackTrace}";
                _logger.Error(msg);
                return new string[] { };                               
            }
        }

        public void ScanProducts(string regionName, string storeName, string[] upcs, DateTime scanDate)
        {
            var eventCreator = _creatorFactory.ForStoreInRegion(regionName, storeName);
            LogRawScanData(upcs);

            eventCreator.BeginBatch(scanDate);
            eventCreator.WriteUPCs(upcs.ToList());
        }

        public void ScanProductsByStoreAbbreviation(string regionAbbrev, string storeAbbrev, string[] upcs, DateTime scanDate)
        {
            ValidateStoreInRegion(regionAbbrev, storeAbbrev);
            var eventCreator = _creatorFactory.ForStoreByAbbreviation(storeAbbrev);

            var success = eventCreator.BeginBatch(scanDate);
            if (success)
                eventCreator.WriteUPCs(upcs.ToList());
        }

        private void ValidateStoreInRegion(string regionAbbrev, string storeAbbrev)
        {
            var region = _regionRepository.ForAbbrev(regionAbbrev);
            AssertRegionValid(region, regionAbbrev);
            var store = region.OpenStores().FirstOrDefault(p => p.Abbrev.Equals(storeAbbrev, StringComparison.OrdinalIgnoreCase));
            AssertStoreValid(store, storeAbbrev);          
        }

        private static void AssertRegionValid(Region region, string regionAbbrev)
        {
            if (region == null)
            {
                throw new InvalidStoreException($"Invalid Region Abbreviation='{regionAbbrev}'");
            }
        }

        private static void AssertStoreValid(Store store, string storeAbbrev)
        {
            if (store == null)
            {
                throw new InvalidStoreException($"Invalid Store Abbreviation='{storeAbbrev}' in Region");
            }
        }
    }
}
