CREATE PROCEDURE [gpm].[AddPrice] 
	@Region nvarchar(2),
	@GpmID uniqueidentifier = NULL,
	@ItemID int,
	@BusinessUnitID int,
	@Price decimal(9,2),
	@StartDate datetime2,
	@EndDate datetime2,
	@PriceType nvarchar(3),
	@PriceTypeAttribute nvarchar(10),
	@SellableUOM nvarchar(3),
	@CurrencyCode nvarchar(3),
	@Multiple int,
	@TagExpirationDate datetime2 = NULL,
	@PercentOff DECIMAL(5,2)  = NULL,
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
			TagExpirationDate,
			PercentOff
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
			@TagExpirationDate,
			@PercentOff
		)';

	DECLARE @params NVARCHAR(500);
	SET @params = N'
		@Region nvarchar(2),
		@GpmID uniqueidentifier,
		@ItemID int,
		@BusinessUnitID int,
		@Price decimal(9,2),
		@StartDate datetime2,
		@EndDate datetime2,
		@PriceType nvarchar(3),
		@PriceTypeAttribute nvarchar(10),
		@SellableUOM nvarchar(3),
		@CurrencyCode nvarchar(3),
		@Multiple int,
		@TagExpirationDate datetime2,
		@PercentOff DECIMAL(5,2)  ';

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
		@TagExpirationDate = @TagExpirationDate,
		@PercentOff = @PercentOff;

	   SELECT @NumberOfRowsAdded = @@ROWCOUNT

END
GO

GRANT EXEC ON [gpm].[AddPrice] TO TibcoRole
GO