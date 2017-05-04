SET QUOTED_IDENTIFIER ON 
GO
SET ANSI_NULLS ON 
GO

if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[GetOrderListUnitID]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[GetOrderListUnitID]
GO


CREATE PROCEDURE dbo.GetOrderListUnitID
@Store_No int,
@Item_Key int
AS

BEGIN

    DECLARE @Unit_ID int

    If (@Store_No IS NULL)
        SELECT @Unit_ID = Vendor_Unit_ID
        FROM Item, Store
        WHERE Item_Key = @Item_Key 

    ELSE

        SELECT @Unit_ID = Distribution_Unit_ID 
        FROM Item, Store
        WHERE Item_Key = @Item_Key AND Store_No = @Store_No


    IF (@Unit_ID IS NULL)
         SELECT @Unit_ID = QuantityUnit FROM OrderItem  WHERE OrderItem_ID =
               (SELECT MAX(OrderItem_ID)
                FROM OrderHeader RIGHT JOIN OrderItem ON (OrderHeader.OrderHeader_ID = OrderItem.OrderHeader_ID)
                WHERE OrderHeader.Transfer_SubTeam IS NULL AND OrderItem.Item_Key =  @Item_Key )

    IF (@Unit_ID IS NULL)
        SELECT @Unit_ID = 1

    SELECT @Unit_ID AS Unit_ID

END
GO
SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO


