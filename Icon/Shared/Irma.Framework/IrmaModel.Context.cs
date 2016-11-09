﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace Irma.Framework
{
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Infrastructure;
    using System.Data.Entity.Core.Objects;
    using System.Linq;
    
    public partial class IrmaContext : DbContext
    {
        public IrmaContext()
            : base("name=IrmaContext")
        {
        }
    
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            throw new UnintentionalCodeFirstException();
        }
    
        public virtual DbSet<IconItemLastChange> IconItemLastChange { get; set; }
        public virtual DbSet<Item> Item { get; set; }
        public virtual DbSet<ItemBrand> ItemBrand { get; set; }
        public virtual DbSet<ItemIdentifier> ItemIdentifier { get; set; }
        public virtual DbSet<TaxClass> TaxClass { get; set; }
        public virtual DbSet<Users> Users { get; set; }
        public virtual DbSet<ValidatedBrand> ValidatedBrand { get; set; }
        public virtual DbSet<ValidatedScanCode> ValidatedScanCode { get; set; }
        public virtual DbSet<Price> Price { get; set; }
        public virtual DbSet<Region> Region { get; set; }
        public virtual DbSet<AppConfigApp> AppConfigApp { get; set; }
        public virtual DbSet<AppConfigEnv> AppConfigEnv { get; set; }
        public virtual DbSet<AppConfigKey> AppConfigKey { get; set; }
        public virtual DbSet<AppConfigValue> AppConfigValue { get; set; }
        public virtual DbSet<IconItemChangeQueue> IconItemChangeQueue { get; set; }
        public virtual DbSet<Version> Version { get; set; }
        public virtual DbSet<ItemUnit> ItemUnit { get; set; }
        public virtual DbSet<Sales_SumByItem> Sales_SumByItem { get; set; }
        public virtual DbSet<TlogReprocessRequest> TlogReprocessRequest { get; set; }
        public virtual DbSet<StoreSubTeam> StoreSubTeam { get; set; }
        public virtual DbSet<ItemCategory> ItemCategory { get; set; }
        public virtual DbSet<IConPOSPushPublish> IConPOSPushPublish { get; set; }
        public virtual DbSet<NatItemClass> NatItemClass { get; set; }
        public virtual DbSet<JobStatus> JobStatus { get; set; }
        public virtual DbSet<ItemOverride> ItemOverride { get; set; }
        public virtual DbSet<SubTeam> SubTeam { get; set; }
        public virtual DbSet<ItemScale> ItemScale { get; set; }
        public virtual DbSet<ItemNutrition> ItemNutrition { get; set; }
        public virtual DbSet<MammothItemChangeEventType> MammothItemChangeEventType { get; set; }
        public virtual DbSet<MammothItemLocaleChangeQueue> MammothItemLocaleChangeQueue { get; set; }
        public virtual DbSet<ItemSignAttribute> ItemSignAttribute { get; set; }
        public virtual DbSet<NutriFacts> NutriFacts { get; set; }
        public virtual DbSet<Store> Store { get; set; }
        public virtual DbSet<MammothPriceChangeQueue> PriceChangeQueue { get; set; }
    
        public virtual int MarkPublishTableEntriesAsInProcess(Nullable<int> numberOfRows, Nullable<int> jobInstance)
        {
            var numberOfRowsParameter = numberOfRows.HasValue ?
                new ObjectParameter("NumberOfRows", numberOfRows) :
                new ObjectParameter("NumberOfRows", typeof(int));
    
            var jobInstanceParameter = jobInstance.HasValue ?
                new ObjectParameter("JobInstance", jobInstance) :
                new ObjectParameter("JobInstance", typeof(int));
    
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction("MarkPublishTableEntriesAsInProcess", numberOfRowsParameter, jobInstanceParameter);
        }
    
        public virtual ObjectResult<GetAppConfigKeysResult> GetAppConfigKeys(string applicationName)
        {
            var applicationNameParameter = applicationName != null ?
                new ObjectParameter("ApplicationName", applicationName) :
                new ObjectParameter("ApplicationName", typeof(string));
    
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction<GetAppConfigKeysResult>("GetAppConfigKeys", applicationNameParameter);
        }
    
        public virtual ObjectResult<GetJobStatusList_Result> GetJobStatusList()
        {
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction<GetJobStatusList_Result>("GetJobStatusList");
        }
    }
}
