if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[UpdateCompetitorImportInfo]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[UpdateCompetitorImportInfo]
GO

CREATE PROCEDURE [dbo].[UpdateCompetitorImportInfo] 
	@CompetitorImportInfoID int,
	@CompetitorImportSessionID int,
	@Item_Key int,
	@CompetitorID int,
	@CompetitorLocationID int,
	@CompetitorStoreID int,
	@FiscalYear smallint,
	@FiscalPeriod tinyint,
	@PeriodWeek tinyint,
	@Competitor varchar(50),
	@Location varchar(50),
	@CompetitorStore varchar(50),
	@UPCCode varchar(50),
	@Description varchar(250),
	@Size decimal (9, 4),
	@Unit_ID int,
	@PriceMultiple tinyint,
	@Price smallmoney,
	@SaleMultiple tinyint,
	@Sale smallmoney,
	@DateChecked smalldatetime
AS
BEGIN

UPDATE CompetitorImportInfo 
SET
	CompetitorImportSessionID = @CompetitorImportSessionID, 
	Item_Key = @Item_Key,
	CompetitorID = @CompetitorID,
	CompetitorLocationID = @CompetitorLocationID,
	CompetitorStoreID = @CompetitorStoreID,
	FiscalYear = @FiscalYear,
	FiscalPeriod = @FiscalPeriod,
	PeriodWeek = @PeriodWeek,
	Competitor = @Competitor,
	Location = @Location,
	CompetitorStore = @CompetitorStore,
	UPCCode = @UPCCode,
	Description = @Description,
	Size = @Size,
	Unit_ID = @Unit_ID,
	PriceMultiple = @PriceMultiple,
	Price = @Price,
	SaleMultiple = @SaleMultiple,
	Sale = @Sale,
	DateChecked = @DateChecked
WHERE
	CompetitorImportInfoID = @CompetitorImportInfoID

END 
go    