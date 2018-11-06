SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID (N'[dbo].[MovementByDay]') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
	DROP PROCEDURE [dbo].[MovementByDay]
GO

CREATE PROCEDURE [dbo].[MovementByDay]
     @store_name varchar(50),
     @start_date smalldatetime,
     @end_date smalldatetime,
     @upc varchar(13)	
AS
BEGIN
	SELECT
		 RTRIM(CAST(DATEPART(mm, ss.Date_Key) AS CHAR(2))) + '/' + RTRIM(CAST(DATEPART(dd, ss.Date_Key) AS CHAR(2))) + '/' + CAST(DATEPART(yy, ss.Date_Key) AS CHAR(4)) AS sales_date
		 , st.Store_Name AS store
		 , b.Brand_Name AS brand
		 , SUBSTRING('0000000000000', 1, 13 - LEN(RTRIM(CAST(id.Identifier AS bigint)))) + RTRIM(id.Identifier) AS upc
		 , i.Item_Description AS description
		 , i.Package_Desc2 AS size
		 , u.Unit_Name AS uom
		 , SUM(ss.Sales_Quantity - ss.Return_Quantity) AS units
		 , SUM(ss.Sales_Amount - ss.Return_Amount) AS dollars
		 , dbo.fn_AvgCostHistory(id.Item_Key, st.Store_No, i.SubTeam_No, ss.Date_Key) AS cost
		 , SUM(ss.Sales_Amount - ss.Return_Amount) - dbo.fn_AvgCostHistory(id.Item_Key, st.Store_No
		 , i.SubTeam_No, ss.Date_Key) AS gm
	FROM
		 ItemIdentifier AS id INNER JOIN
		 Sales_SumByItem AS ss ON id.Item_Key = ss.Item_Key INNER JOIN
		 Item AS i ON ss.Item_Key = i.Item_Key AND id.Item_Key = i.Item_Key INNER JOIN
		 ItemBrand AS b ON i.Brand_ID = b.Brand_ID INNER JOIN
		 Store AS st ON ss.Store_No = st.Store_No INNER JOIN
		 ItemUnit AS u ON i.Retail_Unit_ID = u.Unit_ID
	WHERE
		 id.default_identifier = 1
		 and st.store_name = @store_name
		 and ss.date_key >= @start_date
		 and ss.date_key <= @end_date
		 and (SUBSTRING('0000000000000', 1, 13 - LEN(RTRIM(CAST(id.Identifier AS bigint)))) + RTRIM(id.Identifier)) = @upc
	GROUP BY
		 st.Store_Name
		 , ss.Date_Key
		 , b.Brand_Name
		 , ss.Item_Key
		 , id.Identifier
		 , i.Item_Description
		 , i.Package_Desc2
		 , u.Unit_Name
		 , dbo.fn_AvgCostHistory(id.Item_Key, st.Store_No, i.SubTeam_No, ss.Date_Key)
END
GO
SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO

