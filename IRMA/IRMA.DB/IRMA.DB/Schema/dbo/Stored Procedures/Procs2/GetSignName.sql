CREATE PROCEDURE dbo.GetSignName
@Sign_ID int
AS 

SELECT Sign_Desc
FROM Sign
WHERE Sign_ID = @Sign_ID
GO
GRANT EXECUTE
    ON OBJECT::[dbo].[GetSignName] TO [IRMASupportRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[GetSignName] TO [IRMAClientRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[GetSignName] TO [IRMASchedJobsRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[GetSignName] TO [IRMAReportsRole]
    AS [dbo];

