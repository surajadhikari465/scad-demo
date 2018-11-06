/****** Object:  StoredProcedure [dbo].[fn_GetPriceBatchHeadersForPushing]    Script Date: 06/04/2007  ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[fn_GetPriceBatchHeadersForPushing]') AND type in (N'FN', N'IF', N'TF', N'FS', N'FT'))
DROP FUNCTION [dbo].[fn_GetPriceBatchHeadersForPushing]
GO

/****** Object:  StoredProcedure [dbo].[fn_GetPriceBatchHeadersForPushing]    Script Date: 06/04/2007  ******/
CREATE FUNCTION dbo.fn_GetPriceBatchHeadersForPushing (
    @Date datetime,
	@Deletes bit,
	@MaxBatchItems int
	)
RETURNS @pbhSend TABLE (
	PriceBatchHeaderID int, 
	Store_No int, 
	StartDate smalldatetime, 
	AutoApplyFlag bit, 
	ApplyDate smalldatetime, 
	BatchDescription varchar(30), 
	POSBatchID int
 	)
AS
BEGIN
    DECLARE @CurrDay smalldatetime
    SELECT @CurrDay = CONVERT(smalldatetime, CONVERT(varchar(255), @Date, 101))

    DECLARE @Store TABLE (Store_No int, BatchRecords int)
    DECLARE @BatchRecords int
    
    INSERT INTO @pbhSend (PriceBatchHeaderID, Store_No, StartDate, AutoApplyFlag, ApplyDate, BatchDescription, POSBatchID)
		SELECT PriceBatchHeaderID, H.Store_No, StartDate, AutoApplyFlag, ApplyDate, BatchDescription, POSBatchID
		FROM 
			(SELECT PBD.PriceBatchHeaderID, PBD.Store_No, SentDate, PBH.StartDate, COUNT(*) As TotalItems,
					PBH.AutoApplyFlag, PBH.ApplyDate, PBH.BatchDescription, PBH.POSBatchId
			 FROM PriceBatchDetail PBD
			 INNER JOIN
				 PriceBatchHeader PBH
				 ON PBD.PriceBatchHeaderID = PBH.PriceBatchHeaderID
			 INNER JOIN
				Store (nolock)
				ON Store.Store_No = PBD.Store_No
			 WHERE (Mega_Store = 1 OR WFM_Store = 1) 
				AND PriceBatchStatusID = 5
				AND PBH.StartDate <= @CurrDay
				AND ((ISNULL(PBH.ItemChgTypeID, 0) = 3 AND @Deletes = 1) OR (ISNULL(PBH.ItemChgTypeID, 0) <> 3 AND @Deletes = 0))
				AND PBD.Offer_ID IS NULL -- EXCLUDE OFFER PBD RECORDS
			 GROUP BY PBD.PriceBatchHeaderID, PBD.Store_No, PBH.POSBatchId, SentDate, PBH.StartDate, PBH.AutoApplyFlag, PBH.ApplyDate, PBH.BatchDescription) H
		ORDER BY StartDate, SentDate 
    RETURN
END
GO