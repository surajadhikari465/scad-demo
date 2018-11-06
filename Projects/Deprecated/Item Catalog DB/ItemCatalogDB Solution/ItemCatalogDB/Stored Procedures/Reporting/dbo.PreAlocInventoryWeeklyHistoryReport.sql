
/****** Object:  StoredProcedure [dbo].[PreAlocInventoryWeeklyHistoryReport]    Script Date: 10/04/2012 15:53:20 ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[PreAlocInventoryWeeklyHistoryReport]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[PreAlocInventoryWeeklyHistoryReport]
GO

/****** Object:  StoredProcedure [dbo].[PreAlocInventoryWeeklyHistoryReport]    Script Date: 10/04/2012 15:53:20 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO






/*
	grant exec on WareHouseWeeklyOrderedQuantities to IRMAAdminRole
	grant exec on WareHouseWeeklyOrderedQuantities to IRMAClientRole
	grant exec on WareHouseWeeklyOrderedQuantities to IRMAReportsRole
*/

CREATE PROCEDURE [dbo].[PreAlocInventoryWeeklyHistoryReport]
    @Store_No	int,
	@SubTeam_No int

WITH RECOMPILE
AS
   -- **************************************************************************
   -- Procedure: PreAlocInventoryWeeklyHistoryReport()
   --    Author: n/a
   --      Date: n/a
   --
   -- Description:
   -- This procedure is called from a single RDL file and generates a report consumed
   -- by SSRS procedures. Report that shows the on-hand quantities and quantities that 
   -- the warehouse ordered for each item for each day of the week for this week. This
   -- report is used in two situations. Prior to allocation it is used to retrospectively
   -- compare quantities on hand to quantities ordered each day this week. Post-Allocation,
   -- it is used to determine if remaining quantities are sufficient to fill the next
   -- day?s orders. When run post-allocation, the Inventory Weekly History Report will
   -- have data only for the day on which the report is run.
   --
   -- Modification History:
   -- Date		Init/Bug	Comment
   -- 09/24/08  BBB			Updated SP to utilize OnHand table and compile all
   --						all aggregate count functions into SP instead of passing
   --						data to WS which then bashed the data. Additionally,
   --						changed parameters to match input from user input
   -- 10/07/08  BBB			Data coming from client is the Vendor_ID had to lookup Store_No
   -- 05/11/09  BBB/9639	Converted OnHandQty query to pull from ICH instead of OH;
   --						removed roll-forward addition of OnOrder values;
   -- 06/15/09  FA          Updated report according to MA specification
   -- 06/23/09  FA			Added code to include items which is not in inventory but on order.
   -- 06/30/09  BSR			Added Subteam criteria to the UNION Where Clause
   --						Changed the Order By to use ClassName First 
   --						EXEC PreAlocInventoryWeeklyHistoryReport 40808, 4
   -- 07/10/09  BSR			Changed OH to pull >= 0 from >0
   -- 09/02/09  MD			Bug 10793 - Updated the sp to ensure that items that were not received 
   --						but are expected to be received within a week show up
   -- 10/03/2012	AlexB		Removed all references to ItemCaseHistory
  -- **************************************************************************
BEGIN

