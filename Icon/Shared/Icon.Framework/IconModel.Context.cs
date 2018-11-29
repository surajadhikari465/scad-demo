﻿//------------------------------------------------------------------------------
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
    using System.Data.Entity;
    using System.Data.Entity.Infrastructure;
    using System.Data.Entity.Core.Objects;
    using System.Linq;
    
    public partial class IconContext : DbContext
    {
        public IconContext()
            : base("name=IconContext")
        {
        }
    
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            throw new UnintentionalCodeFirstException();
        }
    
        public virtual DbSet<Item> Item { get; set; }
        public virtual DbSet<ItemType> ItemType { get; set; }
        public virtual DbSet<ScanCode> ScanCode { get; set; }
        public virtual DbSet<ScanCodeType> ScanCodeType { get; set; }
        public virtual DbSet<Trait> Trait { get; set; }
        public virtual DbSet<Hierarchy> Hierarchy { get; set; }
        public virtual DbSet<HierarchyClass> HierarchyClass { get; set; }
        public virtual DbSet<HierarchyPrototype> HierarchyPrototype { get; set; }
        public virtual DbSet<ItemHierarchyClass> ItemHierarchyClass { get; set; }
        public virtual DbSet<LocaleTrait> LocaleTrait { get; set; }
        public virtual DbSet<LocaleType> LocaleType { get; set; }
        public virtual DbSet<TraitGroup> TraitGroup { get; set; }
        public virtual DbSet<IRMAItemSubscription> IRMAItemSubscription { get; set; }
        public virtual DbSet<PLUMap> PLUMap { get; set; }
        public virtual DbSet<MessageType> MessageType { get; set; }
        public virtual DbSet<MessageStatus> MessageStatus { get; set; }
        public virtual DbSet<ItemTrait> ItemTrait { get; set; }
        public virtual DbSet<ItemPrice> ItemPrice { get; set; }
        public virtual DbSet<ItemPriceType> ItemPriceType { get; set; }
        public virtual DbSet<MessageAction> MessageAction { get; set; }
        public virtual DbSet<UOM> UOM { get; set; }
        public virtual DbSet<HierarchyClassTrait> HierarchyClassTrait { get; set; }
        public virtual DbSet<Country> Country { get; set; }
        public virtual DbSet<County> County { get; set; }
        public virtual DbSet<PostalCode> PostalCode { get; set; }
        public virtual DbSet<Territory> Territory { get; set; }
        public virtual DbSet<Timezone> Timezone { get; set; }
        public virtual DbSet<EventType> EventType { get; set; }
        public virtual DbSet<PhysicalAddress> PhysicalAddress { get; set; }
        public virtual DbSet<Address> Address { get; set; }
        public virtual DbSet<AddressType> AddressType { get; set; }
        public virtual DbSet<AddressUsage> AddressUsage { get; set; }
        public virtual DbSet<LocaleAddress> LocaleAddress { get; set; }
        public virtual DbSet<City> City { get; set; }
        public virtual DbSet<MessageQueueHierarchy> MessageQueueHierarchy { get; set; }
        public virtual DbSet<MessageHistory> MessageHistory { get; set; }
        public virtual DbSet<EventQueue> EventQueue { get; set; }
        public virtual DbSet<Locale> Locale { get; set; }
        public virtual DbSet<MessageQueueProduct> MessageQueueProduct { get; set; }
        public virtual DbSet<MessageQueueItemLocale> MessageQueueItemLocale { get; set; }
        public virtual DbSet<MessageQueuePrice> MessageQueuePrice { get; set; }
        public virtual DbSet<ProductSelectionGroupType> ProductSelectionGroupType { get; set; }
        public virtual DbSet<ProductSelectionGroup> ProductSelectionGroup { get; set; }
        public virtual DbSet<BusinessUnitRegionMapping> BusinessUnitRegionMapping { get; set; }
        public virtual DbSet<ItemMovementTransactionHistory> ItemMovementTransactionHistory { get; set; }
        public virtual DbSet<RegionalSettings> RegionalSettings { get; set; }
        public virtual DbSet<Regions> Regions { get; set; }
        public virtual DbSet<Settings> Settings { get; set; }
        public virtual DbSet<SettingSection> SettingSection { get; set; }
        public virtual DbSet<MessageQueueProductSelectionGroup> MessageQueueProductSelectionGroup { get; set; }
        public virtual DbSet<MessageResendStatus> MessageResendStatus { get; set; }
        public virtual DbSet<PLUCategory> PLUCategory { get; set; }
        public virtual DbSet<ItemMovement> ItemMovement { get; set; }
        public virtual DbSet<IRMAPush> IRMAPush { get; set; }
        public virtual DbSet<Agency> Agency { get; set; }
        public virtual DbSet<Mapping> Mapping { get; set; }
        public virtual DbSet<PLURequest> PLURequest { get; set; }
        public virtual DbSet<PLURequestChangeHistory> PLURequestChangeHistory { get; set; }
        public virtual DbSet<PLURequestChangeType> PLURequestChangeType { get; set; }
        public virtual DbSet<ItemNutrition> ItemNutrition { get; set; }
        public virtual DbSet<MammothEventQueue> MammothEventQueue { get; set; }
        public virtual DbSet<MammothEventType> MammothEventType { get; set; }
        public virtual DbSet<VimEventQueue> VimEventQueue { get; set; }
        public virtual DbSet<VimEventType> VimEventType { get; set; }
        public virtual DbSet<MessageQueueNutrition> MessageQueueNutrition { get; set; }
        public virtual DbSet<AuthorizedProductList> AuthorizedProductList { get; set; }
        public virtual DbSet<MessageQueueBusinessUnitInProcess> MessageQueueBusinessUnitInProcess { get; set; }
        public virtual DbSet<PerformanceLog> PerformanceLog { get; set; }
        public virtual DbSet<ItemLink> ItemLink { get; set; }
        public virtual DbSet<APIMessageProcessorLogEntry> APIMessageMonitorLog { get; set; }
        public virtual DbSet<MessageArchiveProduct> MessageArchiveProduct { get; set; }
        public virtual DbSet<InforMessageHistory> InforMessageHistory { get; set; }
        public virtual DbSet<MessageArchiveHierarchy> MessageArchiveHierarchy { get; set; }
        public virtual DbSet<ItemSignAttribute> ItemSignAttribute { get; set; }
        public virtual DbSet<IRMAItem> IRMAItem { get; set; }
        public virtual DbSet<Currency> Currencies { get; set; }
        public virtual DbSet<LocaleSubType> LocaleSubTypes { get; set; }
        public virtual DbSet<MessageQueueLocale> MessageQueueLocale { get; set; }
    
        public virtual int MarkStagingTableEntriesAsInProcessForEsb(Nullable<int> numberOfRows, Nullable<int> jobInstance)
        {
            var numberOfRowsParameter = numberOfRows.HasValue ?
                new ObjectParameter("NumberOfRows", numberOfRows) :
                new ObjectParameter("NumberOfRows", typeof(int));
    
            var jobInstanceParameter = jobInstance.HasValue ?
                new ObjectParameter("JobInstance", jobInstance) :
                new ObjectParameter("JobInstance", typeof(int));
    
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction("MarkStagingTableEntriesAsInProcessForEsb", numberOfRowsParameter, jobInstanceParameter);
        }
    
        public virtual int MarkStagingTableEntriesAsInProcessForUdm(Nullable<int> numberOfRows, Nullable<int> jobInstance)
        {
            var numberOfRowsParameter = numberOfRows.HasValue ?
                new ObjectParameter("NumberOfRows", numberOfRows) :
                new ObjectParameter("NumberOfRows", typeof(int));
    
            var jobInstanceParameter = jobInstance.HasValue ?
                new ObjectParameter("JobInstance", jobInstance) :
                new ObjectParameter("JobInstance", typeof(int));
    
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction("MarkStagingTableEntriesAsInProcessForUdm", numberOfRowsParameter, jobInstanceParameter);
        }
    
        public virtual int UpdateIrmaItemBrand(string currentBrandName, string updatedBrandName)
        {
            var currentBrandNameParameter = currentBrandName != null ?
                new ObjectParameter("currentBrandName", currentBrandName) :
                new ObjectParameter("currentBrandName", typeof(string));
    
            var updatedBrandNameParameter = updatedBrandName != null ?
                new ObjectParameter("updatedBrandName", updatedBrandName) :
                new ObjectParameter("updatedBrandName", typeof(string));
    
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction("UpdateIrmaItemBrand", currentBrandNameParameter, updatedBrandNameParameter);
        }
    
        public virtual int GenerateItemUpdateMessagesByHierarchyClass(Nullable<int> hierarchyClassID)
        {
            var hierarchyClassIDParameter = hierarchyClassID.HasValue ?
                new ObjectParameter("hierarchyClassID", hierarchyClassID) :
                new ObjectParameter("hierarchyClassID", typeof(int));
    
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction("GenerateItemUpdateMessagesByHierarchyClass", hierarchyClassIDParameter);
        }
    
        public virtual int UpdateItemTypeByHierarchyClass(Nullable<int> hierarchyClassID, string itemTypeCode, string userName)
        {
            var hierarchyClassIDParameter = hierarchyClassID.HasValue ?
                new ObjectParameter("hierarchyClassID", hierarchyClassID) :
                new ObjectParameter("hierarchyClassID", typeof(int));
    
            var itemTypeCodeParameter = itemTypeCode != null ?
                new ObjectParameter("itemTypeCode", itemTypeCode) :
                new ObjectParameter("itemTypeCode", typeof(string));
    
            var userNameParameter = userName != null ?
                new ObjectParameter("userName", userName) :
                new ObjectParameter("userName", typeof(string));
    
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction("UpdateItemTypeByHierarchyClass", hierarchyClassIDParameter, itemTypeCodeParameter, userNameParameter);
        }
    
        public virtual int MarkUnsentMessagesAsInProcess(Nullable<int> numberOfRows, Nullable<int> messageTypeId, Nullable<int> jobInstance)
        {
            var numberOfRowsParameter = numberOfRows.HasValue ?
                new ObjectParameter("NumberOfRows", numberOfRows) :
                new ObjectParameter("NumberOfRows", typeof(int));
    
            var messageTypeIdParameter = messageTypeId.HasValue ?
                new ObjectParameter("MessageTypeId", messageTypeId) :
                new ObjectParameter("MessageTypeId", typeof(int));
    
            var jobInstanceParameter = jobInstance.HasValue ?
                new ObjectParameter("JobInstance", jobInstance) :
                new ObjectParameter("JobInstance", typeof(int));
    
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction("MarkUnsentMessagesAsInProcess", numberOfRowsParameter, messageTypeIdParameter, jobInstanceParameter);
        }
    
        public virtual int ApplyMerchTaxMappingToItems(Nullable<int> merchandiseClassId, Nullable<int> taxClassId)
        {
            var merchandiseClassIdParameter = merchandiseClassId.HasValue ?
                new ObjectParameter("MerchandiseClassId", merchandiseClassId) :
                new ObjectParameter("MerchandiseClassId", typeof(int));
    
            var taxClassIdParameter = taxClassId.HasValue ?
                new ObjectParameter("TaxClassId", taxClassId) :
                new ObjectParameter("TaxClassId", typeof(int));
    
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction("ApplyMerchTaxMappingToItems", merchandiseClassIdParameter, taxClassIdParameter);
        }
    
        public virtual ObjectResult<GetDefaultTaxClassMismatches_Result> GetDefaultTaxClassMismatches()
        {
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction<GetDefaultTaxClassMismatches_Result>("GetDefaultTaxClassMismatches");
        }
    
        public virtual int MessageQueueDeleteControllerFromBusinessUnitInProcess(Nullable<int> instanceId, Nullable<int> messageTypeId)
        {
            var instanceIdParameter = instanceId.HasValue ?
                new ObjectParameter("InstanceId", instanceId) :
                new ObjectParameter("InstanceId", typeof(int));
    
            var messageTypeIdParameter = messageTypeId.HasValue ?
                new ObjectParameter("MessageTypeId", messageTypeId) :
                new ObjectParameter("MessageTypeId", typeof(int));
    
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction("MessageQueueDeleteControllerFromBusinessUnitInProcess", instanceIdParameter, messageTypeIdParameter);
        }
    
        public virtual int MessageQueueUpdateControllerBusinessUnitInProcess(Nullable<int> instanceId, Nullable<int> businessUnit, Nullable<int> messageTypeId)
        {
            var instanceIdParameter = instanceId.HasValue ?
                new ObjectParameter("InstanceId", instanceId) :
                new ObjectParameter("InstanceId", typeof(int));
    
            var businessUnitParameter = businessUnit.HasValue ?
                new ObjectParameter("BusinessUnit", businessUnit) :
                new ObjectParameter("BusinessUnit", typeof(int));
    
            var messageTypeIdParameter = messageTypeId.HasValue ?
                new ObjectParameter("MessageTypeId", messageTypeId) :
                new ObjectParameter("MessageTypeId", typeof(int));
    
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction("MessageQueueUpdateControllerBusinessUnitInProcess", instanceIdParameter, businessUnitParameter, messageTypeIdParameter);
        }
    }
}
