/*****************************************************************************************************
Maria Younes, Tom Lux
7/6/2011
General update of National Hierarchy and Item Tables in IRMA






~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
NOTES -- PLEASE READ

--> This script must be run by a user with BULK INSERT permissions.

--> Set output to TEXT and send results to a file.  Output files are 10-20 MB.

~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~







---------------------------------------------------------------------------------------------------------
Change History
Dev					Date				TFS					Desc
---------------------------------------------------------------------------------------------------------
Tom Lux				2011-7-7			none				Initial working version updated from Maria's initial version.
---------------------------------------------------------------------------------------------------------
************* Script Version v1 (July 2011) *************
---------------------------------------------------------------------------------------------------------
Tom Lux				2011-8-19			none				Subteams field values were added to the Family table via new Hierarchy input file.
---------------------------------------------------------------------------------------------------------
************* Script Version v2 (Aug 2011) *************
---------------------------------------------------------------------------------------------------------
Tom Lux				2012-1-25			none				Missed item-coding requirements: Items not in the item-coding list from CEN but currently
assigned a valid national class ID (class ID exists in NatItemClass table), does not change (keeps current class ID value).
---------------------------------------------------------------------------------------------------------
Tom Lux				2012-5-16			none				Finished handling "unmapped items with valid class ID" scenario.
Updated logging.
Added summary item count section to help reconcile item groups and totals.
Changed update of mapped items to only update items that do not already have the correct national class ID, 
so those already set correctly are not updated.
Added @applyItemUpdates debug switch, to allow reruns of script during development.  If set to 0, the item records are not updated, 
but nat-hier tables are still truncated and loaded.  If set to 1, items are updated.
Added custom-input-file-selection logic to handle special cases where regions have customized input files.  UK is only one currently.

[Execution times against idd-xx were 30-60 seconds.]
---------------------------------------------------------------------------------------------------------
************* Script Version v3 (May 2012) -- Filename: Load New National Item Hierarchy and Assign Item Classes in IRMA.v3.sql *************
---------------------------------------------------------------------------------------------------------
Tom Lux				2012-9-5			none				Updated error handling.
Granted read permissions for 'wfm\sqlserverprd' user to hierarchy-input-file location to resolve 'file not found' issue
when run by DBA in Prod (SQL Server uses the default SQL account if the xp_fileexist proc is called by user with SYSADMIN role).
Changed 'return' to "noexec" if input file not found.
Changed fatal raiserror() to "noexec" so sysadmin not required.  This occurs after triggers on the item table are disabled, so we attempt
to enable execution and then enable all item-table triggers as the last actions of this script.
---------------------------------------------------------------------------------------------------------
************* Script Version v4 (September 2012) -- Filename: Load New National Item Hierarchy and Assign Item Classes in IRMA.v4.sql *************
---------------------------------------------------------------------------------------------------------

---------------------------------------------------------------------------------------------------------
Tom Lux				2013-2-25			none				Changing filename to be generic and checking into TFS
Trey D				2013-2-25			none				TFS 9791 - 4.7.1 - removed all references to deleted_item and remove_item (and deleted_identifier) references.
************* Script Version v4.0 (Feb 2013) -- Filename: Load New National Item Hierarchy and Assign Item Classes in IRMA.sql *************
---------------------------------------------------------------------------------------------------------


*****************************************************************************************************/

DECLARE @runTime			AS datetime
DECLARE @runUser			AS varchar(128)
DECLARE @runHost			AS varchar(128)
DECLARE @runDB				AS varchar(128)

SELECT @runTime = getdate(), @runUser = suser_name(), @runHost = host_name(), @runDB = db_name()
PRINT '---------------------------------------------------------------------------------------'
PRINT 'Current System Time...: ' + CONVERT(varchar, @runTime, 121)
PRINT 'User Name.............: ' + @runUser
PRINT 'Running From Host.....: ' + @runHost
PRINT 'Connected To DB Server: ' + @@servername
PRINT 'DB Name...............: ' + @runDB
PRINT '---------------------------------------------------------------------------------------'

SET NOCOUNT OFF
    


---------------------------------- Debug Option ------------------------------------------------
-- See @applyItemUpdates param at the beginning of the transaction (can't declare the var here because there's a batch separator, "GO", below and before the trx).



---------------------------------- File Validations ------------------------------------------------

PRINT '[' + CONVERT(nvarchar, GETDATE(), 121) + '] ' + 'Checking existence of input files...'

-- Verify hierarchy-definition input-file exists.
DECLARE @file_exists int

-- These are the default input-file names.  Any custom/special files for specific regions are set below.
declare @hierarchyFile nvarchar(128) = '\\cewd6503\buildshare\National Hierarchy\Hierarchy.txt'
declare @itemcodingFile nvarchar(128) = '\\cewd6503\buildshare\National Hierarchy\ItemCoding.txt'

if @@servername like '%uk\uk%'
begin
	PRINT '[' + CONVERT(nvarchar, GETDATE(), 121) + '] ' + '**NOTE: Using custom item-coding file for UK region.'
	select @itemcodingFile = '\\cewd6503\buildshare\National Hierarchy\ItemCoding.UK.txt'
