CREATE PROCEDURE dbo.GetOriginAndID 
AS 

SELECT Origin_ID, Origin_Name 
FROM ItemOrigin 
ORDER BY Origin_Name
GO
GRANT EXECUTE
    ON OBJECT::[dbo].[GetOriginAndID] TO [IRMASupportRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[GetOriginAndID] TO [IRMAClientRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[GetOriginAndID] TO [IRMASchedJobsRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[GetOriginAndID] TO [IRMAReportsRole]
    AS [dbo];

