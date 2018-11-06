CREATE PROCEDURE dbo.GetBrandLockStatus 
@Brand_ID int 
AS 

SELECT User_ID 

FROM ItemBrand 

WHERE Brand_ID = @Brand_ID
GO
GRANT EXECUTE
    ON OBJECT::[dbo].[GetBrandLockStatus] TO [IRMASupportRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[GetBrandLockStatus] TO [IRMAClientRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[GetBrandLockStatus] TO [IRMASchedJobsRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[GetBrandLockStatus] TO [IRMAReportsRole]
    AS [dbo];

