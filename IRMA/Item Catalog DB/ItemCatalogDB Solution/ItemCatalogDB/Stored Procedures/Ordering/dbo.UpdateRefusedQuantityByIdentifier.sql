
IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = object_id(N'[dbo].[UpdateRefusedQuantityByIdentifier]') and type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[UpdateRefusedQuantityByIdentifier]
GO

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