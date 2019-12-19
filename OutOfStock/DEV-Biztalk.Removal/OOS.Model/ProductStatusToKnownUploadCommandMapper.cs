using System;
using System.Collections.Generic;
using System.Linq;
using OOSCommon;
using OOSCommon.Import;
using OutOfStock.Messages;

namespace OOS.Model
{
    public class ProductStatusToKnownUploadCommandMapper
    {
        public IEnumerable<KnownUploadCommand> Map(DateTime uploadDate, IEnumerable<ProductStatus> statuses)
        {
            var bulkUpload = TransformToBulkUpload(statuses);
            var upload = new List<KnownUploadCommand>();
            if (bulkUpload.Count == 0) return upload;
            foreach (var pair in bulkUpload)
            {
                var vendorRegionMap = MakeVendorRegionMapFrom(pair.Key);
                if (vendorRegionMap == null) continue;
                var vendorRegions = new List<OOSKnownVendorRegionMap> {vendorRegionMap};
                var command = new KnownUploadCommand
                                  {
                                      UploadDate = uploadDate,
                                      Items = ItemDataMapper.ToKnownUploadItems(pair.Value),
                                      VendorRegionMaps = VendorRegionMapper.ToKnownUploadVendorRegions(vendorRegions), 
                                  };
                uploadDate = GenerateUploadDate(uploadDate);
                upload.Add(command);
            }
            return upload;
        }

        private DateTime GenerateUploadDate(DateTime uploadDate)
        {
            return uploadDate.AddMinutes(1).AddMilliseconds(500);
        }

        private Dictionary<string, IEnumerable<OOSKnownItemData>> TransformToBulkUpload(IEnumerable<ProductStatus> statuses)
        {
            var bulkUpload = new Dictionary<string, List<OOSKnownItemData>>();
            foreach (var status in statuses)
            {
                var mapName = GetMapName(status);
                var item = CreateKnownItemData(status);
                if (!bulkUpload.ContainsKey(mapName))
                {
                    var itemData = new List<OOSKnownItemData> {item};
                    bulkUpload.Add(mapName, itemData);
                }
                else
                {
                    var itemData = bulkUpload[mapName];
                    itemData.Add(item);
                }
            }
            return bulkUpload.ToDictionary<KeyValuePair<string, List<OOSKnownItemData>>, string, IEnumerable<OOSKnownItemData>>(kvp => kvp.Key, kvp => kvp.Value);
        }

        [ObsoleteAttribute("Need to fix this")]
        private string GetMapName(ProductStatus status)
        {
            var region = status.Region;
           // var vendorKey = status.VendorKey;
            return region;
        }

        private OOSKnownItemData CreateKnownItemData(ProductStatus status)
        {
            return new OOSKnownItemData(status.Upc, GetDefaultReasonCode() , GetDefaultStartDate(), "", status.Status, status.ExpirationDate);
            //return new OOSKnownItemData(status.Upc, GetDefaultReasonCode(), GetDefaultStartDate(), status.Vin, status.Status, status.ExpirationDate);
            
        }

        private string GetDefaultStartDate()
        {
            return Constants.StartDateNotSet.ToShortDateString();
        }

        private string GetDefaultReasonCode()
        {
            return Constants.ReasonNotSet;
        }

        private OOSKnownVendorRegionMap MakeVendorRegionMapFrom(string mapName)
        {
            string vendorKey; string region;
            if (!TryGetVendorRegion(mapName, out vendorKey, out region)) return null;
            return new OOSKnownVendorRegionMap(mapName, region, vendorKey);
        }

        private bool TryGetVendorRegion(string vendorRegionName, out string vendorKey, out string region)
        {
            vendorKey = region = string.Empty;

            var map = vendorRegionName.Split(new[] { '-' });
            if (map.Length < 2) 
                return false;

            vendorKey = map[0];
            region = map[1];

            return !string.IsNullOrWhiteSpace(region) && !string.IsNullOrWhiteSpace(vendorKey);
        }
    }
}
