
CREATE PROCEDURE dbo.DeleteExpiredSales
	@RegionCode nvarchar(2)
AS 
BEGIN
	DECLARE @sql nvarchar(max) = 
	'DELETE Price_' + @RegionCode +'
		OUTPUT DELETED.*
	WHERE PriceType <> ''REG'' AND EndDate < GETDATE() - 14'

	EXEC sp_executesql @sql
END