end

EXEC master.dbo.xp_fileexist
	@hierarchyFile,
	@file_exists OUTPUT

IF @file_exists = 1
BEGIN
	PRINT '[' + CONVERT(nvarchar, GETDATE(), 121) + '] ' + 'Hierarchy input-file found.'
END
ELSE
BEGIN
	PRINT '[' + CONVERT(nvarchar, GETDATE(), 121) + '] ' + '[[Error -- Cannot Continue]] Hierarchy input-file not found.  Please verify file and try again.'
	set noexec on
END

-- Verify item-coding input-file exists.
SELECT @file_exists = 0
EXEC master.dbo.xp_fileexist
	@itemcodingFile,
	@file_exists OUTPUT

IF @file_exists = 1
BEGIN
	PRINT '[' + CONVERT(nvarchar, GETDATE(), 121) + '] ' + 'Item-coding input-file found.'
END
ELSE
BEGIN
	PRINT '[' + CONVERT(nvarchar, GETDATE(), 121) + '] ' + '[[Error -- Cannot Continue]] Item-coding file not found.  Please verify file and try again.'
	set noexec on
END


-----------------------------------End File Validations-------------------------------------------

PRINT '[' + CONVERT(nvarchar, GETDATE(), 121) + '] ' + 'Disabling item-table triggers...'
go
disable trigger all on item
go



