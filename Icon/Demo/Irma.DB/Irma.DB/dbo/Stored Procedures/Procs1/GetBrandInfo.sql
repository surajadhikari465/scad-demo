
CREATE PROCEDURE dbo.GetBrandInfo
	@Brand_ID int,
	@User_ID int
AS 

-- **************************************************************************************************
-- Procedure: [GetBrandInfo]
--    Author: unknown
--      Date: unknown
--
-- Modification History:
-- Date       	Init  	TFS   	Comment
-- 2014/04/06	KM		14934	Add join to ValidatedBrand to check for brands that have an association in Icon;
--
-- **************************************************************************************************

BEGIN

    SELECT 
		Brand_Name, 
		Brand_ID, 
		Users.UserName, 
		ItemBrand.User_ID,
		vb.IconBrandId
    FROM 
		Users 
		RIGHT JOIN ItemBrand ON Users.User_ID = ItemBrand.User_ID
		LEFT JOIN ValidatedBrand vb ON ItemBrand.Brand_ID = vb.IrmaBrandId
    WHERE 
		Brand_ID = @Brand_ID

    UPDATE 
		ItemBrand 
	SET 
		User_ID = @User_ID 
    WHERE 
		Brand_ID = @Brand_ID AND User_ID IS NULL

END

GO
GRANT EXECUTE
    ON OBJECT::[dbo].[GetBrandInfo] TO [IRMASupportRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[GetBrandInfo] TO [IRMAClientRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[GetBrandInfo] TO [IRMASchedJobsRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[GetBrandInfo] TO [IRMAReportsRole]
    AS [dbo];

