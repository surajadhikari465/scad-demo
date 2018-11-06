if exists (select * from dbo.sysobjects where id = object_id(N'dbo.GetESRSItemSearch') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure dbo.GetESRSItemSearch
GO

set ANSI_NULLS ON
set QUOTED_IDENTIFIER ON
GO

CREATE PROCEDURE [dbo].[GetESRSItemSearch]
    @Item_Description	varchar(60)='',
    @Identifier			varchar(13)='',
    @Brand_Name			varchar(25)='',
    @SubTeam_No			varchar(10)='',
    @Category_ID		int=0,
    @Store_No			int=0,
    @PriceChgTypeId		int=0
	/* Either a single store or NULL for all */
AS
BEGIN
    SET NOCOUNT ON
	/* -- Modified by Bryce Bartley on 3/27/2008 */
	/* -- Modified by Bryce Bartley on 9/22/2009 - Added True Cost Lookup */
	SELECT @Item_Description = RTRIM(@Item_Description)
	SELECT @Brand_Name = RTRIM(@Brand_Name)
	SELECT @Identifier = RTRIM(@Identifier)
	DECLARE @SQL varchar(2000)
	
	SELECT @SQL = '
	SELECT DISTINCT
		S.Store_No,
		I.Item_Key,
		II.Identifier,
		I.Brand_ID,
		I.Category_ID,
		I.SubTeam_No,
		I.Origin_ID,
		I.TaxClassID,
		B.Brand_Name,
		I.Sign_Description,
		I.Package_Desc1,
		I.Package_Desc2,
		U.Unit_Name,
		U.Unit_Abbreviation,
		P.POSPrice,
		P.POSSale_Price,
		P.Price,
		P.Multiple,
		P.Sale_Price,
		P.Sale_Multiple,
		P.MSRPPrice,
		P.MSRPMultiple,
		P.Sale_Start_Date,
		P.Sale_End_Date,
		P.PriceChgTypeId,
		PCT.PriceChgTypeDesc,
		PCT.On_Sale,
		PCT.MSRP_Required,
		PCT.LineDrive,
		SST.CostFactor,
		SST.CasePriceDiscount,
		I.Item_Description,
		CASE WHEN dbo.fn_IsScaleItem(II.Identifier) = 1 THEN
		E.ExtraText
		ELSE
		I.Ingredients
		END AS Ingredients
	FROM
		Item (nolock) I
	INNER JOIN
		ItemIdentifier (nolock) II
		ON II.Item_Key	= I.Item_Key
	INNER JOIN
		ItemBrand (nolock) B
		ON B.Brand_Id	= I.Brand_Id
	INNER JOIN
		ItemUnit (nolock) U
		ON U.Unit_Id	= I.Package_Unit_Id
	INNER JOIN
		Price (nolock) P
		ON P.Item_Key	= I.Item_Key
	INNER JOIN
		Store (nolock) S
		ON S.Store_No	= P.Store_No
	INNER JOIN
		PriceChgType (nolock) PCT
		ON PCT.PriceChgTypeId = P.PriceChgTypeId
	LEFT JOIN
		ItemScale (nolock) ISC
		ON ISC.Item_Key = I.Item_Key
	LEFT JOIN
		Scale_ExtraText (nolock) E
		ON E.Scale_ExtraText_ID = ISC.Scale_ExtraText_ID
	LEFT JOIN
		StoreSubTeam (nolock) SST
		ON SST.SubTeam_No = I.SubTeam_No
	'
	IF @Identifier <> ''
		BEGIN
			SELECT @SQL = @SQL + ' WHERE II.Identifier LIKE ''%' + @Identifier + '%'' '
			IF (@Store_No <> 0) SELECT @SQL = @SQL + ' AND S.Store_No = ' + CONVERT(VARCHAR(20), @Store_No) + ' '
		END
	ELSE
		BEGIN
			SELECT @SQL = @SQL + ' WHERE 1=1'
			IF (@Store_No <> 0) SELECT @SQL = @SQL + ' AND S.Store_No = ' + CONVERT(VARCHAR(20), @Store_No) + ' '
			IF (@Brand_Name <> '') SELECT @SQL = @SQL + ' AND B.Brand_Name LIKE ''%' + @Brand_Name + '%'' '
			IF (@Item_Description <> '') SELECT @SQL = @SQL + 'AND I.Item_Description LIKE ''%' + @Item_Description + '%'' '
			IF (@SubTeam_No <> '') SELECT @SQL = @SQL + ' AND I.SubTeam_No=''' + @SubTeam_No +''' '
			IF (@Category_ID <> 0) SELECT @SQL = @SQL + ' AND I.Category_ID=''' + @Category_ID +''' '
			IF (@PriceChgTypeId <> 0) SELECT @SQL = @SQL + ' AND P.PriceChgTypeId=''' + @PriceChgTypeId +''' '
		END
	EXECUTE(@SQL)
    SET NOCOUNT OFF
END

GO

