IF EXISTS (SELECT * FROM sysobjects WHERE type = 'P' AND name = 'GetOrderItemLines')
	BEGIN
		DROP  Procedure  GetOrderItemLines
	END

GO

CREATE PROCEDURE dbo.GetOrderItemLines 
    (@OrderHeader_ID int)
	/*

	Written by Rick Kelleher, Dec 2007
	Used for updating 3rd Party Freight values 

	EXEC dbo.GetOrderItemLines 410

	grant exec on dbo.GetOrderItemLines to IRMAClientRole
	grant exec on dbo.GetOrderItemLines to IRMAReportsRole

	*/
AS
BEGIN
    SET NOCOUNT ON
	
SELECT OrderItem_ID
      ,Freight3Party
      ,LineItemFreight3Party
      ,FreightUnit
      ,QuantityOrdered
      ,QuantityReceived
	FROM OrderItem (nolock)
	where OrderHeader_ID = @OrderHeader_ID
	order by QuantityOrdered desc
	SET NOCOUNT OFF
END
