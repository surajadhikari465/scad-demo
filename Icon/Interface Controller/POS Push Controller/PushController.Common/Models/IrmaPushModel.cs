using System;
using System.Collections.Generic;

namespace PushController.Common.Models
{
    // This model mirrors IRMAPush with the addition of the IConPOSPushPublishID property and custom error tracking properties.  There is no FK relationship 
    // between IRMAPush and IConPOSPushPublish so this model will allow a connection to be maintained between the two during POS data staging.

    public class IrmaPushModel
    {
        public int IrmaPushId { get; set; }
        public int IconPosPushPublishId { get; set; }
        public string RegionCode { get; set; }
        public int BusinessUnitId { get; set; }
        public string Identifier { get; set; }
        public string ChangeType { get; set; }
        public DateTime InsertDate { get; set; }
        public decimal RetailSize { get; set; }
        public string RetailPackageUom { get; set; }
        public bool TmDiscountEligible { get; set; }
        public bool CaseDiscount { get; set; }
        public int? AgeCode { get; set; }
        public bool Recall { get; set; }
        public bool RestrictedHours { get; set; }
        public bool SoldByWeight { get; set; }
        public bool ScaleForcedTare { get; set; }
        public bool QuantityRequired { get; set; }
        public bool PriceRequired { get; set; }
        public bool QuantityProhibit { get; set; }
        public bool VisualVerify { get; set; }
        public bool RestrictSale { get; set; }
        public int? PosScaleTare { get; set; }
        public string LinkedIdentifier { get; set; }
        public decimal? Price { get; set; }
        public string RetailUom { get; set; }
        public int? Multiple { get; set; }
        public int? SaleMultiple { get; set; }
        public decimal? SalePrice { get; set; }
        public DateTime? SaleStartDate { get; set; }
        public DateTime? SaleEndDate { get; set; }
        public int? InProcessBy { get; set; }
        public DateTime? InUdmDate { get; set; }
        public DateTime? EsbReadyDate { get; set; }
        public DateTime? UdmFailedDate { get; set; }
        public DateTime? EsbReadyFailedDate { get; set; }
        public string MessageBuildError { get; set; }
    }

    public class IrmaPushModelComparer : IEqualityComparer<IrmaPushModel>
    {
        public bool Equals(IrmaPushModel x, IrmaPushModel y)
        {
            if (x.Identifier == y.Identifier && x.BusinessUnitId == y.BusinessUnitId && x.ChangeType == y.ChangeType)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public int GetHashCode(IrmaPushModel obj)
        {
            return (obj.Identifier + obj.BusinessUnitId.ToString() + obj.ChangeType).GetHashCode();
        }
    }
}
