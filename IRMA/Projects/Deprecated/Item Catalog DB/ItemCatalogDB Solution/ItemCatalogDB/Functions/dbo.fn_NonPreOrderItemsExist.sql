set ANSI_NULLS ON
set QUOTED_IDENTIFIER ON
go

IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[fn_NonPreOrderItemsExist]') AND type in (N'FN', N'IF', N'TF', N'FS', N'FT'))
DROP FUNCTION [dbo].[fn_NonPreOrderItemsExist]

GO

CREATE FUNCTION [dbo].[fn_NonPreOrderItemsExist]
(
    @OrderHeader_ID int
)
RETURNS Bit
AS
BEGIN
    DECLARE @count int
	DECLARE @result bit

    SELECT @count = COUNT(*)
    FROM OrderItem OI
    JOIN Item I ON I.Item_Key = OI.Item_Key
    WHERE OI.OrderHeader_ID = @OrderHeader_ID AND I.Pre_Order = 0
    
    IF @count > 0
		SELECT @result = 1
	ELSE
		SELECT @result = 0

	RETURN @result
END

GO 