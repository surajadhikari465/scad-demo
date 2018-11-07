CREATE PROCEDURE dbo.GetPriceBatchDetail
@PriceBatchHeaderID int

AS

-- ****************************************************************************************************************
-- Procedure: GetPriceBatchDetail()
--    Author: unknown
--      Date: unknown
--
-- Description:
-- Called from PriceBatchDetail.  Displays details about a particular batch.
--
-- Modification History:
-- Date       	Init  	TFS   	Comment
-- 2013-03-27	KM		11723	Check ItemOverride for the Item_Description;
-- ****************************************************************************************************************

BEGIN
    SET NOCOUNT ON

	SELECT * FROM
	( 
		SELECT	
			PBD.Item_Key, 
			PriceBatchDetailID, 
			ISNULL(PBD.Identifier, II.Identifier) AS Identifier, 
			ISNULL(PBD.Item_Description, ISNULL(iov.Item_Description, i.Item_Description)) AS Item_Description,
			CASE WHEN 1 = dbo.fn_OnSale(PBD.PriceChgTypeID) THEN POSSale_Price 
				ELSE POSPrice 
			END AS POSPrice, 
			CASE WHEN 1 = dbo.fn_OnSale(PBD.PriceChgTypeID) THEN CAST(Sale_Multiple AS VARCHAR(10)) + ' @ ' + CAST(POSSale_Price AS VARCHAR(10)) 
				ELSE  CAST(Multiple AS VARCHAR(10)) + ' @ ' + CAST(POSPrice AS VARCHAR(10))
			END AS POSPriceWithMultiple, 
			PBD.StartDate, 
			Sale_End_Date, 
			PrintSign,
			ROW_NUMBER() OVER (PARTITION BY PBD.Item_Key ORDER BY PBD.StartDate DESC) nmbr
    
	FROM 
		PriceBatchDetail				PBD (nolock)
		INNER JOIN	PriceBatchHeader	PBH (nolock)	ON	PBD.PriceBatchHeaderID	= PBH.PriceBatchHeaderID
		INNER JOIN	Item				i	(nolock)	ON	i.Item_Key				= PBD.Item_Key
		LEFT JOIN	ItemOverride		iov (nolock)	ON	i.Item_Key				= iov.Item_Key
		INNER JOIN	ItemIdentifier		II	(nolock)	ON	II.Item_Key				= PBD.Item_Key 
														AND II.Default_Identifier	= 1
    WHERE 
		PBD.PriceBatchHeaderID = @PriceBatchHeaderID
        AND NOT ((ISNULL(PBD.ItemChgTypeID, 0) = 2) AND (PBD.PriceChgTypeID IS NULL) AND (ISNULL(PBH.ItemChgTypeID, 0) <> 2))) PriceBatchDetail
	
	WHERE nmbr = 1

    SET NOCOUNT OFF
END
GO
GRANT EXECUTE
    ON OBJECT::[dbo].[GetPriceBatchDetail] TO [IRMASupportRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[GetPriceBatchDetail] TO [IRMAClientRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[GetPriceBatchDetail] TO [IRMASchedJobsRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[GetPriceBatchDetail] TO [IRMAReportsRole]
    AS [dbo];

