using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Icon.Dashboard.CommonDatabaseAccess
{
    public interface IMessageQueueProduct
    {
        int MessageQueueId { get; set; }
        int MessageTypeId { get; set; }
        int MessageStatusId { get; set; }
        int? MessageHistoryId { get; set; }
        DateTime InsertDate { get; set; }
        int ItemId { get; set; }
        int LocaleId { get; set; }
        string ItemTypeCode { get; set; }
        string ItemTypeDesc { get; set; }
        int ScanCodeId { get; set; }
        string ScanCode { get; set; }
        int ScanCodeTypeId { get; set; }
        string ScanCodeTypeDesc { get; set; }
        string ProductDescription { get; set; }
        string PosDescription { get; set; }
        string PackageUnit { get; set; }
        string RetailSize { get; set; }
        string RetailUom { get; set; }
        string FoodStampEligible { get; set; }
        bool ProhibitDiscount { get; set; }
        string DepartmentSale { get; set; }
        int BrandId { get; set; }
        string BrandName { get; set; }
        int BrandLevel { get; set; }
        int? BrandParentId { get; set; }
        int? BrowsingClassId { get; set; }
        string BrowsingClassName { get; set; }
        int? BrowsingLevel { get; set; }
        int? BrowsingParentId { get; set; }
        int MerchandiseClassId { get; set; }
        string MerchandiseClassName { get; set; }
        int MerchandiseLevel { get; set; }
        int? MerchandiseParentId { get; set; }
        int TaxClassId { get; set; }
        string TaxClassName { get; set; }
        int TaxLevel { get; set; }
        int? TaxParentId { get; set; }
        string FinancialClassId { get; set; }
        string FinancialClassName { get; set; }
        int FinancialLevel { get; set; }
        int? FinancialParentId { get; set; }
        int? InProcessBy { get; set; }
        DateTime? ProcessedDate { get; set; }
        string AnimalWelfareRating { get; set; }
        string Biodynamic { get; set; }
        string CheeseMilkType { get; set; }
        string CheeseRaw { get; set; }
        string EcoScaleRating { get; set; }
        string GlutenFreeAgency { get; set; }
        string HealthyEatingRating { get; set; }
        string KosherAgency { get; set; }
        string Msc { get; set; }
        string NonGmoAgency { get; set; }
        string OrganicAgency { get; set; }
        string PremiumBodyCare { get; set; }
        string SeafoodFreshOrFrozen { get; set; }
        string SeafoodCatchType { get; set; }
        string VeganAgency { get; set; }
        string Vegetarian { get; set; }
        string WholeTrade { get; set; }
        string GrassFed { get; set; }
        string PastureRaised { get; set; }
        string FreeRange { get; set; }
        string DryAged { get; set; }
        string AirChilled { get; set; }
        string MadeInHouse { get; set; }
    }
}
