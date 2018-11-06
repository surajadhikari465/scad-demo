
CREATE PROCEDURE dbo.GetBrandInfoFirst
	@User_ID int
AS 

-- **************************************************************************************************
-- Procedure: [GetBrandInfoFirst]
--    Author: unknown
--      Date: unknown
--
-- Modification History:
-- Date       	Init  	TFS   	Comment
-- 2014/04/06	KM		14934	Add join to ValidatedBrand to check for brands that have an association in Icon;
--
-- **************************************************************************************************

BEGIN

    DECLARE @FirstBrand int

    SELECT @FirstBrand = MIN(Brand_ID) FROM ItemBrand

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
		Brand_ID = @FirstBrand

    UPDATE 
		ItemBrand 
    SET 
		User_ID = @User_ID
    WHERE 
		Brand_ID = @FirstBrand AND User_ID IS NULL

END

GO
GRANT EXECUTE
    ON OBJECT::[dbo].[GetBrandInfoFirst] TO [IRMASupportRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[GetBrandInfoFirst] TO [IRMAClientRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[GetBrandInfoFirst] TO [IRMASchedJobsRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[GetBrandInfoFirst] TO [IRMAReportsRole]
    AS [dbo];

