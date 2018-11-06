SET QUOTED_IDENTIFIER ON 
GO
SET ANSI_NULLS ON 
GO


if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[GetPriceBatchHeaderDetailReport]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
	drop procedure [dbo].[GetPriceBatchHeaderDetailReport]
GO

CREATE PROCEDURE dbo.GetPriceBatchHeaderDetailReport
	@StoreList varchar(8000),
	@StoreListSeparator char(1),
	@SubTeam_No int,
	@StartDate smalldatetime,
	@EndDate smalldatetime,
	@PriceBatchStatusID tinyint
AS
-- **************************************************************************
-- Procedure: GetPriceBatchHeaderDetailReport()
--		Author: n/a
--			Date: n/a
--
-- Description:
-- This procedure is called from multiple RDL files and generates a report consumed
-- by SSRS procedures.
--
-- Modification History:
-- Date       	Init  	TFS   		Comment
-- 06/22/2011  	BBB		2308		removed redundant calls to VCH; removed perf
--									impacting scalar calls; removed perf impacting
--									subqueries;
-- 06/21/2011  	BBB		2308		applied coding standards;
-- **************************************************************************
BEGIN
	SET TRANSACTION ISOLATION LEVEL READ UNCOMMITTED
	SET NOCOUNT ON

	--**************************************************************************
	--Declare internal variables
	--**************************************************************************
	DECLARE @Store TABLE (Store_No int)

	--**************************************************************************
	--populate internal variables
	--**************************************************************************
	 IF @StoreList IS NOT NULL
		INSERT INTO @Store
		SELECT Key_Value
		FROM dbo.fn_Parse_List(@StoreList, @StoreListSeparator) S
	ELSE
		INSERT INTO @Store
		SELECT Store_No FROM Store (nolock)

	--**************************************************************************
	--main query
	--**************************************************************************
	SELECT 
		[BatchDescription]		=	pbh.BatchDescription, 
		[Store_Name]			=	ss.Store_Name,
		[SubTeam_Name]			=	st.SubTeam_Name,
		[Identifier]			=	ii.identifier,
		[Item_Description]		=	i.item_description, 
		[PackSize]				=	i.package_desc1, 
		[ItemSize]				=	i.package_desc2, 
		[UnitName]				=	iu.unit_name,
		[Vendor]				=	v.CompanyName, 
		[Cost]					=	vch.UnitCost, 
		[VendorPack]			=	vch.package_desc1, 
		[ChangeType]			=	CASE ISNULL(pbd.ItemChgTypeID, 0)	
										WHEN 0 THEN 'Price'
										WHEN 1 THEN 'New'
										WHEN 2 THEN 'Item'
										WHEN 3 THEN 'Delete' 
									END,
		[PriceType]				=	ISNULL(pct.PriceChgTypeDesc,''), 
		[RegPrice]				=	pbd.posprice,
		[SalePrice]				=	pbd.possale_price,
		[SaleEndDate]			=	pbd.sale_end_date, 
		[StartDate]				=	pbh.StartDate, 
		[BatchStatus]			=	pbs.PriceBatchStatusDesc,
		[ScaleDescription]		=	isc.scale_description1, 
		[ScaleUOM]				=	isu.unit_name,
		[FixedWeight]			=	isc.scale_fixedweight, 
		[ByCount]				=	isc.scale_bycount, 
		[Ingredients]			=	sce.extratext, 
		[Brand_Name]			=	ib.Brand_Name,
		[CaseSize]				=	vch.Package_Desc1,
		[CaseCost]				=	vch.UnitCost * vch.Package_Desc1,
		[Discount]				=	vdh.Discount,
		[DiscountStartDate]		=	vdh.DiscoStart,
		[DiscountEndDate]		=	vdh.DiscoEnd,
		[Allowance]				=	vdh.Allowance,
		[AllowanceStartDate]	=	vdh.AllowStart,
		[AllowanceEndDate]		=	vdh.AllowEnd,
		[NetUnitCost]			=	SUM(CASE
											WHEN vdh.DealType = '%' THEN
												(vch.UnitCost + UnitFreight) - ISNULL(((vdh.NetDiscount / 100) * vch.UnitCost), 0)
											ELSE
												(vch.UnitCost + UnitFreight) - ISNULL(vdh.NetDiscount, 0)
										END),
		[LastCostChangeDate]	=	vch.InsertDate,
		[CurrentPriceType]		=	ISNULL(cpct.PriceChgTypeDesc,''),
		[CurrentRegPrice]		=	p.Price,
		[CurrentSalePrice]		=	p.POSSale_Price,
		[CurrentMargin]			=	CASE vch.package_desc1
										WHEN 0 THEN 
											0
										ELSE 
											dbo.fn_GetMargin(ISNULL(p.POSSale_Price, p.Price), ISNULL(p.Sale_Multiple, p.Multiple), vch.UnitCost/vch.Package_Desc1)
									END,
		[FutureMargin]			=	CASE vch.package_desc1 
										WHEN 0 THEN 
											0 
										ELSE 
											dbo.fn_GetMargin(ISNULL(pbd.POSSale_Price,  pbd.Price), ISNULL(pbd.Sale_Multiple, pbd.Multiple), vch.UnitCost/vch.Package_Desc1) 
									END,
		[POSLinkCode]			=	pbd.POSLinkCode		
	FROM 
		PriceBatchHeader					(nolock) pbh
		INNER JOIN	PriceBatchDetail 		(nolock) pbd	ON	pbh.PriceBatchHeaderID		= pbd.PriceBatchHeaderID
		INNER JOIN	PriceBatchStatus		(nolock) pbs	ON	pbh.PriceBatchStatusID		= pbs.PriceBatchStatusID
		INNER JOIN	Item					(nolock) i		ON	pbd.Item_Key				= i.Item_Key
		INNER JOIN	@Store							 s		ON	pbd.Store_No				= s.Store_No
		INNER JOIN	Store					(nolock) ss		ON	s.Store_No					= ss.Store_No
		INNER JOIN	SubTeam					(nolock) st		ON	i.SubTeam_No				= st.SubTeam_No
		INNER JOIN	ItemUnit				(nolock) iu		ON	i.package_unit_id			= iu.Unit_id
		INNER JOIN	ItemIdentifier			(nolock) ii		ON	pbd.Item_Key				= ii.Item_Key
															AND	ii.Default_Identifier		= 1
		INNER JOIN	ItemBrand				(nolock) ib		ON	i.Brand_ID					= ib.Brand_ID
		INNER JOIN	Price					(nolock) p		ON	pbd.Item_Key				= p.Item_Key 
															AND	pbd.Store_No				= p.Store_No
		LEFT JOIN	PriceChgType			(nolock) pct	ON	pbh.PriceChgTypeID			= pct.PriceChgTypeID
		LEFT JOIN	PriceChgType			(nolock) cpct	ON	p.PriceChgTypeID			= cpct.PriceChgTypeID
		LEFT JOIN	StoreItemVendor			(nolock) siv	ON	pbd.item_key				= siv.item_key 
															AND	pbd.store_no				= siv.store_no 
															AND	siv.PrimaryVendor			= 1
															AND	(
																siv.DeleteDate				IS NULL
																OR
																siv.DeleteDate				>= pbd.StartDate
																)
		LEFT JOIN	ItemVendor				(nolock) iv		ON	siv.vendor_id				= iv.vendor_id 
															AND	siv.item_key				= iv.item_key
		LEFT JOIN 	Vendor					(nolock) v		ON	iv.vendor_id				= v.vendor_id
		LEFT JOIN	ItemScale				(nolock) isc	ON	i.item_key					= isc.item_key
		LEFT JOIN	ItemUnit				(nolock) isu	ON	isc.scale_scaleuomunit_id	= isu.unit_id
		LEFT JOIN	Scale_ExtraText			(nolock) sce	ON	isc.scale_extratext_id		= sce.scale_extratext_id
		
		CROSS APPLY
					(
						SELECT TOP 1 
							[NetCost]		=	CASE
													WHEN dbo.fn_IsCaseItemUnit(vh2.CostUnit_ID) = 1 THEN
														(ISNULL(vh2.UnitCost, 0) + ISNULL(vh2.UnitFreight, 0)) / vh2.Package_Desc1
													ELSE
														(ISNULL(vh2.UnitCost, 0) + ISNULL(vh2.UnitFreight, 0))
												END,
							[UnitFreight]	=	vh2.UnitFreight,
							[UnitCost]		=	vh2.UnitCost,
							[Package_Desc1]	=	vh2.Package_Desc1, 
							[CostUnit_ID]	=	vh2.CostUnit_ID,
							[CostUnit]		=	iu2.Unit_Abbreviation,
							[MSRP]			=	vh2.MSRP,
							[InsertDate]	=	vh2.InsertDate
						FROM 
							VendorCostHistory			(nolock) vh2
							INNER JOIN	ItemUnit		(nolock) iu2	ON	vh2.CostUnit_ID	= iu2.Unit_ID
						WHERE
							vh2.StoreItemVendorID	=	siv.StoreItemVendorID
							AND vh2.StartDate		<=	pbd.StartDate
							AND vh2.EndDate			>=	pbd.StartDate
						ORDER BY 
							vh2.VendorCostHistoryID DESC
					) AS vch		
		
		OUTER APPLY
					(
						SELECT
							[DealType]		= vt2.CaseAmtType,
							[NetDiscount]	=	SUM(vd2.CaseAmt),
							[Discount]		=	CASE 
													WHEN vt2.Code = 'D' THEN 
														SUM(vd2.CaseAmt)
													ELSE 
														NULL
												END,
							[Allowance]		=	CASE 
													WHEN vt2.Code = 'A' THEN 
														SUM(vd2.CaseAmt)
													ELSE
														NULL
												END,
							[DiscoStart]	=	CASE 
													WHEN vt2.Code = 'D' THEN 
														CONVERT(varchar(255), MIN(vd2.StartDate), 101)
													ELSE
														NULL
												END,
							[DiscoEnd]		=	CASE 
													WHEN vt2.Code = 'D' THEN 
														CONVERT(varchar(255), MAX(vd2.EndDate), 101)
													ELSE
														NULL
												END,
							[AllowStart]	=	CASE 
													WHEN vt2.Code = 'A' THEN 
														CONVERT(varchar(255), MIN(vd2.StartDate), 101)
													ELSE
														NULL
												END,
							[AllowEnd]		=	CASE 
													WHEN vt2.Code = 'A' THEN 
														CONVERT(varchar(255), MAX(vd2.EndDate), 101)
													ELSE
														NULL
												END
						FROM 
							VendorDealHistory			(nolock) vd2
							INNER JOIN VendorDealType	(nolock) vt2	ON vd2.VendorDealTypeID = vt2.VendorDealTypeID
						WHERE
							pbd.StartDate				BETWEEN vd2.StartDate AND vd2.EndDate
							AND vt2.Code				IN		('A', 'D')
							AND vd2.StoreItemVendorID	=		siv.StoreItemVendorID
							AND vd2.InsertDate			=		(SELECT max(vd3.InsertDate) FROM VendorDealHistory vd3 WHERE vd3.StoreItemVendorId = siv.StoreItemVendorID and pbd.StartDate BETWEEN vd3.StartDate AND vd3.EndDate)
						GROUP BY 
							vd2.StoreItemVendorID,
							vt2.Code,
							 vt2.CaseAmtType
					) AS vdh
	WHERE 
		(
		PBH.PriceBatchStatusID	< 6
		OR 
		@PriceBatchStatusID IS NOT NULL
		)
		AND pbh.PriceBatchStatusID	=	ISNULL(@PriceBatchStatusID, pbh.PriceBatchStatusID)
		AND pbh.StartDate			>=	ISNULL(@StartDate, pbh.StartDate)
		AND pbh.StartDate			<=	ISNULL(@EndDate, pbh.StartDate)
		AND i.SubTeam_No			=	ISNULL(@SubTeam_No, i.SubTeam_No)       
	GROUP BY 
		pbh.BatchDescription, 
		ss.Store_Name,
		st.SubTeam_Name,
		ii.identifier,
		i.item_description, 
		i.package_desc1, 
		i.package_desc2, 
		iu.unit_name,
		v.CompanyName, 
		vch.UnitCost, 
		vch.package_desc1, 
		pbd.ItemChgTypeID,
		pct.PriceChgTypeDesc,
		pbd.posprice,
		pbd.possale_price,
		pbd.sale_end_date, 
		pbh.StartDate, 
		pbs.PriceBatchStatusDesc,
		isc.scale_description1, 
		isu.unit_name,
		isc.scale_fixedweight, 
		isc.scale_bycount, 
		sce.extratext, 
		ib.Brand_Name,
		vch.Package_Desc1,
		vdh.Discount,
		vdh.DiscoStart,
		vdh.DiscoEnd,
		vdh.Allowance,
		vdh.AllowStart,
		vdh.AllowEnd,
		vch.InsertDate,
		cpct.PriceChgTypeDesc,
		p.Price,
		p.POSSale_Price,
		p.Sale_Multiple, 
		p.Multiple,
		pbd.POSSale_Price,  
		pbd.Price, 
		pbd.Sale_Multiple, 
		pbd.Multiple,
		pbd.POSLinkCode


	SET NOCOUNT OFF
END

GO


SET QUOTED_IDENTIFIER OFF
GO
SET ANSI_NULLS OFF
GO