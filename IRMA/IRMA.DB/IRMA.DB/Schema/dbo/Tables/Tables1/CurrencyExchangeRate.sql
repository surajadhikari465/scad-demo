CREATE TABLE [dbo].[CurrencyExchangeRate] (
    [FromCurrency] VARCHAR (3) NOT NULL,
    [ToCurrency]   VARCHAR (3) NOT NULL,
    [Multiplier]   FLOAT (53)  NOT NULL,
    [Divider]      FLOAT (53)  NOT NULL,
    [Date]         DATETIME    NOT NULL
);


GO
CREATE Trigger [CurrencyExchangeRateAdd] 
ON dbo.CurrencyExchangeRate
FOR INSERT
AS

-- **************************************************************************
-- Trigger: CurrencyExchangeRateAdd
--    Author: Faisal Ahmed
--      Date: 01/03/2013
--
-- Description:
-- Trigger for whenever a record is inserted to CurrencyExchangeRate table
--
-- Modification History:
-- Date			Init	TFS		Comment
-- 01/03/2013	FA		9251	This trigger is added to store exchange rate history
--                              to CurrencyExchangeRateHistory table
-- **************************************************************************

BEGIN
	DECLARE @Error_No int
	SELECT @Error_No = 0

	-- Add to history table
	INSERT INTO	CurrencyExchangeRateHistory  
		SELECT *, GETDATE() 
		FROM inserted 
		
	SELECT @Error_No = @@ERROR

	IF @Error_No <> 0
	BEGIN
		DECLARE @Severity smallint
		SELECT @Severity = ISNULL((SELECT severity FROM master.dbo.sysmessages WHERE error = @Error_No), 16)
		RAISERROR ('CurrencyExchangeRateAdd trigger failed with @@ERROR: %d', @Severity, 1, @Error_No)
	END
END
GO
GRANT DELETE
    ON OBJECT::[dbo].[CurrencyExchangeRate] TO [IRMASchedJobs]
    AS [dbo];


GO
GRANT INSERT
    ON OBJECT::[dbo].[CurrencyExchangeRate] TO [IRMASchedJobs]
    AS [dbo];


GO
GRANT SELECT
    ON OBJECT::[dbo].[CurrencyExchangeRate] TO [IRMASchedJobs]
    AS [dbo];


GO
GRANT UPDATE
    ON OBJECT::[dbo].[CurrencyExchangeRate] TO [IRMASchedJobs]
    AS [dbo];

