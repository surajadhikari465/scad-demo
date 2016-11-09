//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace Icon.Framework
{
    using System;
    using System.Collections.Generic;
    
    public partial class MessageQueueProduct
    {
        public MessageQueueProduct()
        {
            this.MessageQueueNutrition = new HashSet<MessageQueueNutrition>();
        }
    
        public int MessageQueueId { get; set; }
        public int MessageTypeId { get; set; }
        public int MessageStatusId { get; set; }
        public Nullable<int> MessageHistoryId { get; set; }
        public System.DateTime InsertDate { get; set; }
        public int ItemId { get; set; }
        public int LocaleId { get; set; }
        public string ItemTypeCode { get; set; }
        public string ItemTypeDesc { get; set; }
        public int ScanCodeId { get; set; }
        public string ScanCode { get; set; }
        public int ScanCodeTypeId { get; set; }
        public string ScanCodeTypeDesc { get; set; }
        public string ProductDescription { get; set; }
        public string PosDescription { get; set; }
        public string PackageUnit { get; set; }
        public string RetailSize { get; set; }
        public string RetailUom { get; set; }
        public string FoodStampEligible { get; set; }
        public bool ProhibitDiscount { get; set; }
        public string DepartmentSale { get; set; }
        public int BrandId { get; set; }
        public string BrandName { get; set; }
        public int BrandLevel { get; set; }
        public Nullable<int> BrandParentId { get; set; }
        public Nullable<int> BrowsingClassId { get; set; }
        public string BrowsingClassName { get; set; }
        public Nullable<int> BrowsingLevel { get; set; }
        public Nullable<int> BrowsingParentId { get; set; }
        public int MerchandiseClassId { get; set; }
        public string MerchandiseClassName { get; set; }
        public int MerchandiseLevel { get; set; }
        public Nullable<int> MerchandiseParentId { get; set; }
        public int TaxClassId { get; set; }
        public string TaxClassName { get; set; }
        public int TaxLevel { get; set; }
        public Nullable<int> TaxParentId { get; set; }
        public string FinancialClassId { get; set; }
        public string FinancialClassName { get; set; }
        public int FinancialLevel { get; set; }
        public Nullable<int> FinancialParentId { get; set; }
        public Nullable<int> InProcessBy { get; set; }
        public Nullable<System.DateTime> ProcessedDate { get; set; }
        public string AnimalWelfareRating { get; set; }
        public string Biodynamic { get; set; }
        public string CheeseMilkType { get; set; }
        public string CheeseRaw { get; set; }
        public string EcoScaleRating { get; set; }
        public string GlutenFreeAgency { get; set; }
        public string HealthyEatingRating { get; set; }
        public string KosherAgency { get; set; }
        public string Msc { get; set; }
        public string NonGmoAgency { get; set; }
        public string OrganicAgency { get; set; }
        public string PremiumBodyCare { get; set; }
        public string SeafoodFreshOrFrozen { get; set; }
        public string SeafoodCatchType { get; set; }
        public string VeganAgency { get; set; }
        public string Vegetarian { get; set; }
        public string WholeTrade { get; set; }
        public string GrassFed { get; set; }
        public string PastureRaised { get; set; }
        public string FreeRange { get; set; }
        public string DryAged { get; set; }
        public string AirChilled { get; set; }
        public string MadeInHouse { get; set; }
    
        public virtual MessageHistory MessageHistory { get; set; }
        public virtual MessageStatus MessageStatus { get; set; }
        public virtual MessageType MessageType { get; set; }
        public virtual ICollection<MessageQueueNutrition> MessageQueueNutrition { get; set; }
    }
}
