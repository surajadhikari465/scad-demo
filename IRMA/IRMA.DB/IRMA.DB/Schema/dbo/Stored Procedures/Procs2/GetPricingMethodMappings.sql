CREATE PROCEDURE dbo.GetPricingMethodMappings
AS 

SELECT PMI.POSFileWriterKey, PMI.PricingMethod_Key, PMI.PricingMethod_ID, PW.POSFileWriterCode, PM.PricingMethod_Name
FROM POSWriterPricingMethodId PMI
LEFT JOIN POSWriter PW
	 ON PW.POSFileWriterKey = PMI.POSFileWriterKey
LEFT JOIN PricingMethod PM
	 ON PM.PricingMethod_ID = PMI.PricingMethod_Key
GO
GRANT EXECUTE
    ON OBJECT::[dbo].[GetPricingMethodMappings] TO [IRMAAdminRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[GetPricingMethodMappings] TO [IRMAClientRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[GetPricingMethodMappings] TO [IRMASchedJobsRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[GetPricingMethodMappings] TO [IRMAReportsRole]
    AS [dbo];

