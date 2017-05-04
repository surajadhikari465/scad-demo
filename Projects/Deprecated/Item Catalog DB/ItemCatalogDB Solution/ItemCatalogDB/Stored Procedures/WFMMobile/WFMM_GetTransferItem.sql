SET QUOTED_IDENTIFIER ON 
GO
SET ANSI_NULLS ON 
GO

IF EXISTS (SELECT * FROM sysobjects WHERE type = 'P' AND name = 'WFMM_GetTransferItem')
	BEGIN
		DROP Procedure [dbo].WFMM_GetTransferItem
	END
GO

CREATE PROCEDURE [dbo].[WFMM_GetTransferItem] 
	@Item_Key			int,
    @Identifier			varchar(255),
    @ProductType_ID		int,
    @VendStore_No		int,
    @Vendor_ID			int = NULL,
    @Transfer_SubTeam	int,
    @SupplySubTeam_No	int = NULL

AS


-- **************************************************************************  
-- Procedure: WFMM_GetTransferItem()  
--  
-- Description:  
-- This procedure is called to return item data to an transfer order interface on IRMA handheld  
--  
-- Modification History:  
-- Date			Init	TFS		Comment  
-- 10.01.12		MZ		7374	return item data to an transfer order interface on IRMA handheld.
-- 12.17.12		FA      9535    updated code to pull retail unit name from alt. jurisdiction	
-- 12.17.12		MZ      9470	Modified the logic to set the @cost on a transfer PO to the paid unit cost on the last received PO by the store/subteam with the item on it.
-- 01.04.13 	KM		9251	Select ru.Unit_ID instead of i.Retail_Unit_ID to properly pick up any override Retail Units;
-- 05.23.13     MZ		9684    Modified the Vendor checking code in the end to find an authorized vendor. Since currently an item can be transferred between any stores so
--                              long as the vendor/item is actively authorized to one store, this piece of code may need to be modified again for enhancement.
-- 01.30.13		FA		2552	Modified the code to use hosted cost if there is no last paid cost.
-- 02.10.13		FA		2552	Calculate the correct unit cost
-- **************************************************************************  

