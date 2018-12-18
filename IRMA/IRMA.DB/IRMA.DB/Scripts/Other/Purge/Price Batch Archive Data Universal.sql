/*
Price-Batch Data Purge Process


---------------------------------------------------------------------------------------------------------------------------------------------------------------------
---------------------------------------------------------------------------------------------------------------------------------------------------------------------

KEY NOTES:
This script was checked into source control Dec, 2018.  It's name at that point was "Price Batch Archive Data Universal v2016.6".
It does not make sense to store the version # in the filename, since it's in Git now, however, comments in the change log below
should be made for EVERY edit, to ensure appropriate tracking of "versions" (exact logic herein).


A key setting in this script is '@getCountsOnly'.  This controls whether or not the code herein actually makes changes to the
DB it's run against (purges data from tables).  If set to '1', it only analyzes the data to be purged and displays that info.
If set to '0', it archives data.  Make sure you know how this is set before running.

---------------------------------------------------------------------------------------------------------------------------------------------------------------------
---------------------------------------------------------------------------------------------------------------------------------------------------------------------


Tom Lux
2/15/2010

	
--------------------------------------------------------------------------------------------------------------------------------
--------------------------------------------------------------------------------------------------------------------------------
Change Log	
--------------------------------------------------------------------------------------------------------------------------------
Date				Developer				Desc
--------------------------------------------------------------------------------------------------------------------------------
2010-07-15				Brian R				Combined dynamic archive table creation with index work.
2010-08-27				Tom L				Added table containing 'old PBD item change' rows to the master list table (it wasn't being added, so none of these rows were being purged).
2011-10-13				Tom L				Added config option '@oldPBDItemChgPurgeIncludeNew' to include the "NEW" item-change type in the "old item change purge" category.  Previously, only the "ITEM" item-change type was included
														but now, if this option is set to TRUE, both item and new change-type entries will be purged.
2012-09-12				Tom L				Updated index names (renamed in IRMA v4.6.0).
2015-05-06				Tom L				Updates herein are marked with "v2015.5", so you can search for and locate them easily, if desired.
&											1. Updated get-processed-batches subquery that pulls most recent REG to only include non-deleted items (should greatly reduce subquery result set size).
2015-05-19									2. The query for processed batches was a litte "off" because the most-recent-REG subquery goes off price type, but then the main/outer query joining to it does not,
											it just makes sure the PBD ID is older.  A filter was added to the WHERE clause of the outer query to filter price type.
											3. All batched and unbatched PBD entries linked to an Item record that is marked deleted are now purged.
											4. Merged and added conditional logic around index maintenance because the SO region has different indexes on PBD than all other regions.  This removes need for two different scripts.
											Because some indexes exist in some regions, but are disabled, we must keep track of each active index as we disable it, 
											so we can be sure we only rebuild (activate) the indexes that were active when the script started.
2016-06-06				Tom L				These updates are marked with "v2016.6", so you can search for and locate them easily, if desired.
											1. Added logic to capture the most recent, pushed (batch header status = PROCESSED) entries for all store items so that these
											entries can be preserved (not purged).  If all pushed entries are purged for a store-item that's on sale and there's an already-started
											pending REG, it will appear as batchable in the price-batch-detail search screen, which is bad, as it would prematurely take the item off sale,
											if the REG is batched and pushed accidentally. 
											Added temp tables #latestPushedDateAllSales and #latestPushedEntryAllSales.
											We use two tables because we are taking two passes to exactly identify
											the entries we will preserve.  We first find the most recent entry for all store items on sale.  Then we join our latest-pushed-dates back to PBD 
											to get the PBD-ID for the these entries.  These entries-to-preserve are then removed from the PBD-purge list.
											Some new logging statements:	
											'Building list of latest, pushed sale dates for all items so these entries can be excluded from the purge...'
											'Retrieving entry IDs for latest, pushed sale dates...'
											'Removing latest-pushed entries from processed-active-item PBD list...'
											'Rows removed to preserve latest-pushed entries: '...
											'Updated processed-active-item PBD rows to be purged: '...


*/

use itemcatalog

declare @runTime datetime, @runUser varchar(128), @runHost varchar(128), @runDBName varchar(128), @runDBRO varchar(128), @runDBAccess varchar(128), @runDBState varchar(128)
select @runTime = getdate(), @runUser = suser_name(), @runHost = host_name(), @runDBName = db_name()
select @runDBRO = is_read_only, @runDBAccess = user_access_desc, @runDBState = state_desc from sys.databases where name like db_name()
print '---------------------------------------------------------------------------------------'
print 'Current System Time: ' + convert(varchar, @runTime, 121)
print 'User Name: ' + @runUser
print 'Running From Host: ' + @runHost
print 'Connected To DB Server: ' + @@servername
print 'DB Name: ' + @runDBName
print 'DB Read-Only: ' + @runDBRO
print 'DB Access: ' + @runDBAccess
print 'DB State: ' + @runDBState
print '---------------------------------------------------------------------------------------'
go

set nocount on

/*
#############################################################################
Configuration

There is a date-limit param for each purge category.
If a param is null, the purge for that category is not performed.

The commit frequency is the number of rows that will be purged before each commit.

Processed Batches Purge:
Date limit applies to PBH.processeddate.
*NOTE: Processed PBDs for items marked deleted are including in this category but are NOT affected by the date limit.

Expired PBD Purge:
Date limit applies to PBD.insert_date.

Old PBD Sale Purge:
Date limit applies to PBD.sale_end_date.

Old Item/New Change Type Purge:
Date limit applies to PBD.insert_date.

Unbatched, deleted PBD purge:
Date limit applies to PBD.insert_date.

#############################################################################
*/


declare
	@processedBatchesPurgeDateLimit datetime
	,@expiredPBDPurgeDateLimit datetime
	,@oldPBDSalePurgeDateLimit datetime
	,@oldPBDItemChgPurgeDateLimit datetime
	,@oldPBDItemChgPurgeIncludeNew bit
	,@processedBatchesPurgeRowLimit int
	,@expiredPBDPurgeRowLimit int
	,@oldPBDSalePurgeRowLimit int
	,@oldPBDItemChgPurgeRowLimit int
	-- v2015.5: New unbatched-deleted-items category.
	,@unbatchedDelPBDPurgeDateLimit datetime
	,@unbatchedDelPBDPurgeRowLimit int
	,@commitFrequency int
	,@getCountsOnly bit
	,@rowIndex int
	,@processedBatchesPurgeCount int
	,@expiredPBDPurgeCount int
	,@oldPBDSalePurgeCount int
	,@oldPBDItemChgPurgeCount int
	,@unbatchedDelPBDPurgeCount int
	,@pbdTotalPurgeCount int
	,@pbhTotalPurgeCount int
	,@wrkListAffected int
	,@wrkListLeft int
	,@indexName varchar(128)
	,@sql varchar(max)

-- This script covers all PBH and PBD indexes across all regions, which are not the same in each region,
-- so we capture each active/valid index as we disable it, so we can only enable those indexes after the purge.
-- There are disabled indexes that still exist in some regions and we do not want to resurrect those inadvertently.
declare @activeIndexList table (name varchar(128) not null)

select
	/*
		The next five dates control whether or not each purge-category process is performed,
		and then, of course, set a limit for how much data is retained.
		If a date-limit param is NULL, that disables the purge category.
	*/
	@processedBatchesPurgeDateLimit = getdate() - 180
	,@expiredPBDPurgeDateLimit = getdate()
	,@oldPBDSalePurgeDateLimit = getdate()
	,@oldPBDItemChgPurgeDateLimit = getdate() - 60
	-- v2015.5: New unbatched-deleted-items category.
	,@unbatchedDelPBDPurgeDateLimit = getdate()
	-- The row limits have to be > 0 to take effect.
	,@processedBatchesPurgeRowLimit = null
	,@expiredPBDPurgeRowLimit = null
	,@oldPBDSalePurgeRowLimit = null
	,@oldPBDItemChgPurgeRowLimit = null
	,@unbatchedDelPBDPurgeRowLimit = null
	,@oldPBDItemChgPurgeIncludeNew = 1
	,@commitFrequency = 100000
	,@getCountsOnly = 0
	,@rowIndex = 0 -- This should always be zero.  This is the index used to track each set of price-batch rows being purged.


-- Different limits for SO region.
if @@servername like '%\sod' or @@servername like '%\soq' or @@servername like '%\sop'
begin
	select @processedBatchesPurgeDateLimit = getdate() - 90
