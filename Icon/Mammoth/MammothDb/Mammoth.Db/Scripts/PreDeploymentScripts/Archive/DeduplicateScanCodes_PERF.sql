SET NOCOUNT ON;
if(object_id('tempdb..#itemIDs') is not null) drop table #itemIDs;
  
  select ItemID
  into #itemIDs
  from Items with(nolock)
  where ScanCode in(select ScanCode from Items with(nolock) group by ScanCode having count(*) > 1)
    and ItemID not in
    (
      4104668,
      4108839,
      4112992,
      4112993,
      4119400,
      4119872,
      4123353,
      4123354,
      4123395,
      4128092,
      4131145,
      4133206,
      4152595,
      4157423,
      4170883,
      4170884,
      4175450,
      4188868,
      4189398,
      4191936,
      4195432,
      4195434,
      4195501,
      4195507,
      4201266,
      4202051,
      4205001
    )

delete from ItemAttributes_Locale_FL where ItemID in(SELECT ItemID from #itemIDs)
delete from ItemAttributes_Locale_FL_Ext where ItemID in(SELECT ItemID from #itemIDs)
print 'FL region is completed'
GO
delete from ItemAttributes_Locale_MA where ItemID in(SELECT ItemID from #itemIDs)
delete from ItemAttributes_Locale_MA_Ext where ItemID in(SELECT ItemID from #itemIDs)
print 'MA region is completed'
GO
delete from ItemAttributes_Locale_MW where ItemID in(SELECT ItemID from #itemIDs)
delete from ItemAttributes_Locale_MW_Ext where ItemID in(SELECT ItemID from #itemIDs)
GO
delete from ItemAttributes_Locale_NA where ItemID in(SELECT ItemID from #itemIDs)
delete from ItemAttributes_Locale_NA_Ext where ItemID in(SELECT ItemID from #itemIDs)
print 'NA region is completed'
GO
delete from ItemAttributes_Locale_NC where ItemID in(SELECT ItemID from #itemIDs)
delete from ItemAttributes_Locale_NC_Ext where ItemID in(SELECT ItemID from #itemIDs)
print 'NC region is completed'
GO
delete from ItemAttributes_Locale_NE where ItemID in(SELECT ItemID from #itemIDs)
delete from ItemAttributes_Locale_NE_Ext where ItemID in(SELECT ItemID from #itemIDs)
print 'NE region is completed'
GO
delete from ItemAttributes_Locale_PN where ItemID in(SELECT ItemID from #itemIDs)
delete from ItemAttributes_Locale_PN_Ext where ItemID in(SELECT ItemID from #itemIDs)
print 'PN region is completed'
GO
delete from ItemAttributes_Locale_RM where ItemID in(SELECT ItemID from #itemIDs)
delete from ItemAttributes_Locale_RM_Ext where ItemID in(SELECT ItemID from #itemIDs)
print 'RM region is completed'
GO
delete from ItemAttributes_Locale_SO where ItemID in(SELECT ItemID from #itemIDs)
delete from ItemAttributes_Locale_SO_Ext where ItemID in(SELECT ItemID from #itemIDs)
print 'SO region is completed'
GO
delete from ItemAttributes_Locale_SP where ItemID in(SELECT ItemID from #itemIDs)
delete from ItemAttributes_Locale_SP_Ext where ItemID in(SELECT ItemID from #itemIDs)
print 'SP region is completed'
GO
delete from ItemAttributes_Locale_SW where ItemID in(SELECT ItemID from #itemIDs)
delete from ItemAttributes_Locale_SW_Ext where ItemID in(SELECT ItemID from #itemIDs)
print 'SW region is completed'
GO
delete from ItemAttributes_Locale_TS where ItemID in(SELECT ItemID from #itemIDs)
delete from ItemAttributes_Locale_TS_Ext where ItemID in(SELECT ItemID from #itemIDs)
print 'TS region is completed'
GO
delete from ItemAttributes_Locale_UK where ItemID in(SELECT ItemID from #itemIDs)
delete from ItemAttributes_Locale_UK_Ext where ItemID in(SELECT ItemID from #itemIDs)
print 'UK region is completed'
GO
delete from ItemAttributes_Ext where ItemID in(SELECT ItemID from #itemIDs)
delete from Items where ItemID in(SELECT ItemID from #itemIDs)
print 'Item is completed'
GO

if(object_id('tempdb..#itemIDs') is not null) drop table #itemIDs;

print 'Recreating index dbo.Items.IX_Items_ScanCode...'
if exists(SELECT 1 FROM sys.indexes WHERE name='IX_Items_ScanCode' AND object_id = OBJECT_ID('dbo.Items'))
  DROP INDEX dbo.Items.IX_Items_ScanCode;
GO

CREATE UNIQUE NONCLUSTERED INDEX IX_Items_ScanCode
	ON dbo.Items (ScanCode ASC)
	INCLUDE (ItemID, Desc_Product, PackageUnit, RetailSize, RetailUOM, FoodStampEligible, BrandHCID, PSNumber)
GO
print 'All done'