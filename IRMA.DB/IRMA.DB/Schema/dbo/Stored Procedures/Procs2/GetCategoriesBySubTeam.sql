CREATE PROCEDURE dbo.GetCategoriesBySubTeam
@SubTeam_No as int
AS 
BEGIN
    SET NOCOUNT ON

    SELECT Category_ID, Category_Name
    FROM  ItemCategory (nolock) 
    WHERE SubTeam_No = @SubTeam_No
END
GO
GRANT EXECUTE
    ON OBJECT::[dbo].[GetCategoriesBySubTeam] TO [IRMASupportRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[GetCategoriesBySubTeam] TO [IRMAClientRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[GetCategoriesBySubTeam] TO [IRMASchedJobsRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[GetCategoriesBySubTeam] TO [IRMAReportsRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[GetCategoriesBySubTeam] TO [IRMAExcelRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[GetCategoriesBySubTeam] TO [IRMASLIMRole]
    AS [dbo];

