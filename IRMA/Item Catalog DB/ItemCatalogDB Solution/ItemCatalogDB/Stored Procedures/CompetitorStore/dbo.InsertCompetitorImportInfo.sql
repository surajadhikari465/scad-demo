if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[InsertCompetitorImportInfo]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[InsertCompetitorImportInfo]
GO

CREATE PROCEDURE [dbo].[InsertCompetitorImportInfo] 
	@CompetitorImportSessionID int,
	@Item_Key int = NULL,
	@CompetitorID int = NULL,
	@CompetitorLocationID int = NULL,
	@CompetitorStoreID int = NULL,
	@FiscalYear smallint = NULL,
	@FiscalPeriod tinyint = NULL,
	@PeriodWeek tinyint = NULL,
	@Competitor varchar(50),
	@Location varchar(50),
	@CompetitorStore varchar(50),
	@UPCCode varchar(50),
	@Description varchar(250),
	@Size decimal (9, 4) = null,
	@UnitOfMeasure varchar(50) = NULL,
	@Unit_ID int = NULL,
	@PriceMultiple tinyint = 1,
	@Price smallmoney,
	@SaleMultiple tinyint = NULL,
	@Sale smallmoney = NULL,
	@DateChecked smalldatetime,
	@CompetitorImportInfoID int OUTPUT
AS
BEGIN

-- Map given date checked to a fiscal week
IF(@FiscalYear IS NULL OR @FiscalPeriod IS NULL OR @PeriodWeek IS NULL)
BEGIN
	SELECT
		@FiscalYear = FiscalYear,
		@FiscalPeriod = FiscalPeriod,
		@PeriodWeek = PeriodWeek
	FROM
		FiscalWeek
	WHERE
		@DateChecked BETWEEN StartDate AND EndDate
		
	-- If no valid fiscal week is provided, select the current fiscal week
	IF @FiscalYear IS NULL OR @FiscalPeriod IS NULL OR @PeriodWeek IS NULL
	BEGIN
		SELECT TOP 1
			@FiscalYear = FiscalYear,
			@FiscalPeriod = FiscalPeriod,
			@PeriodWeek = PeriodWeek
		FROM
			FiscalWeek
		WHERE
			EndDate <= GETDATE()
		ORDER BY
			StartDate DESC
	END
END

IF @Unit_ID IS NULL AND @UnitOfMeasure IS NOT NULL
BEGIN
	SELECT TOP 1
		@Unit_ID = Unit_ID
	FROM
		ItemUnit
	WHERE
		Unit_Name = @UnitOfMeasure
		OR
		Unit_Abbreviation = @UnitOfMeasure
END

-- Ensure that SaleMuliple always has a value when sale is provided...
IF(@Sale IS NOT NULL)
BEGIN
	SET @SaleMultiple = ISNULL(@SaleMultiple, 1)
END
-- ... and has none when Sale is not provided
ELSE
BEGIN
	SET @SaleMultiple = NULL
END

INSERT INTO CompetitorImportInfo 
	(CompetitorImportSessionID, 
	Item_Key,
	CompetitorID,
	CompetitorLocationID,
	CompetitorStoreID,
	FiscalYear,
	FiscalPeriod,
	PeriodWeek,
	Competitor,
	Location,
	CompetitorStore,
	UPCCode,
	Description,
	Size,
	Unit_ID,
	PriceMultiple,
	Price,
	SaleMultiple,
	Sale,
	CheckDate,
	DateChecked)
	values 
	(@CompetitorImportSessionID, 
	@Item_Key,
	@CompetitorID,
	@CompetitorLocationID,
	@CompetitorStoreID,
	@FiscalYear,
	@FiscalPeriod,
	@PeriodWeek,
	@Competitor,
	@Location,
	@CompetitorStore,
	@UPCCode,
	@Description,
	@Size,
	@Unit_ID,
	ISNULL(@PriceMultiple, 1),
	@Price,
	@SaleMultiple,
	@Sale,
	@DateChecked,
	GETDATE())

SET @CompetitorImportInfoID = SCOPE_IDENTITY()

END

GO