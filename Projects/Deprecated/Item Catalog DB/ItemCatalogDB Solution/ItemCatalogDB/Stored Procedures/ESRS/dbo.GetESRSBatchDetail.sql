if exists (select * from dbo.sysobjects where id = object_id(N'dbo.GetESRSBatchDetail') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure dbo.GetESRSBatchDetail
GO
set ANSI_NULLS ON
set QUOTED_IDENTIFIER ON
GO

CREATE PROCEDURE [dbo].[GetESRSBatchDetail]
	@PriceBatchHeaderID			INT,
	@Store_No					INT=0
	/* Either a single store or 0 for all */
AS

BEGIN
    SET NOCOUNT ON
	DECLARE @SQL varchar(2000)

	SELECT @SQL = '
	SELECT DISTINCT
		PD.PriceBatchDetailID,
		PD.PriceBatchHeaderID,
		PD.Store_No,
		PD.StartDate,
		PD.Sale_End_Date,
		PD.Price,
		PD.Multiple,
		PD.MSRPPrice,
		PD.MSRPMultiple,
		PD.Sale_Price,
		PD.Sale_Multiple,
		PD.POSPrice,
		PD.POSSale_Price,
		PD.Case_Price,
		PD.Origin_Name,
		PD.Vendor_Id,
		PD.Organic,
		PH.ItemChgTypeID,
		PD.PriceChgTypeID,
		PH.BatchDescription,
		PH.StartDate,
		PH.SentDate,
		PH.PrintedDate,
		PH.ProcessedDate,
		PH.ApplyDate,
		I.Item_Description,
		I.Sign_Description,
		I.Category_ID,
		I.SubTeam_No,
		I.Package_Desc1,
		I.Package_Desc2,
		I.Package_Unit_ID,
		I.Brand_ID,
		I.Category_ID,
		I.SubTeam_No,
		I.Origin_ID,
		I.TaxClassID,
		II.Identifier,	
		IB.Brand_Name,
		PCT.PriceChgTypeDesc,
		PCT.On_Sale,
		PCT.MSRP_Required,
		PCT.LineDrive,
		IU.Unit_Name,
		IU.Unit_Abbreviation,
		CASE WHEN dbo.fn_IsScaleItem(II.Identifier) = 1 THEN
		E.ExtraText
		ELSE
		I.Ingredients
		END AS Ingredients
	FROM
		PriceBatchDetail (nolock) PD
	INNER JOIN
		PriceBatchHeader (nolock) PH
		ON PD.PriceBatchHeaderID = PH.PriceBatchHeaderID
	INNER JOIN
		Item (nolock) I
		ON I.Item_Key = PD.Item_Key
	INNER JOIN
		ItemIdentifier (nolock) II
		ON	II.Item_Key = PD.Item_Key
	INNER JOIN
		ItemBrand (nolock) IB
		ON I.Brand_ID = IB.Brand_ID
	INNER JOIN
		PriceChgType (nolock) PCT
		ON PCT.PriceChgTypeID = PH.PriceChgTypeID
	INNER JOIN
		ItemUnit (nolock) IU
		ON IU.Unit_ID = I.Package_Unit_ID
	LEFT JOIN
		ItemScale (nolock) ISC
		ON ISC.Item_Key = I.Item_Key
	LEFT JOIN
		Scale_ExtraText (nolock) E
		ON E.Scale_ExtraText_ID = ISC.Scale_ExtraText_ID
	WHERE
		PD.PriceBatchHeaderID = ' + CONVERT(VARCHAR(20), @PriceBatchHeaderID)
	IF @Store_No <> 0
		BEGIN
			SELECT @SQL = @SQL + 'AND PD.Store_No = ' + CONVERT(VARCHAR(20), @Store_No)
		END
	EXECUTE(@SQL)
    SET NOCOUNT OFF
END

GO