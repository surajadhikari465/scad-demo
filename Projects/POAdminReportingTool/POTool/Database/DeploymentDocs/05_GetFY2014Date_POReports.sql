USE POReports
GO

--**************************************************************************
-- Set and Populate internal variables
--**************************************************************************
declare @linkedServer varchar(10);
declare @sqlstring nvarchar(max);
declare @server varchar(50);

declare @startdate datetime;
declare @enddate datetime;
declare @insertDate	datetime2;
declare @region varchar(2);

-- error handling
DECLARE @err_no int, @err_sev int, @err_msg varchar(MAX);

-- Adjust start and end dates depending on when deployment happens
select @startdate = '2013-09-30';
select @enddate = DATEADD(ms, -3, '2013-11-11')
select @insertDate = GETDATE();

select @server = @@SERVERNAME;

--**************************************************************************
-- Setup Cursor to loop through linked servers
--**************************************************************************
declare @server_cursor cursor 

-- Check for Dev, QA, or Prod
if CHARINDEX('SHARED2D', @server) > 0
begin
	set @server_cursor = cursor for
		select 
			ss.data_source
		from sys.servers ss
		where 
			ss.data_source like 'IDT-%'
			OR ss.data_source like 'IDD-%'
end
else if CHARINDEX('SHARED2Q', @server) > 0
begin
	set @server_cursor = cursor for
		select 
			ss.data_source
		from sys.servers ss
		where 
			ss.data_source like 'IDQ-%'
end
else if CHARINDEX('SHARED2P', @server) > 0
begin
	set @server_cursor = cursor for
		select 
			ss.data_source
		from sys.servers ss
		where 
			ss.data_source like 'IDP-%'
end

-- If it's a Dev server, query dbo.ItemCatalog_Test
IF CHARINDEX('SHARED2D', @server) > 0
begin
	open @server_cursor
	fetch next from @server_cursor into @linkedServer

	while @@FETCH_STATUS = 0
	begin
		--**************************************************************************
		-- Get List of Orders and Insert Into PODataLoad table for each linked server
		--**************************************************************************
		SET @region = SUBSTRING(CAST(@linkedServer as nvarchar(10)), 5, 2)

		-- Setup sql string to execute
		SET @sqlstring = 'INSERT INTO PODataLoad 
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
			''' + @region + '''												as Region,
			oh.ApprovedDate												as ApprovedDate,
			oh.InvoiceNumber											as InvoiceNumber, 
			''' + CONVERT(nvarchar(23),@insertDate,21) + '''					as InsertDate
		FROM
			[' + @linkedServer + '].[ItemCatalog_Test].[dbo].[OrderHeader]				oh	(nolock)
			INNER JOIN [' + @linkedServer + '].[ItemCatalog_Test].[dbo].[OrderItem]		oi	(nolock) on oh.OrderHeader_ID		= oi.OrderHeader_ID
			INNER JOIN [' + @linkedServer + '].[ItemCatalog_Test].[dbo].[Vendor]		vr	(nolock) on oh.ReceiveLocation_ID	= vr.Vendor_ID
			INNER JOIN [' + @linkedServer + '].[ItemCatalog_Test].[dbo].[Vendor]		v	(nolock) on oh.Vendor_ID			= v.Vendor_ID
			INNER JOIN [' + @linkedServer + '].[ItemCatalog_Test].[dbo].[Users]			ucr (nolock) on oh.CreatedBy			= ucr.User_ID
			INNER JOIN [' + @linkedServer + '].[ItemCatalog_Test].[dbo].[Users]			ucl	(nolock) on oh.ClosedBy				= ucl.User_ID
			INNER JOIN [' + @linkedServer + '].[ItemCatalog_Test].[dbo].[Store]			s	(nolock) on vr.Store_No				= s.Store_No
			INNER JOIN [' + @linkedServer + '].[ItemCatalog_Test].[dbo].[SubTeam]		st	(nolock) on oh.Transfer_To_SubTeam	= st.SubTeam_No
			LEFT JOIN [' + @linkedServer + '].[ItemCatalog_Test].[dbo].[ReasonCodeDetail]	rcd (nolock) on oh.ResolutionCodeID		= rcd.ReasonCodeDetailID
		WHERE
			oh.CloseDate		>= ''' + CONVERT(nvarchar, @StartDate, 21) + ''' 
			AND oh.CloseDate	<= ''' + CONVERT(nvarchar, @EndDate, 21) + ''' 
			AND oh.OrderType_ID <> 3
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
			ucl.FullName'
	
		print @sqlstring
	
		BEGIN TRY
			BEGIN TRAN
	
			exec sp_executesql @sqlstring
			PRINT 'Data has been loaded into PODataLoad table for the ' + @region + ' region.'
	
			-- Insert data into PODataLoadStatus
			INSERT INTO dbo.PODataLoadStatus (Region, InsertDate)
			SELECT @region, @insertDate;
			PRINT 'PODataLoadStatus table has been updated for InsertDate: ' + CAST(@insertDate as nvarchar) + ' and Region: ' + @region + '.'

			COMMIT TRAN

		END TRY
		BEGIN CATCH
			IF @@TRANCOUNT > 0
    			ROLLBACK TRAN
			SELECT @err_no = ERROR_NUMBER(), @err_sev = ERROR_SEVERITY(), @err_msg = ERROR_MESSAGE()
			RAISERROR ('GetFY2014Data failed with error no: %d and message: %s', @err_sev, 1, @err_no, @err_msg)
		END CATCH

		fetch next from @server_cursor into @linkedServer
	end
	close @server_cursor
	deallocate @server_cursor
