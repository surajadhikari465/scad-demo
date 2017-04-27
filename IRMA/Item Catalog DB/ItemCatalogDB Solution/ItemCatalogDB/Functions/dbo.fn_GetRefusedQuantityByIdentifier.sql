
IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = object_id(N'[dbo].[fn_GetRefusedQuantityByIdentifier]') and xtype in (N'FN', N'IF', N'TF'))
DROP FUNCTION [dbo].[fn_GetRefusedQuantityByIdentifier]
GO

CREATE FUNCTION [dbo].[fn_GetRefusedQuantityByIdentifier]
    (
		@OrderHeader_ID		int,
		@Identifier			varchar(13)
)
RETURNS Decimal(18,4)
AS
-- *************************************************************************************************
-- Function: fn_GetRefusedQuantityByIdentifier
--    Author: Faisal Ahmed
--      Date: 03/25/2013
--
-- Description: This function returns the refused total amount for a UPC
--
-- Modification History:
-- Date       	Init  	TFS   	Comment
-- 03/25/2013	FA		8325	Initial Code
-- *************************************************************************************************

BEGIN
	DECLARE	@QuantityRefused	decimal(18,4)
    
    SELECT 
		@QuantityRefused = oir.RefusedQuantity	 
    FROM
		OrderItemRefused	(nolock) oir
    WHERE 
		oir.OrderHeader_ID = @OrderHeader_ID and oir.Identifier = @Identifier

	RETURN ISNULL(@QuantityRefused, 0)	
END

GO