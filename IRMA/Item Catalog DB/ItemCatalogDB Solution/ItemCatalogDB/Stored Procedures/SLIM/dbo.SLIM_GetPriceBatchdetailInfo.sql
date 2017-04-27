set ANSI_NULLS ON
set QUOTED_IDENTIFIER ON
go


/****** Object:  StoredProcedure [dbo].[SLIM_GetPriceBatchDetailInfo]    Script Date: 11/30/2009 10:50:11 ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[SLIM_GetPriceBatchDetailInfo]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[SLIM_GetPriceBatchDetailInfo]
GO

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



