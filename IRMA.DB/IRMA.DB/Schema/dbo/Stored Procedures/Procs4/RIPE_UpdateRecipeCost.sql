CREATE PROCEDURE [dbo].[RIPE_UpdateRecipeCost] 
	@Item_Key integer,
	@FacID int,
	@Package_Desc1 as decimal(9,4),
	@Vendor_ID as int,
	@cost smallmoney,
	@CostUnitID as int
	
AS

SET NOCOUNT ON

------------------------
-- Update the cost.
------------------------

----------------------------------------------------------------------------
--'*** Param List used by SP***
--'StoreList              'Supplied - desired FacilityID
--'StoreListSeparator     '|
--'Item_Key               'Supplied
--'Vendor_ID              'Supplied
--'UnitCost               'Supplied - This is the COST
--'UnitFreight            'NULL
--'Pack_desc1             'Supplied - from item
--'StartDate              'NULL
--'EndDate                'NULL
--'Promotional            '0
--'MSRP                   'NULL
--'FromVendor             '0
--'Source                 '3 - (Ripe UI)--Removed this param bug # 
--'@CostUnitID			  'Supplied
--'@FreightUnitID		  'Fetched from item table below
-----------------------------------------------------------------------------
		--Fetching 	Freight_Unit_ID value to pass to InsertVendorCostHistory  SP call
		DECLARE @FreightUnitID int

		SELECT DISTINCT 				
			@FreightUnitID=IUF.Unit_ID						 
		FROM 
			Item (nolock)
		INNER JOIN 
			ItemUnit IUF (NOLOCK)
			ON IUF.Unit_ID = Item.Freight_Unit_ID 	  			
		WHERE				
			 Item.Item_Key = @Item_Key


EXEC InsertVendorCostHistory @FacID, '|', @Item_Key, @Vendor_ID, @Cost, null, @Package_Desc1, null, null, 0, null, 0, @CostUnitID,@FreightUnitID
GO
GRANT EXECUTE
    ON OBJECT::[dbo].[RIPE_UpdateRecipeCost] TO [IRMAAdminRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[RIPE_UpdateRecipeCost] TO [IRMASupportRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[RIPE_UpdateRecipeCost] TO [IRMAClientRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[RIPE_UpdateRecipeCost] TO [IRMASchedJobsRole]
    AS [dbo];

