
CREATE FUNCTION app.fn_GetNextProductKey() RETURNS INT
AS
BEGIN
	declare @ProductKey int = (select max(productKey) + 1 from dbo.Item)
	return @ProductKey
END
GO
