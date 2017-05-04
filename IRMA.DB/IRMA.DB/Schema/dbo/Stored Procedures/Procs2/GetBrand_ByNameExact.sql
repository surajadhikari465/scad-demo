CREATE PROCEDURE dbo.GetBrand_ByNameExact
	@Brand_Name varchar(25)
AS 

SELECT 
	Brand_ID,
	Brand_Name,
	User_ID
FROM 
	ItemBrand
WHERE 
	Brand_Name = @Brand_Name
GO
GRANT EXECUTE
    ON OBJECT::[dbo].[GetBrand_ByNameExact] TO [IRMASLIMRole]
    AS [dbo];

