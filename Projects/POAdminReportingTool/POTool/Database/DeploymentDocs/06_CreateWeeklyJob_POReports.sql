USE [msdb]
GO

/****** Object:  Job [GetWeeklyOrders_AllRegions]    Script Date: 11/6/2013 3:51:58 PM ******/
BEGIN TRANSACTION
DECLARE @ReturnCode INT
SELECT @ReturnCode = 0
/****** Object:  JobCategory [[Uncategorized (Local)]]]    Script Date: 11/6/2013 3:51:59 PM ******/
IF NOT EXISTS (SELECT name FROM msdb.dbo.syscategories WHERE name=N'[Uncategorized (Local)]' AND category_class=1)
BEGIN
EXEC @ReturnCode = msdb.dbo.sp_add_category @class=N'JOB', @type=N'LOCAL', @name=N'[Uncategorized (Local)]'
IF (@@ERROR <> 0 OR @ReturnCode <> 0) GOTO QuitWithRollback

END

DECLARE @jobId BINARY(16)
EXEC @ReturnCode =  msdb.dbo.sp_add_job @job_name=N'POReports_GetWeeklyOrders_AllRegions', 
		@enabled=1, 
		@notify_level_eventlog=0, 
		@notify_level_email=0, 
		@notify_level_netsend=0, 
		@notify_level_page=0, 
		@delete_level=0, 
		@description=N'This job gets all the PO information from the previous fiscal week.', 
		@category_name=N'[Uncategorized (Local)]', 
		@owner_login_name=N'sa', @job_id = @jobId OUTPUT
IF (@@ERROR <> 0 OR @ReturnCode <> 0) GOTO QuitWithRollback
/****** Object:  Step [FL_Get Weekly Orders By Close Date]    Script Date: 11/6/2013 3:51:59 PM ******/
EXEC @ReturnCode = msdb.dbo.sp_add_jobstep @job_id=@jobId, @step_name=N'FL_Get Weekly Orders By Close Date', 
		@step_id=1, 
		@cmdexec_success_code=0, 
		@on_success_action=3, 
		@on_success_step_id=0, 
		@on_fail_action=4, 
		@on_fail_step_id=27, 
		@retry_attempts=0, 
		@retry_interval=0, 
		@os_run_priority=0, @subsystem=N'TSQL', 
		@command=N'-- FL
USE POReports;
GO

--**************************************************************************
-- Create the synonyms to the appropriate IRMA database
--**************************************************************************
if exists (select * from sysobjects where xtype = ''SN'' and name = ''OrderHeader'')		drop synonym OrderHeader;
if exists (select * from sysobjects where xtype = ''SN'' and name = ''OrderItem'')			drop synonym OrderItem;
if exists (select * from sysobjects where xtype = ''SN'' and name = ''Vendor'')				drop synonym Vendor;
if exists (select * from sysobjects where xtype = ''SN'' and name = ''Users'')				drop synonym Users;
if exists (select * from sysobjects where xtype = ''SN'' and name = ''Store'')				drop synonym Store;
if exists (select * from sysobjects where xtype = ''SN'' and name = ''SubTeam'')			drop synonym SubTeam;
if exists (select * from sysobjects where xtype = ''SN'' and name = ''ReasonCodeDetail'')	drop synonym ReasonCodeDetail;
if exists (select * from sysobjects where xtype = ''SN'' and name = ''Region'')				drop synonym Region;

if exists (select * from sys.servers where name = ''IDD-FL\FLD'')
	begin
		
        create synonym OrderHeader		for [IDD-FL\FLD].[ItemCatalog_Test].[dbo].OrderHeader;
		create synonym OrderItem		for [IDD-FL\FLD].[ItemCatalog_Test].[dbo].OrderItem;
        create synonym Vendor			for [IDD-FL\FLD].[ItemCatalog_Test].[dbo].Vendor;
		create synonym Users			for [IDD-FL\FLD].[ItemCatalog_Test].[dbo].Users;
		create synonym Store			for [IDD-FL\FLD].[ItemCatalog_Test].[dbo].Store;
		create synonym SubTeam			for [IDD-FL\FLD].[ItemCatalog_Test].[dbo].SubTeam;
		create synonym ReasonCodeDetail for [IDD-FL\FLD].[ItemCatalog_Test].[dbo].ReasonCodeDetail;
		create synonym Region			for [IDD-FL\FLD].[ItemCatalog_Test].[dbo].Region;
    end
else if exists (select * from sys.servers where name = ''IDQ-FL\FLQ'')
	begin
        create synonym OrderHeader		for [IDQ-FL\FLQ].[ItemCatalog].[dbo].OrderHeader;
		create synonym OrderItem		for [IDQ-FL\FLQ].[ItemCatalog].[dbo].OrderItem;
        create synonym Vendor			for [IDQ-FL\FLQ].[ItemCatalog].[dbo].Vendor;
		create synonym Users			for [IDQ-FL\FLQ].[ItemCatalog].[dbo].Users;
		create synonym Store			for [IDQ-FL\FLQ].[ItemCatalog].[dbo].Store;
		create synonym SubTeam			for [IDQ-FL\FLQ].[ItemCatalog].[dbo].SubTeam;
		create synonym ReasonCodeDetail for [IDQ-FL\FLQ].[ItemCatalog].[dbo].ReasonCodeDetail;
		create synonym Region			for [IDQ-FL\FLQ].[ItemCatalog].[dbo].Region;
	end    
else if exists (select * from sys.servers where name = ''IDP-FL\FLP'')
	begin
        create synonym OrderHeader		for [IDP-FL\FLP].[ItemCatalog].[dbo].OrderHeader;
		create synonym OrderItem		for [IDP-FL\FLP].[ItemCatalog].[dbo].OrderItem;
        create synonym Vendor			for [IDP-FL\FLP].[ItemCatalog].[dbo].Vendor;
		create synonym Users			for [IDP-FL\FLP].[ItemCatalog].[dbo].Users;
		create synonym Store			for [IDP-FL\FLP].[ItemCatalog].[dbo].Store;
		create synonym SubTeam			for [IDP-FL\FLP].[ItemCatalog].[dbo].SubTeam;
		create synonym ReasonCodeDetail for [IDP-FL\FLP].[ItemCatalog].[dbo].ReasonCodeDetail;
		create synonym Region			for [IDP-FL\FLP].[ItemCatalog].[dbo].Region;
	end

--**************************************************************************
-- Set and Populate internal variables
--**************************************************************************
DECLARE @startdate datetime;
DECLARE @enddate datetime;
DECLARE @region	varchar(2);
DECLARE @insertDate	datetime2;

-- Get Monday 00:00:00 of Last Week and Get Sunday 23:59:59.997 of Last Week
SELECT @startdate = DATEADD(day, -7 , DATEADD(wk, DATEDIFF(wk, 0, GETDATE()), 0))
SELECT @enddate = DATEADD(ms, -3, DATEADD(wk, DATEDIFF(wk, 0, GETDATE()), 0))

SELECT  @region			= RegionCode FROM Region;
SELECT	@insertDate		= GETDATE();

