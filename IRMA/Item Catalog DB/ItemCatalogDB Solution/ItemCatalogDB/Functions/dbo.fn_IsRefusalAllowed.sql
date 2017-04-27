IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[fn_IsRefusalAllowed]') AND type in (N'FN', N'IF', N'TF', N'FS', N'FT'))
DROP FUNCTION [dbo].[fn_IsRefusalAllowed]
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
-- **************************************************************************************************************************
--  Function: fn_IsRefusalAllowed()
--    Author: Faisal Ahmed
--      Date: 03/11/2013
--
-- Description:
-- This function checks whether refusals are allowed for the input PO
--
-- Modification History:
-- Date       	Init  	TFS   	Comment
-- 03/11/2013	FA   	8325	Initial code
-- **************************************************************************************************************************

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
		SELECT @IsRefusalAllowed = 1
	ELSE
		SELECT @IsRefusalAllowed = 0
			
	RETURN @IsRefusalAllowed
END
GO