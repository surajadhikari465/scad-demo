SET QUOTED_IDENTIFIER ON 
GO
SET ANSI_NULLS ON 
GO

if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[GetOrdersNoSubstitutionItem]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[GetOrdersNoSubstitutionItem]
GO


CREATE PROCEDURE dbo.GetOrdersNoSubstitutionItem 
    @Identifier varchar(13),
    @SubIdentifier varchar(13),
	@NonRetail bit 
AS 

BEGIN 

	DECLARE	@Item_Key int
	DECLARE @Sub_Item_Key int

	SELECT @Item_Key = Item_Key FROM ItemIdentifier WHERE Identifier = @Identifier
	SELECT @Sub_Item_Key = Item_Key FROM ItemIdentifier WHERE Identifier = @SubIdentifier
 
	SELECT		
		tmp.OrderHeader_ID As PO,
		[Vendor].CompanyName As Location,
		SubTeam.SubTeam_Name As SubTeam,
		@Identifier As Item,
		tmp.QuantityOrdered As Qty, 
		@SubIdentifier As SubItem
	FROM         
		tmpOrdersAllocateOrderItems tmp
			INNER JOIN
					  OrderHeader ON tmp.OrderHeader_ID = OrderHeader.OrderHeader_ID
			INNER JOIN
					  SubTeam ON OrderHeader.Transfer_To_SubTeam = SubTeam.SubTeam_No
			INNER JOIN
					  Vendor ON OrderHeader.ReceiveLocation_ID = [Vendor].Vendor_ID
			INNER JOIN
					  Item ON tmp.Item_Key = Item.Item_Key
	WHERE
	
		tmp.OrderHeader_ID NOT IN(SELECT OrderHeader_ID FROM tmpOrdersAllocateOrderItems
									WHERE Item_Key = @Sub_Item_Key)
		AND tmp.Item_Key = @Item_Key

		AND CASE 
				WHEN OrderHeader.Transfer_SubTeam = OrderHeader.Transfer_To_SubTeam THEN 0 
				ELSE 1 
			END = @NonRetail

								
																
END

GO

--grant exec on dbo.GetOrdersNoSubstitutionItem TO IRMAClientRole, IRMAAdminRole, IRMAReportsRole, IRMASchedJobsRole