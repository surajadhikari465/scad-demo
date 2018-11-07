CREATE PROCEDURE dbo.GetWarehouses

AS

BEGIN
    SET NOCOUNT ON

    SELECT Store_No, Store_Name, BusinessUnit_ID, ISNULL(EXEWarehouse, 0) As EXEWarehouse
    FROM Store
    WHERE Distribution_Center = 1

    SET NOCOUNT OFF
END
GO
GRANT EXECUTE
    ON OBJECT::[dbo].[GetWarehouses] TO [IRMASupportRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[GetWarehouses] TO [IRMAClientRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[GetWarehouses] TO [IRMASchedJobsRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[GetWarehouses] TO [IRMAReportsRole]
    AS [dbo];

