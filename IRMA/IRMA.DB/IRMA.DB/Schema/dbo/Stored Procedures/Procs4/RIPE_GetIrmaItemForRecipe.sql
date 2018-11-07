CREATE PROCEDURE [dbo].[RIPE_GetIrmaItemForRecipe]
   		@Item_Key as Integer,	
		@FacID as int
AS

DECLARE	@RegionalFacID as int,
		@RegionalVendorID as int,
		@FacVendorID as int		  
		  
SET @RegionalFacID = dbo.fn_GetRegFacID()

SELECT @RegionalVendorID = Vendor_ID FROM Vendor WHERE Store_No = @RegionalFacID

SELECT @FacVendorID = Vendor_ID FROM Vendor WHERE Store_No = @FacID

----------------------------------------------------------------------------------------------------------------------------------------
-- Get the Sub Team Types.
----------------------------------------------------------------------------------------------------------------------------------------

   DECLARE @eSubTeam table(IRMASubTeamType int, eSubTeamType int, EnumName varchar(50))
   INSERT INTO @eSubTeam
   SELECT * FROM fn_GetEnumSubTeamTypes() ETS

------------------------
-- Start main query
------------------------
----------------------------------------------------------------------------------------------------------------------------------------
-- IF Facility then return IRMA Item Detail and Check availability of the Item to that Facility
-- MD 10/16/2008: Incorporated the instoremanufacturedproducts logic in the same select statement 
-- and removed hard coded vendor references
----------------------------------------------------------------------------------------------------------------------------------------
		SELECT DISTINCT 
				Item.Item_Key,
				Item.Item_Description, 
    		    IB.Brand_Name,                  
				ItemIdentifier.Identifier,
				/*BS-080805-Replacing ItemVendor.Item_ID*/
				/*IV.Item_ID AS PLU,*/
				SubTeam.SubTeam_No,
				SubTeam.SubTeam_Name AS SubTeamName,
				Item.CostedByWeight,
				Item.Package_Desc1 AS PackDesc1,
				Item.Package_Desc2 AS PackDesc2,
				IU.Unit_Abbreviation AS PackUnit,
				IU.Unit_ID as PackUnitID,
				IUC.Unit_Abbreviation AS CostUnit,
				IUC.Unit_ID as CostUnitID,
				VC.UnitCost AS CurrentCost,
				IV.Vendor_ID,
				CASE WHEN SIV.StoreItemVendorID IS NULL THEN 1 ELSE 0 END As NotAVendor		 
			FROM 
				Item (nolock)
			LEFT JOIN 
				ItemVendor IV (NOLOCK)
				ON IV.Item_Key = Item.Item_Key
			LEFT JOIN 
				StoreItemVendor SIV 
				on IV.item_key = SIV.item_key and IV.vendor_ID = SIV.vendor_ID 
				and SIV.Store_No = @FacID
			OUTER APPLY
				dbo.fn_VendorCost(Item.Item_Key, IV.Vendor_ID, SIV.Store_No, GetDate()) AS VC
			INNER JOIN 
				ItemUnit IU (NOLOCK)
				ON IU.Unit_ID = Item.Package_Unit_ID
			INNER JOIN 
				ItemUnit IUC (NOLOCK)
				ON IUC.Unit_ID = Item.Cost_Unit_ID 		 			
			INNER JOIN
				ItemIdentifier (nolock)
				on ItemIdentifier.Item_Key = Item.Item_key
				and ItemIdentifier.Deleted_Identifier = 0 
				and ItemIdentifier.Default_Identifier = 1 
			LEFT JOIN
				ItemBrand IB
				ON IB.Brand_Id = Item.Brand_Id
			INNER JOIN 
				SubTeam (nolock)
				on SubTeam.SubTeam_No = Item.SubTeam_No   
			INNER JOIN 
			   @eSubTeam ETS
				ON ETS.IRMASubTeamType = SubTeam.SubTeamType_ID	
			LEFT JOIN
				Vendor (NOLOCK)	
				ON SIV.Vendor_ID = Vendor.Vendor_ID
			WHERE
				(IV.DeleteDate >= getdate() or IV.DeleteDate is null)
				AND Item.Item_Key = @Item_Key
				AND (IV.Vendor_ID = @FacVendorID OR Vendor.InStoreManufacturedProducts = 1)							
		
RETURN
GO
GRANT EXECUTE
    ON OBJECT::[dbo].[RIPE_GetIrmaItemForRecipe] TO [IRMAAdminRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[RIPE_GetIrmaItemForRecipe] TO [IRMASupportRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[RIPE_GetIrmaItemForRecipe] TO [IRMAClientRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[RIPE_GetIrmaItemForRecipe] TO [IRMASchedJobsRole]
    AS [dbo];

