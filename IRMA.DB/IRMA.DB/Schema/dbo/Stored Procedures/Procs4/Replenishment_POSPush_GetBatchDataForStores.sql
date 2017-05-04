﻿

CREATE PROCEDURE [dbo].[Replenishment_POSPush_GetBatchDataForStores]
(
	@Date DATETIME,
	@MaxBatchItems INT
)
AS

BEGIN
SET NOCOUNT ON
SET TRANSACTION ISOLATION LEVEL SNAPSHOT

DECLARE @CurrDay smalldatetime
SELECT @CurrDay = CONVERT(smalldatetime, CONVERT(varchar(255), @Date, 101))

--get the data for the price change headers
SELECT PriceBatchHeaderID, Store_No, StartDate, AutoApplyFlag, ApplyDate, BatchDescription, POSBatchID, NULL AS StoreItemAuthorizationID, 'POSPriceChangeHeaders' AS RowType
FROM dbo.fn_GetPriceBatchHeadersForPushing(@Date, 'false', @MaxBatchItems)
-- get the data for the price batch headers that contain the item deletes
UNION ALL	
SELECT PriceBatchHeaderID, Store_No, StartDate, AutoApplyFlag, ApplyDate, BatchDescription, POSBatchID, NULL AS StoreItemAuthorizationID, 'POSDeleteItemHeaders' AS RowType
FROM dbo.fn_GetPriceBatchHeadersForPushing(@Date, 'true', @MaxBatchItems)
--get the data for the batches with batch change type as 'Offers'
UNION ALL
SELECT PriceBatchHeaderID, Store_No, StartDate, NULL AS AutoApplyFlag, NULL AS ApplyDate, NULL AS BatchDescription
	, NULL AS POSBatchId, NULL AS StoreItemAuthorizationID, 'POSPromoOfferHeaders' AS RowType
FROM 
    (SELECT PBD.PriceBatchHeaderID, PBD.Store_No, SentDate, PBH.StartDate, COUNT(1) As TotalItems
     FROM PriceBatchDetail PBD  
     INNER JOIN PriceBatchHeader PBH  ON PBD.PriceBatchHeaderID = PBH.PriceBatchHeaderID
     WHERE PriceBatchStatusID = 5
        AND PBH.StartDate <= @CurrDay
        AND ISNULL(PBH.ItemChgTypeID, 0) = 4 --OFFERS
     GROUP BY PBD.PriceBatchHeaderID, PBD.Store_No, SentDate, PBH.StartDate) PBH
order by RowType, Store_No, PriceBatchHeaderID

END


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[Replenishment_POSPush_GetBatchDataForStores] TO [IRMAClientRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[Replenishment_POSPush_GetBatchDataForStores] TO [IRSUser]
    AS [dbo];

