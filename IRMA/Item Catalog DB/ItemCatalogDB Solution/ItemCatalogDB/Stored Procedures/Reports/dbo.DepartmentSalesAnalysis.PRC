SET QUOTED_IDENTIFIER ON 
GO
SET ANSI_NULLS ON 
GO

if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].DepartmentSalesAnalysis') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].DepartmentSalesAnalysis
GO

CREATE PROCEDURE dbo.DepartmentSalesAnalysis
	@StartDate		smalldatetime,
	@EndDate		smalldatetime,
	@Location		varchar(max),
	@SubTeam		int,
	@SortBy			varchar(50),
	@Results		int,
	@Identifier		varchar(13)
WITH RECOMPILE
AS
-- **************************************************************************
-- Procedure: DepartmentSalesAnalysis()
--    Author: Billy Blackerby
--      Date: 2/25/2009
--
-- Description:
-- This procedure is called from a single RDL file and generates a report consumed
-- by SSRS procedures.
--
-- Modification History:
-- Date        Init	Comment
-- 02/25/2009  BBB	Updated SP to be more readable, updated table calls to reflect
--					data elements more efficiently, added functionality from
--					TopMover_Summary report to this SP to eliminate redundancy
--					across reports, corrected issue with subteam filter being
--					ignored when joined with top items sub, also added in 
--					additional filters for useability
-- 03/03/2009  BBB	Corrected issue with SortBy, removed team paramater
-- 04/29/2009  RDS	Modified to allow for a string of delimited locations being passed
--					into the report.
-- 07/29/2009  BBB	Added in NULLIF traps to all division to alleviate divide by
--					zero errors
-- 09/13/2013  MZ   TFS #13667 - Added SET TRANSACTION ISOLATION LEVEL READ UNCOMMITTED
-- **************************************************************************	
BEGIN
	SET TRANSACTION ISOLATION LEVEL READ UNCOMMITTED
    SET NOCOUNT ON
	--**************************************************************************
	--Variables
	--**************************************************************************
	DECLARE @SQL			varchar(max)
	DECLARE @SQLIdentifier	varchar(max)
	DECLARE @SQLSubTeam		varchar(max)
	DECLARE @SQLVendor		varchar(max)

	--**************************************************************************
	--SortBy
	--**************************************************************************
	SET @SQL = 'SELECT TOP ' + CONVERT(varchar(10), @Results)

	--**************************************************************************
	--SubTeam
	--**************************************************************************
	IF @SubTeam IS NOT NULL AND @SubTeam <> 0
		SELECT @SQLSubTeam = ' AND ssi.SubTeam_No = ' + CONVERT(varchar(10), @SubTeam)
	ELSE
		SELECT @SQLSubTeam = ' AND ssi.SubTeam_No = ssi.SubTeam_No'

	--**************************************************************************
	--Identifier
	--**************************************************************************
	IF @Identifier IS NOT NULL
		SELECT @SQLIdentifier = ' AND ii.Identifier	= ''' + @Identifier + ''' '
	ELSE
		SELECT @SQLIdentifier = ' AND ii.Identifier	= ii.Identifier'

	--**************************************************************************
	--Main SQL
	--**************************************************************************
	SELECT @SQL = @SQL +
					'
						[Store],
						[SubTeam],
						[Category],		
						[UPC],
						[Desc],
						[Size],
						[UOM],
						[Cost],
						[AvgRetail] = SUM(AvgRetail * Units) / NULLIF(SUM(Units), 0),
						[Units]		= SUM(Units),
						[Sales]		= SUM(Sales)
					FROM
						(
							SELECT
								[Store]		=	s.StoreAbbr,
								[SubTeam]	=	st.SubTeam_Name,
								[UPC]		=	ii.Identifier,
								[Desc]		=	i.Item_Description,
								[Category]	=	ic.Category_Name,
								[Size]		=	i.Package_Desc2,
								[UOM]		=	iu.Unit_Abbreviation,
								[Cost]		=	ISNULL(vch.NetCost, 0),
								[AvgRetail]	=	CASE 
													WHEN dbo.fn_OnSale(p.PriceChgTypeId) = 1 THEN 
														(p.Sale_Price / p.Sale_Multiple)
													ELSE 
														p.Price
												END,
								[Units]		=	CASE
													WHEN ssi.Weight > 0 THEN
														ssi.Weight
													ELSE
														(ssi.Sales_Quantity - ssi.Return_Quantity)
												END,
								[Sales]		=	(ssi.Sales_Amount + ssi.Return_Amount)
							FROM
								Sales_SumByItem				ssi (nolock)
								INNER JOIN Item				i	(nolock)	ON	ssi.Item_Key			= i.Item_Key
								INNER JOIN ItemIdentifier	ii	(nolock)	ON	ssi.Item_Key			= ii.Item_Key
																			AND	ii.Default_Identifier	= 1
								INNER JOIN ItemCategory		ic	(nolock)	ON	i.Category_ID			= ic.Category_ID
								INNER JOIN ItemUnit			iu	(nolock)	ON	i.Package_Unit_ID		= iu.Unit_ID
								INNER JOIN dbo.fn_ParseStringList(''' + @Location + ''', ''|'') sl	ON sl.Key_Value	= ssi.Store_No
								INNER JOIN store			s	(nolock)	ON	ssi.Store_No			= s.store_no
								INNER JOIN SubTeam			st	(nolock)	ON	ssi.SubTeam_No			= st.SubTeam_No
								INNER JOIN Price			p	(nolock)	ON	ssi.Item_Key			= p.Item_Key
																			AND ssi.Store_No			= p.Store_No
								OUTER APPLY
												(
												--**************************************************************************
												-- Select the latest NetCost for the item
												--**************************************************************************
												SELECT TOP 1 
													[NetCost]	=	(ISNULL(UnitCost, 0) + ISNULL(UnitFreight, 0)) / Package_Desc1
												FROM
													VendorCostHistory			(nolock) vch2
													INNER JOIN StoreItemVendor	(nolock) siv2	ON siv2.StoreItemVendorID	= vch2.StoreItemVendorID
												WHERE 
													vch2.StoreItemVendorId	=
																			(		
																			SELECT TOP 1 
																				siv3.storeitemvendorid
																			FROM
																				StoreItemVendor (nolock) siv3
																			where
																				siv3.item_key			= i.Item_Key
																				AND siv3.store_no		= ssi.Store_No
																				AND siv3.primaryvendor	= 1
																			)
													AND StartDate			<= ''' + CONVERT(varchar(30), @StartDate, 101)	+ '''
													AND EndDate				>= ''' + CONVERT(varchar(30), @EndDate, 101)		+ '''
												ORDER BY 
													VendorCostHistoryID DESC
												) vch
							WHERE
								ssi.Date_Key		>=	''' + CONVERT(varchar(30), @StartDate, 101)	+ '''
								AND ssi.Date_Key	<=	''' + CONVERT(varchar(30), @EndDate, 101) + ''' '
								+ @SQLIdentifier
								+ @SQLSubTeam + '
							) ir
					GROUP BY
						[Store],
						[SubTeam],
						[Category],
						[UPC],
						[Desc],
						[Size],
						[UOM],
						[Cost]
					'

	IF @SortBy	= 'TopSales'
		SELECT @SQL = @SQL + ' ORDER BY SUM(Sales) DESC'
	ELSE IF @SortBy	= 'TopUnits'
		SELECT @SQL = @SQL + ' ORDER BY SUM(Units) DESC'
	ELSE IF @SortBy	= 'TopMargin'
		SELECT @SQL = @SQL + ' ORDER BY ((SUM(AvgRetail * Units) / NULLIF(SUM(Units), 0)) - Cost) / NULLIF((SUM(AvgRetail * Units) / NULLIF(SUM(Units), 0)), 0) DESC'
	ELSE IF @SortBy	= 'BottomSales'
		SELECT @SQL = @SQL + ' ORDER BY SUM(Sales) ASC'
	ELSE IF @SortBy	= 'BottomUnits'
		SELECT @SQL = @SQL + ' ORDER BY SUM(Units) ASC'
	ELSE IF @SortBy	= 'BottomMargin'
		SELECT @SQL = @SQL + ' ORDER BY ((SUM(AvgRetail * Units) / NULLIF(SUM(Units), 0)) - Cost) / NULLIF((SUM(AvgRetail * Units) / NULLIF(SUM(Units), 0)), 0) ASC'

	EXEC (@SQL)
	
    SET NOCOUNT OFF
END


GO
SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO
