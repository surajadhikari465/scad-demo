CREATE FUNCTION [dbo].[fn_IsValidRefusedItemList]
(
	@OrderHeader_ID	INT
)
RETURNS BIT

AS

-- **************************************************************************************************************************
--  Function: fn_IsValidRefusedItemList()
--    Author: Faisal Ahmed
--      Date: 03/11/2013
--
-- Description:
-- This function checks whether all the refused items have refused quantity and invoice costs populated
--
-- Modification History:
-- Date       	Init  	TFS   	Comment
-- 03/30/2013	FA   	8325	Initial code
-- 2013-04-03	KM		11804	Add @OrderHeader_ID check to the IF EXISTS clause;
-- **************************************************************************************************************************

BEGIN
	DECLARE @TotalRefusedItems	Int
	
	SELECT 
		@TotalRefusedItems = COUNT(*)
	FROM
		OrderItemRefused (nolock)
	WHERE
		OrderHeader_ID = @OrderHeader_ID
		
	IF @TotalRefusedItems <= 0 RETURN 1
	
	IF EXISTS 
		(
			SELECT 
				* 
			FROM 
				OrderItemRefused oir (nolock) 
			WHERE 
				oir.OrderHeader_ID = @OrderHeader_ID
				AND (ISNULL(oir.InvoiceCost, 0) <= 0.0 OR ISNULL(oir.RefusedQuantity, 0) <= 0.0)
		)
		BEGIN
			RETURN 0
		END
	
	RETURN 1
END
GO
GRANT EXECUTE
    ON OBJECT::[dbo].[fn_IsValidRefusedItemList] TO [IRMAClientRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[fn_IsValidRefusedItemList] TO [IRMAReportsRole]
    AS [dbo];

