using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

namespace KitBuilderWebApi.DatabaseModels
{
    public partial class KitBuilderContext : DbContext
    {
		public KitBuilderContext()
		{
		}
		public KitBuilderContext(DbContextOptions<KitBuilderContext> options)
			: base(options)
		{
		}
		public virtual DbSet<InstructionList> InstructionList { get; set; }
		public virtual DbSet<InstructionListMember> InstructionListMember { get; set; }
		public virtual DbSet<InstructionType> InstructionType { get; set; }
		public virtual DbSet<Items> Items { get; set; }
		public virtual DbSet<Kit> Kit { get; set; }
		public virtual DbSet<KitInstructionList> KitInstructionList { get; set; }
		public virtual DbSet<KitLinkGroup> KitLinkGroup { get; set; }
		public virtual DbSet<KitLinkGroupItem> KitLinkGroupItem { get; set; }
		public virtual DbSet<KitLinkGroupItemLocale> KitLinkGroupItemLocale { get; set; }
		public virtual DbSet<KitLinkGroupLocale> KitLinkGroupLocale { get; set; }
		public virtual DbSet<KitLocale> KitLocale { get; set; }
		public virtual DbSet<LinkGroup> LinkGroup { get; set; }
		public virtual DbSet<LinkGroupItem> LinkGroupItem { get; set; }
		public virtual DbSet<Locale> Locale { get; set; }
		public virtual DbSet<LocaleType> LocaleType { get; set; }
		public virtual DbSet<Status> Status { get; set; }

