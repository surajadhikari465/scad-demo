using AutoMapper;
using Icon.Web.Mvc.RegionalItemCatalogs;
using Icon.Web.Mvc.Models;
using Irma.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Icon.Web.Mvc.AutoMapperConverters
{
    public class IconItemChangeQueueToFailedRegionalItemUpdateViewModelConverter : TypeConverter<IconItemChangeQueue, FailedRegionalEventViewModel>
    {

        protected override FailedRegionalEventViewModel ConvertCore(IconItemChangeQueue source)
        {
            ValidateItemChangeTypeId(source);

            FailedRegionalEventViewModel destination = new FailedRegionalEventViewModel();

            destination.Id = source.QID;
            destination.ScanCode = source.Identifier;
            destination.ProcessFailedDate = source.ProcessFailedDate.Value;

            RegionalItemChangeType itemChangeType = (RegionalItemChangeType)source.ItemChgTypeID;
            switch (itemChangeType)
            {
                case RegionalItemChangeType.New: destination.ChangeType = "New";
                    break;
                case RegionalItemChangeType.Item: destination.ChangeType = "Item";
                    break;
                case RegionalItemChangeType.Delete: destination.ChangeType = "Delete";
                    break;
                case RegionalItemChangeType.Offer: destination.ChangeType = "Offer";
                    break;
                case RegionalItemChangeType.All: destination.ChangeType = "All";
                    break;
                case RegionalItemChangeType.OffPromoCost: destination.ChangeType = "Off Promo Cost";
                    break;
            }

            return destination;
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