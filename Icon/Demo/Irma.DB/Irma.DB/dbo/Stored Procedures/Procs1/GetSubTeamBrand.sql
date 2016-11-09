CREATE PROCEDURE dbo.GetSubTeamBrand 
	@SubTeam_No int
AS 

SET NOCOUNT ON

SELECT DISTINCT 
	ItemBrand.Brand_ID, ItemBrand.Brand_Name 
FROM 
	ItemBrand (NOLOCK)
INNER JOIN
	Item (NOLOCK) ON (Item.Brand_ID = ItemBrand.Brand_ID)
WHERE
	Item.SubTeam_No = @SubTeam_No
Order by ItemBrand.Brand_Name

SET NOCOUNT OFF
GO
GRANT EXECUTE
    ON OBJECT::[dbo].[GetSubTeamBrand] TO [IRMASupportRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[GetSubTeamBrand] TO [IRMAClientRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[GetSubTeamBrand] TO [IRMASchedJobsRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[GetSubTeamBrand] TO [IRMAReportsRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[GetSubTeamBrand] TO [IRMASLIMRole]
    AS [dbo];

