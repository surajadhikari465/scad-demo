CREATE PROCEDURE [dbo].[UpdateRefusedQuantityByIdentifier]
	@OrderHeader_ID		int,
	@Identifier			varchar(13),
	@Quantity			decimal(18,4)
AS
-- *************************************************************************************************
-- Function: UpdateRefusedQuantityByIdentifier
--    Author: Faisal Ahmed
--      Date: 03/25/2013
--
-- Description: This procedure updates the refused total amount for a UPC
--
-- Modification History:
-- Date       	Init  	TFS   	Comment
-- 03/25/2013	FA		8325	Initial Code
-- *************************************************************************************************

BEGIN    
    UPDATE
		OrderItemRefused
	SET
		RefusedQuantity = @Quantity
    WHERE 
		OrderHeader_ID = @OrderHeader_ID and Identifier = @Identifier
END
GO
GRANT EXECUTE
    ON OBJECT::[dbo].[UpdateRefusedQuantityByIdentifier] TO [IRMAAdminRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[UpdateRefusedQuantityByIdentifier] TO [IRMAClientRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[UpdateRefusedQuantityByIdentifier] TO [IRMASchedJobsRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[UpdateRefusedQuantityByIdentifier] TO [IRMAReportsRole]
    AS [dbo];

