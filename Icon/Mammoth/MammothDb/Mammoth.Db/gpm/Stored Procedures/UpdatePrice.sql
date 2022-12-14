﻿CREATE PROCEDURE gpm.UpdatePrice
	@Region NVARCHAR(2),
	@GpmID UNIQUEIDENTIFIER = NULL,
	@ItemID INT,
	@BusinessUnitID INT,
	@Price DECIMAL(9,2),
	@StartDate DATETIME2,
	@EndDate DATETIME2,
	@PriceType NVARCHAR(3),
	@PriceTypeAttribute NVARCHAR(10),
	@SellableUOM NVARCHAR(3),
	@CurrencyCode NVARCHAR(3),
	@Multiple INT,
	@TagExpirationDate DATETIME2 = NULL,
	@PercentOff DECIMAL(5,2)  = NULL,
	@NumberOfRowsUpdated INT OUTPUT

AS
BEGIN
	SET NOCOUNT ON;

	DECLARE @sql NVARCHAR(max) = '
			UPDATE gpm.Price_' + @Region + '
		    SET GpmID = @GpmID,
			      Price = @Price,
			      PriceTypeAttribute = @PriceTypeAttribute,
			      EndDate = @EndDate,
			      SellableUOM = @SellableUOM,
			      CurrencyCode = @CurrencyCode, 
			      Multiple = @Multiple,
			      PercentOff = @PercentOff,
			      TagExpirationDate = ISNULL(@TagExpirationDate, TagExpirationDate),
			      ModifiedDateUtc = SYSUTCDATETIME()
		    WHERE Region = @Region
			    AND ItemID = @ItemID
			    AND	BusinessUnitID = @BusinessUnitID
			    AND CAST(StartDate AS DATE) = CAST(@StartDate AS DATE)
			    AND PriceType = @PriceType';


	DECLARE @params NVARCHAR(500) = '
		@Region NVARCHAR(2),
		@GpmID UNIQUEIDENTIFIER,
		@ItemID INT,
		@BusinessUnitID INT,
		@Price DECIMAL(9,2),
		@StartDate DATETIME2,
		@EndDate DATETIME2,
		@PriceType NVARCHAR(3),
		@PriceTypeAttribute NVARCHAR(10),
		@SellableUOM NVARCHAR(3),
		@CurrencyCode NVARCHAR(3),
		@Multiple INT,
		@TagExpirationDate DATETIME2,
		@PercentOff DECIMAL(5,2)';

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

		SELECT @NumberOfRowsUpdated = @@ROWCOUNT;
END
GO

GRANT EXEC ON gpm.UpdatePrice TO TibcoRole;
GO

GRANT EXEC ON gpm.UpdatePrice TO MammothRole
GO