end


	-- We only check for the archive tables if we are actually going to be purging data.
	if @getCountsOnly = 0
	begin
		-- drop previous archive tables.
		-- PBH:
		if exists (select * from sys.objects where object_id = object_id(N'[dbo].[PriceBatchHeaderArchive]') and type in (N'U'))
		begin
			drop TABLE [dbo].[PriceBatchHeaderArchive]
		end
		-- PBD:
		if  exists (select * from sys.objects where object_id = object_id(N'[dbo].[PriceBatchDetailArchive]') and type in (N'U'))
		begin
			drop table [dbo].[PriceBatchDetailArchive]
		end
		-- create new ones to match current schema
		DECLARE @pbdColumnList varchar(4000)
		SELECT @pbdColumnList = COALESCE(@pbdColumnList + ',', '') +  COLUMN_NAME + ' ' + DATA_TYPE  
					+ case when CHARACTER_MAXIMUM_LENGTH IS NULL then '' else ' (' + CONVERT(varchar,CHARACTER_MAXIMUM_LENGTH) + ')' end
					+ ' NULL'
		FROM information_schema.columns WHERE table_name = 'PriceBatchDetail' ORDER BY ORDINAL_POSITION
		print @pbdColumnList
		set @sql = 'create table dbo.PriceBatchDetailArchive (' + @pbdColumnList + ')'
		print @sql
		exec(@sql)
		
		DECLARE @pbhColumnList varchar(4000)
		SELECT @pbhColumnList = COALESCE( @pbhColumnList + ',', '') +  COLUMN_NAME + ' ' + DATA_TYPE  
					+ case when CHARACTER_MAXIMUM_LENGTH IS NULL then '' else ' (' + CONVERT(varchar,CHARACTER_MAXIMUM_LENGTH) + ')' end
					+ ' NULL'
		FROM information_schema.columns WHERE table_name = 'PriceBatchHeader' ORDER BY ORDINAL_POSITION
		print @pbhColumnList
		set @sql = 'create table dbo.PriceBatchHeaderArchive (' + @pbhColumnList + ')'
		print @sql
		exec(@sql)
	end


/*
End Configuration
#############################################################################
*/

print '[' + convert(nvarchar, getdate(), 121) + '] ' + 'Configuration:'
print '@processedBatchesPurgeDateLimit=' + isnull(convert(varchar, @processedBatchesPurgeDateLimit), 'null') 
print '@processedBatchesPurgeRowLimit=' + isnull(convert(varchar, @processedBatchesPurgeRowLimit), 'null') 
print '@expiredPBDPurgeDateLimit=' + isnull(convert(varchar, @expiredPBDPurgeDateLimit), 'null') 
print '@expiredPBDPurgeRowLimit=' + isnull(convert(varchar, @expiredPBDPurgeRowLimit), 'null') 
print '@oldPBDSalePurgeDateLimit=' + isnull(convert(varchar, @oldPBDSalePurgeDateLimit), 'null')
print '@oldPBDSalePurgeRowLimit=' + isnull(convert(varchar, @oldPBDSalePurgeRowLimit), 'null')
print '@oldPBDItemChgPurgeDateLimit=' + isnull(convert(varchar, @oldPBDItemChgPurgeDateLimit), 'null')
print '@oldPBDItemChgPurgeRowLimit=' + isnull(convert(varchar, @oldPBDItemChgPurgeRowLimit), 'null')
-- v2015.5: New unbatched-deleted-items category.
print '@unbatchedDelPBDPurgeDateLimit=' + isnull(convert(varchar, @unbatchedDelPBDPurgeDateLimit), 'null')
print '@unbatchedDelPBDPurgeRowLimit=' + isnull(convert(varchar, @unbatchedDelPBDPurgeRowLimit), 'null')
print '@oldPBDItemChgPurgeIncludeNew=' + isnull(convert(varchar, @oldPBDItemChgPurgeIncludeNew), 'null')
print '@commitFrequency=' + isnull(convert(varchar, @commitFrequency), 'null')

-- Ensure row-limit params are not null.
select @processedBatchesPurgeRowLimit = isnull(@processedBatchesPurgeRowLimit, 0)
select @expiredPBDPurgeRowLimit = isnull(@expiredPBDPurgeRowLimit, 0)
select @oldPBDSalePurgeRowLimit = isnull(@oldPBDSalePurgeRowLimit, 0)
select @oldPBDItemChgPurgeRowLimit = isnull(@oldPBDItemChgPurgeRowLimit, 0)
select @unbatchedDelPBDPurgeRowLimit = isnull(@unbatchedDelPBDPurgeRowLimit, 0)

/*
	These 'Saved' tables holds refs for all rows to be archived and are not updated
	(nothing deleted from these lists).
	We use the identity column to control/limit the max rows for each purge category.
*/
-- HEADER
if object_id('tempdb..#pbhArchiveFullListSaved') is not null
begin
	drop table #pbhArchiveFullListSaved
end
create table #pbhArchiveFullListSaved (
	rowid int identity(1,1),
	pricebatchheaderid int not null
	PRIMARY KEY (
		pricebatchheaderid
	)
)
-- DETAIL
if object_id('tempdb..#pbdArchiveFullListSaved') is not null
begin
	drop table #pbdArchiveFullListSaved
end
create table #pbdArchiveFullListSaved (
	rowid int identity(1,1),
	pricebatchdetailid int not null
	PRIMARY KEY (
		pricebatchdetailid
	)
)

-- Purge-specific temp tables.
if object_id('tempdb..#processedBatchesPurgeList') is not null
begin
	drop table #processedBatchesPurgeList
end
create table #processedBatchesPurgeList (
	rowid int identity(1,1),
	pricebatchdetailid int not null
	PRIMARY KEY (
		pricebatchdetailid
	)
)
if object_id('tempdb..#expiredPBDPurgeList') is not null
begin
	drop table #expiredPBDPurgeList
end
create table #expiredPBDPurgeList (
	rowid int identity(1,1),
	pricebatchdetailid int not null
	PRIMARY KEY (
		pricebatchdetailid
	)
)
if object_id('tempdb..#oldPBDSalePurgeList') is not null
begin
	drop table #oldPBDSalePurgeList
end
create table #oldPBDSalePurgeList (
	rowid int identity(1,1),
	pricebatchdetailid int not null
	PRIMARY KEY (
		pricebatchdetailid
	)
)
if object_id('tempdb..#oldPBDItemChgPurgeList') is not null
begin
	drop table #oldPBDItemChgPurgeList
end
create table #oldPBDItemChgPurgeList (
	rowid int identity(1,1),
	pricebatchdetailid int not null
	PRIMARY KEY (
		pricebatchdetailid
	)
)
if object_id('tempdb..#unbatchedDelPBDPurgeList') is not null
begin
	drop table #unbatchedDelPBDPurgeList
end
create table #unbatchedDelPBDPurgeList (
	rowid int identity(1,1),
	pricebatchdetailid int not null
	PRIMARY KEY (
		pricebatchdetailid
	)
)



/*
	These tables hold refs for all rows not yet archived, so as the rows are archived,
	they are removed from these table (hence these are the "working" lists).
	We use an identity column in these tables so that we can iterate through sets or groups
	of rows to be purged.
*/
-- HEADER
if object_id('tempdb..#pbhArchiveFullListWorking') is not null
begin
	drop table #pbhArchiveFullListWorking
end
create table #pbhArchiveFullListWorking (
	rowid int identity(1,1),
	pricebatchheaderid int not null
	PRIMARY KEY (
		pricebatchheaderid
	)
)
-- DETAIL
if object_id('tempdb..#pbdArchiveFullListWorking') is not null
begin
	drop table #pbdArchiveFullListWorking
end
create table #pbdArchiveFullListWorking (
	rowid int identity(1,1),
	pricebatchdetailid int not null
	PRIMARY KEY (
		pricebatchdetailid
	)
)

/*
	These tables hold the refs for the current set of rows that will be archived.
*/
-- HEADER
if object_id('tempdb..#pbhArchiveCurrentSet') is not null
begin
	drop table #pbhArchiveCurrentSet
end
create table #pbhArchiveCurrentSet (
	pricebatchheaderid int not null
	PRIMARY KEY (
		pricebatchheaderid
	)
)
-- DETAIL
if object_id('tempdb..#pbdArchiveCurrentSet') is not null
begin
	drop table #pbdArchiveCurrentSet
end
create table #pbdArchiveCurrentSet (
	pricebatchdetailid int not null
	PRIMARY KEY (
		pricebatchdetailid
	)
)

/*

###########################################################################
###########################################################################

PBH/PBD Purge Process

###########################################################################
###########################################################################

*/

declare @spaceUsed table (
	name varchar(128)
	,rows bigint
	,reserved varchar(64)
	,data varchar(64)
	,indexSize varchar(64)
	,unused varchar(64)
)

-- Get current space stats for target tables.
insert into @spaceused exec sp_spaceused N'pricebatchheader'
insert into @spaceused exec sp_spaceused N'pricebatchdetail'
if exists (select * from sys.objects where object_id = object_id(N'[dbo].pricebatchheaderarchive') and type in (N'U'))
	insert into @spaceused exec sp_spaceused N'pricebatchheaderarchive'
else
	print '[No data for PBH-archive table; table not found.]'
if exists (select * from sys.objects where object_id = object_id(N'[dbo].[pricebatchdetailarchive]') and type in (N'U'))
	insert into @spaceused exec sp_spaceused N'pricebatchdetailarchive'
else
	print '[No data for PBD-archive table; table not found.]'

print '[' + convert(nvarchar, getdate(), 121) + '] ' + 'Current PBH & PBD table space attributes:'
select * from @spaceused
-- Clear space table for next use.
delete from @spaceused


if @getCountsOnly = 1
begin
	print ''
	print '***************'
	print '---> Delete process has been disabled!'
	print '---> Only counts of rows targeted for removal will be displayed.'
	print '***************'
	print ''
end

print '[' + convert(nvarchar, getdate(), 121) + '] ' + 'Building lists of IDs to be deleted...'

