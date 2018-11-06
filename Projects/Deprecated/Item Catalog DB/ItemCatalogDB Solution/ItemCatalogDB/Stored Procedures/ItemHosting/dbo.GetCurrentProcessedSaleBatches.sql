if exists (select * from dbo.sysobjects where id = object_id(N'dbo.GetCurrentProcessedSaleBatches') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure dbo.GetCurrentProcessedSaleBatches
GO

CREATE PROCEDURE dbo.GetCurrentProcessedSaleBatches
    @Item_Key int,
    @Store_No int
AS

BEGIN
    SET NOCOUNT ON

	--THIS RETURNS BATCH DATA THAT HAS BEEN PROCESSED BY THE POS PUSH ALREADY
	--AND MUST INCLUDE PRICE CHANGES THAT ARE STILL IN EFFECT (SALE_END_DATE HAS NOT OCCURRED YET)
    SELECT PriceBatchDetailID,
       PBD.Store_No,
       Store_Name,
       PBD.StartDate, 
       PBD.PriceChgTypeID,
       PCT.PriceChgTypeDesc, 
       Multiple,
       Price,
	   POSPrice,       
       Sale_Multiple,
       Sale_Price,
	   POSSale_Price,
       Sale_End_Date,       
       PCT.On_Sale,
       PCT.Priority
	FROM PriceBatchDetail PBD (nolock)
	INNER JOIN
		Store (nolock)
		ON Store.Store_No = PBD.Store_No
	INNER JOIN
		PriceChgType (nolock) PCT
		ON PCT.PriceChgTypeID = PBD.PriceChgTypeID
	LEFT JOIN
		PriceBatchHeader PBH (nolock)
		ON PBD.PriceBatchHeaderID = PBH.PriceBatchHeaderID
	WHERE PBD.Item_Key = @Item_Key
		AND PBD.Store_No = @Store_No
		AND PBH.PriceBatchStatusID = 6 --PROCESSED BATCHES ONLY
		AND PBD.PriceChgTypeID IS NOT NULL --PRICE CHANGES REQUIRED
		AND PCT.On_Sale = 1 --MUST BE A "SALE" PRICE CHANGE TYPE
		AND Expired = 0 --EXCLUDE EXPIRED PRICE BATCHES
		AND Sale_Price IS NOT NULL
		AND (Sale_End_Date IS NULL OR Sale_End_Date > GetDate())  --SALE IS STILL ONGOING			
	ORDER BY PBD.StartDate, PCT.Priority

    SET NOCOUNT OFF
END

GO
 