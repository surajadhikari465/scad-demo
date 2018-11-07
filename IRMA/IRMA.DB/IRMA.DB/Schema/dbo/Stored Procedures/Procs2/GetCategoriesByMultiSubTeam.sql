CREATE PROCEDURE [dbo].[GetCategoriesByMultiSubTeam]
	@SubteamList varchar (8000)
AS 
BEGIN
    SET NOCOUNT ON

    SELECT 
		Category_ID, 
		Category_Name
    FROM  
		ItemCategory ic (nolock)
		INNER JOIN dbo.fn_Parse_List(''+ @SubteamList + '', ',') stl
			ON stl.Key_Value = ic.SubTeam_No 
    SET NOCOUNT OFF
END
GO
GRANT EXECUTE
    ON OBJECT::[dbo].[GetCategoriesByMultiSubTeam] TO [IRMAReportsRole]
    AS [dbo];

