SET QUOTED_IDENTIFIER ON 
GO
SET ANSI_NULLS ON 
GO

if exists (select * from dbo.sysobjects where id = object_id(N'dbo.GetOrderAllocOrderItems') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
	drop procedure dbo.GetOrderAllocOrderItems
GO


CREATE PROCEDURE [dbo].[GetOrderAllocOrderItems]
    @Store_No int,
    @SubTeam_No int,
    @NonRetail int
/* 

    grant exec on dbo.GetOrderAllocOrderItems to IRMAAdminRole
    grant exec on dbo.GetOrderAllocOrderItems to IRMAClientRole
    grant exec on dbo.GetOrderAllocOrderItems to IRMASchedJobsRole
    grant exec on dbo.GetOrderAllocOrderItems to public

*/
AS
BEGIN
    SET NOCOUNT ON

INSERT INTO dbo.tmpOrdersAllocateOrderItems
    (
		OrderItem_ID,
		CompanyName,
		OrderHeader_ID,
		Transfer_To_SubTeam,
		Item_Key,
		QuantityOrdered,
		QuantityAllocated,
		Package_Desc1,
		OrigQuantityAllocated,
		OrigPackage_Desc1
	)
    SELECT 

		Orders.OrderItem_ID, 
		Orders.CompanyName, 
		Orders.OrderHeader_ID, 
		Orders.Transfer_To_SubTeam,
		Orders.Item_Key, 
		Orders.QuantityOrdered, 
		Orders.QuantityAllocated, 
		Orders.Package_Desc1, 
		Orders.OriginalQtyAlloc, 
		Orders.OriginalPackAlloc
	FROM 
		(
			SELECT
				OrderItem.OrderItem_ID, 
				RL.CompanyName, 
				OrderItem.OrderHeader_ID, 
				OrderHeader.Transfer_To_SubTeam,
				OrderItem.Item_Key, 
				OrderItem.QuantityOrdered, 
				OrderItem.QuantityAllocated, 
				OrderItem.Package_Desc1, 
				OrderItem.QuantityAllocated As OriginalQtyAlloc, 
				OrderItem.Package_Desc1 As OriginalPackAlloc,
				CASE 
					WHEN OrderHeader.Transfer_SubTeam = OrderHeader.Transfer_To_SubTeam THEN 0 
					ELSE 1 
				END AS Retail
			FROM OrderHeader (NOLOCK)
			INNER JOIN 
				Vendor (NOLOCK) 
				ON (OrderHeader.Vendor_ID = Vendor.Vendor_ID AND Vendor.Store_No = @Store_No)
				AND OrderHeader.OrderType_ID <> 1	-- exclude purchase orders
			INNER JOIN
				Vendor RL (NOLOCK)
				ON RL.Vendor_ID = OrderHeader.ReceiveLocation_ID
			INNER JOIN
				OrderItem (NOLOCK)
				ON (OrderItem.OrderHeader_ID = OrderHeader.OrderHeader_ID AND OrderItem.QuantityOrdered > 0)
			INNER JOIN
				Item (NOLOCK)
				ON (Item.Item_Key = OrderItem.Item_Key)
			INNER JOIN ItemIdentifier (NOLOCK) 
				ON (ItemIdentifier.Item_Key = Item.Item_Key AND ItemIdentifier.Default_Identifier = 1)
			WHERE 
				Transfer_SubTeam = @SubTeam_No 
				--AND @NonRetail = CASE WHEN Transfer_SubTeam <> Transfer_To_SubTeam THEN 1 ELSE 0 END 
				AND CloseDate IS NULL 
				AND DATEDIFF(day, GETDATE(), Expected_Date) = 1 
				AND Sent = 1 
				AND WarehouseSent = 0
    			AND NOT EXISTS (SELECT * FROM OrderItem OI WHERE OI.OrderHeader_ID = OrderHeader.OrderHeader_ID AND DateReceived IS NOT NULL)
		) AS Orders
    WHERE Retail = (CASE WHEN @NonRetail = -1 THEN Retail ELSE @NonRetail END)
    
    SET NOCOUNT OFF
END
GO
SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO


