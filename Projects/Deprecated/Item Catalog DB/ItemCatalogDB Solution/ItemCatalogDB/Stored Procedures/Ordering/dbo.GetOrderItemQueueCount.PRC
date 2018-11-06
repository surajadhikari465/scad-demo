SET QUOTED_IDENTIFIER ON 
GO
SET ANSI_NULLS ON 
GO

if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[GetOrderItemQueueCount]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[GetOrderItemQueueCount]
GO

CREATE PROCEDURE dbo.GetOrderItemQueueCount
	@PurchasingVendor_ID int,
	@ToSubTeam_No int 
AS 
BEGIN

	SET NOCOUNT ON
		
	DECLARE @Store_No int
	IF @PurchasingVendor_ID > 0 BEGIN SELECT @Store_No = Store_No FROM Vendor WHERE Vendor_ID = @PurchasingVendor_ID END
	
	SELECT 
		COUNT(OrderItemQueue_ID) AS ItemCount 
	FROM 
		OrderItemQueue 
	WHERE 
		Store_No = @Store_No 
		AND 
		TransferToSubTeam_No = @ToSubTeam_No
	
	SET NOCOUNT OFF
END
GO
SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO

