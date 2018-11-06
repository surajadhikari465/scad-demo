CREATE PROCEDURE [dbo].[UpdateCompetitorPrice] 
	@Item_Key int,
	@CompetitorStoreID int,
	@FiscalYear smallint,
	@FiscalPeriod tinyint,
	@PeriodWeek tinyint,
	@UPCCode varchar(50),
	@Description varchar(250),
	@PriceMultiple tinyint,
	@Price smallmoney,
	@SaleMultiple tinyint,
	@Sale smallmoney,
	@Size decimal(9,4),
	@Unit_ID int,
	@UpdateUserID int,
	@UpdateDateTime smalldatetime
AS
BEGIN

	UPDATE CompetitorPrice
	SET
		UPCCode = @UPCCode,
		Description = @Description,
		PriceMultiple = @PriceMultiple,
		Price = @Price,
		SaleMultiple = @SaleMultiple,
		Sale = @Sale,
		Size = @Size,
		Unit_ID = @Unit_ID,
		UpdateUserID = @UpdateUserID,
		UpdateDateTime = @UpdateDateTime
	WHERE
		Item_Key = @Item_Key
		AND
		CompetitorStoreID = @CompetitorStoreID
		AND
		FiscalYear = @FiscalYear
		AND
		FiscalPeriod = @FiscalPeriod
		AND
		PeriodWeek = @PeriodWeek
END
GO
GRANT EXECUTE
    ON OBJECT::[dbo].[UpdateCompetitorPrice] TO [IRMAClientRole]
    AS [dbo];