		protected override void OnModelCreating(ModelBuilder modelBuilder)
		{
			modelBuilder.Entity<InstructionList>(entity =>
			{
				entity.HasIndex(e => e.InstructionTypeId);

				entity.HasIndex(e => e.StatusId);

				entity.Property(e => e.InsertDateUtc).HasDefaultValueSql("(sysutcdatetime())");

				entity.Property(e => e.Name)
					.IsRequired()
					.HasMaxLength(10);

				entity.HasOne(d => d.InstructionType)
					.WithMany(p => p.InstructionList)
					.HasForeignKey(d => d.InstructionTypeId)
					.OnDelete(DeleteBehavior.ClientSetNull);

				entity.HasOne(d => d.Status)
					.WithMany(p => p.InstructionList)
					.HasForeignKey(d => d.StatusId)
					.OnDelete(DeleteBehavior.ClientSetNull);
			});

			modelBuilder.Entity<InstructionListMember>(entity =>
			{
				entity.HasIndex(e => e.InstructionListId);

				entity.Property(e => e.Group)
					.IsRequired()
					.HasMaxLength(60);

				entity.Property(e => e.InsertDateUtc).HasDefaultValueSql("(sysutcdatetime())");

				entity.Property(e => e.Member).HasMaxLength(15);

				entity.HasOne(d => d.InstructionList)
					.WithMany(p => p.InstructionListMember)
					.HasForeignKey(d => d.InstructionListId)
					.OnDelete(DeleteBehavior.ClientSetNull)
					.HasConstraintName("FK_InstructionListMember_InstructionList");
			});

			modelBuilder.Entity<InstructionType>(entity =>
			{
				entity.Property(e => e.Name)
					.IsRequired()
					.HasMaxLength(100);
			});

			modelBuilder.Entity<Items>(entity =>
			{
				entity.HasKey(e => e.ItemId);

				entity.Property(e => e.ItemId).ValueGeneratedNever();

				entity.Property(e => e.BrandName).HasMaxLength(255);

				entity.Property(e => e.CustomerFriendlyDesc).HasMaxLength(255);

				entity.Property(e => e.InsertDateUtc).HasDefaultValueSql("(sysutcdatetime())");

				entity.Property(e => e.KitchenDesc).HasMaxLength(255);

				entity.Property(e => e.ProductDesc).HasMaxLength(255);

				entity.Property(e => e.ScanCode)
					.IsRequired()
					.HasMaxLength(13);
			});

			modelBuilder.Entity<Kit>(entity =>
			{
				entity.HasIndex(e => e.ItemId);

				entity.Property(e => e.Description).HasMaxLength(255);

				entity.Property(e => e.InsertDateUtc).HasDefaultValueSql("(sysutcdatetime())");

				entity.HasOne(d => d.Item)
					.WithMany(p => p.Kit)
					.HasForeignKey(d => d.ItemId)
					.OnDelete(DeleteBehavior.ClientSetNull)
					.HasConstraintName("FK_Kit_Items");
			});

			modelBuilder.Entity<KitInstructionList>(entity =>
			{
				entity.HasIndex(e => e.InstructionListId);

				entity.HasIndex(e => e.KitId);

				entity.HasOne(d => d.InstructionList)
					.WithMany(p => p.KitInstructionList)
					.HasForeignKey(d => d.InstructionListId)
					.OnDelete(DeleteBehavior.ClientSetNull)
					.HasConstraintName("FK_KitInstructionList_InstructionList");

				entity.HasOne(d => d.Kit)
					.WithMany(p => p.KitInstructionList)
					.HasForeignKey(d => d.KitId)
					.OnDelete(DeleteBehavior.ClientSetNull)
					.HasConstraintName("FK_KitInstructionList_Kit");
			});

			modelBuilder.Entity<KitLinkGroup>(entity =>
			{
				entity.HasIndex(e => e.KitId);

				entity.HasIndex(e => e.LinkGroupId);

				entity.Property(e => e.InsertDateUtc).HasDefaultValueSql("(sysutcdatetime())");

				entity.HasOne(d => d.Kit)
					.WithMany(p => p.KitLinkGroup)
					.HasForeignKey(d => d.KitId)
					.OnDelete(DeleteBehavior.ClientSetNull)
					.HasConstraintName("FK_KitLinkGroup_Kit");

				entity.HasOne(d => d.LinkGroup)
					.WithMany(p => p.KitLinkGroup)
					.HasForeignKey(d => d.LinkGroupId)
					.OnDelete(DeleteBehavior.ClientSetNull)
					.HasConstraintName("FK_KitLinkGroup_LinkGroup");
			});

			modelBuilder.Entity<KitLinkGroupItem>(entity =>
			{
				entity.HasIndex(e => e.KitLinkGroupId);

				entity.HasIndex(e => e.LinkGroupItemId);

				entity.Property(e => e.InsertDateUtc).HasDefaultValueSql("(sysutcdatetime())");

				entity.HasOne(d => d.KitLinkGroup)
					.WithMany(p => p.KitLinkGroupItem)
					.HasForeignKey(d => d.KitLinkGroupId)
					.OnDelete(DeleteBehavior.ClientSetNull)
					.HasConstraintName("FK_KitLinkGroupItem_Kit");

				entity.HasOne(d => d.LinkGroupItem)
					.WithMany(p => p.KitLinkGroupItem)
					.HasForeignKey(d => d.LinkGroupItemId)
					.OnDelete(DeleteBehavior.ClientSetNull)
					.HasConstraintName("FK_KitLinkGroupItem_LinkGroupItem");
			});

			modelBuilder.Entity<KitLinkGroupItemLocale>(entity =>
			{
				entity.HasIndex(e => e.KitLinkGroupItemId);

				entity.HasIndex(e => e.KitLinkGroupLocaleId);

				entity.Property(e => e.InsertDateUtc).HasDefaultValueSql("(sysutcdatetime())");

				entity.Property(e => e.LastModifiedBy)
					.IsRequired()
					.HasMaxLength(100);

				entity.HasOne(d => d.KitLinkGroupItem)
					.WithMany(p => p.KitLinkGroupItemLocale)
					.HasForeignKey(d => d.KitLinkGroupItemId)
					.OnDelete(DeleteBehavior.ClientSetNull)
					.HasConstraintName("FK_KitLinkGroupItemLocale_KitLinkGroupItem");

				entity.HasOne(d => d.KitLinkGroupLocale)
					.WithMany(p => p.KitLinkGroupItemLocale)
					.HasForeignKey(d => d.KitLinkGroupLocaleId)
					.HasConstraintName("FK_KitLinkGroupItemLocale_KitLocale");
			});

			modelBuilder.Entity<KitLinkGroupLocale>(entity =>
			{
				entity.HasIndex(e => e.KitLinkGroupId);

				entity.HasIndex(e => e.KitLocaleId);

				entity.Property(e => e.InsertDateUtc).HasDefaultValueSql("(sysutcdatetime())");

				entity.Property(e => e.LastModifiedBy).HasMaxLength(100);

				entity.HasOne(d => d.KitLinkGroup)
					.WithMany(p => p.KitLinkGroupLocale)
					.HasForeignKey(d => d.KitLinkGroupId)
					.OnDelete(DeleteBehavior.ClientSetNull)
					.HasConstraintName("FK_KitLinkGroupLocale_KitLinkGroup");

				entity.HasOne(d => d.KitLocale)
					.WithMany(p => p.KitLinkGroupLocale)
					.HasForeignKey(d => d.KitLocaleId)
					.HasConstraintName("FK_KitLinkGroupLocale_KitLocale");
			});

			modelBuilder.Entity<KitLocale>(entity =>
			{
				entity.HasIndex(e => e.KitId)
					.HasName("IX_KitLocale_Kit");

				entity.HasIndex(e => e.LocaleId);

				entity.HasIndex(e => e.StatusId)
					.HasName("IX_KitLocale_Status");

				entity.Property(e => e.InsertDateUtc).HasDefaultValueSql("(sysutcdatetime())");

				entity.HasOne(d => d.Kit)
					.WithMany(p => p.KitLocale)
					.HasForeignKey(d => d.KitId)
					.OnDelete(DeleteBehavior.ClientSetNull)
					.HasConstraintName("FK_KitLocale_Kit");

				entity.HasOne(d => d.Locale)
					.WithMany(p => p.KitLocale)
					.HasForeignKey(d => d.LocaleId)
					.OnDelete(DeleteBehavior.ClientSetNull)
					.HasConstraintName("FK_KitLocale_Locale");

				entity.HasOne(d => d.Status)
					.WithMany(p => p.KitLocale)
					.HasForeignKey(d => d.StatusId)
					.OnDelete(DeleteBehavior.ClientSetNull)
					.HasConstraintName("FK_KitLocale_Status");
			});

			modelBuilder.Entity<LinkGroup>(entity =>
			{
				entity.Property(e => e.GroupDescription)
					.IsRequired()
					.HasMaxLength(500);

				entity.Property(e => e.GroupName)
					.IsRequired()
					.HasMaxLength(100);

				entity.Property(e => e.InsertDateUtc).HasDefaultValueSql("(sysutcdatetime())");
			});

			modelBuilder.Entity<LinkGroupItem>(entity =>
			{
				entity.HasIndex(e => e.InstructionListId);

				entity.HasIndex(e => e.ItemId);

				entity.HasIndex(e => e.LinkGroupId);

				entity.Property(e => e.InsertDateUtc).HasDefaultValueSql("(sysutcdatetime())");

				entity.HasOne(d => d.InstructionList)
					.WithMany(p => p.LinkGroupItem)
					.HasForeignKey(d => d.InstructionListId)
					.HasConstraintName("FK_LinkGroupItem_InstructionList");

				entity.HasOne(d => d.Item)
					.WithMany(p => p.LinkGroupItem)
					.HasForeignKey(d => d.ItemId)
					.OnDelete(DeleteBehavior.ClientSetNull)
					.HasConstraintName("FK_LinkGroupItem_Items");

				entity.HasOne(d => d.LinkGroup)
					.WithMany(p => p.LinkGroupItem)
					.HasForeignKey(d => d.LinkGroupId)
					.OnDelete(DeleteBehavior.ClientSetNull)
					.HasConstraintName("FK_LinkGroupItem_LinkGroup");
			});

			modelBuilder.Entity<Locale>(entity =>
			{
				entity.HasIndex(e => e.LocaleTypeId)
					.HasName("IX_Locale_Column");

				entity.Property(e => e.LocaleId).ValueGeneratedNever();

				entity.Property(e => e.CurrencyCode).HasMaxLength(5);

				entity.Property(e => e.LocaleName)
					.IsRequired()
					.HasMaxLength(255);

				entity.Property(e => e.RegionCode).HasMaxLength(2);

				entity.Property(e => e.StoreAbbreviation).HasMaxLength(5);

				entity.HasOne(d => d.LocaleType)
					.WithMany(p => p.Locale)
					.HasForeignKey(d => d.LocaleTypeId)
					.OnDelete(DeleteBehavior.ClientSetNull)
					.HasConstraintName("FK_Locale_LocaleType");
			});

			modelBuilder.Entity<LocaleType>(entity =>
			{
				entity.Property(e => e.LocaleTypeId)
					.HasColumnName("localeTypeId")
					.ValueGeneratedNever();

				entity.Property(e => e.LocaleTypeCode)
					.HasColumnName("localeTypeCode")
					.HasMaxLength(3);

				entity.Property(e => e.LocaleTypeDesc)
					.HasColumnName("localeTypeDesc")
					.HasMaxLength(255);
			});

			modelBuilder.Entity<Status>(entity =>
			{
				entity.Property(e => e.StatusId).HasColumnName("StatusID");

				entity.Property(e => e.StatusCode)
					.IsRequired()
					.HasMaxLength(3);

				entity.Property(e => e.StatusDescription)
					.IsRequired()
					.HasMaxLength(100);
			});
		}
	}
}
