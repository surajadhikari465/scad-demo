SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO

if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[DoFSASubstitution]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[DoFSASubstitution]
GO


CREATE PROCEDURE dbo.DoFSASubstitution
	@Identifier varchar(13),
	@SubIdentifier varchar(13),
	@NonRetail bit
AS 
BEGIN
	DECLARE	@Item_Key int
	DECLARE @Sub_Item_Key int

	SELECT @Item_Key = Item_Key FROM ItemIdentifier WHERE Identifier = @Identifier
	SELECT @Sub_Item_Key = Item_Key FROM ItemIdentifier WHERE Identifier = @SubIdentifier

	UPDATE	tmpOrdersAllocateOrderItems
	SET		tmpOrdersAllocateOrderItems.QuantityAllocated = 
					(tmpOrdersAllocateOrderItems.QuantityAllocated + (
																		SELECT		(tmpSource.QuantityOrdered - tmpSource.QuantityAllocated) As SubstitutionQty
																		FROM		tmpOrdersAllocateOrderItems tmpSource
																		INNER JOIN	OrderHeader ON tmpSource.OrderHeader_ID = OrderHeader.OrderHeader_ID
																		WHERE		Item_Key = @Item_Key
																		AND			tmpOrdersAllocateOrderItems.OrderHeader_ID = tmpSource.OrderHeader_ID
																		AND			CASE 
																						WHEN OrderHeader.Transfer_SubTeam = OrderHeader.Transfer_To_SubTeam 
																							THEN 0 
																						ELSE 1
																					END =	@NonRetail))
	WHERE tmpOrdersAllocateOrderItems.Item_Key = @Sub_Item_Key

END

GO
SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO

--grant exec on dbo.DoFSASubstitution TO IRMAClientRole

