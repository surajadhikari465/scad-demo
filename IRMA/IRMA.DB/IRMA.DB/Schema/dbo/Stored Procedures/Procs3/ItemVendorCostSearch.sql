CREATE PROCEDURE [dbo].[ItemVendorCostSearch]
	@Item_Key	int,
	@Vendor_ID	int,
	@Zone_ID	int,
	@Store_No	int,
	@WFM_Store	bit,
	@Mega_Store bit,
	@State		varchar(2),
	@StartDate	datetime,
	@EndDate	datetime

AS

-- ****************************************************************************************************************
-- Procedure: ItemVendorCostSearch()
--    Author: unknown
--      Date: unknown
--
-- Description:
-- Called from VendorCost.vb.
--
-- Modification History:
-- Date       	Init  	TFS   	Comment
-- 2013-01-30	KM		9251	Add update history template; Change Currency join to LEFT;
-- ****************************************************************************************************************

BEGIN
	SET NOCOUNT ON

	DECLARE @tmp_ItemVendorCostCurrent TABLE 
	(
		VendorCostHistoryID		int, 
		Store_No				int, 
		Store_Name				varchar(50),
		Type					char(1),
		UnitCost				smallmoney, 
		UnitFreight				smallmoney, 
		StartDate				smalldatetime, 
		EndDate					smalldatetime,
		PrimaryVendor			bit, 
		fromVendor				bit, 
		Package_Desc1			decimal(10,4), 
		NetDiscount				decimal(10,4), 
		NetCost					decimal(10,4), 
		VendorPack				decimal(10,4),
		CostUnit_Name			varchar(50), 
		FreightUnit_Name		varchar(50), 
		IgnoreCasePack			bit, 
		CurrencyCode			char(3), 
		InsertDate				smalldateTime 
	)
    
	DECLARE @tmp_ItemVendorCostSearch TABLE 
	(
		VendorCostHistoryID		int, 
		Store_No				int, 
		Store_Name				varchar(50),
		Type					char(1),
		UnitCost				smallmoney, 
		UnitFreight				smallmoney, 
		StartDate				smalldatetime, 
		EndDate					smalldatetime,
		PrimaryVendor			bit, 
		fromVendor				bit, 
		Package_Desc1			decimal(10,4), 
		NetDiscount				decimal(10,4), 
		NetCost					decimal(10,4), 
		VendorPack				decimal(10,4),
		CostUnit_Name			varchar(50), 
		FreightUnit_Name		varchar(50), 
		IgnoreCasePack			bit, 
		CurrencyCode			char(3), 
		InsertDate				smalldateTime 
	)
 
	INSERT INTO @tmp_ItemVendorCostSearch
	SELECT 
		VendorCostHistoryID,
		SIV.Store_No,
		Store_Name, 
		CASE WHEN Promotional = 1 THEN 'P' ELSE 'R' END As Type,
		UnitCost, --REG COST
		ISNULL(UnitFreight, 0) As UnitFreight,
		StartDate,
		EndDate,
		PrimaryVendor,
		FromVendor,
		Package_Desc1 = case when isnull(IV.IgnoreCasePack,0) = 1 then IV.RetailCasePack else VCH.Package_Desc1 end,            
		--NET DISCOUNT AMT
		ISNULL(dbo.fn_ItemNetDiscount(SIV.Store_No, @Item_Key, @Vendor_ID, ISNULL(UnitCost, 0), VCH.StartDate),0) AS NetDiscount,
		--NET COST = REG COST - NET DISCOUNT + FREIGHT
		ISNULL(UnitCost, 0) - ISNULL(dbo.fn_ItemNetDiscount(SIV.Store_No, @Item_Key, @Vendor_ID, ISNULL(UnitCost, 0), VCH.StartDate), 0) + ISNULL(UnitFreight, 0) AS NetCost,
		VCH.Package_Desc1 as VendorPack,
		CostItemUnit.Unit_Name AS CostUnit_Name,
		FreightItemUnit.Unit_Name AS FreightUnit_Name,
		IV.IgnoreCasePack,
		C.CurrencyCode,
		VCH.InsertDate
	
	FROM
		VendorCostHistory VCH						(nolock)
		INNER JOIN	StoreItemVendor SIV				(nolock)	ON	SIV.StoreItemVendorID	= VCH.StoreItemVendorID
																AND SIV.Item_Key			= @Item_Key
																AND SIV.Vendor_ID			= @Vendor_ID
																AND SIV.Store_No			= ISNULL(@Store_No, SIV.Store_No)
		INNER JOIN	Store							(nolock)	ON	Store.Store_No			= SIV.Store_No
																AND Store.Zone_ID			= ISNULL(@Zone_ID, Store.Zone_ID)
		INNER JOIN	Vendor							(nolock)	ON	Vendor.Store_No			= SIV.Store_No
		LEFT JOIN	Currency C						(nolock)	ON	C.CurrencyID			= Vendor.CurrencyID
		LEFT JOIN	ItemUnit AS CostItemUnit		(nolock)	ON	CostItemUnit.Unit_ID	= VCH.CostUnit_ID
		LEFT JOIN	ItemUnit AS FreightItemUnit		(nolock)	ON	FreightItemUnit.Unit_ID = VCH.FreightUnit_ID
		INNER JOIN 	ItemVendor IV					(NoLock)	ON	SIV.Vendor_ID			= IV.Vendor_ID
																AND SIV.Item_Key			= IV.Item_Key	
	WHERE
		WFM_Store = ISNULL(@WFM_Store, WFM_Store)
		AND Mega_Store = ISNULL(@Mega_Store, Mega_Store)
		AND ISNULL(State, '') = ISNULL(@State, ISNULL(State, ''))
		AND StartDate >= ISNULL(@StartDate, StartDate) 
		AND EndDate >= ISNULL(@EndDate, EndDate)
		AND ((SIV.DeleteDate IS NULL) OR (ISNULL(@EndDate, convert(datetime, convert(varchar(255), GETDATE()))) < SIV.DeleteDate))
	
	ORDER BY
		Store_Name, StartDate, EndDate
		
						
	DECLARE @StoreNo int, @VendorCostHistoryID int, @NetDiscount decimal(10,4), @NetCost decimal(10,4) 
                             
    DECLARE StoreNo_Cursor CURSOR FOR
	SELECT Store_No FROM @tmp_ItemVendorCostSearch GROUP BY Store_No

	OPEN StoreNo_Cursor 
	FETCH NEXT FROM StoreNo_Cursor INTO @StoreNo

	WHILE @@FETCH_STATUS = 0

		BEGIN
			SELECT TOP 1 @VendorCostHistoryID = VendorCostHistoryID 
			FROM @tmp_ItemVendorCostSearch 
			WHERE Store_No = @StoreNo 
			ORDER BY StartDate DESC
		
			INSERT INTO  @tmp_ItemVendorCostCurrent
			EXECUTE [dbo].[ItemVendorCostCurrent] @Item_Key, @Vendor_ID, @Zone_ID, @StoreNo, @WFM_Store, @Mega_Store, @State

			SELECT @NetDiscount = NetDiscount, @NetCost = NetCost FROM @tmp_ItemVendorCostCurrent
		  
			UPDATE @tmp_ItemVendorCostSearch SET NetDiscount = @NetDiscount, NetCost = @NetCost 
			WHERE VendorCostHistoryID = @VendorCostHistoryID
		  
			DELETE FROM @tmp_ItemVendorCostCurrent
		
			FETCH NEXT FROM StoreNo_Cursor into @StoreNo
		END 

	CLOSE StoreNo_Cursor
	DEALLOCATE StoreNo_Cursor
              
	SELECT 
		VendorCostHistoryID, 
		Store_No, 
		Store_Name, 
		Type, 
		UnitCost, 
		UnitFreight, 
		StartDate, 
		EndDate, 
		PrimaryVendor, 
		fromVendor, 
		Package_Desc1, 
		NetDiscount, 
		NetCost, 
		VendorPack, 
		CostUnit_Name, 
		FreightUnit_Name, 
		IgnoreCasePack, 
		CurrencyCode, 
		InsertDate  
	
	FROM 
		@tmp_ItemVendorCostSearch
			
	SET NOCOUNT OFF
END
GO
GRANT EXECUTE
    ON OBJECT::[dbo].[ItemVendorCostSearch] TO [IRMASupportRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[ItemVendorCostSearch] TO [IRMAClientRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[ItemVendorCostSearch] TO [IRMASchedJobsRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[ItemVendorCostSearch] TO [IRMAReportsRole]
    AS [dbo];

