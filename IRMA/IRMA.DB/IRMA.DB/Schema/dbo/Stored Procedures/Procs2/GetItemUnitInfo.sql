CREATE PROCEDURE dbo.GetItemUnitInfo
    @Item_Key int
AS

BEGIN
    SET NOCOUNT ON

    SELECT Package_Desc1, Package_Desc2, Package_Unit_ID, 
           PU.Unit_Name As PU_Name
    FROM Item WITH (nolock)
    INNER JOIN
        ItemUnit PU WITH (nolock)
        ON PU.Unit_ID = Item.Package_Unit_ID    
    WHERE Item_Key = @Item_Key

    SET NOCOUNT OFF
END
GO
GRANT EXECUTE
    ON OBJECT::[dbo].[GetItemUnitInfo] TO [IRMASupportRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[GetItemUnitInfo] TO [IRMAClientRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[GetItemUnitInfo] TO [IRMASchedJobsRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[GetItemUnitInfo] TO [IRMAReportsRole]
    AS [dbo];

