CREATE PROCEDURE [stage].[ItemLocaleExport]    
 @Region char(2), @GroupSize int = 100, @MaxRows int = 0 , @StartRange int, @EndRange int  
 AS    
BEGIN
/*
Performance-tuning updates for Mammoth-DB stage.ItemLocaleExport procedure:
- Altered staging indexes (changed column order to match other key objects).
- New staging index to support group-ID.
- Skip re-populate of staging table: If the region passed in is already in the staging table, we do not trunctate and repopulate, so if you need to force a repopulation of the main staging table, 
  you must either run a different region first or have staging table truncated before calling proc.
- Altered groupId build order-by to match other key objects.
- Attribute values are retrieved using dynamic queries that join to the specific region's attribute table, which removes the IO against the 11 other attribute tables (the original code joins to a global view, which is effectively joining to 12 regional tables).
- Each update to staging tbl uses new temp work tables (with mirror of PK index on main staging table) and only process 300K max rows per statement (including supplier updates) to help reduce resource usage.
- The join to ItemAttributes_Locale_??_Ext can have dups, so the code is taking the value returned by max(), so is this acceptable (should we do first, last, diff method)?
- Logs to App.AppLog table; use query: select * from app.applog where appid=18 and logger='Amazon Load' order by applogid desc
*/
 SET TRANSACTION ISOLATION LEVEL READ UNCOMMITTED    
    
 DECLARE @rowCount int     
    
 IF IsNull(@MaxRows, 0) = 0     
  SET @MaxRows = 2147483647 -- max int    
    
 insert into app.applog select /*job scheduler*/18, 'Info', 'Amazon Load', suser_name(), host_name(), getdate(), getdate(), '1', ('@Region='+@Region+', @GroupSize='+cast(@GroupSize as varchar)+', @MaxRows='+cast(@MaxRows as varchar)+', @StartRange='+cast(@StartRange as varchar)+', @EndRange='+cast(@EndRange as varchar)), null, null, null

 DECLARE @ColorAddedId INT = (    
   SELECT AttributeID    
   FROM Attributes    
   WHERE AttributeCode = 'CLA'    
   )    
 DECLARE @CountryOfProcessingId INT = (    
   SELECT AttributeID    
   FROM Attributes    
   WHERE AttributeDesc LIKE 'Country of Processing'    
   )    
 DECLARE @OriginId INT = (    
   SELECT AttributeID    
   FROM Attributes    
   WHERE AttributeDesc LIKE 'Origin'    
   )    
 DECLARE @EstId INT = (    
   SELECT AttributeID    
   FROM Attributes    
   WHERE AttributeDesc LIKE 'Electronic Shelf Tag'    
   )    
 DECLARE @ExclusiveId INT = (    
   SELECT AttributeID    
   FROM Attributes    
   WHERE AttributeDesc LIKE 'Exclusive'    
   )    
 DECLARE @NumDigitsToScaleId INT = (    
   SELECT AttributeID    
   FROM Attributes    
   WHERE AttributeDesc LIKE 'Number of Digits Sent To Scale'    
   )    
 DECLARE @ChicagoBabyId INT = (    
   SELECT AttributeID    
   FROM Attributes    
   WHERE AttributeDesc LIKE 'Chicago Baby'    
   )    
 DECLARE @TagUomId INT = (    
   SELECT AttributeID    
   FROM Attributes    
   WHERE AttributeDesc LIKE 'Tag UOM'    
   )    
 DECLARE @LinkedScanCodeId INT = (    
   SELECT AttributeID    
   FROM Attributes    
   WHERE AttributeDesc LIKE 'Linked Scan Code'    
   )    
     
    
 DECLARE @ScaleExtraTextId INT = (SELECT AttributeID FROM Attributes WHERE AttributeDesc LIKE 'Scale Extra Text' )    
 DECLARE @CFSSendtoScale INT = (SELECT AttributeID FROM Attributes WHERE AttributeDesc LIKE 'CFS Send to Scale' )     
 DECLARE @ForceTare INT = (SELECT AttributeID FROM Attributes WHERE AttributeDesc LIKE 'Force Tare' )     
 DECLARE @ShelfLife INT = (SELECT AttributeID FROM Attributes WHERE AttributeCode = 'SHL' )     
 DECLARE @UnwrappedTareWeight INT = (SELECT AttributeID FROM Attributes WHERE AttributeDesc LIKE 'Unwrapped Tare Weight' )     
 DECLARE @UseByEAB INT = (SELECT AttributeID FROM Attributes WHERE AttributeDesc LIKE 'Use By EAB' )     
 DECLARE @WrappedTareWeight INT = (SELECT AttributeID FROM Attributes WHERE AttributeDesc LIKE 'Wrapped Tare Weight' )     
 DECLARE @posScaleTare INT = (SELECT AttributeID FROM Attributes WHERE AttributeDesc LIKE 'POS Scale Tare' )    
 DECLARE @lockedForSale INT = (SELECT AttributeID FROM Attributes WHERE AttributeCode = 'RS' )    
    
 DECLARE @timestamp DATETIME,    
  @msg VARCHAR(255)    
    
-- We only clear and rebuild the staging data if the region passed in is different from the region in the table (and we cover "empty table" case with coalesce fn).
-- This saves a lot of time on repeated calls to this proc to process subsets/pieces of a single region.
if @region not like coalesce((select top 1 region from stage.ItemLocaleExportStaging), '')
begin

 TRUNCATE TABLE stage.ItemLocaleExportStaging    
    
 IF EXISTS (    
   SELECT *    
   FROM sys.indexes    
   WHERE name LIKE '%IX_ItemLocaleExportStaging%'    
   )    
  DROP INDEX IX_ItemLocaleExportStaging ON stage.ItemLocaleExportStaging    
    
   IF EXISTS (        
   SELECT *        
   FROM sys.indexes        
   WHERE name LIKE '%IX_ItemLocaleExportStaging_BU%'        
   )        
  DROP INDEX IX_ItemLocaleExportStaging_BU ON stage.ItemLocaleExportStaging        
    
 SET @timestamp = GETDATE();    
 SET @msg = CONVERT(VARCHAR, @timestamp, 120) + ': begin item locale staging '    
    
 RAISERROR (    
   @msg,    
   0,    
   1    
   )    
 WITH NOWAIT    
    
 insert into app.applog select /*job scheduler*/18, 'Info', 'Amazon Load', suser_name(), host_name(), getdate(), getdate(), '1', 'Building ItemLocaleExportStaging data', null, null, null

 INSERT INTO [stage].[ItemLocaleExportStaging]    
 SELECT TOP (@MaxRows) s.Region,    
  s.BusinessUnitId,    
  l.LocaleID,    
  i.ItemID,    
  it.ItemTypeCode,    
  it.ItemTypeDesc,    
  l.StoreName AS LocaleName,    
  i.ScanCode,    
  s.Discount_Case AS [CaseDiscount],    
  s.Discount_TM [TmDiscount],    
  s.Restriction_Age [AgeRestriction],    
  s.Restriction_Hours [RestrictedHours],    
  s.Authorized,    
  s.Discontinued,    
  s.LabelTypeDesc [LabelTypeDescription],    
  s.LocalItem,    
  s.Product_Code [ProductCode],    
  s.RetailUnit,    
  s.Sign_Desc [SignDescription],    
  s.Locality,    
  s.Sign_RomanceText_Long [SignRomanceLong],    
  s.Sign_RomanceText_Short [SignRomanceShort],    
  NULL [ColorAdded],    
  NULL [CountryOfProcessing],    
  NULL [Origin],    
  NULL [ElectronicShelfTag],    
  NULL [Exclusive],    
  NULL [NumberOfDigitsSentToScale],    
  NULL [ChicagoBaby],    
  NULL [TagUom],    
  NULL [LinkedItem],    
  NULL [ScaleExtraText],    
  NULL [CFS Send to Scale],    
  NULL [Force Tare],    
  NULL [Shelf Life],    
  NULL [Unwrapped Tare Weight],    
  NULL [Use By EAB],    
  NULL [Wrapped Tare Weight],    
  s.Msrp [Msrp],    
  NULL [SupplierName],    
  NULL [IrmaVendorKey],    
  NULL [SupplierItemID],    
  NULL [SupplierCaseSize],    
  s.OrderedByInfor [OrderedByInfor],    
  s.AltRetailSize [AltRetailSize],    
  s.AltRetailUOM [AltRetailUOM],    
  s.DefaultScanCode [DefaultScanCode],    
  s.IrmaItemKey [IrmaItemKey],    
  NULL Groupid,    
  0 Processed,    
  NULL [PosScaleTare],    
  s.ScaleItem [ScaleItem],  
  NULL [LockedForSale]  
  FROM dbo.ItemLocaleAttributes s    
  INNER JOIN dbo.Items i ON s.ItemID = i.ItemID    
  INNER JOIN dbo.ItemTypes it ON i.ItemTypeID = it.ItemTypeID    
  INNER JOIN dbo.Locale l ON l.Region = @region AND s.BusinessUnitID = l.BusinessUnitID    
  WHERE s.Region = @region    
 option (recompile)    
    
 SET @rowCount = @@ROWCOUNT;    
 SET @timestamp = GETDATE();    
 SET @msg = CONVERT(VARCHAR, @timestamp, 120) + ': items staged'    
    
 RAISERROR (    
   @msg,    
   0,    
   1    
   )    
 WITH NOWAIT

 -- CORRECTED INDEXES
 insert into app.applog select /*job scheduler*/18, 'Info', 'Amazon Load', suser_name(), host_name(), getdate(), getdate(), '1', 'Building ItemLocaleExportStaging indexes', null, null, null
 CREATE NONCLUSTERED INDEX IX_ItemLocaleExportStaging ON stage.ItemLocaleExportStaging (    
   itemid,  
   localeid
 );

 CREATE NONCLUSTERED INDEX IX_ItemLocaleExportStaging_BU ON stage.ItemLocaleExportStaging (        
   itemid,
   BusinessUnitId
 );  

 insert into app.applog select /*job scheduler*/18, 'Info', 'Amazon Load', suser_name(), host_name(), getdate(), getdate(), '1', 'Set ItemLocaleExportStaging group IDs', null, null, null
  
   -- assign batches based on @GroupSize
   -- Corrected order-by to be itemid, localeid to match key indexes.
 ;WITH cte    
 AS (    
  SELECT ItemId,    
   GroupId,    
   (    
    RANK() OVER (    
     ORDER BY itemid, localeid
     ) - 1    
    ) / @GroupSize CalculatedGroupId    
  FROM [stage].[ItemLocaleExportStaging]    
  )    
 UPDATE cte    
 SET GroupId = cte.CalculatedGroupId    
    
 SET @timestamp = GETDATE();    
 SET @msg = CONVERT(VARCHAR, @timestamp, 120) + ': group ids assigned'    
    
 RAISERROR (    
   @msg,    
   0,    
   1    
   )    
 WITH NOWAIT    

end -- Check if region we are processing is already in the stage.ItemLocaleExportStaging table.
else
begin
	-- This is normally set after population of staging table, so need to set if that part is skipped.
	select @rowCount = count(*) from [stage].[ItemLocaleExportStaging]
	insert into app.applog select /*job scheduler*/18, 'Info', 'Amazon Load', suser_name(), host_name(), getdate(), getdate(), '1', 'SKIPPED re-populate ItemLocaleExportStaging table', null, null, null
end
    
-- New index to help with groupId references.
if not exists (select 1 from sys.indexes where name = 'IX_ItemLocaleExportStaging_GroupId_Item_Locale')
begin
	insert into app.applog select /*job scheduler*/18, 'Info', 'Amazon Load', suser_name(), host_name(), getdate(), getdate(), '1', 'Creating groupid index on staging', null, null, null
	CREATE NONCLUSTERED INDEX [IX_ItemLocaleExportStaging_GroupId_Item_Locale] ON stage.[ItemLocaleExportStaging]([groupid]) INCLUDE(itemid, localeid)
end

declare @targetedRows int = (select count(*) from stage.ItemLocaleExportStaging where groupid between @StartRange and @EndRange)
insert into app.applog select /*job scheduler*/18, 'Info', 'Amazon Load', suser_name(), host_name(), getdate(), getdate(), '1', ('TargetedRows='+cast(@targetedRows as varchar)), null, null, null

-- Work tables to manage item-locale-supplier updates.
if object_id('tempdb..#targetedStagingRows') is not null drop table #targetedStagingRows
create table #targetedStagingRows (
	ItemId int not null,
	LocaleId int not null,
	attrVal nvarchar(max),
	PRIMARY KEY CLUSTERED (ItemId, LocaleId)
)

if object_id('tempdb..#targetedStagingRowsCurrentPass') is not null drop table #targetedStagingRowsCurrentPass
create table #targetedStagingRowsCurrentPass (
	ItemId int not null, LocaleId int not null,
	attrVal nvarchar(max),
	PRIMARY KEY CLUSTERED (ItemId, LocaleId)
)

