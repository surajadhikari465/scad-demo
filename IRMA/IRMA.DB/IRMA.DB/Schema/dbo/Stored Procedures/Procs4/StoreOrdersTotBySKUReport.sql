CREATE Procedure dbo.StoreOrdersTotBySKUReport
	(
		@Warehouse int,
		@Team int,
		@SubTeam int,
		@categoryID int,
		@Level3 int,
		@Level4 int,
		@StartDate datetime,
		@EndDate datetime
	)
	/*

	EXEC dbo.StoreOrdersTotBySKUReport 5906, null, 4200

	*/
AS
BEGIN
	SET TRANSACTION ISOLATION LEVEL READ UNCOMMITTED
	-- @StoresPerReportRow is the number of stores in each row of the Report Services report
	declare @StoresPerReportRow int,
			@VendorID int
	set @StoresPerReportRow = 10

	SELECT @VendorID = Vendor_id FROM vendor WHERE Store_No = @Warehouse

	IF EXISTS (SELECT * FROM sysobjects WHERE type = 'U' AND name = '#ReportData')
		DROP TABLE #ReportData

	select DISTINCT
		k.CompanyName as Kitchen
	,	Core.Vendor_ID
	,	Core.Item_Key
	,	Core.SubTeam
	,	st.Team_No
	,	Team.Team_Name
	,	st.SubTeam_Abbreviation as SubTeamAbbr
	,	ii.Identifier as SKU
	,	item.Item_Description as ProductDesc
	--  Commented to fix the bug 6490
	--	,	replace(rtrim(replace(replace(rtrim(replace(convert(varchar,Package_Desc2),'0',' ')),' ','0'),'.',' ')),' ','.')
	--	 +' '+ ItemUnit.Unit_Abbreviation as UOM
    ,dbo.fn_GetCurrentVendorPackage_Desc1(Item.Item_key,@Warehouse) as UOM
	,	Core.Store_No					
	,	Core.Qty
	INTO #ReportData
	FROM (
		select 
			oh.Vendor_ID
		,	oi.Item_Key
		,	oh.Transfer_To_SubTeam as SubTeam
		,	RL.Store_No					-- Store_No for the PurchaseLocation(!) store
		,	SUM(oi.QuantityOrdered) as Qty
		from OrderHeader oh (NOLOCK)
		left join OrderItem oi (NOLOCK) on oh.OrderHeader_ID = oi.OrderHeader_ID
		left JOIN Vendor RL (NOLOCK) ON RL.Vendor_ID = OH.ReceiveLocation_ID 
		where 
			Transfer_SubTeam is not null		-- filter for "store transfers"
		    -- filter for the Kitchen
			and oh.Vendor_ID = @VendorID

		    -- filter for team input criteria
		    and oh.Transfer_To_SubTeam in (SELECT SubTeam_No FROM SubTeam where Team_No = isnull(@Team, Team_No))  

		    -- filter for SubTeam input criteria			
			and oh.Transfer_To_SubTeam = isnull(@SubTeam, oh.Transfer_To_SubTeam)

			-- correct filters for pending orders?
			AND OH.CloseDate IS NULL				
		--	AND ISNULL(Expected_Date, 0) >= ISNULL(GETDATE(), ISNULL(Expected_Date, 0)) 
			AND OH.Sent = 1							
			AND OI.DateReceived IS NULL				
			AND OH.Return_order = 0 -- POs only, no credits (returns)
			AND Store_No IS NOT NULL	-- *** KEEP THIS CRITERIA ?? (ReceiveLocation_ID must be a Vendor_ID with a non-null Store_No)
			AND CONVERT(varchar, OH.Expected_Date, 101) BETWEEN CONVERT(varchar, @StartDate, 101) AND CONVERT(varchar, @EndDate, 101)

			GROUP BY oh.Vendor_ID, oi.Item_Key, RL.Store_No, oh.Transfer_To_SubTeam
			) as Core
		left JOIN Vendor k (NOLOCK) ON k.Vendor_ID = Core.Vendor_ID 
		left join SubTeam st (NOLOCK) on Core.SubTeam = st.SubTeam_No
		left join Team (NOLOCK) on st.Team_No = Team.Team_No
		left join Item (NOLOCK) on Item.Item_Key = Core.Item_Key
		left join ItemUnit (NOLOCK) on Item.Package_Unit_ID = ItemUnit.Unit_ID
		left join 
			(select * from dbo.ItemIdentifier (NOLOCK) where Default_Identifier = 1) ii --Bug 9638 removed where IdentifierType = 'S' Added Default_Identifier = 1
			on Item.Item_Key = ii.Item_Key

		INNER JOIN ProdHierarchyLevel3 ON Item.Category_ID =  ProdHierarchyLevel3.Category_ID 
			AND ProdHierarchyLevel3.ProdHierarchyLevel3_ID = ISNULL(@Level3, ProdHierarchyLevel3.ProdHierarchyLevel3_ID)
		INNER JOIN ProdHierarchyLevel4 ON ProdHierarchyLevel3.ProdHierarchyLevel3_ID =  ProdHierarchyLevel4.ProdHierarchyLevel3_ID 
			AND ProdHierarchyLevel4.ProdHierarchyLevel4_ID = ISNULL(@Level4, ProdHierarchyLevel4.ProdHierarchyLevel4_ID)
		
		where Item.Category_ID = isnull(@categoryID, Item.Category_ID)


	IF EXISTS (SELECT * FROM sysobjects WHERE type = 'U' AND name = '#Totals')
		BEGIN
			DROP TABLE #Totals
		END

	SELECT Team_No,	SubTeam, Item_Key, SUM(Qty) as Orders, COUNT(*) as StoreCount 
		INTO #Totals
		FROM #ReportData
		GROUP BY Team_No, SubTeam, Item_Key

	--SELECT * FROM #Totals
	--	ORDER BY Team_No, SubTeam, Item_Key
	 
	IF EXISTS (SELECT * FROM sysobjects WHERE type = 'U' AND name = '#XJoin')
		BEGIN
			DROP TABLE #XJoin
		END

	-- Make Cross Join to make records for every Item for every Store_No
	SELECT 
		h.Team_No
	,	h.SubTeam
	,	h.Item_Key
	,	s.Store_No
	INTO #XJoin
	FROM (
		SELECT DISTINCT
			Team_No
		,	SubTeam
		,	Item_Key
		FROM #ReportData
		) as h
	CROSS JOIN (
		SELECT DISTINCT Store_No 
			FROM #ReportData
			WHERE Store_No is not null
		) as s

	-- Linkin the Qty to make the #Pivot table

	IF EXISTS (SELECT * FROM sysobjects WHERE type = 'U' AND name = '#Pivot')
		DROP TABLE #Pivot

	SELECT 
			x.Team_No 
		,	x.SubTeam 
		,	x.Item_Key 
		,	x.Store_No
		,	r.Qty
	INTO #Pivot
	FROM #XJoin x
	LEFT JOIN #ReportData r
		on 	x.Team_No = r.Team_No
		and	x.SubTeam = r.SubTeam
		and	x.Item_Key = r.Item_Key
		and	x.Store_No = r.Store_No

	-- Convert Nulls to Zeros per business requirements
	UPDATE #Pivot
		SET Qty = 0
		WHERE Qty is null

	-- Pad the SKUs to 12 characters
	UPDATE #ReportData
		SET SKU = RIGHT('000000000000'+SKU, 12)
		WHERE SKU is not null

	IF EXISTS (SELECT * FROM sysobjects WHERE type = 'U' AND name = '#ReportData2')
		DROP TABLE #ReportData2

	SELECT DISTINCT
		r.Kitchen
	,	r.Vendor_ID
	,	r.Team_Name
	,	r.SubTeamAbbr
	,	r.SKU
	,	r.ProductDesc
	,	r.UOM
	,	t.Orders
	,	t.StoreCount
	,	s.* 
	INTO #ReportData2
	FROM #Pivot s
		inner join #ReportData r
			on s.Team_No = r.Team_No
			and	s.SubTeam = r.SubTeam
			and	s.Item_Key = r.Item_Key
		inner join #Totals t
			on s.Team_No = t.Team_No
			and	s.SubTeam = t.SubTeam
			and	s.Item_Key = t.Item_Key

	--SELECT * FROM #ReportData2
	--	ORDER BY Team_No ,	SubTeam ,	Item_Key ,	Store_No


	IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'#ReportData3') AND type in (N'U'))
		BEGIN
			DROP TABLE #ReportData3
		END

	CREATE TABLE #ReportData3
	(
		RowID int IDENTITY(1,1) NOT NULL,
		Kitchen varchar(50)  NULL,
		Vendor_ID int NULL,
		Team_Name varchar(100)  NULL,
		SubTeamAbbr varchar(10)  NULL,
		UOM varchar(8000)  NULL,
		Orders int NULL,
		StoreCount int NULL,
		Team_No int NULL,
		SubTeam int NULL,
		Item_Key int NOT NULL,
		Item_Description varchar(60)  NULL,
		Identifier varchar(13)  NULL,
		Store01Name varchar(5)  NULL,
		Store01Qty smallint NULL,
		Store02Name varchar(5)  NULL,
		Store02Qty smallint NULL,
		Store03Name varchar(5)  NULL,
		Store03Qty smallint NULL,
		Store04Name varchar(5)  NULL,
		Store04Qty smallint NULL,
		Store05Name varchar(5)  NULL,
		Store05Qty smallint NULL,
		Store06Name varchar(5)  NULL,
		Store06Qty smallint NULL,
		Store07Name varchar(5)  NULL,
		Store07Qty smallint NULL,
		Store08Name varchar(5)  NULL,
		Store08Qty smallint NULL,
		Store09Name varchar(5)  NULL,
		Store09Qty smallint NULL,
		Store10Name varchar(5)  NULL,
		Store10Qty smallint NULL,
		 CONSTRAINT PK_tmpStoreOrdersSKU PRIMARY KEY CLUSTERED 
		(
			RowID ASC
		)
	)

	-- Loop through records and fill tmpStoreOrdersSKU
	-- key fields are
	--		x.Team_No 
	--	,	x.SubTeam 
	--	,	x.Item_Key 
	--	,	x.Store_No

	DECLARE @CurTeam as int,
			@CurSubTeam as int,
			@CurItem_Key as int,
			@CurStore_No as int,
			@StoreCount int,
			@FieldNum int,
			@QueryStr as varchar(1000),
			@StoreName int,
			@StoreQty int,
			@LastRowID as int

	select @CurTeam = min(Team_No) from #ReportData2 

	-- FOR EACH Team_No...	
	while isnull(@CurTeam, 0) != 0 begin
		
		select @CurSubTeam = min(SubTeam) from #ReportData2 
			where Team_No = @CurTeam

		-- FOR EACH SubTeam...	
		while isnull(@CurSubTeam, 0) != 0 begin
			
			select @CurItem_Key = min(Item_Key) from #ReportData2 
			where Team_No = @CurTeam
				and SubTeam = @CurSubTeam

			-- FOR EACH Item_Key...	
			while isnull(@CurItem_Key, 0) != 0 begin

				select @CurStore_No = min(Store_No) from #ReportData2 
				where Team_No = @CurTeam
					and SubTeam = @CurSubTeam
					and Item_Key = @CurItem_Key

				set @StoreCount = 0
				set @FieldNum = 0

				-- FOR EACH Store_No...	
				while isnull(@CurStore_No, 0) != 0 begin

					-- do processing
					set @StoreCount = @StoreCount + 1
					set @FieldNum = @StoreCount % @StoresPerReportRow

					if @FieldNum = 1 begin
						-- INSERT new record
						INSERT INTO #ReportData3
								   (Kitchen,Vendor_ID,Team_Name,SubTeamAbbr,UOM,Team_No
								   ,SubTeam,Item_Key,Item_Description,Identifier,Orders, StoreCount)
						SELECT Kitchen,Vendor_ID,Team_Name,SubTeamAbbr,UOM,Team_No
								   ,SubTeam,Item_Key,ProductDesc,SKU,Orders, StoreCount
						FROM #ReportData2
							where Store_No = @CurStore_No
							and Team_No = @CurTeam
							and SubTeam = @CurSubTeam
							and Item_Key = @CurItem_Key

						SELECT @LastRowID = SCOPE_IDENTITY()
					end

					SELECT @StoreName = Store_No,
						   @StoreQty = Qty
					FROM #ReportData2
						where Store_No = @CurStore_No
						and Team_No = @CurTeam
						and SubTeam = @CurSubTeam
						and Item_Key = @CurItem_Key

					if @FieldNum = 0 
						set @FieldNum = @StoresPerReportRow

					-- Update the appropriate field
					set @QueryStr = 'UPDATE #ReportData3 SET Store'+right(replicate(0, 2) + cast(@FieldNum as varchar), 2)
								+'Name = '+convert(varchar,@StoreName)+', Store'+right(replicate(0, 2) + cast(@FieldNum as varchar), 2)
								+'Qty = '+convert(varchar,@StoreQty)+' WHERE RowID = '+convert(varchar, @LastRowID )
					EXEC (@QueryStr)
					--print @QueryStr 

					-- scroll to the next Store_No 
					select @CurStore_No = min(Store_No) from #ReportData2 
						where Store_No > @CurStore_No
						and Team_No = @CurTeam
						and SubTeam = @CurSubTeam
						and Item_Key = @CurItem_Key

	-- IF @LastRowID > 800 return
			
				end

				-- scroll to the next Item_Key 
				select @CurItem_Key = min(Item_Key) from #ReportData2 
					where Item_Key > @CurItem_Key
					and Team_No = @CurTeam
					and SubTeam = @CurSubTeam
			end

			-- scroll to the next SubTeam 
			select @CurSubTeam = min(SubTeam) from #ReportData2 
				where SubTeam > @CurSubTeam
				and Team_No = @CurTeam
		end

		-- scroll to the next Team 
		select @CurTeam = min(Team_No) from #ReportData2 
			where Team_No > @CurTeam

	end

	SELECT * FROM #ReportData3
	RETURN

END


SET QUOTED_IDENTIFIER ON
GO
GRANT EXECUTE
    ON OBJECT::[dbo].[StoreOrdersTotBySKUReport] TO [IRMAAdminRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[StoreOrdersTotBySKUReport] TO [IRMAClientRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[StoreOrdersTotBySKUReport] TO [IRMAReportsRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[StoreOrdersTotBySKUReport] TO [IRMAExcelRole]
    AS [dbo];

