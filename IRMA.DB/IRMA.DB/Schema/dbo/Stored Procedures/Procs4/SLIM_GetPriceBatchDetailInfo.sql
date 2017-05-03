CREATE PROCEDURE [dbo].[SLIM_GetPriceBatchDetailInfo]
	@RequestID int

AS
-- **************************************************************************
	-- Procedure: SLIM_GetPriceBatchDetailInfo()
	--    Author: 
	--      Date: 
	--
	-- Modification History:
	-- Date			Init	Comment
	-- 2013-09-10   FA		Add transaction isolation level
	-- **************************************************************************
BEGIN
	SET NOCOUNT ON;

	DECLARE @ISSPriceTypeID AS int

	SET TRANSACTION ISOLATION LEVEL READ UNCOMMITTED
	
	BEGIN TRAN

    SET @ISSPriceTypeID = (SELECT PriceChgTypeID FROM PriceChgType WHERE (PriceChgTypeDesc = 'ISS'))

	SELECT     
		pbd.PriceBatchDetailID,
		sis.Item_Key,
		sis.Store_no,
		sis.Multiple,
		sis.Price,
		sis.POSPrice,
		sis.SalePrice,
		sis.SaleMultiple,
		sis.EndDate

	FROM PriceBatchDetail AS pbd (NOLOCK) INNER JOIN
		 SLIM_InStoreSpecials AS sis (NOLOCK) ON 
			pbd.SLIMRequestID = sis.RequestId AND 
			pbd.Item_Key = sis.Item_Key AND 
			pbd.Store_No = sis.Store_no
	WHERE 
		pbd.SLIMRequestID = @requestID AND
		pbd.PriceChgTypeID = @ISSPriceTypeID

	COMMIT TRAN
END
GO
GRANT EXECUTE
    ON OBJECT::[dbo].[SLIM_GetPriceBatchDetailInfo] TO [IRMASLIMRole]
    AS [dbo];