if @processedBatchesPurgeDateLimit is not null
begin
	/*
		Design Note
		We build the list of PBD rows to be purged here.
		The PBH row corresponding to a set of PBD rows is only removed if all the PBD rows are removed.
		In the case where we leave the original REG PBD in a batch, we can't remove the PBH.
	*/
	print '[' + convert(nvarchar, getdate(), 121) + '] ' + 'Getting processed, active-item PBD IDs to be purged...'
	insert into #processedBatchesPurgeList
		select pbd.pricebatchdetailid
		from pricebatchheader pbh (nolock)
		join pricebatchstatus pbs (nolock) on pbh.pricebatchstatusid = pbs.pricebatchstatusid
		join pricebatchdetail pbd (nolock) on pbh.pricebatchheaderid = pbd.pricebatchheaderid
		left join pricechgtype pct (nolock) on pbd.pricechgtypeid = pct.pricechgtypeid
		join 
		(	-- Grab most recent REG.
			-- v2015.5, added join to Item table to exclude deleted items.
			select
				_pbd.item_key 
				,_pbd.store_no
				,pricebatchdetailid = max(_pbd.pricebatchdetailid)
			from item i (nolock) 
			join pricebatchdetail _pbd (nolock) on i.item_key = _pbd.item_key
			join pricechgtype pct (nolock) on _pbd.pricechgtypeid = pct.pricechgtypeid
			where
				i.deleted_item = 0
				and _pbd.expired = 0
				and lower(rtrim(ltrim(pct.pricechgtypedesc))) = 'reg'
			group by _pbd.item_key, _pbd.store_no
		) dreg
			on pbd.item_key = dreg.item_key
			and pbd.store_no = dreg.store_no
		where
			(	-- v2015.5: Restricting to only REGs.
				lower(rtrim(ltrim(isnull(pct.pricechgtypedesc, '')))) <> 'reg' -- If PBD if a REG, make sure it is not most recent.
				or
				pbd.pricebatchdetailid < dreg.pricebatchdetailid
			)
			and pbh.processeddate < @processedBatchesPurgeDateLimit
			and lower(rtrim(ltrim(pbs.pricebatchstatusdesc))) = 'processed' -- Pushed/processed batches only.

	select @processedBatchesPurgeCount = @@rowcount
	print '[' + convert(nvarchar, getdate(), 121) + '] ' + 'Processed, active-item PBD rows to be purged: ' + convert(varchar, @processedBatchesPurgeCount)


	/*	v2016.6:
		This next section captures the most recent, pushed (batch header status = PROCESSED) entries for all store items so that these
		entries can be preserved (not purged).  If all pushed entries are purged for a store-item that's on sale and there's an already-started
		pending REG, it will appear as batchable in the price-batch-detail search screen, which is bad, as it would prematurely take the item off sale,
		if the REG is batched and pushed accidentally.
	*/

	if object_id('tempdb..#latestPushedDateAllSales') is not null
	begin
		drop table #latestPushedDateAllSales
	end
	create table #latestPushedDateAllSales (
		Item_Key int not null,
		Store_No int not null,
		PriceChgTypeId tinyint not null,
		Identifier varchar(13) not null,
		ProcessedDate datetime not null
		PRIMARY KEY (
			Item_Key, Store_No, PriceChgTypeId, Identifier, ProcessedDate
		),
		UNIQUE NONCLUSTERED (ProcessedDate, Item_Key, Store_No, PriceChgTypeId)
	)
	;

	if object_id('tempdb..#latestPushedEntryAllSales') is not null
	begin
		drop table #latestPushedEntryAllSales
	end
	create table #latestPushedEntryAllSales (
		PriceBatchDetailID int not null,
		Item_Key int not null,
		Store_No int not null,
		PriceChgTypeId tinyint not null,
		Identifier varchar(13) not null,
		ProcessedDate datetime
		PRIMARY KEY (
			PriceBatchDetailID
		),
		UNIQUE NONCLUSTERED (ProcessedDate, Item_Key, Store_No, PriceChgTypeId, Identifier, PriceBatchDetailID)
	)

	print '[' + convert(nvarchar, getdate(), 121) + '] ' + 'Building list of latest, pushed sale dates for all items so these entries can be excluded from the purge...'

	-- This grabs the most recent pushed entries for all items on sale.
	insert into #latestPushedDateAllSales
	select p.item_key, p.Store_No, pbd.PriceChgTypeId, pbd.Identifier, max(pbh.processeddate)
	from price p 	
	join pricebatchdetail pbd (nolock)
		on p.item_key = pbd.item_key 
		and p.Store_No = pbd.Store_No
	join itemidentifier ii (nolock)
		on pbd.item_key = ii.item_key 
		and ii.Identifier = pbd.Identifier
	join item i (nolock)
		on p.item_key = i.item_key
	join PriceBatchHeader pbh (nolock)
		on pbd.pricebatchheaderid = pbh.pricebatchheaderid
	where
		i.deleted_item = 0
		and ii.Deleted_Identifier = 0
		-- Sale price type (non-REG)
		and p.PriceChgTypeID > 1
		-- Dates make it currently on sale
		and p.Sale_Start_Date < getdate() and p.sale_end_date > getdate()	
		-- We'll leave the pushed-entries sale price types open (don't link to current sale price type) so we can save any nested sales also.
		and pbd.PriceChgTypeId is not null
		and pbh.processeddate is not null -- pushed
	group by
		p.item_key, p.Store_No, pbd.PriceChgTypeId, pbd.Identifier

	print '[' + convert(nvarchar, getdate(), 121) + '] ' + 'Done building latest, pushed sale dates.'

	print '[' + convert(nvarchar, getdate(), 121) + '] ' + 'Retrieving entry IDs for latest, pushed sale dates...'

	/*
	Now we join our latest-pushed-dates back to PBD to get the PBD-ID for the these entries.
	NOTE: There are cases where the most recent pushed entry's start and end dates do not
	match what's in the Price table, but we'll preserve these entries anyway.
	*/
	insert into #latestPushedEntryAllSales
	select
		pbd.PriceBatchDetailID,
		lpd.item_key, 
		lpd.Store_No, 
		lpd.PriceChgTypeId, 
		lpd.Identifier, 
		lpd.processeddate
	from #latestPushedDateAllSales lpd
	join pricebatchdetail pbd (nolock)
		on lpd.item_key = pbd.item_key 
		and lpd.Store_No = pbd.Store_No
		and lpd.Identifier = pbd.Identifier
		and lpd.PriceChgTypeID = pbd.PriceChgTypeId
	join PriceBatchHeader pbh (nolock)
		on pbd.pricebatchheaderid = pbh.pricebatchheaderid
		and lpd.processeddate = pbh.processeddate

	print '[' + convert(nvarchar, getdate(), 121) + '] ' + 'Done retrieving entry IDs for latest, pushed sale dates.'

	print '[' + convert(nvarchar, getdate(), 121) + '] ' + 'Removing latest-pushed entries from processed-active-item PBD list...'
	delete pl
	from #latestPushedEntryAllSales lpe
	join #processedBatchesPurgeList pl
		on lpe.PriceBatchDetailID = pl.pricebatchdetailid

	print '[' + convert(nvarchar, getdate(), 121) + '] ' + 'Rows removed to preserve latest-pushed entries: ' + convert(varchar, @@rowcount)

	select @processedBatchesPurgeCount = count(*) from #processedBatchesPurgeList
	print '[' + convert(nvarchar, getdate(), 121) + '] ' + 'Updated processed-active-item PBD rows to be purged: ' + convert(varchar, @processedBatchesPurgeCount)


	/*
		v2015.5: Add batched, deleted items to processed list.  We do not restrict by date, so any/all deleted items in batches will be marked to be purged.
		A developer can pull data from archive table for investigation after the purge, if needed.
	*/
	print '[' + convert(nvarchar, getdate(), 121) + '] ' + 'Getting processed, deleted-item PBD IDs to be purged...'
	-- PBD entries for deleted items in batches.
	insert into #processedBatchesPurgeList
		select pbd.pricebatchdetailid
		from item i (nolock) 
		join pricebatchdetail pbd (nolock) on i.item_key = pbd.item_key
		left join #processedBatchesPurgeList pbpl on pbd.pricebatchdetailid = pbpl.pricebatchdetailid -- To only insert PBD IDs that are not yet in processed list.
		where
			pbpl.pricebatchdetailid is null -- Only insert PBD IDs that are not yet in the list.
			and pbd.pricebatchheaderid is not null 
			and i.deleted_item = 1


	select @processedBatchesPurgeCount = @@rowcount
	print '[' + convert(nvarchar, getdate(), 121) + '] ' + 'Processed, deleted-item PBD rows to be purged: ' + convert(varchar, @processedBatchesPurgeCount)

	-- Check purge limit.
	if @processedBatchesPurgeRowLimit > 0
	begin
		print '[' + convert(nvarchar, getdate(), 121) + '] ' + 'Applying ' + convert(varchar, @processedBatchesPurgeRowLimit) + ' processed-batches purge limit (removing excess PBD IDs from purge list)...'
		delete #processedBatchesPurgeList
		where rowid > @processedBatchesPurgeRowLimit

		print '[' + convert(nvarchar, getdate(), 121) + '] ' + 'Rows removed for processed-batches purge limit: ' + convert(varchar, @@rowcount)
	end

	print '[' + convert(nvarchar, getdate(), 121) + '] ' + 'Getting processed PBH IDs to be purged...'
	insert into #pbhArchiveFullListSaved
		select h.pricebatchheaderid
		from pricebatchheader h (nolock)
			join pricebatchdetail d (nolock)
				on h.pricebatchheaderid = d.pricebatchheaderid
			join #processedBatchesPurgeList dlist
				on d.pricebatchdetailid = dlist.pricebatchdetailid -- PBH list is restricted to PBD purge list rows that have PBH references.
		group by h.pricebatchheaderid

	select @pbhTotalPurgeCount = @@rowcount
	print '[' + convert(nvarchar, getdate(), 121) + '] ' + 'Processed PBH rows to be purged: ' + convert(varchar, @pbhTotalPurgeCount)
