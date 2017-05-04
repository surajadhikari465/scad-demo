SET QUOTED_IDENTIFIER ON 
GO
SET ANSI_NULLS ON 
GO

IF exists (SELECT * FROM dbo.sysobjects WHERE id = object_id(N'[dbo].[ZeroMovers52Week]') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
DROP PROCEDURE [dbo].[ZeroMovers52Week]
GO

CREATE PROCEDURE [dbo].[ZeroMovers52Week]
AS

-- **************************************************************************
-- Procedure: ZeroMovers52Week()
--    Author: n/a
--      Date: n/a
--
-- Description:
-- This procedure is called from multiple RDL files and generates reports consumed
-- by SSRS procedures.
--
-- Modification History:
-- Date        Init	Comment
-- 01/12/2013  BAS	Update i.Discontinue_Item to account for schema change.
--					Renamed file to .sql.
-- **************************************************************************

BEGIN
	SET NOCOUNT ON
	DECLARE @MaxDate_Key smalldatetime
	    SET @MaxDate_Key = (SELECT MAX(Date_Key) AS MaxDate_Key FROM Sales_SumbyItemWkly WHERE Date_Key <= GETDATE())

	CREATE TABLE #ItemList 
	(
		Item_Key int PRIMARY KEY,
		SubTeam_Name varchar(100), 
		Identifier varchar(13),
		Item_Description varchar(60),
		Insert_Date datetime,
		SubTeam_No int,
		Brand_Name varchar(25),
		Package_Desc2 decimal(9,4),
		Unit_Name varchar(25),
		Text_1 varchar(50)
	)

     INSERT INTO #ItemList
		SELECT
			 i.Item_Key,
			st.SubTeam_Name, 
			ii.Identifier,
			 i.Item_Description,
			 i.Insert_Date,
			 i.SubTeam_No,
			 b.Brand_Name,
			 i.Package_Desc2,
			 u.Unit_Name,
			ia.Text_1
		FROM 
			Item							i	(nolock)
			INNER JOIN ItemIdentifier		ii	(nolock)	ON	i.item_key			= ii.item_key
			INNER JOIN Subteam				st				ON	i.Subteam_no		= st.Subteam_no
			INNER JOIN ItemBrand			b				ON	i.brand_id			= b.brand_id
			INNER JOIN itemUnit				u				ON	i.Package_Unit_Id	= u.Unit_ID
			LEFT OUTER JOIN ItemAttribute	ia				ON	i.item_key			= ia.item_key
				--the item attribute table should have a 1:1 item_key ratio
				--with the item table.  But, there are no constraint
				--on the table to prevent multiple item_key entries.
				--this was added to prevent duplicates.  It can be removed
				--when it's fixed.
																AND ia.ItemAttribute_ID = (SELECT MAX(ia2.ItemAttribute_ID)
																						   FROM ItemAttribute ia2
																						   WHERE ia.Item_Key = ia2.Item_Key)
		WHERE
			ii.Default_Identifier = 1 
			AND i.Insert_Date <= CONVERT(smalldatetime, getdate()-31) 
			AND NOT 
			(
				--exclude what SW considers the CIX D and S status
				--equivalents in IRMA
			   ((dbo.fn_GetDiscontinueStatus(i.Item_Key, NULL, NULL) = 1 OR i.Not_Available = 1) AND i.Retail_Sale <> 1) OR	
				(i.Deleted_Item = 1 or ii.Deleted_Identifier = 1)
			)

	CREATE TABLE #SoldList (Item_Key int PRIMARY KEY)
	 INSERT INTO #SoldList
		SELECT  
			item_key 
		FROM 
			sales_sumbyitemwkly (nolock)
		WHERE 
			date_key between @MaxDate_Key - 357 AND @MaxDate_Key
		GROUP BY 
			item_key


	/* This is the list of items for the selected team that have not had sales 
	in the period specified */
	SELECT 
		Item_Key,
		SubTeam_Name, 
		Identifier,
		Item_Description,
		Insert_Date,
		SubTeam_No,
		Brand_Name,
		Package_Desc2,
		Unit_Name,
		Text_1
	FROM 
		#ItemList
	WHERE 
		Item_key NOT IN (SELECT item_key FROM #SoldList)

	DROP TABLE #ItemList
	DROP TABLE #SoldList

    SET NOCOUNT OFF
END

GO
SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO

