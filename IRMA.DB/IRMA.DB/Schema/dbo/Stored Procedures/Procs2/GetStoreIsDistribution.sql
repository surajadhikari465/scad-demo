CREATE PROCEDURE dbo.GetStoreIsDistribution 
@Store_No int
AS 

SELECT Distribution_Center
FROM Store
WHERE Store_No = @Store_No
GO
GRANT EXECUTE
    ON OBJECT::[dbo].[GetStoreIsDistribution] TO [IRMASupportRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[GetStoreIsDistribution] TO [IRMAClientRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[GetStoreIsDistribution] TO [IRMASchedJobsRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[GetStoreIsDistribution] TO [IRMAReportsRole]
    AS [dbo];

