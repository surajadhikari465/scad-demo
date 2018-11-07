CREATE PROCEDURE dbo.CheckForDuplicateBrands 
@Brand_ID int, 
@Brand_Name varchar(25) 
AS 

SELECT COUNT(*) AS BrandCount 
FROM ItemBrand 
WHERE Brand_Name = @Brand_Name AND Brand_ID <> @Brand_ID
GO
GRANT EXECUTE
    ON OBJECT::[dbo].[CheckForDuplicateBrands] TO [IRMASupportRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[CheckForDuplicateBrands] TO [IRMAClientRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[CheckForDuplicateBrands] TO [IRMASchedJobsRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[CheckForDuplicateBrands] TO [IRMAReportsRole]
    AS [dbo];

