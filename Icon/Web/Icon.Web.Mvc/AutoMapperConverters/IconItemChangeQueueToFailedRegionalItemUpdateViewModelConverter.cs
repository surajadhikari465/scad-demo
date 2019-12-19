using AutoMapper;
using Icon.Web.Mvc.RegionalItemCatalogs;
using Icon.Web.Mvc.Models;
using Irma.Framework;
using System;
using System.Linq;

namespace Icon.Web.Mvc.AutoMapperConverters
{
    public class IconItemChangeQueueToFailedRegionalItemUpdateViewModelConverter : ITypeConverter<IconItemChangeQueue, FailedRegionalEventViewModel>
    {
        public FailedRegionalEventViewModel Convert(IconItemChangeQueue source, FailedRegionalEventViewModel destination, ResolutionContext context)
        {
            ValidateItemChangeTypeId(source);

            FailedRegionalEventViewModel result = new FailedRegionalEventViewModel();

            result.Id = source.QID;
            result.ScanCode = source.Identifier;
            result.ProcessFailedDate = source.ProcessFailedDate.Value;

            RegionalItemChangeType itemChangeType = (RegionalItemChangeType)source.ItemChgTypeID;
            switch (itemChangeType)
            {
                case RegionalItemChangeType.New:
                    result.ChangeType = "New";
                    break;
                case RegionalItemChangeType.Item:
                    result.ChangeType = "Item";
                    break;
                case RegionalItemChangeType.Delete:
                    result.ChangeType = "Delete";
                    break;
                case RegionalItemChangeType.Offer:
                    result.ChangeType = "Offer";
                    break;
                case RegionalItemChangeType.All:
                    result.ChangeType = "All";
                    break;
                case RegionalItemChangeType.OffPromoCost:
                    result.ChangeType = "Off Promo Cost";
                    break;
            }

            return result;
        }

        private void ValidateItemChangeTypeId(IconItemChangeQueue source)
        {            
            if(!Enum.IsDefined(typeof(RegionalItemChangeType), source.ItemChgTypeID))
            {
                string[] regionalItemChangeTypes = (Enum.GetValues(typeof(RegionalItemChangeType)) as int[])
                    .Select(i => i.ToString())
                    .ToArray();
                throw new ArgumentException(String.Format("IconItemChangeQueue.ItemChgTypeID is not defined as a RegionalItemChangeType. ItemChgTypeID : {0}, RegionalItemChangeTypes [{1}].", 
                    source.ItemChgTypeID, 
                    String.Join(", ", regionalItemChangeTypes)));
            }
        }
    }
}