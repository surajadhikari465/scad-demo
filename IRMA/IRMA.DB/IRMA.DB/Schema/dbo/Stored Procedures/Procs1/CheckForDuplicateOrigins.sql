CREATE PROCEDURE dbo.CheckForDuplicateOrigins 
@Origin_ID int, 
@Origin_Name varchar(25) 
AS 

SELECT COUNT(*) AS OriginCount 
FROM ItemOrigin 
WHERE Origin_Name = @Origin_Name AND 
      Origin_ID <> @Origin_ID
GO
GRANT EXECUTE
    ON OBJECT::[dbo].[CheckForDuplicateOrigins] TO [IRMASupportRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[CheckForDuplicateOrigins] TO [IRMAClientRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[CheckForDuplicateOrigins] TO [IRMASchedJobsRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[CheckForDuplicateOrigins] TO [IRMAReportsRole]
    AS [dbo];

