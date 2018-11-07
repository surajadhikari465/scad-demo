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
GRANT EXECUTE
    ON OBJECT::[dbo].[GetOrderListUnitID] TO [IRMASupportRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[GetOrderListUnitID] TO [IRMAClientRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[GetOrderListUnitID] TO [IRMASchedJobsRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[GetOrderListUnitID] TO [IRMAReportsRole]
    AS [dbo];