declare @sql nvarchar(max), @attr varchar(64)
-------------------------------------------------
select @attr = '@ColorAddedId'
insert into app.applog select /*job scheduler*/18, 'Info', 'Amazon Load', suser_name(), host_name(), getdate(), getdate(), '1', 'Start '+@attr, null, null, null
truncate table #targetedStagingRows
select @sql = '
insert into #targetedStagingRows(itemId, localeId, attrVal) select il.itemId, il.localeId, max(ext.AttributeValue)
from [stage].ItemLocaleExportStaging il JOIN ItemAttributes_Locale_' + @Region + '_Ext ext ON il.ItemId = ext.ItemID AND il.LocaleId = ext.LocaleID  
where ext.AttributeId = ' + cast(@ColorAddedId as nvarchar) + ' and il.GroupId between ' + cast(@StartRange as nvarchar) + ' and ' + cast(@EndRange as nvarchar) + ' group by il.itemid, il.localeid option (recompile);'
print @sql
exec sp_executesql @sql

while exists (select top 1 itemId from #targetedStagingRows)
begin
	insert into app.applog select /*job scheduler*/18, 'Info', 'Amazon Load', suser_name(), host_name(), getdate(), getdate(), '1', ('Remaining rows: '+ cast((select count(itemId) from #targetedStagingRows) as varchar)), null, null, null
	truncate table #targetedStagingRowsCurrentPass;
	insert into #targetedStagingRowsCurrentPass select top 300000 * from #targetedStagingRows;
	update il set ColorAdded = tpass.attrVal from [stage].ItemLocaleExportStaging il join #targetedStagingRowsCurrentPass tPass on il.itemId = tPass.itemId and il.localeId = tpass.localeId;
	delete tRows from #targetedStagingRows tRows join #targetedStagingRowsCurrentPass tPass on tRows.itemId = tPass.itemId and tRows.localeId = tPass.localeId;
end
insert into app.applog select /*job scheduler*/18, 'Info', 'Amazon Load', suser_name(), host_name(), getdate(), getdate(), '1', 'DONE: '+@attr, null, null, null
-------------------------
-------------------------
select @attr = '@CountryOfProcessingId'
insert into app.applog select /*job scheduler*/18, 'Info', 'Amazon Load', suser_name(), host_name(), getdate(), getdate(), '1', 'Start '+@attr, null, null, null
truncate table #targetedStagingRows
select @sql = '
insert into #targetedStagingRows(itemId, localeId, attrVal) select il.itemId, il.localeId, max(ext.AttributeValue)
from [stage].ItemLocaleExportStaging il JOIN ItemAttributes_Locale_' + @Region + '_Ext ext ON il.ItemId = ext.ItemID AND il.LocaleId = ext.LocaleID  
where ext.AttributeId = ' + cast(@CountryOfProcessingId as nvarchar) + ' and il.GroupId between ' + cast(@StartRange as nvarchar) + ' and ' + cast(@EndRange as nvarchar) + ' group by il.itemid, il.localeid option (recompile);'
print @sql
exec sp_executesql @sql

while exists (select top 1 itemId from #targetedStagingRows)
begin
	insert into app.applog select /*job scheduler*/18, 'Info', 'Amazon Load', suser_name(), host_name(), getdate(), getdate(), '1', ('Remaining rows: '+ cast((select count(itemId) from #targetedStagingRows) as varchar)), null, null, null
	truncate table #targetedStagingRowsCurrentPass;
	insert into #targetedStagingRowsCurrentPass select top 300000 * from #targetedStagingRows;
	update il set CountryOfProcessing = tpass.attrVal from [stage].ItemLocaleExportStaging il join #targetedStagingRowsCurrentPass tPass on il.itemId = tPass.itemId and il.localeId = tpass.localeId;
	delete tRows from #targetedStagingRows tRows join #targetedStagingRowsCurrentPass tPass on tRows.itemId = tPass.itemId and tRows.localeId = tPass.localeId;
end
insert into app.applog select /*job scheduler*/18, 'Info', 'Amazon Load', suser_name(), host_name(), getdate(), getdate(), '1', 'DONE: '+@attr, null, null, null
-------------------------
-------------------------
select @attr = '@OriginId'
insert into app.applog select /*job scheduler*/18, 'Info', 'Amazon Load', suser_name(), host_name(), getdate(), getdate(), '1', 'Start '+@attr, null, null, null
truncate table #targetedStagingRows
select @sql = '
insert into #targetedStagingRows(itemId, localeId, attrVal) select il.itemId, il.localeId, max(ext.AttributeValue)
from [stage].ItemLocaleExportStaging il JOIN ItemAttributes_Locale_' + @Region + '_Ext ext ON il.ItemId = ext.ItemID AND il.LocaleId = ext.LocaleID  
where ext.AttributeId = ' + cast(@OriginId as nvarchar) + ' and il.GroupId between ' + cast(@StartRange as nvarchar) + ' and ' + cast(@EndRange as nvarchar) + ' group by il.itemid, il.localeid option (recompile);'
print @sql
exec sp_executesql @sql

while exists (select top 1 itemId from #targetedStagingRows)
begin
	insert into app.applog select /*job scheduler*/18, 'Info', 'Amazon Load', suser_name(), host_name(), getdate(), getdate(), '1', ('Remaining rows: '+ cast((select count(itemId) from #targetedStagingRows) as varchar)), null, null, null
	truncate table #targetedStagingRowsCurrentPass;
	insert into #targetedStagingRowsCurrentPass select top 300000 * from #targetedStagingRows;
	update il set Origin = tpass.attrVal from [stage].ItemLocaleExportStaging il join #targetedStagingRowsCurrentPass tPass on il.itemId = tPass.itemId and il.localeId = tpass.localeId;
	delete tRows from #targetedStagingRows tRows join #targetedStagingRowsCurrentPass tPass on tRows.itemId = tPass.itemId and tRows.localeId = tPass.localeId;
end
insert into app.applog select /*job scheduler*/18, 'Info', 'Amazon Load', suser_name(), host_name(), getdate(), getdate(), '1', 'DONE: '+@attr, null, null, null
-------------------------
-------------------------
select @attr = '@EstId'
insert into app.applog select /*job scheduler*/18, 'Info', 'Amazon Load', suser_name(), host_name(), getdate(), getdate(), '1', 'Start '+@attr, null, null, null
truncate table #targetedStagingRows
select @sql = '
insert into #targetedStagingRows(itemId, localeId, attrVal) select il.itemId, il.localeId, max(ext.AttributeValue)
from [stage].ItemLocaleExportStaging il JOIN ItemAttributes_Locale_' + @Region + '_Ext ext ON il.ItemId = ext.ItemID AND il.LocaleId = ext.LocaleID  
where ext.AttributeId = ' + cast(@EstId as nvarchar) + ' and il.GroupId between ' + cast(@StartRange as nvarchar) + ' and ' + cast(@EndRange as nvarchar) + ' group by il.itemid, il.localeid option (recompile);'
print @sql
exec sp_executesql @sql

while exists (select top 1 itemId from #targetedStagingRows)
begin
	insert into app.applog select /*job scheduler*/18, 'Info', 'Amazon Load', suser_name(), host_name(), getdate(), getdate(), '1', ('Remaining rows: '+ cast((select count(itemId) from #targetedStagingRows) as varchar)), null, null, null
	truncate table #targetedStagingRowsCurrentPass;
	insert into #targetedStagingRowsCurrentPass select top 300000 * from #targetedStagingRows;
	update il set ElectronicShelfTag = tpass.attrVal from [stage].ItemLocaleExportStaging il join #targetedStagingRowsCurrentPass tPass on il.itemId = tPass.itemId and il.localeId = tpass.localeId;
	delete tRows from #targetedStagingRows tRows join #targetedStagingRowsCurrentPass tPass on tRows.itemId = tPass.itemId and tRows.localeId = tPass.localeId;
end
insert into app.applog select /*job scheduler*/18, 'Info', 'Amazon Load', suser_name(), host_name(), getdate(), getdate(), '1', 'DONE: '+@attr, null, null, null
-------------------------
-------------------------
select @attr = '@ExclusiveId'
insert into app.applog select /*job scheduler*/18, 'Info', 'Amazon Load', suser_name(), host_name(), getdate(), getdate(), '1', 'Start '+@attr, null, null, null
truncate table #targetedStagingRows
select @sql = '
insert into #targetedStagingRows(itemId, localeId, attrVal) select il.itemId, il.localeId, max(ext.AttributeValue)
from [stage].ItemLocaleExportStaging il JOIN ItemAttributes_Locale_' + @Region + '_Ext ext ON il.ItemId = ext.ItemID AND il.LocaleId = ext.LocaleID  
where ext.AttributeId = ' + cast(@ExclusiveId as nvarchar) + ' and il.GroupId between ' + cast(@StartRange as nvarchar) + ' and ' + cast(@EndRange as nvarchar) + ' group by il.itemid, il.localeid option (recompile);'
print @sql
exec sp_executesql @sql

while exists (select top 1 itemId from #targetedStagingRows)
begin
	insert into app.applog select /*job scheduler*/18, 'Info', 'Amazon Load', suser_name(), host_name(), getdate(), getdate(), '1', ('Remaining rows: '+ cast((select count(itemId) from #targetedStagingRows) as varchar)), null, null, null
	truncate table #targetedStagingRowsCurrentPass;
	insert into #targetedStagingRowsCurrentPass select top 300000 * from #targetedStagingRows;
	update il set Exclusive = tpass.attrVal from [stage].ItemLocaleExportStaging il join #targetedStagingRowsCurrentPass tPass on il.itemId = tPass.itemId and il.localeId = tpass.localeId;
	delete tRows from #targetedStagingRows tRows join #targetedStagingRowsCurrentPass tPass on tRows.itemId = tPass.itemId and tRows.localeId = tPass.localeId;
end
insert into app.applog select /*job scheduler*/18, 'Info', 'Amazon Load', suser_name(), host_name(), getdate(), getdate(), '1', 'DONE: '+@attr, null, null, null
-------------------------
-------------------------
select @attr = '@NumDigitsToScaleId'
insert into app.applog select /*job scheduler*/18, 'Info', 'Amazon Load', suser_name(), host_name(), getdate(), getdate(), '1', 'Start '+@attr, null, null, null
truncate table #targetedStagingRows
select @sql = '
insert into #targetedStagingRows(itemId, localeId, attrVal) select il.itemId, il.localeId, max(ext.AttributeValue)
from [stage].ItemLocaleExportStaging il JOIN ItemAttributes_Locale_' + @Region + '_Ext ext ON il.ItemId = ext.ItemID AND il.LocaleId = ext.LocaleID  
where ext.AttributeId = ' + cast(@NumDigitsToScaleId as nvarchar) + ' and il.GroupId between ' + cast(@StartRange as nvarchar) + ' and ' + cast(@EndRange as nvarchar) + ' group by il.itemid, il.localeid option (recompile);'
print @sql
exec sp_executesql @sql

while exists (select top 1 itemId from #targetedStagingRows)
begin
	insert into app.applog select /*job scheduler*/18, 'Info', 'Amazon Load', suser_name(), host_name(), getdate(), getdate(), '1', ('Remaining rows: '+ cast((select count(itemId) from #targetedStagingRows) as varchar)), null, null, null
	truncate table #targetedStagingRowsCurrentPass;
	insert into #targetedStagingRowsCurrentPass select top 300000 * from #targetedStagingRows;
	update il set NumberOfDigitsSentToScale = tpass.attrVal from [stage].ItemLocaleExportStaging il join #targetedStagingRowsCurrentPass tPass on il.itemId = tPass.itemId and il.localeId = tpass.localeId;
	delete tRows from #targetedStagingRows tRows join #targetedStagingRowsCurrentPass tPass on tRows.itemId = tPass.itemId and tRows.localeId = tPass.localeId;
end
insert into app.applog select /*job scheduler*/18, 'Info', 'Amazon Load', suser_name(), host_name(), getdate(), getdate(), '1', 'DONE: '+@attr, null, null, null
-------------------------
-------------------------
select @attr = '@ChicagoBabyId'
insert into app.applog select /*job scheduler*/18, 'Info', 'Amazon Load', suser_name(), host_name(), getdate(), getdate(), '1', 'Start '+@attr, null, null, null
truncate table #targetedStagingRows
select @sql = '
insert into #targetedStagingRows(itemId, localeId, attrVal) select il.itemId, il.localeId, max(ext.AttributeValue)
from [stage].ItemLocaleExportStaging il JOIN ItemAttributes_Locale_' + @Region + '_Ext ext ON il.ItemId = ext.ItemID AND il.LocaleId = ext.LocaleID  
where ext.AttributeId = ' + cast(@ChicagoBabyId as nvarchar) + ' and il.GroupId between ' + cast(@StartRange as nvarchar) + ' and ' + cast(@EndRange as nvarchar) + ' group by il.itemid, il.localeid option (recompile);'
print @sql
exec sp_executesql @sql

while exists (select top 1 itemId from #targetedStagingRows)
begin
	insert into app.applog select /*job scheduler*/18, 'Info', 'Amazon Load', suser_name(), host_name(), getdate(), getdate(), '1', ('Remaining rows: '+ cast((select count(itemId) from #targetedStagingRows) as varchar)), null, null, null
	truncate table #targetedStagingRowsCurrentPass;
	insert into #targetedStagingRowsCurrentPass select top 300000 * from #targetedStagingRows;
	update il set ChicagoBaby = tpass.attrVal from [stage].ItemLocaleExportStaging il join #targetedStagingRowsCurrentPass tPass on il.itemId = tPass.itemId and il.localeId = tpass.localeId;
	delete tRows from #targetedStagingRows tRows join #targetedStagingRowsCurrentPass tPass on tRows.itemId = tPass.itemId and tRows.localeId = tPass.localeId;
end
insert into app.applog select /*job scheduler*/18, 'Info', 'Amazon Load', suser_name(), host_name(), getdate(), getdate(), '1', 'DONE: '+@attr, null, null, null
-------------------------
-------------------------
select @attr = '@TagUomId'
insert into app.applog select /*job scheduler*/18, 'Info', 'Amazon Load', suser_name(), host_name(), getdate(), getdate(), '1', 'Start '+@attr, null, null, null
truncate table #targetedStagingRows
select @sql = '
insert into #targetedStagingRows(itemId, localeId, attrVal) select il.itemId, il.localeId, max(ext.AttributeValue)
from [stage].ItemLocaleExportStaging il JOIN ItemAttributes_Locale_' + @Region + '_Ext ext ON il.ItemId = ext.ItemID AND il.LocaleId = ext.LocaleID  
where ext.AttributeId = ' + cast(@TagUomId as nvarchar) + ' and il.GroupId between ' + cast(@StartRange as nvarchar) + ' and ' + cast(@EndRange as nvarchar) + ' group by il.itemid, il.localeid option (recompile);'
print @sql
exec sp_executesql @sql

while exists (select top 1 itemId from #targetedStagingRows)
begin
	insert into app.applog select /*job scheduler*/18, 'Info', 'Amazon Load', suser_name(), host_name(), getdate(), getdate(), '1', ('Remaining rows: '+ cast((select count(itemId) from #targetedStagingRows) as varchar)), null, null, null
	truncate table #targetedStagingRowsCurrentPass;
	insert into #targetedStagingRowsCurrentPass select top 300000 * from #targetedStagingRows;
	update il set TagUom = tpass.attrVal from [stage].ItemLocaleExportStaging il join #targetedStagingRowsCurrentPass tPass on il.itemId = tPass.itemId and il.localeId = tpass.localeId;
	delete tRows from #targetedStagingRows tRows join #targetedStagingRowsCurrentPass tPass on tRows.itemId = tPass.itemId and tRows.localeId = tPass.localeId;
end
insert into app.applog select /*job scheduler*/18, 'Info', 'Amazon Load', suser_name(), host_name(), getdate(), getdate(), '1', 'DONE: '+@attr, null, null, null
-------------------------
-------------------------
select @attr = '@LinkedScanCodeId'
insert into app.applog select /*job scheduler*/18, 'Info', 'Amazon Load', suser_name(), host_name(), getdate(), getdate(), '1', 'Start '+@attr, null, null, null
truncate table #targetedStagingRows
select @sql = '
insert into #targetedStagingRows(itemId, localeId, attrVal) select il.itemId, il.localeId, max(ext.AttributeValue)
from [stage].ItemLocaleExportStaging il JOIN ItemAttributes_Locale_' + @Region + '_Ext ext ON il.ItemId = ext.ItemID AND il.LocaleId = ext.LocaleID  
where ext.AttributeId = ' + cast(@LinkedScanCodeId as nvarchar) + ' and il.GroupId between ' + cast(@StartRange as nvarchar) + ' and ' + cast(@EndRange as nvarchar) + ' group by il.itemid, il.localeid option (recompile);'
print @sql
exec sp_executesql @sql

while exists (select top 1 itemId from #targetedStagingRows)
begin
	insert into app.applog select /*job scheduler*/18, 'Info', 'Amazon Load', suser_name(), host_name(), getdate(), getdate(), '1', ('Remaining rows: '+ cast((select count(itemId) from #targetedStagingRows) as varchar)), null, null, null
	truncate table #targetedStagingRowsCurrentPass;
	insert into #targetedStagingRowsCurrentPass select top 300000 * from #targetedStagingRows;
	update il set LinkedItem = tpass.attrVal from [stage].ItemLocaleExportStaging il join #targetedStagingRowsCurrentPass tPass on il.itemId = tPass.itemId and il.localeId = tpass.localeId;
	delete tRows from #targetedStagingRows tRows join #targetedStagingRowsCurrentPass tPass on tRows.itemId = tPass.itemId and tRows.localeId = tPass.localeId;
end
insert into app.applog select /*job scheduler*/18, 'Info', 'Amazon Load', suser_name(), host_name(), getdate(), getdate(), '1', 'DONE: '+@attr, null, null, null
-------------------------
-------------------------
select @attr = '@ScaleExtraTextId'
insert into app.applog select /*job scheduler*/18, 'Info', 'Amazon Load', suser_name(), host_name(), getdate(), getdate(), '1', 'Start '+@attr, null, null, null
truncate table #targetedStagingRows
select @sql = '
insert into #targetedStagingRows(itemId, localeId, attrVal) select il.itemId, il.localeId, max(ext.AttributeValue)
from [stage].ItemLocaleExportStaging il JOIN ItemAttributes_Locale_' + @Region + '_Ext ext ON il.ItemId = ext.ItemID AND il.LocaleId = ext.LocaleID  
where ext.AttributeId = ' + cast(@ScaleExtraTextId as nvarchar) + ' and il.GroupId between ' + cast(@StartRange as nvarchar) + ' and ' + cast(@EndRange as nvarchar) + ' group by il.itemid, il.localeid option (recompile);'
print @sql
exec sp_executesql @sql

while exists (select top 1 itemId from #targetedStagingRows)
begin
	insert into app.applog select /*job scheduler*/18, 'Info', 'Amazon Load', suser_name(), host_name(), getdate(), getdate(), '1', ('Remaining rows: '+ cast((select count(itemId) from #targetedStagingRows) as varchar)), null, null, null
	truncate table #targetedStagingRowsCurrentPass;
	insert into #targetedStagingRowsCurrentPass select top 300000 * from #targetedStagingRows;
	update il set ScaleExtraText = tpass.attrVal from [stage].ItemLocaleExportStaging il join #targetedStagingRowsCurrentPass tPass on il.itemId = tPass.itemId and il.localeId = tpass.localeId;
	delete tRows from #targetedStagingRows tRows join #targetedStagingRowsCurrentPass tPass on tRows.itemId = tPass.itemId and tRows.localeId = tPass.localeId;
end
insert into app.applog select /*job scheduler*/18, 'Info', 'Amazon Load', suser_name(), host_name(), getdate(), getdate(), '1', 'DONE: '+@attr, null, null, null
-------------------------
-------------------------
select @attr = '@CFSSendtoScale'
insert into app.applog select /*job scheduler*/18, 'Info', 'Amazon Load', suser_name(), host_name(), getdate(), getdate(), '1', 'Start '+@attr, null, null, null
truncate table #targetedStagingRows
select @sql = '
insert into #targetedStagingRows(itemId, localeId, attrVal) select il.itemId, il.localeId, max(ext.AttributeValue)
from [stage].ItemLocaleExportStaging il JOIN ItemAttributes_Locale_' + @Region + '_Ext ext ON il.ItemId = ext.ItemID AND il.LocaleId = ext.LocaleID  
where ext.AttributeId = ' + cast(@CFSSendtoScale as nvarchar) + ' and il.GroupId between ' + cast(@StartRange as nvarchar) + ' and ' + cast(@EndRange as nvarchar) + ' group by il.itemid, il.localeid option (recompile);'
print @sql
exec sp_executesql @sql

while exists (select top 1 itemId from #targetedStagingRows)
begin
	insert into app.applog select /*job scheduler*/18, 'Info', 'Amazon Load', suser_name(), host_name(), getdate(), getdate(), '1', ('Remaining rows: '+ cast((select count(itemId) from #targetedStagingRows) as varchar)), null, null, null
	truncate table #targetedStagingRowsCurrentPass;
	insert into #targetedStagingRowsCurrentPass select top 300000 * from #targetedStagingRows;
	update il set [CFS Send to Scale] = tpass.attrVal from [stage].ItemLocaleExportStaging il join #targetedStagingRowsCurrentPass tPass on il.itemId = tPass.itemId and il.localeId = tpass.localeId;
	delete tRows from #targetedStagingRows tRows join #targetedStagingRowsCurrentPass tPass on tRows.itemId = tPass.itemId and tRows.localeId = tPass.localeId;
end
insert into app.applog select /*job scheduler*/18, 'Info', 'Amazon Load', suser_name(), host_name(), getdate(), getdate(), '1', 'DONE: '+@attr, null, null, null
-------------------------
-------------------------
select @attr = '@ForceTare'
insert into app.applog select /*job scheduler*/18, 'Info', 'Amazon Load', suser_name(), host_name(), getdate(), getdate(), '1', 'Start '+@attr, null, null, null
truncate table #targetedStagingRows
select @sql = '
insert into #targetedStagingRows(itemId, localeId, attrVal) select il.itemId, il.localeId, max(ext.AttributeValue)
from [stage].ItemLocaleExportStaging il JOIN ItemAttributes_Locale_' + @Region + '_Ext ext ON il.ItemId = ext.ItemID AND il.LocaleId = ext.LocaleID  
where ext.AttributeId = ' + cast(@ForceTare as nvarchar) + ' and il.GroupId between ' + cast(@StartRange as nvarchar) + ' and ' + cast(@EndRange as nvarchar) + ' group by il.itemid, il.localeid option (recompile);'
print @sql
exec sp_executesql @sql

while exists (select top 1 itemId from #targetedStagingRows)
begin
	insert into app.applog select /*job scheduler*/18, 'Info', 'Amazon Load', suser_name(), host_name(), getdate(), getdate(), '1', ('Remaining rows: '+ cast((select count(itemId) from #targetedStagingRows) as varchar)), null, null, null
	truncate table #targetedStagingRowsCurrentPass;
	insert into #targetedStagingRowsCurrentPass select top 300000 * from #targetedStagingRows;
	update il set [Force Tare] = tpass.attrVal from [stage].ItemLocaleExportStaging il join #targetedStagingRowsCurrentPass tPass on il.itemId = tPass.itemId and il.localeId = tpass.localeId;
	delete tRows from #targetedStagingRows tRows join #targetedStagingRowsCurrentPass tPass on tRows.itemId = tPass.itemId and tRows.localeId = tPass.localeId;
end
insert into app.applog select /*job scheduler*/18, 'Info', 'Amazon Load', suser_name(), host_name(), getdate(), getdate(), '1', 'DONE: '+@attr, null, null, null
-------------------------
-------------------------
select @attr = '@ShelfLife'
insert into app.applog select /*job scheduler*/18, 'Info', 'Amazon Load', suser_name(), host_name(), getdate(), getdate(), '1', 'Start '+@attr, null, null, null
truncate table #targetedStagingRows
select @sql = '
insert into #targetedStagingRows(itemId, localeId, attrVal) select il.itemId, il.localeId, max(ext.AttributeValue)
from [stage].ItemLocaleExportStaging il JOIN ItemAttributes_Locale_' + @Region + '_Ext ext ON il.ItemId = ext.ItemID AND il.LocaleId = ext.LocaleID  
where ext.AttributeId = ' + cast(@ShelfLife as nvarchar) + ' and il.GroupId between ' + cast(@StartRange as nvarchar) + ' and ' + cast(@EndRange as nvarchar) + ' group by il.itemid, il.localeid option (recompile);'
print @sql
exec sp_executesql @sql

while exists (select top 1 itemId from #targetedStagingRows)
begin
	insert into app.applog select /*job scheduler*/18, 'Info', 'Amazon Load', suser_name(), host_name(), getdate(), getdate(), '1', ('Remaining rows: '+ cast((select count(itemId) from #targetedStagingRows) as varchar)), null, null, null
	truncate table #targetedStagingRowsCurrentPass;
	insert into #targetedStagingRowsCurrentPass select top 300000 * from #targetedStagingRows;
	update il set [Shelf Life] = tpass.attrVal from [stage].ItemLocaleExportStaging il join #targetedStagingRowsCurrentPass tPass on il.itemId = tPass.itemId and il.localeId = tpass.localeId;
	delete tRows from #targetedStagingRows tRows join #targetedStagingRowsCurrentPass tPass on tRows.itemId = tPass.itemId and tRows.localeId = tPass.localeId;
end
insert into app.applog select /*job scheduler*/18, 'Info', 'Amazon Load', suser_name(), host_name(), getdate(), getdate(), '1', 'DONE: '+@attr, null, null, null
-------------------------
-------------------------
select @attr = '@UnwrappedTareWeight'
insert into app.applog select /*job scheduler*/18, 'Info', 'Amazon Load', suser_name(), host_name(), getdate(), getdate(), '1', 'Start '+@attr, null, null, null
truncate table #targetedStagingRows
select @sql = '
insert into #targetedStagingRows(itemId, localeId, attrVal) select il.itemId, il.localeId, max(ext.AttributeValue)
from [stage].ItemLocaleExportStaging il JOIN ItemAttributes_Locale_' + @Region + '_Ext ext ON il.ItemId = ext.ItemID AND il.LocaleId = ext.LocaleID  
where ext.AttributeId = ' + cast(@UnwrappedTareWeight as nvarchar) + ' and il.GroupId between ' + cast(@StartRange as nvarchar) + ' and ' + cast(@EndRange as nvarchar) + ' group by il.itemid, il.localeid option (recompile);'
print @sql
exec sp_executesql @sql

while exists (select top 1 itemId from #targetedStagingRows)
begin
	insert into app.applog select /*job scheduler*/18, 'Info', 'Amazon Load', suser_name(), host_name(), getdate(), getdate(), '1', ('Remaining rows: '+ cast((select count(itemId) from #targetedStagingRows) as varchar)), null, null, null
	truncate table #targetedStagingRowsCurrentPass;
	insert into #targetedStagingRowsCurrentPass select top 300000 * from #targetedStagingRows;
	update il set [Unwrapped Tare Weight] = tpass.attrVal from [stage].ItemLocaleExportStaging il join #targetedStagingRowsCurrentPass tPass on il.itemId = tPass.itemId and il.localeId = tpass.localeId;
	delete tRows from #targetedStagingRows tRows join #targetedStagingRowsCurrentPass tPass on tRows.itemId = tPass.itemId and tRows.localeId = tPass.localeId;
end
insert into app.applog select /*job scheduler*/18, 'Info', 'Amazon Load', suser_name(), host_name(), getdate(), getdate(), '1', 'DONE: '+@attr, null, null, null
-------------------------
-------------------------
select @attr = '@posScaleTare'
insert into app.applog select /*job scheduler*/18, 'Info', 'Amazon Load', suser_name(), host_name(), getdate(), getdate(), '1', 'Start '+@attr, null, null, null
truncate table #targetedStagingRows
select @sql = '
insert into #targetedStagingRows(itemId, localeId, attrVal) select il.itemId, il.localeId, max(ext.AttributeValue)
from [stage].ItemLocaleExportStaging il JOIN ItemAttributes_Locale_' + @Region + '_Ext ext ON il.ItemId = ext.ItemID AND il.LocaleId = ext.LocaleID  
where ext.AttributeId = ' + cast(@posScaleTare as nvarchar) + ' and il.GroupId between ' + cast(@StartRange as nvarchar) + ' and ' + cast(@EndRange as nvarchar) + ' group by il.itemid, il.localeid option (recompile);'
print @sql
exec sp_executesql @sql

while exists (select top 1 itemId from #targetedStagingRows)
begin
	insert into app.applog select /*job scheduler*/18, 'Info', 'Amazon Load', suser_name(), host_name(), getdate(), getdate(), '1', ('Remaining rows: '+ cast((select count(itemId) from #targetedStagingRows) as varchar)), null, null, null
	truncate table #targetedStagingRowsCurrentPass;
	insert into #targetedStagingRowsCurrentPass select top 300000 * from #targetedStagingRows;
	update il set [PosScaleTare] = tpass.attrVal from [stage].ItemLocaleExportStaging il join #targetedStagingRowsCurrentPass tPass on il.itemId = tPass.itemId and il.localeId = tpass.localeId;
	delete tRows from #targetedStagingRows tRows join #targetedStagingRowsCurrentPass tPass on tRows.itemId = tPass.itemId and tRows.localeId = tPass.localeId;
end
insert into app.applog select /*job scheduler*/18, 'Info', 'Amazon Load', suser_name(), host_name(), getdate(), getdate(), '1', 'DONE: '+@attr, null, null, null
-------------------------
-------------------------
select @attr = '@lockedForSale'
insert into app.applog select /*job scheduler*/18, 'Info', 'Amazon Load', suser_name(), host_name(), getdate(), getdate(), '1', 'Start '+@attr, null, null, null
truncate table #targetedStagingRows
select @sql = '
insert into #targetedStagingRows(itemId, localeId, attrVal) select il.itemId, il.localeId, max(ext.AttributeValue)
from [stage].ItemLocaleExportStaging il JOIN ItemAttributes_Locale_' + @Region + '_Ext ext ON il.ItemId = ext.ItemID AND il.LocaleId = ext.LocaleID  
where ext.AttributeId = ' + cast(@lockedForSale as nvarchar) + ' and il.GroupId between ' + cast(@StartRange as nvarchar) + ' and ' + cast(@EndRange as nvarchar) + ' group by il.itemid, il.localeid option (recompile);'
print @sql
exec sp_executesql @sql

while exists (select top 1 itemId from #targetedStagingRows)
begin
	insert into app.applog select /*job scheduler*/18, 'Info', 'Amazon Load', suser_name(), host_name(), getdate(), getdate(), '1', ('Remaining rows: '+ cast((select count(itemId) from #targetedStagingRows) as varchar)), null, null, null
	truncate table #targetedStagingRowsCurrentPass;
	insert into #targetedStagingRowsCurrentPass select top 300000 * from #targetedStagingRows;
	update il set [LockedForSale] = tpass.attrVal from [stage].ItemLocaleExportStaging il join #targetedStagingRowsCurrentPass tPass on il.itemId = tPass.itemId and il.localeId = tpass.localeId;
	delete tRows from #targetedStagingRows tRows join #targetedStagingRowsCurrentPass tPass on tRows.itemId = tPass.itemId and tRows.localeId = tPass.localeId;
end
insert into app.applog select /*job scheduler*/18, 'Info', 'Amazon Load', suser_name(), host_name(), getdate(), getdate(), '1', 'DONE: '+@attr, null, null, null
-------------------------
-------------------------
select @attr = '@UseByEAB'
insert into app.applog select /*job scheduler*/18, 'Info', 'Amazon Load', suser_name(), host_name(), getdate(), getdate(), '1', 'Start '+@attr, null, null, null
truncate table #targetedStagingRows
select @sql = '
insert into #targetedStagingRows(itemId, localeId, attrVal) select il.itemId, il.localeId, max(ext.AttributeValue)
from [stage].ItemLocaleExportStaging il JOIN ItemAttributes_Locale_' + @Region + '_Ext ext ON il.ItemId = ext.ItemID AND il.LocaleId = ext.LocaleID  
where ext.AttributeId = ' + cast(@UseByEAB as nvarchar) + ' and il.GroupId between ' + cast(@StartRange as nvarchar) + ' and ' + cast(@EndRange as nvarchar) + ' group by il.itemid, il.localeid option (recompile);'
print @sql
exec sp_executesql @sql

while exists (select top 1 itemId from #targetedStagingRows)
begin
	insert into app.applog select /*job scheduler*/18, 'Info', 'Amazon Load', suser_name(), host_name(), getdate(), getdate(), '1', ('Remaining rows: '+ cast((select count(itemId) from #targetedStagingRows) as varchar)), null, null, null
	truncate table #targetedStagingRowsCurrentPass;
	insert into #targetedStagingRowsCurrentPass select top 300000 * from #targetedStagingRows;
	update il set [Use By EAB] = tpass.attrVal from [stage].ItemLocaleExportStaging il join #targetedStagingRowsCurrentPass tPass on il.itemId = tPass.itemId and il.localeId = tpass.localeId;
	delete tRows from #targetedStagingRows tRows join #targetedStagingRowsCurrentPass tPass on tRows.itemId = tPass.itemId and tRows.localeId = tPass.localeId;
end
insert into app.applog select /*job scheduler*/18, 'Info', 'Amazon Load', suser_name(), host_name(), getdate(), getdate(), '1', 'DONE: '+@attr, null, null, null
-------------------------
-------------------------
select @attr = '@WrappedTareWeight'
insert into app.applog select /*job scheduler*/18, 'Info', 'Amazon Load', suser_name(), host_name(), getdate(), getdate(), '1', 'Start '+@attr, null, null, null
truncate table #targetedStagingRows
select @sql = '
insert into #targetedStagingRows(itemId, localeId, attrVal) select il.itemId, il.localeId, max(ext.AttributeValue)
from [stage].ItemLocaleExportStaging il JOIN ItemAttributes_Locale_' + @Region + '_Ext ext ON il.ItemId = ext.ItemID AND il.LocaleId = ext.LocaleID  
where ext.AttributeId = ' + cast(@WrappedTareWeight as nvarchar) + ' and il.GroupId between ' + cast(@StartRange as nvarchar) + ' and ' + cast(@EndRange as nvarchar) + ' group by il.itemid, il.localeid option (recompile);'
print @sql
exec sp_executesql @sql

while exists (select top 1 itemId from #targetedStagingRows)
begin
	insert into app.applog select /*job scheduler*/18, 'Info', 'Amazon Load', suser_name(), host_name(), getdate(), getdate(), '1', ('Remaining rows: '+ cast((select count(itemId) from #targetedStagingRows) as varchar)), null, null, null
	truncate table #targetedStagingRowsCurrentPass;
	insert into #targetedStagingRowsCurrentPass select top 300000 * from #targetedStagingRows;
	update il set [Wrapped Tare Weight] = tpass.attrVal from [stage].ItemLocaleExportStaging il join #targetedStagingRowsCurrentPass tPass on il.itemId = tPass.itemId and il.localeId = tpass.localeId;
	delete tRows from #targetedStagingRows tRows join #targetedStagingRowsCurrentPass tPass on tRows.itemId = tPass.itemId and tRows.localeId = tPass.localeId;
end
insert into app.applog select /*job scheduler*/18, 'Info', 'Amazon Load', suser_name(), host_name(), getdate(), getdate(), '1', 'DONE: '+@attr, null, null, null
-------------------------
-------------------------

-- Work tables to manage item-locale-supplier updates.
if object_id('tempdb..#targetedILSRows') is not null drop table #targetedILSRows
create table #targetedILSRows (
	Region nchar(2) not null,
	ItemLocaleSupplierId int not null,
	PRIMARY KEY CLUSTERED (Region, ItemLocaleSupplierId)
)

if object_id('tempdb..#targetedILSRowsCurrentPass') is not null drop table #targetedILSRowsCurrentPass
create table #targetedILSRowsCurrentPass (
	Region nchar(2) not null,
	ItemLocaleSupplierId int not null,
	PRIMARY KEY CLUSTERED (Region, ItemLocaleSupplierId)
)

insert into app.applog select /*job scheduler*/18, 'Info', 'Amazon Load', suser_name(), host_name(), getdate(), getdate(), '1', 'Starting Item-Locale Supplier update', null, null, null

declare @targetedILSRows int

select @sql = '
insert into #targetedILSRows
select ''' + @Region + ''', ils.ItemLocaleSupplierId 
FROM [stage].ItemLocaleExportStaging il
join dbo.ItemLocale_Supplier_' + @Region + ' ils
on ils.ItemID = il.ItemId
and ils.BusinessUnitID = il.BusinessUnitId
and il.GroupId between ' + cast(@StartRange as nvarchar) + ' and ' + cast(@EndRange as nvarchar) + '
option (recompile);'
print @sql
exec sp_executesql @sql
select @targetedILSRows = @@ROWCOUNT

insert into app.applog select /*job scheduler*/18, 'Info', 'Amazon Load', suser_name(), host_name(), getdate(), getdate(), '1', ('TargetedILSRows='+cast(@targetedILSRows as varchar)), null, null, null

while exists (select top 1 region from #targetedILSRows)
begin
	insert into app.applog select /*job scheduler*/18, 'Info', 'Amazon Load', suser_name(), host_name(), getdate(), getdate(), '1', ('Remaining ILS rows: '+ cast((select count(Region) from #targetedILSRows) as varchar)), null, null, null
	truncate table #targetedILSRowsCurrentPass;
	insert into #targetedILSRowsCurrentPass select top 300000 * from #targetedILSRows;

	select @sql = '
	update il set [SupplierName] = ils.SupplierName,[IrmaVendorKey] = ils.IrmaVendorKey,[SupplierItemID] = ils.SupplierItemID,[SupplierCaseSize] = ils.SupplierCaseSize
		from [stage].ItemLocaleExportStaging il join dbo.ItemLocale_Supplier_' + @Region + ' ils on ils.ItemID = il.ItemId and ils.BusinessUnitID = il.BusinessUnitId
		join #targetedILSRowsCurrentPass tPass on tPass.region = ils.region and tPass.ItemLocaleSupplierId = ils.ItemLocaleSupplierId option (recompile);
	'
	print @sql
	exec sp_executesql @sql

	delete tRows from #targetedILSRows tRows join #targetedILSRowsCurrentPass tPass on tRows.region = tPass.region and tRows.ItemLocaleSupplierId = tPass.ItemLocaleSupplierId;
end -- loop through #targetedILSRows


insert into app.applog select /*job scheduler*/18, 'Info', 'Amazon Load', suser_name(), host_name(), getdate(), getdate(), '1', 'DONE: Item-Locale Supplier update', null, null, null

-- Drop added index to help with groupId.
drop index [IX_ItemLocaleExportStaging_GroupId_Item_Locale] on stage.[ItemLocaleExportStaging]
    
    
 SET @timestamp = GETDATE();    
 SET @msg = CONVERT(VARCHAR, @timestamp, 120) + ': attributes updated'    
    
 RAISERROR (    
   @msg,    
   0,    
   1    
   )    
 WITH NOWAIT;    
      
 SELECT @rowCount AS [rowCount]    
END  
GO

GRANT EXECUTE ON stage.ItemLocaleExport TO [MammothRole];
GO