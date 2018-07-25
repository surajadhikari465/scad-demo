CREATE PROCEDURE [dbo].[DeletePricesForClosedStores]
AS
BEGIN
	DECLARE @today DATETIME = CAST(GETDATE() AS DATE)
	
	PRINT CONVERT(nvarchar, SYSDATETIME(), 121) + ', Beginning [DeletePricesForClosedStores].'
	PRINT CONVERT(nvarchar, SYSDATETIME(), 121) + ', Deleting Prices for Closed Stores in FL.'

	DELETE dbo.Price_FL
	WHERE BusinessUnitID IN
	(
		SELECT BusinessUnitID
		FROM dbo.Locales_FL
		WHERE LocaleCloseDate < @today
	)
	
	PRINT CONVERT(nvarchar, SYSDATETIME(), 121) + ', Deleting Prices for Closed Stores in MA.'

	DELETE dbo.Price_MA
	WHERE BusinessUnitID IN
	(
		SELECT BusinessUnitID
		FROM dbo.Locales_MA
		WHERE LocaleCloseDate < @today
	)

	PRINT CONVERT(nvarchar, SYSDATETIME(), 121) + ', Deleting Prices for Closed Stores in MW.'

	DELETE dbo.Price_MW
	WHERE BusinessUnitID IN
	(
		SELECT BusinessUnitID
		FROM dbo.Locales_MW
		WHERE LocaleCloseDate < @today
	)
	
	PRINT CONVERT(nvarchar, SYSDATETIME(), 121) + ', Deleting Prices for Closed Stores in NA.'

	DELETE dbo.Price_NA
	WHERE BusinessUnitID IN
	(
		SELECT BusinessUnitID
		FROM dbo.Locales_NA
		WHERE LocaleCloseDate < @today
	)
	
	PRINT CONVERT(nvarchar, SYSDATETIME(), 121) + ', Deleting Prices for Closed Stores in NC.'

	DELETE dbo.Price_NC
	WHERE BusinessUnitID IN
	(
		SELECT BusinessUnitID
		FROM dbo.Locales_NC
		WHERE LocaleCloseDate < @today
	)
	
	PRINT CONVERT(nvarchar, SYSDATETIME(), 121) + ', Deleting Prices for Closed Stores in NE.'

	DELETE dbo.Price_NE
	WHERE BusinessUnitID IN
	(
		SELECT BusinessUnitID
		FROM dbo.Locales_NE
		WHERE LocaleCloseDate < @today
	)
	
	PRINT CONVERT(nvarchar, SYSDATETIME(), 121) + ', Deleting Prices for Closed Stores in PN.'

	DELETE dbo.Price_PN
	WHERE BusinessUnitID IN
	(
		SELECT BusinessUnitID
		FROM dbo.Locales_PN
		WHERE LocaleCloseDate < @today
	)
	
	PRINT CONVERT(nvarchar, SYSDATETIME(), 121) + ', Deleting Prices for Closed Stores in RM.'

	DELETE dbo.Price_RM
	WHERE BusinessUnitID IN
	(
		SELECT BusinessUnitID
		FROM dbo.Locales_RM
		WHERE LocaleCloseDate < @today
	)
	
	PRINT CONVERT(nvarchar, SYSDATETIME(), 121) + ', Deleting Prices for Closed Stores in SO.'

	DELETE dbo.Price_SO
	WHERE BusinessUnitID IN
	(
		SELECT BusinessUnitID
		FROM dbo.Locales_SO
		WHERE LocaleCloseDate < @today
	)
	
	PRINT CONVERT(nvarchar, SYSDATETIME(), 121) + ', Deleting Prices for Closed Stores in SP.'

	DELETE dbo.Price_SP
	WHERE BusinessUnitID IN
	(
		SELECT BusinessUnitID
		FROM dbo.Locales_SP
		WHERE LocaleCloseDate < @today
	)
	
	PRINT CONVERT(nvarchar, SYSDATETIME(), 121) + ', Deleting Prices for Closed Stores in SW.'

	DELETE dbo.Price_SW
	WHERE BusinessUnitID IN
	(
		SELECT BusinessUnitID
		FROM dbo.Locales_SW
		WHERE LocaleCloseDate < @today
	)
	
	PRINT CONVERT(nvarchar, SYSDATETIME(), 121) + ', Deleting Prices for Closed Stores in TS.'

	DELETE dbo.Price_TS
	WHERE BusinessUnitID IN
	(
		SELECT BusinessUnitID
		FROM dbo.Locales_TS
		WHERE LocaleCloseDate < @today
	)
	
	PRINT CONVERT(nvarchar, SYSDATETIME(), 121) + ', Deleting Prices for Closed Stores in UK.'

	DELETE dbo.Price_UK
	WHERE BusinessUnitID IN
	(
		SELECT BusinessUnitID
		FROM dbo.Locales_UK
		WHERE LocaleCloseDate < @today
	)
	
	PRINT CONVERT(nvarchar, SYSDATETIME(), 121) + ', Finished [DeletePricesForClosedStores].'
END
