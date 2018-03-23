namespace Mammoth.Framework
{
    using System;
    using System.Data.Entity;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Linq;
    using System.Collections.Generic;

    public partial class MammothContext : DbContext
    {
        public MammothContext()
            : base("name=MammothContext")
        {
        }

        public virtual DbSet<AttributeGroup> AttributeGroups { get; set; }
        public virtual DbSet<Attribute> Attributes { get; set; }
        public virtual DbSet<Financial_SubTeam> Financial_SubTeam { get; set; }
        public virtual DbSet<Hierarchy> Hierarchies { get; set; }
        public virtual DbSet<Hierarchy_Merchandise> Hierarchy_Merchandise { get; set; }
        public virtual DbSet<Hierarchy_NationalClass> Hierarchy_NationalClass { get; set; }
        public virtual DbSet<HierarchyClass> HierarchyClasses { get; set; }
        public virtual DbSet<Item> Items { get; set; }
        public virtual DbSet<ItemType> ItemTypes { get; set; }
        public virtual DbSet<MessageAction> MessageActions { get; set; }
        public virtual DbSet<MessageHistory> MessageHistories { get; set; }
        public virtual DbSet<MessageQueueItemLocale> MessageQueueItemLocales { get; set; }
        public virtual DbSet<MessageQueuePrice> MessageQueuePrices { get; set; }
        public virtual DbSet<MessageStatus> MessageStatus { get; set; }
        public virtual DbSet<MessageType> MessageTypes { get; set; }
        public virtual DbSet<PriceResetMessageHistory> PriceResetMessageHistories { get; set; }
        public virtual DbSet<RegionGpmStatus> RegionGpmStatuses { get; set; }
    }
}
