SET QUOTED_IDENTIFIER ON 
GO
SET ANSI_NULLS ON 
GO

if exists (select * from dbo.sysobjects where id = object_id(N'dbo.ItemVendorCostCurrent') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure dbo.ItemVendorCostCurrent
GO

CREATE PROCEDURE [dbo].[ItemVendorCostCurrent]
	@Item_Key	int,
	@Vendor_ID	int,
	@Zone_ID	int,
	@Store_No	int,
	@WFM_Store	bit,
	@Mega_Store	bit,
	@State		varchar(2)

AS

-- ****************************************************************************************************************
-- Procedure: ItemVendorCostCurrent()
--    Author: unknown
--      Date: unknown
--
-- Description:
-- Called from VendorCost.vb.
--
-- Modification History:
-- Date       	Init  	TFS   	Comment
-- 2013-01-30	KM				Add update history template; Change Currency join to LEFT;
-- ****************************************************************************************************************

BEGIN
	SET NOCOUNT ON

	DECLARE @CurrDate datetime

	SELECT @CurrDate = CONVERT(datetime, CONVERT(varchar(255), GETDATE(), 101))

	SELECT 
		VC.VendorCostHistoryID,
		SIV.Store_No,
		Store_Name, 
		CASE WHEN VC.Promotional = 1 THEN 'P' ELSE 'R' END As Type,
		VC.UnitCost as UnitCost, --REG COST
		ISNULL(VC.UnitFreight, 0) As UnitFreight,
		VC.StartDate,
		VC.EndDate,
		PrimaryVendor,
		VC.FromVendor,
		Package_Desc1 = case when isnull(IV.IgnoreCasePack,0) = 1 then IV.RetailCasePack else VC.Package_Desc1 end,
		NetDiscount,
		NetCost,
		VC.Package_Desc1 as VendorPack,
		CostItemUnit.Unit_Name AS CostUnit_Name,
		FreightItemUnit.Unit_Name AS FreightUnit_Name,
		IV.IgnoreCasePack,
		C.CurrencyCode,
		VC.InsertDate
	
	FROM
		StoreItemVendor SIV						(nolock)
		INNER JOIN	Store						(nolock)	ON	SIV.Item_Key				= @Item_Key
															AND SIV.Vendor_ID				= @Vendor_ID
															AND SIV.Store_No				= ISNULL(@Store_No, SIV.Store_No)
															AND Store.Store_No				= SIV.Store_No
															AND Store.Zone_ID				= ISNULL(@Zone_ID, Store.Zone_ID)
		INNER JOIN	Vendor						(nolock)	ON	Vendor.Store_No				= SIV.Store_No
		LEFT JOIN	Currency C					(nolock)	ON	C.CurrencyID				= Vendor.CurrencyID
		INNER JOIN	fn_VendorCostStores(@Item_Key, @Vendor_ID, @CurrDate) VC
															ON	Store.Store_No				= VC.Store_No
		LEFT JOIN	ItemUnit AS CostItemUnit	(nolock)	ON	CostItemUnit.Unit_ID		= VC.CostUnit_ID
		LEFT JOIN	ItemUnit AS FreightItemUnit (nolock)	ON	FreightItemUnit.Unit_ID		= VC.FreightUnit_ID
		INNER JOIN 	ItemVendor IV				(NoLock)	ON	SIV.Vendor_ID				= IV.Vendor_ID
															AND SIV.Item_Key				= IV.Item_Key
	WHERE
		WFM_Store = ISNULL(@WFM_Store, WFM_Store)
		AND Mega_Store = ISNULL(@Mega_Store, Mega_Store)
		AND ISNULL(State, '') = ISNULL(@State, ISNULL(State, ''))
		AND (SIV.DeleteDate IS NULL OR @CurrDate < SIV.DeleteDate)
	
	ORDER BY
		Store_Name, StartDate, EndDate
	
	SET NOCOUNT OFF
END
GO
SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO