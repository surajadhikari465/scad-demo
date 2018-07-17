CREATE PROCEDURE [gpm].[UpdatePrice]
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
	@NumberOfRowsUpdated INT OUTPUT

AS
BEGIN
	SET NOCOUNT ON;

	DECLARE @sql nvarchar(max);
	SET @sql = N'
		UPDATE [gpm].[Price_' + @Region + ']
		SET
			GpmID = @GpmID,
			Price = @Price,
			PriceTypeAttribute = @PriceTypeAttribute,
			EndDate = @EndDate,
			SellableUOM = @SellableUOM,
			CurrencyCode = @CurrencyCode, 
			Multiple = @Multiple,
			PercentOff = @PercentOff,
			TagExpirationDate = ISNULL(@TagExpirationDate, TagExpirationDate),
			ModifiedDateUtc = SYSUTCDATETIME()
		WHERE 
			Region = @Region
			AND ItemID = @ItemID
			AND	BusinessUnitID = @BusinessUnitID
			AND StartDate = @StartDate
			AND PriceType = @PriceType';

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
		@PercentOff DECIMAL(5,2) ';

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

		SELECT @NumberOfRowsUpdated = @@ROWCOUNT
END
GO

GRANT EXEC ON [gpm].[UpdatePrice] TO TibcoRole
GO