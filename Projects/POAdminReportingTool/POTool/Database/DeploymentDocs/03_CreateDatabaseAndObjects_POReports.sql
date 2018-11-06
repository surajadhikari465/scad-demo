USE [msdb]
GO
EXEC sp_addrolemember 'SQLAgentReaderRole', 'WFM\IRMA.Developers'
GO

USE [POReports]
GO
/****** Object:  User [WFM\SPOReportsDev]    Script Date: 11/6/2013 2:09:03 PM ******/
CREATE USER [WFM\SPOReports] FOR LOGIN [WFM\SPOReports] WITH DEFAULT_SCHEMA=[dbo]
GO
/****** Object:  User [WFM\IRMA Developers]    Script Date: 11/6/2013 2:09:03 PM ******/
CREATE USER [WFM\IRMA.Developers] FOR LOGIN [WFM\IRMA.Developers]
GO
EXEC sp_addrolemember 'db_datareader', 'WFM\SPOReports'
GO

EXEC sp_addrolemember 'db_datareader', 'WFM\IRMA.Developers'
GO

GRANT VIEW DEFINITION to [WFM\IRMA.Developers]
GO

/****** Object:  StoredProcedure [dbo].[GetFiscalPeriodPCN]    Script Date: 11/19/2013 3:09:54 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE PROCEDURE [dbo].[GetFiscalPeriodPCN]
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    -- Insert statements for procedure here
	DECLARE @Date DATETIME = GETDATE();
	DECLARE @NextDate DATETIME;
	DECLARE @PrevDate DATETIME;
	DECLARE @Temp TABLE(
		FiscalPeriodNumber int,
		FiscalYear int,
		Quarter int,
		FPStartDate DATETIME,
		FPEndDate DATETIME
	)

	INSERT INTO @Temp SELECT TOP 1 FiscalPeriodNumber, FiscalYear, Quarter, FPStartDate, FPEndDate FROM FiscalPeriods WHERE FPStartDate <= @Date AND FPEndDate >= @Date
	-- Use Current FP to get next and previous
	SELECT @NextDate = (SELECT TOP 1 FPEndDate+1 FROM @Temp)
	SELECT @PrevDate = (SELECT TOP 1 FPStartDate-1 FROM @Temp)
	INSERT INTO @Temp SELECT TOP 1 FiscalPeriodNumber, FiscalYear, Quarter, FPStartDate, FPEndDate FROM FiscalPeriods WHERE FPStartDate = @NextDate
	INSERT INTO @Temp SELECT TOP 1 FiscalPeriodNumber, FiscalYear, Quarter, FPStartDate, FPEndDate FROM FiscalPeriods WHERE FPEndDate = @PrevDate
	
	-- Reformat to match C# class
	SELECT 
		[Period] = T.FiscalPeriodNumber,
		[Quarter] = T.Quarter,
		[Year] = T.FiscalYear,
		[StartDate] = T.FPStartDate,
		[EndDate] = T.FPEndDate
	FROM @Temp T ORDER BY [Year], [Quarter], [Period] ASC
END


GO
/****** Object:  StoredProcedure [dbo].[GetFiscalWeekPCN]    Script Date: 11/19/2013 3:09:54 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE PROCEDURE [dbo].[GetFiscalWeekPCN]
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    -- Insert statements for procedure here
	DECLARE @Date DATETIME = GETDATE();
	DECLARE @NextDate DATETIME;
	DECLARE @PrevDate DATETIME;
	DECLARE @Temp TABLE(
		Week int,
		Period int,
		Year int,
		Description varchar(255),
		StartDate DATETIME,
		EndDate DATETIME
	)

	INSERT INTO @Temp SELECT TOP 1 Week, Period, Year, Description, StartDate, EndDate FROM FiscalWeek WHERE StartDate <= @Date AND EndDate >= @Date
	-- Use Current FP to get next and previous
	SELECT @NextDate = (SELECT TOP 1 EndDate+1 FROM @Temp)
	SELECT @PrevDate = (SELECT TOP 1 StartDate-1 FROM @Temp)
	INSERT INTO @Temp SELECT TOP 1 Week, Period, Year, Description, StartDate, EndDate FROM FiscalWeek WHERE StartDate <= @NextDate AND EndDate >= @NextDate
	INSERT INTO @Temp SELECT TOP 1 Week, Period, Year, Description, StartDate, EndDate FROM FiscalWeek WHERE StartDate <= @PrevDate AND EndDate >= @PrevDate

	SELECT * FROM @Temp ORDER BY Year, Period, Week ASC
END


GO
/****** Object:  StoredProcedure [dbo].[GetResolutionCodePOTotals]    Script Date: 11/19/2013 3:09:54 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE PROCEDURE [dbo].[GetResolutionCodePOTotals]
	-- Add the parameters for the stored procedure here
	@ResolutionCode varchar(200) = NULL,
	@StartDate DATETIME = NULL,
	@EndDate DATETIME = NULL,
	@Region varchar(5) = NULL
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;
	DECLARE @TotalPOs int = 0;
	DECLARE @SuspendedPOs int = 0;

    -- Insert statements for procedure here
	IF @StartDate IS NOT NULL AND @EndDate IS NOT NULL AND @ResolutionCode IS NOT NULL AND @Region IS NOT NULL
	BEGIN
		
		SELECT @TotalPOs = COUNT(*) FROM POData (NOLOCK) WHERE POData.CloseDate BETWEEN @StartDate AND @EndDate AND ResolutionCode = @ResolutionCode AND Region = @Region
		SELECT @SuspendedPOs = COUNT(*) FROM POData(NOLOCK) WHERE POData.CloseDate BETWEEN @StartDate AND @EndDate AND Suspended = 'Y' AND ResolutionCode = @ResolutionCode AND Region = @Region
	END
	
	SELECT [TotalPOs] = @TotalPOs, [SuspendedPOs] = @SuspendedPOs
END


GO
/****** Object:  StoredProcedure [dbo].[GetResolutionCodeTotals]    Script Date: 11/19/2013 3:09:54 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE PROCEDURE [dbo].[GetResolutionCodeTotals]
	-- Add the parameters for the stored procedure here
	@StartDate datetime = NULL,
	@EndDate datetime = NULL,
	@Vendor varchar(200) = NULL,
	@Store varchar(200) = NULL,
	@Region varchar(5) = NULL
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    -- Insert statements for procedure here
    IF @StartDate IS NOT NULL AND @EndDate IS NOT NULL AND @Region IS NOT NULL
	BEGIN
		SELECT 
			[Name] = PO.ResolutionCode,
			COUNT(PO.PONumber) AS Total
		FROM
			POData PO (nolock) 
		WHERE 
			PO.CloseDate BETWEEN @StartDate AND @EndDate
			AND (@Vendor IS NULL OR PO.Vendor = @Vendor)
			AND (@Store IS NULL OR PO.Store = @Store)
			AND PO.Suspended = 'Y'
			AND PO.Region = @Region
		GROUP BY PO.ResolutionCode
		ORDER BY Total DESC
	END
END


GO
/****** Object:  StoredProcedure [dbo].[GetStorePOTotals]    Script Date: 11/19/2013 3:09:54 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE PROCEDURE [dbo].[GetStorePOTotals]
	-- Add the parameters for the stored procedure here
	@Store varchar(200) = NULL,
	@StartDate DATETIME = NULL,
	@EndDate DATETIME = NULL,
	@Region varchar(5) = NULL
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;
	DECLARE @TotalPOs int = 0;
	DECLARE @SuspendedPOs int = 0;

    -- Insert statements for procedure here
	IF @StartDate IS NOT NULL AND @EndDate IS NOT NULL AND @Store IS NOT NULL AND @Region IS NOT NULL
	BEGIN
		
		SELECT @TotalPOs = COUNT(*) FROM POData (NOLOCK) WHERE POData.CloseDate BETWEEN @StartDate AND @EndDate AND Store = @Store AND Region = @Region
		SELECT @SuspendedPOs = COUNT(*) FROM POData (NOLOCK) WHERE POData.CloseDate BETWEEN @StartDate AND @EndDate AND Suspended = 'Y' AND Store = @Store AND Region = @Region
	END
	
	SELECT [TotalPOs] = @TotalPOs, [SuspendedPOs] = @SuspendedPOs
END


GO
/****** Object:  StoredProcedure [dbo].[GetVendorPOTotals]    Script Date: 11/19/2013 3:09:54 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE PROCEDURE [dbo].[GetVendorPOTotals]
	-- Add the parameters for the stored procedure here
	@Vendor varchar(200) = NULL,
	@StartDate DATETIME = NULL,
	@EndDate DATETIME = NULL
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;
	DECLARE @TotalPOs int = 0;
	DECLARE @SuspendedPOs int = 0;

    -- Insert statements for procedure here
	IF @StartDate IS NOT NULL AND @EndDate IS NOT NULL AND @Vendor IS NOT NULL
	BEGIN
		
		SELECT @TotalPOs = COUNT(*) FROM POData WHERE POData.CloseDate BETWEEN @StartDate AND @EndDate AND Vendor = @Vendor
		SELECT @SuspendedPOs = COUNT(*) FROM POData WHERE POData.CloseDate BETWEEN @StartDate AND @EndDate AND Suspended = 'Y' AND Vendor = @Vendor
	END
	
	SELECT [TotalPOs] = @TotalPOs, [SuspendedPOs] = @SuspendedPOs
END


GO
/****** Object:  StoredProcedure [dbo].[UpdatePOTotals]    Script Date: 11/19/2013 3:09:54 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE PROCEDURE [dbo].[UpdatePOTotals]
	-- Add the parameters for the stored procedure here
	@FP int = 0,
	@FY int = 0,
	@FW int = 0,
	@StartDate DATETIME = NULL,
	@EndDate DATETIME = NULL,
	@Region varchar(5) = NULL
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;
	DECLARE @TotalPOs int = 0;
	DECLARE @SuspendedPOs int = 0;
	DECLARE @RecordExists int = 0;

    -- Insert statements for procedure here
	IF @FP > 0 AND @FY > 0 AND @FW > 0 AND @StartDate IS NOT NULL AND @EndDate IS NOT NULL AND @Region IS NOT NULL
	BEGIN
		
		SELECT @TotalPOs = COUNT(*) FROM POData (NOLOCK) WHERE POData.CloseDate BETWEEN @StartDate AND @EndDate AND POData.Region = @Region
		SELECT @SuspendedPOs = COUNT(*) FROM POData (NOLOCK) WHERE POData.CloseDate BETWEEN @StartDate AND @EndDate AND Suspended = 'Y' AND POData.Region = @Region
		SELECT @RecordExists = COUNT(*) FROM POTotals (NOLOCK) WHERE FP=@FP AND FY=@FY AND FW=@FW AND POTotals.Region = @Region
		
		IF @RecordExists > 0
		BEGIN
			-- Update record
			UPDATE POTotals SET TotalPO=@TotalPOs, SuspendedPO=@SuspendedPOs, LastUpdated=GETDATE() WHERE FP=@FP AND FY=@FY AND FW=@FW AND Region=@Region
		END
		ELSE
		BEGIN
			-- Insert record
			INSERT INTO POTotals (FP, FW, FY, TotalPO, SuspendedPO, LastUpdated, Region) VALUES (@FP, @FW, @FY, @TotalPOs, @SuspendedPOs, GETDATE(), @Region)
		END
	END
	
	SELECT [TotalPOs] = @TotalPOs, [SuspendedPOs] = @SuspendedPOs
END


GO
/****** Object:  Table [dbo].[POData]    Script Date: 11/19/2013 3:09:54 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[POData](
	[PODataID] [int] IDENTITY(1,1) NOT NULL,
	[PONumber] [int] NOT NULL,
	[Suspended] [varchar](1) NOT NULL,
	[CloseDate] [datetime2](7) NULL,
	[ResolutionCode] [varchar](255) NULL,
	[AdminNotes] [varchar](2000) NULL,
	[Vendor] [varchar](50) NOT NULL,
	[Subteam] [int] NOT NULL,
	[Store] [varchar](50) NOT NULL,
	[AdjustedCost] [varchar](1) NULL,
	[CreditPO] [varchar](1) NOT NULL,
	[VendorType] [varchar](50) NOT NULL,
	[POCreator] [varchar](255) NULL,
	[EInvoiceMatchedToPO] [varchar](1) NOT NULL,
	[PONotes] [varchar](4000) NOT NULL,
	[ClosedBy] [varchar](255) NULL,
	[Region] [varchar](5) NULL,
	[ApprovedDate] [datetime2](7) NULL,
	[InvoiceNumber] [varchar](20) NULL,
	[InsertDate] [datetime2](7) NOT NULL,
 CONSTRAINT [PK_POData] PRIMARY KEY CLUSTERED 
(
	[PODataID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, FILLFACTOR = 80) ON [PRIMARY]
) ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[PODataLoad]    Script Date: 11/19/2013 3:09:54 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[PODataLoad](
	[PODataLoadID] [int] IDENTITY(1,1) NOT NULL,
	[PONumber] [int] NOT NULL,
	[Suspended] [varchar](1) NOT NULL,
	[CloseDate] [datetime2](7) NULL,
	[ResolutionCode] [varchar](255) NULL,
	[AdminNotes] [varchar](2000) NULL,
	[Vendor] [varchar](50) NOT NULL,
	[Subteam] [int] NOT NULL,
	[Store] [varchar](50) NOT NULL,
	[AdjustedCost] [varchar](1) NULL,
	[CreditPO] [varchar](1) NOT NULL,
	[VendorType] [varchar](50) NOT NULL,
	[POCreator] [varchar](255) NULL,
	[EInvoiceMatchedToPO] [varchar](1) NOT NULL,
	[PONotes] [varchar](4000) NOT NULL,
	[ClosedBy] [varchar](255) NULL,
	[Region] [varchar](5) NULL,
	[ApprovedDate] [datetime2](7) NULL,
	[InvoiceNumber] [varchar](20) NULL,
	[InsertDate] [datetime2](7) NOT NULL,
 CONSTRAINT [PK_PODataLoad] PRIMARY KEY CLUSTERED 
(
	[PODataLoadID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, FILLFACTOR = 80) ON [PRIMARY]
) ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[PODataLoadStatus]    Script Date: 11/19/2013 3:09:54 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[PODataLoadStatus](
	[PODataLoadStatusID] [int] IDENTITY(1,1) NOT NULL,
	[Region] [varchar](5) NULL,
	[InsertDate] [datetime2](7) NOT NULL,
 CONSTRAINT [PK_PODataLoadStatus] PRIMARY KEY CLUSTERED 
(
	[PODataLoadStatusID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, FILLFACTOR = 80) ON [PRIMARY]
) ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[POTotals]    Script Date: 11/19/2013 3:09:54 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[POTotals](
	[FP] [int] NOT NULL,
	[FW] [int] NOT NULL,
	[FY] [int] NOT NULL,
	[TotalPO] [int] NOT NULL,
	[SuspendedPO] [int] NOT NULL,
	[LastUpdated] [datetime] NOT NULL,
	[Region] [varchar](5) NULL
) ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[POUsers]    Script Date: 11/19/2013 3:09:54 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[POUsers](
	[UserID] [varchar](255) NOT NULL,
	[DisplayName] [varchar](255) NOT NULL,
	[StoreTLC] [varchar](255) NOT NULL,
	[SecurityGroupID] [int] NOT NULL,
	[Region] [varchar](5) NOT NULL,
	[DateCreated] [datetime] NOT NULL,
	[LastLogin] [datetime] NULL,
	[LastLoginIPAddress] [varchar](15) NULL,
 CONSTRAINT [PK_POSecurity] PRIMARY KEY CLUSTERED 
(
	[UserID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, FILLFACTOR = 80) ON [PRIMARY]
) ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO

GRANT EXECUTE ON [dbo].[GetFiscalPeriodPCN] TO [WFM\SPOReports]
GO
GRANT EXECUTE ON [dbo].[GetFiscalWeekPCN] TO [WFM\SPOReports]
GO
GRANT EXECUTE ON [dbo].[GetResolutionCodePOTotals] TO [WFM\SPOReports]
GO
GRANT EXECUTE ON [dbo].[GetResolutionCodeTotals] TO [WFM\SPOReports]
GO
GRANT EXECUTE ON [dbo].[GetStorePOTotals] TO [WFM\SPOReports]
GO
GRANT EXECUTE ON [dbo].[GetVendorPOTotals] TO [WFM\SPOReports]
GO
GRANT EXECUTE ON [dbo].[UpdatePOTotals] TO [WFM\SPOReports]
GO
