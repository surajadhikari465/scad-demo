CREATE PROCEDURE [gpm].[AddPrice] 
	@Region nvarchar(2),
	@GpmID uniqueidentifier,
	@ItemID int,
	@BusinessUnitID int,
	@Price decimal(19,4),
	@StartDate datetime2,
	@EndDate datetime2,
	@PriceType nvarchar(3),
	@PriceTypeAttribute nvarchar(3),
	@SellableUOM nvarchar(3),
	@CurrencyCode nvarchar(3),
	@Multiple int,
	@NewTagExpiration datetime2 = NULL,
	@NumberOfRowsAdded INT OUTPUT
AS
BEGIN

	SET NOCOUNT ON;

	DECLARE @sql nvarchar(max);
	SET @sql = N'
		INSERT INTO [gpm].[Price_' + @Region + ']
		(
			Region,
			GpmID,
			ItemID,
			BusinessUnitID,
			Price,
			StartDate,
			EndDate,
			PriceType,
			PriceTypeAttribute,
			SellableUOM,
			CurrencyCode,
			Multiple,
			NewTagExpiration
		)
		VALUES
		(
			@Region,
			@GpmID,
			@ItemID,
			@BusinessUnitID,
			@Price,
			@StartDate,
			@EndDate,
			@PriceType,
			@PriceTypeAttribute,
			@SellableUOM,
			@CurrencyCode,
			@Multiple,
			@NewTagExpiration
		)';

	DECLARE @params NVARCHAR(500);
	SET @params = N'
		@Region nvarchar(2),
		@GpmID uniqueidentifier,
		@ItemID int,
		@BusinessUnitID int,
		@Price decimal(19,4),
		@StartDate datetime2,
		@EndDate datetime2,
		@PriceType nvarchar(3),
		@PriceTypeAttribute nvarchar(3),
		@SellableUOM nvarchar(3),
		@CurrencyCode nvarchar(3),
		@Multiple int,
		@NewTagExpiration datetime2';

	EXEC sp_executesql
		@sql,
		@params,
		@Region = @Region,
		@GpmID = @GpmID,
		@ItemID = @ItemID,
		@BusinessUnitID = @BusinessUnitID,
		@Price = @Price,
		@StartDate = @StartDate,
		@EndDate = @EndDate,
		@PriceType = @PriceType,
		@PriceTypeAttribute = @PriceTypeAttribute,
		@SellableUOM = @SellableUOM,
		@CurrencyCode = @CurrencyCode,
		@Multiple = @Multiple,
		@NewTagExpiration = @NewTagExpiration;

	   SELECT @NumberOfRowsAdded = @@ROWCOUNT

END
GO