/******************************************************************************
		SO [dbo].[OrderHeaderHistory]
		Change Steps
******************************************************************************/
PRINT N'Status: BEGIN FL [dbo].[OrderHeaderHistory] ChangeSteps (takes about 15 minutes in TEST):'+ ' ---  [dbo].[OrderHeaderHistory] --- ' + CONVERT(NVARCHAR(MAX), SYSDATETIME(), 9)
GO
PRINT N'Status: Approximate Target End Time for Script to Complete: --- [dbo].[OrderHeaderHistory] --- ' + CONVERT(NVARCHAR(MAX), DATEADD(MINUTE, 15, SYSDATETIME()), 9)
GO
USE [ItemCatalog]
--USE [ItemCatalog_Test]
GO
SET NOCOUNT ON;
GO
/******************************************************************************
		1. Disable SO Change Tracking
******************************************************************************/
PRINT N'Status: 1. Disable SO Change Tracking --- [dbo].[OrderHeaderHistory] --- ' + CONVERT(NVARCHAR(MAX), SYSDATETIME(), 9)
GO
PRINT N'No Change Tracking on [dbo].[OrderHeaderHistory].  This Step is N/A'
GO
/******************************************************************************
		2. Drop SO Defaults (Manually Generated)
******************************************************************************/
PRINT N'Status: 2. Drop SO Defaults (Manually Generated) --- [dbo].[OrderHeaderHistory] --- ' + CONVERT(NVARCHAR(MAX), SYSDATETIME(), 9)
GO
ALTER TABLE [OrderHeaderHistory] DROP CONSTRAINT [DF_OrderHeaderHistory_InsertDate]
GO
ALTER TABLE [OrderHeaderHistory] DROP CONSTRAINT [DF_OrderHeaderHistory_FromQueue]
GO
ALTER TABLE [OrderHeaderHistory] DROP CONSTRAINT [DF_OrderHeaderHistory_AccrualException]
GO
ALTER TABLE [OrderHeaderHistory] DROP CONSTRAINT [DF__OrderHead__PayBy__240BF313]
GO
/******************************************************************************
		3. Drop SO Extended Properties
******************************************************************************/
PRINT N'Status: 3. Drop SO Extended Properties --- [dbo].[OrderHeaderHistory] --- ' + CONVERT(NVARCHAR(MAX), SYSDATETIME(), 9)
GO
PRINT N'No Extended Properties on [dbo].[OrderHeaderHistory].  This Step is N/A'
GO
/******************************************************************************
		4. Drop SO Triggers
******************************************************************************/
PRINT N'Status: 4. Drop SO Triggers --- [dbo].[OrderHeaderHistory] --- ' + CONVERT(NVARCHAR(MAX), SYSDATETIME(), 9)
GO
PRINT N'No Triggers on [dbo].[OrderHeaderHistory].  This Step is N/A'
GO
/******************************************************************************
		5. Drop SO Foreign Keys
******************************************************************************/
PRINT N'Status: 5. Drop SO Foreign Keys --- [dbo].[OrderHeaderHistory] --- ' + CONVERT(NVARCHAR(MAX), SYSDATETIME(), 9)
GO
PRINT N'No Foreign Keys on [dbo].[OrderHeaderHistory].  This Step is N/A'
GO
/******************************************************************************
		6. Drop SO Indexes
******************************************************************************/
PRINT N'Status: 6. Drop SO Indexes --- [dbo].[OrderHeaderHistory] --- ' + CONVERT(NVARCHAR(MAX), SYSDATETIME(), 9)
GO
IF  EXISTS (SELECT * FROM sys.indexes WHERE object_id = OBJECT_ID(N'[dbo].[OrderHeaderHistory]') AND name = N'idxOrderHeaderHistoryOrderHeader_ID')
DROP INDEX [idxOrderHeaderHistoryOrderHeader_ID] ON [dbo].[OrderHeaderHistory]
GO
IF  EXISTS (SELECT * FROM sys.indexes WHERE object_id = OBJECT_ID(N'[dbo].[OrderHeaderHistory]') AND name = N'idxClustered_OrderDate')
DROP INDEX [idxClustered_OrderDate] ON [dbo].[OrderHeaderHistory] WITH ( ONLINE = OFF )
GO
/******************************************************************************
		7. Rename SO PK 
******************************************************************************/
PRINT N'Status: 7. Rename SO PK  --- [dbo].[OrderHeaderHistory] --- ' + CONVERT(NVARCHAR(MAX), SYSDATETIME(), 9)
GO
PRINT N'No Primary Key on [dbo].[OrderHeaderHistory].  This Step is N/A'
GO
/******************************************************************************
		8. Rename SO Table
******************************************************************************/
PRINT N'Status: 8. Rename SO Table --- [dbo].[OrderHeaderHistory] --- ' + CONVERT(NVARCHAR(MAX), SYSDATETIME(), 9)
GO
EXECUTE sp_rename N'[dbo].[OrderHeaderHistory]', N'OrderHeaderHistory_Unaligned';
GO
/******************************************************************************
		9. Create FL Table
******************************************************************************/
PRINT N'Status: 9. Create FL Table --- [dbo].[OrderHeaderHistory] --- ' + CONVERT(NVARCHAR(MAX), SYSDATETIME(), 9)
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[OrderHeaderHistory]') AND type in (N'U'))
BEGIN
CREATE TABLE [dbo].[OrderHeaderHistory](
	[OrderHeader_ID] [int] NOT NULL,
	[InvoiceNumber] [varchar](20) NULL,
	[OrderHeaderDesc] [varchar](4000) NULL,
	[Vendor_ID] [int] NOT NULL,
	[PurchaseLocation_ID] [int] NOT NULL,
	[ReceiveLocation_ID] [int] NOT NULL,
	[CreatedBy] [int] NOT NULL,
	[OrderDate] [smalldatetime] NOT NULL,
	[CloseDate] [datetime] NULL,
	[OriginalCloseDate] [datetime] NULL,
	[SystemGenerated] [bit] NOT NULL,
	[Sent] [bit] NOT NULL,
	[Fax_Order] [bit] NOT NULL,
	[Expected_Date] [smalldatetime] NULL,
	[SentDate] [smalldatetime] NULL,
	[QuantityDiscount] [decimal](9, 2) NOT NULL,
	[DiscountType] [int] NOT NULL,
	[Transfer_SubTeam] [int] NULL,
	[Transfer_To_SubTeam] [int] NULL,
	[Return_Order] [bit] NOT NULL,
	[User_ID] [int] NULL,
	[Temperature] [tinyint] NULL,
	[Accounting_In_DateStamp] [smalldatetime] NULL,
	[Accounting_In_UserID] [int] NULL,
	[InvoiceDate] [smalldatetime] NULL,
	[ApprovedDate] [smalldatetime] NULL,
	[ApprovedBy] [int] NULL,
	[UploadedDate] [smalldatetime] NULL,
	[RecvLogDate] [datetime] NULL,
	[RecvLog_No] [int] NULL,
	[RecvLogUser_ID] [int] NULL,
	[VendorDoc_ID] [varchar](16) NULL,
	[VendorDocDate] [smalldatetime] NULL,
	[WarehouseSent] [bit] NOT NULL,
	[WarehouseSentDate] [smalldatetime] NULL,
	[Host_Name] [varchar](20) NULL,
	[InsertDate] [datetime] NOT NULL CONSTRAINT [DF_OrderHeaderHistory_InsertDate]  DEFAULT (getdate()),
	[OrderType_ID] [tinyint] NULL,
	[ProductType_ID] [tinyint] NULL,
	[FromQueue] [bit] NOT NULL CONSTRAINT [DF_OrderHeaderHistory_FromQueue]  DEFAULT ((0)),
	[SentToFaxDate] [smalldatetime] NULL,
	[ClosedBy] [int] NULL,
	[MatchingValidationCode] [int] NULL,
	[MatchingUser_ID] [int] NULL,
	[MatchingDate] [datetime] NULL,
	[Freight3Party_OrderCost] [smallmoney] NULL,
	[WarehouseCancelled] [datetime] NULL,
	[PayByAgreedCost] [bit] NOT NULL CONSTRAINT [DF_OrderHeaderHistory_PayByAgreedCost]  DEFAULT ((0)),
	[OrderRefreshCostSource_ID] [int] NULL
) ON [PRIMARY]
END
GO
/******************************************************************************
		10. Populate FL Table in Batches
******************************************************************************/
PRINT N'Status: 10. Populate FL Table in Batches (takes about 3 minutes in TEST for batches of 10000): --- [dbo].[OrderHeaderHistory] --- ' + CONVERT(NVARCHAR(MAX), SYSDATETIME(), 9)
GO
PRINT N'Status: Approximate Target End Time for this Step to Complete: --- [dbo].[OrderHeaderHistory] --- ' + CONVERT(NVARCHAR(MAX), DATEADD(MINUTE, 3, SYSDATETIME()), 9)
GO
SET TRANSACTION ISOLATION LEVEL SERIALIZABLE;
SET XACT_ABORT ON;
IF EXISTS (SELECT TOP 1 1 FROM   [dbo].[OrderHeaderHistory_Unaligned])
    BEGIN
        DECLARE @RowsToLoad BIGINT;
		DECLARE @RowsPerBatch INT = 10000;
		DECLARE @LeftBoundary BIGINT = 0;
		DECLARE @RightBoundary BIGINT = @RowsPerBatch;
		SELECT @RowsToLoad = MAX([OrderHeader_ID]) FROM [dbo].[OrderHeaderHistory_Unaligned]
		WHILE @LeftBoundary < @RowsToLoad
		BEGIN
			INSERT INTO [dbo].[OrderHeaderHistory] ([OrderHeader_ID], [InvoiceNumber], [OrderHeaderDesc], [Vendor_ID], [PurchaseLocation_ID], [ReceiveLocation_ID], [CreatedBy], [OrderDate], [CloseDate], [OriginalCloseDate], [SystemGenerated], [Sent], [Fax_Order], [Expected_Date], [SentDate], [QuantityDiscount], [DiscountType], [Transfer_SubTeam], [Transfer_To_SubTeam], [Return_Order], [User_ID], [Temperature], [Accounting_In_DateStamp], [Accounting_In_UserID], [InvoiceDate], [ApprovedDate], [ApprovedBy], [UploadedDate], [RecvLogDate], [RecvLog_No], [RecvLogUser_ID], [VendorDoc_ID], [VendorDocDate], [WarehouseSent], [WarehouseSentDate], [Host_Name], [InsertDate], [OrderType_ID], [ProductType_ID], [FromQueue], [SentToFaxDate], [ClosedBy], [MatchingValidationCode], [MatchingUser_ID], [MatchingDate], [Freight3Party_OrderCost], [PayByAgreedCost], [WarehouseCancelled], [OrderRefreshCostSource_ID])
			SELECT src.[OrderHeader_ID],
				   src.[InvoiceNumber],
				   src.[OrderHeaderDesc],
				   src.[Vendor_ID],
				   src.[PurchaseLocation_ID],
				   src.[ReceiveLocation_ID],
				   src.[CreatedBy],
				   src.[OrderDate],
				   src.[CloseDate],
				   src.[OriginalCloseDate],
				   src.[SystemGenerated],
				   src.[Sent],
				   src.[Fax_Order],
				   src.[Expected_Date],
				   src.[SentDate],
				   src.[QuantityDiscount],
				   src.[DiscountType],
				   src.[Transfer_SubTeam],
				   src.[Transfer_To_SubTeam],
				   src.[Return_Order],
				   src.[User_ID],
				   src.[Temperature],
				   src.[Accounting_In_DateStamp],
				   src.[Accounting_In_UserID],
				   src.[InvoiceDate],
				   src.[ApprovedDate],
				   src.[ApprovedBy],
				   src.[UploadedDate],
				   src.[RecvLogDate],
				   src.[RecvLog_No],
				   src.[RecvLogUser_ID],
				   src.[VendorDoc_ID],
				   src.[VendorDocDate],
				   src.[WarehouseSent],
				   src.[WarehouseSentDate],
				   src.[Host_Name],
				   src.[InsertDate],
				   src.[OrderType_ID],
				   src.[ProductType_ID],
				   src.[FromQueue],
				   src.[SentToFaxDate],
				   src.[ClosedBy],
				   src.[MatchingValidationCode],
				   src.[MatchingUser_ID],
				   src.[MatchingDate],
				   src.[Freight3Party_OrderCost],
				   src.[PayByAgreedCost],
				   src.[WarehouseCancelled],
				   src.[OrderRefreshCostSource_ID]
			FROM   [dbo].[OrderHeaderHistory_Unaligned] src    
			WHERE
				src.[OrderHeader_ID] > @LeftBoundary
				AND src.[OrderHeader_ID] <= @RightBoundary
			ORDER BY [OrderHeader_ID] ASC;
			SET @LeftBoundary = @LeftBoundary + @RowsPerBatch;
			SET @RightBoundary = @RightBoundary + @RowsPerBatch;
		END
    END
	SET TRANSACTION ISOLATION LEVEL READ COMMITTED;
