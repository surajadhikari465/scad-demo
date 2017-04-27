IF EXISTS (SELECT * 
	   FROM   sysobjects 
	   WHERE  name = N'fn_IsRefusalAllowed')
	DROP FUNCTION fn_IsRefusalAllowed
GO


SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER OFF
GO

CREATE FUNCTION [dbo].[fn_IsRefusalAllowed]
(
	@OrderHeader_ID	INT
)
RETURNS BIT
AS
BEGIN
	DECLARE @IsOrderSent			INT
	DECLARE @IsShortpayProhibited	INT
	DECLARE @IsRefusalAllowed		BIT

	SELECT
		@IsShortpayProhibited	= v.ShortpayProhibited,
		@IsOrderSent			= oh.Sent
	FROM OrderHeader oh INNER JOIN Vendor v
	ON oh.Vendor_ID = v.Vendor_ID
	WHERE	oh.OrderHeader_ID	= @OrderHeader_ID
			
	IF @IsShortpayProhibited = 0 AND @IsOrderSent = 1
		SET @IsRefusalAllowed = 1
	ELSE
		SET @IsRefusalAllowed = 0
			
	RETURN @IsRefusalAllowed
END
GO