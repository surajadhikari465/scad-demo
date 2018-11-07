CREATE PROCEDURE [dbo].[DeleteOrderItemRefused]
	@OrderItemRefusedID int
AS 
-- **************************************************************************
-- Procedure: DeleteOrderItemRefused()
--    Author: Faisal Ahmed
--      Date: 03/08/2013
--
-- Description:
-- This procedure deletes a record from OrderItemRefused table
--
-- Modification History:
-- Date       	Init  	TFS   	Comment
-- 03/08/2013	FA   	8325	Initial Code
BEGIN

	DELETE OrderItemRefused
	WHERE OrderItemRefusedID = @OrderItemRefusedID
END
SET QUOTED_IDENTIFIER ON
GO
GRANT EXECUTE
    ON OBJECT::[dbo].[DeleteOrderItemRefused] TO [IRMAClientRole]
    AS [dbo];

