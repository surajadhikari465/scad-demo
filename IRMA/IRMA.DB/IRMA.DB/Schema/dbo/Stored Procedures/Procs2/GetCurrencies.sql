CREATE PROCEDURE dbo.GetCurrencies 
AS

BEGIN

	SET NOCOUNT ON

	SELECT CurrencyID, CurrencyCode, CurrencyName
	FROM Currency
	WHERE IsDeleted = 0

	SET NOCOUNT OFF

END
GO
GRANT EXECUTE
    ON OBJECT::[dbo].[GetCurrencies] TO [IRMAAdminRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[GetCurrencies] TO [IRMAClientRole]
    AS [dbo];

