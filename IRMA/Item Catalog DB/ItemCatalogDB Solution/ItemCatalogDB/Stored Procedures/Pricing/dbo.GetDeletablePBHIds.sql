CREATE PROCEDURE [dbo].[GetDeletablePBHIds]
    @PriceBatchHeaderIDs varchar(max)
AS

BEGIN
	SELECT pbd.PriceBatchHeaderId
	  FROM [dbo].[fn_Parse_List] (@PriceBatchHeaderIDs, ',') ids	  
	  JOIN [dbo].[PriceBatchDetail] pbd ON ids.Key_Value = pbd.PriceBatchHeaderId
 LEFT JOIN [mammoth].[PriceChangeQueue] pcq ON pcq.EventReferenceID = pbd.PriceBatchDetailId
       AND pcq.ProcessFailedDate is null
	   WHERE  pcq.QueueID IS NULL
END