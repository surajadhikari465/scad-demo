CREATE PROCEDURE dbo.DeleteExpiredPrices
	@RegionCode nvarchar(2)
AS 
BEGIN
	DECLARE @sql nvarchar(max) = 
	'DELETE FROM Price_' + @RegionCode + '
		OUTPUT DELETED.*
	WHERE 
		PriceType = ''REG'' AND
		StartDate <
		(SELECT MIN(d.StartDate)
		FROM
			(SELECT TOP 2 StartDate
			FROM Price_' + @RegionCode + ' p2
			WHERE p2.ItemID = Price_' + @RegionCode + '.ItemID
					AND p2.BusinessUnitID = Price_' + @RegionCode + '.BusinessUnitID
					AND p2.StartDate < GETDATE()
					AND p2.PriceType = ''REG''
			ORDER BY StartDate DESC) AS d)'

	EXEC sp_executesql @sql
END