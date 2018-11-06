CREATE PROCEDURE dbo.CheckForDuplicateManifest
@DistLoc_ID int, 
@Customer_ID int 
AS 
SELECT COUNT(*) AS ManifestCount 
FROM Manifest 
WHERE Customer_ID = @Customer_ID AND 
DistLoc_ID = @DistLoc_ID AND 
ShipDate IS NULL
GO
GRANT EXECUTE
    ON OBJECT::[dbo].[CheckForDuplicateManifest] TO [IRMASupportRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[CheckForDuplicateManifest] TO [IRMAClientRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[CheckForDuplicateManifest] TO [IRMASchedJobsRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[CheckForDuplicateManifest] TO [IRMAReportsRole]
    AS [dbo];