--**************************************************************************
-- Get List of Orders and Insert Into PODataLoad table
--**************************************************************************
BEGIN TRY
BEGIN TRAN

	-- Insert into PODataLoad table
	INSERT INTO PODataLoad
	SELECT 
		oh.OrderHeader_ID											as PONumber,
		CASE 
			WHEN oh.ResolutionCodeID IS NOT NULL AND oh.ApprovedDate IS NOT NULL THEN ''Y'' 
			WHEN oh.ApprovedDate IS NULL THEN ''Y''
			ELSE ''N'' 
		END															as Suspended,
		oh.CloseDate												as CloseDate,
		CASE
			WHEN oh.ResolutionCodeID IS NOT NULL AND oh.ApprovedDate IS NOT NULL THEN rcd.ReasonCodeDesc
			WHEN oh.ApprovedDate IS NULL AND oh.InvoiceNumber IS NOT NULL THEN ''Suspended but not Approved''
			WHEN oh.ApprovedDate IS NULL AND oh.InvoiceNumber IS NULL THEN ''Closed as Other/None''
			ELSE ''''
		END															as ResolutionCode,
		ISNULL(oh.AdminNotes, '''')									as AdminNotes,
		v.CompanyName												as Vendor,
		st.SubTeam_No												as Subteam,
		s.Store_Name												as Store,
		CASE 
			WHEN oh.DiscountType > 0 THEN ''Y''
			WHEN 0 < SUM(oi.DiscountType) Then ''Y''
			WHEN 0 < SUM(oi.AdjustedCost) Then ''Y''
			ELSE ''N''
		END															as AdjustedCost,
		CASE 
			WHEN oh.Return_Order = 1 THEN ''Y'' 
			ELSE ''N'' 
		END															as CreditPO,
		CASE 
			WHEN oh.PayByAgreedCost = 1 THEN ''Pay By Agreed Cost'' 
			ELSE ''Pay By Invoice'' 
		END															as VendorType,
		ucr.FullName												as POCreator,
		CASE 
			WHEN oh.Einvoice_id IS NOT NULL THEN ''Y'' 
			ELSE ''N'' 
		END															as EInvoiceMatchedToPO,
		ISNULL(oh.OrderHeaderDesc, '''')								as PONotes,
		ucl.FullName												as ClosedBy,
		@region														as Region,
		oh.ApprovedDate												as ApprovedDate,
		oh.InvoiceNumber											as InvoiceNumber,
		@insertDate													as InsertDate
	FROM
		OrderHeader					oh	(nolock)
		INNER JOIN OrderItem		oi	(nolock) on oh.OrderHeader_ID		= oi.OrderHeader_ID
		INNER JOIN Vendor			vr	(nolock) on oh.ReceiveLocation_ID	= vr.Vendor_ID
		INNER JOIN Vendor			v	(nolock) on oh.Vendor_ID			= v.Vendor_ID
		INNER JOIN Users			ucr (nolock) on oh.CreatedBy			= ucr.User_ID
		INNER JOIN Users			ucl	(nolock) on oh.ClosedBy				= ucl.User_ID
		INNER JOIN Store			s	(nolock) on vr.Store_No				= s.Store_No
		INNER JOIN SubTeam			st	(nolock) on oh.Transfer_To_SubTeam	= st.SubTeam_No
		LEFT JOIN ReasonCodeDetail	rcd (nolock) on oh.ResolutionCodeID		= rcd.ReasonCodeDetailID
	WHERE
		oh.CloseDate		>= @StartDate
		AND oh.CloseDate	<= @EndDate
		AND oh.OrderType_ID <> 3 -- filter out Transfer orders
	GROUP BY
		oh.OrderHeader_ID,
		oh.DiscountType,
		oh.Return_Order,
		oh.PayByAgreedCost,
		oh.eInvoice_Id,
		oh.ResolutionCodeID,
		oh.Vendor_ID,
		oh.ReceiveLocation_ID,
		oh.CreatedBy,
		oh.ClosedBy,
		oh.Transfer_To_SubTeam,
		oh.AdminNotes,
		oh.OrderHeaderDesc,
		oh.CloseDate,
		oh.ApprovedDate,
		oh.InvoiceNumber,
		rcd.ReasonCodeDesc,
		v.CompanyName,
		st.SubTeam_No,
		s.Store_Name,
		ucr.FullName,
		ucl.FullName

	PRINT ''Data has been loaded into PODataLoad table for the '' + @region + '' region.''
	
	-- Insert data into PODataLoadStatus
	INSERT INTO dbo.PODataLoadStatus (Region, InsertDate)
	SELECT @region, @insertDate;
	PRINT ''PODataLoadStatus table has been updated for InsertDate: '' + CAST(@insertDate as nvarchar) + '' and Region: '' + @region + ''.'';

	COMMIT TRAN

END TRY
BEGIN CATCH
	IF @@TRANCOUNT > 0
    	ROLLBACK TRAN
	DECLARE @err_no int, @err_sev int, @err_msg varchar(MAX)
    SELECT @err_no = ERROR_NUMBER(), @err_sev = ERROR_SEVERITY(), @err_msg = ERROR_MESSAGE()
	RAISERROR (''GetWeeklyPOData failed with error no: %d and message: %s'', @err_sev, 1, @err_no, @err_msg)
END CATCH', 
		@database_name=N'POReports', 
		@flags=0
IF (@@ERROR <> 0 OR @ReturnCode <> 0) GOTO QuitWithRollback
/****** Object:  Step [FL_Update Approved Orders]    Script Date: 11/6/2013 3:51:59 PM ******/
EXEC @ReturnCode = msdb.dbo.sp_add_jobstep @job_id=@jobId, @step_name=N'FL_Update Approved Orders', 
		@step_id=2, 
		@cmdexec_success_code=0, 
		@on_success_action=3, 
		@on_success_step_id=0, 
		@on_fail_action=4, 
		@on_fail_step_id=27, 
		@retry_attempts=0, 
		@retry_interval=0, 
		@os_run_priority=0, @subsystem=N'TSQL', 
		@command=N'--**************************************************************************
-- Job Script: PO_Job_GetWeeklyPODataFromIRMA
-- Author: Ben Sims
-- Date: 08/19/2013
--
-- Description:
-- This script is called from the Suspended PO Tool Reporting Database SQL Agent
-- This is the TEST version which connects to IDT-XX\XXT or IDD-XX\XXD.dbo.ItemCatalog_Test
--
-- Modification History:
-- Date       	Init  	TFS   	Comment
-- 08/19/2013	BAS   	12061	Edited Initial SQL Script created by Bryce Bartley
--								which used to query OrderHeaderHistory
-- 08/29/2013	BAS		12061	Changed oh.AdjustedReceivedCost to CASE statement logic
--								and updated ResolutionCode logic
-- 09/18/2013	BAS		12061	Added synonyms for readability and to make it
--								non-environment specific.
--**************************************************************************
USE POReports;
GO

--**************************************************************************
-- Create the synonyms to the appropriate IRMA database
--**************************************************************************
if exists (select * from sysobjects where xtype = ''SN'' and name = ''OrderHeader'')		drop synonym OrderHeader;
if exists (select * from sysobjects where xtype = ''SN'' and name = ''OrderItem'')			drop synonym OrderItem;
if exists (select * from sysobjects where xtype = ''SN'' and name = ''Vendor'')				drop synonym Vendor;
if exists (select * from sysobjects where xtype = ''SN'' and name = ''Users'')				drop synonym Users;
if exists (select * from sysobjects where xtype = ''SN'' and name = ''Store'')				drop synonym Store;
if exists (select * from sysobjects where xtype = ''SN'' and name = ''SubTeam'')			drop synonym SubTeam;
if exists (select * from sysobjects where xtype = ''SN'' and name = ''ReasonCodeDetail'')	drop synonym ReasonCodeDetail;
if exists (select * from sysobjects where xtype = ''SN'' and name = ''Region'')				drop synonym Region;

if exists (select * from sys.servers where name = ''IDD-FL\FLD'')
	begin
		
        create synonym OrderHeader		for [IDD-FL\FLD].[ItemCatalog_Test].[dbo].OrderHeader;
		create synonym OrderItem		for [IDD-FL\FLD].[ItemCatalog_Test].[dbo].OrderItem;
        create synonym Vendor			for [IDD-FL\FLD].[ItemCatalog_Test].[dbo].Vendor;
		create synonym Users			for [IDD-FL\FLD].[ItemCatalog_Test].[dbo].Users;
		create synonym Store			for [IDD-FL\FLD].[ItemCatalog_Test].[dbo].Store;
		create synonym SubTeam			for [IDD-FL\FLD].[ItemCatalog_Test].[dbo].SubTeam;
		create synonym ReasonCodeDetail for [IDD-FL\FLD].[ItemCatalog_Test].[dbo].ReasonCodeDetail;
		create synonym Region			for [IDD-FL\FLD].[ItemCatalog_Test].[dbo].Region;
    end
else if exists (select * from sys.servers where name = ''IDQ-FL\FLQ'')
	begin
        create synonym OrderHeader		for [IDQ-FL\FLQ].[ItemCatalog].[dbo].OrderHeader;
		create synonym OrderItem		for [IDQ-FL\FLQ].[ItemCatalog].[dbo].OrderItem;
        create synonym Vendor			for [IDQ-FL\FLQ].[ItemCatalog].[dbo].Vendor;
		create synonym Users			for [IDQ-FL\FLQ].[ItemCatalog].[dbo].Users;
		create synonym Store			for [IDQ-FL\FLQ].[ItemCatalog].[dbo].Store;
		create synonym SubTeam			for [IDQ-FL\FLQ].[ItemCatalog].[dbo].SubTeam;
		create synonym ReasonCodeDetail for [IDQ-FL\FLQ].[ItemCatalog].[dbo].ReasonCodeDetail;
		create synonym Region			for [IDQ-FL\FLQ].[ItemCatalog].[dbo].Region;
	end    
else if exists (select * from sys.servers where name = ''IDP-FL\FLP'')
	begin
        create synonym OrderHeader		for [IDP-FL\FLP].[ItemCatalog].[dbo].OrderHeader;
		create synonym OrderItem		for [IDP-FL\FLP].[ItemCatalog].[dbo].OrderItem;
        create synonym Vendor			for [IDP-FL\FLP].[ItemCatalog].[dbo].Vendor;
		create synonym Users			for [IDP-FL\FLP].[ItemCatalog].[dbo].Users;
		create synonym Store			for [IDP-FL\FLP].[ItemCatalog].[dbo].Store;
		create synonym SubTeam			for [IDP-FL\FLP].[ItemCatalog].[dbo].SubTeam;
		create synonym ReasonCodeDetail for [IDP-FL\FLP].[ItemCatalog].[dbo].ReasonCodeDetail;
		create synonym Region			for [IDP-FL\FLP].[ItemCatalog].[dbo].Region;
	end

--**************************************************************************
-- Set and Populate internal variables
--**************************************************************************
DECLARE @startdate datetime;
DECLARE @enddate datetime;
DECLARE @region	varchar(2);

-- Get Monday 00:00:00 of Last Week
-- SELECT @startdate = DATEADD(day, -7 , DATEADD(wk, DATEDIFF(wk, 0, GETDATE()), 0))

-- for testing, get two weeks ago start
SELECT @startdate = DATEADD(day, -14 , DATEADD(wk, DATEDIFF(wk, 0, GETDATE()), 0))

-- Get Sunday 23:59:59.997 of Last Week
--SELECT @enddate = DATEADD(ms, -3, DATEADD(wk, DATEDIFF(wk, 0, GETDATE()), 0))

-- for testing, get two weeks ago end
SELECT @enddate = DATEADD(ms, -3, DATEADD(wk, DATEDIFF(wk, 0, GETDATE()-7), 0))

select @startdate, @enddate
SELECT  @region	= RegionCode FROM Region;

--**************************************************************************
-- Get list of orders that were approved during the previous week
-- This will be used for updating orders in POData
--**************************************************************************
SELECT
	oh.OrderHeader_ID				as PONumber,
	oh.ApprovedDate					as ApprovedDate,
	ISNULL(rcd.ReasonCodeDesc, '''')	as ResolutionCode,
	oh.InvoiceNumber				as InvoiceNumber
INTO #approvedOrders
FROM
	OrderHeader					oh	(nolock)
	LEFT JOIN ReasonCodeDetail	rcd (nolock) on oh.ResolutionCodeID	= rcd.ReasonCodeDetailID
WHERE
	oh.ApprovedDate		>= @startdate
	AND oh.ApprovedDate <= @enddate

create clustered index idx_c_ApprovedOrders_PONumber on #approvedOrders (PONumber)
create nonclustered index idx_ApprovedOrders_ResolutionCode on #approvedOrders (ResolutionCode)

--**************************************************************************
-- Update orders in POData that were ''Suspended but not Approved'' or 
-- orders that were closed as Other/None but are now approved
--**************************************************************************
-- Update orders with resolution code if there is one assigned for orders that have invoice info.
DECLARE @err_no int, @err_sev int, @err_msg varchar(MAX)

BEGIN TRY
	BEGIN TRAN
		UPDATE pod
		SET
			pod.ResolutionCode = ao.ResolutionCode,
			pod.ApprovedDate = ao.ApprovedDate
		FROM
			POData pod
			INNER JOIN #approvedOrders ao on pod.PONumber = ao.PONumber
		WHERE
			ao.ResolutionCode <> ''''
			AND ao.InvoiceNumber IS NOT NULL
			AND ao.ApprovedDate IS NOT NULL

	COMMIT TRAN
END TRY
BEGIN CATCH
	IF @@TRANCOUNT > 0
    	ROLLBACK TRAN
    SELECT @err_no = ERROR_NUMBER(), @err_sev = ERROR_SEVERITY(), @err_msg = ERROR_MESSAGE()
	RAISERROR (''Updating Newly Approved Orders with Resolution Code failed with error no: %d and message: %s'', @err_sev, 1, @err_no, @err_msg)
END CATCH

-- Update the suspended status for orders closed as Other/None that do not have resolution code but are now approved
BEGIN TRY
	BEGIN TRAN
		UPDATE pod
		SET
			pod.Suspended = ''N'',
			pod.ApprovedDate = ao.ApprovedDate
		FROM
			POData pod
			INNER JOIN #approvedOrders ao on pod.PONumber = ao.PONumber
		WHERE
			ao.ResolutionCode = ''''
			AND ao.ApprovedDate IS NOT NULL
			AND ao.InvoiceNumber IS NOT NULL

		COMMIT TRAN
END TRY
BEGIN CATCH
	IF @@TRANCOUNT > 0
    	ROLLBACK TRAN
    SELECT @err_no = ERROR_NUMBER(), @err_sev = ERROR_SEVERITY(), @err_msg = ERROR_MESSAGE()
	RAISERROR (''Updating the Suspended Status of orders that were closed as Other/None failed with error no: %d and message: %s'', @err_sev, 1, @err_no, @err_msg)
END CATCH', 
		@database_name=N'POReports', 
		@flags=0
IF (@@ERROR <> 0 OR @ReturnCode <> 0) GOTO QuitWithRollback
/****** Object:  Step [MA_Get Weekly Orders By Close Date]    Script Date: 11/6/2013 3:51:59 PM ******/
EXEC @ReturnCode = msdb.dbo.sp_add_jobstep @job_id=@jobId, @step_name=N'MA_Get Weekly Orders By Close Date', 
		@step_id=3, 
		@cmdexec_success_code=0, 
		@on_success_action=3, 
		@on_success_step_id=0, 
		@on_fail_action=4, 
		@on_fail_step_id=27, 
		@retry_attempts=0, 
		@retry_interval=0, 
		@os_run_priority=0, @subsystem=N'TSQL', 
		@command=N'-- MA
USE POReports;
GO

--**************************************************************************
-- Create the synonyms to the appropriate IRMA database
--**************************************************************************
if exists (select * from sysobjects where xtype = ''SN'' and name = ''OrderHeader'')		drop synonym OrderHeader;
if exists (select * from sysobjects where xtype = ''SN'' and name = ''OrderItem'')			drop synonym OrderItem;
if exists (select * from sysobjects where xtype = ''SN'' and name = ''Vendor'')				drop synonym Vendor;
if exists (select * from sysobjects where xtype = ''SN'' and name = ''Users'')				drop synonym Users;
if exists (select * from sysobjects where xtype = ''SN'' and name = ''Store'')				drop synonym Store;
if exists (select * from sysobjects where xtype = ''SN'' and name = ''SubTeam'')			drop synonym SubTeam;
if exists (select * from sysobjects where xtype = ''SN'' and name = ''ReasonCodeDetail'')	drop synonym ReasonCodeDetail;
if exists (select * from sysobjects where xtype = ''SN'' and name = ''Region'')				drop synonym Region;

if exists (select * from sys.servers where name = ''IDD-MA\MAD'')
	begin
		
        create synonym OrderHeader		for [IDD-MA\MAD].[ItemCatalog_Test].[dbo].OrderHeader;
		create synonym OrderItem		for [IDD-MA\MAD].[ItemCatalog_Test].[dbo].OrderItem;
        create synonym Vendor			for [IDD-MA\MAD].[ItemCatalog_Test].[dbo].Vendor;
		create synonym Users			for [IDD-MA\MAD].[ItemCatalog_Test].[dbo].Users;
		create synonym Store			for [IDD-MA\MAD].[ItemCatalog_Test].[dbo].Store;
		create synonym SubTeam			for [IDD-MA\MAD].[ItemCatalog_Test].[dbo].SubTeam;
		create synonym ReasonCodeDetail for [IDD-MA\MAD].[ItemCatalog_Test].[dbo].ReasonCodeDetail;
		create synonym Region			for [IDD-MA\MAD].[ItemCatalog_Test].[dbo].Region;
    end
else if exists (select * from sys.servers where name = ''IDQ-MA\MAQ'')
	begin
        create synonym OrderHeader		for [IDQ-MA\MAQ].[ItemCatalog].[dbo].OrderHeader;
		create synonym OrderItem		for [IDQ-MA\MAQ].[ItemCatalog].[dbo].OrderItem;
        create synonym Vendor			for [IDQ-MA\MAQ].[ItemCatalog].[dbo].Vendor;
		create synonym Users			for [IDQ-MA\MAQ].[ItemCatalog].[dbo].Users;
		create synonym Store			for [IDQ-MA\MAQ].[ItemCatalog].[dbo].Store;
		create synonym SubTeam			for [IDQ-MA\MAQ].[ItemCatalog].[dbo].SubTeam;
		create synonym ReasonCodeDetail for [IDQ-MA\MAQ].[ItemCatalog].[dbo].ReasonCodeDetail;
		create synonym Region			for [IDQ-MA\MAQ].[ItemCatalog].[dbo].Region;
	end    
else if exists (select * from sys.servers where name = ''IDP-MA\MAP'')
	begin
        create synonym OrderHeader		for [IDP-MA\MAP].[ItemCatalog].[dbo].OrderHeader;
		create synonym OrderItem		for [IDP-MA\MAP].[ItemCatalog].[dbo].OrderItem;
        create synonym Vendor			for [IDP-MA\MAP].[ItemCatalog].[dbo].Vendor;
		create synonym Users			for [IDP-MA\MAP].[ItemCatalog].[dbo].Users;
		create synonym Store			for [IDP-MA\MAP].[ItemCatalog].[dbo].Store;
		create synonym SubTeam			for [IDP-MA\MAP].[ItemCatalog].[dbo].SubTeam;
		create synonym ReasonCodeDetail for [IDP-MA\MAP].[ItemCatalog].[dbo].ReasonCodeDetail;
		create synonym Region			for [IDP-MA\MAP].[ItemCatalog].[dbo].Region;
	end

--**************************************************************************
-- Set and Populate internal variables
--**************************************************************************
DECLARE @startdate datetime;
DECLARE @enddate datetime;
DECLARE @region	varchar(2);
DECLARE @insertDate	datetime2;

-- Get Monday 00:00:00 of Last Week and Get Sunday 23:59:59.997 of Last Week
SELECT @startdate = DATEADD(day, -7 , DATEADD(wk, DATEDIFF(wk, 0, GETDATE()), 0))
SELECT @enddate = DATEADD(ms, -3, DATEADD(wk, DATEDIFF(wk, 0, GETDATE()), 0))

SELECT  @region			= RegionCode FROM Region;
SELECT	@insertDate		= GETDATE();

--**************************************************************************
-- Get List of Orders and Insert Into PODataLoad table
--**************************************************************************
BEGIN TRY
BEGIN TRAN

	-- Insert into PODataLoad table
	INSERT INTO PODataLoad
	SELECT 
		oh.OrderHeader_ID											as PONumber,
		CASE 
			WHEN oh.ResolutionCodeID IS NOT NULL AND oh.ApprovedDate IS NOT NULL THEN ''Y'' 
			WHEN oh.ApprovedDate IS NULL THEN ''Y''
			ELSE ''N'' 
		END															as Suspended,
		oh.CloseDate												as CloseDate,
		CASE
			WHEN oh.ResolutionCodeID IS NOT NULL AND oh.ApprovedDate IS NOT NULL THEN rcd.ReasonCodeDesc
			WHEN oh.ApprovedDate IS NULL AND oh.InvoiceNumber IS NOT NULL THEN ''Suspended but not Approved''
			WHEN oh.ApprovedDate IS NULL AND oh.InvoiceNumber IS NULL THEN ''Closed as Other/None''
			ELSE ''''
		END															as ResolutionCode,
		ISNULL(oh.AdminNotes, '''')									as AdminNotes,
		v.CompanyName												as Vendor,
		st.SubTeam_No												as Subteam,
		s.Store_Name												as Store,
		CASE 
			WHEN oh.DiscountType > 0 THEN ''Y''
			WHEN 0 < SUM(oi.DiscountType) Then ''Y''
			WHEN 0 < SUM(oi.AdjustedCost) Then ''Y''
			ELSE ''N''
		END															as AdjustedCost,
		CASE 
			WHEN oh.Return_Order = 1 THEN ''Y'' 
			ELSE ''N'' 
		END															as CreditPO,
		CASE 
			WHEN oh.PayByAgreedCost = 1 THEN ''Pay By Agreed Cost'' 
			ELSE ''Pay By Invoice'' 
		END															as VendorType,
		ucr.FullName												as POCreator,
		CASE 
			WHEN oh.Einvoice_id IS NOT NULL THEN ''Y'' 
			ELSE ''N'' 
		END															as EInvoiceMatchedToPO,
		ISNULL(oh.OrderHeaderDesc, '''')								as PONotes,
		ucl.FullName												as ClosedBy,
		@region														as Region,
		oh.ApprovedDate												as ApprovedDate,
		oh.InvoiceNumber											as InvoiceNumber,
		@insertDate													as InsertDate
	FROM
		OrderHeader					oh	(nolock)
		INNER JOIN OrderItem		oi	(nolock) on oh.OrderHeader_ID		= oi.OrderHeader_ID
		INNER JOIN Vendor			vr	(nolock) on oh.ReceiveLocation_ID	= vr.Vendor_ID
		INNER JOIN Vendor			v	(nolock) on oh.Vendor_ID			= v.Vendor_ID
		INNER JOIN Users			ucr (nolock) on oh.CreatedBy			= ucr.User_ID
		INNER JOIN Users			ucl	(nolock) on oh.ClosedBy				= ucl.User_ID
		INNER JOIN Store			s	(nolock) on vr.Store_No				= s.Store_No
		INNER JOIN SubTeam			st	(nolock) on oh.Transfer_To_SubTeam	= st.SubTeam_No
		LEFT JOIN ReasonCodeDetail	rcd (nolock) on oh.ResolutionCodeID		= rcd.ReasonCodeDetailID
	WHERE
		oh.CloseDate		>= @StartDate
		AND oh.CloseDate	<= @EndDate
		AND oh.OrderType_ID <> 3 -- filter out Transfer orders
	GROUP BY
		oh.OrderHeader_ID,
		oh.DiscountType,
		oh.Return_Order,
		oh.PayByAgreedCost,
		oh.eInvoice_Id,
		oh.ResolutionCodeID,
		oh.Vendor_ID,
		oh.ReceiveLocation_ID,
		oh.CreatedBy,
		oh.ClosedBy,
		oh.Transfer_To_SubTeam,
		oh.AdminNotes,
		oh.OrderHeaderDesc,
		oh.CloseDate,
		oh.ApprovedDate,
		oh.InvoiceNumber,
		rcd.ReasonCodeDesc,
		v.CompanyName,
		st.SubTeam_No,
		s.Store_Name,
		ucr.FullName,
		ucl.FullName

	PRINT ''Data has been loaded into PODataLoad table for the '' + @region + '' region.''
	
	-- Insert data into PODataLoadStatus
	INSERT INTO dbo.PODataLoadStatus (Region, InsertDate)
	SELECT @region, @insertDate;
	PRINT ''PODataLoadStatus table has been updated for InsertDate: '' + CAST(@insertDate as nvarchar) + '' and Region: '' + @region + ''.'';

	COMMIT TRAN

END TRY
BEGIN CATCH
	IF @@TRANCOUNT > 0
    	ROLLBACK TRAN
	DECLARE @err_no int, @err_sev int, @err_msg varchar(MAX)
    SELECT @err_no = ERROR_NUMBER(), @err_sev = ERROR_SEVERITY(), @err_msg = ERROR_MESSAGE()
	RAISERROR (''GetWeeklyPOData failed with error no: %d and message: %s'', @err_sev, 1, @err_no, @err_msg)
END CATCH', 
		@database_name=N'POReports', 
		@flags=0
IF (@@ERROR <> 0 OR @ReturnCode <> 0) GOTO QuitWithRollback
/****** Object:  Step [MA_Update Approved Orders]    Script Date: 11/6/2013 3:51:59 PM ******/
EXEC @ReturnCode = msdb.dbo.sp_add_jobstep @job_id=@jobId, @step_name=N'MA_Update Approved Orders', 
		@step_id=4, 
		@cmdexec_success_code=0, 
		@on_success_action=3, 
		@on_success_step_id=0, 
		@on_fail_action=4, 
		@on_fail_step_id=27, 
		@retry_attempts=0, 
		@retry_interval=0, 
		@os_run_priority=0, @subsystem=N'TSQL', 
		@command=N'--**************************************************************************
-- Job Script: PO_Job_GetWeeklyPODataFromIRMA
-- Author: Ben Sims
-- Date: 08/19/2013
--
-- Description:
-- This script is called from the Suspended PO Tool Reporting Database SQL Agent
-- This is the TEST version which connects to IDT-XX\XXT or IDD-XX\XXD.dbo.ItemCatalog_Test
--
-- Modification History:
-- Date       	Init  	TFS   	Comment
-- 08/19/2013	BAS   	12061	Edited Initial SQL Script created by Bryce Bartley
--								which used to query OrderHeaderHistory
-- 08/29/2013	BAS		12061	Changed oh.AdjustedReceivedCost to CASE statement logic
--								and updated ResolutionCode logic
-- 09/18/2013	BAS		12061	Added synonyms for readability and to make it
--								non-environment specific.
--**************************************************************************
USE POReports;
GO

--**************************************************************************
-- Create the synonyms to the appropriate IRMA database
--**************************************************************************
if exists (select * from sysobjects where xtype = ''SN'' and name = ''OrderHeader'')		drop synonym OrderHeader;
if exists (select * from sysobjects where xtype = ''SN'' and name = ''OrderItem'')			drop synonym OrderItem;
if exists (select * from sysobjects where xtype = ''SN'' and name = ''Vendor'')				drop synonym Vendor;
if exists (select * from sysobjects where xtype = ''SN'' and name = ''Users'')				drop synonym Users;
if exists (select * from sysobjects where xtype = ''SN'' and name = ''Store'')				drop synonym Store;
if exists (select * from sysobjects where xtype = ''SN'' and name = ''SubTeam'')			drop synonym SubTeam;
if exists (select * from sysobjects where xtype = ''SN'' and name = ''ReasonCodeDetail'')	drop synonym ReasonCodeDetail;
if exists (select * from sysobjects where xtype = ''SN'' and name = ''Region'')				drop synonym Region;

if exists (select * from sys.servers where name = ''IDD-MA\MAD'')
	begin
		
        create synonym OrderHeader		for [IDD-MA\MAD].[ItemCatalog_Test].[dbo].OrderHeader;
		create synonym OrderItem		for [IDD-MA\MAD].[ItemCatalog_Test].[dbo].OrderItem;
        create synonym Vendor			for [IDD-MA\MAD].[ItemCatalog_Test].[dbo].Vendor;
		create synonym Users			for [IDD-MA\MAD].[ItemCatalog_Test].[dbo].Users;
		create synonym Store			for [IDD-MA\MAD].[ItemCatalog_Test].[dbo].Store;
		create synonym SubTeam			for [IDD-MA\MAD].[ItemCatalog_Test].[dbo].SubTeam;
		create synonym ReasonCodeDetail for [IDD-MA\MAD].[ItemCatalog_Test].[dbo].ReasonCodeDetail;
		create synonym Region			for [IDD-MA\MAD].[ItemCatalog_Test].[dbo].Region;
    end
else if exists (select * from sys.servers where name = ''IDQ-MA\MAQ'')
	begin
        create synonym OrderHeader		for [IDQ-MA\MAQ].[ItemCatalog].[dbo].OrderHeader;
		create synonym OrderItem		for [IDQ-MA\MAQ].[ItemCatalog].[dbo].OrderItem;
        create synonym Vendor			for [IDQ-MA\MAQ].[ItemCatalog].[dbo].Vendor;
		create synonym Users			for [IDQ-MA\MAQ].[ItemCatalog].[dbo].Users;
		create synonym Store			for [IDQ-MA\MAQ].[ItemCatalog].[dbo].Store;
		create synonym SubTeam			for [IDQ-MA\MAQ].[ItemCatalog].[dbo].SubTeam;
		create synonym ReasonCodeDetail for [IDQ-MA\MAQ].[ItemCatalog].[dbo].ReasonCodeDetail;
		create synonym Region			for [IDQ-MA\MAQ].[ItemCatalog].[dbo].Region;
	end    
else if exists (select * from sys.servers where name = ''IDP-MA\MAP'')
	begin
        create synonym OrderHeader		for [IDP-MA\MAP].[ItemCatalog].[dbo].OrderHeader;
		create synonym OrderItem		for [IDP-MA\MAP].[ItemCatalog].[dbo].OrderItem;
        create synonym Vendor			for [IDP-MA\MAP].[ItemCatalog].[dbo].Vendor;
		create synonym Users			for [IDP-MA\MAP].[ItemCatalog].[dbo].Users;
		create synonym Store			for [IDP-MA\MAP].[ItemCatalog].[dbo].Store;
		create synonym SubTeam			for [IDP-MA\MAP].[ItemCatalog].[dbo].SubTeam;
		create synonym ReasonCodeDetail for [IDP-MA\MAP].[ItemCatalog].[dbo].ReasonCodeDetail;
		create synonym Region			for [IDP-MA\MAP].[ItemCatalog].[dbo].Region;
	end

--**************************************************************************
-- Set and Populate internal variables
--**************************************************************************
DECLARE @startdate datetime;
DECLARE @enddate datetime;
DECLARE @region	varchar(2);

-- Get Monday 00:00:00 of Last Week
-- SELECT @startdate = DATEADD(day, -7 , DATEADD(wk, DATEDIFF(wk, 0, GETDATE()), 0))

-- for testing, get two weeks ago start
SELECT @startdate = DATEADD(day, -14 , DATEADD(wk, DATEDIFF(wk, 0, GETDATE()), 0))

-- Get Sunday 23:59:59.997 of Last Week
--SELECT @enddate = DATEADD(ms, -3, DATEADD(wk, DATEDIFF(wk, 0, GETDATE()), 0))

-- for testing, get two weeks ago end
SELECT @enddate = DATEADD(ms, -3, DATEADD(wk, DATEDIFF(wk, 0, GETDATE()-7), 0))

select @startdate, @enddate
SELECT  @region	= RegionCode FROM Region;

--**************************************************************************
-- Get list of orders that were approved during the previous week
-- This will be used for updating orders in POData
--**************************************************************************
SELECT
	oh.OrderHeader_ID				as PONumber,
	oh.ApprovedDate					as ApprovedDate,
	ISNULL(rcd.ReasonCodeDesc, '''')	as ResolutionCode,
	oh.InvoiceNumber				as InvoiceNumber
INTO #approvedOrders
FROM
	OrderHeader					oh	(nolock)
	LEFT JOIN ReasonCodeDetail	rcd (nolock) on oh.ResolutionCodeID	= rcd.ReasonCodeDetailID
WHERE
	oh.ApprovedDate		>= @startdate
	AND oh.ApprovedDate <= @enddate

create clustered index idx_c_ApprovedOrders_PONumber on #approvedOrders (PONumber)
create nonclustered index idx_ApprovedOrders_ResolutionCode on #approvedOrders (ResolutionCode)

--**************************************************************************
-- Update orders in POData that were ''Suspended but not Approved'' or 
-- orders that were closed as Other/None but are now approved
--**************************************************************************
-- Update orders with resolution code if there is one assigned for orders that have invoice info.
DECLARE @err_no int, @err_sev int, @err_msg varchar(MAX)

BEGIN TRY
	BEGIN TRAN
		UPDATE pod
		SET
			pod.ResolutionCode = ao.ResolutionCode,
			pod.ApprovedDate = ao.ApprovedDate
		FROM
			POData pod
			INNER JOIN #approvedOrders ao on pod.PONumber = ao.PONumber
		WHERE
			ao.ResolutionCode <> ''''
			AND ao.InvoiceNumber IS NOT NULL
			AND ao.ApprovedDate IS NOT NULL

	COMMIT TRAN
END TRY
BEGIN CATCH
	IF @@TRANCOUNT > 0
    	ROLLBACK TRAN
    SELECT @err_no = ERROR_NUMBER(), @err_sev = ERROR_SEVERITY(), @err_msg = ERROR_MESSAGE()
	RAISERROR (''Updating Newly Approved Orders with Resolution Code failed with error no: %d and message: %s'', @err_sev, 1, @err_no, @err_msg)
END CATCH

-- Update the suspended status for orders closed as Other/None that do not have resolution code but are now approved
BEGIN TRY
	BEGIN TRAN
		UPDATE pod
		SET
			pod.Suspended = ''N'',
			pod.ApprovedDate = ao.ApprovedDate
		FROM
			POData pod
			INNER JOIN #approvedOrders ao on pod.PONumber = ao.PONumber
		WHERE
			ao.ResolutionCode = ''''
			AND ao.ApprovedDate IS NOT NULL
			AND ao.InvoiceNumber IS NOT NULL

		COMMIT TRAN
END TRY
BEGIN CATCH
	IF @@TRANCOUNT > 0
    	ROLLBACK TRAN
    SELECT @err_no = ERROR_NUMBER(), @err_sev = ERROR_SEVERITY(), @err_msg = ERROR_MESSAGE()
	RAISERROR (''Updating the Suspended Status of orders that were closed as Other/None failed with error no: %d and message: %s'', @err_sev, 1, @err_no, @err_msg)
END CATCH', 
		@database_name=N'POReports', 
		@flags=0
IF (@@ERROR <> 0 OR @ReturnCode <> 0) GOTO QuitWithRollback
/****** Object:  Step [MW_Get Weekly Orders By Close Date]    Script Date: 11/6/2013 3:51:59 PM ******/
EXEC @ReturnCode = msdb.dbo.sp_add_jobstep @job_id=@jobId, @step_name=N'MW_Get Weekly Orders By Close Date', 
		@step_id=5, 
		@cmdexec_success_code=0, 
		@on_success_action=3, 
		@on_success_step_id=0, 
		@on_fail_action=4, 
		@on_fail_step_id=27, 
		@retry_attempts=0, 
		@retry_interval=0, 
		@os_run_priority=0, @subsystem=N'TSQL', 
		@command=N'-- MW
USE POReports;
GO

--**************************************************************************
-- Create the synonyms to the appropriate IRMA database
--**************************************************************************
if exists (select * from sysobjects where xtype = ''SN'' and name = ''OrderHeader'')		drop synonym OrderHeader;
if exists (select * from sysobjects where xtype = ''SN'' and name = ''OrderItem'')			drop synonym OrderItem;
if exists (select * from sysobjects where xtype = ''SN'' and name = ''Vendor'')				drop synonym Vendor;
if exists (select * from sysobjects where xtype = ''SN'' and name = ''Users'')				drop synonym Users;
if exists (select * from sysobjects where xtype = ''SN'' and name = ''Store'')				drop synonym Store;
if exists (select * from sysobjects where xtype = ''SN'' and name = ''SubTeam'')			drop synonym SubTeam;
if exists (select * from sysobjects where xtype = ''SN'' and name = ''ReasonCodeDetail'')	drop synonym ReasonCodeDetail;
if exists (select * from sysobjects where xtype = ''SN'' and name = ''Region'')				drop synonym Region;

if exists (select * from sys.servers where name = ''IDD-MW\MWD'')
	begin
		
        create synonym OrderHeader		for [IDD-MW\MWD].[ItemCatalog_Test].[dbo].OrderHeader;
		create synonym OrderItem		for [IDD-MW\MWD].[ItemCatalog_Test].[dbo].OrderItem;
        create synonym Vendor			for [IDD-MW\MWD].[ItemCatalog_Test].[dbo].Vendor;
		create synonym Users			for [IDD-MW\MWD].[ItemCatalog_Test].[dbo].Users;
		create synonym Store			for [IDD-MW\MWD].[ItemCatalog_Test].[dbo].Store;
		create synonym SubTeam			for [IDD-MW\MWD].[ItemCatalog_Test].[dbo].SubTeam;
		create synonym ReasonCodeDetail for [IDD-MW\MWD].[ItemCatalog_Test].[dbo].ReasonCodeDetail;
		create synonym Region			for [IDD-MW\MWD].[ItemCatalog_Test].[dbo].Region;
    end
else if exists (select * from sys.servers where name = ''IDQ-MW\MWQ'')
	begin
        create synonym OrderHeader		for [IDQ-MW\MWQ].[ItemCatalog].[dbo].OrderHeader;
		create synonym OrderItem		for [IDQ-MW\MWQ].[ItemCatalog].[dbo].OrderItem;
        create synonym Vendor			for [IDQ-MW\MWQ].[ItemCatalog].[dbo].Vendor;
		create synonym Users			for [IDQ-MW\MWQ].[ItemCatalog].[dbo].Users;
		create synonym Store			for [IDQ-MW\MWQ].[ItemCatalog].[dbo].Store;
		create synonym SubTeam			for [IDQ-MW\MWQ].[ItemCatalog].[dbo].SubTeam;
		create synonym ReasonCodeDetail for [IDQ-MW\MWQ].[ItemCatalog].[dbo].ReasonCodeDetail;
		create synonym Region			for [IDQ-MW\MWQ].[ItemCatalog].[dbo].Region;
	end    
else if exists (select * from sys.servers where name = ''IDP-MW\MWP'')
	begin
        create synonym OrderHeader		for [IDP-MW\MWP].[ItemCatalog].[dbo].OrderHeader;
		create synonym OrderItem		for [IDP-MW\MWP].[ItemCatalog].[dbo].OrderItem;
        create synonym Vendor			for [IDP-MW\MWP].[ItemCatalog].[dbo].Vendor;
		create synonym Users			for [IDP-MW\MWP].[ItemCatalog].[dbo].Users;
		create synonym Store			for [IDP-MW\MWP].[ItemCatalog].[dbo].Store;
		create synonym SubTeam			for [IDP-MW\MWP].[ItemCatalog].[dbo].SubTeam;
		create synonym ReasonCodeDetail for [IDP-MW\MWP].[ItemCatalog].[dbo].ReasonCodeDetail;
		create synonym Region			for [IDP-MW\MWP].[ItemCatalog].[dbo].Region;
	end

--**************************************************************************
-- Set and Populate internal variables
--**************************************************************************
DECLARE @startdate datetime;
DECLARE @enddate datetime;
DECLARE @region	varchar(2);
DECLARE @insertDate	datetime2;

-- Get Monday 00:00:00 of Last Week and Get Sunday 23:59:59.997 of Last Week
SELECT @startdate = DATEADD(day, -7 , DATEADD(wk, DATEDIFF(wk, 0, GETDATE()), 0))
SELECT @enddate = DATEADD(ms, -3, DATEADD(wk, DATEDIFF(wk, 0, GETDATE()), 0))

SELECT  @region			= RegionCode FROM Region;
SELECT	@insertDate		= GETDATE();

--**************************************************************************
-- Get List of Orders and Insert Into PODataLoad table
--**************************************************************************
BEGIN TRY
BEGIN TRAN

	-- Insert into PODataLoad table
	INSERT INTO PODataLoad
	SELECT 
		oh.OrderHeader_ID											as PONumber,
		CASE 
			WHEN oh.ResolutionCodeID IS NOT NULL AND oh.ApprovedDate IS NOT NULL THEN ''Y'' 
			WHEN oh.ApprovedDate IS NULL THEN ''Y''
			ELSE ''N'' 
		END															as Suspended,
		oh.CloseDate												as CloseDate,
		CASE
			WHEN oh.ResolutionCodeID IS NOT NULL AND oh.ApprovedDate IS NOT NULL THEN rcd.ReasonCodeDesc
			WHEN oh.ApprovedDate IS NULL AND oh.InvoiceNumber IS NOT NULL THEN ''Suspended but not Approved''
			WHEN oh.ApprovedDate IS NULL AND oh.InvoiceNumber IS NULL THEN ''Closed as Other/None''
			ELSE ''''
		END															as ResolutionCode,
		ISNULL(oh.AdminNotes, '''')									as AdminNotes,
		v.CompanyName												as Vendor,
		st.SubTeam_No												as Subteam,
		s.Store_Name												as Store,
		CASE 
			WHEN oh.DiscountType > 0 THEN ''Y''
			WHEN 0 < SUM(oi.DiscountType) Then ''Y''
			WHEN 0 < SUM(oi.AdjustedCost) Then ''Y''
			ELSE ''N''
		END															as AdjustedCost,
		CASE 
			WHEN oh.Return_Order = 1 THEN ''Y'' 
			ELSE ''N'' 
		END															as CreditPO,
		CASE 
			WHEN oh.PayByAgreedCost = 1 THEN ''Pay By Agreed Cost'' 
			ELSE ''Pay By Invoice'' 
		END															as VendorType,
		ucr.FullName												as POCreator,
		CASE 
			WHEN oh.Einvoice_id IS NOT NULL THEN ''Y'' 
			ELSE ''N'' 
		END															as EInvoiceMatchedToPO,
		ISNULL(oh.OrderHeaderDesc, '''')								as PONotes,
		ucl.FullName												as ClosedBy,
		@region														as Region,
		oh.ApprovedDate												as ApprovedDate,
		oh.InvoiceNumber											as InvoiceNumber,
		@insertDate													as InsertDate
	FROM
		OrderHeader					oh	(nolock)
		INNER JOIN OrderItem		oi	(nolock) on oh.OrderHeader_ID		= oi.OrderHeader_ID
		INNER JOIN Vendor			vr	(nolock) on oh.ReceiveLocation_ID	= vr.Vendor_ID
		INNER JOIN Vendor			v	(nolock) on oh.Vendor_ID			= v.Vendor_ID
		INNER JOIN Users			ucr (nolock) on oh.CreatedBy			= ucr.User_ID
		INNER JOIN Users			ucl	(nolock) on oh.ClosedBy				= ucl.User_ID
		INNER JOIN Store			s	(nolock) on vr.Store_No				= s.Store_No
		INNER JOIN SubTeam			st	(nolock) on oh.Transfer_To_SubTeam	= st.SubTeam_No
		LEFT JOIN ReasonCodeDetail	rcd (nolock) on oh.ResolutionCodeID		= rcd.ReasonCodeDetailID
	WHERE
		oh.CloseDate		>= @StartDate
		AND oh.CloseDate	<= @EndDate
		AND oh.OrderType_ID <> 3 -- filter out Transfer orders
	GROUP BY
		oh.OrderHeader_ID,
		oh.DiscountType,
		oh.Return_Order,
		oh.PayByAgreedCost,
		oh.eInvoice_Id,
		oh.ResolutionCodeID,
		oh.Vendor_ID,
		oh.ReceiveLocation_ID,
		oh.CreatedBy,
		oh.ClosedBy,
		oh.Transfer_To_SubTeam,
		oh.AdminNotes,
		oh.OrderHeaderDesc,
		oh.CloseDate,
		oh.ApprovedDate,
		oh.InvoiceNumber,
		rcd.ReasonCodeDesc,
		v.CompanyName,
		st.SubTeam_No,
		s.Store_Name,
		ucr.FullName,
		ucl.FullName

	PRINT ''Data has been loaded into PODataLoad table for the '' + @region + '' region.''
	
	-- Insert data into PODataLoadStatus
	INSERT INTO dbo.PODataLoadStatus (Region, InsertDate)
	SELECT @region, @insertDate;
	PRINT ''PODataLoadStatus table has been updated for InsertDate: '' + CAST(@insertDate as nvarchar) + '' and Region: '' + @region + ''.'';

	COMMIT TRAN

END TRY
BEGIN CATCH
	IF @@TRANCOUNT > 0
    	ROLLBACK TRAN
	DECLARE @err_no int, @err_sev int, @err_msg varchar(MAX)
    SELECT @err_no = ERROR_NUMBER(), @err_sev = ERROR_SEVERITY(), @err_msg = ERROR_MESSAGE()
	RAISERROR (''GetWeeklyPOData failed with error no: %d and message: %s'', @err_sev, 1, @err_no, @err_msg)
END CATCH', 
		@database_name=N'POReports', 
		@flags=0
IF (@@ERROR <> 0 OR @ReturnCode <> 0) GOTO QuitWithRollback
/****** Object:  Step [MW_Update Approved Orders]    Script Date: 11/6/2013 3:51:59 PM ******/
EXEC @ReturnCode = msdb.dbo.sp_add_jobstep @job_id=@jobId, @step_name=N'MW_Update Approved Orders', 
		@step_id=6, 
		@cmdexec_success_code=0, 
		@on_success_action=3, 
		@on_success_step_id=0, 
		@on_fail_action=4, 
		@on_fail_step_id=27, 
		@retry_attempts=0, 
		@retry_interval=0, 
		@os_run_priority=0, @subsystem=N'TSQL', 
		@command=N'-- MW Update Approved Orders

USE POReports;
GO

--**************************************************************************
-- Create the synonyms to the appropriate IRMA database
--**************************************************************************
if exists (select * from sysobjects where xtype = ''SN'' and name = ''OrderHeader'')		drop synonym OrderHeader;
if exists (select * from sysobjects where xtype = ''SN'' and name = ''OrderItem'')			drop synonym OrderItem;
if exists (select * from sysobjects where xtype = ''SN'' and name = ''Vendor'')				drop synonym Vendor;
if exists (select * from sysobjects where xtype = ''SN'' and name = ''Users'')				drop synonym Users;
if exists (select * from sysobjects where xtype = ''SN'' and name = ''Store'')				drop synonym Store;
if exists (select * from sysobjects where xtype = ''SN'' and name = ''SubTeam'')			drop synonym SubTeam;
if exists (select * from sysobjects where xtype = ''SN'' and name = ''ReasonCodeDetail'')	drop synonym ReasonCodeDetail;
if exists (select * from sysobjects where xtype = ''SN'' and name = ''Region'')				drop synonym Region;

if exists (select * from sys.servers where name = ''IDD-MW\MWD'')
	begin
		
        create synonym OrderHeader		for [IDD-MW\MWD].[ItemCatalog_Test].[dbo].OrderHeader;
		create synonym OrderItem		for [IDD-MW\MWD].[ItemCatalog_Test].[dbo].OrderItem;
        create synonym Vendor			for [IDD-MW\MWD].[ItemCatalog_Test].[dbo].Vendor;
		create synonym Users			for [IDD-MW\MWD].[ItemCatalog_Test].[dbo].Users;
		create synonym Store			for [IDD-MW\MWD].[ItemCatalog_Test].[dbo].Store;
		create synonym SubTeam			for [IDD-MW\MWD].[ItemCatalog_Test].[dbo].SubTeam;
		create synonym ReasonCodeDetail for [IDD-MW\MWD].[ItemCatalog_Test].[dbo].ReasonCodeDetail;
		create synonym Region			for [IDD-MW\MWD].[ItemCatalog_Test].[dbo].Region;
    end
else if exists (select * from sys.servers where name = ''IDQ-MW\MWQ'')
	begin
        create synonym OrderHeader		for [IDQ-MW\MWQ].[ItemCatalog].[dbo].OrderHeader;
		create synonym OrderItem		for [IDQ-MW\MWQ].[ItemCatalog].[dbo].OrderItem;
        create synonym Vendor			for [IDQ-MW\MWQ].[ItemCatalog].[dbo].Vendor;
		create synonym Users			for [IDQ-MW\MWQ].[ItemCatalog].[dbo].Users;
		create synonym Store			for [IDQ-MW\MWQ].[ItemCatalog].[dbo].Store;
		create synonym SubTeam			for [IDQ-MW\MWQ].[ItemCatalog].[dbo].SubTeam;
		create synonym ReasonCodeDetail for [IDQ-MW\MWQ].[ItemCatalog].[dbo].ReasonCodeDetail;
		create synonym Region			for [IDQ-MW\MWQ].[ItemCatalog].[dbo].Region;
	end    
else if exists (select * from sys.servers where name = ''IDP-MW\MWP'')
	begin
        create synonym OrderHeader		for [IDP-MW\MWP].[ItemCatalog].[dbo].OrderHeader;
		create synonym OrderItem		for [IDP-MW\MWP].[ItemCatalog].[dbo].OrderItem;
        create synonym Vendor			for [IDP-MW\MWP].[ItemCatalog].[dbo].Vendor;
		create synonym Users			for [IDP-MW\MWP].[ItemCatalog].[dbo].Users;
		create synonym Store			for [IDP-MW\MWP].[ItemCatalog].[dbo].Store;
		create synonym SubTeam			for [IDP-MW\MWP].[ItemCatalog].[dbo].SubTeam;
		create synonym ReasonCodeDetail for [IDP-MW\MWP].[ItemCatalog].[dbo].ReasonCodeDetail;
		create synonym Region			for [IDP-MW\MWP].[ItemCatalog].[dbo].Region;
	end

--**************************************************************************
-- Set and Populate internal variables
--**************************************************************************
DECLARE @startdate datetime;
DECLARE @enddate datetime;
DECLARE @region	varchar(2);

-- Get Monday 00:00:00 of Last Week
SELECT @startdate = DATEADD(day, -7 , DATEADD(wk, DATEDIFF(wk, 0, GETDATE()), 0))

-- Get Sunday 23:59:59.997 of Last Week
SELECT @enddate = DATEADD(ms, -3, DATEADD(wk, DATEDIFF(wk, 0, GETDATE()), 0))

select @startdate, @enddate
SELECT  @region	= RegionCode FROM Region;

--**************************************************************************
-- Get list of orders that were approved during the previous week
-- This will be used for updating orders in POData
--**************************************************************************
SELECT
	oh.OrderHeader_ID				as PONumber,
	oh.ApprovedDate					as ApprovedDate,
	ISNULL(rcd.ReasonCodeDesc, '''')	as ResolutionCode,
	oh.InvoiceNumber				as InvoiceNumber
INTO #approvedOrders
FROM
	OrderHeader					oh	(nolock)
	LEFT JOIN ReasonCodeDetail	rcd (nolock) on oh.ResolutionCodeID	= rcd.ReasonCodeDetailID
WHERE
	oh.ApprovedDate		>= @startdate
	AND oh.ApprovedDate <= @enddate

create clustered index idx_c_ApprovedOrders_PONumber on #approvedOrders (PONumber)
create nonclustered index idx_ApprovedOrders_ResolutionCode on #approvedOrders (ResolutionCode)

--**************************************************************************
-- Update orders in POData that were ''Suspended but not Approved'' or 
-- orders that were closed as Other/None but are now approved
--**************************************************************************
-- Update orders with resolution code if there is one assigned for orders that have invoice info.
DECLARE @err_no int, @err_sev int, @err_msg varchar(MAX)

BEGIN TRY
	BEGIN TRAN
		UPDATE pod
		SET
			pod.ResolutionCode = ao.ResolutionCode,
			pod.ApprovedDate = ao.ApprovedDate
		FROM
			POData pod
			INNER JOIN #approvedOrders ao on pod.PONumber = ao.PONumber
		WHERE
			ao.ResolutionCode <> ''''
			AND ao.InvoiceNumber IS NOT NULL
			AND ao.ApprovedDate IS NOT NULL

	COMMIT TRAN
END TRY
BEGIN CATCH
	IF @@TRANCOUNT > 0
    	ROLLBACK TRAN
    SELECT @err_no = ERROR_NUMBER(), @err_sev = ERROR_SEVERITY(), @err_msg = ERROR_MESSAGE()
	RAISERROR (''Updating Newly Approved Orders with Resolution Code failed with error no: %d and message: %s'', @err_sev, 1, @err_no, @err_msg)
END CATCH

-- Update the suspended status for orders closed as Other/None that do not have resolution code but are now approved
BEGIN TRY
	BEGIN TRAN
		UPDATE pod
		SET
			pod.Suspended = ''N'',
			pod.ApprovedDate = ao.ApprovedDate
		FROM
			POData pod
			INNER JOIN #approvedOrders ao on pod.PONumber = ao.PONumber
		WHERE
			ao.ResolutionCode = ''''
			AND ao.ApprovedDate IS NOT NULL
			AND ao.InvoiceNumber IS NOT NULL

		COMMIT TRAN
END TRY
BEGIN CATCH
	IF @@TRANCOUNT > 0
    	ROLLBACK TRAN
    SELECT @err_no = ERROR_NUMBER(), @err_sev = ERROR_SEVERITY(), @err_msg = ERROR_MESSAGE()
	RAISERROR (''Updating the Suspended Status of orders that were closed as Other/None failed with error no: %d and message: %s'', @err_sev, 1, @err_no, @err_msg)
END CATCH', 
		@database_name=N'POReports', 
		@flags=0
IF (@@ERROR <> 0 OR @ReturnCode <> 0) GOTO QuitWithRollback
/****** Object:  Step [NA_Get Weekly Orders By Close Date]    Script Date: 11/6/2013 3:51:59 PM ******/
EXEC @ReturnCode = msdb.dbo.sp_add_jobstep @job_id=@jobId, @step_name=N'NA_Get Weekly Orders By Close Date', 
		@step_id=7, 
		@cmdexec_success_code=0, 
		@on_success_action=3, 
		@on_success_step_id=0, 
		@on_fail_action=4, 
		@on_fail_step_id=27, 
		@retry_attempts=0, 
		@retry_interval=0, 
		@os_run_priority=0, @subsystem=N'TSQL', 
		@command=N'-- NA
USE POReports;
GO

--**************************************************************************
-- Create the synonyms to the appropriate IRMA database
--**************************************************************************
if exists (select * from sysobjects where xtype = ''SN'' and name = ''OrderHeader'')		drop synonym OrderHeader;
if exists (select * from sysobjects where xtype = ''SN'' and name = ''OrderItem'')			drop synonym OrderItem;
if exists (select * from sysobjects where xtype = ''SN'' and name = ''Vendor'')				drop synonym Vendor;
if exists (select * from sysobjects where xtype = ''SN'' and name = ''Users'')				drop synonym Users;
if exists (select * from sysobjects where xtype = ''SN'' and name = ''Store'')				drop synonym Store;
if exists (select * from sysobjects where xtype = ''SN'' and name = ''SubTeam'')			drop synonym SubTeam;
if exists (select * from sysobjects where xtype = ''SN'' and name = ''ReasonCodeDetail'')	drop synonym ReasonCodeDetail;
if exists (select * from sysobjects where xtype = ''SN'' and name = ''Region'')				drop synonym Region;

if exists (select * from sys.servers where name = ''IDD-NA\NAD'')
	begin
		
        create synonym OrderHeader		for [IDD-NA\NAD].[ItemCatalog_Test].[dbo].OrderHeader;
		create synonym OrderItem		for [IDD-NA\NAD].[ItemCatalog_Test].[dbo].OrderItem;
        create synonym Vendor			for [IDD-NA\NAD].[ItemCatalog_Test].[dbo].Vendor;
		create synonym Users			for [IDD-NA\NAD].[ItemCatalog_Test].[dbo].Users;
		create synonym Store			for [IDD-NA\NAD].[ItemCatalog_Test].[dbo].Store;
		create synonym SubTeam			for [IDD-NA\NAD].[ItemCatalog_Test].[dbo].SubTeam;
		create synonym ReasonCodeDetail for [IDD-NA\NAD].[ItemCatalog_Test].[dbo].ReasonCodeDetail;
		create synonym Region			for [IDD-NA\NAD].[ItemCatalog_Test].[dbo].Region;
    end
else if exists (select * from sys.servers where name = ''IDQ-NA\NAQ'')
	begin
        create synonym OrderHeader		for [IDQ-NA\NAQ].[ItemCatalog].[dbo].OrderHeader;
		create synonym OrderItem		for [IDQ-NA\NAQ].[ItemCatalog].[dbo].OrderItem;
        create synonym Vendor			for [IDQ-NA\NAQ].[ItemCatalog].[dbo].Vendor;
		create synonym Users			for [IDQ-NA\NAQ].[ItemCatalog].[dbo].Users;
		create synonym Store			for [IDQ-NA\NAQ].[ItemCatalog].[dbo].Store;
		create synonym SubTeam			for [IDQ-NA\NAQ].[ItemCatalog].[dbo].SubTeam;
		create synonym ReasonCodeDetail for [IDQ-NA\NAQ].[ItemCatalog].[dbo].ReasonCodeDetail;
		create synonym Region			for [IDQ-NA\NAQ].[ItemCatalog].[dbo].Region;
	end    
else if exists (select * from sys.servers where name = ''IDP-NA\NAP'')
	begin
        create synonym OrderHeader		for [IDP-NA\NAP].[ItemCatalog].[dbo].OrderHeader;
		create synonym OrderItem		for [IDP-NA\NAP].[ItemCatalog].[dbo].OrderItem;
        create synonym Vendor			for [IDP-NA\NAP].[ItemCatalog].[dbo].Vendor;
		create synonym Users			for [IDP-NA\NAP].[ItemCatalog].[dbo].Users;
		create synonym Store			for [IDP-NA\NAP].[ItemCatalog].[dbo].Store;
		create synonym SubTeam			for [IDP-NA\NAP].[ItemCatalog].[dbo].SubTeam;
		create synonym ReasonCodeDetail for [IDP-NA\NAP].[ItemCatalog].[dbo].ReasonCodeDetail;
		create synonym Region			for [IDP-NA\NAP].[ItemCatalog].[dbo].Region;
	end

--**************************************************************************
-- Set and Populate internal variables
--**************************************************************************
DECLARE @startdate datetime;
DECLARE @enddate datetime;
DECLARE @region	varchar(2);
DECLARE @insertDate	datetime2;

-- Get Monday 00:00:00 of Last Week and Get Sunday 23:59:59.997 of Last Week
SELECT @startdate = DATEADD(day, -7 , DATEADD(wk, DATEDIFF(wk, 0, GETDATE()), 0))
SELECT @enddate = DATEADD(ms, -3, DATEADD(wk, DATEDIFF(wk, 0, GETDATE()), 0))

SELECT  @region			= RegionCode FROM Region;
SELECT	@insertDate		= GETDATE();

--**************************************************************************
-- Get List of Orders and Insert Into PODataLoad table
--**************************************************************************
BEGIN TRY
BEGIN TRAN

	-- Insert into PODataLoad table
	INSERT INTO PODataLoad
	SELECT 
		oh.OrderHeader_ID											as PONumber,
		CASE 
			WHEN oh.ResolutionCodeID IS NOT NULL AND oh.ApprovedDate IS NOT NULL THEN ''Y'' 
			WHEN oh.ApprovedDate IS NULL THEN ''Y''
			ELSE ''N'' 
		END															as Suspended,
		oh.CloseDate												as CloseDate,
		CASE
			WHEN oh.ResolutionCodeID IS NOT NULL AND oh.ApprovedDate IS NOT NULL THEN rcd.ReasonCodeDesc
			WHEN oh.ApprovedDate IS NULL AND oh.InvoiceNumber IS NOT NULL THEN ''Suspended but not Approved''
			WHEN oh.ApprovedDate IS NULL AND oh.InvoiceNumber IS NULL THEN ''Closed as Other/None''
			ELSE ''''
		END															as ResolutionCode,
		ISNULL(oh.AdminNotes, '''')									as AdminNotes,
		v.CompanyName												as Vendor,
		st.SubTeam_No												as Subteam,
		s.Store_Name												as Store,
		CASE 
			WHEN oh.DiscountType > 0 THEN ''Y''
			WHEN 0 < SUM(oi.DiscountType) Then ''Y''
			WHEN 0 < SUM(oi.AdjustedCost) Then ''Y''
			ELSE ''N''
		END															as AdjustedCost,
		CASE 
			WHEN oh.Return_Order = 1 THEN ''Y'' 
			ELSE ''N'' 
		END															as CreditPO,
		CASE 
			WHEN oh.PayByAgreedCost = 1 THEN ''Pay By Agreed Cost'' 
			ELSE ''Pay By Invoice'' 
		END															as VendorType,
		ucr.FullName												as POCreator,
		CASE 
			WHEN oh.Einvoice_id IS NOT NULL THEN ''Y'' 
			ELSE ''N'' 
		END															as EInvoiceMatchedToPO,
		ISNULL(oh.OrderHeaderDesc, '''')								as PONotes,
		ucl.FullName												as ClosedBy,
		@region														as Region,
		oh.ApprovedDate												as ApprovedDate,
		oh.InvoiceNumber											as InvoiceNumber,
		@insertDate													as InsertDate
	FROM
		OrderHeader					oh	(nolock)
		INNER JOIN OrderItem		oi	(nolock) on oh.OrderHeader_ID		= oi.OrderHeader_ID
		INNER JOIN Vendor			vr	(nolock) on oh.ReceiveLocation_ID	= vr.Vendor_ID
		INNER JOIN Vendor			v	(nolock) on oh.Vendor_ID			= v.Vendor_ID
		INNER JOIN Users			ucr (nolock) on oh.CreatedBy			= ucr.User_ID
		INNER JOIN Users			ucl	(nolock) on oh.ClosedBy				= ucl.User_ID
		INNER JOIN Store			s	(nolock) on vr.Store_No				= s.Store_No
		INNER JOIN SubTeam			st	(nolock) on oh.Transfer_To_SubTeam	= st.SubTeam_No
		LEFT JOIN ReasonCodeDetail	rcd (nolock) on oh.ResolutionCodeID		= rcd.ReasonCodeDetailID
	WHERE
		oh.CloseDate		>= @StartDate
		AND oh.CloseDate	<= @EndDate
		AND oh.OrderType_ID <> 3 -- filter out Transfer orders
	GROUP BY
		oh.OrderHeader_ID,
		oh.DiscountType,
		oh.Return_Order,
		oh.PayByAgreedCost,
		oh.eInvoice_Id,
		oh.ResolutionCodeID,
		oh.Vendor_ID,
		oh.ReceiveLocation_ID,
		oh.CreatedBy,
		oh.ClosedBy,
		oh.Transfer_To_SubTeam,
		oh.AdminNotes,
		oh.OrderHeaderDesc,
		oh.CloseDate,
		oh.ApprovedDate,
		oh.InvoiceNumber,
		rcd.ReasonCodeDesc,
		v.CompanyName,
		st.SubTeam_No,
		s.Store_Name,
		ucr.FullName,
		ucl.FullName

	PRINT ''Data has been loaded into PODataLoad table for the '' + @region + '' region.''
	
	-- Insert data into PODataLoadStatus
	INSERT INTO dbo.PODataLoadStatus (Region, InsertDate)
	SELECT @region, @insertDate;
	PRINT ''PODataLoadStatus table has been updated for InsertDate: '' + CAST(@insertDate as nvarchar) + '' and Region: '' + @region + ''.'';

	COMMIT TRAN

END TRY
BEGIN CATCH
	IF @@TRANCOUNT > 0
    	ROLLBACK TRAN
	DECLARE @err_no int, @err_sev int, @err_msg varchar(MAX)
    SELECT @err_no = ERROR_NUMBER(), @err_sev = ERROR_SEVERITY(), @err_msg = ERROR_MESSAGE()
	RAISERROR (''GetWeeklyPOData failed with error no: %d and message: %s'', @err_sev, 1, @err_no, @err_msg)
END CATCH', 
		@database_name=N'POReports', 
		@flags=0
IF (@@ERROR <> 0 OR @ReturnCode <> 0) GOTO QuitWithRollback
/****** Object:  Step [NA_Update Approved Orders]    Script Date: 11/6/2013 3:51:59 PM ******/
EXEC @ReturnCode = msdb.dbo.sp_add_jobstep @job_id=@jobId, @step_name=N'NA_Update Approved Orders', 
		@step_id=8, 
		@cmdexec_success_code=0, 
		@on_success_action=3, 
		@on_success_step_id=0, 
		@on_fail_action=4, 
		@on_fail_step_id=27, 
		@retry_attempts=0, 
		@retry_interval=0, 
		@os_run_priority=0, @subsystem=N'TSQL', 
		@command=N'-- NA
-- Update Approved Orders

USE POReports;
GO

--**************************************************************************
-- Create the synonyms to the appropriate IRMA database
--**************************************************************************
if exists (select * from sysobjects where xtype = ''SN'' and name = ''OrderHeader'')		drop synonym OrderHeader;
if exists (select * from sysobjects where xtype = ''SN'' and name = ''OrderItem'')			drop synonym OrderItem;
if exists (select * from sysobjects where xtype = ''SN'' and name = ''Vendor'')				drop synonym Vendor;
if exists (select * from sysobjects where xtype = ''SN'' and name = ''Users'')				drop synonym Users;
if exists (select * from sysobjects where xtype = ''SN'' and name = ''Store'')				drop synonym Store;
if exists (select * from sysobjects where xtype = ''SN'' and name = ''SubTeam'')			drop synonym SubTeam;
if exists (select * from sysobjects where xtype = ''SN'' and name = ''ReasonCodeDetail'')	drop synonym ReasonCodeDetail;
if exists (select * from sysobjects where xtype = ''SN'' and name = ''Region'')				drop synonym Region;

if exists (select * from sys.servers where name = ''IDD-NA\NAD'')
	begin
		
        create synonym OrderHeader		for [IDD-NA\NAD].[ItemCatalog_Test].[dbo].OrderHeader;
		create synonym OrderItem		for [IDD-NA\NAD].[ItemCatalog_Test].[dbo].OrderItem;
        create synonym Vendor			for [IDD-NA\NAD].[ItemCatalog_Test].[dbo].Vendor;
		create synonym Users			for [IDD-NA\NAD].[ItemCatalog_Test].[dbo].Users;
		create synonym Store			for [IDD-NA\NAD].[ItemCatalog_Test].[dbo].Store;
		create synonym SubTeam			for [IDD-NA\NAD].[ItemCatalog_Test].[dbo].SubTeam;
		create synonym ReasonCodeDetail for [IDD-NA\NAD].[ItemCatalog_Test].[dbo].ReasonCodeDetail;
		create synonym Region			for [IDD-NA\NAD].[ItemCatalog_Test].[dbo].Region;
    end
else if exists (select * from sys.servers where name = ''IDQ-NA\NAQ'')
	begin
        create synonym OrderHeader		for [IDQ-NA\NAQ].[ItemCatalog].[dbo].OrderHeader;
		create synonym OrderItem		for [IDQ-NA\NAQ].[ItemCatalog].[dbo].OrderItem;
        create synonym Vendor			for [IDQ-NA\NAQ].[ItemCatalog].[dbo].Vendor;
		create synonym Users			for [IDQ-NA\NAQ].[ItemCatalog].[dbo].Users;
		create synonym Store			for [IDQ-NA\NAQ].[ItemCatalog].[dbo].Store;
		create synonym SubTeam			for [IDQ-NA\NAQ].[ItemCatalog].[dbo].SubTeam;
		create synonym ReasonCodeDetail for [IDQ-NA\NAQ].[ItemCatalog].[dbo].ReasonCodeDetail;
		create synonym Region			for [IDQ-NA\NAQ].[ItemCatalog].[dbo].Region;
	end    
else if exists (select * from sys.servers where name = ''IDP-NA\NAP'')
	begin
        create synonym OrderHeader		for [IDP-NA\NAP].[ItemCatalog].[dbo].OrderHeader;
		create synonym OrderItem		for [IDP-NA\NAP].[ItemCatalog].[dbo].OrderItem;
        create synonym Vendor			for [IDP-NA\NAP].[ItemCatalog].[dbo].Vendor;
		create synonym Users			for [IDP-NA\NAP].[ItemCatalog].[dbo].Users;
		create synonym Store			for [IDP-NA\NAP].[ItemCatalog].[dbo].Store;
		create synonym SubTeam			for [IDP-NA\NAP].[ItemCatalog].[dbo].SubTeam;
		create synonym ReasonCodeDetail for [IDP-NA\NAP].[ItemCatalog].[dbo].ReasonCodeDetail;
		create synonym Region			for [IDP-NA\NAP].[ItemCatalog].[dbo].Region;
	end

--**************************************************************************
-- Set and Populate internal variables
--**************************************************************************
DECLARE @startdate datetime;
DECLARE @enddate datetime;
DECLARE @region	varchar(2);

-- Get Monday 00:00:00 of Last Week
SELECT @startdate = DATEADD(day, -7 , DATEADD(wk, DATEDIFF(wk, 0, GETDATE()), 0))

-- Get Sunday 23:59:59.997 of Last Week
SELECT @enddate = DATEADD(ms, -3, DATEADD(wk, DATEDIFF(wk, 0, GETDATE()), 0))

select @startdate, @enddate
SELECT  @region	= RegionCode FROM Region;

--**************************************************************************
-- Get list of orders that were approved during the previous week
-- This will be used for updating orders in POData
--**************************************************************************
SELECT
	oh.OrderHeader_ID				as PONumber,
	oh.ApprovedDate					as ApprovedDate,
	ISNULL(rcd.ReasonCodeDesc, '''')	as ResolutionCode,
	oh.InvoiceNumber				as InvoiceNumber
INTO #approvedOrders
FROM
	OrderHeader					oh	(nolock)
	LEFT JOIN ReasonCodeDetail	rcd (nolock) on oh.ResolutionCodeID	= rcd.ReasonCodeDetailID
WHERE
	oh.ApprovedDate		>= @startdate
	AND oh.ApprovedDate <= @enddate

create clustered index idx_c_ApprovedOrders_PONumber on #approvedOrders (PONumber)
create nonclustered index idx_ApprovedOrders_ResolutionCode on #approvedOrders (ResolutionCode)

--**************************************************************************
-- Update orders in POData that were ''Suspended but not Approved'' or 
-- orders that were closed as Other/None but are now approved
--**************************************************************************
-- Update orders with resolution code if there is one assigned for orders that have invoice info.
DECLARE @err_no int, @err_sev int, @err_msg varchar(MAX)

BEGIN TRY
	BEGIN TRAN
		UPDATE pod
		SET
			pod.ResolutionCode = ao.ResolutionCode,
			pod.ApprovedDate = ao.ApprovedDate
		FROM
			POData pod
			INNER JOIN #approvedOrders ao on pod.PONumber = ao.PONumber
		WHERE
			ao.ResolutionCode <> ''''
			AND ao.InvoiceNumber IS NOT NULL
			AND ao.ApprovedDate IS NOT NULL

	COMMIT TRAN
END TRY
BEGIN CATCH
	IF @@TRANCOUNT > 0
    	ROLLBACK TRAN
    SELECT @err_no = ERROR_NUMBER(), @err_sev = ERROR_SEVERITY(), @err_msg = ERROR_MESSAGE()
	RAISERROR (''Updating Newly Approved Orders with Resolution Code failed with error no: %d and message: %s'', @err_sev, 1, @err_no, @err_msg)
END CATCH

-- Update the suspended status for orders closed as Other/None that do not have resolution code but are now approved
BEGIN TRY
	BEGIN TRAN
		UPDATE pod
		SET
			pod.Suspended = ''N'',
			pod.ApprovedDate = ao.ApprovedDate
		FROM
			POData pod
			INNER JOIN #approvedOrders ao on pod.PONumber = ao.PONumber
		WHERE
			ao.ResolutionCode = ''''
			AND ao.ApprovedDate IS NOT NULL
			AND ao.InvoiceNumber IS NOT NULL

		COMMIT TRAN
END TRY
BEGIN CATCH
	IF @@TRANCOUNT > 0
    	ROLLBACK TRAN
    SELECT @err_no = ERROR_NUMBER(), @err_sev = ERROR_SEVERITY(), @err_msg = ERROR_MESSAGE()
	RAISERROR (''Updating the Suspended Status of orders that were closed as Other/None failed with error no: %d and message: %s'', @err_sev, 1, @err_no, @err_msg)
END CATCH', 
		@database_name=N'POReports', 
		@flags=0
IF (@@ERROR <> 0 OR @ReturnCode <> 0) GOTO QuitWithRollback
/****** Object:  Step [NC_Get Weekly Orders by Close Date]    Script Date: 11/6/2013 3:51:59 PM ******/
EXEC @ReturnCode = msdb.dbo.sp_add_jobstep @job_id=@jobId, @step_name=N'NC_Get Weekly Orders by Close Date', 
		@step_id=9, 
		@cmdexec_success_code=0, 
		@on_success_action=3, 
		@on_success_step_id=0, 
		@on_fail_action=4, 
		@on_fail_step_id=27, 
		@retry_attempts=0, 
		@retry_interval=0, 
		@os_run_priority=0, @subsystem=N'TSQL', 
		@command=N'-- NC
USE POReports;
GO

--**************************************************************************
-- Create the synonyms to the appropriate IRMA database
--**************************************************************************
if exists (select * from sysobjects where xtype = ''SN'' and name = ''OrderHeader'')		drop synonym OrderHeader;
if exists (select * from sysobjects where xtype = ''SN'' and name = ''OrderItem'')			drop synonym OrderItem;
if exists (select * from sysobjects where xtype = ''SN'' and name = ''Vendor'')				drop synonym Vendor;
if exists (select * from sysobjects where xtype = ''SN'' and name = ''Users'')				drop synonym Users;
if exists (select * from sysobjects where xtype = ''SN'' and name = ''Store'')				drop synonym Store;
if exists (select * from sysobjects where xtype = ''SN'' and name = ''SubTeam'')			drop synonym SubTeam;
if exists (select * from sysobjects where xtype = ''SN'' and name = ''ReasonCodeDetail'')	drop synonym ReasonCodeDetail;
if exists (select * from sysobjects where xtype = ''SN'' and name = ''Region'')				drop synonym Region;

if exists (select * from sys.servers where name = ''IDT-NC\NCT'')
	begin
		
        create synonym OrderHeader		for [IDT-NC\NCT].[ItemCatalog_Test].[dbo].OrderHeader;
		create synonym OrderItem		for [IDT-NC\NCT].[ItemCatalog_Test].[dbo].OrderItem;
        create synonym Vendor			for [IDT-NC\NCT].[ItemCatalog_Test].[dbo].Vendor;
		create synonym Users			for [IDT-NC\NCT].[ItemCatalog_Test].[dbo].Users;
		create synonym Store			for [IDT-NC\NCT].[ItemCatalog_Test].[dbo].Store;
		create synonym SubTeam			for [IDT-NC\NCT].[ItemCatalog_Test].[dbo].SubTeam;
		create synonym ReasonCodeDetail for [IDT-NC\NCT].[ItemCatalog_Test].[dbo].ReasonCodeDetail;
		create synonym Region			for [IDT-NC\NCT].[ItemCatalog_Test].[dbo].Region;
    end
else if exists (select * from sys.servers where name = ''IDQ-NC\NCQ'')
	begin
        create synonym OrderHeader		for [IDQ-NC\NCQ].[ItemCatalog].[dbo].OrderHeader;
		create synonym OrderItem		for [IDQ-NC\NCQ].[ItemCatalog].[dbo].OrderItem;
        create synonym Vendor			for [IDQ-NC\NCQ].[ItemCatalog].[dbo].Vendor;
		create synonym Users			for [IDQ-NC\NCQ].[ItemCatalog].[dbo].Users;
		create synonym Store			for [IDQ-NC\NCQ].[ItemCatalog].[dbo].Store;
		create synonym SubTeam			for [IDQ-NC\NCQ].[ItemCatalog].[dbo].SubTeam;
		create synonym ReasonCodeDetail for [IDQ-NC\NCQ].[ItemCatalog].[dbo].ReasonCodeDetail;
		create synonym Region			for [IDQ-NC\NCQ].[ItemCatalog].[dbo].Region;
	end    
else if exists (select * from sys.servers where name = ''IDP-NC\NCP'')
	begin
        create synonym OrderHeader		for [IDP-NC\NCP].[ItemCatalog].[dbo].OrderHeader;
		create synonym OrderItem		for [IDP-NC\NCP].[ItemCatalog].[dbo].OrderItem;
        create synonym Vendor			for [IDP-NC\NCP].[ItemCatalog].[dbo].Vendor;
		create synonym Users			for [IDP-NC\NCP].[ItemCatalog].[dbo].Users;
		create synonym Store			for [IDP-NC\NCP].[ItemCatalog].[dbo].Store;
		create synonym SubTeam			for [IDP-NC\NCP].[ItemCatalog].[dbo].SubTeam;
		create synonym ReasonCodeDetail for [IDP-NC\NCP].[ItemCatalog].[dbo].ReasonCodeDetail;
		create synonym Region			for [IDP-NC\NCP].[ItemCatalog].[dbo].Region;
	end

--**************************************************************************
-- Set and Populate internal variables
--**************************************************************************
DECLARE @startdate datetime;
DECLARE @enddate datetime;
DECLARE @region	varchar(2);
DECLARE @insertDate	datetime2;

-- Get Monday 00:00:00 of Last Week and Get Sunday 23:59:59.997 of Last Week
SELECT @startdate = DATEADD(day, -7 , DATEADD(wk, DATEDIFF(wk, 0, GETDATE()), 0))
SELECT @enddate = DATEADD(ms, -3, DATEADD(wk, DATEDIFF(wk, 0, GETDATE()), 0))

SELECT  @region			= RegionCode FROM Region;
SELECT	@insertDate		= GETDATE();

--**************************************************************************
-- Get List of Orders and Insert Into PODataLoad table
--**************************************************************************
BEGIN TRY
BEGIN TRAN

	-- Insert into PODataLoad table
	INSERT INTO PODataLoad
	SELECT 
		oh.OrderHeader_ID											as PONumber,
		CASE 
			WHEN oh.ResolutionCodeID IS NOT NULL AND oh.ApprovedDate IS NOT NULL THEN ''Y'' 
			WHEN oh.ApprovedDate IS NULL THEN ''Y''
			ELSE ''N'' 
		END															as Suspended,
		oh.CloseDate												as CloseDate,
		CASE
			WHEN oh.ResolutionCodeID IS NOT NULL AND oh.ApprovedDate IS NOT NULL THEN rcd.ReasonCodeDesc
			WHEN oh.ApprovedDate IS NULL AND oh.InvoiceNumber IS NOT NULL THEN ''Suspended but not Approved''
			WHEN oh.ApprovedDate IS NULL AND oh.InvoiceNumber IS NULL THEN ''Closed as Other/None''
			ELSE ''''
		END															as ResolutionCode,
		ISNULL(oh.AdminNotes, '''')									as AdminNotes,
		v.CompanyName												as Vendor,
		st.SubTeam_No												as Subteam,
		s.Store_Name												as Store,
		CASE 
			WHEN oh.DiscountType > 0 THEN ''Y''
			WHEN 0 < SUM(oi.DiscountType) Then ''Y''
			WHEN 0 < SUM(oi.AdjustedCost) Then ''Y''
			ELSE ''N''
		END															as AdjustedCost,
		CASE 
			WHEN oh.Return_Order = 1 THEN ''Y'' 
			ELSE ''N'' 
		END															as CreditPO,
		CASE 
			WHEN oh.PayByAgreedCost = 1 THEN ''Pay By Agreed Cost'' 
			ELSE ''Pay By Invoice'' 
		END															as VendorType,
		ucr.FullName												as POCreator,
		CASE 
			WHEN oh.Einvoice_id IS NOT NULL THEN ''Y'' 
			ELSE ''N'' 
		END															as EInvoiceMatchedToPO,
		ISNULL(oh.OrderHeaderDesc, '''')								as PONotes,
		ucl.FullName												as ClosedBy,
		@region														as Region,
		oh.ApprovedDate												as ApprovedDate,
		oh.InvoiceNumber											as InvoiceNumber,
		@insertDate													as InsertDate
	FROM
		OrderHeader					oh	(nolock)
		INNER JOIN OrderItem		oi	(nolock) on oh.OrderHeader_ID		= oi.OrderHeader_ID
		INNER JOIN Vendor			vr	(nolock) on oh.ReceiveLocation_ID	= vr.Vendor_ID
		INNER JOIN Vendor			v	(nolock) on oh.Vendor_ID			= v.Vendor_ID
		INNER JOIN Users			ucr (nolock) on oh.CreatedBy			= ucr.User_ID
		INNER JOIN Users			ucl	(nolock) on oh.ClosedBy				= ucl.User_ID
		INNER JOIN Store			s	(nolock) on vr.Store_No				= s.Store_No
		INNER JOIN SubTeam			st	(nolock) on oh.Transfer_To_SubTeam	= st.SubTeam_No
		LEFT JOIN ReasonCodeDetail	rcd (nolock) on oh.ResolutionCodeID		= rcd.ReasonCodeDetailID
	WHERE
		oh.CloseDate		>= @StartDate
		AND oh.CloseDate	<= @EndDate
		AND oh.OrderType_ID <> 3 -- filter out Transfer orders
	GROUP BY
		oh.OrderHeader_ID,
		oh.DiscountType,
		oh.Return_Order,
		oh.PayByAgreedCost,
		oh.eInvoice_Id,
		oh.ResolutionCodeID,
		oh.Vendor_ID,
		oh.ReceiveLocation_ID,
		oh.CreatedBy,
		oh.ClosedBy,
		oh.Transfer_To_SubTeam,
		oh.AdminNotes,
		oh.OrderHeaderDesc,
		oh.CloseDate,
		oh.ApprovedDate,
		oh.InvoiceNumber,
		rcd.ReasonCodeDesc,
		v.CompanyName,
		st.SubTeam_No,
		s.Store_Name,
		ucr.FullName,
		ucl.FullName

	PRINT ''Data has been loaded into PODataLoad table for the '' + @region + '' region.''
	
	-- Insert data into PODataLoadStatus
	INSERT INTO dbo.PODataLoadStatus (Region, InsertDate)
	SELECT @region, @insertDate;
	PRINT ''PODataLoadStatus table has been updated for InsertDate: '' + CAST(@insertDate as nvarchar) + '' and Region: '' + @region + ''.'';

	COMMIT TRAN

END TRY
BEGIN CATCH
	IF @@TRANCOUNT > 0
    	ROLLBACK TRAN
	DECLARE @err_no int, @err_sev int, @err_msg varchar(MAX)
    SELECT @err_no = ERROR_NUMBER(), @err_sev = ERROR_SEVERITY(), @err_msg = ERROR_MESSAGE()
	RAISERROR (''GetWeeklyPOData failed with error no: %d and message: %s'', @err_sev, 1, @err_no, @err_msg)
END CATCH', 
		@database_name=N'POReports', 
		@flags=0
IF (@@ERROR <> 0 OR @ReturnCode <> 0) GOTO QuitWithRollback
/****** Object:  Step [NC_Update Approved Orders]    Script Date: 11/6/2013 3:51:59 PM ******/
EXEC @ReturnCode = msdb.dbo.sp_add_jobstep @job_id=@jobId, @step_name=N'NC_Update Approved Orders', 
		@step_id=10, 
		@cmdexec_success_code=0, 
		@on_success_action=3, 
		@on_success_step_id=0, 
		@on_fail_action=4, 
		@on_fail_step_id=27, 
		@retry_attempts=0, 
		@retry_interval=0, 
		@os_run_priority=0, @subsystem=N'TSQL', 
		@command=N'-- NC
-- Update Approved Orders

USE POReports;
GO

--**************************************************************************
-- Create the synonyms to the appropriate IRMA database
--**************************************************************************
if exists (select * from sysobjects where xtype = ''SN'' and name = ''OrderHeader'')		drop synonym OrderHeader;
if exists (select * from sysobjects where xtype = ''SN'' and name = ''OrderItem'')			drop synonym OrderItem;
if exists (select * from sysobjects where xtype = ''SN'' and name = ''Vendor'')				drop synonym Vendor;
if exists (select * from sysobjects where xtype = ''SN'' and name = ''Users'')				drop synonym Users;
if exists (select * from sysobjects where xtype = ''SN'' and name = ''Store'')				drop synonym Store;
if exists (select * from sysobjects where xtype = ''SN'' and name = ''SubTeam'')			drop synonym SubTeam;
if exists (select * from sysobjects where xtype = ''SN'' and name = ''ReasonCodeDetail'')	drop synonym ReasonCodeDetail;
if exists (select * from sysobjects where xtype = ''SN'' and name = ''Region'')				drop synonym Region;

if exists (select * from sys.servers where name = ''IDT-NC\NCT'')
	begin
		
        create synonym OrderHeader		for [IDT-NC\NCT].[ItemCatalog_Test].[dbo].OrderHeader;
		create synonym OrderItem		for [IDT-NC\NCT].[ItemCatalog_Test].[dbo].OrderItem;
        create synonym Vendor			for [IDT-NC\NCT].[ItemCatalog_Test].[dbo].Vendor;
		create synonym Users			for [IDT-NC\NCT].[ItemCatalog_Test].[dbo].Users;
		create synonym Store			for [IDT-NC\NCT].[ItemCatalog_Test].[dbo].Store;
		create synonym SubTeam			for [IDT-NC\NCT].[ItemCatalog_Test].[dbo].SubTeam;
		create synonym ReasonCodeDetail for [IDT-NC\NCT].[ItemCatalog_Test].[dbo].ReasonCodeDetail;
		create synonym Region			for [IDT-NC\NCT].[ItemCatalog_Test].[dbo].Region;
    end
else if exists (select * from sys.servers where name = ''IDQ-NC\NCQ'')
	begin
        create synonym OrderHeader		for [IDQ-NC\NCQ].[ItemCatalog].[dbo].OrderHeader;
		create synonym OrderItem		for [IDQ-NC\NCQ].[ItemCatalog].[dbo].OrderItem;
        create synonym Vendor			for [IDQ-NC\NCQ].[ItemCatalog].[dbo].Vendor;
		create synonym Users			for [IDQ-NC\NCQ].[ItemCatalog].[dbo].Users;
		create synonym Store			for [IDQ-NC\NCQ].[ItemCatalog].[dbo].Store;
		create synonym SubTeam			for [IDQ-NC\NCQ].[ItemCatalog].[dbo].SubTeam;
		create synonym ReasonCodeDetail for [IDQ-NC\NCQ].[ItemCatalog].[dbo].ReasonCodeDetail;
		create synonym Region			for [IDQ-NC\NCQ].[ItemCatalog].[dbo].Region;
	end    
else if exists (select * from sys.servers where name = ''IDP-NC\NCP'')
	begin
        create synonym OrderHeader		for [IDP-NC\NCP].[ItemCatalog].[dbo].OrderHeader;
		create synonym OrderItem		for [IDP-NC\NCP].[ItemCatalog].[dbo].OrderItem;
        create synonym Vendor			for [IDP-NC\NCP].[ItemCatalog].[dbo].Vendor;
		create synonym Users			for [IDP-NC\NCP].[ItemCatalog].[dbo].Users;
		create synonym Store			for [IDP-NC\NCP].[ItemCatalog].[dbo].Store;
		create synonym SubTeam			for [IDP-NC\NCP].[ItemCatalog].[dbo].SubTeam;
		create synonym ReasonCodeDetail for [IDP-NC\NCP].[ItemCatalog].[dbo].ReasonCodeDetail;
		create synonym Region			for [IDP-NC\NCP].[ItemCatalog].[dbo].Region;
	end

--**************************************************************************
-- Set and Populate internal variables
--**************************************************************************
DECLARE @startdate datetime;
DECLARE @enddate datetime;
DECLARE @region	varchar(2);

-- Get Monday 00:00:00 of Last Week
SELECT @startdate = DATEADD(day, -7 , DATEADD(wk, DATEDIFF(wk, 0, GETDATE()), 0))

-- Get Sunday 23:59:59.997 of Last Week
SELECT @enddate = DATEADD(ms, -3, DATEADD(wk, DATEDIFF(wk, 0, GETDATE()), 0))

select @startdate, @enddate
SELECT  @region	= RegionCode FROM Region;

--**************************************************************************
-- Get list of orders that were approved during the previous week
-- This will be used for updating orders in POData
--**************************************************************************
SELECT
	oh.OrderHeader_ID				as PONumber,
	oh.ApprovedDate					as ApprovedDate,
	ISNULL(rcd.ReasonCodeDesc, '''')	as ResolutionCode,
	oh.InvoiceNumber				as InvoiceNumber
INTO #approvedOrders
FROM
	OrderHeader					oh	(nolock)
	LEFT JOIN ReasonCodeDetail	rcd (nolock) on oh.ResolutionCodeID	= rcd.ReasonCodeDetailID
WHERE
	oh.ApprovedDate		>= @startdate
	AND oh.ApprovedDate <= @enddate

create clustered index idx_c_ApprovedOrders_PONumber on #approvedOrders (PONumber)
create nonclustered index idx_ApprovedOrders_ResolutionCode on #approvedOrders (ResolutionCode)

--**************************************************************************
-- Update orders in POData that were ''Suspended but not Approved'' or 
-- orders that were closed as Other/None but are now approved
--**************************************************************************
-- Update orders with resolution code if there is one assigned for orders that have invoice info.
DECLARE @err_no int, @err_sev int, @err_msg varchar(MAX)

BEGIN TRY
	BEGIN TRAN
		UPDATE pod
		SET
			pod.ResolutionCode = ao.ResolutionCode,
			pod.ApprovedDate = ao.ApprovedDate
		FROM
			POData pod
			INNER JOIN #approvedOrders ao on pod.PONumber = ao.PONumber
		WHERE
			ao.ResolutionCode <> ''''
			AND ao.InvoiceNumber IS NOT NULL
			AND ao.ApprovedDate IS NOT NULL

	COMMIT TRAN
END TRY
BEGIN CATCH
	IF @@TRANCOUNT > 0
    	ROLLBACK TRAN
    SELECT @err_no = ERROR_NUMBER(), @err_sev = ERROR_SEVERITY(), @err_msg = ERROR_MESSAGE()
	RAISERROR (''Updating Newly Approved Orders with Resolution Code failed with error no: %d and message: %s'', @err_sev, 1, @err_no, @err_msg)
END CATCH

-- Update the suspended status for orders closed as Other/None that do not have resolution code but are now approved
BEGIN TRY
	BEGIN TRAN
		UPDATE pod
		SET
			pod.Suspended = ''N'',
			pod.ApprovedDate = ao.ApprovedDate
		FROM
			POData pod
			INNER JOIN #approvedOrders ao on pod.PONumber = ao.PONumber
		WHERE
			ao.ResolutionCode = ''''
			AND ao.ApprovedDate IS NOT NULL
			AND ao.InvoiceNumber IS NOT NULL

		COMMIT TRAN
END TRY
BEGIN CATCH
	IF @@TRANCOUNT > 0
    	ROLLBACK TRAN
    SELECT @err_no = ERROR_NUMBER(), @err_sev = ERROR_SEVERITY(), @err_msg = ERROR_MESSAGE()
	RAISERROR (''Updating the Suspended Status of orders that were closed as Other/None failed with error no: %d and message: %s'', @err_sev, 1, @err_no, @err_msg)
END CATCH', 
		@database_name=N'POReports', 
		@flags=0
IF (@@ERROR <> 0 OR @ReturnCode <> 0) GOTO QuitWithRollback
/****** Object:  Step [NE_Get Weekly Orders By Close Date]    Script Date: 11/6/2013 3:51:59 PM ******/
EXEC @ReturnCode = msdb.dbo.sp_add_jobstep @job_id=@jobId, @step_name=N'NE_Get Weekly Orders By Close Date', 
		@step_id=11, 
		@cmdexec_success_code=0, 
		@on_success_action=3, 
		@on_success_step_id=0, 
		@on_fail_action=4, 
		@on_fail_step_id=27, 
		@retry_attempts=0, 
		@retry_interval=0, 
		@os_run_priority=0, @subsystem=N'TSQL', 
		@command=N'-- NE
USE POReports;
GO

--**************************************************************************
-- Create the synonyms to the appropriate IRMA database
--**************************************************************************
if exists (select * from sysobjects where xtype = ''SN'' and name = ''OrderHeader'')		drop synonym OrderHeader;
if exists (select * from sysobjects where xtype = ''SN'' and name = ''OrderItem'')			drop synonym OrderItem;
if exists (select * from sysobjects where xtype = ''SN'' and name = ''Vendor'')				drop synonym Vendor;
if exists (select * from sysobjects where xtype = ''SN'' and name = ''Users'')				drop synonym Users;
if exists (select * from sysobjects where xtype = ''SN'' and name = ''Store'')				drop synonym Store;
if exists (select * from sysobjects where xtype = ''SN'' and name = ''SubTeam'')			drop synonym SubTeam;
if exists (select * from sysobjects where xtype = ''SN'' and name = ''ReasonCodeDetail'')	drop synonym ReasonCodeDetail;
if exists (select * from sysobjects where xtype = ''SN'' and name = ''Region'')				drop synonym Region;

if exists (select * from sys.servers where name = ''IDT-NE\NET'')
	begin
		
        create synonym OrderHeader		for [IDT-NE\NET].[ItemCatalog_Test].[dbo].OrderHeader;
		create synonym OrderItem		for [IDT-NE\NET].[ItemCatalog_Test].[dbo].OrderItem;
        create synonym Vendor			for [IDT-NE\NET].[ItemCatalog_Test].[dbo].Vendor;
		create synonym Users			for [IDT-NE\NET].[ItemCatalog_Test].[dbo].Users;
		create synonym Store			for [IDT-NE\NET].[ItemCatalog_Test].[dbo].Store;
		create synonym SubTeam			for [IDT-NE\NET].[ItemCatalog_Test].[dbo].SubTeam;
		create synonym ReasonCodeDetail for [IDT-NE\NET].[ItemCatalog_Test].[dbo].ReasonCodeDetail;
		create synonym Region			for [IDT-NE\NET].[ItemCatalog_Test].[dbo].Region;
    end
else if exists (select * from sys.servers where name = ''IDQ-NE\NEQ'')
	begin
        create synonym OrderHeader		for [IDQ-NE\NEQ].[ItemCatalog].[dbo].OrderHeader;
		create synonym OrderItem		for [IDQ-NE\NEQ].[ItemCatalog].[dbo].OrderItem;
        create synonym Vendor			for [IDQ-NE\NEQ].[ItemCatalog].[dbo].Vendor;
		create synonym Users			for [IDQ-NE\NEQ].[ItemCatalog].[dbo].Users;
		create synonym Store			for [IDQ-NE\NEQ].[ItemCatalog].[dbo].Store;
		create synonym SubTeam			for [IDQ-NE\NEQ].[ItemCatalog].[dbo].SubTeam;
		create synonym ReasonCodeDetail for [IDQ-NE\NEQ].[ItemCatalog].[dbo].ReasonCodeDetail;
		create synonym Region			for [IDQ-NE\NEQ].[ItemCatalog].[dbo].Region;
	end    
else if exists (select * from sys.servers where name = ''IDP-NE\NEP'')
	begin
        create synonym OrderHeader		for [IDP-NE\NEP].[ItemCatalog].[dbo].OrderHeader;
		create synonym OrderItem		for [IDP-NE\NEP].[ItemCatalog].[dbo].OrderItem;
        create synonym Vendor			for [IDP-NE\NEP].[ItemCatalog].[dbo].Vendor;
		create synonym Users			for [IDP-NE\NEP].[ItemCatalog].[dbo].Users;
		create synonym Store			for [IDP-NE\NEP].[ItemCatalog].[dbo].Store;
		create synonym SubTeam			for [IDP-NE\NEP].[ItemCatalog].[dbo].SubTeam;
		create synonym ReasonCodeDetail for [IDP-NE\NEP].[ItemCatalog].[dbo].ReasonCodeDetail;
		create synonym Region			for [IDP-NE\NEP].[ItemCatalog].[dbo].Region;
	end

--**************************************************************************
-- Set and Populate internal variables
--**************************************************************************
DECLARE @startdate datetime;
DECLARE @enddate datetime;
DECLARE @region	varchar(2);
DECLARE @insertDate	datetime2;

-- Get Monday 00:00:00 of Last Week and Get Sunday 23:59:59.997 of Last Week
SELECT @startdate = DATEADD(day, -7 , DATEADD(wk, DATEDIFF(wk, 0, GETDATE()), 0))
SELECT @enddate = DATEADD(ms, -3, DATEADD(wk, DATEDIFF(wk, 0, GETDATE()), 0))

SELECT  @region			= RegionCode FROM Region;
SELECT	@insertDate		= GETDATE();

--**************************************************************************
-- Get List of Orders and Insert Into PODataLoad table
--**************************************************************************
BEGIN TRY
BEGIN TRAN

	-- Insert into PODataLoad table
	INSERT INTO PODataLoad
	SELECT 
		oh.OrderHeader_ID											as PONumber,
		CASE 
			WHEN oh.ResolutionCodeID IS NOT NULL AND oh.ApprovedDate IS NOT NULL THEN ''Y'' 
			WHEN oh.ApprovedDate IS NULL THEN ''Y''
			ELSE ''N'' 
		END															as Suspended,
		oh.CloseDate												as CloseDate,
		CASE
			WHEN oh.ResolutionCodeID IS NOT NULL AND oh.ApprovedDate IS NOT NULL THEN rcd.ReasonCodeDesc
			WHEN oh.ApprovedDate IS NULL AND oh.InvoiceNumber IS NOT NULL THEN ''Suspended but not Approved''
			WHEN oh.ApprovedDate IS NULL AND oh.InvoiceNumber IS NULL THEN ''Closed as Other/None''
			ELSE ''''
		END															as ResolutionCode,
		ISNULL(oh.AdminNotes, '''')									as AdminNotes,
		v.CompanyName												as Vendor,
		st.SubTeam_No												as Subteam,
		s.Store_Name												as Store,
		CASE 
			WHEN oh.DiscountType > 0 THEN ''Y''
			WHEN 0 < SUM(oi.DiscountType) Then ''Y''
			WHEN 0 < SUM(oi.AdjustedCost) Then ''Y''
			ELSE ''N''
		END															as AdjustedCost,
		CASE 
			WHEN oh.Return_Order = 1 THEN ''Y'' 
			ELSE ''N'' 
		END															as CreditPO,
		CASE 
			WHEN oh.PayByAgreedCost = 1 THEN ''Pay By Agreed Cost'' 
			ELSE ''Pay By Invoice'' 
		END															as VendorType,
		ucr.FullName												as POCreator,
		CASE 
			WHEN oh.Einvoice_id IS NOT NULL THEN ''Y'' 
			ELSE ''N'' 
		END															as EInvoiceMatchedToPO,
		ISNULL(oh.OrderHeaderDesc, '''')								as PONotes,
		ucl.FullName												as ClosedBy,
		@region														as Region,
		oh.ApprovedDate												as ApprovedDate,
		oh.InvoiceNumber											as InvoiceNumber,
		@insertDate													as InsertDate
	FROM
		OrderHeader					oh	(nolock)
		INNER JOIN OrderItem		oi	(nolock) on oh.OrderHeader_ID		= oi.OrderHeader_ID
		INNER JOIN Vendor			vr	(nolock) on oh.ReceiveLocation_ID	= vr.Vendor_ID
		INNER JOIN Vendor			v	(nolock) on oh.Vendor_ID			= v.Vendor_ID
		INNER JOIN Users			ucr (nolock) on oh.CreatedBy			= ucr.User_ID
		INNER JOIN Users			ucl	(nolock) on oh.ClosedBy				= ucl.User_ID
		INNER JOIN Store			s	(nolock) on vr.Store_No				= s.Store_No
		INNER JOIN SubTeam			st	(nolock) on oh.Transfer_To_SubTeam	= st.SubTeam_No
		LEFT JOIN ReasonCodeDetail	rcd (nolock) on oh.ResolutionCodeID		= rcd.ReasonCodeDetailID
	WHERE
		oh.CloseDate		>= @StartDate
		AND oh.CloseDate	<= @EndDate
		AND oh.OrderType_ID <> 3 -- filter out Transfer orders
	GROUP BY
		oh.OrderHeader_ID,
		oh.DiscountType,
		oh.Return_Order,
		oh.PayByAgreedCost,
		oh.eInvoice_Id,
		oh.ResolutionCodeID,
		oh.Vendor_ID,
		oh.ReceiveLocation_ID,
		oh.CreatedBy,
		oh.ClosedBy,
		oh.Transfer_To_SubTeam,
		oh.AdminNotes,
		oh.OrderHeaderDesc,
		oh.CloseDate,
		oh.ApprovedDate,
		oh.InvoiceNumber,
		rcd.ReasonCodeDesc,
		v.CompanyName,
		st.SubTeam_No,
		s.Store_Name,
		ucr.FullName,
		ucl.FullName

	PRINT ''Data has been loaded into PODataLoad table for the '' + @region + '' region.''
	
	-- Insert data into PODataLoadStatus
	INSERT INTO dbo.PODataLoadStatus (Region, InsertDate)
	SELECT @region, @insertDate;
	PRINT ''PODataLoadStatus table has been updated for InsertDate: '' + CAST(@insertDate as nvarchar) + '' and Region: '' + @region + ''.'';

	COMMIT TRAN

END TRY
BEGIN CATCH
	IF @@TRANCOUNT > 0
    	ROLLBACK TRAN
	DECLARE @err_no int, @err_sev int, @err_msg varchar(MAX)
    SELECT @err_no = ERROR_NUMBER(), @err_sev = ERROR_SEVERITY(), @err_msg = ERROR_MESSAGE()
	RAISERROR (''GetWeeklyPOData failed with error no: %d and message: %s'', @err_sev, 1, @err_no, @err_msg)
END CATCH', 
		@database_name=N'POReports', 
		@flags=0
IF (@@ERROR <> 0 OR @ReturnCode <> 0) GOTO QuitWithRollback
/****** Object:  Step [NE_Update Approved Orders]    Script Date: 11/6/2013 3:51:59 PM ******/
EXEC @ReturnCode = msdb.dbo.sp_add_jobstep @job_id=@jobId, @step_name=N'NE_Update Approved Orders', 
		@step_id=12, 
		@cmdexec_success_code=0, 
		@on_success_action=3, 
		@on_success_step_id=0, 
		@on_fail_action=4, 
		@on_fail_step_id=27, 
		@retry_attempts=0, 
		@retry_interval=0, 
		@os_run_priority=0, @subsystem=N'TSQL', 
		@command=N'-- NE
-- Update Approved Orders

USE POReports;
GO

--**************************************************************************
-- Create the synonyms to the appropriate IRMA database
--**************************************************************************
if exists (select * from sysobjects where xtype = ''SN'' and name = ''OrderHeader'')		drop synonym OrderHeader;
if exists (select * from sysobjects where xtype = ''SN'' and name = ''OrderItem'')			drop synonym OrderItem;
if exists (select * from sysobjects where xtype = ''SN'' and name = ''Vendor'')				drop synonym Vendor;
if exists (select * from sysobjects where xtype = ''SN'' and name = ''Users'')				drop synonym Users;
if exists (select * from sysobjects where xtype = ''SN'' and name = ''Store'')				drop synonym Store;
if exists (select * from sysobjects where xtype = ''SN'' and name = ''SubTeam'')			drop synonym SubTeam;
if exists (select * from sysobjects where xtype = ''SN'' and name = ''ReasonCodeDetail'')	drop synonym ReasonCodeDetail;
if exists (select * from sysobjects where xtype = ''SN'' and name = ''Region'')				drop synonym Region;

if exists (select * from sys.servers where name = ''IDT-NE\NET'')
	begin
		
        create synonym OrderHeader		for [IDT-NE\NET].[ItemCatalog_Test].[dbo].OrderHeader;
		create synonym OrderItem		for [IDT-NE\NET].[ItemCatalog_Test].[dbo].OrderItem;
        create synonym Vendor			for [IDT-NE\NET].[ItemCatalog_Test].[dbo].Vendor;
		create synonym Users			for [IDT-NE\NET].[ItemCatalog_Test].[dbo].Users;
		create synonym Store			for [IDT-NE\NET].[ItemCatalog_Test].[dbo].Store;
		create synonym SubTeam			for [IDT-NE\NET].[ItemCatalog_Test].[dbo].SubTeam;
		create synonym ReasonCodeDetail for [IDT-NE\NET].[ItemCatalog_Test].[dbo].ReasonCodeDetail;
		create synonym Region			for [IDT-NE\NET].[ItemCatalog_Test].[dbo].Region;
    end
else if exists (select * from sys.servers where name = ''IDQ-NE\NEQ'')
	begin
        create synonym OrderHeader		for [IDQ-NE\NEQ].[ItemCatalog].[dbo].OrderHeader;
		create synonym OrderItem		for [IDQ-NE\NEQ].[ItemCatalog].[dbo].OrderItem;
        create synonym Vendor			for [IDQ-NE\NEQ].[ItemCatalog].[dbo].Vendor;
		create synonym Users			for [IDQ-NE\NEQ].[ItemCatalog].[dbo].Users;
		create synonym Store			for [IDQ-NE\NEQ].[ItemCatalog].[dbo].Store;
		create synonym SubTeam			for [IDQ-NE\NEQ].[ItemCatalog].[dbo].SubTeam;
		create synonym ReasonCodeDetail for [IDQ-NE\NEQ].[ItemCatalog].[dbo].ReasonCodeDetail;
		create synonym Region			for [IDQ-NE\NEQ].[ItemCatalog].[dbo].Region;
	end    
else if exists (select * from sys.servers where name = ''IDP-NE\NEP'')
	begin
        create synonym OrderHeader		for [IDP-NE\NEP].[ItemCatalog].[dbo].OrderHeader;
		create synonym OrderItem		for [IDP-NE\NEP].[ItemCatalog].[dbo].OrderItem;
        create synonym Vendor			for [IDP-NE\NEP].[ItemCatalog].[dbo].Vendor;
		create synonym Users			for [IDP-NE\NEP].[ItemCatalog].[dbo].Users;
		create synonym Store			for [IDP-NE\NEP].[ItemCatalog].[dbo].Store;
		create synonym SubTeam			for [IDP-NE\NEP].[ItemCatalog].[dbo].SubTeam;
		create synonym ReasonCodeDetail for [IDP-NE\NEP].[ItemCatalog].[dbo].ReasonCodeDetail;
		create synonym Region			for [IDP-NE\NEP].[ItemCatalog].[dbo].Region;
	end

--**************************************************************************
-- Set and Populate internal variables
--**************************************************************************
DECLARE @startdate datetime;
DECLARE @enddate datetime;
DECLARE @region	varchar(2);

-- Get Monday 00:00:00 of Last Week
SELECT @startdate = DATEADD(day, -7 , DATEADD(wk, DATEDIFF(wk, 0, GETDATE()), 0))

-- Get Sunday 23:59:59.997 of Last Week
SELECT @enddate = DATEADD(ms, -3, DATEADD(wk, DATEDIFF(wk, 0, GETDATE()), 0))

select @startdate, @enddate
SELECT  @region	= RegionCode FROM Region;

--**************************************************************************
-- Get list of orders that were approved during the previous week
-- This will be used for updating orders in POData
--**************************************************************************
SELECT
	oh.OrderHeader_ID				as PONumber,
	oh.ApprovedDate					as ApprovedDate,
	ISNULL(rcd.ReasonCodeDesc, '''')	as ResolutionCode,
	oh.InvoiceNumber				as InvoiceNumber
INTO #approvedOrders
FROM
	OrderHeader					oh	(nolock)
	LEFT JOIN ReasonCodeDetail	rcd (nolock) on oh.ResolutionCodeID	= rcd.ReasonCodeDetailID
WHERE
	oh.ApprovedDate		>= @startdate
	AND oh.ApprovedDate <= @enddate

create clustered index idx_c_ApprovedOrders_PONumber on #approvedOrders (PONumber)
create nonclustered index idx_ApprovedOrders_ResolutionCode on #approvedOrders (ResolutionCode)

--**************************************************************************
-- Update orders in POData that were ''Suspended but not Approved'' or 
-- orders that were closed as Other/None but are now approved
--**************************************************************************
-- Update orders with resolution code if there is one assigned for orders that have invoice info.
DECLARE @err_no int, @err_sev int, @err_msg varchar(MAX)

BEGIN TRY
	BEGIN TRAN
		UPDATE pod
		SET
			pod.ResolutionCode = ao.ResolutionCode,
			pod.ApprovedDate = ao.ApprovedDate
		FROM
			POData pod
			INNER JOIN #approvedOrders ao on pod.PONumber = ao.PONumber
		WHERE
			ao.ResolutionCode <> ''''
			AND ao.InvoiceNumber IS NOT NULL
			AND ao.ApprovedDate IS NOT NULL

	COMMIT TRAN
END TRY
BEGIN CATCH
	IF @@TRANCOUNT > 0
    	ROLLBACK TRAN
    SELECT @err_no = ERROR_NUMBER(), @err_sev = ERROR_SEVERITY(), @err_msg = ERROR_MESSAGE()
	RAISERROR (''Updating Newly Approved Orders with Resolution Code failed with error no: %d and message: %s'', @err_sev, 1, @err_no, @err_msg)
END CATCH

-- Update the suspended status for orders closed as Other/None that do not have resolution code but are now approved
BEGIN TRY
	BEGIN TRAN
		UPDATE pod
		SET
			pod.Suspended = ''N'',
			pod.ApprovedDate = ao.ApprovedDate
		FROM
			POData pod
			INNER JOIN #approvedOrders ao on pod.PONumber = ao.PONumber
		WHERE
			ao.ResolutionCode = ''''
			AND ao.ApprovedDate IS NOT NULL
			AND ao.InvoiceNumber IS NOT NULL

		COMMIT TRAN
END TRY
BEGIN CATCH
	IF @@TRANCOUNT > 0
    	ROLLBACK TRAN
    SELECT @err_no = ERROR_NUMBER(), @err_sev = ERROR_SEVERITY(), @err_msg = ERROR_MESSAGE()
	RAISERROR (''Updating the Suspended Status of orders that were closed as Other/None failed with error no: %d and message: %s'', @err_sev, 1, @err_no, @err_msg)
END CATCH', 
		@database_name=N'POReports', 
		@flags=0
IF (@@ERROR <> 0 OR @ReturnCode <> 0) GOTO QuitWithRollback
/****** Object:  Step [PN_Get Weekly Orders By Close Date]    Script Date: 11/6/2013 3:51:59 PM ******/
EXEC @ReturnCode = msdb.dbo.sp_add_jobstep @job_id=@jobId, @step_name=N'PN_Get Weekly Orders By Close Date', 
		@step_id=13, 
		@cmdexec_success_code=0, 
		@on_success_action=3, 
		@on_success_step_id=0, 
		@on_fail_action=4, 
		@on_fail_step_id=27, 
		@retry_attempts=0, 
		@retry_interval=0, 
		@os_run_priority=0, @subsystem=N'TSQL', 
		@command=N'-- PN
USE POReports;
GO

--**************************************************************************
-- Create the synonyms to the appropriate IRMA database
--**************************************************************************
if exists (select * from sysobjects where xtype = ''SN'' and name = ''OrderHeader'')		drop synonym OrderHeader;
if exists (select * from sysobjects where xtype = ''SN'' and name = ''OrderItem'')			drop synonym OrderItem;
if exists (select * from sysobjects where xtype = ''SN'' and name = ''Vendor'')				drop synonym Vendor;
if exists (select * from sysobjects where xtype = ''SN'' and name = ''Users'')				drop synonym Users;
if exists (select * from sysobjects where xtype = ''SN'' and name = ''Store'')				drop synonym Store;
if exists (select * from sysobjects where xtype = ''SN'' and name = ''SubTeam'')			drop synonym SubTeam;
if exists (select * from sysobjects where xtype = ''SN'' and name = ''ReasonCodeDetail'')	drop synonym ReasonCodeDetail;
if exists (select * from sysobjects where xtype = ''SN'' and name = ''Region'')				drop synonym Region;

if exists (select * from sys.servers where name = ''IDT-PN\PNT'')
	begin
		
        create synonym OrderHeader		for [IDT-PN\PNT].[ItemCatalog_Test].[dbo].OrderHeader;
		create synonym OrderItem		for [IDT-PN\PNT].[ItemCatalog_Test].[dbo].OrderItem;
        create synonym Vendor			for [IDT-PN\PNT].[ItemCatalog_Test].[dbo].Vendor;
		create synonym Users			for [IDT-PN\PNT].[ItemCatalog_Test].[dbo].Users;
		create synonym Store			for [IDT-PN\PNT].[ItemCatalog_Test].[dbo].Store;
		create synonym SubTeam			for [IDT-PN\PNT].[ItemCatalog_Test].[dbo].SubTeam;
		create synonym ReasonCodeDetail for [IDT-PN\PNT].[ItemCatalog_Test].[dbo].ReasonCodeDetail;
		create synonym Region			for [IDT-PN\PNT].[ItemCatalog_Test].[dbo].Region;
    end
else if exists (select * from sys.servers where name = ''IDQ-PN\PNQ'')
	begin
        create synonym OrderHeader		for [IDQ-PN\PNQ].[ItemCatalog].[dbo].OrderHeader;
		create synonym OrderItem		for [IDQ-PN\PNQ].[ItemCatalog].[dbo].OrderItem;
        create synonym Vendor			for [IDQ-PN\PNQ].[ItemCatalog].[dbo].Vendor;
		create synonym Users			for [IDQ-PN\PNQ].[ItemCatalog].[dbo].Users;
		create synonym Store			for [IDQ-PN\PNQ].[ItemCatalog].[dbo].Store;
		create synonym SubTeam			for [IDQ-PN\PNQ].[ItemCatalog].[dbo].SubTeam;
		create synonym ReasonCodeDetail for [IDQ-PN\PNQ].[ItemCatalog].[dbo].ReasonCodeDetail;
		create synonym Region			for [IDQ-PN\PNQ].[ItemCatalog].[dbo].Region;
	end    
else if exists (select * from sys.servers where name = ''IDP-PN\PNP'')
	begin
        create synonym OrderHeader		for [IDP-PN\PNP].[ItemCatalog].[dbo].OrderHeader;
		create synonym OrderItem		for [IDP-PN\PNP].[ItemCatalog].[dbo].OrderItem;
        create synonym Vendor			for [IDP-PN\PNP].[ItemCatalog].[dbo].Vendor;
		create synonym Users			for [IDP-PN\PNP].[ItemCatalog].[dbo].Users;
		create synonym Store			for [IDP-PN\PNP].[ItemCatalog].[dbo].Store;
		create synonym SubTeam			for [IDP-PN\PNP].[ItemCatalog].[dbo].SubTeam;
		create synonym ReasonCodeDetail for [IDP-PN\PNP].[ItemCatalog].[dbo].ReasonCodeDetail;
		create synonym Region			for [IDP-PN\PNP].[ItemCatalog].[dbo].Region;
	end

--**************************************************************************
-- Set and Populate internal variables
--**************************************************************************
DECLARE @startdate datetime;
DECLARE @enddate datetime;
DECLARE @region	varchar(2);
DECLARE @insertDate	datetime2;

-- Get Monday 00:00:00 of Last Week and Get Sunday 23:59:59.997 of Last Week
SELECT @startdate = DATEADD(day, -7 , DATEADD(wk, DATEDIFF(wk, 0, GETDATE()), 0))
SELECT @enddate = DATEADD(ms, -3, DATEADD(wk, DATEDIFF(wk, 0, GETDATE()), 0))

SELECT  @region			= RegionCode FROM Region;
SELECT	@insertDate		= GETDATE();

--**************************************************************************
-- Get List of Orders and Insert Into PODataLoad table
--**************************************************************************
BEGIN TRY
BEGIN TRAN

	-- Insert into PODataLoad table
	INSERT INTO PODataLoad
	SELECT 
		oh.OrderHeader_ID											as PONumber,
		CASE 
			WHEN oh.ResolutionCodeID IS NOT NULL AND oh.ApprovedDate IS NOT NULL THEN ''Y'' 
			WHEN oh.ApprovedDate IS NULL THEN ''Y''
			ELSE ''N'' 
		END															as Suspended,
		oh.CloseDate												as CloseDate,
		CASE
			WHEN oh.ResolutionCodeID IS NOT NULL AND oh.ApprovedDate IS NOT NULL THEN rcd.ReasonCodeDesc
			WHEN oh.ApprovedDate IS NULL AND oh.InvoiceNumber IS NOT NULL THEN ''Suspended but not Approved''
			WHEN oh.ApprovedDate IS NULL AND oh.InvoiceNumber IS NULL THEN ''Closed as Other/None''
			ELSE ''''
		END															as ResolutionCode,
		ISNULL(oh.AdminNotes, '''')									as AdminNotes,
		v.CompanyName												as Vendor,
		st.SubTeam_No												as Subteam,
		s.Store_Name												as Store,
		CASE 
			WHEN oh.DiscountType > 0 THEN ''Y''
			WHEN 0 < SUM(oi.DiscountType) Then ''Y''
			WHEN 0 < SUM(oi.AdjustedCost) Then ''Y''
			ELSE ''N''
		END															as AdjustedCost,
		CASE 
			WHEN oh.Return_Order = 1 THEN ''Y'' 
			ELSE ''N'' 
		END															as CreditPO,
		CASE 
			WHEN oh.PayByAgreedCost = 1 THEN ''Pay By Agreed Cost'' 
			ELSE ''Pay By Invoice'' 
		END															as VendorType,
		ucr.FullName												as POCreator,
		CASE 
			WHEN oh.Einvoice_id IS NOT NULL THEN ''Y'' 
			ELSE ''N'' 
		END															as EInvoiceMatchedToPO,
		ISNULL(oh.OrderHeaderDesc, '''')								as PONotes,
		ucl.FullName												as ClosedBy,
		@region														as Region,
		oh.ApprovedDate												as ApprovedDate,
		oh.InvoiceNumber											as InvoiceNumber,
		@insertDate													as InsertDate
	FROM
		OrderHeader					oh	(nolock)
		INNER JOIN OrderItem		oi	(nolock) on oh.OrderHeader_ID		= oi.OrderHeader_ID
		INNER JOIN Vendor			vr	(nolock) on oh.ReceiveLocation_ID	= vr.Vendor_ID
		INNER JOIN Vendor			v	(nolock) on oh.Vendor_ID			= v.Vendor_ID
		INNER JOIN Users			ucr (nolock) on oh.CreatedBy			= ucr.User_ID
		INNER JOIN Users			ucl	(nolock) on oh.ClosedBy				= ucl.User_ID
		INNER JOIN Store			s	(nolock) on vr.Store_No				= s.Store_No
		INNER JOIN SubTeam			st	(nolock) on oh.Transfer_To_SubTeam	= st.SubTeam_No
		LEFT JOIN ReasonCodeDetail	rcd (nolock) on oh.ResolutionCodeID		= rcd.ReasonCodeDetailID
	WHERE
		oh.CloseDate		>= @StartDate
		AND oh.CloseDate	<= @EndDate
		AND oh.OrderType_ID <> 3 -- filter out Transfer orders
	GROUP BY
		oh.OrderHeader_ID,
		oh.DiscountType,
		oh.Return_Order,
		oh.PayByAgreedCost,
		oh.eInvoice_Id,
		oh.ResolutionCodeID,
		oh.Vendor_ID,
		oh.ReceiveLocation_ID,
		oh.CreatedBy,
		oh.ClosedBy,
		oh.Transfer_To_SubTeam,
		oh.AdminNotes,
		oh.OrderHeaderDesc,
		oh.CloseDate,
		oh.ApprovedDate,
		oh.InvoiceNumber,
		rcd.ReasonCodeDesc,
		v.CompanyName,
		st.SubTeam_No,
		s.Store_Name,
		ucr.FullName,
		ucl.FullName

	PRINT ''Data has been loaded into PODataLoad table for the '' + @region + '' region.''
	
	-- Insert data into PODataLoadStatus
	INSERT INTO dbo.PODataLoadStatus (Region, InsertDate)
	SELECT @region, @insertDate;
	PRINT ''PODataLoadStatus table has been updated for InsertDate: '' + CAST(@insertDate as nvarchar) + '' and Region: '' + @region + ''.'';

	COMMIT TRAN

END TRY
BEGIN CATCH
	IF @@TRANCOUNT > 0
    	ROLLBACK TRAN
	DECLARE @err_no int, @err_sev int, @err_msg varchar(MAX)
    SELECT @err_no = ERROR_NUMBER(), @err_sev = ERROR_SEVERITY(), @err_msg = ERROR_MESSAGE()
	RAISERROR (''GetWeeklyPOData failed with error no: %d and message: %s'', @err_sev, 1, @err_no, @err_msg)
END CATCH', 
		@database_name=N'POReports', 
		@flags=0
IF (@@ERROR <> 0 OR @ReturnCode <> 0) GOTO QuitWithRollback
/****** Object:  Step [PN_Update Approved Orders]    Script Date: 11/6/2013 3:51:59 PM ******/
EXEC @ReturnCode = msdb.dbo.sp_add_jobstep @job_id=@jobId, @step_name=N'PN_Update Approved Orders', 
		@step_id=14, 
		@cmdexec_success_code=0, 
		@on_success_action=3, 
		@on_success_step_id=0, 
		@on_fail_action=4, 
		@on_fail_step_id=27, 
		@retry_attempts=0, 
		@retry_interval=0, 
		@os_run_priority=0, @subsystem=N'TSQL', 
		@command=N'-- PN
-- Update Approved Orders

USE POReports;
GO

--**************************************************************************
-- Create the synonyms to the appropriate IRMA database
--**************************************************************************
if exists (select * from sysobjects where xtype = ''SN'' and name = ''OrderHeader'')		drop synonym OrderHeader;
if exists (select * from sysobjects where xtype = ''SN'' and name = ''OrderItem'')			drop synonym OrderItem;
if exists (select * from sysobjects where xtype = ''SN'' and name = ''Vendor'')				drop synonym Vendor;
if exists (select * from sysobjects where xtype = ''SN'' and name = ''Users'')				drop synonym Users;
if exists (select * from sysobjects where xtype = ''SN'' and name = ''Store'')				drop synonym Store;
if exists (select * from sysobjects where xtype = ''SN'' and name = ''SubTeam'')			drop synonym SubTeam;
if exists (select * from sysobjects where xtype = ''SN'' and name = ''ReasonCodeDetail'')	drop synonym ReasonCodeDetail;
if exists (select * from sysobjects where xtype = ''SN'' and name = ''Region'')				drop synonym Region;

if exists (select * from sys.servers where name = ''IDT-PN\PNT'')
	begin
		
        create synonym OrderHeader		for [IDT-PN\PNT].[ItemCatalog_Test].[dbo].OrderHeader;
		create synonym OrderItem		for [IDT-PN\PNT].[ItemCatalog_Test].[dbo].OrderItem;
        create synonym Vendor			for [IDT-PN\PNT].[ItemCatalog_Test].[dbo].Vendor;
		create synonym Users			for [IDT-PN\PNT].[ItemCatalog_Test].[dbo].Users;
		create synonym Store			for [IDT-PN\PNT].[ItemCatalog_Test].[dbo].Store;
		create synonym SubTeam			for [IDT-PN\PNT].[ItemCatalog_Test].[dbo].SubTeam;
		create synonym ReasonCodeDetail for [IDT-PN\PNT].[ItemCatalog_Test].[dbo].ReasonCodeDetail;
		create synonym Region			for [IDT-PN\PNT].[ItemCatalog_Test].[dbo].Region;
    end
else if exists (select * from sys.servers where name = ''IDQ-PN\PNQ'')
	begin
        create synonym OrderHeader		for [IDQ-PN\PNQ].[ItemCatalog].[dbo].OrderHeader;
		create synonym OrderItem		for [IDQ-PN\PNQ].[ItemCatalog].[dbo].OrderItem;
        create synonym Vendor			for [IDQ-PN\PNQ].[ItemCatalog].[dbo].Vendor;
		create synonym Users			for [IDQ-PN\PNQ].[ItemCatalog].[dbo].Users;
		create synonym Store			for [IDQ-PN\PNQ].[ItemCatalog].[dbo].Store;
		create synonym SubTeam			for [IDQ-PN\PNQ].[ItemCatalog].[dbo].SubTeam;
		create synonym ReasonCodeDetail for [IDQ-PN\PNQ].[ItemCatalog].[dbo].ReasonCodeDetail;
		create synonym Region			for [IDQ-PN\PNQ].[ItemCatalog].[dbo].Region;
	end    
else if exists (select * from sys.servers where name = ''IDP-PN\PNP'')
	begin
        create synonym OrderHeader		for [IDP-PN\PNP].[ItemCatalog].[dbo].OrderHeader;
		create synonym OrderItem		for [IDP-PN\PNP].[ItemCatalog].[dbo].OrderItem;
        create synonym Vendor			for [IDP-PN\PNP].[ItemCatalog].[dbo].Vendor;
		create synonym Users			for [IDP-PN\PNP].[ItemCatalog].[dbo].Users;
		create synonym Store			for [IDP-PN\PNP].[ItemCatalog].[dbo].Store;
		create synonym SubTeam			for [IDP-PN\PNP].[ItemCatalog].[dbo].SubTeam;
		create synonym ReasonCodeDetail for [IDP-PN\PNP].[ItemCatalog].[dbo].ReasonCodeDetail;
		create synonym Region			for [IDP-PN\PNP].[ItemCatalog].[dbo].Region;
	end

--**************************************************************************
-- Set and Populate internal variables
--**************************************************************************
DECLARE @startdate datetime;
DECLARE @enddate datetime;
DECLARE @region	varchar(2);

-- Get Monday 00:00:00 of Last Week
SELECT @startdate = DATEADD(day, -7 , DATEADD(wk, DATEDIFF(wk, 0, GETDATE()), 0))

-- Get Sunday 23:59:59.997 of Last Week
SELECT @enddate = DATEADD(ms, -3, DATEADD(wk, DATEDIFF(wk, 0, GETDATE()), 0))

select @startdate, @enddate
SELECT  @region	= RegionCode FROM Region;

--**************************************************************************
-- Get list of orders that were approved during the previous week
-- This will be used for updating orders in POData
--**************************************************************************
SELECT
	oh.OrderHeader_ID				as PONumber,
	oh.ApprovedDate					as ApprovedDate,
	ISNULL(rcd.ReasonCodeDesc, '''')	as ResolutionCode,
	oh.InvoiceNumber				as InvoiceNumber
INTO #approvedOrders
FROM
	OrderHeader					oh	(nolock)
	LEFT JOIN ReasonCodeDetail	rcd (nolock) on oh.ResolutionCodeID	= rcd.ReasonCodeDetailID
WHERE
	oh.ApprovedDate		>= @startdate
	AND oh.ApprovedDate <= @enddate

create clustered index idx_c_ApprovedOrders_PONumber on #approvedOrders (PONumber)
create nonclustered index idx_ApprovedOrders_ResolutionCode on #approvedOrders (ResolutionCode)

--**************************************************************************
-- Update orders in POData that were ''Suspended but not Approved'' or 
-- orders that were closed as Other/None but are now approved
--**************************************************************************
-- Update orders with resolution code if there is one assigned for orders that have invoice info.
DECLARE @err_no int, @err_sev int, @err_msg varchar(MAX)

BEGIN TRY
	BEGIN TRAN
		UPDATE pod
		SET
			pod.ResolutionCode = ao.ResolutionCode,
			pod.ApprovedDate = ao.ApprovedDate
		FROM
			POData pod
			INNER JOIN #approvedOrders ao on pod.PONumber = ao.PONumber
		WHERE
			ao.ResolutionCode <> ''''
			AND ao.InvoiceNumber IS NOT NULL
			AND ao.ApprovedDate IS NOT NULL

	COMMIT TRAN
END TRY
BEGIN CATCH
	IF @@TRANCOUNT > 0
    	ROLLBACK TRAN
    SELECT @err_no = ERROR_NUMBER(), @err_sev = ERROR_SEVERITY(), @err_msg = ERROR_MESSAGE()
	RAISERROR (''Updating Newly Approved Orders with Resolution Code failed with error no: %d and message: %s'', @err_sev, 1, @err_no, @err_msg)
END CATCH

-- Update the suspended status for orders closed as Other/None that do not have resolution code but are now approved
BEGIN TRY
	BEGIN TRAN
		UPDATE pod
		SET
			pod.Suspended = ''N'',
			pod.ApprovedDate = ao.ApprovedDate
		FROM
			POData pod
			INNER JOIN #approvedOrders ao on pod.PONumber = ao.PONumber
		WHERE
			ao.ResolutionCode = ''''
			AND ao.ApprovedDate IS NOT NULL
			AND ao.InvoiceNumber IS NOT NULL

		COMMIT TRAN
END TRY
BEGIN CATCH
	IF @@TRANCOUNT > 0
    	ROLLBACK TRAN
    SELECT @err_no = ERROR_NUMBER(), @err_sev = ERROR_SEVERITY(), @err_msg = ERROR_MESSAGE()
	RAISERROR (''Updating the Suspended Status of orders that were closed as Other/None failed with error no: %d and message: %s'', @err_sev, 1, @err_no, @err_msg)
END CATCH', 
		@database_name=N'POReports', 
		@flags=0
IF (@@ERROR <> 0 OR @ReturnCode <> 0) GOTO QuitWithRollback
/****** Object:  Step [RM_Get Weekly Orders By Close Date]    Script Date: 11/6/2013 3:51:59 PM ******/
EXEC @ReturnCode = msdb.dbo.sp_add_jobstep @job_id=@jobId, @step_name=N'RM_Get Weekly Orders By Close Date', 
		@step_id=15, 
		@cmdexec_success_code=0, 
		@on_success_action=3, 
		@on_success_step_id=0, 
		@on_fail_action=4, 
		@on_fail_step_id=27, 
		@retry_attempts=0, 
		@retry_interval=0, 
		@os_run_priority=0, @subsystem=N'TSQL', 
		@command=N'-- RM
USE POReports;
GO

--**************************************************************************
-- Create the synonyms to the appropriate IRMA database
--**************************************************************************
if exists (select * from sysobjects where xtype = ''SN'' and name = ''OrderHeader'')		drop synonym OrderHeader;
if exists (select * from sysobjects where xtype = ''SN'' and name = ''OrderItem'')			drop synonym OrderItem;
if exists (select * from sysobjects where xtype = ''SN'' and name = ''Vendor'')				drop synonym Vendor;
if exists (select * from sysobjects where xtype = ''SN'' and name = ''Users'')				drop synonym Users;
if exists (select * from sysobjects where xtype = ''SN'' and name = ''Store'')				drop synonym Store;
if exists (select * from sysobjects where xtype = ''SN'' and name = ''SubTeam'')			drop synonym SubTeam;
if exists (select * from sysobjects where xtype = ''SN'' and name = ''ReasonCodeDetail'')	drop synonym ReasonCodeDetail;
if exists (select * from sysobjects where xtype = ''SN'' and name = ''Region'')				drop synonym Region;

if exists (select * from sys.servers where name = ''IDD-RM\RMD'')
	begin
		
        create synonym OrderHeader		for [IDD-RM\RMD].[ItemCatalog_Test].[dbo].OrderHeader;
		create synonym OrderItem		for [IDD-RM\RMD].[ItemCatalog_Test].[dbo].OrderItem;
        create synonym Vendor			for [IDD-RM\RMD].[ItemCatalog_Test].[dbo].Vendor;
		create synonym Users			for [IDD-RM\RMD].[ItemCatalog_Test].[dbo].Users;
		create synonym Store			for [IDD-RM\RMD].[ItemCatalog_Test].[dbo].Store;
		create synonym SubTeam			for [IDD-RM\RMD].[ItemCatalog_Test].[dbo].SubTeam;
		create synonym ReasonCodeDetail for [IDD-RM\RMD].[ItemCatalog_Test].[dbo].ReasonCodeDetail;
		create synonym Region			for [IDD-RM\RMD].[ItemCatalog_Test].[dbo].Region;
    end
else if exists (select * from sys.servers where name = ''IDQ-RM\RMQ'')
	begin
        create synonym OrderHeader		for [IDQ-RM\RMQ].[ItemCatalog].[dbo].OrderHeader;
		create synonym OrderItem		for [IDQ-RM\RMQ].[ItemCatalog].[dbo].OrderItem;
        create synonym Vendor			for [IDQ-RM\RMQ].[ItemCatalog].[dbo].Vendor;
		create synonym Users			for [IDQ-RM\RMQ].[ItemCatalog].[dbo].Users;
		create synonym Store			for [IDQ-RM\RMQ].[ItemCatalog].[dbo].Store;
		create synonym SubTeam			for [IDQ-RM\RMQ].[ItemCatalog].[dbo].SubTeam;
		create synonym ReasonCodeDetail for [IDQ-RM\RMQ].[ItemCatalog].[dbo].ReasonCodeDetail;
		create synonym Region			for [IDQ-RM\RMQ].[ItemCatalog].[dbo].Region;
	end    
else if exists (select * from sys.servers where name = ''IDP-RM\RMP'')
	begin
        create synonym OrderHeader		for [IDP-RM\RMP].[ItemCatalog].[dbo].OrderHeader;
		create synonym OrderItem		for [IDP-RM\RMP].[ItemCatalog].[dbo].OrderItem;
        create synonym Vendor			for [IDP-RM\RMP].[ItemCatalog].[dbo].Vendor;
		create synonym Users			for [IDP-RM\RMP].[ItemCatalog].[dbo].Users;
		create synonym Store			for [IDP-RM\RMP].[ItemCatalog].[dbo].Store;
		create synonym SubTeam			for [IDP-RM\RMP].[ItemCatalog].[dbo].SubTeam;
		create synonym ReasonCodeDetail for [IDP-RM\RMP].[ItemCatalog].[dbo].ReasonCodeDetail;
		create synonym Region			for [IDP-RM\RMP].[ItemCatalog].[dbo].Region;
	end

--**************************************************************************
-- Set and Populate internal variables
--**************************************************************************
DECLARE @startdate datetime;
DECLARE @enddate datetime;
DECLARE @region	varchar(2);
DECLARE @insertDate	datetime2;

-- Get Monday 00:00:00 of Last Week and Get Sunday 23:59:59.997 of Last Week
SELECT @startdate = DATEADD(day, -7 , DATEADD(wk, DATEDIFF(wk, 0, GETDATE()), 0))
SELECT @enddate = DATEADD(ms, -3, DATEADD(wk, DATEDIFF(wk, 0, GETDATE()), 0))

SELECT  @region			= RegionCode FROM Region;
SELECT	@insertDate		= GETDATE();

--**************************************************************************
-- Get List of Orders and Insert Into PODataLoad table
--**************************************************************************
BEGIN TRY
BEGIN TRAN

	-- Insert into PODataLoad table
	INSERT INTO PODataLoad
	SELECT 
		oh.OrderHeader_ID											as PONumber,
		CASE 
			WHEN oh.ResolutionCodeID IS NOT NULL AND oh.ApprovedDate IS NOT NULL THEN ''Y'' 
			WHEN oh.ApprovedDate IS NULL THEN ''Y''
			ELSE ''N'' 
		END															as Suspended,
		oh.CloseDate												as CloseDate,
		CASE
			WHEN oh.ResolutionCodeID IS NOT NULL AND oh.ApprovedDate IS NOT NULL THEN rcd.ReasonCodeDesc
			WHEN oh.ApprovedDate IS NULL AND oh.InvoiceNumber IS NOT NULL THEN ''Suspended but not Approved''
			WHEN oh.ApprovedDate IS NULL AND oh.InvoiceNumber IS NULL THEN ''Closed as Other/None''
			ELSE ''''
		END															as ResolutionCode,
		ISNULL(oh.AdminNotes, '''')									as AdminNotes,
		v.CompanyName												as Vendor,
		st.SubTeam_No												as Subteam,
		s.Store_Name												as Store,
		CASE 
			WHEN oh.DiscountType > 0 THEN ''Y''
			WHEN 0 < SUM(oi.DiscountType) Then ''Y''
			WHEN 0 < SUM(oi.AdjustedCost) Then ''Y''
			ELSE ''N''
		END															as AdjustedCost,
		CASE 
			WHEN oh.Return_Order = 1 THEN ''Y'' 
			ELSE ''N'' 
		END															as CreditPO,
		CASE 
			WHEN oh.PayByAgreedCost = 1 THEN ''Pay By Agreed Cost'' 
			ELSE ''Pay By Invoice'' 
		END															as VendorType,
		ucr.FullName												as POCreator,
		CASE 
			WHEN oh.Einvoice_id IS NOT NULL THEN ''Y'' 
			ELSE ''N'' 
		END															as EInvoiceMatchedToPO,
		ISNULL(oh.OrderHeaderDesc, '''')								as PONotes,
		ucl.FullName												as ClosedBy,
		@region														as Region,
		oh.ApprovedDate												as ApprovedDate,
		oh.InvoiceNumber											as InvoiceNumber,
		@insertDate													as InsertDate
	FROM
		OrderHeader					oh	(nolock)
		INNER JOIN OrderItem		oi	(nolock) on oh.OrderHeader_ID		= oi.OrderHeader_ID
		INNER JOIN Vendor			vr	(nolock) on oh.ReceiveLocation_ID	= vr.Vendor_ID
		INNER JOIN Vendor			v	(nolock) on oh.Vendor_ID			= v.Vendor_ID
		INNER JOIN Users			ucr (nolock) on oh.CreatedBy			= ucr.User_ID
		INNER JOIN Users			ucl	(nolock) on oh.ClosedBy				= ucl.User_ID
		INNER JOIN Store			s	(nolock) on vr.Store_No				= s.Store_No
		INNER JOIN SubTeam			st	(nolock) on oh.Transfer_To_SubTeam	= st.SubTeam_No
		LEFT JOIN ReasonCodeDetail	rcd (nolock) on oh.ResolutionCodeID		= rcd.ReasonCodeDetailID
	WHERE
		oh.CloseDate		>= @StartDate
		AND oh.CloseDate	<= @EndDate
		AND oh.OrderType_ID <> 3 -- filter out Transfer orders
	GROUP BY
		oh.OrderHeader_ID,
		oh.DiscountType,
		oh.Return_Order,
		oh.PayByAgreedCost,
		oh.eInvoice_Id,
		oh.ResolutionCodeID,
		oh.Vendor_ID,
		oh.ReceiveLocation_ID,
		oh.CreatedBy,
		oh.ClosedBy,
		oh.Transfer_To_SubTeam,
		oh.AdminNotes,
		oh.OrderHeaderDesc,
		oh.CloseDate,
		oh.ApprovedDate,
		oh.InvoiceNumber,
		rcd.ReasonCodeDesc,
		v.CompanyName,
		st.SubTeam_No,
		s.Store_Name,
		ucr.FullName,
		ucl.FullName

	PRINT ''Data has been loaded into PODataLoad table for the '' + @region + '' region.''
	
	-- Insert data into PODataLoadStatus
	INSERT INTO dbo.PODataLoadStatus (Region, InsertDate)
	SELECT @region, @insertDate;
	PRINT ''PODataLoadStatus table has been updated for InsertDate: '' + CAST(@insertDate as nvarchar) + '' and Region: '' + @region + ''.'';

	COMMIT TRAN

END TRY
BEGIN CATCH
	IF @@TRANCOUNT > 0
    	ROLLBACK TRAN
	DECLARE @err_no int, @err_sev int, @err_msg varchar(MAX)
    SELECT @err_no = ERROR_NUMBER(), @err_sev = ERROR_SEVERITY(), @err_msg = ERROR_MESSAGE()
	RAISERROR (''GetWeeklyPOData failed with error no: %d and message: %s'', @err_sev, 1, @err_no, @err_msg)
END CATCH', 
		@database_name=N'POReports', 
		@flags=0
IF (@@ERROR <> 0 OR @ReturnCode <> 0) GOTO QuitWithRollback
/****** Object:  Step [RM_Update Approved Orders]    Script Date: 11/6/2013 3:51:59 PM ******/
EXEC @ReturnCode = msdb.dbo.sp_add_jobstep @job_id=@jobId, @step_name=N'RM_Update Approved Orders', 
		@step_id=16, 
		@cmdexec_success_code=0, 
		@on_success_action=3, 
		@on_success_step_id=0, 
		@on_fail_action=4, 
		@on_fail_step_id=27, 
		@retry_attempts=0, 
		@retry_interval=0, 
		@os_run_priority=0, @subsystem=N'TSQL', 
		@command=N'-- RM
-- Update Approved Orders

USE POReports;
GO

--**************************************************************************
-- Create the synonyms to the appropriate IRMA database
--**************************************************************************
if exists (select * from sysobjects where xtype = ''SN'' and name = ''OrderHeader'')		drop synonym OrderHeader;
if exists (select * from sysobjects where xtype = ''SN'' and name = ''OrderItem'')			drop synonym OrderItem;
if exists (select * from sysobjects where xtype = ''SN'' and name = ''Vendor'')				drop synonym Vendor;
if exists (select * from sysobjects where xtype = ''SN'' and name = ''Users'')				drop synonym Users;
if exists (select * from sysobjects where xtype = ''SN'' and name = ''Store'')				drop synonym Store;
if exists (select * from sysobjects where xtype = ''SN'' and name = ''SubTeam'')			drop synonym SubTeam;
if exists (select * from sysobjects where xtype = ''SN'' and name = ''ReasonCodeDetail'')	drop synonym ReasonCodeDetail;
if exists (select * from sysobjects where xtype = ''SN'' and name = ''Region'')				drop synonym Region;

if exists (select * from sys.servers where name = ''IDD-RM\RMD'')
	begin
		
        create synonym OrderHeader		for [IDD-RM\RMD].[ItemCatalog_Test].[dbo].OrderHeader;
		create synonym OrderItem		for [IDD-RM\RMD].[ItemCatalog_Test].[dbo].OrderItem;
        create synonym Vendor			for [IDD-RM\RMD].[ItemCatalog_Test].[dbo].Vendor;
		create synonym Users			for [IDD-RM\RMD].[ItemCatalog_Test].[dbo].Users;
		create synonym Store			for [IDD-RM\RMD].[ItemCatalog_Test].[dbo].Store;
		create synonym SubTeam			for [IDD-RM\RMD].[ItemCatalog_Test].[dbo].SubTeam;
		create synonym ReasonCodeDetail for [IDD-RM\RMD].[ItemCatalog_Test].[dbo].ReasonCodeDetail;
		create synonym Region			for [IDD-RM\RMD].[ItemCatalog_Test].[dbo].Region;
    end
else if exists (select * from sys.servers where name = ''IDQ-RM\RMQ'')
	begin
        create synonym OrderHeader		for [IDQ-RM\RMQ].[ItemCatalog].[dbo].OrderHeader;
		create synonym OrderItem		for [IDQ-RM\RMQ].[ItemCatalog].[dbo].OrderItem;
        create synonym Vendor			for [IDQ-RM\RMQ].[ItemCatalog].[dbo].Vendor;
		create synonym Users			for [IDQ-RM\RMQ].[ItemCatalog].[dbo].Users;
		create synonym Store			for [IDQ-RM\RMQ].[ItemCatalog].[dbo].Store;
		create synonym SubTeam			for [IDQ-RM\RMQ].[ItemCatalog].[dbo].SubTeam;
		create synonym ReasonCodeDetail for [IDQ-RM\RMQ].[ItemCatalog].[dbo].ReasonCodeDetail;
		create synonym Region			for [IDQ-RM\RMQ].[ItemCatalog].[dbo].Region;
	end    
else if exists (select * from sys.servers where name = ''IDP-RM\RMP'')
	begin
        create synonym OrderHeader		for [IDP-RM\RMP].[ItemCatalog].[dbo].OrderHeader;
		create synonym OrderItem		for [IDP-RM\RMP].[ItemCatalog].[dbo].OrderItem;
        create synonym Vendor			for [IDP-RM\RMP].[ItemCatalog].[dbo].Vendor;
		create synonym Users			for [IDP-RM\RMP].[ItemCatalog].[dbo].Users;
		create synonym Store			for [IDP-RM\RMP].[ItemCatalog].[dbo].Store;
		create synonym SubTeam			for [IDP-RM\RMP].[ItemCatalog].[dbo].SubTeam;
		create synonym ReasonCodeDetail for [IDP-RM\RMP].[ItemCatalog].[dbo].ReasonCodeDetail;
		create synonym Region			for [IDP-RM\RMP].[ItemCatalog].[dbo].Region;
	end

--**************************************************************************
-- Set and Populate internal variables
--**************************************************************************
DECLARE @startdate datetime;
DECLARE @enddate datetime;
DECLARE @region	varchar(2);

-- Get Monday 00:00:00 of Last Week
SELECT @startdate = DATEADD(day, -7 , DATEADD(wk, DATEDIFF(wk, 0, GETDATE()), 0))

-- Get Sunday 23:59:59.997 of Last Week
SELECT @enddate = DATEADD(ms, -3, DATEADD(wk, DATEDIFF(wk, 0, GETDATE()), 0))

select @startdate, @enddate
SELECT  @region	= RegionCode FROM Region;

--**************************************************************************
-- Get list of orders that were approved during the previous week
-- This will be used for updating orders in POData
--**************************************************************************
SELECT
	oh.OrderHeader_ID				as PONumber,
	oh.ApprovedDate					as ApprovedDate,
	ISNULL(rcd.ReasonCodeDesc, '''')	as ResolutionCode,
	oh.InvoiceNumber				as InvoiceNumber
INTO #approvedOrders
FROM
	OrderHeader					oh	(nolock)
	LEFT JOIN ReasonCodeDetail	rcd (nolock) on oh.ResolutionCodeID	= rcd.ReasonCodeDetailID
WHERE
	oh.ApprovedDate		>= @startdate
	AND oh.ApprovedDate <= @enddate

create clustered index idx_c_ApprovedOrders_PONumber on #approvedOrders (PONumber)
create nonclustered index idx_ApprovedOrders_ResolutionCode on #approvedOrders (ResolutionCode)

--**************************************************************************
-- Update orders in POData that were ''Suspended but not Approved'' or 
-- orders that were closed as Other/None but are now approved
--**************************************************************************
-- Update orders with resolution code if there is one assigned for orders that have invoice info.
DECLARE @err_no int, @err_sev int, @err_msg varchar(MAX)

BEGIN TRY
	BEGIN TRAN
		UPDATE pod
		SET
			pod.ResolutionCode = ao.ResolutionCode,
			pod.ApprovedDate = ao.ApprovedDate
		FROM
			POData pod
			INNER JOIN #approvedOrders ao on pod.PONumber = ao.PONumber
		WHERE
			ao.ResolutionCode <> ''''
			AND ao.InvoiceNumber IS NOT NULL
			AND ao.ApprovedDate IS NOT NULL

	COMMIT TRAN
END TRY
BEGIN CATCH
	IF @@TRANCOUNT > 0
    	ROLLBACK TRAN
    SELECT @err_no = ERROR_NUMBER(), @err_sev = ERROR_SEVERITY(), @err_msg = ERROR_MESSAGE()
	RAISERROR (''Updating Newly Approved Orders with Resolution Code failed with error no: %d and message: %s'', @err_sev, 1, @err_no, @err_msg)
END CATCH

-- Update the suspended status for orders closed as Other/None that do not have resolution code but are now approved
BEGIN TRY
	BEGIN TRAN
		UPDATE pod
		SET
			pod.Suspended = ''N'',
			pod.ApprovedDate = ao.ApprovedDate
		FROM
			POData pod
			INNER JOIN #approvedOrders ao on pod.PONumber = ao.PONumber
		WHERE
			ao.ResolutionCode = ''''
			AND ao.ApprovedDate IS NOT NULL
			AND ao.InvoiceNumber IS NOT NULL

		COMMIT TRAN
END TRY
BEGIN CATCH
	IF @@TRANCOUNT > 0
    	ROLLBACK TRAN
    SELECT @err_no = ERROR_NUMBER(), @err_sev = ERROR_SEVERITY(), @err_msg = ERROR_MESSAGE()
	RAISERROR (''Updating the Suspended Status of orders that were closed as Other/None failed with error no: %d and message: %s'', @err_sev, 1, @err_no, @err_msg)
END CATCH', 
		@database_name=N'POReports', 
		@flags=0
IF (@@ERROR <> 0 OR @ReturnCode <> 0) GOTO QuitWithRollback
/****** Object:  Step [SO_Get Weekly Orders By Close Date]    Script Date: 11/6/2013 3:51:59 PM ******/
EXEC @ReturnCode = msdb.dbo.sp_add_jobstep @job_id=@jobId, @step_name=N'SO_Get Weekly Orders By Close Date', 
		@step_id=17, 
		@cmdexec_success_code=0, 
		@on_success_action=3, 
		@on_success_step_id=0, 
		@on_fail_action=4, 
		@on_fail_step_id=27, 
		@retry_attempts=0, 
		@retry_interval=0, 
		@os_run_priority=0, @subsystem=N'TSQL', 
		@command=N'-- SO
USE POReports;
GO

--**************************************************************************
-- Create the synonyms to the appropriate IRMA database
--**************************************************************************
if exists (select * from sysobjects where xtype = ''SN'' and name = ''OrderHeader'')		drop synonym OrderHeader;
if exists (select * from sysobjects where xtype = ''SN'' and name = ''OrderItem'')			drop synonym OrderItem;
if exists (select * from sysobjects where xtype = ''SN'' and name = ''Vendor'')				drop synonym Vendor;
if exists (select * from sysobjects where xtype = ''SN'' and name = ''Users'')				drop synonym Users;
if exists (select * from sysobjects where xtype = ''SN'' and name = ''Store'')				drop synonym Store;
if exists (select * from sysobjects where xtype = ''SN'' and name = ''SubTeam'')			drop synonym SubTeam;
if exists (select * from sysobjects where xtype = ''SN'' and name = ''ReasonCodeDetail'')	drop synonym ReasonCodeDetail;
if exists (select * from sysobjects where xtype = ''SN'' and name = ''Region'')				drop synonym Region;

if exists (select * from sys.servers where name = ''IDD-SO\SOD'')
	begin
		
        create synonym OrderHeader		for [IDD-SO\SOD].[ItemCatalog_Test].[dbo].OrderHeader;
		create synonym OrderItem		for [IDD-SO\SOD].[ItemCatalog_Test].[dbo].OrderItem;
        create synonym Vendor			for [IDD-SO\SOD].[ItemCatalog_Test].[dbo].Vendor;
		create synonym Users			for [IDD-SO\SOD].[ItemCatalog_Test].[dbo].Users;
		create synonym Store			for [IDD-SO\SOD].[ItemCatalog_Test].[dbo].Store;
		create synonym SubTeam			for [IDD-SO\SOD].[ItemCatalog_Test].[dbo].SubTeam;
		create synonym ReasonCodeDetail for [IDD-SO\SOD].[ItemCatalog_Test].[dbo].ReasonCodeDetail;
		create synonym Region			for [IDD-SO\SOD].[ItemCatalog_Test].[dbo].Region;
    end
else if exists (select * from sys.servers where name = ''IDQ-SO\SOQ'')
	begin
        create synonym OrderHeader		for [IDQ-SO\SOQ].[ItemCatalog].[dbo].OrderHeader;
		create synonym OrderItem		for [IDQ-SO\SOQ].[ItemCatalog].[dbo].OrderItem;
        create synonym Vendor			for [IDQ-SO\SOQ].[ItemCatalog].[dbo].Vendor;
		create synonym Users			for [IDQ-SO\SOQ].[ItemCatalog].[dbo].Users;
		create synonym Store			for [IDQ-SO\SOQ].[ItemCatalog].[dbo].Store;
		create synonym SubTeam			for [IDQ-SO\SOQ].[ItemCatalog].[dbo].SubTeam;
		create synonym ReasonCodeDetail for [IDQ-SO\SOQ].[ItemCatalog].[dbo].ReasonCodeDetail;
		create synonym Region			for [IDQ-SO\SOQ].[ItemCatalog].[dbo].Region;
	end    
else if exists (select * from sys.servers where name = ''IDP-SO\SOP'')
	begin
        create synonym OrderHeader		for [IDP-SO\SOP].[ItemCatalog].[dbo].OrderHeader;
		create synonym OrderItem		for [IDP-SO\SOP].[ItemCatalog].[dbo].OrderItem;
        create synonym Vendor			for [IDP-SO\SOP].[ItemCatalog].[dbo].Vendor;
		create synonym Users			for [IDP-SO\SOP].[ItemCatalog].[dbo].Users;
		create synonym Store			for [IDP-SO\SOP].[ItemCatalog].[dbo].Store;
		create synonym SubTeam			for [IDP-SO\SOP].[ItemCatalog].[dbo].SubTeam;
		create synonym ReasonCodeDetail for [IDP-SO\SOP].[ItemCatalog].[dbo].ReasonCodeDetail;
		create synonym Region			for [IDP-SO\SOP].[ItemCatalog].[dbo].Region;
	end

--**************************************************************************
-- Set and Populate internal variables
--**************************************************************************
DECLARE @startdate datetime;
DECLARE @enddate datetime;
DECLARE @region	varchar(2);
DECLARE @insertDate	datetime2;

-- Get Monday 00:00:00 of Last Week and Get Sunday 23:59:59.997 of Last Week
SELECT @startdate = DATEADD(day, -7 , DATEADD(wk, DATEDIFF(wk, 0, GETDATE()), 0))
SELECT @enddate = DATEADD(ms, -3, DATEADD(wk, DATEDIFF(wk, 0, GETDATE()), 0))

SELECT  @region			= RegionCode FROM Region;
SELECT	@insertDate		= GETDATE();

--**************************************************************************
-- Get List of Orders and Insert Into PODataLoad table
--**************************************************************************
BEGIN TRY
BEGIN TRAN

	-- Insert into PODataLoad table
	INSERT INTO PODataLoad
	SELECT 
		oh.OrderHeader_ID											as PONumber,
		CASE 
			WHEN oh.ResolutionCodeID IS NOT NULL AND oh.ApprovedDate IS NOT NULL THEN ''Y'' 
			WHEN oh.ApprovedDate IS NULL THEN ''Y''
			ELSE ''N'' 
		END															as Suspended,
		oh.CloseDate												as CloseDate,
		CASE
			WHEN oh.ResolutionCodeID IS NOT NULL AND oh.ApprovedDate IS NOT NULL THEN rcd.ReasonCodeDesc
			WHEN oh.ApprovedDate IS NULL AND oh.InvoiceNumber IS NOT NULL THEN ''Suspended but not Approved''
			WHEN oh.ApprovedDate IS NULL AND oh.InvoiceNumber IS NULL THEN ''Closed as Other/None''
			ELSE ''''
		END															as ResolutionCode,
		ISNULL(oh.AdminNotes, '''')									as AdminNotes,
		v.CompanyName												as Vendor,
		st.SubTeam_No												as Subteam,
		s.Store_Name												as Store,
		CASE 
			WHEN oh.DiscountType > 0 THEN ''Y''
			WHEN 0 < SUM(oi.DiscountType) Then ''Y''
			WHEN 0 < SUM(oi.AdjustedCost) Then ''Y''
			ELSE ''N''
		END															as AdjustedCost,
		CASE 
			WHEN oh.Return_Order = 1 THEN ''Y'' 
			ELSE ''N'' 
		END															as CreditPO,
		CASE 
			WHEN oh.PayByAgreedCost = 1 THEN ''Pay By Agreed Cost'' 
			ELSE ''Pay By Invoice'' 
		END															as VendorType,
		ucr.FullName												as POCreator,
		CASE 
			WHEN oh.Einvoice_id IS NOT NULL THEN ''Y'' 
			ELSE ''N'' 
		END															as EInvoiceMatchedToPO,
		ISNULL(oh.OrderHeaderDesc, '''')								as PONotes,
		ucl.FullName												as ClosedBy,
		@region														as Region,
		oh.ApprovedDate												as ApprovedDate,
		oh.InvoiceNumber											as InvoiceNumber,
		@insertDate													as InsertDate
	FROM
		OrderHeader					oh	(nolock)
		INNER JOIN OrderItem		oi	(nolock) on oh.OrderHeader_ID		= oi.OrderHeader_ID
		INNER JOIN Vendor			vr	(nolock) on oh.ReceiveLocation_ID	= vr.Vendor_ID
		INNER JOIN Vendor			v	(nolock) on oh.Vendor_ID			= v.Vendor_ID
		INNER JOIN Users			ucr (nolock) on oh.CreatedBy			= ucr.User_ID
		INNER JOIN Users			ucl	(nolock) on oh.ClosedBy				= ucl.User_ID
		INNER JOIN Store			s	(nolock) on vr.Store_No				= s.Store_No
		INNER JOIN SubTeam			st	(nolock) on oh.Transfer_To_SubTeam	= st.SubTeam_No
		LEFT JOIN ReasonCodeDetail	rcd (nolock) on oh.ResolutionCodeID		= rcd.ReasonCodeDetailID
	WHERE
		oh.CloseDate		>= @StartDate
		AND oh.CloseDate	<= @EndDate
		AND oh.OrderType_ID <> 3 -- filter out Transfer orders
	GROUP BY
		oh.OrderHeader_ID,
		oh.DiscountType,
		oh.Return_Order,
		oh.PayByAgreedCost,
		oh.eInvoice_Id,
		oh.ResolutionCodeID,
		oh.Vendor_ID,
		oh.ReceiveLocation_ID,
		oh.CreatedBy,
		oh.ClosedBy,
		oh.Transfer_To_SubTeam,
		oh.AdminNotes,
		oh.OrderHeaderDesc,
		oh.CloseDate,
		oh.ApprovedDate,
		oh.InvoiceNumber,
		rcd.ReasonCodeDesc,
		v.CompanyName,
		st.SubTeam_No,
		s.Store_Name,
		ucr.FullName,
		ucl.FullName

	PRINT ''Data has been loaded into PODataLoad table for the '' + @region + '' region.''
	
	-- Insert data into PODataLoadStatus
	INSERT INTO dbo.PODataLoadStatus (Region, InsertDate)
	SELECT @region, @insertDate;
	PRINT ''PODataLoadStatus table has been updated for InsertDate: '' + CAST(@insertDate as nvarchar) + '' and Region: '' + @region + ''.'';

	COMMIT TRAN

END TRY
BEGIN CATCH
	IF @@TRANCOUNT > 0
    	ROLLBACK TRAN
	DECLARE @err_no int, @err_sev int, @err_msg varchar(MAX)
    SELECT @err_no = ERROR_NUMBER(), @err_sev = ERROR_SEVERITY(), @err_msg = ERROR_MESSAGE()
	RAISERROR (''GetWeeklyPOData failed with error no: %d and message: %s'', @err_sev, 1, @err_no, @err_msg)
END CATCH', 
		@database_name=N'POReports', 
		@flags=0
IF (@@ERROR <> 0 OR @ReturnCode <> 0) GOTO QuitWithRollback
/****** Object:  Step [SO_Update Approved Orders]    Script Date: 11/6/2013 3:51:59 PM ******/
EXEC @ReturnCode = msdb.dbo.sp_add_jobstep @job_id=@jobId, @step_name=N'SO_Update Approved Orders', 
		@step_id=18, 
		@cmdexec_success_code=0, 
		@on_success_action=3, 
		@on_success_step_id=0, 
		@on_fail_action=4, 
		@on_fail_step_id=27, 
		@retry_attempts=0, 
		@retry_interval=0, 
		@os_run_priority=0, @subsystem=N'TSQL', 
		@command=N'-- SO
-- Update Approved Orders

USE POReports;
GO

--**************************************************************************
-- Create the synonyms to the appropriate IRMA database
--**************************************************************************
if exists (select * from sysobjects where xtype = ''SN'' and name = ''OrderHeader'')		drop synonym OrderHeader;
if exists (select * from sysobjects where xtype = ''SN'' and name = ''OrderItem'')			drop synonym OrderItem;
if exists (select * from sysobjects where xtype = ''SN'' and name = ''Vendor'')				drop synonym Vendor;
if exists (select * from sysobjects where xtype = ''SN'' and name = ''Users'')				drop synonym Users;
if exists (select * from sysobjects where xtype = ''SN'' and name = ''Store'')				drop synonym Store;
if exists (select * from sysobjects where xtype = ''SN'' and name = ''SubTeam'')			drop synonym SubTeam;
if exists (select * from sysobjects where xtype = ''SN'' and name = ''ReasonCodeDetail'')	drop synonym ReasonCodeDetail;
if exists (select * from sysobjects where xtype = ''SN'' and name = ''Region'')				drop synonym Region;

if exists (select * from sys.servers where name = ''IDD-SO\SOD'')
	begin
		
        create synonym OrderHeader		for [IDD-SO\SOD].[ItemCatalog_Test].[dbo].OrderHeader;
		create synonym OrderItem		for [IDD-SO\SOD].[ItemCatalog_Test].[dbo].OrderItem;
        create synonym Vendor			for [IDD-SO\SOD].[ItemCatalog_Test].[dbo].Vendor;
		create synonym Users			for [IDD-SO\SOD].[ItemCatalog_Test].[dbo].Users;
		create synonym Store			for [IDD-SO\SOD].[ItemCatalog_Test].[dbo].Store;
		create synonym SubTeam			for [IDD-SO\SOD].[ItemCatalog_Test].[dbo].SubTeam;
		create synonym ReasonCodeDetail for [IDD-SO\SOD].[ItemCatalog_Test].[dbo].ReasonCodeDetail;
		create synonym Region			for [IDD-SO\SOD].[ItemCatalog_Test].[dbo].Region;
    end
else if exists (select * from sys.servers where name = ''IDQ-SO\SOQ'')
	begin
        create synonym OrderHeader		for [IDQ-SO\SOQ].[ItemCatalog].[dbo].OrderHeader;
		create synonym OrderItem		for [IDQ-SO\SOQ].[ItemCatalog].[dbo].OrderItem;
        create synonym Vendor			for [IDQ-SO\SOQ].[ItemCatalog].[dbo].Vendor;
		create synonym Users			for [IDQ-SO\SOQ].[ItemCatalog].[dbo].Users;
		create synonym Store			for [IDQ-SO\SOQ].[ItemCatalog].[dbo].Store;
		create synonym SubTeam			for [IDQ-SO\SOQ].[ItemCatalog].[dbo].SubTeam;
		create synonym ReasonCodeDetail for [IDQ-SO\SOQ].[ItemCatalog].[dbo].ReasonCodeDetail;
		create synonym Region			for [IDQ-SO\SOQ].[ItemCatalog].[dbo].Region;
	end    
else if exists (select * from sys.servers where name = ''IDP-SO\SOP'')
	begin
        create synonym OrderHeader		for [IDP-SO\SOP].[ItemCatalog].[dbo].OrderHeader;
		create synonym OrderItem		for [IDP-SO\SOP].[ItemCatalog].[dbo].OrderItem;
        create synonym Vendor			for [IDP-SO\SOP].[ItemCatalog].[dbo].Vendor;
		create synonym Users			for [IDP-SO\SOP].[ItemCatalog].[dbo].Users;
		create synonym Store			for [IDP-SO\SOP].[ItemCatalog].[dbo].Store;
		create synonym SubTeam			for [IDP-SO\SOP].[ItemCatalog].[dbo].SubTeam;
		create synonym ReasonCodeDetail for [IDP-SO\SOP].[ItemCatalog].[dbo].ReasonCodeDetail;
		create synonym Region			for [IDP-SO\SOP].[ItemCatalog].[dbo].Region;
	end

--**************************************************************************
-- Set and Populate internal variables
--**************************************************************************
DECLARE @startdate datetime;
DECLARE @enddate datetime;
DECLARE @region	varchar(2);

-- Get Monday 00:00:00 of Last Week
SELECT @startdate = DATEADD(day, -7 , DATEADD(wk, DATEDIFF(wk, 0, GETDATE()), 0))

-- Get Sunday 23:59:59.997 of Last Week
SELECT @enddate = DATEADD(ms, -3, DATEADD(wk, DATEDIFF(wk, 0, GETDATE()), 0))

select @startdate, @enddate
SELECT  @region	= RegionCode FROM Region;

--**************************************************************************
-- Get list of orders that were approved during the previous week
-- This will be used for updating orders in POData
--**************************************************************************
SELECT
	oh.OrderHeader_ID				as PONumber,
	oh.ApprovedDate					as ApprovedDate,
	ISNULL(rcd.ReasonCodeDesc, '''')	as ResolutionCode,
	oh.InvoiceNumber				as InvoiceNumber
INTO #approvedOrders
FROM
	OrderHeader					oh	(nolock)
	LEFT JOIN ReasonCodeDetail	rcd (nolock) on oh.ResolutionCodeID	= rcd.ReasonCodeDetailID
WHERE
	oh.ApprovedDate		>= @startdate
	AND oh.ApprovedDate <= @enddate

create clustered index idx_c_ApprovedOrders_PONumber on #approvedOrders (PONumber)
create nonclustered index idx_ApprovedOrders_ResolutionCode on #approvedOrders (ResolutionCode)

--**************************************************************************
-- Update orders in POData that were ''Suspended but not Approved'' or 
-- orders that were closed as Other/None but are now approved
--**************************************************************************
-- Update orders with resolution code if there is one assigned for orders that have invoice info.
DECLARE @err_no int, @err_sev int, @err_msg varchar(MAX)

BEGIN TRY
	BEGIN TRAN
		UPDATE pod
		SET
			pod.ResolutionCode = ao.ResolutionCode,
			pod.ApprovedDate = ao.ApprovedDate
		FROM
			POData pod
			INNER JOIN #approvedOrders ao on pod.PONumber = ao.PONumber
		WHERE
			ao.ResolutionCode <> ''''
			AND ao.InvoiceNumber IS NOT NULL
			AND ao.ApprovedDate IS NOT NULL

	COMMIT TRAN
END TRY
BEGIN CATCH
	IF @@TRANCOUNT > 0
    	ROLLBACK TRAN
    SELECT @err_no = ERROR_NUMBER(), @err_sev = ERROR_SEVERITY(), @err_msg = ERROR_MESSAGE()
	RAISERROR (''Updating Newly Approved Orders with Resolution Code failed with error no: %d and message: %s'', @err_sev, 1, @err_no, @err_msg)
END CATCH

-- Update the suspended status for orders closed as Other/None that do not have resolution code but are now approved
BEGIN TRY
	BEGIN TRAN
		UPDATE pod
		SET
			pod.Suspended = ''N'',
			pod.ApprovedDate = ao.ApprovedDate
		FROM
			POData pod
			INNER JOIN #approvedOrders ao on pod.PONumber = ao.PONumber
		WHERE
			ao.ResolutionCode = ''''
			AND ao.ApprovedDate IS NOT NULL
			AND ao.InvoiceNumber IS NOT NULL

		COMMIT TRAN
END TRY
BEGIN CATCH
	IF @@TRANCOUNT > 0
    	ROLLBACK TRAN
    SELECT @err_no = ERROR_NUMBER(), @err_sev = ERROR_SEVERITY(), @err_msg = ERROR_MESSAGE()
	RAISERROR (''Updating the Suspended Status of orders that were closed as Other/None failed with error no: %d and message: %s'', @err_sev, 1, @err_no, @err_msg)
END CATCH', 
		@database_name=N'POReports', 
		@flags=0
IF (@@ERROR <> 0 OR @ReturnCode <> 0) GOTO QuitWithRollback
/****** Object:  Step [SP_Get Weekly Orders By Close Date]    Script Date: 11/6/2013 3:51:59 PM ******/
EXEC @ReturnCode = msdb.dbo.sp_add_jobstep @job_id=@jobId, @step_name=N'SP_Get Weekly Orders By Close Date', 
		@step_id=19, 
		@cmdexec_success_code=0, 
		@on_success_action=3, 
		@on_success_step_id=0, 
		@on_fail_action=4, 
		@on_fail_step_id=27, 
		@retry_attempts=0, 
		@retry_interval=0, 
		@os_run_priority=0, @subsystem=N'TSQL', 
		@command=N'-- SP
USE POReports;
GO

--**************************************************************************
-- Create the synonyms to the appropriate IRMA database
--**************************************************************************
if exists (select * from sysobjects where xtype = ''SN'' and name = ''OrderHeader'')		drop synonym OrderHeader;
if exists (select * from sysobjects where xtype = ''SN'' and name = ''OrderItem'')			drop synonym OrderItem;
if exists (select * from sysobjects where xtype = ''SN'' and name = ''Vendor'')				drop synonym Vendor;
if exists (select * from sysobjects where xtype = ''SN'' and name = ''Users'')				drop synonym Users;
if exists (select * from sysobjects where xtype = ''SN'' and name = ''Store'')				drop synonym Store;
if exists (select * from sysobjects where xtype = ''SN'' and name = ''SubTeam'')			drop synonym SubTeam;
if exists (select * from sysobjects where xtype = ''SN'' and name = ''ReasonCodeDetail'')	drop synonym ReasonCodeDetail;
if exists (select * from sysobjects where xtype = ''SN'' and name = ''Region'')				drop synonym Region;

if exists (select * from sys.servers where name = ''IDT-SP\SPT'')
	begin
		
        create synonym OrderHeader		for [IDT-SP\SPT].[ItemCatalog_Test].[dbo].OrderHeader;
		create synonym OrderItem		for [IDT-SP\SPT].[ItemCatalog_Test].[dbo].OrderItem;
        create synonym Vendor			for [IDT-SP\SPT].[ItemCatalog_Test].[dbo].Vendor;
		create synonym Users			for [IDT-SP\SPT].[ItemCatalog_Test].[dbo].Users;
		create synonym Store			for [IDT-SP\SPT].[ItemCatalog_Test].[dbo].Store;
		create synonym SubTeam			for [IDT-SP\SPT].[ItemCatalog_Test].[dbo].SubTeam;
		create synonym ReasonCodeDetail for [IDT-SP\SPT].[ItemCatalog_Test].[dbo].ReasonCodeDetail;
		create synonym Region			for [IDT-SP\SPT].[ItemCatalog_Test].[dbo].Region;
    end
else if exists (select * from sys.servers where name = ''IDQ-SP\SPQ'')
	begin
        create synonym OrderHeader		for [IDQ-SP\SPQ].[ItemCatalog].[dbo].OrderHeader;
		create synonym OrderItem		for [IDQ-SP\SPQ].[ItemCatalog].[dbo].OrderItem;
        create synonym Vendor			for [IDQ-SP\SPQ].[ItemCatalog].[dbo].Vendor;
		create synonym Users			for [IDQ-SP\SPQ].[ItemCatalog].[dbo].Users;
		create synonym Store			for [IDQ-SP\SPQ].[ItemCatalog].[dbo].Store;
		create synonym SubTeam			for [IDQ-SP\SPQ].[ItemCatalog].[dbo].SubTeam;
		create synonym ReasonCodeDetail for [IDQ-SP\SPQ].[ItemCatalog].[dbo].ReasonCodeDetail;
		create synonym Region			for [IDQ-SP\SPQ].[ItemCatalog].[dbo].Region;
	end    
else if exists (select * from sys.servers where name = ''IDP-SP\SPP'')
	begin
        create synonym OrderHeader		for [IDP-SP\SPP].[ItemCatalog].[dbo].OrderHeader;
		create synonym OrderItem		for [IDP-SP\SPP].[ItemCatalog].[dbo].OrderItem;
        create synonym Vendor			for [IDP-SP\SPP].[ItemCatalog].[dbo].Vendor;
		create synonym Users			for [IDP-SP\SPP].[ItemCatalog].[dbo].Users;
		create synonym Store			for [IDP-SP\SPP].[ItemCatalog].[dbo].Store;
		create synonym SubTeam			for [IDP-SP\SPP].[ItemCatalog].[dbo].SubTeam;
		create synonym ReasonCodeDetail for [IDP-SP\SPP].[ItemCatalog].[dbo].ReasonCodeDetail;
		create synonym Region			for [IDP-SP\SPP].[ItemCatalog].[dbo].Region;
	end

--**************************************************************************
-- Set and Populate internal variables
--**************************************************************************
DECLARE @startdate datetime;
DECLARE @enddate datetime;
DECLARE @region	varchar(2);
DECLARE @insertDate	datetime2;

-- Get Monday 00:00:00 of Last Week and Get Sunday 23:59:59.997 of Last Week
SELECT @startdate = DATEADD(day, -7 , DATEADD(wk, DATEDIFF(wk, 0, GETDATE()), 0))
SELECT @enddate = DATEADD(ms, -3, DATEADD(wk, DATEDIFF(wk, 0, GETDATE()), 0))

SELECT  @region			= RegionCode FROM Region;
SELECT	@insertDate		= GETDATE();

--**************************************************************************
-- Get List of Orders and Insert Into PODataLoad table
--**************************************************************************
BEGIN TRY
BEGIN TRAN

	-- Insert into PODataLoad table
	INSERT INTO PODataLoad
	SELECT 
		oh.OrderHeader_ID											as PONumber,
		CASE 
			WHEN oh.ResolutionCodeID IS NOT NULL AND oh.ApprovedDate IS NOT NULL THEN ''Y'' 
			WHEN oh.ApprovedDate IS NULL THEN ''Y''
			ELSE ''N'' 
		END															as Suspended,
		oh.CloseDate												as CloseDate,
		CASE
			WHEN oh.ResolutionCodeID IS NOT NULL AND oh.ApprovedDate IS NOT NULL THEN rcd.ReasonCodeDesc
			WHEN oh.ApprovedDate IS NULL AND oh.InvoiceNumber IS NOT NULL THEN ''Suspended but not Approved''
			WHEN oh.ApprovedDate IS NULL AND oh.InvoiceNumber IS NULL THEN ''Closed as Other/None''
			ELSE ''''
		END															as ResolutionCode,
		ISNULL(oh.AdminNotes, '''')									as AdminNotes,
		v.CompanyName												as Vendor,
		st.SubTeam_No												as Subteam,
		s.Store_Name												as Store,
		CASE 
			WHEN oh.DiscountType > 0 THEN ''Y''
			WHEN 0 < SUM(oi.DiscountType) Then ''Y''
			WHEN 0 < SUM(oi.AdjustedCost) Then ''Y''
			ELSE ''N''
		END															as AdjustedCost,
		CASE 
			WHEN oh.Return_Order = 1 THEN ''Y'' 
			ELSE ''N'' 
		END															as CreditPO,
		CASE 
			WHEN oh.PayByAgreedCost = 1 THEN ''Pay By Agreed Cost'' 
			ELSE ''Pay By Invoice'' 
		END															as VendorType,
		ucr.FullName												as POCreator,
		CASE 
			WHEN oh.Einvoice_id IS NOT NULL THEN ''Y'' 
			ELSE ''N'' 
		END															as EInvoiceMatchedToPO,
		ISNULL(oh.OrderHeaderDesc, '''')								as PONotes,
		ucl.FullName												as ClosedBy,
		@region														as Region,
		oh.ApprovedDate												as ApprovedDate,
		oh.InvoiceNumber											as InvoiceNumber,
		@insertDate													as InsertDate
	FROM
		OrderHeader					oh	(nolock)
		INNER JOIN OrderItem		oi	(nolock) on oh.OrderHeader_ID		= oi.OrderHeader_ID
		INNER JOIN Vendor			vr	(nolock) on oh.ReceiveLocation_ID	= vr.Vendor_ID
		INNER JOIN Vendor			v	(nolock) on oh.Vendor_ID			= v.Vendor_ID
		INNER JOIN Users			ucr (nolock) on oh.CreatedBy			= ucr.User_ID
		INNER JOIN Users			ucl	(nolock) on oh.ClosedBy				= ucl.User_ID
		INNER JOIN Store			s	(nolock) on vr.Store_No				= s.Store_No
		INNER JOIN SubTeam			st	(nolock) on oh.Transfer_To_SubTeam	= st.SubTeam_No
		LEFT JOIN ReasonCodeDetail	rcd (nolock) on oh.ResolutionCodeID		= rcd.ReasonCodeDetailID
	WHERE
		oh.CloseDate		>= @StartDate
		AND oh.CloseDate	<= @EndDate
		AND oh.OrderType_ID <> 3 -- filter out Transfer orders
	GROUP BY
		oh.OrderHeader_ID,
		oh.DiscountType,
		oh.Return_Order,
		oh.PayByAgreedCost,
		oh.eInvoice_Id,
		oh.ResolutionCodeID,
		oh.Vendor_ID,
		oh.ReceiveLocation_ID,
		oh.CreatedBy,
		oh.ClosedBy,
		oh.Transfer_To_SubTeam,
		oh.AdminNotes,
		oh.OrderHeaderDesc,
		oh.CloseDate,
		oh.ApprovedDate,
		oh.InvoiceNumber,
		rcd.ReasonCodeDesc,
		v.CompanyName,
		st.SubTeam_No,
		s.Store_Name,
		ucr.FullName,
		ucl.FullName

	PRINT ''Data has been loaded into PODataLoad table for the '' + @region + '' region.''
	
	-- Insert data into PODataLoadStatus
	INSERT INTO dbo.PODataLoadStatus (Region, InsertDate)
	SELECT @region, @insertDate;
	PRINT ''PODataLoadStatus table has been updated for InsertDate: '' + CAST(@insertDate as nvarchar) + '' and Region: '' + @region + ''.'';

	COMMIT TRAN

END TRY
BEGIN CATCH
	IF @@TRANCOUNT > 0
    	ROLLBACK TRAN
	DECLARE @err_no int, @err_sev int, @err_msg varchar(MAX)
    SELECT @err_no = ERROR_NUMBER(), @err_sev = ERROR_SEVERITY(), @err_msg = ERROR_MESSAGE()
	RAISERROR (''GetWeeklyPOData failed with error no: %d and message: %s'', @err_sev, 1, @err_no, @err_msg)
END CATCH', 
		@database_name=N'POReports', 
		@flags=0
IF (@@ERROR <> 0 OR @ReturnCode <> 0) GOTO QuitWithRollback
/****** Object:  Step [SP_Update Approved Orders]    Script Date: 11/6/2013 3:51:59 PM ******/
EXEC @ReturnCode = msdb.dbo.sp_add_jobstep @job_id=@jobId, @step_name=N'SP_Update Approved Orders', 
		@step_id=20, 
		@cmdexec_success_code=0, 
		@on_success_action=3, 
		@on_success_step_id=0, 
		@on_fail_action=4, 
		@on_fail_step_id=27, 
		@retry_attempts=0, 
		@retry_interval=0, 
		@os_run_priority=0, @subsystem=N'TSQL', 
		@command=N'-- SP
-- Update Approved Orders

USE POReports;
GO

--**************************************************************************
-- Create the synonyms to the appropriate IRMA database
--**************************************************************************
if exists (select * from sysobjects where xtype = ''SN'' and name = ''OrderHeader'')		drop synonym OrderHeader;
if exists (select * from sysobjects where xtype = ''SN'' and name = ''OrderItem'')			drop synonym OrderItem;
if exists (select * from sysobjects where xtype = ''SN'' and name = ''Vendor'')				drop synonym Vendor;
if exists (select * from sysobjects where xtype = ''SN'' and name = ''Users'')				drop synonym Users;
if exists (select * from sysobjects where xtype = ''SN'' and name = ''Store'')				drop synonym Store;
if exists (select * from sysobjects where xtype = ''SN'' and name = ''SubTeam'')			drop synonym SubTeam;
if exists (select * from sysobjects where xtype = ''SN'' and name = ''ReasonCodeDetail'')	drop synonym ReasonCodeDetail;
if exists (select * from sysobjects where xtype = ''SN'' and name = ''Region'')				drop synonym Region;

if exists (select * from sys.servers where name = ''IDT-SP\SPT'')
	begin
		
        create synonym OrderHeader		for [IDT-SP\SPT].[ItemCatalog_Test].[dbo].OrderHeader;
		create synonym OrderItem		for [IDT-SP\SPT].[ItemCatalog_Test].[dbo].OrderItem;
        create synonym Vendor			for [IDT-SP\SPT].[ItemCatalog_Test].[dbo].Vendor;
		create synonym Users			for [IDT-SP\SPT].[ItemCatalog_Test].[dbo].Users;
		create synonym Store			for [IDT-SP\SPT].[ItemCatalog_Test].[dbo].Store;
		create synonym SubTeam			for [IDT-SP\SPT].[ItemCatalog_Test].[dbo].SubTeam;
		create synonym ReasonCodeDetail for [IDT-SP\SPT].[ItemCatalog_Test].[dbo].ReasonCodeDetail;
		create synonym Region			for [IDT-SP\SPT].[ItemCatalog_Test].[dbo].Region;
    end
else if exists (select * from sys.servers where name = ''IDQ-SP\SPQ'')
	begin
        create synonym OrderHeader		for [IDQ-SP\SPQ].[ItemCatalog].[dbo].OrderHeader;
		create synonym OrderItem		for [IDQ-SP\SPQ].[ItemCatalog].[dbo].OrderItem;
        create synonym Vendor			for [IDQ-SP\SPQ].[ItemCatalog].[dbo].Vendor;
		create synonym Users			for [IDQ-SP\SPQ].[ItemCatalog].[dbo].Users;
		create synonym Store			for [IDQ-SP\SPQ].[ItemCatalog].[dbo].Store;
		create synonym SubTeam			for [IDQ-SP\SPQ].[ItemCatalog].[dbo].SubTeam;
		create synonym ReasonCodeDetail for [IDQ-SP\SPQ].[ItemCatalog].[dbo].ReasonCodeDetail;
		create synonym Region			for [IDQ-SP\SPQ].[ItemCatalog].[dbo].Region;
	end    
else if exists (select * from sys.servers where name = ''IDP-SP\SPP'')
	begin
        create synonym OrderHeader		for [IDP-SP\SPP].[ItemCatalog].[dbo].OrderHeader;
		create synonym OrderItem		for [IDP-SP\SPP].[ItemCatalog].[dbo].OrderItem;
        create synonym Vendor			for [IDP-SP\SPP].[ItemCatalog].[dbo].Vendor;
		create synonym Users			for [IDP-SP\SPP].[ItemCatalog].[dbo].Users;
		create synonym Store			for [IDP-SP\SPP].[ItemCatalog].[dbo].Store;
		create synonym SubTeam			for [IDP-SP\SPP].[ItemCatalog].[dbo].SubTeam;
		create synonym ReasonCodeDetail for [IDP-SP\SPP].[ItemCatalog].[dbo].ReasonCodeDetail;
		create synonym Region			for [IDP-SP\SPP].[ItemCatalog].[dbo].Region;
	end

--**************************************************************************
-- Set and Populate internal variables
--**************************************************************************
DECLARE @startdate datetime;
DECLARE @enddate datetime;
DECLARE @region	varchar(2);

-- Get Monday 00:00:00 of Last Week
SELECT @startdate = DATEADD(day, -7 , DATEADD(wk, DATEDIFF(wk, 0, GETDATE()), 0))

-- Get Sunday 23:59:59.997 of Last Week
SELECT @enddate = DATEADD(ms, -3, DATEADD(wk, DATEDIFF(wk, 0, GETDATE()), 0))

select @startdate, @enddate
SELECT  @region	= RegionCode FROM Region;

--**************************************************************************
-- Get list of orders that were approved during the previous week
-- This will be used for updating orders in POData
--**************************************************************************
SELECT
	oh.OrderHeader_ID				as PONumber,
	oh.ApprovedDate					as ApprovedDate,
	ISNULL(rcd.ReasonCodeDesc, '''')	as ResolutionCode,
	oh.InvoiceNumber				as InvoiceNumber
INTO #approvedOrders
FROM
	OrderHeader					oh	(nolock)
	LEFT JOIN ReasonCodeDetail	rcd (nolock) on oh.ResolutionCodeID	= rcd.ReasonCodeDetailID
WHERE
	oh.ApprovedDate		>= @startdate
	AND oh.ApprovedDate <= @enddate

create clustered index idx_c_ApprovedOrders_PONumber on #approvedOrders (PONumber)
create nonclustered index idx_ApprovedOrders_ResolutionCode on #approvedOrders (ResolutionCode)

--**************************************************************************
-- Update orders in POData that were ''Suspended but not Approved'' or 
-- orders that were closed as Other/None but are now approved
--**************************************************************************
-- Update orders with resolution code if there is one assigned for orders that have invoice info.
DECLARE @err_no int, @err_sev int, @err_msg varchar(MAX)

BEGIN TRY
	BEGIN TRAN
		UPDATE pod
		SET
			pod.ResolutionCode = ao.ResolutionCode,
			pod.ApprovedDate = ao.ApprovedDate
		FROM
			POData pod
			INNER JOIN #approvedOrders ao on pod.PONumber = ao.PONumber
		WHERE
			ao.ResolutionCode <> ''''
			AND ao.InvoiceNumber IS NOT NULL
			AND ao.ApprovedDate IS NOT NULL

	COMMIT TRAN
END TRY
BEGIN CATCH
	IF @@TRANCOUNT > 0
    	ROLLBACK TRAN
    SELECT @err_no = ERROR_NUMBER(), @err_sev = ERROR_SEVERITY(), @err_msg = ERROR_MESSAGE()
	RAISERROR (''Updating Newly Approved Orders with Resolution Code failed with error no: %d and message: %s'', @err_sev, 1, @err_no, @err_msg)
END CATCH

-- Update the suspended status for orders closed as Other/None that do not have resolution code but are now approved
BEGIN TRY
	BEGIN TRAN
		UPDATE pod
		SET
			pod.Suspended = ''N'',
			pod.ApprovedDate = ao.ApprovedDate
		FROM
			POData pod
			INNER JOIN #approvedOrders ao on pod.PONumber = ao.PONumber
		WHERE
			ao.ResolutionCode = ''''
			AND ao.ApprovedDate IS NOT NULL
			AND ao.InvoiceNumber IS NOT NULL

		COMMIT TRAN
END TRY
BEGIN CATCH
	IF @@TRANCOUNT > 0
    	ROLLBACK TRAN
    SELECT @err_no = ERROR_NUMBER(), @err_sev = ERROR_SEVERITY(), @err_msg = ERROR_MESSAGE()
	RAISERROR (''Updating the Suspended Status of orders that were closed as Other/None failed with error no: %d and message: %s'', @err_sev, 1, @err_no, @err_msg)
END CATCH', 
		@database_name=N'POReports', 
		@flags=0
IF (@@ERROR <> 0 OR @ReturnCode <> 0) GOTO QuitWithRollback
/****** Object:  Step [SW_Get Weekly Orders By Close Date]    Script Date: 11/6/2013 3:51:59 PM ******/
EXEC @ReturnCode = msdb.dbo.sp_add_jobstep @job_id=@jobId, @step_name=N'SW_Get Weekly Orders By Close Date', 
		@step_id=21, 
		@cmdexec_success_code=0, 
		@on_success_action=3, 
		@on_success_step_id=0, 
		@on_fail_action=4, 
		@on_fail_step_id=27, 
		@retry_attempts=0, 
		@retry_interval=0, 
		@os_run_priority=0, @subsystem=N'TSQL', 
		@command=N'-- SW
USE POReports;
GO

--**************************************************************************
-- Create the synonyms to the appropriate IRMA database
--**************************************************************************
if exists (select * from sysobjects where xtype = ''SN'' and name = ''OrderHeader'')		drop synonym OrderHeader;
if exists (select * from sysobjects where xtype = ''SN'' and name = ''OrderItem'')			drop synonym OrderItem;
if exists (select * from sysobjects where xtype = ''SN'' and name = ''Vendor'')				drop synonym Vendor;
if exists (select * from sysobjects where xtype = ''SN'' and name = ''Users'')				drop synonym Users;
if exists (select * from sysobjects where xtype = ''SN'' and name = ''Store'')				drop synonym Store;
if exists (select * from sysobjects where xtype = ''SN'' and name = ''SubTeam'')			drop synonym SubTeam;
if exists (select * from sysobjects where xtype = ''SN'' and name = ''ReasonCodeDetail'')	drop synonym ReasonCodeDetail;
if exists (select * from sysobjects where xtype = ''SN'' and name = ''Region'')				drop synonym Region;

if exists (select * from sys.servers where name = ''IDT-SW\SWT'')
	begin
		
        create synonym OrderHeader		for [IDT-SW\SWT].[ItemCatalog_Test].[dbo].OrderHeader;
		create synonym OrderItem		for [IDT-SW\SWT].[ItemCatalog_Test].[dbo].OrderItem;
        create synonym Vendor			for [IDT-SW\SWT].[ItemCatalog_Test].[dbo].Vendor;
		create synonym Users			for [IDT-SW\SWT].[ItemCatalog_Test].[dbo].Users;
		create synonym Store			for [IDT-SW\SWT].[ItemCatalog_Test].[dbo].Store;
		create synonym SubTeam			for [IDT-SW\SWT].[ItemCatalog_Test].[dbo].SubTeam;
		create synonym ReasonCodeDetail for [IDT-SW\SWT].[ItemCatalog_Test].[dbo].ReasonCodeDetail;
		create synonym Region			for [IDT-SW\SWT].[ItemCatalog_Test].[dbo].Region;
    end
else if exists (select * from sys.servers where name = ''IDQ-SW\SWQ'')
	begin
        create synonym OrderHeader		for [IDQ-SW\SWQ].[ItemCatalog].[dbo].OrderHeader;
		create synonym OrderItem		for [IDQ-SW\SWQ].[ItemCatalog].[dbo].OrderItem;
        create synonym Vendor			for [IDQ-SW\SWQ].[ItemCatalog].[dbo].Vendor;
		create synonym Users			for [IDQ-SW\SWQ].[ItemCatalog].[dbo].Users;
		create synonym Store			for [IDQ-SW\SWQ].[ItemCatalog].[dbo].Store;
		create synonym SubTeam			for [IDQ-SW\SWQ].[ItemCatalog].[dbo].SubTeam;
		create synonym ReasonCodeDetail for [IDQ-SW\SWQ].[ItemCatalog].[dbo].ReasonCodeDetail;
		create synonym Region			for [IDQ-SW\SWQ].[ItemCatalog].[dbo].Region;
	end    
else if exists (select * from sys.servers where name = ''IDP-SW\SWP'')
	begin
        create synonym OrderHeader		for [IDP-SW\SWP].[ItemCatalog].[dbo].OrderHeader;
		create synonym OrderItem		for [IDP-SW\SWP].[ItemCatalog].[dbo].OrderItem;
        create synonym Vendor			for [IDP-SW\SWP].[ItemCatalog].[dbo].Vendor;
		create synonym Users			for [IDP-SW\SWP].[ItemCatalog].[dbo].Users;
		create synonym Store			for [IDP-SW\SWP].[ItemCatalog].[dbo].Store;
		create synonym SubTeam			for [IDP-SW\SWP].[ItemCatalog].[dbo].SubTeam;
		create synonym ReasonCodeDetail for [IDP-SW\SWP].[ItemCatalog].[dbo].ReasonCodeDetail;
		create synonym Region			for [IDP-SW\SWP].[ItemCatalog].[dbo].Region;
	end

--**************************************************************************
-- Set and Populate internal variables
--**************************************************************************
DECLARE @startdate datetime;
DECLARE @enddate datetime;
DECLARE @region	varchar(2);
DECLARE @insertDate	datetime2;

-- Get Monday 00:00:00 of Last Week and Get Sunday 23:59:59.997 of Last Week
SELECT @startdate = DATEADD(day, -7 , DATEADD(wk, DATEDIFF(wk, 0, GETDATE()), 0))
SELECT @enddate = DATEADD(ms, -3, DATEADD(wk, DATEDIFF(wk, 0, GETDATE()), 0))

SELECT  @region			= RegionCode FROM Region;
SELECT	@insertDate		= GETDATE();

--**************************************************************************
-- Get List of Orders and Insert Into PODataLoad table
--**************************************************************************
BEGIN TRY
BEGIN TRAN

	-- Insert into PODataLoad table
	INSERT INTO PODataLoad
	SELECT 
		oh.OrderHeader_ID											as PONumber,
		CASE 
			WHEN oh.ResolutionCodeID IS NOT NULL AND oh.ApprovedDate IS NOT NULL THEN ''Y'' 
			WHEN oh.ApprovedDate IS NULL THEN ''Y''
			ELSE ''N'' 
		END															as Suspended,
		oh.CloseDate												as CloseDate,
		CASE
			WHEN oh.ResolutionCodeID IS NOT NULL AND oh.ApprovedDate IS NOT NULL THEN rcd.ReasonCodeDesc
			WHEN oh.ApprovedDate IS NULL AND oh.InvoiceNumber IS NOT NULL THEN ''Suspended but not Approved''
			WHEN oh.ApprovedDate IS NULL AND oh.InvoiceNumber IS NULL THEN ''Closed as Other/None''
			ELSE ''''
		END															as ResolutionCode,
		ISNULL(oh.AdminNotes, '''')									as AdminNotes,
		v.CompanyName												as Vendor,
		st.SubTeam_No												as Subteam,
		s.Store_Name												as Store,
		CASE 
			WHEN oh.DiscountType > 0 THEN ''Y''
			WHEN 0 < SUM(oi.DiscountType) Then ''Y''
			WHEN 0 < SUM(oi.AdjustedCost) Then ''Y''
			ELSE ''N''
		END															as AdjustedCost,
		CASE 
			WHEN oh.Return_Order = 1 THEN ''Y'' 
			ELSE ''N'' 
		END															as CreditPO,
		CASE 
			WHEN oh.PayByAgreedCost = 1 THEN ''Pay By Agreed Cost'' 
			ELSE ''Pay By Invoice'' 
		END															as VendorType,
		ucr.FullName												as POCreator,
		CASE 
			WHEN oh.Einvoice_id IS NOT NULL THEN ''Y'' 
			ELSE ''N'' 
		END															as EInvoiceMatchedToPO,
		ISNULL(oh.OrderHeaderDesc, '''')								as PONotes,
		ucl.FullName												as ClosedBy,
		@region														as Region,
		oh.ApprovedDate												as ApprovedDate,
		oh.InvoiceNumber											as InvoiceNumber,
		@insertDate													as InsertDate
	FROM
		OrderHeader					oh	(nolock)
		INNER JOIN OrderItem		oi	(nolock) on oh.OrderHeader_ID		= oi.OrderHeader_ID
		INNER JOIN Vendor			vr	(nolock) on oh.ReceiveLocation_ID	= vr.Vendor_ID
		INNER JOIN Vendor			v	(nolock) on oh.Vendor_ID			= v.Vendor_ID
		INNER JOIN Users			ucr (nolock) on oh.CreatedBy			= ucr.User_ID
		INNER JOIN Users			ucl	(nolock) on oh.ClosedBy				= ucl.User_ID
		INNER JOIN Store			s	(nolock) on vr.Store_No				= s.Store_No
		INNER JOIN SubTeam			st	(nolock) on oh.Transfer_To_SubTeam	= st.SubTeam_No
		LEFT JOIN ReasonCodeDetail	rcd (nolock) on oh.ResolutionCodeID		= rcd.ReasonCodeDetailID
	WHERE
		oh.CloseDate		>= @StartDate
		AND oh.CloseDate	<= @EndDate
		AND oh.OrderType_ID <> 3 -- filter out Transfer orders
	GROUP BY
		oh.OrderHeader_ID,
		oh.DiscountType,
		oh.Return_Order,
		oh.PayByAgreedCost,
		oh.eInvoice_Id,
		oh.ResolutionCodeID,
		oh.Vendor_ID,
		oh.ReceiveLocation_ID,
		oh.CreatedBy,
		oh.ClosedBy,
		oh.Transfer_To_SubTeam,
		oh.AdminNotes,
		oh.OrderHeaderDesc,
		oh.CloseDate,
		oh.ApprovedDate,
		oh.InvoiceNumber,
		rcd.ReasonCodeDesc,
		v.CompanyName,
		st.SubTeam_No,
		s.Store_Name,
		ucr.FullName,
		ucl.FullName

	PRINT ''Data has been loaded into PODataLoad table for the '' + @region + '' region.''
	
	-- Insert data into PODataLoadStatus
	INSERT INTO dbo.PODataLoadStatus (Region, InsertDate)
	SELECT @region, @insertDate;
	PRINT ''PODataLoadStatus table has been updated for InsertDate: '' + CAST(@insertDate as nvarchar) + '' and Region: '' + @region + ''.'';

	COMMIT TRAN

END TRY
BEGIN CATCH
	IF @@TRANCOUNT > 0
    	ROLLBACK TRAN
	DECLARE @err_no int, @err_sev int, @err_msg varchar(MAX)
    SELECT @err_no = ERROR_NUMBER(), @err_sev = ERROR_SEVERITY(), @err_msg = ERROR_MESSAGE()
	RAISERROR (''GetWeeklyPOData failed with error no: %d and message: %s'', @err_sev, 1, @err_no, @err_msg)
END CATCH', 
		@database_name=N'POReports', 
		@flags=0
IF (@@ERROR <> 0 OR @ReturnCode <> 0) GOTO QuitWithRollback
/****** Object:  Step [SW_Update Approved Orders]    Script Date: 11/6/2013 3:51:59 PM ******/
EXEC @ReturnCode = msdb.dbo.sp_add_jobstep @job_id=@jobId, @step_name=N'SW_Update Approved Orders', 
		@step_id=22, 
		@cmdexec_success_code=0, 
		@on_success_action=3, 
		@on_success_step_id=0, 
		@on_fail_action=4, 
		@on_fail_step_id=27, 
		@retry_attempts=0, 
		@retry_interval=0, 
		@os_run_priority=0, @subsystem=N'TSQL', 
		@command=N'-- SW
-- Update Approved Orders

USE POReports;
GO

--**************************************************************************
-- Create the synonyms to the appropriate IRMA database
--**************************************************************************
if exists (select * from sysobjects where xtype = ''SN'' and name = ''OrderHeader'')		drop synonym OrderHeader;
if exists (select * from sysobjects where xtype = ''SN'' and name = ''OrderItem'')			drop synonym OrderItem;
if exists (select * from sysobjects where xtype = ''SN'' and name = ''Vendor'')				drop synonym Vendor;
if exists (select * from sysobjects where xtype = ''SN'' and name = ''Users'')				drop synonym Users;
if exists (select * from sysobjects where xtype = ''SN'' and name = ''Store'')				drop synonym Store;
if exists (select * from sysobjects where xtype = ''SN'' and name = ''SubTeam'')			drop synonym SubTeam;
if exists (select * from sysobjects where xtype = ''SN'' and name = ''ReasonCodeDetail'')	drop synonym ReasonCodeDetail;
if exists (select * from sysobjects where xtype = ''SN'' and name = ''Region'')				drop synonym Region;

if exists (select * from sys.servers where name = ''IDT-SW\SWT'')
	begin
		
        create synonym OrderHeader		for [IDT-SW\SWT].[ItemCatalog_Test].[dbo].OrderHeader;
		create synonym OrderItem		for [IDT-SW\SWT].[ItemCatalog_Test].[dbo].OrderItem;
        create synonym Vendor			for [IDT-SW\SWT].[ItemCatalog_Test].[dbo].Vendor;
		create synonym Users			for [IDT-SW\SWT].[ItemCatalog_Test].[dbo].Users;
		create synonym Store			for [IDT-SW\SWT].[ItemCatalog_Test].[dbo].Store;
		create synonym SubTeam			for [IDT-SW\SWT].[ItemCatalog_Test].[dbo].SubTeam;
		create synonym ReasonCodeDetail for [IDT-SW\SWT].[ItemCatalog_Test].[dbo].ReasonCodeDetail;
		create synonym Region			for [IDT-SW\SWT].[ItemCatalog_Test].[dbo].Region;
    end
else if exists (select * from sys.servers where name = ''IDQ-SW\SWQ'')
	begin
        create synonym OrderHeader		for [IDQ-SW\SWQ].[ItemCatalog].[dbo].OrderHeader;
		create synonym OrderItem		for [IDQ-SW\SWQ].[ItemCatalog].[dbo].OrderItem;
        create synonym Vendor			for [IDQ-SW\SWQ].[ItemCatalog].[dbo].Vendor;
		create synonym Users			for [IDQ-SW\SWQ].[ItemCatalog].[dbo].Users;
		create synonym Store			for [IDQ-SW\SWQ].[ItemCatalog].[dbo].Store;
		create synonym SubTeam			for [IDQ-SW\SWQ].[ItemCatalog].[dbo].SubTeam;
		create synonym ReasonCodeDetail for [IDQ-SW\SWQ].[ItemCatalog].[dbo].ReasonCodeDetail;
		create synonym Region			for [IDQ-SW\SWQ].[ItemCatalog].[dbo].Region;
	end    
else if exists (select * from sys.servers where name = ''IDP-SW\SWP'')
	begin
        create synonym OrderHeader		for [IDP-SW\SWP].[ItemCatalog].[dbo].OrderHeader;
		create synonym OrderItem		for [IDP-SW\SWP].[ItemCatalog].[dbo].OrderItem;
        create synonym Vendor			for [IDP-SW\SWP].[ItemCatalog].[dbo].Vendor;
		create synonym Users			for [IDP-SW\SWP].[ItemCatalog].[dbo].Users;
		create synonym Store			for [IDP-SW\SWP].[ItemCatalog].[dbo].Store;
		create synonym SubTeam			for [IDP-SW\SWP].[ItemCatalog].[dbo].SubTeam;
		create synonym ReasonCodeDetail for [IDP-SW\SWP].[ItemCatalog].[dbo].ReasonCodeDetail;
		create synonym Region			for [IDP-SW\SWP].[ItemCatalog].[dbo].Region;
	end

--**************************************************************************
-- Set and Populate internal variables
--**************************************************************************
DECLARE @startdate datetime;
DECLARE @enddate datetime;
DECLARE @region	varchar(2);

-- Get Monday 00:00:00 of Last Week
SELECT @startdate = DATEADD(day, -7 , DATEADD(wk, DATEDIFF(wk, 0, GETDATE()), 0))

-- Get Sunday 23:59:59.997 of Last Week
SELECT @enddate = DATEADD(ms, -3, DATEADD(wk, DATEDIFF(wk, 0, GETDATE()), 0))

select @startdate, @enddate
SELECT  @region	= RegionCode FROM Region;

--**************************************************************************
-- Get list of orders that were approved during the previous week
-- This will be used for updating orders in POData
--**************************************************************************
SELECT
	oh.OrderHeader_ID				as PONumber,
	oh.ApprovedDate					as ApprovedDate,
	ISNULL(rcd.ReasonCodeDesc, '''')	as ResolutionCode,
	oh.InvoiceNumber				as InvoiceNumber
INTO #approvedOrders
FROM
	OrderHeader					oh	(nolock)
	LEFT JOIN ReasonCodeDetail	rcd (nolock) on oh.ResolutionCodeID	= rcd.ReasonCodeDetailID
WHERE
	oh.ApprovedDate		>= @startdate
	AND oh.ApprovedDate <= @enddate

create clustered index idx_c_ApprovedOrders_PONumber on #approvedOrders (PONumber)
create nonclustered index idx_ApprovedOrders_ResolutionCode on #approvedOrders (ResolutionCode)

--**************************************************************************
-- Update orders in POData that were ''Suspended but not Approved'' or 
-- orders that were closed as Other/None but are now approved
--**************************************************************************
-- Update orders with resolution code if there is one assigned for orders that have invoice info.
DECLARE @err_no int, @err_sev int, @err_msg varchar(MAX)

BEGIN TRY
	BEGIN TRAN
		UPDATE pod
		SET
			pod.ResolutionCode = ao.ResolutionCode,
			pod.ApprovedDate = ao.ApprovedDate
		FROM
			POData pod
			INNER JOIN #approvedOrders ao on pod.PONumber = ao.PONumber
		WHERE
			ao.ResolutionCode <> ''''
			AND ao.InvoiceNumber IS NOT NULL
			AND ao.ApprovedDate IS NOT NULL

	COMMIT TRAN
END TRY
BEGIN CATCH
	IF @@TRANCOUNT > 0
    	ROLLBACK TRAN
    SELECT @err_no = ERROR_NUMBER(), @err_sev = ERROR_SEVERITY(), @err_msg = ERROR_MESSAGE()
	RAISERROR (''Updating Newly Approved Orders with Resolution Code failed with error no: %d and message: %s'', @err_sev, 1, @err_no, @err_msg)
END CATCH

-- Update the suspended status for orders closed as Other/None that do not have resolution code but are now approved
BEGIN TRY
	BEGIN TRAN
		UPDATE pod
		SET
			pod.Suspended = ''N'',
			pod.ApprovedDate = ao.ApprovedDate
		FROM
			POData pod
			INNER JOIN #approvedOrders ao on pod.PONumber = ao.PONumber
		WHERE
			ao.ResolutionCode = ''''
			AND ao.ApprovedDate IS NOT NULL
			AND ao.InvoiceNumber IS NOT NULL

		COMMIT TRAN
END TRY
BEGIN CATCH
	IF @@TRANCOUNT > 0
    	ROLLBACK TRAN
    SELECT @err_no = ERROR_NUMBER(), @err_sev = ERROR_SEVERITY(), @err_msg = ERROR_MESSAGE()
	RAISERROR (''Updating the Suspended Status of orders that were closed as Other/None failed with error no: %d and message: %s'', @err_sev, 1, @err_no, @err_msg)
END CATCH', 
		@database_name=N'POReports', 
		@flags=0
IF (@@ERROR <> 0 OR @ReturnCode <> 0) GOTO QuitWithRollback
/****** Object:  Step [UK_Get Weekly Orders By Close Date]    Script Date: 11/6/2013 3:51:59 PM ******/
EXEC @ReturnCode = msdb.dbo.sp_add_jobstep @job_id=@jobId, @step_name=N'UK_Get Weekly Orders By Close Date', 
		@step_id=23, 
		@cmdexec_success_code=0, 
		@on_success_action=3, 
		@on_success_step_id=0, 
		@on_fail_action=4, 
		@on_fail_step_id=27, 
		@retry_attempts=0, 
		@retry_interval=0, 
		@os_run_priority=0, @subsystem=N'TSQL', 
		@command=N'-- UK
USE POReports;
GO

--**************************************************************************
-- Create the synonyms to the appropriate IRMA database
--**************************************************************************
if exists (select * from sysobjects where xtype = ''SN'' and name = ''OrderHeader'')		drop synonym OrderHeader;
if exists (select * from sysobjects where xtype = ''SN'' and name = ''OrderItem'')			drop synonym OrderItem;
if exists (select * from sysobjects where xtype = ''SN'' and name = ''Vendor'')				drop synonym Vendor;
if exists (select * from sysobjects where xtype = ''SN'' and name = ''Users'')				drop synonym Users;
if exists (select * from sysobjects where xtype = ''SN'' and name = ''Store'')				drop synonym Store;
if exists (select * from sysobjects where xtype = ''SN'' and name = ''SubTeam'')			drop synonym SubTeam;
if exists (select * from sysobjects where xtype = ''SN'' and name = ''ReasonCodeDetail'')	drop synonym ReasonCodeDetail;
if exists (select * from sysobjects where xtype = ''SN'' and name = ''Region'')				drop synonym Region;

if exists (select * from sys.servers where name = ''IDT-UK\UKT'')
	begin
		
        create synonym OrderHeader		for [IDT-UK\UKT].[ItemCatalog_Test].[dbo].OrderHeader;
		create synonym OrderItem		for [IDT-UK\UKT].[ItemCatalog_Test].[dbo].OrderItem;
        create synonym Vendor			for [IDT-UK\UKT].[ItemCatalog_Test].[dbo].Vendor;
		create synonym Users			for [IDT-UK\UKT].[ItemCatalog_Test].[dbo].Users;
		create synonym Store			for [IDT-UK\UKT].[ItemCatalog_Test].[dbo].Store;
		create synonym SubTeam			for [IDT-UK\UKT].[ItemCatalog_Test].[dbo].SubTeam;
		create synonym ReasonCodeDetail for [IDT-UK\UKT].[ItemCatalog_Test].[dbo].ReasonCodeDetail;
		create synonym Region			for [IDT-UK\UKT].[ItemCatalog_Test].[dbo].Region;
    end
else if exists (select * from sys.servers where name = ''IDQ-UK\UKQ'')
	begin
        create synonym OrderHeader		for [IDQ-UK\UKQ].[ItemCatalog].[dbo].OrderHeader;
		create synonym OrderItem		for [IDQ-UK\UKQ].[ItemCatalog].[dbo].OrderItem;
        create synonym Vendor			for [IDQ-UK\UKQ].[ItemCatalog].[dbo].Vendor;
		create synonym Users			for [IDQ-UK\UKQ].[ItemCatalog].[dbo].Users;
		create synonym Store			for [IDQ-UK\UKQ].[ItemCatalog].[dbo].Store;
		create synonym SubTeam			for [IDQ-UK\UKQ].[ItemCatalog].[dbo].SubTeam;
		create synonym ReasonCodeDetail for [IDQ-UK\UKQ].[ItemCatalog].[dbo].ReasonCodeDetail;
		create synonym Region			for [IDQ-UK\UKQ].[ItemCatalog].[dbo].Region;
	end    
else if exists (select * from sys.servers where name = ''IDP-UK\UKP'')
	begin
        create synonym OrderHeader		for [IDP-UK\UKP].[ItemCatalog].[dbo].OrderHeader;
		create synonym OrderItem		for [IDP-UK\UKP].[ItemCatalog].[dbo].OrderItem;
        create synonym Vendor			for [IDP-UK\UKP].[ItemCatalog].[dbo].Vendor;
		create synonym Users			for [IDP-UK\UKP].[ItemCatalog].[dbo].Users;
		create synonym Store			for [IDP-UK\UKP].[ItemCatalog].[dbo].Store;
		create synonym SubTeam			for [IDP-UK\UKP].[ItemCatalog].[dbo].SubTeam;
		create synonym ReasonCodeDetail for [IDP-UK\UKP].[ItemCatalog].[dbo].ReasonCodeDetail;
		create synonym Region			for [IDP-UK\UKP].[ItemCatalog].[dbo].Region;
	end

--**************************************************************************
-- Set and Populate internal variables
--**************************************************************************
DECLARE @startdate datetime;
DECLARE @enddate datetime;
DECLARE @region	varchar(2);
DECLARE @insertDate	datetime2;

-- Get Monday 00:00:00 of Last Week and Get Sunday 23:59:59.997 of Last Week
SELECT @startdate = DATEADD(day, -7 , DATEADD(wk, DATEDIFF(wk, 0, GETDATE()), 0))
SELECT @enddate = DATEADD(ms, -3, DATEADD(wk, DATEDIFF(wk, 0, GETDATE()), 0))

SELECT  @region			= RegionCode FROM Region;
SELECT	@insertDate		= GETDATE();

--**************************************************************************
-- Get List of Orders and Insert Into PODataLoad table
--**************************************************************************
BEGIN TRY
BEGIN TRAN

	-- Insert into PODataLoad table
	INSERT INTO PODataLoad
	SELECT 
		oh.OrderHeader_ID											as PONumber,
		CASE 
			WHEN oh.ResolutionCodeID IS NOT NULL AND oh.ApprovedDate IS NOT NULL THEN ''Y'' 
			WHEN oh.ApprovedDate IS NULL THEN ''Y''
			ELSE ''N'' 
		END															as Suspended,
		oh.CloseDate												as CloseDate,
		CASE
			WHEN oh.ResolutionCodeID IS NOT NULL AND oh.ApprovedDate IS NOT NULL THEN rcd.ReasonCodeDesc
			WHEN oh.ApprovedDate IS NULL AND oh.InvoiceNumber IS NOT NULL THEN ''Suspended but not Approved''
			WHEN oh.ApprovedDate IS NULL AND oh.InvoiceNumber IS NULL THEN ''Closed as Other/None''
			ELSE ''''
		END															as ResolutionCode,
		ISNULL(oh.AdminNotes, '''')									as AdminNotes,
		v.CompanyName												as Vendor,
		st.SubTeam_No												as Subteam,
		s.Store_Name												as Store,
		CASE 
			WHEN oh.DiscountType > 0 THEN ''Y''
			WHEN 0 < SUM(oi.DiscountType) Then ''Y''
			WHEN 0 < SUM(oi.AdjustedCost) Then ''Y''
			ELSE ''N''
		END															as AdjustedCost,
		CASE 
			WHEN oh.Return_Order = 1 THEN ''Y'' 
			ELSE ''N'' 
		END															as CreditPO,
		CASE 
			WHEN oh.PayByAgreedCost = 1 THEN ''Pay By Agreed Cost'' 
			ELSE ''Pay By Invoice'' 
		END															as VendorType,
		ucr.FullName												as POCreator,
		CASE 
			WHEN oh.Einvoice_id IS NOT NULL THEN ''Y'' 
			ELSE ''N'' 
		END															as EInvoiceMatchedToPO,
		ISNULL(oh.OrderHeaderDesc, '''')								as PONotes,
		ucl.FullName												as ClosedBy,
		@region														as Region,
		oh.ApprovedDate												as ApprovedDate,
		oh.InvoiceNumber											as InvoiceNumber,
		@insertDate													as InsertDate
	FROM
		OrderHeader					oh	(nolock)
		INNER JOIN OrderItem		oi	(nolock) on oh.OrderHeader_ID		= oi.OrderHeader_ID
		INNER JOIN Vendor			vr	(nolock) on oh.ReceiveLocation_ID	= vr.Vendor_ID
		INNER JOIN Vendor			v	(nolock) on oh.Vendor_ID			= v.Vendor_ID
		INNER JOIN Users			ucr (nolock) on oh.CreatedBy			= ucr.User_ID
		INNER JOIN Users			ucl	(nolock) on oh.ClosedBy				= ucl.User_ID
		INNER JOIN Store			s	(nolock) on vr.Store_No				= s.Store_No
		INNER JOIN SubTeam			st	(nolock) on oh.Transfer_To_SubTeam	= st.SubTeam_No
		LEFT JOIN ReasonCodeDetail	rcd (nolock) on oh.ResolutionCodeID		= rcd.ReasonCodeDetailID
	WHERE
		oh.CloseDate		>= @StartDate
		AND oh.CloseDate	<= @EndDate
		AND oh.OrderType_ID <> 3 -- filter out Transfer orders
	GROUP BY
		oh.OrderHeader_ID,
		oh.DiscountType,
		oh.Return_Order,
		oh.PayByAgreedCost,
		oh.eInvoice_Id,
		oh.ResolutionCodeID,
		oh.Vendor_ID,
		oh.ReceiveLocation_ID,
		oh.CreatedBy,
		oh.ClosedBy,
		oh.Transfer_To_SubTeam,
		oh.AdminNotes,
		oh.OrderHeaderDesc,
		oh.CloseDate,
		oh.ApprovedDate,
		oh.InvoiceNumber,
		rcd.ReasonCodeDesc,
		v.CompanyName,
		st.SubTeam_No,
		s.Store_Name,
		ucr.FullName,
		ucl.FullName

	PRINT ''Data has been loaded into PODataLoad table for the '' + @region + '' region.''
	
	-- Insert data into PODataLoadStatus
	INSERT INTO dbo.PODataLoadStatus (Region, InsertDate)
	SELECT @region, @insertDate;
	PRINT ''PODataLoadStatus table has been updated for InsertDate: '' + CAST(@insertDate as nvarchar) + '' and Region: '' + @region + ''.'';

	COMMIT TRAN

END TRY
BEGIN CATCH
	IF @@TRANCOUNT > 0
    	ROLLBACK TRAN
	DECLARE @err_no int, @err_sev int, @err_msg varchar(MAX)
    SELECT @err_no = ERROR_NUMBER(), @err_sev = ERROR_SEVERITY(), @err_msg = ERROR_MESSAGE()
	RAISERROR (''GetWeeklyPOData failed with error no: %d and message: %s'', @err_sev, 1, @err_no, @err_msg)
END CATCH', 
		@database_name=N'POReports', 
		@flags=0
IF (@@ERROR <> 0 OR @ReturnCode <> 0) GOTO QuitWithRollback
/****** Object:  Step [UK_Update Approved Orders]    Script Date: 11/6/2013 3:51:59 PM ******/
EXEC @ReturnCode = msdb.dbo.sp_add_jobstep @job_id=@jobId, @step_name=N'UK_Update Approved Orders', 
		@step_id=24, 
		@cmdexec_success_code=0, 
		@on_success_action=3, 
		@on_success_step_id=0, 
		@on_fail_action=4, 
		@on_fail_step_id=27, 
		@retry_attempts=0, 
		@retry_interval=0, 
		@os_run_priority=0, @subsystem=N'TSQL', 
		@command=N'-- UK
-- Update Approved Orders

USE POReports;
GO

--**************************************************************************
-- Create the synonyms to the appropriate IRMA database
--**************************************************************************
if exists (select * from sysobjects where xtype = ''SN'' and name = ''OrderHeader'')		drop synonym OrderHeader;
if exists (select * from sysobjects where xtype = ''SN'' and name = ''OrderItem'')			drop synonym OrderItem;
if exists (select * from sysobjects where xtype = ''SN'' and name = ''Vendor'')				drop synonym Vendor;
if exists (select * from sysobjects where xtype = ''SN'' and name = ''Users'')				drop synonym Users;
if exists (select * from sysobjects where xtype = ''SN'' and name = ''Store'')				drop synonym Store;
if exists (select * from sysobjects where xtype = ''SN'' and name = ''SubTeam'')			drop synonym SubTeam;
if exists (select * from sysobjects where xtype = ''SN'' and name = ''ReasonCodeDetail'')	drop synonym ReasonCodeDetail;
if exists (select * from sysobjects where xtype = ''SN'' and name = ''Region'')				drop synonym Region;

if exists (select * from sys.servers where name = ''IDT-UK\UKT'')
	begin
		
        create synonym OrderHeader		for [IDT-UK\UKT].[ItemCatalog_Test].[dbo].OrderHeader;
		create synonym OrderItem		for [IDT-UK\UKT].[ItemCatalog_Test].[dbo].OrderItem;
        create synonym Vendor			for [IDT-UK\UKT].[ItemCatalog_Test].[dbo].Vendor;
		create synonym Users			for [IDT-UK\UKT].[ItemCatalog_Test].[dbo].Users;
		create synonym Store			for [IDT-UK\UKT].[ItemCatalog_Test].[dbo].Store;
		create synonym SubTeam			for [IDT-UK\UKT].[ItemCatalog_Test].[dbo].SubTeam;
		create synonym ReasonCodeDetail for [IDT-UK\UKT].[ItemCatalog_Test].[dbo].ReasonCodeDetail;
		create synonym Region			for [IDT-UK\UKT].[ItemCatalog_Test].[dbo].Region;
    end
else if exists (select * from sys.servers where name = ''IDQ-UK\UKQ'')
	begin
        create synonym OrderHeader		for [IDQ-UK\UKQ].[ItemCatalog].[dbo].OrderHeader;
		create synonym OrderItem		for [IDQ-UK\UKQ].[ItemCatalog].[dbo].OrderItem;
        create synonym Vendor			for [IDQ-UK\UKQ].[ItemCatalog].[dbo].Vendor;
		create synonym Users			for [IDQ-UK\UKQ].[ItemCatalog].[dbo].Users;
		create synonym Store			for [IDQ-UK\UKQ].[ItemCatalog].[dbo].Store;
		create synonym SubTeam			for [IDQ-UK\UKQ].[ItemCatalog].[dbo].SubTeam;
		create synonym ReasonCodeDetail for [IDQ-UK\UKQ].[ItemCatalog].[dbo].ReasonCodeDetail;
		create synonym Region			for [IDQ-UK\UKQ].[ItemCatalog].[dbo].Region;
	end    
else if exists (select * from sys.servers where name = ''IDP-UK\UKP'')
	begin
        create synonym OrderHeader		for [IDP-UK\UKP].[ItemCatalog].[dbo].OrderHeader;
		create synonym OrderItem		for [IDP-UK\UKP].[ItemCatalog].[dbo].OrderItem;
        create synonym Vendor			for [IDP-UK\UKP].[ItemCatalog].[dbo].Vendor;
		create synonym Users			for [IDP-UK\UKP].[ItemCatalog].[dbo].Users;
		create synonym Store			for [IDP-UK\UKP].[ItemCatalog].[dbo].Store;
		create synonym SubTeam			for [IDP-UK\UKP].[ItemCatalog].[dbo].SubTeam;
		create synonym ReasonCodeDetail for [IDP-UK\UKP].[ItemCatalog].[dbo].ReasonCodeDetail;
		create synonym Region			for [IDP-UK\UKP].[ItemCatalog].[dbo].Region;
	end

--**************************************************************************
-- Set and Populate internal variables
--**************************************************************************
DECLARE @startdate datetime;
DECLARE @enddate datetime;
DECLARE @region	varchar(2);

-- Get Monday 00:00:00 of Last Week
SELECT @startdate = DATEADD(day, -7 , DATEADD(wk, DATEDIFF(wk, 0, GETDATE()), 0))

-- Get Sunday 23:59:59.997 of Last Week
SELECT @enddate = DATEADD(ms, -3, DATEADD(wk, DATEDIFF(wk, 0, GETDATE()), 0))

select @startdate, @enddate
SELECT  @region	= RegionCode FROM Region;

--**************************************************************************
-- Get list of orders that were approved during the previous week
-- This will be used for updating orders in POData
--**************************************************************************
SELECT
	oh.OrderHeader_ID				as PONumber,
	oh.ApprovedDate					as ApprovedDate,
	ISNULL(rcd.ReasonCodeDesc, '''')	as ResolutionCode,
	oh.InvoiceNumber				as InvoiceNumber
INTO #approvedOrders
FROM
	OrderHeader					oh	(nolock)
	LEFT JOIN ReasonCodeDetail	rcd (nolock) on oh.ResolutionCodeID	= rcd.ReasonCodeDetailID
WHERE
	oh.ApprovedDate		>= @startdate
	AND oh.ApprovedDate <= @enddate

create clustered index idx_c_ApprovedOrders_PONumber on #approvedOrders (PONumber)
create nonclustered index idx_ApprovedOrders_ResolutionCode on #approvedOrders (ResolutionCode)

--**************************************************************************
-- Update orders in POData that were ''Suspended but not Approved'' or 
-- orders that were closed as Other/None but are now approved
--**************************************************************************
-- Update orders with resolution code if there is one assigned for orders that have invoice info.
DECLARE @err_no int, @err_sev int, @err_msg varchar(MAX)

BEGIN TRY
	BEGIN TRAN
		UPDATE pod
		SET
			pod.ResolutionCode = ao.ResolutionCode,
			pod.ApprovedDate = ao.ApprovedDate
		FROM
			POData pod
			INNER JOIN #approvedOrders ao on pod.PONumber = ao.PONumber
		WHERE
			ao.ResolutionCode <> ''''
			AND ao.InvoiceNumber IS NOT NULL
			AND ao.ApprovedDate IS NOT NULL

	COMMIT TRAN
END TRY
BEGIN CATCH
	IF @@TRANCOUNT > 0
    	ROLLBACK TRAN
    SELECT @err_no = ERROR_NUMBER(), @err_sev = ERROR_SEVERITY(), @err_msg = ERROR_MESSAGE()
	RAISERROR (''Updating Newly Approved Orders with Resolution Code failed with error no: %d and message: %s'', @err_sev, 1, @err_no, @err_msg)
END CATCH

-- Update the suspended status for orders closed as Other/None that do not have resolution code but are now approved
BEGIN TRY
	BEGIN TRAN
		UPDATE pod
		SET
			pod.Suspended = ''N'',
			pod.ApprovedDate = ao.ApprovedDate
		FROM
			POData pod
			INNER JOIN #approvedOrders ao on pod.PONumber = ao.PONumber
		WHERE
			ao.ResolutionCode = ''''
			AND ao.ApprovedDate IS NOT NULL
			AND ao.InvoiceNumber IS NOT NULL

		COMMIT TRAN
END TRY
BEGIN CATCH
	IF @@TRANCOUNT > 0
    	ROLLBACK TRAN
    SELECT @err_no = ERROR_NUMBER(), @err_sev = ERROR_SEVERITY(), @err_msg = ERROR_MESSAGE()
	RAISERROR (''Updating the Suspended Status of orders that were closed as Other/None failed with error no: %d and message: %s'', @err_sev, 1, @err_no, @err_msg)
END CATCH', 
		@database_name=N'POReports', 
		@flags=0
IF (@@ERROR <> 0 OR @ReturnCode <> 0) GOTO QuitWithRollback
/****** Object:  Step [Insert Into POData]    Script Date: 11/6/2013 3:51:59 PM ******/
EXEC @ReturnCode = msdb.dbo.sp_add_jobstep @job_id=@jobId, @step_name=N'Insert Into POData', 
		@step_id=25, 
		@cmdexec_success_code=0, 
		@on_success_action=3, 
		@on_success_step_id=0, 
		@on_fail_action=4, 
		@on_fail_step_id=27, 
		@retry_attempts=0, 
		@retry_interval=0, 
		@os_run_priority=0, @subsystem=N'TSQL', 
		@command=N'--**************************************************************************
-- Step 2 of Sql Agent Job
-- -----------------------
-- This script ''transfers'' the data from the PODataLoad table to the
-- POData table which is what the SuspendedPOReports application uses
--**************************************************************************
USE [POReports];
GO

--**************************************************************************
-- Insert data into POData table from PODataLoad Table
--**************************************************************************
BEGIN TRY
	BEGIN TRAN

	INSERT INTO POData
	SELECT 
		PONumber,
		Suspended,
		CloseDate,
		ResolutionCode,
		AdminNotes,
		Vendor,
		Subteam,
		Store,
		AdjustedCost,
		CreditPO,
		VendorType,
		POCreator,
		EInvoiceMatchedToPO,
		PONotes,
		ClosedBy,
		Region,
		ApprovedDate,
		InvoiceNumber,
		InsertDate
	FROM
		dbo.PODataLoad
	PRINT ''Data has been inserted into POData table from PODataLoad table.''
	
	TRUNCATE TABLE POReports.dbo.PODataLoad
	PRINT ''dbo.PODataLoad table has been Truncated.''

	COMMIT TRAN

END TRY
BEGIN CATCH
	IF @@TRANCOUNT > 0
    	ROLLBACK TRAN
	DECLARE @err_no int, @err_sev int, @err_msg varchar(MAX)
    SELECT @err_no = ERROR_NUMBER(), @err_sev = ERROR_SEVERITY(), @err_msg = ERROR_MESSAGE()
	RAISERROR (''Step 2 failed with error no: %d and message: %s'', @err_sev, 1, @err_no, @err_msg)
END CATCH', 
		@database_name=N'POReports', 
		@flags=0
IF (@@ERROR <> 0 OR @ReturnCode <> 0) GOTO QuitWithRollback
/****** Object:  Step [Calculate PO Totals]    Script Date: 11/6/2013 3:51:59 PM ******/
EXEC @ReturnCode = msdb.dbo.sp_add_jobstep @job_id=@jobId, @step_name=N'Calculate PO Totals', 
		@step_id=26, 
		@cmdexec_success_code=0, 
		@on_success_action=1, 
		@on_success_step_id=0, 
		@on_fail_action=4, 
		@on_fail_step_id=27, 
		@retry_attempts=0, 
		@retry_interval=0, 
		@os_run_priority=0, @subsystem=N'TSQL', 
		@command=N'BEGIN TRY

	BEGIN TRANSACTION [UpdateTotals]

	DECLARE @OldestPODate datetime;
	DECLARE @NewestPODate datetime;
	
	-- Get oldest and newest po close dates
	SELECT @OldestPODate = MIN(CloseDate), @NewestPODate = MAX(CloseDate) FROM POData;

	-- Find start and end fiscal week range
	SELECT TOP 1 @OldestPODate = StartDate FROM FiscalWeek WHERE StartDate <= @OldestPODate AND EndDate >= @OldestPODate;
	SELECT TOP 1 @NewestPODate = EndDate FROM FiscalWeek WHERE StartDate <= @NewestPODate AND EndDate >= @NewestPODate;

	-- Clear totals
	TRUNCATE TABLE POTotals
	
	-- Re-Calculate totals
	INSERT INTO POTotals 
	SELECT  
		FW.[Period], 
		FW.[Week], 
		FW.[Year], 
		(SELECT COUNT(*) 
		FROM POData (NOLOCK) POT 
		WHERE 
			POT.CloseDate >= CAST(fw.StartDate as datetime)
			AND POT.CloseDate <= DATEADD(ms, -3, DATEADD(dd, 1, CAST(fw.EndDate as datetime)))
			AND POT.Region = R.RegionID
			AND (POT.ApprovedDate IS NOT NULL OR (POT.ApprovedDate IS NULL AND POT.ResolutionCode = ''Suspended but not Approved''))) as TotalPO, 
		(SELECT COUNT(*)
		FROM POData (NOLOCK) POS 
		WHERE 
			POS.CloseDate >= CAST(fw.StartDate as datetime)
			AND POS.CloseDate <= DATEADD(ms, -3, DATEADD(dd, 1, CAST(fw.EndDate as datetime)))
			AND Suspended = ''Y'' 
			AND POS.Region = R.RegionID 
			AND (POS.ApprovedDate IS NOT NULL OR (POS.ApprovedDate IS NULL AND POS.ResolutionCode = ''Suspended but not Approved''))) as SuspendedPO,
		LastUpdated = GETDATE(), 
		Region=R.RegionID 
	FROM 
		FiscalWeek (NOLOCK) fw
		INNER JOIN Regions R ON R.RegionID != ''CEN'' 
	WHERE 
		fw.StartDate >= @OldestPODate AND fw.EndDate <= @NewestPODate;
	
	COMMIT TRANSACTION [UpdateTotals]
END TRY
BEGIN CATCH
	IF @@TRANCOUNT > 0
    	ROLLBACK TRANSACTION [UpdateTotals]
	DECLARE @err_no int, @err_sev int, @err_msg varchar(MAX)
    SELECT @err_no = ERROR_NUMBER(), @err_sev = ERROR_SEVERITY(), @err_msg = ERROR_MESSAGE()
	RAISERROR (''Calculate Totals failed with error no: %d and message: %s'', @err_sev, 1, @err_no, @err_msg)
END CATCH
GO', 
		@database_name=N'POReports', 
		@flags=0
IF (@@ERROR <> 0 OR @ReturnCode <> 0) GOTO QuitWithRollback
/****** Object:  Step [Failure Alert]    Script Date: 11/6/2013 3:52:00 PM ******/
EXEC @ReturnCode = msdb.dbo.sp_add_jobstep @job_id=@jobId, @step_name=N'Failure Alert', 
		@step_id=27, 
		@cmdexec_success_code=0, 
		@on_success_action=2, 
		@on_success_step_id=0, 
		@on_fail_action=2, 
		@on_fail_step_id=0, 
		@retry_attempts=0, 
		@retry_interval=0, 
		@os_run_priority=0, @subsystem=N'TSQL', 
		@command=N'DECLARE @tableHTML NVARCHAR(MAX);   
DECLARE @jobname NVARCHAR(128)
select @jobname =  name from msdb.dbo.sysjobs where job_id = $(ESCAPE_SQUOTE(JOBID))
SET @tableHTML =
    N''<table border="1">'' +
    N''<tr><th>Job Name</th><th>Run Date</th>'' +
    N''<th>Run Duration</th><th>Error Message</th>'' +
    CAST ( ( SELECT td = j.name, '''',
                    td = CONVERT(DATETIME, CONVERT(CHAR(8), run_date, 112) + '' '' + STUFF(STUFF(RIGHT(''000000'' + CONVERT(VARCHAR(8), run_time), 6), 5, 0, '':''), 3, 0, '':''), 121), '''',
                    td =CASE len(jh.run_duration)
		WHEN 1 THEN CAST(''00:00:0'' + CAST(jh.run_duration as char) as char (8))
		WHEN 2 THEN CAST(''00:00:'' + CAST(jh.run_duration as char) as char (8))
		WHEN 3 THEN CAST(''00:0'' + LEFT(RIGHT(jh.run_duration, 3), 1) + '':'' + RIGHT(jh.run_duration, 2) as char (8))
		WHEN 4 THEN CAST(''00:'' + LEFT(RIGHT(jh.run_duration, 4), 2) + '':'' + RIGHT(jh.run_duration, 2) as char (8))
		WHEN 5 THEN CAST(''0'' + LEFT(RIGHT(jh.run_duration, 5), 1) + '':'' + LEFT(RIGHT(jh.run_duration, 4), 2) +'':'' + RIGHT(jh.run_duration, 2) as char (8))
		WHEN 6 THEN CAST(LEFT(RIGHT(jh.run_duration, 6), 2) + '':'' + LEFT(RIGHT(jh.run_duration, 4), 2) + '':'' + RIGHT(jh.run_duration, 2) as char (8))
		END, '''',
                    td = jh.message, ''''
              FROM msdb.dbo.sysjobhistory as jh
              JOIN msdb.dbo.sysjobs j ON jh.job_id = j.job_id
              WHERE j.name = @jobname
              		AND jh.run_date = CONVERT(varchar, GETDATE(), 112)
              ORDER BY jh.instance_id ASC
              FOR XML PATH(''tr''), TYPE ) AS NVARCHAR(MAX) ) +
    N''</table>'';
DECLARE @mailSubject varchar(MAX); 
SET @mailSubject = @jobname + '' failed on server '' + @@SERVERNAME
EXEC msdb.dbo.sp_send_dbmail
	@profile_name = ''PO Report'',
	@recipients= ''IRMA.support@wholefoods.com; irma.developers@wholefoods.com'',
	@subject = @mailSubject,
	@body = @tableHTML,
	@body_format = ''HTML''', 
		@database_name=N'msdb', 
		@flags=0
IF (@@ERROR <> 0 OR @ReturnCode <> 0) GOTO QuitWithRollback
EXEC @ReturnCode = msdb.dbo.sp_update_job @job_id = @jobId, @start_step_id = 1
IF (@@ERROR <> 0 OR @ReturnCode <> 0) GOTO QuitWithRollback
EXEC @ReturnCode = msdb.dbo.sp_add_jobschedule @job_id=@jobId, @name=N'Monday Morning Weekly', 
		@enabled=1, 
		@freq_type=8, 
		@freq_interval=2, 
		@freq_subday_type=1, 
		@freq_subday_interval=0, 
		@freq_relative_interval=0, 
		@freq_recurrence_factor=1, 
		@active_start_date=20131004, 
		@active_end_date=20131129, 
		@active_start_time=50000, 
		@active_end_time=235959, 
		@schedule_uid=N'4b0aab53-754a-472f-b45d-d30e4926c688'
IF (@@ERROR <> 0 OR @ReturnCode <> 0) GOTO QuitWithRollback
EXEC @ReturnCode = msdb.dbo.sp_add_jobserver @job_id = @jobId, @server_name = N'(local)'
IF (@@ERROR <> 0 OR @ReturnCode <> 0) GOTO QuitWithRollback
COMMIT TRANSACTION
GOTO EndSave
QuitWithRollback:
    IF (@@TRANCOUNT > 0) ROLLBACK TRANSACTION
EndSave:

GO


