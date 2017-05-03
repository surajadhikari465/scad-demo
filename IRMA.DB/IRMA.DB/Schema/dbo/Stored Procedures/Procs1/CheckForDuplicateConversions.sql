CREATE PROCEDURE dbo.CheckForDuplicateConversions 
@FromUnit_ID int, 
@ToUnit_ID int 
AS 

SELECT COUNT(*) AS ConversionCount 
FROM ItemConversion 
WHERE FromUnit_ID = @FromUnit_ID AND ToUnit_ID = @ToUnit_ID
GO
GRANT EXECUTE
    ON OBJECT::[dbo].[CheckForDuplicateConversions] TO [IRMASupportRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[CheckForDuplicateConversions] TO [IRMAClientRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[CheckForDuplicateConversions] TO [IRMASchedJobsRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[CheckForDuplicateConversions] TO [IRMAReportsRole]
    AS [dbo];

