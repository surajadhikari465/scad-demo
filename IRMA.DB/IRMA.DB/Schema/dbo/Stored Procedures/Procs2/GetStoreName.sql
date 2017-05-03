CREATE PROCEDURE dbo.GetStoreName
@Store_No int
AS 

SELECT Store_Name
FROM Store 
WHERE Store_No = @Store_No
GO
GRANT EXECUTE
    ON OBJECT::[dbo].[GetStoreName] TO [IRMASupportRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[GetStoreName] TO [IRMAClientRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[GetStoreName] TO [IRMASchedJobsRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[GetStoreName] TO [IRMAReportsRole]
    AS [dbo];

