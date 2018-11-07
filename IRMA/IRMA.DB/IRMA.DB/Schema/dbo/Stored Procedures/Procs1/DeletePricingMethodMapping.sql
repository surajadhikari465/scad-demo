CREATE PROCEDURE dbo.DeletePricingMethodMapping
@POSFileWriterKey int,
@PricingMethod_Key int,
@PricingMethod_ID int
AS 

DELETE FROM POSWriterPricingMethodId
WHERE POSFileWriterKey = @POSFileWriterKey AND PricingMethod_Key = @PricingMethod_Key AND PricingMethod_ID = @PricingMethod_ID
GO
GRANT EXECUTE
    ON OBJECT::[dbo].[DeletePricingMethodMapping] TO [IRMAAdminRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[DeletePricingMethodMapping] TO [IRMAClientRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[DeletePricingMethodMapping] TO [IRMASchedJobsRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[DeletePricingMethodMapping] TO [IRMAReportsRole]
    AS [dbo];

