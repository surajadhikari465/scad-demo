-- select all stores that support a specific Pricing method
CREATE PROCEDURE  [dbo].[EPromotions_GetStoresByPricingMethod]
	@PricingMethodId int
AS 
	
BEGIN
    SET NOCOUNT ON
	select  s.* from store s 
		inner join StorePOSConfig spc on s.Store_No = spc.Store_No
		inner join POSWriter pw on spc.POSFileWriterKey = pw.POSFileWriterKey
		inner join POSWriterPricingMethods ppm on pw.POSFileWriterKey = ppm.POSFileWriterKey
		inner join PricingMethod pm on ppm.PricingMethod_Id = pm.PricingMethod_Id
	where pm.PricingMethod_Id  = @PricingMethodId

    SET NOCOUNT OFF
END
GO
GRANT EXECUTE
    ON OBJECT::[dbo].[EPromotions_GetStoresByPricingMethod] TO [IRMAClientRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[EPromotions_GetStoresByPricingMethod] TO [IRMASchedJobsRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[EPromotions_GetStoresByPricingMethod] TO [IRMAReportsRole]
    AS [dbo];

