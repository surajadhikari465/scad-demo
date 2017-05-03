if exists (select * from dbo.sysobjects where id = object_id(N'dbo.GrossMarginExceptionReportVsMvmt') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure dbo.GrossMarginExceptionReportVsMvmt
GO
set ANSI_NULLS ON
set QUOTED_IDENTIFIER ON
go

CREATE PROCEDURE [dbo].[GrossMarginExceptionReportVsMvmt]
    @MinTeam_No		int,
	@MaxTeam_No		int,
	@MinSubTeam_No	int,
	@MaxSubTeam_No	int,
    @Vendor			int,
	@MinGM			decimal(9,3),
    @MaxGM			decimal(9,3),
	@StoreNo		int,
	@MinDate		datetime,
	@MaxDate		datetime
AS
   -- **************************************************************************
   -- Procedure: GrossMarginExceptionReportVsMvmt()
   --    Author: n/a
   --      Date: n/a
   --
   -- Description:
   -- This procedure is called from multiple RDL files and generates reports consumed
   -- by SSRS procedures.
   --
   -- Modification History:
   -- Date        Init	Comment
   -- 09/11/2008  BBB	Updated SP to be more readable and removed Min/Max StoreNo call
   --					and went with more specific StoreNo call, including logic for null
   --					value being passed in as a parameter. Additionally, updated
   --					GetMargin call to utilize the IsCaseItemItemUnit function to 
   --					fix broken margins being report based upon UOM not being considered.
   -- 09/15/2008  BBB	Removed BETWEEN calls and went back to <= and >= based upon how
   --					report is utilized by user base.
   -- 01/10/2013  BAS	Update i.Discontinue_Item filter in WHERE clause to
   --					account for schema change.  Using a table variable was
   --					faster than using the scalar function we used in other cases.
  -- **************************************************************************
BEGIN
	--**************************************************************************
	-- Create the table Store variable if no StoreNo param was provided
	--**************************************************************************
	DECLARE @tblStoreNo TABLE 
		(StoreNo int)

	IF @StoreNo IS NULL
		BEGIN
			INSERT INTO @tblStoreNO

			SELECT DISTINCT Store_NO FROM Store
		END
	ELSE
		BEGIN
			INSERT INTO @tblStoreNO (StoreNo) VALUES (@StoreNo)
		END

	--**************************************************************************
	-- Create the table Vendor variable if no VendorNo param was provided
	--**************************************************************************
	DECLARE @tblVendorNo TABLE 
		(VendorNo int)

	IF @Vendor IS NULL
		BEGIN
			INSERT INTO @tblVendorNo

			SELECT DISTINCT Vendor_ID FROM Vendor
		END
	ELSE
		BEGIN
			INSERT INTO @tblVendorNo (VendorNo) VALUES (@Vendor)
		END

	--**************************************************************************
	-- Create table that hosts the Item Discontinue Information
	--**************************************************************************
	DECLARE @tblItemDisco TABLE (Item_Key int, Discontinue bit)
	INSERT INTO @tblItemDisco
		SELECT
			siv.Item_Key,
			MIN(CAST(siv.DiscontinueItem as int)) as Discontinue
		FROM
			StoreItemVendor siv
		GROUP BY
			siv.Item_Key

	--**************************************************************************
	-- Select all values contained within inner_result and outer_result
	--**************************************************************************
	SELECT 
		*
	FROM 
		(
			--**************************************************************************
			-- Select the calculated values into based upon selections within inner_result
			--**************************************************************************
			SELECT	
				*, 
				CASE 
					WHEN dbo.fn_IsCaseItemUnit(CostUnit_ID) = 0 THEN
						dbo.fn_GetMargin(Price, Multiple, Cost)
					ELSE
						dbo.fn_GetMargin(Price, Multiple, (Cost / NULLIF(Package_Desc1, 0)))
				END											AS Margin, 
				SalesQty * Cost								AS ExtCost, 
				(Price / NULLIF(Multiple, 0)) * SalesQty	AS ExtRetail     
			FROM 
				(
					--**************************************************************************
					-- Select the static joined values based upon user-def params
					--**************************************************************************
					SELECT
						t.Team_No,
						t.Team_Name,
						st.SubTeam_No,
						st.SubTeam_Name,
						ii.Identifier,
						i.Item_Key,
						i.Item_Description,
						p.Store_No, 
						s.Store_Name,
						p.Multiple, 
						p.Price, 
						p.PriceChgTypeId,
						dbo.fn_GetCurrentNetCost(i.Item_Key, s.Store_No) AS 'Cost', 
						vch.CostUnit_ID,
						vch.Package_Desc1,
						ib.Brand_Name,
						siv.Vendor_ID,
						v.CompanyName,
						IsNull(dbo.fn_GetSumSalesQuantity(i.Item_Key, s.Store_No, @MinDate, @MaxDate),0) AS SalesQty					
					FROM
						Item						(nolock) i
						INNER JOIN	SubTeam			(nolock) st		ON st.SubTeam_No			= i.SubTeam_No
						INNER JOIN	Team			(nolock) t		ON st.Team_No				= t.Team_No
						LEFT JOIN	ItemBrand		(nolock) ib		ON i.Brand_ID				= ib.Brand_ID
						INNER JOIN	ItemIdentifier	(nolock) ii		ON i.Item_Key				= ii.Item_Key 
																	AND ii.Default_Identifier	= 1
						INNER JOIN	Price			(nolock) p		ON i.Item_Key				= p.Item_Key
						INNER JOIN	Store			(nolock) s		ON s.Store_No				= p.Store_No 
						INNER JOIN	StoreItemVendor	(nolock) siv	ON p.Item_Key				= siv.Item_Key 
																	AND p.Store_No				= siv.Store_No
						INNER JOIN	Vendor			(nolock) v		ON v.Vendor_ID				= siv.Vendor_ID
						INNER JOIN	@tblItemDisco			 tid	ON i.Item_Key				= tid.Item_Key
						CROSS APPLY
							dbo.fn_VendorCost(i.Item_Key, v.Vendor_ID, p.Store_No, CONVERT(smalldatetime, GetDate(), 101)) vch						
					WHERE
						Retail_Sale				= 1 
						AND i.Deleted_Item		= 0 
						AND	tid.Discontinue		= 0 
						AND p.Price				> 0
						AND (WFM_Store			= 1 OR Mega_Store = 1)
						AND t.Team_No			>= IsNull(@MinTeam_No,		(SELECT min(Team_No)	FROM Team)) 
						AND t.Team_No			<= IsNull(@MaxTeam_No,		(SELECT max(Team_No)	FROM Team)) 
						AND i.SubTeam_No		>= IsNull(@MinSubTeam_No,	(SELECT min(SubTeam_No) FROM SubTeam))
						AND i.SubTeam_No		<= IsNull(@MaxSubTeam_No,	(SELECT max(SubTeam_No) FROM SubTeam))
						AND p.Store_No			IN		(SELECT StoreNo		FROM @tblStoreNo)
						AND v.Vendor_ID			IN		(SELECT VendorNo	FROM @tblVendorNo)
					) AS inner_result
			WHERE 
				Cost IS NOT NULL
		) AS outer_result
	WHERE
		Margin		>= @MinGM 
		AND Margin	<= @MaxGM
END

GO