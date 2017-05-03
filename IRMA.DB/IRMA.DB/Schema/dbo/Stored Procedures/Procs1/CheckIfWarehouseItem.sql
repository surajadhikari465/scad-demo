CREATE PROCEDURE dbo.CheckIfWarehouseItem
    @Item_Key int
AS

BEGIN
    SET NOCOUNT ON

    SELECT dbo.fn_IsEXEDistributed(@Item_Key)

    SET NOCOUNT OFF
END
GO
GRANT EXECUTE
    ON OBJECT::[dbo].[CheckIfWarehouseItem] TO [IRMASupportRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[CheckIfWarehouseItem] TO [IRMAClientRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[CheckIfWarehouseItem] TO [IRMASchedJobsRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[CheckIfWarehouseItem] TO [IRMAReportsRole]
    AS [dbo];

