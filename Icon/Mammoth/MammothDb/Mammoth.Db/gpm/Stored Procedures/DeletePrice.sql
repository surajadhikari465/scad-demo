CREATE PROCEDURE [gpm].[DeletePrice]
	@Region nvarchar(2),
	@ItemID int,
	@BusinessUnitID int,
	@StartDate datetime2,
	@PriceType nvarchar(3),
	@NumberOfRowsDeleted INT OUTPUT
AS
BEGIN
	SET NOCOUNT ON;

	DECLARE @sql nvarchar(max);
	SET @sql = N'
		DELETE [gpm].[Price_' + @Region + ']
		WHERE
			Region = @Region
			AND ItemID = @ItemID
			AND	BusinessUnitID = @BusinessUnitID
			AND StartDate = @StartDate
			AND PriceType = @PriceType';

DECLARE @params NVARCHAR(500);
	SET @params = N'
		@Region nvarchar(2),
		@ItemID int,
		@BusinessUnitID int,
		@StartDate datetime2,
		@PriceType nvarchar(3)';

	EXEC sp_executesql
		@sql,
		@params,
		@Region = @Region,
		@ItemID = @ItemID,
		@BusinessUnitID = @BusinessUnitID,
		@StartDate = @StartDate,
		@PriceType = @PriceType;

		SELECT @NumberOfRowsDeleted = @@ROWCOUNT
END
go