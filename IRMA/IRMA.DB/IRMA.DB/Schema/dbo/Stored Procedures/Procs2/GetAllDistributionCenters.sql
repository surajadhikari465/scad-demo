CREATE PROCEDURE dbo.GetAllDistributionCenters
AS 

SELECT Store_Name, Store_No
FROM Store 
WHERE (Distribution_Center = 1 OR Manufacturer = 1) AND Regional = 0
ORDER BY Store_Name
GO
GRANT EXECUTE
    ON OBJECT::[dbo].[GetAllDistributionCenters] TO [IRMASupportRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[GetAllDistributionCenters] TO [IRMAClientRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[GetAllDistributionCenters] TO [IRMASchedJobsRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[GetAllDistributionCenters] TO [IRMAReportsRole]
    AS [dbo];

