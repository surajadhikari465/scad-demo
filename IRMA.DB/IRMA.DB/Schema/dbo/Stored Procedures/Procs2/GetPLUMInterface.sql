CREATE PROCEDURE dbo.GetPLUMInterface

AS

BEGIN
    SET NOCOUNT ON

    SELECT * FROM PLUMInterface
    
    SET NOCOUNT OFF
END
GO
GRANT EXECUTE
    ON OBJECT::[dbo].[GetPLUMInterface] TO [IRMASupportRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[GetPLUMInterface] TO [IRMAClientRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[GetPLUMInterface] TO [IRMASchedJobsRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[GetPLUMInterface] TO [IRMAReportsRole]
    AS [dbo];

