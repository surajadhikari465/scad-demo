CREATE PROCEDURE dbo.GetPLUMStoreMap

AS

BEGIN
    SET NOCOUNT ON

    SELECT Store_No, PLUMStoreNo
    FROM Store (nolock)
    WHERE PLUMStoreNo IS NOT NULL

    SET NOCOUNT OFF
END
GO
GRANT EXECUTE
    ON OBJECT::[dbo].[GetPLUMStoreMap] TO [IRMASupportRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[GetPLUMStoreMap] TO [IRMAClientRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[GetPLUMStoreMap] TO [IRMASchedJobsRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[GetPLUMStoreMap] TO [IRMAReportsRole]
    AS [dbo];