GO
/******************************************************************************
		11. Enable FL Change Tracking
******************************************************************************/
PRINT N'Status: 11. Enable FL Change Tracking --- [dbo].[OrderHeaderHistory] --- ' + CONVERT(NVARCHAR(MAX), SYSDATETIME(), 9)
GO
PRINT N'No Change Tracking on [dbo].[OrderHeaderHistory].  This Step is N/A'
GO
/******************************************************************************
		12. Create FL Indexes
******************************************************************************/
PRINT N'Status: 12. Create FL Indexes --- [dbo].[OrderHeaderHistory] --- ' + CONVERT(NVARCHAR(MAX), SYSDATETIME(), 9)
GO
IF NOT EXISTS (SELECT * FROM sys.indexes WHERE object_id = OBJECT_ID(N'[dbo].[OrderHeaderHistory]') AND name = N'idxOrderHeaderHistoryOrderHeader_ID')
CREATE NONCLUSTERED INDEX [idxOrderHeaderHistoryOrderHeader_ID] ON [dbo].[OrderHeaderHistory]
(
	[OrderHeader_ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, FILLFACTOR = 80) ON [PRIMARY]
GO
/******************************************************************************
		13. Create FL Foreign Keys
******************************************************************************/
PRINT N'Status: 13. Create FL Foreign Keys --- [dbo].[OrderHeaderHistory] --- ' + CONVERT(NVARCHAR(MAX), SYSDATETIME(), 9)
GO
PRINT N'No Foreign Keys on [dbo].[OrderHeaderHistory].  This Step is N/A'
GO
/******************************************************************************
		14. Create FL Triggers
******************************************************************************/
PRINT N'Status: 14. Create FL Triggers --- [dbo].[OrderHeaderHistory] --- ' + CONVERT(NVARCHAR(MAX), SYSDATETIME(), 9)
GO
PRINT N'No Triggers on [dbo].[OrderHeaderHistory].  This Step is N/A'
GO
/******************************************************************************
		15. Grant SO Perms
******************************************************************************/
PRINT N'Status: 15. Grant SO Perms --- [dbo].[OrderHeaderHistory] --- ' + CONVERT(NVARCHAR(MAX), SYSDATETIME(), 9)
GO
GRANT SELECT ON [dbo].[OrderHeaderHistory] TO [IRMAClientRole] AS [dbo]
GO
GRANT SELECT ON [dbo].[OrderHeaderHistory] TO [IRMAReportsRole] AS [dbo]
GO
GRANT SELECT ON [dbo].[OrderHeaderHistory] TO [IRMASchedJobsRole] AS [dbo]
GO
GRANT SELECT ON [dbo].[OrderHeaderHistory] TO [IRSUser] AS [dbo]
GO
GRANT SELECT ON [dbo].[OrderHeaderHistory] TO [SOAppsUserAdmin] AS [dbo]
GO
GRANT SELECT ON [dbo].[OrderHeaderHistory] TO [sobluesky] AS [dbo]
GO
/******************************************************************************
		16. Check FL Checks (generated from VS schema compare)
******************************************************************************/
PRINT N'Status: 16. Check FL Checks --- [dbo].[OrderHeaderHistory] --- ' + CONVERT(NVARCHAR(MAX), SYSDATETIME(), 9)
GO
PRINT N'No Check/Checks were generated on [dbo].[OrderHeaderHistory].  This Step is N/A'
GO
/******************************************************************************
		17. Compare SO and FL Tables
******************************************************************************/
PRINT N'Status: 17. Compare SO and FL Tables (takes about 5 minutes in TEST): --- [dbo].[OrderHeaderHistory] --- ' + CONVERT(NVARCHAR(MAX), SYSDATETIME(), 9)
GO
DECLARE @old BIGINT
DECLARE	@new BIGINT

SELECT @old = count(*)
FROM [dbo].[OrderHeaderHistory_Unaligned](NOLOCK)

SELECT @new = count(*)
FROM [dbo].[OrderHeaderHistory](NOLOCK)

PRINT N'[dbo].[OrderHeaderHistory]_Unaligned Row Count:	' + CONVERT(NVARCHAR(30), @old)
PRINT N'[dbo].[OrderHeaderHistory] Aligned Row Count:	' + CONVERT(NVARCHAR(30), @new)
IF @old = @new
BEGIN
	PRINT N'**** SUCCESS!!**** New Table Row Count Matches Old Row Count. --- [dbo].[OrderHeaderHistory] --- ' + CONVERT(NVARCHAR(MAX), SYSDATETIME(), 9)
END
ELSE
BEGIN
	PRINT N'**** OPERATION FAILED **** New Table Row Count Does Not Match Old Row Count. --- [dbo].[OrderHeaderHistory] --- ' + CONVERT(NVARCHAR(MAX), SYSDATETIME(), 9)
END
