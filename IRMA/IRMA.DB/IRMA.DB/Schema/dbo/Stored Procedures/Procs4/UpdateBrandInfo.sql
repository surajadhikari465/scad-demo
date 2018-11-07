CREATE PROCEDURE dbo.UpdateBrandInfo
@Brand_ID int,
@Brand_Name varchar(50)
AS 

UPDATE ItemBrand
SET User_ID = NULL,
    Brand_Name = @Brand_Name
WHERE Brand_ID = @Brand_ID
GO
GRANT EXECUTE
    ON OBJECT::[dbo].[UpdateBrandInfo] TO [IRMAClientRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[UpdateBrandInfo] TO [IRMASchedJobsRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[UpdateBrandInfo] TO [IRMAReportsRole]
    AS [dbo];

