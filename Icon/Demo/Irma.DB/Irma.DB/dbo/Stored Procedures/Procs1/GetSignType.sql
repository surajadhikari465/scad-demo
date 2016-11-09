CREATE PROCEDURE dbo.GetSignType
@Sign_ID int
AS 

SELECT * 
FROM Sign
WHERE Sign_ID = @Sign_ID
GO
GRANT EXECUTE
    ON OBJECT::[dbo].[GetSignType] TO [IRMASupportRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[GetSignType] TO [IRMAClientRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[GetSignType] TO [IRMASchedJobsRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[GetSignType] TO [IRMAReportsRole]
    AS [dbo];

