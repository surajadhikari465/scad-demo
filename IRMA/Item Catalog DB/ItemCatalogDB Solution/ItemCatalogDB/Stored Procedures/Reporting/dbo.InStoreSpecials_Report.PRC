SET QUOTED_IDENTIFIER ON 
GO
SET ANSI_NULLS ON 
GO

if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[InStoreSpecials_Report]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[InStoreSpecials_Report]
GO


CREATE PROCEDURE dbo.InStoreSpecials_Report
	@Store_No varchar(8000),
	@SubTeam_No varchar(8000),
	@Sale_Start_Date varchar(8000),
	@Sale_End_Date varchar(8000)
AS

	DECLARE @SQL varchar(8000)
	
	IF @Sale_Start_Date IS NULL OR @Sale_End_Date IS NULL
		BEGIN
			SELECT @Sale_Start_Date = CAST(GETDATE() AS Varchar(500))
			SELECT @Sale_End_Date = CAST(GETDATE() AS Varchar(500))
		END
		
	SELECT @SQL = '		
		SELECT
			II.Identifier,
			ST.SubTeam_Name,
			P.Store_No,
			S.Store_Name,
			P.Sale_Price,
			PCT.PriceChgTypeDesc,
			P.Price,
			P.Sale_Start_Date,
			P.Sale_End_Date,
			IB.Brand_Name,
			I.Item_Description,
			I.Package_Desc2 As Retail_Size,
			IU.Unit_Abbreviation
		FROM
			Item I (nolock)
			JOIN
				ItemIdentifier II (nolock)
				ON II.Item_Key = I.Item_Key
			JOIN
				SubTeam ST (nolock)
				ON ST.SubTeam_No = I.SubTeam_No
			JOIN
				Price P (nolock)
				ON P.Item_Key = I.Item_Key
			JOIN
				Store S (nolock)
				ON S.Store_No = P.Store_No
			JOIN 
				ItemBrand IB (nolock)
				ON IB.Brand_Id = I.Brand_Id
			JOIN
				ItemUnit IU (nolock)
				ON IU.Unit_Id = I.Package_Unit_Id
			JOIN 
				PriceChgType PCT (nolock)
				ON PCT.PriceChgTypeId = P.PriceChgTypeId
		WHERE
			I.SubTeam_No IN (' + REPLACE(@SubTeam_No,'|',',') + ') AND
			P.Store_No = ''' + @Store_No + ''' AND
			P.Sale_Start_Date >= ''' + @Sale_Start_Date + ''' AND
			P.Sale_End_Date >= ''' + @Sale_End_Date + ''' AND
			P.PriceChgTypeId = 10
				
		ORDER BY
			I.SubTeam_No'
				
	EXEC (@SQL)			
	
GO
SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO
