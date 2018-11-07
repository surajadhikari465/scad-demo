
CREATE PROCEDURE [dbo].[AutomaticOrderList]
	@OrderHeader_ID		int,
	@SearchSubTeam_No	int,
	@DistSubTeam_No		int, 
	@Not_Available		tinyint,
	@ProductType_ID		int 

AS 

-- **************************************************************************
-- Procedure: AutomaticOrderList()
--    Author: n/a
--      Date: n/a
--
-- Description:
-- This procedure is called from IRMA Client code to populate the OrderList
-- UltraGrid
--
-- Modification History:
-- Date      Init	TFS		Comment
-- 09/02/09  BBB			Added join to ItemOverride for PackageUnitID
-- 09/03/09  BBB			Corrected issue with INNER instead of LEFT
-- 11/17/09  BBB			update existing SP that will correctly identify Non-Regional
--							WFM Facilities and return utilize the vendor on the order
--							to retrieve item auth as opposed to the store placing the order;
--							reformatted for readability;
-- 04/06/10  BBB			Removed lookup needed for GL Enhancements and leave values
--							set in initial query as is
-- 07//5/10  BSR			Swapped use of VCA.UnitCost to VCA.NetCost, also added code 
--							to use SentDate if available for VCH calls.
-- 01/24/11  TTL	759		Updated @CostDate var to include any vendor lead-time in the date used to pull vendor cost attributes.
--							Only updated to include lead-time for @OrderType_ID = 1, not @OrderType_ID = 2 because these sections of code use different dates.
-- 02/08/11  BBB	1367	reformatted SQL (1) per coding standards;
-- 08/17/11  BBB	2716	Removed extraneous tables and left joins; formatted 2nd query
-- 12/29/11	 BJL	3751	Added RegCost = VCA.UnitCost to return.
-- 02/08/12  BJL	4681	Added ISNULL check on VCA.UnitCost to return 0 for NULL UnitCost.
-- 01/03/13	 KM		9251	Check ItemOverride for new 4.8 override values (Not_Available, Not_AvailableNote, Brand_Name);
-- 01/04/13  BS		8755	Updated Item.Discontinue_Item with siv.DiscontinueItem due to schema change
-- 10/15/13	 BS		1066	Create temp tables of Item_Keys, Cost Info	and Orderitem
--							to JOIN to main query to improve performance
-- **************************************************************************

BEGIN      
	SET NOCOUNT ON      

	--**************************************************************************
	--Internal variables
	--**************************************************************************     
	DECLARE 
		@Vendor_ID						int,       
		@VendStore_No					int,       
		@CustStore_No					int,       
		@RecvVend_ID					int,   
		@Transfer_From_SubTeam			int,       
		@Transfer_To_SubTeam			int,       
		@WFM_Store						int,       
		@HFM_Store						int,       
		@Distribution_Center			int,       
		@Manufacturer					int,       
		@OrderType_ID					int,       
		@Internal						tinyint,       
		@CustBusinessUnit_ID			int,       
		@IsVendorInternalManufacturer	bit,       
		@Now							smalldatetime,  
		@ReturnRows						int,
		@Cost							money,
		@CloseDate						datetime,
		@EXEWarehouse					int,
		@WFM							int,
		@Customer						int,
		@CostDate						smalldatetime,
		@StoreJurisdictionID			int
	
	--**************************************************************************
	-- Get the Order Header info    
	--**************************************************************************
	SELECT	
		@Transfer_From_SubTeam			= oh.Transfer_SubTeam,       
		@Transfer_To_SubTeam			= oh.Transfer_To_SubTeam,      
		@Vendor_ID						= oh.Vendor_ID,       
		@RecvVend_ID					= oh.ReceiveLocation_ID,
		@VendStore_No					= v.Store_No,     
		@CustStore_No					= vc.Store_No,      
		@CustBusinessUnit_ID			= sc.BusinessUnit_ID,      
		@OrderType_ID					= oh.OrderType_ID,       
		@WFM_Store						= sc.WFM_Store,       
		@HFM_Store						= sc.Mega_Store,       
		@Distribution_Center			= sc.Distribution_Center,       
		@Manufacturer					= sc.Manufacturer,      
		@Internal						= sc.Internal,
		@EXEWarehouse					= sv.EXEWarehouse,      
		@IsVendorInternalManufacturer	= CASE 
											WHEN (sv.Manufacturer = 1) AND (sv.BusinessUnit_ID IS NOT NULL) THEN 1       
											ELSE 0
										  END,      
		@Now							= GETDATE(),
		@CloseDate						= oh.CloseDate,
		@CostDate						= ISNULL(oh.SentDate, GETDATE()) + dbo.fn_GetLeadTimeDays(oh.Vendor_ID),
		@StoreJurisdictionID			= sc.StoreJurisdictionID
	
	FROM 
		Vendor					(nolock) v       
		INNER JOIN OrderHeader	(nolock) oh		ON (v.Vendor_ID		= oh.Vendor_ID)      
		INNER JOIN Vendor		(nolock) vc		ON (vc.Vendor_ID	= oh.ReceiveLocation_ID)      
		LEFT JOIN Store			(nolock) sc		ON (sc.Store_No		= vc.Store_No)      
		LEFT JOIN Store			(nolock) sv		ON (v.Store_No		= sv.Store_No)      
		 
	WHERE 
		oh.OrderHeader_ID = @OrderHeader_ID

	--**************************************************************************
	-- Populate SubTeamList
	--**************************************************************************
	DECLARE @SubTeamList TABLE (SubTeam_No int primary key)  

	IF @ProductType_ID = 1 AND @Distribution_Center = 0
		INSERT @SubTeamList                       
			SELECT SubTeam_No FROM SubTeam (nolock) WHERE SubTeamType_ID IN (1,2,3,4,7)
		
	IF @ProductType_ID = 1 AND @Distribution_Center = 1
		INSERT @SubTeamList                       
			SELECT SubTeam_No FROM SubTeam (nolock) WHERE SubTeamType_ID IN (1,2,3,4,5,6)
			
	IF @ProductType_ID = 2  
		INSERT @SubTeamList  
			SELECT SubTeam_No FROM SubTeam (nolock) WHERE SubTeamType_ID = 5  
			
	IF @ProductType_ID = 3  
		INSERT @SubTeamList  
			SELECT SubTeam_No FROM SubTeam (nolock) WHERE SubTeamType_ID = 6  
  
	IF @SearchSubTeam_No = NULL  
		SET @ReturnRows = 1000  
	ELSE  
		SET @ReturnRows = 0

	--**************************************************************************	
	-- Get List of Item_Keys and item Data based on Store, Vendor, and SubTeams
	--**************************************************************************	
	SELECT
		siv.Item_Key			as Item_Key,
		siv.DiscontinueItem		as DiscontinueItem,
		siv.StoreItemVendorID	as StoreItemVendorID,
		i.Package_Desc1			as Package_Desc1,
		i.Package_Desc2			as Package_Desc2,
		i.Package_Unit_ID		as Package_Unit_ID,
		i.Vendor_Unit_ID		as Vendor_Unit_ID,
		i.Distribution_Unit_ID	as Distribution_Unit_ID,
		i.Item_Description		as Item_Description,
		i.Pre_Order				as Pre_Order,
		i.Not_Available			as Not_Available,
		i.Category_ID			as Category_ID,
		i.SubTeam_No			as SubTeam_No,
		i.Not_AvailableNote		as Not_AvailableNote,
		i.EXEDistributed		as EXEDistributed,
		i.DistSubTeam_No		as DistSubTeam_No,
		i.HFM_Item				as HFM_Item,
		i.WFM_Item				as WFM_Item,
		i.Brand_ID				as Brand_ID
	INTO #itemKeys
	FROM
		Item						i	(nolock)
		INNER JOIN StoreItem		si	(nolock) on i.Item_Key			= si.Item_Key
													AND si.Store_No		= @CustStore_No
		INNER JOIN StoreItemVendor	siv (nolock) on i.Item_Key			= siv.Item_Key
													AND si.Store_No		= siv.Store_No
													AND siv.Vendor_ID	= @Vendor_ID
													AND (siv.DeleteDate IS NULL OR siv.DeleteDate >= @Now)
		INNER JOIN @SubTeamList	st				 on i.SubTeam_No		= st.SubTeam_No
	WHERE
		i.Deleted_Item = 0
		AND i.Remove_Item = 0
		AND (@WFM_Store <> 1 OR si.Authorized = @WFM_Store)
		AND (@WFM_Store <> 1 OR i.WFM_Item = @WFM_Store)
		AND (@HFM_Store <> 1 OR i.HFM_Item = @HFM_Store)
		AND (i.DistSubTeam_No = @DistSubTeam_No 
			OR @DistSubTeam_No = 0
			OR i.DistSubTeam_No = i.DistSubTeam_No 
			OR 0 = 0)

	create clustered	index idx_itemKeys_Item_Key				on #itemKeys (Item_Key)
	create nonclustered	index idx_itemKeys_StoreItemVendorID	on #itemKeys (StoreItemVendorID)

	--****************************************************************************	
	-- Manually get cost information (this is faster than using fn_VendorCostAll)
	--****************************************************************************
	-------------------------------------------------------
	-- Get Max VendorCostHistoryIDs for latest cost record
	-------------------------------------------------------
	SELECT
		itk.StoreItemVendorID		 as StoreItemVendorID,
		MAX(vch.VendorCostHistoryID) as VendorCostHistoryID
	INTO #costIDs
	FROM
		#itemKeys						itk
		INNER JOIN VendorCostHistory	vch (nolock) on itk.StoreItemVendorID = vch.StoreItemVendorID
	WHERE
		((@CostDate >= vch.StartDate) AND (@CostDate <= vch.EndDate))
	GROUP BY
		itk.StoreItemVendorID

	create clustered index idx_costIDs_VendorCostHistoryID on #costIDs (VendorCostHistoryID)
	create nonclustered index idx_costIDs_StoreItemVendorID on #costIDs (StoreItemVendorID) INCLUDE (VendorCostHistoryID)

	--------------------------------------------------
	-- Get Promo Insert Dates from VendorDealHistory
	--------------------------------------------------
	select
		cid.StoreItemVendorID	as StoreItemVendorID,
		max(vdh.insertdate)		as InsertDate
	into #promoDates
	from
		#costIDs					cid	
		left join vendordealhistory vdh (nolock) on cid.StoreItemVendorID	= vdh.StoreItemVendorID

	where
		CONVERT(VARCHAR(10), @CostDate, 101) between vdh.StartDate and vdh.EndDate
	group by
		cid.StoreItemVendorID

	create clustered index idx_promoDates_StoreItemVendorID on #promoDates (StoreItemVendorID)
	create nonclustered index ix_promoDates_InsertDate on #promoDates (InsertDate) include (StoreItemVendorID)

	----------------------------------------------------------
	-- Get promo amounts based on Insert Dates in #promoDates
	----------------------------------------------------------
	select
		cd.StoreItemVendorID	as StoreItemVendorID,
		cd.VendorCostHistoryID	as VendorCostHistoryID,
		ISNULL(SUM(	CASE 
						WHEN vdt.CaseAmtType = '%' THEN (vdh.CaseAmt / 100) * ISNULL(vch.UnitCost,0)
						ELSE vdh.CaseAmt
					END),0)		as PromoAmount
	into #promos
	from
		#costIDs cd
		inner join VendorCostHistory vch (nolock) on cd.VendorCostHistoryID = vch.VendorCostHistoryID
		left join #promoDates		 prd (nolock) on cd.StoreItemVendorID = prd.StoreItemVendorID
		left join vendordealhistory vdh (nolock) on cd.StoreItemVendorID	= vdh.StoreItemVendorID
		left join VendorDealType	vdt	(nolock) on vdh.VendorDealTypeID	= vdt.VendorDealTypeID
	where
		prd.InsertDate = vdh.InsertDate
		and CONVERT(VARCHAR(10), @CostDate, 101) BETWEEN vdh.StartDate and vdh.EndDate
	group by
		cd.StoreItemVendorID,
		cd.VendorCostHistoryID
	
	create nonclustered index idx_promos_StoreItemVendorID on #promos (StoreItemVendorID) include (PromoAmount)

	--------------------------------------------------
	-- Put All Needed Cost Fields into it's own table
	--------------------------------------------------
	select
		cid.StoreItemVendorID	as StoreItemVendorID,
		cid.VendorCostHistoryID as VendorCostHistoryID,
		vch.UnitCost			as UnitCost,
		vch.Package_Desc1		as Package_Desc1,
		ISNULL(vch.UnitCost,0) - ISNULL(pr.PromoAmount,0) + ISNULL(vch.UnitFreight,0)	as NetCost
	into #vendorCost
	from
		#costIDs					cid
		left join VendorCostHistory vch (nolock) on cid.VendorCostHistoryID		= vch.VendorCostHistoryID
		left join #promos			pr	(nolock) on cid.StoreItemVendorID		= pr.StoreItemVendorID

	create nonclustered index idx_vendorCost_StoreItemVendorID on #vendorCost (StoreItemVendorID) include (VendorCostHistoryID, UnitCost, Package_Desc1, NetCost)

	--**************************************************************************	
	-- Get OrderItem Information (this removes large GROUP BY in main query)
	--**************************************************************************
	SELECT
		oi.Item_Key					as Item_Key,
		MAX(oi.OrderItem_ID)		as OrderItem_ID,
		MAX(oi.QuantityOrdered)		as QuantityOrdered,
		MAX(oi.CreditReason_ID)		as CreditReason_ID,
		SUM(oi.QuantityReceived)	as QuantityReceived,
		MAX(cr.CreditReason)		as CreditReason,
		MAX(iuq.Unit_Name)			as Unit_Name,
		MAX(iuq.Unit_ID)			as QuantityUnit,
		COUNT(*)					as OrderCount
	INTO #orderItems
	FROM
		#itemKeys				i
		INNER JOIN OrderItem	oi	(nolock) on i.Item_Key					= oi.Item_Key
		LEFT JOIN ItemUnit		iuq	(nolock) on oi.QuantityUnit				= iuq.Unit_ID
		LEFT JOIN CreditReasons	cr	(nolock) on oi.CreditReason_ID			= cr.CreditReason_ID
		LEFT JOIN ItemOverride	ior (nolock) on i.Item_Key					= ior.Item_Key
												AND ior.StoreJurisdictionID = @StoreJurisdictionID
	WHERE
		oi.OrderHeader_ID = @OrderHeader_ID
	GROUP BY
		oi.Item_Key

	create nonclustered index idx_orderItems_Item_Key on #orderItems (Item_Key)

	--**************************************************************************	
	-- PURCHASE ORDER
	--**************************************************************************	
	IF @OrderType_ID = 1 -- Purchase  
	BEGIN

		SELECT
			[Item_Key]				= ik.Item_Key,
			[VendorCostHistoryID]	= vca.VendorCostHistoryID, 
			[Package_Desc1]			= vca.Package_Desc1,
			[Package_Desc2]			= ISNULL(ior.Package_Desc2, ik.Package_Desc2),       
			[Package_Unit]			= ISNULL(ivp.Unit_Abbreviation, iup.Unit_Abbreviation),  
			[Identifier]			= ii.Identifier,       
			[Item_Description]		= ISNULL(ior.Item_Description, ik.Item_Description),      
			[Pre_Order]				= ik.Pre_Order,      
			[Not_Available]			= ISNULL(ior.Not_Available, ik.Not_Available),      
			[Category_ID]			= ik.Category_ID,       
			[Category_Name]			= ic.Category_Name, 
			[OrderItem_ID]			= oi.OrderItem_ID,  
			[QuantityOrdered]		= oi.QuantityOrdered,
			[QuantityUnit]			= ISNULL(oi.QuantityUnit, ISNULL(ior.Vendor_Unit_Id, ik.Vendor_Unit_Id)),
			[SubTeam_No]			= ISNULL(p.ExceptionSubteam_No, ik.SubTeam_No),      
			[CreditReason_ID]		= oi.CreditReason_ID,
			[Not_AvailableNote]		= ISNULL(ior.Not_AvailableNote, ik.Not_AvailableNote),      
			[EXEDistributed]		= ik.EXEDistributed,       
			[DistSubTeam_No]		= ik.DistSubTeam_No,       
			[Discontinue_Item]		= ik.DiscontinueItem,      
			[Unit_Name]				= oi.Unit_Name,
			[QuantityReceived]		= oi.QuantityReceived,
			[OrderCount]			= ISNULL(oi.OrderCount,1),
			[Vendor_Unit]			= iuv.Unit_Name,      
			[Vendor_Unit_Id]		= iuv.Unit_ID,
			[CreditReason]			= oi.CreditReason,
			[VendorItemID]			= iv.Item_ID, 
			[Brand]					= ISNULL(ibo.Brand_Name, ib.Brand_Name),
			[Cost]					= vca.NetCost,
			[CurrentVendorCost]		= ISNULL(vca.UnitCost,0),
			[VendorItemStatus]		= vis.StatusCode,
			[VendorItemStatusFull]	= vis.StatusName,
			[VendorItemStatusSort]	= CASE WHEN vis.StatusCode IS NULL THEN 'ZZZZZ-NULL' ELSE vis.StatusCode END

		FROM
			#itemKeys					ik
			INNER JOIN ItemIdentifier	ii	(nolock) on ik.Item_Key			= ii.Item_Key
														AND ii.Default_Identifier = 1
														AND ii.Deleted_Identifier = 0
			INNER JOIN Price			p	(nolock) on ik.Item_Key			= p.Item_Key
														AND p.Store_No		= @CustStore_No
			INNER JOIN ItemVendor		iv	(nolock) on ik.Item_Key			= iv.Item_Key
														AND iv.Vendor_ID	= @Vendor_ID
			INNER JOIN ItemUnit			iup	(nolock) on ik.Package_Unit_ID	= iup.Unit_ID
			INNER JOIN ItemUnit			iuv	(nolock) on ik.Vendor_Unit_ID	= iuv.Unit_ID
			INNER JOIN ItemBrand		ib	(nolock) on ik.Brand_ID			= ib.Brand_ID
			LEFT JOIN #vendorCost		vca			 on ik.StoreItemVendorID = vca.StoreItemVendorID
			LEFT JOIN #orderItems		oi			 on ik.Item_Key			= oi.Item_Key
			LEFT JOIN ItemOverride		ior (nolock) on ik.Item_Key			= ior.Item_Key
														AND ior.StoreJurisdictionID = @StoreJurisdictionID
			LEFT JOIN VendorItemStatuses vis (nolock) on iv.VendorItemStatus	=	vis.StatusID
			LEFT JOIN ItemCategory		ic	(nolock) on ik.Category_ID		= ic.Category_ID
			LEFT JOIN ItemBrand			ibo	(nolock) on ior.Brand_ID		= ibo.Brand_ID
			LEFT JOIN ItemUnit			ivp	(nolock) on	ior.Package_Unit_ID	= ivp.Unit_ID
			LEFT JOIN ItemUnit			ivv	(nolock) on ior.Vendor_Unit_ID	= ivv.Unit_ID
		WHERE
			(ior.Not_Available		= @Not_Available 
			OR ik.Not_Available		= @Not_Available
			OR ior.Not_Available	= ior.Not_Available 
			OR ik.Not_Available		= ik.Not_Available)
		ORDER BY
			OrderCount DESC,
			QuantityReceived DESC

	END
	  
	--**************************************************************************	
	-- DISTRIBUTION ORDER
	--**************************************************************************	
	IF @OrderType_ID = 2 -- Distribution   
	BEGIN   
		SET ROWCOUNT @ReturnRows  

		SELECT
			[Item_Key]				= ik.Item_Key,
			[VendorCostHistoryID]	= vca.VendorCostHistoryID, 
			[Package_Desc1]			= vca.Package_Desc1,    
			[Package_Desc2]			= ISNULL(ior.Package_Desc2, ik.Package_Desc2),       
			[Package_Unit]			= ISNULL(ivp.Unit_Abbreviation, iup.Unit_Abbreviation),  
			[Identifier]			= ii.Identifier,       
			[Item_Description]		= ISNULL(ior.Item_Description, ik.Item_Description),      
			[Pre_Order]				= ik.Pre_Order,      
			[Not_Available]			= ISNULL(ior.Not_Available, ik.Not_Available),      
			[Category_ID]			= ik.Category_ID,       
			[Category_Name]			= ic.Category_Name,       
			[OrderItem_ID]			= oi.OrderItem_ID,    
			[QuantityOrdered]		= oi.QuantityOrdered,
			[QuantityUnit]			= ISNULL(oi.QuantityUnit, ISNULL(ior.Vendor_Unit_Id, ik.Vendor_Unit_Id)),
			[SubTeam_No]			= ISNULL(p.ExceptionSubteam_No, ik.SubTeam_No),
			[CreditReason_ID]		= oi.CreditReason_ID,
			[Not_AvailableNote]		= ISNULL(ior.Not_AvailableNote, ik.Not_AvailableNote),      
			[EXEDistributed]		= ik.EXEDistributed,       
			[DistSubTeam_No]		= ik.DistSubTeam_No,       
			[Discontinue_Item]		= ik.DiscontinueItem,      
			[Unit_Name]				= oi.Unit_Name,       
			[QuantityReceived]		= oi.QuantityReceived,
			[OrderCount]			= ISNULL(oi.OrderCount,1),
			[Vendor_Unit]			= ISNULL(ivd.Unit_Name, iud.Unit_Name),      
			[Vendor_Unit_Id]		= ISNULL(ivd.Unit_ID, iud.Unit_ID),
			[CreditReason]			= oi.CreditReason,
			[VendorItemID]			= iv.Item_ID, 
			[Brand]					= ISNULL(ibo.Brand_Name, ib.Brand_Name),
			[Cost]					=	CASE 
											WHEN @EXEWarehouse = 1 THEN
												(dbo.fn_AvgCostHistory(ik.Item_Key, @VendStore_No, @Transfer_From_SubTeam, ISNULL(@CloseDate,@Now)) * vca.Package_Desc1) + dbo.fn_GetCurrentHandlingCharge(ik.Item_Key, @Vendor_ID)
											ELSE
												dbo.fn_GetCurrentCostForVendor(ik.Item_Key, @CustStore_No,@Vendor_Id) 
										END,
			[CurrentVendorCost]		=	CASE 
											WHEN @Distribution_Center = 1 THEN 
												ISNULL((dbo.fn_AvgCostHistory(ik.Item_Key, @VendStore_No,  @Transfer_From_SubTeam, ISNULL(@CloseDate,@Now)) * vca.Package_Desc1), 0)
											ELSE 
												ISNULL(vca.UnitCost,0)
										END,
			[VendorItemStatus]		= vis.StatusCode,
			[VendorItemStatusFull]	= vis.StatusName,
			[VendorItemStatusSort]	= CASE WHEN vis.StatusCode IS NULL THEN 'ZZZZZ-NULL' ELSE vis.StatusCode END

		FROM
			#itemKeys						ik
			INNER JOIN ItemIdentifier		ii	(nolock) on ik.Item_Key					= ii.Item_Key
															AND ii.Default_Identifier	= 1
															AND ii.Deleted_Identifier	= 0
			INNER JOIN ItemVendor			iv	(nolock) on ik.Item_Key					= iv.Item_Key
															AND iv.Vendor_ID			= @Vendor_ID
			INNER JOIN ItemUnit				iup	(nolock) on ik.Package_Unit_ID			= iup.Unit_ID
			INNER JOIN ItemUnit				iud	(nolock) on ik.Distribution_Unit_ID		= iud.Unit_ID
			INNER JOIN ItemBrand			ib	(nolock) on ik.Brand_ID					= ib.Brand_ID
			LEFT JOIN #orderItems			oi	(nolock) on ik.Item_Key					= oi.Item_Key
			LEFT JOIN ItemOverride			ior (nolock) on ik.Item_Key					= ior.Item_Key
															AND ior.StoreJurisdictionID = @StoreJurisdictionID
			LEFT JOIN VendorItemStatuses	vis (nolock) on iv.VendorItemStatus			= vis.StatusID
			LEFT JOIN Price					p	(nolock) on ik.Item_Key					= p.Item_Key
															AND p.Store_No				= @CustStore_No
			LEFT JOIN ItemUnit				iuq	(nolock) on oi.QuantityUnit				= iuq.Unit_ID
			LEFT JOIN ItemCategory			ic	(nolock) on ik.Category_ID				= ic.Category_ID
			LEFT JOIN CreditReasons			cr	(nolock) on oi.CreditReason_ID			= cr.CreditReason_ID
			LEFT JOIN ItemBrand				ibo	(nolock) on ior.Brand_ID				= ibo.Brand_ID
			LEFT JOIN ItemUnit				ivp	(nolock) on	ior.Package_Unit_ID			= ivp.Unit_ID
			LEFT JOIN ItemUnit				ivd	(nolock) on ior.Distribution_Unit_ID	= ivd.Unit_ID
			LEFT JOIN #vendorCost			vca	(nolock) on ik.StoreItemVendorID		= vca.StoreItemVendorID
		WHERE
			(ior.Not_Available		= @Not_Available 
			OR ik.Not_Available		= @Not_Available
			OR ior.Not_Available	= ior.Not_Available 
			OR ik.Not_Available		= ik.Not_Available)
			AND (p.ExceptionSubteam_No		= @SearchSubTeam_No
				OR ik.SubTeam_No			= @SearchSubTeam_No
				OR p.ExceptionSubteam_No	= ik.SubTeam_No
				OR ik.SubTeam_No			= ik.SubTeam_No)
			AND (ik.DistSubTeam_No	= @Transfer_From_SubTeam
				OR (ik.SubTeam_No	= @Transfer_From_SubTeam AND ik.DistSubTeam_No IS NULL))
		ORDER BY
			OrderCount DESC,
			QuantityReceived DESC

	END  
	  
	SET NOCOUNT OFF      
END

GO
GRANT EXECUTE
    ON OBJECT::[dbo].[AutomaticOrderList] TO [IRMASupportRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[AutomaticOrderList] TO [IRMAClientRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[AutomaticOrderList] TO [IRMASchedJobsRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[AutomaticOrderList] TO [IRMAReportsRole]
    AS [dbo];

