if exists (select * from dbo.sysobjects where id = object_id(N'dbo.GetESRSPriceCheck') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure dbo.GetESRSPriceCheck
GO

set ANSI_NULLS ON
set QUOTED_IDENTIFIER ON
GO

CREATE PROCEDURE [dbo].[GetESRSPriceCheck]
    @IdentifierList				varchar(8000),
    @IdentifierListSeparator	char(1)=',',
	@Store_No					int=0	
	/* Either a single store or 0 for all */
AS

BEGIN
    SET NOCOUNT ON
	DECLARE @SQL VARCHAR(2000)
	SELECT @SQL = '
	SELECT DISTINCT
		S.Store_No,
		I.Item_Key,
		II.Identifier,
		P.Price,
		P.Multiple,
		P.Sale_Price,
		P.Sale_Multiple,
		P.MSRPPrice,
		P.MSRPMultiple,
		P.PriceChgTypeID,
		PT.PriceChgTypeDesc,
		PT.On_Sale,
		PT.MSRP_Required,
		PT.LineDrive
	FROM
		Item (nolock) I
	INNER JOIN
		ItemIdentifier (nolock) II
		ON II.Item_Key	= I.Item_Key
	INNER JOIN
		fn_ParseStringList(''' + @IdentifierList + ''', ''' + @IdentifierListSeparator + ''') L 
		ON II.Identifier LIKE ''%'' + L.Key_Value + ''%'' 
	INNER JOIN
		Price (nolock) P
		ON P.Item_Key	= I.Item_Key
	INNER JOIN
		Store (nolock) S
		ON S.Store_No	= P.Store_No
	INNER JOIN
		PriceChgType (nolock) PT
		ON P.PriceChgTypeID = PT.PriceChgTypeID
	'
	/* Misisng required information so cause query to fail */
	IF @Store_No = 0
		SELECT @SQL = @SQL + 'WHERE 1=2'
	ELSE 
		SELECT @SQL + 'WHERE S.Store_No = ' + CONVERT(VARCHAR(20), @Store_No)
	
	EXEC(@SQL)
    SET NOCOUNT OFF
END

GO