end
else
begin
	print '[' + convert(nvarchar, getdate(), 121) + '] ' + 'Processed-batches purge disabled.'
end


if @expiredPBDPurgeDateLimit is not null
begin
-- We shouldn't need to keep the most recent REG here, because these are expired entries only.
	print '[' + convert(nvarchar, getdate(), 121) + '] ' + 'Getting expired PBD IDs to be purged...'
	insert into #expiredPBDPurgeList
		select pricebatchdetailid
		from pricebatchdetail (nolock)
		where pricebatchheaderid is null
		and expired = 1
		and insert_date < @expiredPBDPurgeDateLimit

	select @expiredPBDPurgeCount = @@rowcount
	print '[' + convert(nvarchar, getdate(), 121) + '] ' + 'Expired PBD rows to be purged: ' + convert(varchar, @expiredPBDPurgeCount)

	-- Check purge limit.
	if @expiredPBDPurgeRowLimit > 0
	begin
		print '[' + convert(nvarchar, getdate(), 121) + '] ' + 'Applying ' + convert(varchar, @expiredPBDPurgeRowLimit) + ' expired-PBD purge limit (removing excess PBD IDs from purge list)...'
		delete #expiredPBDPurgeList
		where rowid > @expiredPBDPurgeRowLimit

		print '[' + convert(nvarchar, getdate(), 121) + '] ' + 'Rows removed for expired-PBD purge limit: ' + convert(varchar, @@rowcount)
	end
end
else
begin
	print '[' + convert(nvarchar, getdate(), 121) + '] ' + 'Expired PBD purge disabled.'
end


if @oldPBDSalePurgeDateLimit is not null
begin
-- We shouldn't need to keep the most recent REG here, because these are non-REG entries only.
	print '[' + convert(nvarchar, getdate(), 121) + '] ' + 'Getting old PBD sale IDs to be purged...'
	insert into #oldPBDSalePurgeList
		select d.pricebatchdetailid
		from pricebatchdetail d (nolock)
			join pricechgtype pct (nolock) on d.pricechgtypeid = pct.pricechgtypeid
		where d.pricebatchheaderid is null
		and d.expired = 0
		and pct.on_sale = 1
		and d.sale_end_date <= @oldPBDSalePurgeDateLimit

	select @oldPBDSalePurgeCount = @@rowcount
	print '[' + convert(nvarchar, getdate(), 121) + '] ' + 'Old PBD sale rows to be purged: ' + convert(varchar, @oldPBDSalePurgeCount)

	-- Check purge limit.
	if @oldPBDSalePurgeRowLimit > 0
	begin
		print '[' + convert(nvarchar, getdate(), 121) + '] ' + 'Applying ' + convert(varchar, @oldPBDSalePurgeRowLimit) + ' old-PBD-sale purge limit (removing excess PBD IDs from purge list)...'
		delete #oldPBDSalePurgeList
		where rowid > @oldPBDSalePurgeRowLimit

		print '[' + convert(nvarchar, getdate(), 121) + '] ' + 'Rows removed for old-PBD-sale purge limit: ' + convert(varchar, @@rowcount)
	end
end
else
begin
	print '[' + convert(nvarchar, getdate(), 121) + '] ' + 'Old PBD sale purge disabled.'
end


if @oldPBDItemChgPurgeDateLimit is not null
begin
-- We shouldn't need to keep the most recent REG here, because these are unbatched item-change entries only.
	print '[' + convert(nvarchar, getdate(), 121) + '] ' + 'Getting old PBD item change IDs to be purged...'
	-- Get "ITEM" item-chg type.
	declare @itemChgTypeId int;
	select @itemChgTypeId = itemchgtypeid
	from itemchgtype (nolock)
	where ltrim(rtrim(lower(itemchgtypedesc))) = 'item' -- This exists for all regions.

	-- Get "NEW" item-chg type.
	declare @newChgTypeId int;
	select @newChgTypeId = itemchgtypeid
	from itemchgtype (nolock)
	where ltrim(rtrim(lower(itemchgtypedesc))) = 'new' -- This exists for all regions.

	-- If the "NEW" item change type should not be included in this purge category, we set the new-chg-type var to an invalid value and the data-retrieval query will work normally.
	if @oldPBDItemChgPurgeIncludeNew = 0
		select @newChgTypeId = -1

	insert into #oldPBDItemChgPurgeList
		select d.pricebatchdetailid
		from pricebatchdetail d (nolock)
		where d.pricebatchheaderid is null
		and d.expired = 0
		and d.itemchgtypeid in (@itemChgTypeId, @newChgTypeId)
		and d.insert_date < @oldPBDItemChgPurgeDateLimit

	select @oldPBDItemChgPurgeCount = @@rowcount
	print '[' + convert(nvarchar, getdate(), 121) + '] ' + 'Old PBD item change rows to be purged: ' + convert(varchar, @oldPBDItemChgPurgeCount)

	-- Check purge limit.
	if @oldPBDItemChgPurgeRowLimit > 0
	begin
		print '[' + convert(nvarchar, getdate(), 121) + '] ' + 'Applying ' + convert(varchar, @oldPBDItemChgPurgeRowLimit) + ' old-PBD-ItemChg purge limit (removing excess PBD IDs from purge list)...'
		delete #oldPBDItemChgPurgeList
		where rowid > @oldPBDItemChgPurgeRowLimit

		print '[' + convert(nvarchar, getdate(), 121) + '] ' + 'Rows removed for old-item-change purge limit: ' + convert(varchar, @@rowcount)
	end
end
else
begin
	print '[' + convert(nvarchar, getdate(), 121) + '] ' + 'Old PBD item change purge disabled.'
end


-- v2015.5: New unbatched-deleted-items category.
if @unbatchedDelPBDPurgeDateLimit is not null
begin
	-- Add unbatched, deleted items to unbatched-del list.  
	print '[' + convert(nvarchar, getdate(), 121) + '] ' + 'Getting unbatched, deleted-item PBD IDs to be purged...'
	-- Unbatched PBD entries for deleted items.
	insert into #unbatchedDelPBDPurgeList
		select pbd.pricebatchdetailid
		from item i (nolock) 
		join pricebatchdetail pbd (nolock) on i.item_key = pbd.item_key
		where
			pbd.pricebatchheaderid is null 
			and i.deleted_item = 1
			and pbd.insert_date < @unbatchedDelPBDPurgeDateLimit

	select @unbatchedDelPBDPurgeCount = @@rowcount
	print '[' + convert(nvarchar, getdate(), 121) + '] ' + 'Initial unbatched-deleted rows to be purged: ' + convert(varchar, @unbatchedDelPBDPurgeCount)

	-- Check purge limit.
	if @unbatchedDelPBDPurgeRowLimit > 0
	begin
		print '[' + convert(nvarchar, getdate(), 121) + '] ' + 'Applying ' + convert(varchar, @unbatchedDelPBDPurgeRowLimit) + ' unbatched-deleted-PBD purge limit (removing excess PBD IDs from purge list)...'
		delete #unbatchedDelPBDPurgeList
		where rowid > @unbatchedDelPBDPurgeRowLimit

		print '[' + convert(nvarchar, getdate(), 121) + '] ' + 'Rows removed for unbatched-deleted-PBD purge limit: ' + convert(varchar, @@rowcount)
	end

	/*
		Remove any items from unbatched-deleted list that already exist in the other unbatched lists (no dups allowed in full archive list):
		1. #expiredPBDPurgeList
		2. #oldPBDSalePurgeList
		3. #oldPBDItemChgPurgeList
	*/
	print '[' + convert(nvarchar, getdate(), 121) + '] ' + 'Removing unbatched, deleted-item PBD IDs that already exist in other unbatched purge lists...'
	delete #unbatchedDelPBDPurgeList
	from #unbatchedDelPBDPurgeList ud
	left join #expiredPBDPurgeList ex on ud.pricebatchdetailid = ex.pricebatchdetailid
	left join #oldPBDSalePurgeList os on ud.pricebatchdetailid = os.pricebatchdetailid
	left join #oldPBDItemChgPurgeList oi on ud.pricebatchdetailid = oi.pricebatchdetailid
	where
		ex.pricebatchdetailid is not null -- Already in expired list.
		or
		os.pricebatchdetailid is not null -- Already in old-sale list.
		or
		oi.pricebatchdetailid is not null -- Already in old-item-change list.

	select @unbatchedDelPBDPurgeCount = count(*) from #unbatchedDelPBDPurgeList
	print '[' + convert(nvarchar, getdate(), 121) + '] ' + 'Final unbatched-deleted rows to be purged (after removing already-targeted entries): ' + convert(varchar, @unbatchedDelPBDPurgeCount)
	
end
else
begin
	print '[' + convert(nvarchar, getdate(), 121) + '] ' + 'Unbatched, deleted-item PBD purge disabled.'
end


--------------------------------------------------------------------------------------------------------------------
--------------------------------------------------------------------------------------------------------------------
--------------------------------------------------------------------------------------------------------------------

-- Holds first and last PBD IDs.
declare @pbdStats varchar(64);

