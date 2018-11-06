
CREATE PROCEDURE dbo.GetBrandAndID 
AS 

-- **************************************************************************************************
-- Procedure: [GetBrandAndID]
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
		Brand_ID, 
		Brand_Name, 
		IconBrandId 
	FROM 
		ItemBrand (nolock)					ib
		LEFT JOIN ValidatedBrand (nolock)	vb	ON	ib.Brand_ID = vb.IrmaBrandId
	ORDER BY 
		Brand_Name

END

GO
GRANT EXECUTE
    ON OBJECT::[dbo].[GetBrandAndID] TO [IRMASupportRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[GetBrandAndID] TO [IRMAClientRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[GetBrandAndID] TO [IRMASchedJobsRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[GetBrandAndID] TO [IRMAReportsRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[GetBrandAndID] TO [IRMAExcelRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[GetBrandAndID] TO [IRMASLIMRole]
    AS [dbo];