SET NOCOUNT ON
	--**************************************************************************
	--Get Store_No from Vendor Table
	--**************************************************************************	
	SET @Store_No = (SELECT Store_No FROM Vendor WHERE Vendor_ID = @Store_No)
	
	--**************************************************************************
	--Set Week timeline to be used throughout queries
	--**************************************************************************	
	Set DATEFIRST 1
	
	DECLARE @Today  varchar(20)
	Set @Today = datename(dw, getdate())

	DECLARE @DayNumberOfWeek  int
	Set @DayNumberOfWeek = datepart(dw, getdate())

	DECLARE @StartDate varchar(12)
	SET @StartDate = CONVERT(varchar(12), dateadd(dd, 1-datepart(dw,GETDATE()),GETDATE()), 101)

	DECLARE @EndDate varchar(12)
	SET @EndDate = dateadd("d",6,@StartDate)

	--**************************************************************************
	--Select current On Hand Quantities from OnHand Table
	--**************************************************************************	
	DECLARE @OnHandQty TABLE 
			(
			Store_No			int,
			Store_Name			varchar(50),
			SubTeam_Name		varchar(50),
			ClassName			varchar(50),
			Item_Key			int,
			Level3				varchar(50),
			Level4				varchar(50), 
			Identifier			varchar(15),
			Unit_Name			varchar(50),
			Item_Description	varchar(100),
			Qty					int
			)
	
	INSERT INTO @OnHandQty
		SELECT 
			* 
		FROM 
			(
			SELECT
				[Store_No]			= oh.Store_No,	
				[Store_Name]		= s.Store_Name,
				[SubTeam_Name]		= st.SubTeam_Name,
				[ClassName]			= ic.Category_Name,	
				[Item_Key]			= ih.Item_Key,	
				[Level3]			= lv3.Description,
				[Level4]			= lv4.Description,
				[Identifier]		= REPLICATE('0',12-LEN(RTRIM(ii.Identifier))) + RTRIM(ii.Identifier),
				[Unit_Name]			= dbo.fn_GetCurrentVendorPackage_Desc1(ih.item_key,@store_No),
				[Item_Description]	= i.Item_Description,
				[Qty]				= SUM(										 
												CASE 
													WHEN ISNULL(ih.Quantity, 0) > 0 THEN 
														ih.Quantity / 
																				CASE 
																					WHEN i.Package_Desc1 <> 0 THEN 
																						i.Package_Desc1 
																					ELSE 
																						1 
																				END
													ELSE 
														ISNULL(ih.Weight, 0) / 
																				CASE 
																					WHEN i.Package_Desc1 * i.Package_Desc2 <> 0 THEN 
																						(i.Package_Desc1 * i.Package_Desc2) 
																					ELSE 
																						1 
																				END 
												END
												 * ia.Adjustment_Type)
			FROM
				OnHand								(nolock) oh
				INNER JOIN ItemHistory				(nolock) ih		ON	ih.Item_Key					= oh.Item_Key 
																	AND ih.Store_No					= oh.Store_No 
																	AND ih.SubTeam_No				= oh.SubTeam_No
				INNER JOIN ItemAdjustment			(nolock) ia		ON	ih.Adjustment_ID			= ia.Adjustment_ID				
				INNER JOIN		Item				(nolock) i		ON	i.Item_Key					= oh.Item_Key
				INNER JOIN		ItemIdentifier		(nolock) ii		ON	ii.Item_Key					= i.Item_Key 
																	AND ii.Default_Identifier		= 1
				INNER JOIN		ItemCategory		(nolock) ic		ON	i.Category_ID				= ic.Category_ID
				INNER JOIN		SubTeam				(nolock) st		ON	st.SubTeam_No				= oh.SubTeam_No
				INNER JOIN		Store				(nolock) s		ON	s.Store_No					= oh.Store_No
				LEFT OUTER JOIN	ProdHierarchyLevel4	(nolock) lv4	ON	lv4.ProdHierarchyLevel4_ID	= i.ProdHierarchyLevel4_ID
				LEFT OUTER JOIN ProdHierarchyLevel3	(nolock) lv3	ON	lv3.ProdHierarchyLevel3_ID	= lv4.ProdHierarchyLevel3_ID
				LEFT OUTER JOIN	NatItemClass		(nolock) nic	ON	nic.ClassID					= i.ClassID
			WHERE
				oh.Store_No			=	ISNULL(@Store_No, oh.Store_No)
				AND st.SubTeam_No	=	ISNULL(@SubTeam_No, st.SubTeam_No)
				AND ih.DateStamp	>=	ISNULL(oh.LastReset, ih.DateStamp)
			GROUP BY
				oh.Store_No,	
				s.Store_Name,
				st.SubTeam_Name,
				ic.Category_Name,
				ih.Item_Key,	
				lv3.Description,
				lv4.Description,
				ii.Identifier,
				i.Item_Description,
				i.Package_Desc1,
				i.Package_Desc2
			) AS OnHandQty
		WHERE Qty >= 0
		UNION
		SELECT
				[Store_No]			= S.store_No,	
				[Store_Name]		= s.Store_Name,
				[SubTeam_Name]		= st.SubTeam_Name,
				[ClassName]			= ic.Category_Name,	
				[Item_Key]			= oi.Item_Key,	
				[Level3]			= lv3.Description,
				[Level4]			= lv4.Description,
				[Identifier]		= REPLICATE('0',12-LEN(RTRIM(ii.Identifier))) + RTRIM(ii.Identifier),
				[Unit_Name]			= dbo.fn_GetCurrentVendorPackage_Desc1(oi.item_key,@store_No),
				[Item_Description]	= i.Item_Description,
				[Qty]				= 0 --SUM(oi.QuantityOrdered)		
		FROM
			OrderItem							(nolock) oi
			INNER JOIN		OrderHeader			(nolock) oh		ON  oh.Orderheader_ID			= oi.Orderheader_ID
			INNER JOIN		Vendor				(nolock) v		on  v.vendor_id					= oh.purchaselocation_id
			INNER JOIN		Item				(nolock) i		ON	i.Item_Key					= oi.Item_Key
			INNER JOIN		ItemIdentifier		(nolock) ii		ON	ii.Item_Key					= i.Item_Key AND ii.Default_Identifier = 1
			INNER JOIN		ItemCategory		(nolock) ic		ON	i.Category_ID				= ic.Category_ID
			INNER JOIN		SubTeam				(nolock) st		ON	st.SubTeam_No				= oh.Transfer_To_SubTeam
			INNER JOIN		Store				(nolock) s		ON	s.Store_No					=  @Store_No
			LEFT OUTER JOIN	ProdHierarchyLevel4	(nolock) lv4	ON	lv4.ProdHierarchyLevel4_ID	= i.ProdHierarchyLevel4_ID
			LEFT OUTER JOIN ProdHierarchyLevel3	(nolock) lv3	ON	lv3.ProdHierarchyLevel3_ID	= lv4.ProdHierarchyLevel3_ID
			LEFT OUTER JOIN	NatItemClass		(nolock) nic	ON	nic.ClassID					= i.ClassID

		WHERE
			oh.Expected_Date		>=	@StartDate
			AND oh.Expected_Date	<=	@EndDate
			-- Bug 10793 removed below statement from where clause to incorporate items that are not onhand but expected in week
			-- and oi.item_key not in (select distinct Item_key from onhand)
		GROUP BY
				s.Store_No,	
				s.Store_Name,
				st.SubTeam_Name,
				ic.Category_Name,
				oi.Item_Key,	
				lv3.Description,
				lv4.Description,
				ii.Identifier,
				i.Item_Description,
				i.Package_Desc1,
				i.Package_Desc2
		HAVING SUM(oi.QuantityOrdered)	> 0

		--DN FIX for 10793 (do not delete) removes duplicates
		DECLARE @TEMPIdentifier AS TABLE(
		Identifier varchar(15),
		CNT INT)

		INSERT INTO @TEMPIdentifier
		SELECT Identifier, count(*)
		FROM @OnHandQty
		GROUP BY Identifier
		HAVING count(*) > 1

		DELETE FROM  @OnHandQty
		where Identifier IN (SELECT Identifier FROM @TEMPIdentifier) AND Qty = 0
		--DN end fix

	--**************************************************************************
	-- Get Daily item qty modifiers based upon Orders from OrderHeader
	--**************************************************************************
	DECLARE @ModOrder TABLE 
			(
			ModItem		int,
			ModDate		datetime,
			ModQty		int,
			ModDay		varchar(50)
			)

	INSERT INTO @ModOrder
		SELECT
			oi.Item_key						AS ModItem,
			oh.Expected_Date				AS ModDate,
			SUM(oi.QuantityOrdered)     	AS ModQty,
			datename(dw, oh.Expected_Date)	AS ModDay
		FROM
			OrderItem				(nolock) oi
			INNER JOIN OrderHeader	(nolock) oh	ON oh.Orderheader_ID	= oi.Orderheader_ID
			INNER JOIN Vendor (nolock) v on v.vendor_id = oh.purchaselocation_id
		WHERE
			v.Store_No = @Store_No AND 
			oh.Expected_Date		>=	@StartDate
			AND oh.Expected_Date	<=	@EndDate
		GROUP BY
			oi.Item_key, oh.Expected_Date

	--**************************************************************************
	-- Merge Send and Orders to come up with total daily modifiers
	--**************************************************************************
	DECLARE @ModQty TABLE 
			(
			ModItem		int,
			ModDay		varchar(50),
			ModQty		int
			)

	INSERT INTO @ModQty
		SELECT
			ModItem,
			ModDay,
			0 as ModQty
		FROM
			(
			SELECT * FROM @ModOrder
			) AS Mods
		GROUP BY 	
			ModItem,
			ModDay

	DECLARE @ModOrderQty TABLE 
			(
			ModItem		int,
			ModDay		varchar(50),
			ModQty		int
			)

	DECLARE @tmpRecords AS TABLE
			(
				StartDate  DateTime,
				EndDate  DateTime,	
				Store_No  Int,	
				Store_Name  varchar(50),
				SubTeam_Name  varchar (50),
				ClassName  varchar (50),	
				Item_Key  Int,	
				Level3   Varchar (50),
				Level4   Varchar (50),
				Identifier  Varchar (15),
				Unit_Name  Varchar (50),
				Item_Description  Varchar (100),
				Qty1  Int,
				MonOH  Int,
				TueOH  Int,
				WedOH  Int,
				ThuOH  Int,
				FriOH  Int,
				SatOH  Int,
				SunOH  Int,
				MonOD  Int,
				TueOD  Int,
				WedOD  Int,
				ThuOD  Int,
				FriOD  Int,
				SatOD  Int,
				SunOD  Int
			)
	--**************************************************************************
	-- Merging/aggregating the data for displaying in the report Output
	--**************************************************************************
	INSERT INTO @tmpRecords
	SELECT
		@StartDate as StartDate,
		convert(varchar(12), dateadd("d",6,@StartDate), 101) as EndDate,	
		tot.Store_No,	
		tot.Store_Name,
		tot.SubTeam_Name,
		tot.ClassName,	
		tot.Item_Key,	
		tot.Level3,
		tot.Level4,
		convert(bigint, tot.Identifier) as Identifier,
		tot.Unit_Name,
		tot.Item_Description,
		IsNull((tot.Qty), 0),
		IsNull((tot.Qty + tot.ModMon), 0)																				AS MonOH,
		
		case when @DayNumberOfWeek >= 2
			Then IsNull((tot.Qty + tot.ModMon + tot.ModTue), 0)
			else null end																									AS TueOH,

		case when @DayNumberOfWeek >= 3
			Then IsNull((tot.Qty + tot.ModMon + tot.ModTue + tot.ModWed), 0)		
			else null end																									AS WedOH,

		case when @DayNumberOfWeek >= 4
			Then IsNull((tot.Qty + tot.ModMon + tot.ModTue + tot.ModWed + tot.ModThu), 0)		
			else null end																									AS ThuOH,

		case when @DayNumberOfWeek >= 5
			Then IsNull((tot.Qty + tot.ModMon + tot.ModTue + tot.ModWed + tot.ModThu + tot.ModFri), 0)				
			else null end																									AS FriOH,

		case when @DayNumberOfWeek >= 6
			Then IsNull((tot.Qty + tot.ModMon + tot.ModTue + tot.ModWed + tot.ModThu + tot.ModFri + tot.ModSat), 0)				
			else null end																									AS SatOH,	

		case when @DayNumberOfWeek = 7
			Then IsNull((tot.Qty + tot.ModMon + tot.ModTue + tot.ModWed + tot.ModThu + tot.ModFri + tot.ModSat + tot.ModSun), 0)				
			else null end																									AS SunOH,

		IsNull((ord.Mon), 0)																							AS MonOD,
		case when @DayNumberOfWeek >= 2
			Then IsNull((ord.Tue), 0)	
			else case when ord.Tue = 0 then null else ord.Tue end
		end																												AS TueOD,
		case when @DayNumberOfWeek >= 3
			Then IsNull((ord.Wed), 0)	
			else case when ord.Wed = 0 then null else ord.Wed end
		end																												AS WedOD,
		case when @DayNumberOfWeek >= 4
			Then IsNull((ord.Thu), 0)	
			else case when ord.Thu = 0 then null else ord.Thu end
		end																												AS ThuOD,
		case when @DayNumberOfWeek >= 5
			Then IsNull((ord.Fri), 0)	
			else case when ord.Fri = 0 then null else ord.Fri end
		end																												AS FriOD,
		case when @DayNumberOfWeek >= 6
			Then IsNull((ord.Sat), 0)	
			else case when ord.Sat = 0 then null else ord.Sat end
		end																												AS SatOD,
		case when @DayNumberOfWeek = 7
			Then IsNull((ord.Sun), 0)	
			else case when ord.Sun = 0 then null else ord.Sun end
		end																												AS SunOD
	FROM
		(
		SELECT
			oh.Store_No,	
			oh.Store_Name,
			oh.SubTeam_Name,
			oh.ClassName,	
			oh.Item_Key,	
			oh.Level3,
			oh.Level4,
			oh.Identifier,
			oh.Unit_Name,
			oh.Item_Description,
			oh.Qty,
			SUM(CASE WHEN ModDay = 'Monday'		THEN ModQty ELSE 0 END) AS ModMon,        
			SUM(CASE WHEN ModDay = 'Tuesday'	THEN ModQty ELSE 0 END) AS ModTue,
			SUM(CASE WHEN ModDay = 'Wednesday'	THEN ModQty ELSE 0 END) AS ModWed,
			SUM(CASE WHEN ModDay = 'Thursday'	THEN ModQty ELSE 0 END) AS ModThu,
			SUM(CASE WHEN ModDay = 'Friday'		THEN ModQty ELSE 0 END) AS ModFri,
			SUM(CASE WHEN ModDay = 'Saturday'	THEN ModQty ELSE 0 END) AS ModSat,
			SUM(CASE WHEN ModDay = 'Sunday'		THEN ModQty ELSE 0 END) AS ModSun
		FROM 
			@OnHandQty oh

			LEFT OUTER JOIN
			(
			SELECT
				oq.ModItem,
				oq.ModDay,
				oq.ModQty
			FROM
				@ModQty oq
			GROUP BY
				oq.ModItem,
				oq.ModDay,
				oq.ModQty
			) mod ON oh.Item_Key = mod.ModItem			
		GROUP BY
			oh.Store_No,	
			oh.Store_Name,
			oh.SubTeam_Name,
			oh.ClassName,	
			oh.Item_Key,	
			oh.Level3,
			oh.Level4,
			oh.Identifier,
			oh.Unit_Name,
			oh.Item_Description,
			oh.Qty
		) AS tot
		LEFT OUTER JOIN 
		(
		SELECT
			ModItem,
			SUM(CASE WHEN ModDay = 'Monday'		THEN ModQty ELSE 0 END) AS Mon,        
			SUM(CASE WHEN ModDay = 'Tuesday'	THEN ModQty ELSE 0 END) AS Tue,
			SUM(CASE WHEN ModDay = 'Wednesday'	THEN ModQty ELSE 0 END) AS Wed,
			SUM(CASE WHEN ModDay = 'Thursday'	THEN ModQty ELSE 0 END) AS Thu,
			SUM(CASE WHEN ModDay = 'Friday'		THEN ModQty ELSE 0 END) AS Fri,
			SUM(CASE WHEN ModDay = 'Saturday'	THEN ModQty ELSE 0 END) AS Sat,
			SUM(CASE WHEN ModDay = 'Sunday'		THEN ModQty ELSE 0 END) AS Sun
		FROM
			@ModOrder
		GROUP BY
			ModItem
		) AS ord ON tot.Item_Key = ord.ModItem
	ORDER BY 
		tot.identifier,
		tot.Item_Description

	SELECT * FROM @tmpRecords 
		WHERE 
		(MonOD > 0 OR TueOD > 0 OR WedOD > 0 OR ThuOD > 0 OR FriOD > 0 OR SatOD > 0 OR SunOD > 0)
		OR
		(MonOH > 0 OR TueOH > 0 OR WedOH > 0 OR ThuOH > 0 OR FriOH > 0 OR SatOH > 0 OR SunOH > 0)
		OR
		QTY1 > 0
	
   SET NOCOUNT OFF
END


GO


