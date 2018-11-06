if exists (select * from dbo.sysobjects where id = object_id(N'dbo.GetESRSItemList') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure dbo.GetESRSItemList
GO

set ANSI_NULLS ON
set QUOTED_IDENTIFIER ON
GO

CREATE PROCEDURE [dbo].[GetESRSItemList]
    @IdentifierList				varchar(8000),
    @IdentifierListSeparator	char(1)=',',
	@Store_No					int=0		
	/* Either a single store or 0 for all */

AS

BEGIN
	/* Modified by Bryce Bartley on 3/27/2008 */
    SET NOCOUNT ON
	SELECT @IdentifierList = RTRIM(@IdentifierList)
	
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
		P.Price,
		P.Multiple,
		P.Sale_Price,
		P.Sale_Multiple,
		P.MSRPPrice,
		P.MSRPMultiple,
		P.POSPrice,
		P.POSSalePrice,
		P.Sale_Start_Date,
		P.Sale_End_Date,
		PCT.PriceChgTypeDesc,
		PCT.On_Sale,
		PCT.MSRP_Required,
		PCT.LineDrive,
		SST.CostFactor,
		SST.CasePriceDiscount,
		I.Item_Description,
		C.Category_Name,
		CASE WHEN dbo.fn_IsScaleItem(II.Identifier) = 1 THEN
		E.ExtraText
		ELSE
		I.Ingredients
		END AS Ingredients
	FROM
		Item (nolock) I
	INNER JOIN
		ItemIdentifier (nolock) II
		ON II.Item_Key = I.Item_Key
	INNER JOIN
		fn_ParseStringList(''' + @IdentifierList + ''', ''' + @IdentifierListSeparator + ''') L 
		ON II.Identifier LIKE ''%'' + L.Key_Value + ''%'' 
	INNER JOIN
		ItemBrand (nolock) B
		ON B.Brand_Id	= I.Brand_Id
	INNER JOIN
		ItemCategory (nolock) C
		ON C.Category_Id = I.Category_Id
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
	IF @Store_No <> 0
		BEGIN
			SELECT @SQL = @SQL + ' WHERE S.Store_No = ' + CONVERT(VARCHAR(20), @Store_No) + ' '
		END
	EXECUTE(@SQL)
    SET NOCOUNT OFF
END

GO