BEGIN

    SET NOCOUNT ON

	IF @SupplySubTeam_No = 0
		SET @SupplySubTeam_No = NULL
	
	DECLARE @SubTeamList TABLE (SubTeam_No int)
	IF @SupplySubTeam_No IS NULL 
		BEGIN
			IF @ProductType_ID = 1 
				INSERT @SubTeamList				
					SELECT SubTeam_No FROM SubTeam (nolock) WHERE SubTeamType_ID IN (1,2,3,4) 
			IF @ProductType_ID = 2 
				INSERT @SubTeamList
					SELECT SubTeam_No FROM SubTeam (nolock) WHERE SubTeamType_ID = 5 
			IF @ProductType_ID = 3 
				INSERT @SubTeamList
					SELECT SubTeam_No FROM SubTeam (nolock) WHERE SubTeamType_ID = 6 
		END 
		
	DECLARE	
		@UnitID				int,
		@PoundID			int,
		@UseAvgCost			bit,	
		@SourcePONumber     int,
		@IsCostedByWeight	bit,
		@Cost				money,
		@Cost_Unit_ID		int,
		@Retail_Unit_ID     int,
		@Package_Desc1	    decimal(9,4),
		@Package_Desc2		decimal(9,4),
		@Package_Unit_ID	int
	
	IF @Vendor_ID IS NULL OR @Vendor_ID = 0
		SELECT @Vendor_ID  = ISNULL(Vendor_ID,0) FROM Vendor WHERE Store_no = @VendStore_No
		
	SELECT @UseAvgCost = ISNULL(UseAvgCostHistory,0) FROM Store WHERE Store_No = @VendStore_No
	
	--**************************************************************************
	--Populate variables
	--**************************************************************************
	SELECT @UnitID = Unit_ID FROM ItemUnit (nolock) WHERE lower(UnitSysCode) = 'unit'      
	SELECT @PoundID = Unit_ID FROM ItemUnit (nolock) WHERE lower(UnitSysCode) = 'lbs' 
			
    IF @item_key IS NULL OR @item_key = 0
        SET @item_key =	(SELECT TOP 1
							Item_Key
						FROM   
							ItemIdentifier
						WHERE  
							Deleted_Identifier		= 0
                            AND Remove_Identifier	= 0
                            AND Identifier			= @Identifier)  
	
	IF @UseAvgCost = 1
		BEGIN
			SET @SourcePONumber = ISNULL(dbo.fn_GetLastItemPOID(@Item_Key, @Vendor_ID), 0)
			IF @SourcePONumber > 0
				SELECT 
					@Package_Desc1		=	Package_Desc1,
					@Package_Desc2		=	Package_Desc2,
					@Cost_Unit_ID		=	CostUnit,
					@Package_Unit_ID	=	Package_Unit_ID,
					@Retail_Unit_ID     =	Retail_Unit_ID
				FROM 
					OrderItem
				WHERE 
					OrderHeader_ID		=	@SourcePONumber
					AND	Item_Key		=	@Item_Key
			
			SELECT @Cost = dbo.fn_AvgCostHistory(@Item_Key, @VendStore_No, @Transfer_SubTeam, GETDATE())  
			SELECT @Cost = dbo.fn_CostConversion(@Cost, CASE WHEN @IsCostedByWeight = 1 THEN @PoundID ELSE @UnitID END, @Retail_Unit_ID, @Package_Desc1, @Package_Desc2, @Package_Unit_ID)
		END
	ELSE
		BEGIN
			SET @SourcePONumber = ISNULL(dbo.fn_GetLastItemPOID(@Item_Key, @Vendor_ID), 0)
			IF @SourcePONumber > 0																	
				SELECT 
					-- The @Cost is the LineItem's paid cost which was copied from the calculation of [PaidLineItemCost] in GetOrderItemInfo SP
					@Cost =	CASE
						--•	For a pay by agreed cost vendor when the LineItem is approved, display the amount the PO Admin selects to pay via the SPOT. Either line item received cost or line item invoice cost.
						WHEN poc.PayOrderedCostID IS NOT NULL AND oh.CloseDate IS NOT NULL AND oi.ApprovedDate IS NOT NULL AND oi.ApprovedByUserId IS NOT NULL THEN
							oi.PaidCost / oi.QuantityReceived
						--•	For a pay by invoice, eInvoiced order, and received quantity is greater than 0, then display the line item invoice cost. 
						WHEN poc.PayOrderedCostID IS NULL AND oh.eInvoice_ID IS NOT NULL AND oh.CloseDate IS NOT NULL  AND oi.QuantityReceived > 0 THEN
							 oi.InvoiceExtendedCost / oi.QuantityReceived
						--•	For a pay by invoice, paper invoiced order, it will be the line item received cost.
						WHEN poc.PayOrderedCostID IS NULL AND oh.eInvoice_ID IS NULL AND oh.CloseDate IS NOT NULL THEN
							oi.ReceivedItemCost / oi.QuantityReceived
						--•	For a pay by agreed cost vendor on which the ordered and eInvoiced costs match and the LineItem did not suspend, display the line item received cost.
						WHEN poc.PayOrderedCostID IS NOT NULL AND oh.CloseDate IS NOT NULL AND oi.ApprovedDate IS NULL AND oi.ApprovedByUserID IS NULL THEN
							oi.ReceivedItemCost	/ oi.QuantityReceived		
						--• For a pay by agreed cost vendor LineItem that suspended, this field will display the line item received cost while in suspended status.
						WHEN poc.PayOrderedCostID IS NOT NULL AND oh.CloseDate IS NOT NULL AND oi.ApprovedDate IS NULL AND oi.LineItemSuspended = 1 THEN
							oi.ReceivedItemCost	/ oi.QuantityReceived											
						ELSE
							0
						END,
					@Package_Desc1		=	Package_Desc1,
					@Package_Desc2		=	Package_Desc2,
					@Cost_Unit_ID		=	CostUnit,
					@Package_Unit_ID	=	Package_Unit_ID
				
				FROM 
				OrderItem						(nolock) oi
				INNER JOIN	OrderHeader			(nolock) oh		ON	oi.OrderHeader_ID		=	oh.OrderHeader_ID
				INNER JOIN	Vendor				(nolock) v		ON	oh.Vendor_ID			=	v.Vendor_ID
				INNER JOIN	Vendor				(nolock) vr		ON	oh.ReceiveLocation_ID	=	vr.Vendor_ID
				INNER JOIN	Store				(nolock) sr		ON	vr.Store_No				=	sr.Store_No
				LEFT JOIN	PayOrderedCost		(nolock) poc	ON	oh.Vendor_ID			=	poc.Vendor_ID
				   												AND	sr.Store_No				=	poc.Store_No
																AND poc.BeginDate			<=	oh.SentDate
				WHERE 
					oi.OrderHeader_ID	= @SourcePONumber
					AND	oi.Item_Key		= @Item_Key
			
			-- now convert cost to Unit UOM
			SELECT @Cost = dbo.fn_CostConversion(@Cost, @Cost_Unit_ID, CASE WHEN @IsCostedByWeight = 1 THEN @PoundID ELSE @UnitID END, @Package_Desc1, @Package_Desc2, @Package_Unit_ID)
		END
    
    -- If @Cost is 0 then use hosted cost by calling fn_GetCurrentNetCost
    IF ISNULL(@Cost, 0) = 0 
    BEGIN
		DECLARE @vendorpacksize	decimal(9,4)
		DECLARE @casepackcost	money
		
		-- calculate vendor case pack size
		SELECT @vendorpacksize = isnull(dbo.fn_GetCurrentVendorPackage_Desc1(@Item_Key, @VendStore_No), 0)	
		IF @vendorpacksize = 0 SELECT @vendorpacksize = 1	
		
		-- calculate case pack cost
		SELECT @casepackcost = isnull(dbo.fn_GetCurrentNetCost(@Item_Key, @VendStore_No), 0)
		
		-- calculate unit cost
		SELECT @Cost = @casepackcost / @vendorpacksize
    END
    
    SELECT    
		I.Item_Key ,
        II.Identifier ,
        ISNULL(IOR.Item_Description, I.Item_Description) AS Item_Description ,
        @Vendor_ID AS Vendor_ID,
        ISNULL(@Cost,0) AS Vendor_Cost,
        RTRIM(CAST(CAST(ISNULL(ISNULL(@Package_Desc1, IOR.Package_Desc1), I.Package_Desc1) AS INT) AS VARCHAR(14))) + ' / ' + RTRIM(CAST(CAST(ISNULL(ISNULL(@Package_Desc2, IOR.Package_Desc2),I.Package_Desc2) AS INT) AS VARCHAR(14))) + ' ' + RTRIM(PU.Unit_Name) AS VendorPack,
        ISNULL(RU.Weight_Unit, 0) AS Sold_By_Weight ,
        RTRIM(VU.Unit_Name) AS Vendor_Unit_Name ,
        RTRIM(RU.Unit_Name) AS Retail_Unit_Name ,
		ru.Unit_ID AS Retail_Unit_ID,
		CASE @ProductType_ID
			WHEN 1 THEN 0
			ELSE I.SubTeam_No
		END AS RetailSubTeam_No, 
		ST.SubTeam_Name as RetailSubTeam_Name,
		CASE @ProductType_ID
			WHEN 1 THEN 0
			WHEN 2 THEN ST.GLPackagingAcct
			WHEN 3 THEN ST.GLSuppliesAcct
		END AS GLAcct
	
	FROM		
		Item I							( NOLOCK )
		INNER JOIN	ItemIdentifier II	( NOLOCK ) ON II.Item_Key = I.Item_Key
		INNER JOIN	ItemVendor IV		( NOLOCK ) ON I.Item_Key = IV.Item_Key
		INNER JOIN	SubTeam ST			( NOLOCK ) ON I.SubTeam_No = ST.SubTeam_No
		INNER JOIN	Store S				( NOLOCK ) ON S.Store_No = @VendStore_No
		LEFT JOIN	ItemOverride IOR	( NOLOCK ) ON I.Item_Key = IOR.Item_Key AND ior.StoreJurisdictionID = S.StoreJurisdictionID
		LEFT JOIN	ItemUnit PU			( NOLOCK ) ON ISNULL(ISNULL(@Package_Unit_ID, IOR.Package_Unit_ID), I.Package_Unit_ID) = PU.Unit_ID
		LEFT JOIN	ItemUnit RU			( NOLOCK ) ON ISNULL(IOR.Retail_Unit_ID, I.Retail_Unit_ID) = RU.Unit_ID
		LEFT JOIN	ItemUnit VU			( NOLOCK ) ON ISNULL(IOR.Vendor_Unit_ID, I.Vendor_Unit_ID) = VU.Unit_ID
    
	WHERE		
		II.identifier = @identifier
        AND ((NOT EXISTS (SELECT 1 FROM @SubTeamList)) OR (I.SubTeam_No in (SELECT SubTeam_No FROM @SubTeamList)))
        AND I.SubTeam_No = ISNULL(@SupplySubTeam_No, I.SubTeam_No)
        AND II.Deleted_Identifier = 0 
        AND II.Remove_Identifier = 0
        AND I.Deleted_Item = 0
        AND I.Remove_Item = 0
        AND IV.DeleteDate IS NULL 
        AND IV.Vendor_ID =	(select top 1 siv.Vendor_ID 
							from StoreItem si (nolock) 
							join storeitemvendor siv (nolock) on si.Store_No = siv.Store_No 
							 and si.Item_Key = siv.Item_Key and si.Authorized = 1 
							join ItemVendor iv2 (nolock) on si.Item_key = iv2.Item_key
							 and iv2.Vendor_ID = siv.Vendor_ID
						   where si.Item_Key = I.Item_Key
						     and siv.DeleteDate is null 
						     and iv2.DeleteDate is null
						Order by siv.PrimaryVendor desc)

    SET NOCOUNT OFF
END
GO