-- Main Routine   
BEGIN TRY

	BEGIN TRANSACTION		

	---------------------------------- Debug Option ------------------------------------------------
	-- If you do not want to update the item records, set this param to 0 (zero).
	-- **** NOTE: National Hierarchy tables are still truncated and loaded.
	declare @applyItemUpdates bit = 1

	-- Create temporary Hierarchy table.  This table will hold the hierarchy plus PeopleSoft-to-IRMA-Subteam mappings for all regions.  The column names are the regional abbreviations.
	PRINT '[' + CONVERT(nvarchar, GETDATE(), 121) + '] ' + 'Creating tmpMainHierarchy table...'
	if object_id('tempdb..#tmpMainHierarchyAll') is not null
	begin
		drop table #tmpMainHierarchyAll
	end
	CREATE TABLE #tmpMainHierarchyAll
		(
			[Nat_Family_Num]			[int]			NOT NULL,
			[Nat_Family_Name]			[varchar](65)	NULL,
			[Nat_Category_Num]			[int]			NOT NULL,
			[Nat_Category_Name]			[varchar](65)	NULL,
			[Nat_Class_Num]				[int]			NOT NULL,
			[Nat_Class_Name]			[varchar](65)	NULL,
			[Subteam]				[int]			NOT NULL,
			[EU]			[int]			NOT NULL,
			[FL]			[int]			NOT NULL,
			[MA]				[int]			NOT NULL,
			[MW]				[int]			NOT NULL,
			[NA]				[int]			NOT NULL,
			[NC]				[int]			NOT NULL,
			[NE]				[int]			NOT NULL,
			[PN]				[int]			NOT NULL,
			[RM]				[int]			NOT NULL,
			[SO]				[int]			NOT NULL,
			[SP]				[int]			NOT NULL,
			[SW]			[int]			NOT NULL
			PRIMARY KEY (
				[Nat_Family_Num],
				[Nat_Category_Num],
				[Nat_Class_Num]
			)
		) ON [PRIMARY]
		
	-- Import Hierarchy data.
	PRINT '[' + CONVERT(nvarchar, GETDATE(), 121) + '] ' + 'Importing hierarchy text file...'
	BULK INSERT #tmpMainHierarchyAll FROM '\\cewd6503\buildshare\National Hierarchy\Hierarchy.txt'
	WITH (FIRSTROW = 2,     FIELDTERMINATOR = '\t') 

	-- This is our "working" hierarchy table.  We'll pull the POS Dept (IRMA Subteam) mapping in for the region where we are running.
	if object_id('tempdb..#tmpMainHierarchy') is not null
	begin
		drop table #tmpMainHierarchy
	end
	CREATE TABLE #tmpMainHierarchy
		(
			[Nat_Family_Num]			[int]			NOT NULL,
			[Nat_Family_Name]			[varchar](65)	NULL,
			[Nat_Category_Num]			[int]			NOT NULL,
			[Nat_Category_Name]			[varchar](65)	NULL,
			[Nat_Class_Num]				[int]			NOT NULL,
			[Nat_Class_Name]			[varchar](65)	NULL,
			[Subteam]				[int]			NOT NULL,
			[POSDept]			[int]			NOT NULL
			PRIMARY KEY (
				[Nat_Family_Num],
				[Nat_Category_Num],
				[Nat_Class_Num]
			)
		) ON [PRIMARY]

	-- Get region.
	declare @region varchar(2)
	select @region = RegionCode from Region
	
	-- Copy hierarchy for this region from #tmpMainHierarchyAll into #tmpMainHierarchy.
	declare @sql nvarchar(4000)
	select @sql = 
		'insert into #tmpMainHierarchy
			select 
				[Nat_Family_Num]
				,[Nat_Family_Name]
				,[Nat_Category_Num]
				,[Nat_Category_Name]
				,[Nat_Class_Num]
				,[Nat_Class_Name]
				,[Subteam]
				,' + @region + '
			from #tmpMainHierarchyAll'
	
	exec sp_executesql @sql
	
	-- If we didnt copy any data from the hierarchy-all table, we raise a fatal error to stop execution.
	if not exists (select top 1 * from #tmpMainHierarchy)
	begin
		PRINT '[' + CONVERT(nvarchar, GETDATE(), 121) + '] ' + '[[Error -- Cannot Continue]] Region [' + @region + '] not found in all-hierarchy list.  Check source hierarchy file [\\cewd6503\buildshare\National Hierarchy\Hierarchy.txt].'
		set noexec on
	end
	
	-- Display Master Hierarchy tables before processing
	PRINT '[' + CONVERT(nvarchar, GETDATE(), 121) + '] ' + 'Displaying all NatItemCat table before TRUNCATE process...'
	SELECT * FROM NatItemCat (NOLOCK)
	PRINT '[' + CONVERT(nvarchar, GETDATE(), 121) + '] ' + 'Displaying all NatItemClass table before TRUNCATE process...'
	SELECT * FROM NatItemClass (NOLOCK)
	PRINT '[' + CONVERT(nvarchar, GETDATE(), 121) + '] ' + 'Displaying all NatItemFamily table before TRUNCATE process...'
	SELECT * FROM NatItemFamily (NOLOCK)

	-- Clean up Master Hierarchy tables
	PRINT '[' + CONVERT(nvarchar, GETDATE(), 121) + '] ' + 'Truncating NatItemCat, NatItemClass and NatItemFamily tables...'
	TRUNCATE TABLE NatItemCat
	TRUNCATE TABLE NatItemClass 
	TRUNCATE TABLE NatItemFamily

	-- Insert latest NatItemFamily info 
	PRINT '[' + CONVERT(nvarchar, GETDATE(), 121) + '] ' + 'Inserting into NatItemFamily table...'
	INSERT INTO NatItemFamily (NatFamilyID, NatFamilyName, NatSubTeam_No, SubTeam_No, LastUpdateTimeStamp)
			SELECT DISTINCT 
				Nat_Family_Num,
				Nat_Family_Name,
				Subteam,
				POSDept,
				null
			FROM #tmpMainHierarchy

	-- Insert latest NatItemClass info 
	PRINT '[' + CONVERT(nvarchar, GETDATE(), 121) + '] ' + 'Inserting into NatItemClass table...'
	INSERT INTO NatItemClass (ClassID, ClassName, NatCatID, LastUpdateTimeStamp)
	SELECT DISTINCT 
		Nat_Class_Num,
		Nat_Class_Name,
		Nat_Category_Num,
		null
	FROM #tmpMainHierarchy

	-- Insert latest NatItemCat info 
	PRINT '[' + CONVERT(nvarchar, GETDATE(), 121) + '] ' + 'Inserting into NatItemCat table...'
	INSERT INTO NatItemCat (NatCatID, NatCatName, NatFamilyID, LastUpdateTimeStamp)
	SELECT DISTINCT 
		Nat_Category_Num,
		Nat_Category_Name,
		Nat_Family_Num,
		null
	FROM #tmpMainHierarchy

	-- Display Master Hierarchy tables after insert
	PRINT '[' + CONVERT(nvarchar, GETDATE(), 121) + '] ' + 'Displaying all NatItemFamily table after INSERT process...'
	SELECT * FROM NatItemFamily (NOLOCK)
	PRINT '[' + CONVERT(nvarchar, GETDATE(), 121) + '] ' + 'Displaying all NatItemCat table after INSERT process...'
	SELECT * FROM NatItemCat (NOLOCK)
	PRINT '[' + CONVERT(nvarchar, GETDATE(), 121) + '] ' + 'Displaying all NatItemClass table after INSERT process...'
	SELECT * FROM NatItemClass (NOLOCK)

	-- Create temporary ItemCoding table
	PRINT '[' + CONVERT(nvarchar, GETDATE(), 121) + '] ' + 'Creating #tmpItemCoding table...'

	if object_id('tempdb..#tmpItemCoding') is not null
	begin
		drop table #tmpItemCoding
	end
	CREATE TABLE #tmpItemCoding
		(
			[Identifier]				[bigint]		NOT NULL, -- We're forcing/converting the identifier to a number for joining.
			[Nat_Class_Num]				[int]			NOT NULL,
			PRIMARY KEY (
				[Identifier],
				[Nat_Class_Num]
			)
		) ON [PRIMARY]

	-- Import from temporary ItemCoding table
	PRINT '[' + CONVERT(nvarchar, GETDATE(), 121) + '] ' + 'Importing ItemCoding text file...'
	BULK INSERT #tmpItemCoding FROM '\\cewd6503\buildshare\National Hierarchy\ItemCoding.txt'
	WITH (FIRSTROW = 2,     FIELDTERMINATOR = '\t') 

	/*
		The following three "Item List" tables hold sets of items to be updated and facilitate displaying information about items that will be updated.
	*/
	
	-- Items with multiple identifiers that result in the item being mapped to multiple new item classes.  (The default identifier is ultimately used.)
	if object_id('tempdb..#ItemListMappedMultiple') is not null
	begin
		drop table #ItemListMappedMultiple
	end
	CREATE TABLE #ItemListMappedMultiple
		(
			Item_Key				int			NOT NULL,
			NewNatClassID		int			NOT NULL			
		) ON [PRIMARY]

	-- Items with a mapping defined from master/global/CEN list.
	if object_id('tempdb..#ItemListMapped') is not null
	begin
		drop table #ItemListMapped
	end
	CREATE TABLE #ItemListMapped
		(
			Item_Key				int			NOT NULL,
			NewNatClassID		int			NOT NULL			
			PRIMARY KEY (
				Item_Key				
			)
		) ON [PRIMARY]

	-- Items with a mapping defined from master/global/CEN list that do not have the correct class ID and need to be updated.
	if object_id('tempdb..#ItemListMappedUpdateNeeded') is not null
	begin
		drop table #ItemListMappedUpdateNeeded
	end
	CREATE TABLE #ItemListMappedUpdateNeeded
		(
			Item_Key				int			NOT NULL,
			NewNatClassID		int			NOT NULL			
			PRIMARY KEY (
				Item_Key				
			)
		) ON [PRIMARY]

	-- [Fix in v3] Items that are not mapped in the master list but have a national class ID that exists in the new class ID list.  These will retain their class ID (do not get "unidentified" classification).
	if object_id('tempdb..#ItemListUnmappedWithValidClassId') is not null
	begin
		drop table #ItemListUnmappedWithValidClassId
	end
	CREATE TABLE #ItemListUnmappedWithValidClassId
		(
			Item_Key				int			NOT NULL,
			NatClassID		int			NOT NULL			
			PRIMARY KEY (
				Item_Key		
				,NatClassID		
			)
		) ON [PRIMARY]

	-- Items that are not mapped in the master list.
	if object_id('tempdb..#ItemListUnmapped') is not null
	begin
		drop table #ItemListUnmapped
	end
	CREATE TABLE #ItemListUnmapped
		(
			Item_Key				int			NOT NULL,
			NewNatClassID		int			NOT NULL			
			PRIMARY KEY (
				Item_Key		
				,NewNatClassID		
			)
		) ON [PRIMARY]

	/*
	=================================================================
	=================================================================
	[[ Display Information About Items That Will Be Updated ]] 
	=================================================================
	=================================================================
	*/

	declare
		@mappedCount int -- Number of items (item keys) mapped to a unique hierarchy class.
		,@mappedUpdateNeededCount int -- Number of items (item keys) mapped to a unique hierarchy class that do not have the correct class and actually need to be updated.  Some mapped items will already have the correct class ID and will not be updated.
		,@unmappedCount int -- Number of items (item keys) that could not be mapped to a new hierarchy class.
		,@unmappedValidClassIDCount int -- Number of items (item keys) that were not in the global item mapping, but had valid class IDs in the new hierarchy, and were not updated.
		,@expectedUpdateCount int -- Total number of items we expect to update with new hierarchy class ID.
		,@totalUpdateCount int -- Total number of items updated with new hierarchy class ID.
		,@IRMAItemCount int -- Total number of targeted items for hierarchy class updates.

	select @IRMAItemCount = COUNT(*)
	from Item i

	PRINT '[' + CONVERT(nvarchar, GETDATE(), 121) + '] ' + 'Total items in scope for update in IRMA: ' + cast(@IRMAItemCount as varchar)


	PRINT '[' + CONVERT(nvarchar, GETDATE(), 121) + '] ' + 'Building unrestricted list of items that will include any items mapped to multiple new nat-class IDs...'
	-- Identify items with multiple identifiers that result in the item being mapped to multiple new item classes.
	insert into #ItemListMappedMultiple
		select distinct
			i.item_key
			,it.Nat_Class_Num
		FROM Item i 
			INNER JOIN ItemIdentifier ii ON i.Item_Key = ii.Item_Key
			INNER JOIN #tmpItemCoding it ON it.Identifier = cast(ii.identifier as bigint)

	PRINT '[' + CONVERT(nvarchar, GETDATE(), 121) + '] ' + 'Items added to unrestricted item list: ' + cast(@@rowcount as varchar)


	PRINT '[' + CONVERT(nvarchar, GETDATE(), 121) + '] ' + 'The following dataset shows identifier details for items containing multiple identifiers that result in the parent item mapping to more than one new nat-class:'
	select 'multiple new class IDs' dataset_desc, OldClassName = onic.ClassName, NewClassName = mh.Nat_Class_Name
	,i.Item_Description, i.SubTeam_No, ilmm.*, it.*, ii.* 
	from #ItemListMappedMultiple	ilmm
				INNER JOIN ItemIdentifier ii ON ilmm.Item_Key = ii.Item_Key
				INNER JOIN #tmpItemCoding it ON it.Identifier = cast(ii.identifier as bigint)
					and ilmm.NewNatClassID = it.Nat_Class_Num
				join Item i on ii.Item_Key = i.Item_Key -- To get item desc and subteam.
				join natitemclass onic on i.ClassID = onic.ClassID -- To get old nat-class name.
				join #tmpMainHierarchy mh on it.Nat_Class_Num = cast(mh.Nat_Class_Num as int) -- To get new nat-class name.
	where ilmm.Item_Key in (
		select item_key from #ItemListMappedMultiple group by Item_Key having COUNT(*) > 1
	)
	order by ilmm.Item_Key, ii.Default_Identifier, ii.Identifier

	PRINT '[' + CONVERT(nvarchar, GETDATE(), 121) + '] ' + 'Items and identifiers with multiple class IDs displayed: ' + cast(@@rowcount as varchar)


	PRINT '[' + CONVERT(nvarchar, GETDATE(), 121) + '] ' + 'Building list of mapped items for update...'
	-- Build list of unique item keys for items that are mapped from our source file.
	insert into #ItemListMapped
		select distinct
			i.item_key
			,it.Nat_Class_Num
		FROM Item i 
			INNER JOIN ItemIdentifier ii ON i.Item_Key = ii.Item_Key
			INNER JOIN #tmpItemCoding it ON it.Identifier = cast(ii.identifier as bigint)
		WHERE
			 ii.Default_Identifier = 1

	PRINT '[' + CONVERT(nvarchar, GETDATE(), 121) + '] ' + 'Items added to mapped-items list: ' + cast(@@rowcount as varchar)

	PRINT '[' + CONVERT(nvarchar, GETDATE(), 121) + '] ' + 'Building list of mapped items that do not already have the correct class ID and need to be updated:'
	insert into #ItemListMappedUpdateNeeded
		select
			i.item_key
			,ilm.NewNatClassID
		FROM Item i 
			-- We want pure joins here because we only care about items that ARE mapped in the item coding file.
			join ItemIdentifier ii
				ON i.Item_Key = ii.Item_Key
			join #tmpItemCoding it
				ON it.Identifier = cast(ii.identifier as bigint)
			join #ItemListMapped ilm ON i.Item_Key = ilm.Item_Key
		WHERE
			ii.default_identifier = 1 -- We use the default identifier to map items, so we only care about these identifiers for our already-mapped list.
			and isnull(i.classid, -1) <> ilm.NewNatClassID -- We are selecting items where their current class ID doesn't match the new nat-hier class ID.  *Important to not miss the items that have null class IDs.

	PRINT '[' + CONVERT(nvarchar, GETDATE(), 121) + '] ' + 'Items added to mapped-items-update-needed list: ' + cast(@@rowcount as varchar)


	PRINT '[' + CONVERT(nvarchar, GETDATE(), 121) + '] ' + 'The following dataset shows mapped items that already have correct class ID (will not be updated):'
	select
		'mapped, OK class ID' dataset_desc
		,i.item_key
		,i.item_description
		,i.subteam_no
		,ii.identifier
		,nic.ClassName
		,ItemClassID = i.classid
		,MappedNewClassID = ilm.NewNatClassID
		,ItemCodingClassID = it.Nat_Class_Num
	FROM Item i 
		-- We want pure joins here because we only care about items that ARE mapped in the item coding file.
		join ItemIdentifier ii
			ON i.Item_Key = ii.Item_Key
		join #tmpItemCoding it
			ON it.Identifier = cast(ii.identifier as bigint)
		join #ItemListMapped ilm
			ON i.Item_Key = ilm.Item_Key
		join NatItemClass nic
			on i.ClassID = nic.ClassID
	WHERE
		ii.default_identifier = 1 -- We use the default identifier to map items, so we only care about these identifiers for our already-mapped list.
		and i.classid = ilm.NewNatClassID


	PRINT '[' + CONVERT(nvarchar, GETDATE(), 121) + '] ' + 'No-update-needed mapped items displayed: ' + cast(@@rowcount as varchar)


	PRINT '[' + CONVERT(nvarchar, GETDATE(), 121) + '] ' + 'The following dataset shows items that were assigned a new class ID that is not defined in the new hierarchy:'
	PRINT '(These will be assigned to the *unidentified* class ID)'
	select
		'invalid new class ID' dataset_desc
		,ilm.*
		,i.item_description
		,i.subteam_no
		,ii.identifier
	from
		#ItemListMapped ilm
		join item i -- To get item desc and subteam.
			on ilm.item_key = i.item_key
		left join itemidentifier ii -- To get default identifier, LEFT JOIN in case no identifiers exists for the item.
			on i.item_key = ii.item_key and ii.default_identifier = 1
	where ilm.Item_Key not in (
		select
			ilm.Item_Key
		from #ItemListMapped ilm
		join #tmpMainHierarchy mh on ilm.NewNatClassID = cast(mh.Nat_Class_Num as int) -- This join only includes items that were mapped to a new hierarchy that is actually defined, so the NOT IN clause above will show the exceptions.
	)

	PRINT '[' + CONVERT(nvarchar, GETDATE(), 121) + '] ' + 'Items assigned to invalid new class ID: ' + cast(@@rowcount as varchar)


	PRINT '[' + CONVERT(nvarchar, GETDATE(), 121) + '] ' + 'Resetting items mapped to invalid nat class IDs, assigning unidentified class ID...'
	update #ItemListMappedUpdateNeeded
	set NewNatClassID = 99999
	where Item_Key not in (
		select
			ilmun.Item_Key
		from #ItemListMappedUpdateNeeded ilmun
		join #tmpMainHierarchy mh on ilmun.NewNatClassID = cast(mh.Nat_Class_Num as int) -- This join only includes items that were mapped to a new hierarchy that is actually defined, so the NOT IN clause above will show the exceptions.
	)

	PRINT '[' + CONVERT(nvarchar, GETDATE(), 121) + '] ' + 'Items changed to unidentified class ID in mapped-items list: ' + cast(@@rowcount as varchar)


	PRINT '[' + CONVERT(nvarchar, GETDATE(), 121) + '] ' + 'Building list of unmapped items for update...'
	PRINT 'Includes: 1) items/identifiers not in mapping file, 2) multi-identifier items mapped to multiple nat class IDs, 3) items with no associated identifiers.'
	-- Build list of unique item keys for items that were not mapped in the source file.
	insert into #ItemListUnmapped
		select distinct
			i.item_key
			,99999 -- Default: "Unidentified Items"
		FROM Item i 
			LEFT JOIN ItemIdentifier ii
				ON i.Item_Key = ii.Item_Key -- The LEFT join here is necessary to update item records that have no identifiers associated (item_key doesn't exist in ItemIdentifier table).  *This requires ISNULL checks in WHERE clause.
			LEFT JOIN #tmpItemCoding it
				ON it.Identifier = cast(ii.identifier as bigint)
		WHERE
			isnull(ii.Default_Identifier, 1) = 1 -- This will restrict to a single identifier, which will effectively be a single, unique item.
			AND it.identifier is null -- We only want cases where the unique (default) identifier in IRMA isn't mapped in global-mapping file.

	PRINT '[' + CONVERT(nvarchar, GETDATE(), 121) + '] ' + 'Items added to unmapped-items list: ' + cast(@@rowcount as varchar)


	PRINT '[' + CONVERT(nvarchar, GETDATE(), 121) + '] ' + 'Building list of unmapped items that will keep their original, valid nat class ID...'
	-- Build list of unique item keys, from unmapped items, that have a valid national class ID (class ID exists in NatItemClass table).
	insert into #ItemListUnmappedWithValidClassId
		select distinct
			i.item_key
			,i.classID
		FROM Item i
			join #ItemListUnmapped ilu -- Filter down to unmapped items only.
				on i.Item_Key = ilu.Item_Key
			join NatItemClass nic -- This links the new hierarchy class to the existing class ID in the item record, creating our filter for items that already have valid class IDs.
				on i.classID = nic.classID
			
	PRINT '[' + CONVERT(nvarchar, GETDATE(), 121) + '] ' + 'Items added to unmapped-items-with-valid-class-IDs list: ' + cast(@@rowcount as varchar)
			
	PRINT '[' + CONVERT(nvarchar, GETDATE(), 121) + '] ' + 'Removing unmapped items that will keep their original nat class ID from unmapped list...'
	delete #ItemListUnmapped
	from #ItemListUnmapped ilu
		join #ItemListUnmappedWithValidClassId del
			on ilu.item_key = del.item_key

	PRINT '[' + CONVERT(nvarchar, GETDATE(), 121) + '] ' + 'Keeping-original-class-ID items removed from unmapped-items list: ' + cast(@@rowcount as varchar)

	PRINT '[' + CONVERT(nvarchar, GETDATE(), 121) + '] ' + 'The following dataset shows unmapped items that will not be updated (retaining their current, valid class ID):'
	select
		'retain class ID' dataset_desc
		,iluv.*
		,nic.ClassName
		,i.item_description
		,i.subteam_no
		,ii.identifier
	from
		#ItemListUnmappedWithValidClassId iluv
		join item i -- To get item desc and subteam.
			on iluv.item_key = i.item_key
		left join itemidentifier ii -- To get default identifier, LEFT JOIN in case no identifiers exists for the item.
			on i.item_key = ii.item_key and ii.default_identifier = 1
		join NatItemClass nic
			on i.ClassID = nic.ClassID

	PRINT '[' + CONVERT(nvarchar, GETDATE(), 121) + '] ' + 'No-update, unmapped items displayed: ' + cast(@@rowcount as varchar)


	PRINT '[' + CONVERT(nvarchar, GETDATE(), 121) + '] ' + 'Adding items with no default identifier to list of unmapped items for update...'
	insert into #ItemListUnmapped
		select distinct
			i.item_key
			,99999 -- Default: "Unidentified Items"
		from
			Item i -- We take "everything else" from the Item table because the previous sets of queries (building mapped and unmapped lists) were restricted to default identifiers only.
			left join itemidentifier ii 
				on i.item_key = ii.item_key and ii.default_identifier = 1 -- Left join restricts to default identifiers, so we'll select all rows where the join fails.
			left join #ItemListUnmapped ilu
				on i.item_key = ilu.item_key -- We bring in the unmapped items so we can exclude them and prohibit duplicates.
			left join #ItemListUnmappedWithValidClassId iluv
				on i.item_key = iluv.item_key -- We also need to exclude items that were picked up when the ItemListUnmappedWithValidClassId list was generated.  These are items with no default identifier, but that have a valid class ID in the new class ID list.  We take rows where this join fails.
		WHERE
			ii.item_key is null -- If we have no ItemIdentifier record, there's no default identifier for the item.
			and ilu.item_key is null -- Exclude items already in the unmapped list.
			and iluv.item_key is null -- Exclude unmapped items with valid class IDs because they will not be updated so should not go into the "unmapped list for update".

	PRINT '[' + CONVERT(nvarchar, GETDATE(), 121) + '] ' + 'No-default-identifier items added to unmapped-items list: ' + cast(@@rowcount as varchar)

	PRINT '[' + CONVERT(nvarchar, GETDATE(), 121) + '] ' + 'The following dataset shows item-keys in both the mapped and unmapped lists (no item should be in both lists):'
	select *
	from
		#ItemListMapped ilm
		join #ItemListUnmapped ilu
			on ilm.item_key = ilu.item_key

	PRINT '[' + CONVERT(nvarchar, GETDATE(), 121) + '] ' + 'Items in both mapped-items and unmapped-items lists displayed: ' + cast(@@rowcount as varchar)


	/*
	=================================================================
	=================================================================
	[[ Update Items ]] 
	=================================================================
	=================================================================
	*/

	-- Create temporary NatHier_History table
	PRINT '[' + CONVERT(nvarchar, GETDATE(), 121) + '] ' + 'Creating #NatHier_History table...'
	if object_id('tempdb..#NatHier_History') is not null
	begin
		drop table #NatHier_History
	end
	CREATE TABLE #NatHier_History
		(
			Item_Key				int			NOT NULL,
			OldNatClassID		int			NULL,
			NewNatClassID		int			NOT NULL			
			PRIMARY KEY (
				Item_Key				
			)
		) ON [PRIMARY]


	if @applyItemUpdates = 1
	begin

		-- Update Item table with new Hierarchy Class ID
		PRINT '[' + CONVERT(nvarchar, GETDATE(), 121) + '] ' + 'Updating mapped items...'
		UPDATE Item
		SET 
				ClassID = ilmun.NewNatClassID
			OUTPUT inserted.Item_Key, deleted.ClassID, inserted.ClassID
			INTO #NatHier_History
		FROM Item i 
			INNER JOIN #ItemListMappedUpdateNeeded ilmun ON i.Item_Key = ilmun.Item_Key
		-- No WHERE clause needed because we did our filtering above when we built our list of mapped items to be updated.

		PRINT '[' + CONVERT(nvarchar, GETDATE(), 121) + '] ' + 'Item records updated from mapped-items-update-needed list: ' + cast(@@rowcount as varchar)

		PRINT '[' + CONVERT(nvarchar, GETDATE(), 121) + '] ' + 'Updating *un*mapped items...'
		-- Update non-mapped items, setting default hierarchy.
		UPDATE Item
		SET 
				ClassID = ilu.NewNatClassID -- Default: "Unidentified Items"
			OUTPUT inserted.Item_Key, deleted.ClassID, inserted.ClassID
			INTO #NatHier_History
		FROM Item i 
			INNER JOIN #ItemListUnmapped ilu ON i.Item_Key = ilu.Item_Key
		-- No WHERE clause needed because we did our filtering above when we created our list of mapped items to be updated.

		PRINT '[' + CONVERT(nvarchar, GETDATE(), 121) + '] ' + 'Item records updated from unmapped-items list: ' + cast(@@rowcount as varchar)
	end
	else
	begin
		PRINT '[' + CONVERT(nvarchar, GETDATE(), 121) + '] ' + '**** ITEM UPDATE SKIPPED **** ---> No item records were udpated.'
	end

	/*
	=================================================================
	=================================================================
	[[ Show Results ]] 
	=================================================================
	=================================================================
	*/

	PRINT '[' + CONVERT(nvarchar, GETDATE(), 121) + '] ' + 'The following dataset shows all updated items with their old and new nat-class values:'
	select
		'updated item' dataset_desc
		,h.Item_Key
		,ii.Identifier
		,i.Item_Description
		,st.SubTeam_Name
		,h.OldNatClassID
		,OldClassName = onic.ClassName
		,h.NewNatClassID
		,NewClassName = mh.Nat_Class_Name
	from
		#NatHier_History h 
		left join natitemclass onic -- Join to get old nat-class name; LEFT JOIN in case items had invalid old class IDs.
			on h.OldNatClassID = onic.ClassID
		left join #tmpMainHierarchy mh 
			on h.NewNatClassID = cast(mh.Nat_Class_Num as int) -- To get new nat-class name.
		join Item i -- For item desc/info.
			on h.Item_Key = i.Item_Key
		left join ItemIdentifier ii -- For default identifier.
			on i.Item_Key = ii.Item_Key and ii.Default_Identifier = 1
		left join SubTeam st -- For subteam name.
			on i.SubTeam_No = st.SubTeam_No

	PRINT '[' + CONVERT(nvarchar, GETDATE(), 121) + '] ' + 'Updated items displayed: ' + cast(@@rowcount as varchar)

	select @mappedCount = COUNT(*) from #ItemListMapped
	select @mappedUpdateNeededCount = COUNT(*) from #ItemListMappedUpdateNeeded
	select @unmappedCount = COUNT(*) from #ItemListUnmapped -- This list contains the items targeted for update.
	select @unmappedValidClassIDCount = COUNT(*) from #ItemListUnmappedWithValidClassId
	select @totalUpdateCount = COUNT(*) from #NatHier_History
	select @expectedUpdateCount = @unmappedCount + @mappedUpdateNeededCount

	declare @summaryPrefix varchar(64) = '[Summary] '
	print '[' + CONVERT(nvarchar, GETDATE(), 121) + '] ' + 'Item-counts summary:' 
	print @summaryPrefix + 'Total items in scope: ' + cast(@IRMAItemCount as varchar)
	print @summaryPrefix + 'Total mapped items: ' + cast(@mappedCount as varchar)
	print @summaryPrefix + 'Mapped valid-class-ID items (no update): ' + cast(@mappedCount - @mappedUpdateNeededCount as varchar)
	print @summaryPrefix + 'Mapped items needing nat-class update: ' + cast(@mappedUpdateNeededCount as varchar)
	print @summaryPrefix + 'Total unmapped items: ' + cast(@unmappedCount + @unmappedValidClassIDCount as varchar) 
	print @summaryPrefix + 'Unmapped valid-class-ID items (no update): ' + cast(@unmappedValidClassIDCount as varchar)
	print @summaryPrefix + 'Unmapped items needing nat-class update: ' + cast(@unmappedCount as varchar)
	print @summaryPrefix + 'Expected updates: ' + cast(@expectedUpdateCount as varchar)
	print @summaryPrefix + 'Item records updated: ' + cast(@totalUpdateCount as varchar)

	if @expectedUpdateCount = @totalUpdateCount
	begin
		print '[' + CONVERT(nvarchar, GETDATE(), 121) + '] ' + 'Update Results: All ' 
			+ cast(@expectedUpdateCount as varchar) + ' expected updates were made against the total ' 
			+ cast(@IRMAItemCount as varchar) + ' possible items in scope (' 
			+ cast(@unmappedValidClassIDCount + @mappedCount - @mappedUpdateNeededCount as varchar) + ' items not updated).'
	end
	else
	begin
		print '[' + CONVERT(nvarchar, GETDATE(), 121) + '] ' + '**WARNING** -- Overall Update Results: ' 
			+ cast((@expectedUpdateCount - @totalUpdateCount) as varchar) + ' items of the '
			+ cast(@expectedUpdateCount as varchar) + ' targeted were *NOT* updated.'

		print '[' + CONVERT(nvarchar, GETDATE(), 121) + '] ' + 'Item-keys not updated:' 
		select ilm.*
		from #ItemListMapped ilm
			left join #NatHier_History h
				on ilm.item_key = h.item_key
		WHERE
			h.item_key is null
		union
		select ilu.*
		from #ItemListUnmapped ilu
			left join #NatHier_History h
				on ilu.item_key = h.item_key
		WHERE
			h.item_key is null

		PRINT '[' + CONVERT(nvarchar, GETDATE(), 121) + '] ' + 'Not-updated items displayed: ' + cast(@@rowcount as varchar)
	end
	
	
    IF @@TRANCOUNT > 0
    	PRINT '[' + CONVERT(nvarchar, GETDATE(), 121) + '] ' + 'Committing transaction...'
		COMMIT TRANSACTION
		PRINT '[' + CONVERT(nvarchar, GETDATE(), 121) + '] ' + 'The National-Hierarchy updates were saved successfully.'


END TRY
----------------------------------------------------- E   N   D ------------------------------------------------------   

BEGIN CATCH
	IF @@TRANCOUNT > 0
		ROLLBACK TRANSACTION
		SELECT
			[Info] = 'An error occurred.  Database changes were rolled back!',
			[ErrorNumber] =		ERROR_NUMBER(),
			[ErrorSeverity] =	ERROR_SEVERITY(),
			[ErrorState] =		ERROR_STATE(),
			[ErrorProcedure] =	ERROR_PROCEDURE(),
			[ErrorLine] =		ERROR_LINE(),
			[ErrorMessage] =	ERROR_MESSAGE()

END CATCH 
go


-- Enable execution, in case it was disabled due to an error above, and ensure triggers are enabled.
set noexec off
go


PRINT '[' + CONVERT(nvarchar, GETDATE(), 121) + '] ' + 'Enabling item-table triggers...'
/*
	If errors occur, there is typically an uncommitted transaction "lingering".
	We commit changes above, if all is well, so otherwise, we attempt a roll back here, to ensure we can enable triggers.
*/
IF @@TRANCOUNT > 0
	ROLLBACK TRANSACTION
go
enable trigger all on item
go