END
-- If it's a QA or Prod server, query dbo.ItemCatalog
ELSE IF (CHARINDEX('SHARED2Q', @server) > 0 OR CHARINDEX('SHARED2P', @server) > 0)
BEGIN
	open @server_cursor
	fetch next from @server_cursor into @linkedServer

	while @@FETCH_STATUS = 0
	begin
		--**************************************************************************
		-- Get List of Orders and Insert Into PODataLoad table for each linked server
		--**************************************************************************
		SET @region = SUBSTRING(CAST(@linkedServer as nvarchar(10)), 5, 2)

		-- Setup sql string to execute
		SET @sqlstring = 'INSERT INTO PODataLoad 
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
			''' + @region + '''												as Region,
			oh.ApprovedDate												as ApprovedDate,
			oh.InvoiceNumber											as InvoiceNumber, 
			''' + CONVERT(nvarchar(23),@insertDate,21) + '''					as InsertDate
		FROM
			[' + @linkedServer + '].[ItemCatalog].[dbo].[OrderHeader]				oh	(nolock)
			INNER JOIN [' + @linkedServer + '].[ItemCatalog].[dbo].[OrderItem]		oi	(nolock) on oh.OrderHeader_ID		= oi.OrderHeader_ID
			INNER JOIN [' + @linkedServer + '].[ItemCatalog].[dbo].[Vendor]		vr	(nolock) on oh.ReceiveLocation_ID	= vr.Vendor_ID
			INNER JOIN [' + @linkedServer + '].[ItemCatalog].[dbo].[Vendor]		v	(nolock) on oh.Vendor_ID			= v.Vendor_ID
			INNER JOIN [' + @linkedServer + '].[ItemCatalog].[dbo].[Users]			ucr (nolock) on oh.CreatedBy			= ucr.User_ID
			INNER JOIN [' + @linkedServer + '].[ItemCatalog].[dbo].[Users]			ucl	(nolock) on oh.ClosedBy				= ucl.User_ID
			INNER JOIN [' + @linkedServer + '].[ItemCatalog].[dbo].[Store]			s	(nolock) on vr.Store_No				= s.Store_No
			INNER JOIN [' + @linkedServer + '].[ItemCatalog].[dbo].[SubTeam]		st	(nolock) on oh.Transfer_To_SubTeam	= st.SubTeam_No
			LEFT JOIN [' + @linkedServer + '].[ItemCatalog].[dbo].[ReasonCodeDetail]	rcd (nolock) on oh.ResolutionCodeID		= rcd.ReasonCodeDetailID
		WHERE
			oh.CloseDate		>= ''' + CONVERT(nvarchar, @StartDate, 21) + ''' 
			AND oh.CloseDate	<= ''' + CONVERT(nvarchar, @EndDate, 21) + ''' 
			AND oh.OrderType_ID <> 3
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
			ucl.FullName'
	
		print @sqlstring
	
		BEGIN TRY
			BEGIN TRAN
	
			exec sp_executesql @sqlstring
			PRINT 'Data has been loaded into PODataLoad table for the ' + @region + ' region.'
	
			-- Insert data into PODataLoadStatus
			INSERT INTO dbo.PODataLoadStatus (Region, InsertDate)
			SELECT @region, @insertDate;
			PRINT 'PODataLoadStatus table has been updated for InsertDate: ' + CAST(@insertDate as nvarchar) + ' and Region: ' + @region + '.'

			COMMIT TRAN

		END TRY
		BEGIN CATCH
			IF @@TRANCOUNT > 0
    			ROLLBACK TRAN
			SELECT @err_no = ERROR_NUMBER(), @err_sev = ERROR_SEVERITY(), @err_msg = ERROR_MESSAGE()
			RAISERROR ('GetFY2014Data failed with error no: %d and message: %s', @err_sev, 1, @err_no, @err_msg)
		END CATCH

		fetch next from @server_cursor into @linkedServer
	end
	close @server_cursor
	deallocate @server_cursor
END

-- Insert Into POData from PODataLoad
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
		PRINT 'Data has been inserted into POData table from PODataLoad table.'
	
		TRUNCATE TABLE POReports.dbo.PODataLoad
		PRINT 'dbo.PODataLoad table has been Truncated.'

	COMMIT TRAN
END TRY
BEGIN CATCH
	IF @@TRANCOUNT > 0
    	ROLLBACK TRAN
	SELECT @err_no = ERROR_NUMBER(), @err_sev = ERROR_SEVERITY(), @err_msg = ERROR_MESSAGE()
	RAISERROR ('GetWeeklyPOData failed with error no: %d and message: %s', @err_sev, 1, @err_no, @err_msg)
END CATCH