-- Consolidate PBD lists into saved list.
print '[' + convert(nvarchar, getdate(), 121) + '] ' + 'Consolidating PBD purge lists into master list...'
insert into #pbdArchiveFullListSaved
	select pricebatchdetailid from #processedBatchesPurgeList
	union
	select pricebatchdetailid from #expiredPBDPurgeList
	union
	select pricebatchdetailid from #oldPBDSalePurgeList
	union
	select pricebatchdetailid from #oldPBDItemChgPurgeList
	union
	select pricebatchdetailid from #unbatchedDelPBDPurgeList
	order by pricebatchdetailid


select @pbdTotalPurgeCount = @@rowcount
print '[' + convert(nvarchar, getdate(), 121) + '] ' + 'Total rows merged to master PBD purge list: ' + convert(varchar, @pbdTotalPurgeCount)

print '[' + convert(nvarchar, getdate(), 121) + '] ' + 'Copying master PBD ID list to working list...'
insert into #pbdArchiveFullListWorking
	select pricebatchdetailid from #pbdArchiveFullListSaved order by pricebatchdetailid

select @pbdTotalPurgeCount = @@rowcount
select @pbdStats = 'FirstID=' + convert(varchar, min(pricebatchdetailid)) + ', LastID=' + convert(varchar, max(pricebatchdetailid)) from #pbdArchiveFullListWorking
print '[' + convert(nvarchar, getdate(), 121) + '] ' + 'Total PBD rows to be purged: ' + convert(varchar, @pbdTotalPurgeCount) + '; ' + @pbdStats

print '[' + convert(nvarchar, getdate(), 121) + '] ' + 'Copying master PBH ID list to working list...'
insert into #pbhArchiveFullListWorking
	select pricebatchheaderid from #pbhArchiveFullListSaved order by pricebatchheaderid

select @pbhTotalPurgeCount = @@rowcount
print '[' + convert(nvarchar, getdate(), 121) + '] ' + 'Total PBH rows that *may* be purged: ' + convert(varchar, @pbhTotalPurgeCount)

-- Now both working-list tables have list of rows to be archived.

-- v2015.5: (Next 2 Queries) Added summaries to show groups of targeted PBDs by date, chg types, deleted status, and insert date.
print '[' + convert(nvarchar, getdate(), 121) + '] ' + 'Summary of all batched PBD entries to be purged: '
-- Summaries of batched entries to be purged.
select
	batched = 1, 
	'Year Processed' = year(pbh.processeddate), 
	'Item Chg Type' = ict.itemchgtypedesc, 
	'Price Chg Type' = pct.pricechgtypedesc, 
	'Is Deleted' = i.deleted_item, 
	'Insert Application' = pbd.insertapplication, 
	'Row Count' = count(*)
from pricebatchheader pbh (nolock) 
join pricebatchdetail pbd (nolock) on pbh.pricebatchheaderid = pbd.pricebatchheaderid 
join #pbdArchiveFullListSaved targ on pbd.pricebatchdetailid = targ.pricebatchdetailid -- Filter to only detail rows targeted by purge settings above.
left join item i (nolock) on i.item_key = pbd.item_key
left join pricechgtype pct (nolock) on pbd.pricechgtypeid = pct.pricechgtypeid
left join itemchgtype ict (nolock) on pbd.itemchgtypeid = ict.itemchgtypeid
group by year(pbh.processeddate), ict.itemchgtypedesc, pct.pricechgtypedesc, i.deleted_item, pbd.insertapplication
order by year(pbh.processeddate), ict.itemchgtypedesc, pct.pricechgtypedesc, i.deleted_item, pbd.insertapplication

print '[' + convert(nvarchar, getdate(), 121) + '] ' + 'Summary of all unbatched PBD entries to be purged: '
-- Summaries of unbatched entries to be purged.
select
	batched = 0, 
	'year inserted' = year(pbd.insert_date), 
	'Item Chg Type' = ict.itemchgtypedesc, 
	'Price Chg Type' = pct.pricechgtypedesc, 
	'Is Deleted' = i.deleted_item, 
	'Insert Application' = pbd.insertapplication, 
	'Row Count' = count(*)
from pricebatchdetail pbd (nolock)
join #pbdArchiveFullListSaved targ on pbd.pricebatchdetailid = targ.pricebatchdetailid -- Filter to only detail rows targeted by purge settings above.
left join item i (nolock) on i.item_key = pbd.item_key
left join pricechgtype pct (nolock) on pbd.pricechgtypeid = pct.pricechgtypeid
left join itemchgtype ict (nolock) on pbd.itemchgtypeid = ict.itemchgtypeid
where pricebatchheaderid is null
group by year(pbd.insert_date), ict.itemchgtypedesc, pct.pricechgtypedesc, i.deleted_item, pbd.insertapplication
order by year(pbd.insert_date), ict.itemchgtypedesc, pct.pricechgtypedesc, i.deleted_item, pbd.insertapplication


