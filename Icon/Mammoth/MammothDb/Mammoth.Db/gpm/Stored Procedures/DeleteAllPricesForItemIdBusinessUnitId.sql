CREATE PROCEDURE [gpm].[DeleteAllPricesForItemIdBusinessUnitId]
    @Region nvarchar(2),
    @ItemID int,
    @BusinessUnitID int
AS
BEGIN
    SET NOCOUNT ON;

    DECLARE @sql nvarchar(max);
    SET @sql = N'
        DELETE [gpm].[Price_' + @Region + ']
        WHERE
            Region = @Region
            AND ItemID = @ItemID
            AND BusinessUnitID = @BusinessUnitID';

    DECLARE @params NVARCHAR(500);
    SET @params = N'
        @Region nvarchar(2),
        @ItemID int,
        @BusinessUnitID int';

    EXEC sp_executesql
        @sql,
        @params,
        @Region,
        @ItemID,
		@BusinessUnitID
END

GO
GRANT EXEC ON [gpm].[DeleteAllPricesForItemIdBusinessUnitId] TO TibcoRole
GO
    
GRANT EXEC ON [gpm].[DeleteAllPricesForItemIdBusinessUnitId] TO MammothRole
GO