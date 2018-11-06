CREATE PROCEDURE [dbo].[Administration_UserAudit_UpdateUser]
	--User Info
	@AccountEnabled bit,
	@Telxon_Store_Limit int,
	@Title int,
	@User_Id int

AS
BEGIN
	
	DECLARE @titleChanged bit = 1

    SELECT @titleChanged = 0 
        FROM Users
        WHERE User_ID = @User_Id
        AND Title =  @Title

    UPDATE Users
        SET 
            [AccountEnabled] = @AccountEnabled,
            [Telxon_Store_Limit] = @Telxon_Store_Limit,
            [Title] = @Title
        WHERE  
            User_ID = @User_Id

    -- update any UserStoreTeamTitle values with the new title information.
    IF @AccountEnabled = 1
    BEGIN
        UPDATE UserStoreTeamTitle
            SET Title_ID = @Title
            WHERE User_ID = @User_ID
    END 

    -- If user title is changed, set the user permission to the title's default permission.
    IF @titleChanged = 1
    UPDATE u
        SET u.Accountant = d.Accountant,
            u.BatchBuildOnly = d.BatchBuildOnly,
            u.Buyer = d.Buyer,
            u.Coordinator = d.Coordinator,
            u.CostAdmin = d.CostAdministrator,
            u.FacilityCreditProcessor = d.FacilityCreditProcessor,
            u.DCAdmin = d.DCAdmin,
            u.DeletePO = d.DeletePO,
            u.EInvoicing_Administrator = d.EInvoicing,
            u.Inventory_Administrator = d.InventoryAdministrator,
            u.Item_Administrator = d.ItemAdministrator,
            u.Lock_Administrator = d.LockAdministrator,
            u.PO_Accountant = d.POAccountant,
            u.POApprovalAdmin = d.POApprovalAdministrator,
            u.POEditor = d.POEditor,
            u.PriceBatchProcessor = d.PriceBatchProcessor,
            u.Distributor = d.Distributor,
            u.Vendor_Administrator = d.VendorAdministrator,
            u.VendorCostDiscrepancyAdmin = d.VendorCostDiscrepancyAdmin,
            u.Warehouse = d.Warehouse,
            u.ApplicationConfigAdmin = d.ApplicationConfigAdmin,
            u.DataAdministrator = d.DataAdministrator,
            u.JobAdministrator = d.JobAdministrator,
            u.POSInterfaceAdministrator = d.POSInterfaceAdministrator,
            u.StoreAdministrator = d.StoreAdministrator,
            u.UserMaintenance = d.UserMaintenance,
            u.Shrink = d.Shrink,
            u.ShrinkAdmin = d.ShrinkAdmin,
            u.TaxAdministrator = d.TaxAdministrator
        FROM Users u
        JOIN TitleDefaultPermission d ON u.Title = d.TitleId 
        WHERE u.User_Id = @User_Id
END

GO
GRANT EXECUTE
    ON OBJECT::[dbo].[Administration_UserAudit_UpdateUser] TO [IRMAAdminRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[Administration_UserAudit_UpdateUser] TO [IRMAClientRole]
    AS [dbo];