-- select all stores that support a specific Pricing method
CREATE PROCEDURE  [dbo].[EPromotions_GetStoresByPromotionId]
	@OfferId int
AS 
	
BEGIN
    SET NOCOUNT ON
select  
 case when exists ( select Offer_id from PromotionalOfferStore where Store_No = s.Store_No and Offer_id = po.Offer_Id) 
		then 
			case when (select active from PromotionalOfferStore where Store_No = s.Store_No and Offer_id = po.Offer_Id) = 1 then 'ACTIVE' else 'YES' end else 'NO' end as IsAssigned,
s.* from store s 
	inner join StorePOSConfig spc on s.Store_No = spc.Store_No
	inner join POSWriter pw on spc.POSFileWriterKey = pw.POSFileWriterKey
	inner join POSWriterPricingMethods ppm on pw.POSFileWriterKey = ppm.POSFileWriterKey
	inner join PricingMethod pm on ppm.PricingMethod_Id = pm.PricingMethod_Id
	inner join PromotionalOffer po on pm.PricingMethod_Id = po.PricingMethod_Id
where po.Offer_Id = @OfferId


    SET NOCOUNT OFF
END
GO
GRANT EXECUTE
    ON OBJECT::[dbo].[EPromotions_GetStoresByPromotionId] TO [IRMAClientRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[EPromotions_GetStoresByPromotionId] TO [IRMASchedJobsRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[EPromotions_GetStoresByPromotionId] TO [IRMAReportsRole]
    AS [dbo];