-- We only run the main loop if we are going to be purging data.
if @getCountsOnly = 0
begin

	print '[' + convert(nvarchar, getdate(), 121) + '] ' + 'Disabling PBD indexes...'
	-- Disable PBD non-clustered indexes.

	-- Common indexes (all regions, don't need if-exists check).
	print '[' + convert(nvarchar, getdate(), 121) + '] ' + 'Disabling index: idxPriceBatchHeader...'
	alter index idxPriceBatchHeader
	on PriceBatchDetail
	disable;

	print '[' + convert(nvarchar, getdate(), 121) + '] ' + 'Disabling index: _dta_index_PriceBatchDetail_StoreNo...'
	alter index _dta_index_PriceBatchDetail_StoreNo
	on PriceBatchDetail
	disable;

	print '[' + convert(nvarchar, getdate(), 121) + '] ' + 'Disabling index: _dta_index_PriceBatchDetail_PBHID...'
	alter index _dta_index_PriceBatchDetail_PBHID
	on PriceBatchDetail
	disable;

	print '[' + convert(nvarchar, getdate(), 121) + '] ' + 'Disabling index: _dta_index_PriceBatchDetail_Expired...'
	alter index _dta_index_PriceBatchDetail_Expired
	on PriceBatchDetail
	disable;

	-- The following indexes may not exist in every region, so we add any active/enabled index to the active list before disabling so it can be rebuilt at the end.

	-- Mostly common (don't exist in South region).
	select @indexName = 'IX_PriceBatchDetail_Store'
	if exists (SELECT * FROM sys.indexes WHERE is_disabled = 0 and name=@indexName AND object_id = OBJECT_ID('PriceBatchDetail'))
	begin
		insert into @activeIndexList values (@indexName)
		print '[' + convert(nvarchar, getdate(), 121) + '] ' + 'Disabling index: ' + @indexName + '...'
		alter index IX_PriceBatchDetail_Store
		on PriceBatchDetail
		disable;
	end

	select @indexName = 'IX_PriceBatchDetail_StartDate'
	if exists (SELECT * FROM sys.indexes WHERE is_disabled = 0 and name='IX_PriceBatchDetail_StartDate' AND object_id = OBJECT_ID('PriceBatchDetail'))
	begin
		insert into @activeIndexList values (@indexName)
		print '[' + convert(nvarchar, getdate(), 121) + '] ' + 'Disabling index: ' + @indexName + '...'
		alter index IX_PriceBatchDetail_StartDate
		on PriceBatchDetail
		disable;
	end

	select @indexName = 'IX_PriceBatchDetail_ItemKey'
	if exists (SELECT * FROM sys.indexes WHERE is_disabled = 0 and name='IX_PriceBatchDetail_ItemKey' AND object_id = OBJECT_ID('PriceBatchDetail'))
	begin
		insert into @activeIndexList values (@indexName)
		print '[' + convert(nvarchar, getdate(), 121) + '] ' + 'Disabling index: ' + @indexName + '...'
		alter index IX_PriceBatchDetail_ItemKey
		on PriceBatchDetail
		disable;
	end

	select @indexName = 'IX_PriceBatchDetail_ItemChgType'
	if exists (SELECT * FROM sys.indexes WHERE is_disabled = 0 and name='IX_PriceBatchDetail_ItemChgType' AND object_id = OBJECT_ID('PriceBatchDetail'))
	begin
		insert into @activeIndexList values (@indexName)
		print '[' + convert(nvarchar, getdate(), 121) + '] ' + 'Disabling index: ' + @indexName + '...'
		alter index IX_PriceBatchDetail_ItemChgType
		on PriceBatchDetail
		disable;
	end

	select @indexName = 'idxPriceBatchDetail_ItemKeyStoreNo'
	if exists (SELECT * FROM sys.indexes WHERE is_disabled = 0 and name='idxPriceBatchDetail_ItemKeyStoreNo' AND object_id = OBJECT_ID('PriceBatchDetail'))
	begin
		insert into @activeIndexList values (@indexName)
		print '[' + convert(nvarchar, getdate(), 121) + '] ' + 'Disabling index: ' + @indexName + '...'
		alter index idxPriceBatchDetail_ItemKeyStoreNo
		on PriceBatchDetail
		disable;
	end


	-- Mostly-South-region-only indexes (one exists in MW?).
	select @indexName = '_dta_index_PriceBatchDetail_7_2037582297__K3_K5_K2_K1_K7_K4_K75_6_15_24_56'
	if exists (SELECT * FROM sys.indexes WHERE is_disabled = 0 and name='_dta_index_PriceBatchDetail_7_2037582297__K3_K5_K2_K1_K7_K4_K75_6_15_24_56' AND object_id = OBJECT_ID('PriceBatchDetail'))
	begin
		insert into @activeIndexList values (@indexName)
		print '[' + convert(nvarchar, getdate(), 121) + '] ' + 'Disabling index: ' + @indexName + '...'
		alter index _dta_index_PriceBatchDetail_7_2037582297__K3_K5_K2_K1_K7_K4_K75_6_15_24_56
		on PriceBatchDetail
		disable;
	end
	
	select @indexName = '_dta_index_PriceBatchDetail_7_2037582297__K4_K56_K3_K75_K2_1_5_6_7_15_24'
	if exists (SELECT * FROM sys.indexes WHERE is_disabled = 0 and name='_dta_index_PriceBatchDetail_7_2037582297__K4_K56_K3_K75_K2_1_5_6_7_15_24' AND object_id = OBJECT_ID('PriceBatchDetail'))
	begin
		insert into @activeIndexList values (@indexName)
		print '[' + convert(nvarchar, getdate(), 121) + '] ' + 'Disabling index: ' + @indexName + '...'
		alter index _dta_index_PriceBatchDetail_7_2037582297__K4_K56_K3_K75_K2_1_5_6_7_15_24
		on PriceBatchDetail
		disable;
	end
	
	select @indexName = '_dta_index_PriceBatchDetail_7_2037582297__K75_K2_K3_K6_K1_K7_K4_K5_K15_24_56'
	if exists (SELECT * FROM sys.indexes WHERE is_disabled = 0 and name='_dta_index_PriceBatchDetail_7_2037582297__K75_K2_K3_K6_K1_K7_K4_K5_K15_24_56' AND object_id = OBJECT_ID('PriceBatchDetail'))
	begin
		insert into @activeIndexList values (@indexName)
		print '[' + convert(nvarchar, getdate(), 121) + '] ' + 'Disabling index: ' + @indexName + '...'
		alter index _dta_index_PriceBatchDetail_7_2037582297__K75_K2_K3_K6_K1_K7_K4_K5_K15_24_56
		on PriceBatchDetail
		disable;
	end
	
	select @indexName = 'idxPriceBatchDetail_StoreItem'
	if exists (SELECT * FROM sys.indexes WHERE is_disabled = 0 and name='idxPriceBatchDetail_StoreItem' AND object_id = OBJECT_ID('PriceBatchDetail'))
	begin
		insert into @activeIndexList values (@indexName)
		print '[' + convert(nvarchar, getdate(), 121) + '] ' + 'Disabling index: ' + @indexName + '...'
		alter index idxPriceBatchDetail_StoreItem
		on PriceBatchDetail
		disable;
	end
	
	select @indexName = 'IX_PriceBatchDetail_POSPushSearch'
	if exists (SELECT * FROM sys.indexes WHERE is_disabled = 0 and name='IX_PriceBatchDetail_POSPushSearch' AND object_id = OBJECT_ID('PriceBatchDetail'))
	begin
		insert into @activeIndexList values (@indexName)
		print '[' + convert(nvarchar, getdate(), 121) + '] ' + 'Disabling index: ' + @indexName + '...'
		alter index IX_PriceBatchDetail_POSPushSearch
		on PriceBatchDetail
		disable;
	end


	print '[' + convert(nvarchar, getdate(), 121) + '] ' + 'All non-clustered PBD indexes have been disabled.'

	/*
		This is a flag that helps us handle special cases in the loop below.
		When we need to terminate the loop, we set this flag, which is then checked *after* the transaction handling at the bottom of the loop.
	*/
	declare @breakLoopNeeded bit; select @breakLoopNeeded = 0
	/*
		Loop through working list, archiving data until the working list is empty.
	*/
	print '[' + convert(nvarchar, getdate(), 121) + '] ' + 'Beginning purge; committing every ' + convert(varchar, @commitFrequency) + ' rows...'
	while exists
	(
		select top 1 *
		from #pbdArchiveFullListWorking
	)
	begin

	begin tran
	begin try
		truncate table #pbdArchiveCurrentSet

		-- * AVOID ENDLESS LOOPS *
		-- If our index is greater than the total PBD purge count before we increment, something bad has happened and we'd better stop.
		if @rowIndex > @pbdTotalPurgeCount
		begin
			print '[' + convert(nvarchar, getdate(), 121) + '] ' + '*ERROR* - Unexpected state: row index ''' + convert(varchar, @rowIndex)
				+ ''' is greater than total PBD purge size ''' + convert(varchar, @pbdTotalPurgeCount) + ''' and there are still rows left in the PBD full working list.'
			select @breakLoopNeeded = 1
		end
		select @rowIndex = @rowIndex + @commitFrequency

		-- If we only want the count for this exercise, we'll skip the populate of the current-set table, which will kick us out of this loop.
		print '[' + convert(nvarchar, getdate(), 121) + '] ' + 'Getting current set of PBD IDs to be purged (index ' + convert(varchar, @rowIndex) + ' of ' + convert(varchar, @pbdTotalPurgeCount) + ')...'
		insert into #pbdArchiveCurrentSet
			select pricebatchdetailid from #pbdArchiveFullListWorking where rowid <= @rowIndex order by pricebatchdetailid

		-- * AVOID ENDLESS LOOPS *
		-- If we have no working set, we should exit.
		if not exists (
			select top 1 * from #pbdArchiveCurrentSet
		)
		begin
			print '[' + convert(nvarchar, getdate(), 121) + '] ' + '*Nothing To Do* - No rows in PBD current set table.  Exiting loop...'
			select @breakLoopNeeded = 1
		end


		select @pbdStats = 'FirstID=' + convert(varchar, min(pricebatchdetailid)) + ', LastID=' + convert(varchar, max(pricebatchdetailid)) from #pbdArchiveCurrentSet
		print '[' + convert(nvarchar, getdate(), 121) + '] ' + 'Purging PBD and archiving each row to PriceBatchDetailArchive table...'
		delete pricebatchdetail
		output deleted.*
			into pricebatchdetailarchive
		from pricebatchdetail d
			join #pbdArchiveCurrentSet crnt
				on d.pricebatchdetailid = crnt.pricebatchdetailid
		print '[' + convert(nvarchar, getdate(), 121) + '] ' + 'PBD purge complete; rows affected: ' + convert(varchar, @@rowcount) + '; ' + @pbdStats + '.'

		print '[' + convert(nvarchar, getdate(), 121) + '] ' + 'Updating PBD working list (removing deleted IDs)...'
		delete #pbdArchiveFullListWorking
		from #pbdArchiveFullListWorking wrk
			join #pbdArchiveCurrentSet crnt
				on wrk.pricebatchdetailid = crnt.pricebatchdetailid

		select @wrkListAffected = @@rowcount
		select @wrkListLeft = count(*) from #pbdArchiveFullListWorking
		print '[' + convert(nvarchar, getdate(), 121) + '] ' + 'Working list PBD updated; rows affected: ' + convert(varchar, @wrkListAffected) + '; rows left: ' + convert(varchar, @wrkListLeft) + '.'


		if @@TRANCOUNT > 0
		begin
			print '-------------------------------------------------';
			print '[' + convert(nvarchar, getdate(), 121) + '] ' + 'Committing ' + cast(@@TRANCOUNT as varchar) + ' transaction(s)...';
			COMMIT TRANSACTION
			print '[' + convert(nvarchar, getdate(), 121) + '] ' + 'Updates committed successfully.';
			print '-------------------------------------------------';
		end
		else
		begin
			PRINT '[' + convert(nvarchar, getdate(), 121) + '] ' + '**Warning** -- No updates to commit.';
		end
	end try
	begin catch
		IF @@TRANCOUNT > 0
		begin
			print '-------------------------------------------------';
			print '[' + convert(nvarchar, getdate(), 121) + '] ' + 'Performing transaction rollback...';
			ROLLBACK TRANSACTION
			print '[' + convert(nvarchar, getdate(), 121) + '] ' + 'Rollback complete.';
			print '-------------------------------------------------';
		end
		else
		begin
			print '[' + convert(nvarchar, getdate(), 121) + '] ' + '**Nothing to rollback.';
		end

		SELECT
			[Info] = 'An error occurred.  Database changes were rolled back!',
			[ErrorNumber] = ERROR_NUMBER(),
			[ErrorSeverity] = ERROR_SEVERITY(),
			[ErrorState] = ERROR_STATE(),
			[ErrorProcedure] = ERROR_PROCEDURE(),
			[ErrorLine] = ERROR_LINE(),
			[ErrorMessage] = ERROR_MESSAGE()

	end catch


		-- Check for special app-config flag that tells this process to stop at the end of the next batch, which would be here at the bottom of the main loop.
		if exists (
			-- It doesn't matter what IRMA application the key is associated with, it just needs a value of '1' in any application.
			select * from appconfigkey k join appconfigvalue v on k.keyid = v.keyid
			where lower(name) = lower('stoppricebatchpurge')
				and v.value = '1' and v.deleted = 0
		)
		begin
			select @breakLoopNeeded = 1
			print '[' + convert(nvarchar, getdate(), 121) + '] ' + '*ALERT* - Admin user requested this purge process be stopped via ''StopPriceBatchPurge'' app-setting.'
		end

		if @breakLoopNeeded = 1
		begin
			break
		end

	end -- End loop.
	---------------------------------
	-- END OF MAIN LOOP
	---------------------------------

	-- Index work ahead...

	---------------------------------
	-- Rebuild PBD Indexes
	---------------------------------
	-- Rebuild PBD clustered index.
	select @indexName = 'PK_PriceBatchDetail_PriceBatchDetailID'
	print '[' + convert(nvarchar, getdate(), 121) + '] ' + 'Rebuilding index: ' + @indexName + '...'
	alter index PK_PriceBatchDetail_PriceBatchDetailID on PriceBatchDetail rebuild;
	print '[' + convert(nvarchar, getdate(), 121) + '] ' + 'Index rebuild complete: ' + @indexName + '.'

	-- Rebuild/enable PBD non-clustered indexes.

	-- Common indexes (all regions, don't need if-exists check).
	select @indexName = 'idxPriceBatchHeader'
	print '[' + convert(nvarchar, getdate(), 121) + '] ' + 'Rebuilding index: ' + @indexName + '...'
	alter index idxPriceBatchHeader on PriceBatchDetail rebuild;
	print '[' + convert(nvarchar, getdate(), 121) + '] ' + 'Index rebuild complete: ' + @indexName + '.'

	select @indexName = '_dta_index_PriceBatchDetail_StoreNo'
	print '[' + convert(nvarchar, getdate(), 121) + '] ' + 'Rebuilding index: ' + @indexName + '...'
	alter index _dta_index_PriceBatchDetail_StoreNo on PriceBatchDetail rebuild;
	print '[' + convert(nvarchar, getdate(), 121) + '] ' + 'Index rebuild complete: ' + @indexName + '.'

	select @indexName = '_dta_index_PriceBatchDetail_PBHID'
	print '[' + convert(nvarchar, getdate(), 121) + '] ' + 'Rebuilding index: ' + @indexName + '...'
	alter index _dta_index_PriceBatchDetail_PBHID on PriceBatchDetail rebuild;
	print '[' + convert(nvarchar, getdate(), 121) + '] ' + 'Index rebuild complete: ' + @indexName + '.'

	select @indexName = '_dta_index_PriceBatchDetail_Expired'
	print '[' + convert(nvarchar, getdate(), 121) + '] ' + 'Rebuilding index: ' + @indexName + '...'
	alter index _dta_index_PriceBatchDetail_Expired on PriceBatchDetail rebuild;
	print '[' + convert(nvarchar, getdate(), 121) + '] ' + 'Index rebuild complete: ' + @indexName + '.'


	-- The following indexes may not exist in every region, so we make sure they are in the active list before rebuilding.

	-- Mostly common (don't exist in South region).
	select @indexName = 'IX_PriceBatchDetail_Store'
	if exists (SELECT * FROM sys.indexes WHERE name=@indexName AND object_id = OBJECT_ID('PriceBatchDetail'))
	and exists (select * from @activeIndexList where name like @indexName)
	begin
		print '[' + convert(nvarchar, getdate(), 121) + '] ' + 'Rebuilding index: ' + @indexName + '...'
		alter index IX_PriceBatchDetail_Store on PriceBatchDetail rebuild;
		print '[' + convert(nvarchar, getdate(), 121) + '] ' + 'Index rebuild complete: ' + @indexName + '.'
	end

	select @indexName = 'IX_PriceBatchDetail_StartDate'
	if exists (SELECT * FROM sys.indexes WHERE name=@indexName AND object_id = OBJECT_ID('PriceBatchDetail'))
	and exists (select * from @activeIndexList where name like @indexName)
	begin
		print '[' + convert(nvarchar, getdate(), 121) + '] ' + 'Rebuilding index: ' + @indexName + '...'
		alter index IX_PriceBatchDetail_StartDate on PriceBatchDetail rebuild;
		print '[' + convert(nvarchar, getdate(), 121) + '] ' + 'Index rebuild complete: ' + @indexName + '.'
	end

	select @indexName = 'IX_PriceBatchDetail_ItemKey'
	if exists (SELECT * FROM sys.indexes WHERE name=@indexName AND object_id = OBJECT_ID('PriceBatchDetail'))
	and exists (select * from @activeIndexList where name like @indexName)
	begin
		print '[' + convert(nvarchar, getdate(), 121) + '] ' + 'Rebuilding index: ' + @indexName + '...'
		alter index IX_PriceBatchDetail_ItemKey on PriceBatchDetail rebuild;
		print '[' + convert(nvarchar, getdate(), 121) + '] ' + 'Index rebuild complete: ' + @indexName + '.'
	end

	select @indexName = 'IX_PriceBatchDetail_ItemChgType'
	if exists (SELECT * FROM sys.indexes WHERE name=@indexName AND object_id = OBJECT_ID('PriceBatchDetail'))
	and exists (select * from @activeIndexList where name like @indexName)
	begin
		print '[' + convert(nvarchar, getdate(), 121) + '] ' + 'Rebuilding index: ' + @indexName + '...'
		alter index IX_PriceBatchDetail_ItemChgType on PriceBatchDetail rebuild;
		print '[' + convert(nvarchar, getdate(), 121) + '] ' + 'Index rebuild complete: ' + @indexName + '.'
	end

	select @indexName = 'idxPriceBatchDetail_ItemKeyStoreNo'
	if exists (SELECT * FROM sys.indexes WHERE name=@indexName AND object_id = OBJECT_ID('PriceBatchDetail'))
	and exists (select * from @activeIndexList where name like @indexName)
	begin
		print '[' + convert(nvarchar, getdate(), 121) + '] ' + 'Rebuilding index: ' + @indexName + '...'
		alter index idxPriceBatchDetail_ItemKeyStoreNo on PriceBatchDetail rebuild;
		print '[' + convert(nvarchar, getdate(), 121) + '] ' + 'Index rebuild complete: ' + @indexName + '.'
	end

	-- Mostly-South-region-only indexes (one exists in MW?).
	select @indexName = '_dta_index_PriceBatchDetail_7_2037582297__K3_K5_K2_K1_K7_K4_K75_6_15_24_56'
	if exists (SELECT * FROM sys.indexes WHERE name=@indexName AND object_id = OBJECT_ID('PriceBatchDetail'))
	and exists (select * from @activeIndexList where name like @indexName)
	begin
		print '[' + convert(nvarchar, getdate(), 121) + '] ' + 'Rebuilding index: ' + @indexName + '...'
		alter index _dta_index_PriceBatchDetail_7_2037582297__K3_K5_K2_K1_K7_K4_K75_6_15_24_56 on PriceBatchDetail rebuild;
		print '[' + convert(nvarchar, getdate(), 121) + '] ' + 'Index rebuild complete: ' + @indexName + '.'
	end
	
	select @indexName = '_dta_index_PriceBatchDetail_7_2037582297__K4_K56_K3_K75_K2_1_5_6_7_15_24'
	if exists (SELECT * FROM sys.indexes WHERE name=@indexName AND object_id = OBJECT_ID('PriceBatchDetail'))
	and exists (select * from @activeIndexList where name like @indexName)
	begin
		print '[' + convert(nvarchar, getdate(), 121) + '] ' + 'Rebuilding index: ' + @indexName + '...'
		alter index _dta_index_PriceBatchDetail_7_2037582297__K4_K56_K3_K75_K2_1_5_6_7_15_24 on PriceBatchDetail rebuild;
		print '[' + convert(nvarchar, getdate(), 121) + '] ' + 'Index rebuild complete: ' + @indexName + '.'
	end
	
	select @indexName = '_dta_index_PriceBatchDetail_7_2037582297__K75_K2_K3_K6_K1_K7_K4_K5_K15_24_56'
	if exists (SELECT * FROM sys.indexes WHERE name=@indexName AND object_id = OBJECT_ID('PriceBatchDetail'))
	and exists (select * from @activeIndexList where name like @indexName)
	begin
		print '[' + convert(nvarchar, getdate(), 121) + '] ' + 'Rebuilding index: ' + @indexName + '...'
		alter index _dta_index_PriceBatchDetail_7_2037582297__K75_K2_K3_K6_K1_K7_K4_K5_K15_24_56 on PriceBatchDetail rebuild;
		print '[' + convert(nvarchar, getdate(), 121) + '] ' + 'Index rebuild complete: ' + @indexName + '.'
	end
	
	select @indexName = 'idxPriceBatchDetail_StoreItem'
	if exists (SELECT * FROM sys.indexes WHERE name=@indexName AND object_id = OBJECT_ID('PriceBatchDetail'))
	and exists (select * from @activeIndexList where name like @indexName)
	begin
		print '[' + convert(nvarchar, getdate(), 121) + '] ' + 'Rebuilding index: ' + @indexName + '...'
		alter index idxPriceBatchDetail_StoreItem on PriceBatchDetail rebuild;
		print '[' + convert(nvarchar, getdate(), 121) + '] ' + 'Index rebuild complete: ' + @indexName + '.'
	end
	
	select @indexName = 'IX_PriceBatchDetail_POSPushSearch'
	if exists (SELECT * FROM sys.indexes WHERE name=@indexName AND object_id = OBJECT_ID('PriceBatchDetail'))
	and exists (select * from @activeIndexList where name like @indexName)
	begin
		print '[' + convert(nvarchar, getdate(), 121) + '] ' + 'Rebuilding index: ' + @indexName + '...'
		alter index IX_PriceBatchDetail_POSPushSearch on PriceBatchDetail rebuild;
		print '[' + convert(nvarchar, getdate(), 121) + '] ' + 'Index rebuild complete: ' + @indexName + '.'
	end

	---------------------------------
	-- PBH Purge
	---------------------------------

	print '[' + convert(nvarchar, getdate(), 121) + '] ' + 'Disabling PBH indexes...'

	-- Disable PBH non-clustered indexes.

	-- Mostly common (don't exist in South region).
	select @indexName = 'IX_PriceBatchHeader_StartDate'
	if exists (SELECT * FROM sys.indexes WHERE is_disabled = 0 and name=@indexName AND object_id = OBJECT_ID('PriceBatchHeader'))
	begin
		insert into @activeIndexList values (@indexName)
		print '[' + convert(nvarchar, getdate(), 121) + '] ' + 'Disabling index: ' + @indexName + '...'
		alter index IX_PriceBatchHeader_StartDate
		on PriceBatchHeader
		disable;
	end

	-- South-only indexes.
	select @indexName = 'idxPriceBatchHeaderIDStatus'
	if exists (SELECT * FROM sys.indexes WHERE is_disabled = 0 and name=@indexName AND object_id = OBJECT_ID('PriceBatchHeader'))
	begin
		insert into @activeIndexList values (@indexName)
		print '[' + convert(nvarchar, getdate(), 121) + '] ' + 'Disabling index: ' + @indexName + '...'
		alter index idxPriceBatchHeaderIDStatus
		on PriceBatchHeader
		disable;
	end
	
	select @indexName = 'IX_PriceBatchHeader_POSPushSearch2'
	if exists (SELECT * FROM sys.indexes WHERE is_disabled = 0 and name=@indexName AND object_id = OBJECT_ID('PriceBatchHeader'))
	begin
		insert into @activeIndexList values (@indexName)
		print '[' + convert(nvarchar, getdate(), 121) + '] ' + 'Disabling index: ' + @indexName + '...'
		alter index IX_PriceBatchHeader_POSPushSearch2
		on PriceBatchHeader
		disable;	
	end

	print '[' + convert(nvarchar, getdate(), 121) + '] ' + 'All non-clustered PBH indexes have been disabled.'

	begin tran
	begin try
		/*
			The PBD list to be purged drives the purge process, meaning we don't key off the PBH list.
			The true/accurate PBH list to be purged depends on what batches are completely empty after a pass of the PBD purge.
			The PBH list to be purged is, therefore, generated dymanically here.
			So, we get the PBH IDs before deleting the PBD rows because once they're gone, we can't get the PBH ID.
			*Another way to do this would be to build another master list of target purge data that is specific to the 'processed batches' purge.
			*It would contain the PBH and corresponding PBD IDs, so that we don't have to go back to PBD to pull the PBH IDs.
			*Shouldn't be too slow joining small lists to PBD to pull PBH IDs.

		*/	


		-- Update target list of PBH rows to be purged by excluding PBH rows that have one or more PBD rows.	
		print '[' + convert(nvarchar, getdate(), 121) + '] ' + 'Excluding PBH rows from purge list if they have PBD rows...'
		delete #pbhArchiveFullListWorking
		where pricebatchheaderid in (
			select d.pricebatchheaderid
			from #pbhArchiveFullListWorking crnt
				join pricebatchdetail d on crnt.pricebatchheaderid = d.pricebatchheaderid
			group by d.pricebatchheaderid
			having count(*) > 0 -- We only purge PBH entries that have no PBD rows left after PBD delete.
		)
		print '[' + convert(nvarchar, getdate(), 121) + '] ' + 'PBH rows excluded: ' + convert(varchar, @@rowcount) + '.'

		alter index all on #pbhArchiveFullListWorking rebuild
		print '[' + convert(nvarchar, getdate(), 121) + '] ' + 'Rebuilt PBH target ID list temp table indexes.'

		print '[' + convert(nvarchar, getdate(), 121) + '] ' + 'Purging PBH and archiving each row to PriceBatchHeaderArchive table...'
		delete pricebatchheader
		output deleted.*
			into pricebatchheaderarchive
		from pricebatchheader h
			join #pbhArchiveFullListWorking crnt
				on crnt.pricebatchheaderid = h.pricebatchheaderid
		print '[' + convert(nvarchar, getdate(), 121) + '] ' + 'PBH purge complete; rows affected: ' + convert(varchar, @@rowcount) + '.'


		if @@TRANCOUNT > 0
		begin
			print '-------------------------------------------------';
			print '[' + convert(nvarchar, getdate(), 121) + '] ' + 'Committing ' + cast(@@TRANCOUNT as varchar) + ' transaction(s)...';
			COMMIT TRANSACTION
			print '[' + convert(nvarchar, getdate(), 121) + '] ' + 'Updates committed successfully.';
			print '-------------------------------------------------';
		end
		else
		begin
			PRINT '[' + convert(nvarchar, getdate(), 121) + '] ' + '**Warning** -- No updates to commit.';
		end
	end try
	begin catch
		IF @@TRANCOUNT > 0
		begin
			print '-------------------------------------------------';
			print '[' + convert(nvarchar, getdate(), 121) + '] ' + 'Performing transaction rollback...';
			ROLLBACK TRANSACTION
			print '[' + convert(nvarchar, getdate(), 121) + '] ' + 'Rollback complete.';
			print '-------------------------------------------------';
		end
		else
		begin
			print '[' + convert(nvarchar, getdate(), 121) + '] ' + '**Nothing to rollback.';
		end

		SELECT
			[Info] = 'An error occurred.  Database changes were rolled back!',
			[ErrorNumber] = ERROR_NUMBER(),
			[ErrorSeverity] = ERROR_SEVERITY(),
			[ErrorState] = ERROR_STATE(),
			[ErrorProcedure] = ERROR_PROCEDURE(),
			[ErrorLine] = ERROR_LINE(),
			[ErrorMessage] = ERROR_MESSAGE()

	end catch

	---------------------------------
	-- Rebuild PBH Indexes
	---------------------------------

	-- Rebuild PBH clustered index.
	select @indexName = 'PK_PriceBatchHeader_PriceBatchHeaderID'
	print '[' + convert(nvarchar, getdate(), 121) + '] ' + 'Rebuilding index: ' + @indexName + '...'
	alter index PK_PriceBatchHeader_PriceBatchHeaderID on PriceBatchHeader rebuild;
	print '[' + convert(nvarchar, getdate(), 121) + '] ' + 'Index rebuild complete: ' + @indexName + '.'

	-- Rebuild/enable PBH non-clustered indexes.

	-- Mostly common (don't exist in South region).
	select @indexName = 'IX_PriceBatchHeader_StartDate'
	if exists (SELECT * FROM sys.indexes WHERE name=@indexName AND object_id = OBJECT_ID('PriceBatchHeader'))
	and exists (select * from @activeIndexList where name like @indexName)
	begin
		print '[' + convert(nvarchar, getdate(), 121) + '] ' + 'Rebuilding index: ' + @indexName + '...'
		alter index IX_PriceBatchHeader_StartDate on PriceBatchHeader rebuild;
		print '[' + convert(nvarchar, getdate(), 121) + '] ' + 'Index rebuild complete: ' + @indexName + '.'
	end

	-- South-only indexes.
	select @indexName = 'idxPriceBatchHeaderIDStatus'
	if exists (SELECT * FROM sys.indexes WHERE name=@indexName AND object_id = OBJECT_ID('PriceBatchHeader'))
	and exists (select * from @activeIndexList where name like @indexName)
	begin
		print '[' + convert(nvarchar, getdate(), 121) + '] ' + 'Rebuilding index: ' + @indexName + '...'
		alter index idxPriceBatchHeaderIDStatus on PriceBatchHeader rebuild;
		print '[' + convert(nvarchar, getdate(), 121) + '] ' + 'Index rebuild complete: ' + @indexName + '.'
	end
	
	select @indexName = 'IX_PriceBatchHeader_POSPushSearch2'
	if exists (SELECT * FROM sys.indexes WHERE name=@indexName AND object_id = OBJECT_ID('PriceBatchHeader'))
	and exists (select * from @activeIndexList where name like @indexName)
	begin
		print '[' + convert(nvarchar, getdate(), 121) + '] ' + 'Rebuilding index: ' + @indexName + '...'
		alter index IX_PriceBatchHeader_POSPushSearch2 on PriceBatchHeader rebuild;
		print '[' + convert(nvarchar, getdate(), 121) + '] ' + 'Index rebuild complete: ' + @indexName + '.'
	end

end -- End of IF condition: getCountsOnly = 0

-- Remove temp tables.
drop table #pbhArchiveFullListSaved
drop table #pbdArchiveFullListSaved
drop table #pbhArchiveFullListWorking
drop table #pbdArchiveFullListWorking
drop table #pbhArchiveCurrentSet
drop table #pbdArchiveCurrentSet
drop table #processedBatchesPurgeList
drop table #expiredPBDPurgeList
drop table #oldPBDSalePurgeList
drop table #oldPBDItemChgPurgeList

-- Get current space stats for target tables.
insert into @spaceused exec sp_spaceused N'pricebatchheader'
insert into @spaceused exec sp_spaceused N'pricebatchdetail'
if exists (select * from sys.objects where object_id = object_id(N'[dbo].pricebatchheaderarchive') and type in (N'U'))
	insert into @spaceused exec sp_spaceused N'pricebatchheaderarchive'
else
	print '[No data for PBH-archive table; table not found.]'
if exists (select * from sys.objects where object_id = object_id(N'[dbo].[pricebatchdetailarchive]') and type in (N'U'))
	insert into @spaceused exec sp_spaceused N'pricebatchdetailarchive'
else
	print '[No data for PBD-archive table; table not found.]'

print '[' + convert(nvarchar, getdate(), 121) + '] ' + 'Current PBH & PBD table space attributes:'
select * from @spaceused

print '===== ===== ===== ===== =====';
print 'Finish time: ' + convert(nvarchar, getdate(), 121);

