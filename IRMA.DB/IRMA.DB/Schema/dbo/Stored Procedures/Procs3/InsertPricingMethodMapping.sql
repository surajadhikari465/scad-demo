CREATE PROCEDURE dbo.InsertPricingMethodMapping
@POSFileWriterKey int,
@PricingMethod_Key int,
@PricingMethod_ID int
AS 

INSERT INTO POSWriterPricingMethodId (POSFileWriterKey,PricingMethod_Key,PricingMethod_ID) 
VALUES (@POSFileWriterKey,@PricingMethod_Key,@PricingMethod_ID)
GO
GRANT EXECUTE
    ON OBJECT::[dbo].[InsertPricingMethodMapping] TO [IRMAAdminRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[InsertPricingMethodMapping] TO [IRMAClientRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[InsertPricingMethodMapping] TO [IRMASchedJobsRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[InsertPricingMethodMapping] TO [IRMAReportsRole]
    AS [dbo];

