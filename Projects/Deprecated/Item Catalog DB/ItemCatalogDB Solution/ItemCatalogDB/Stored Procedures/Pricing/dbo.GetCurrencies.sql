  if exists (select * from dbo.sysobjects where id = object_id(N'dbo.GetCurrencies') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure dbo.GetCurrencies
GO

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
  