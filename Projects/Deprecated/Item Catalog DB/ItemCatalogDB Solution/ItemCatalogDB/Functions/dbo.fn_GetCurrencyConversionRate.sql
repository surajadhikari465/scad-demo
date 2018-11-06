 IF EXISTS (SELECT * 
	   FROM   sysobjects 
	   WHERE  name = N'fn_GetCurrencyConversionRate')
	DROP FUNCTION fn_GetCurrencyConversionRate
GO

-- =======================================================================================================
-- Author:		Faisal Ahmed
-- Create date: 12/15/2012
-- Description:	Calculate the exchange rate between two currencies
-- =======================================================================================================

CREATE FUNCTION [dbo].[fn_GetCurrencyConversionRate]
(
    @FromCurrencyID int,
	@ToCurrencyID int
)

RETURNS float
AS
BEGIN 
	DECLARE @FromCurrency	varchar(3)
	DECLARE @TOCurrency		varchar(3)
	DECLARE @ConversionRate float
	
	SELECT @FromCurrency	= C.CurrencyCode From Currency C WHERE C.CurrencyID = @FromCurrencyID
	SELECT @ToCurrency		= C.CurrencyCode From Currency C WHERE C.CurrencyID = @ToCurrencyID

	IF @FromCurrencyID != @ToCurrencyID
		BEGIN	
			SELECT @ConversionRate = Multiplier/Divider 
			FROM CurrencyExchangeRate
			WHERE FromCurrency = @FromCurrency and ToCurrency = @ToCurrency 

		END
	ELSE
		SELECT @ConversionRate = 1

	RETURN isnull(@ConversionRate,1)
END
GO