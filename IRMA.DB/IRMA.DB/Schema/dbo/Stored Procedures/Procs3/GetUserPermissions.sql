CREATE PROCEDURE dbo.GetUserPermissions 
	@UserName varchar(25) 
AS
SELECT
	User_ID,
	UserName,
	AccountEnabled,
	SuperUser,
	PO_Accountant,
	Accountant,
	Distributor,
	FacilityCreditProcessor,
	Buyer,
	Coordinator,
	Item_Administrator,
	Vendor_Administrator,
	Lock_Administrator,
	Telxon_Store_Limit,
	Warehouse,
	PriceBatchProcessor,
	Inventory_Administrator,
	DCAdmin,
	CostAdmin,
	Shrink,
	ShrinkAdmin,
	ApplicationConfigAdmin,
	DataAdministrator,
	JobAdministrator,
	POSInterfaceAdministrator,
	SecurityAdministrator,
	StoreAdministrator,
	SystemConfigurationAdministrator,
	UserMaintenance,
	POApprovalAdmin,
	EInvoicing_Administrator,
	VendorCostDiscrepancyAdmin
FROM 
	USERS
WHERE USERNAME = @UserName
GO
GRANT EXECUTE
    ON OBJECT::[dbo].[GetUserPermissions] TO [IRMAAdminRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[GetUserPermissions] TO [IRMAClientRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[GetUserPermissions] TO [IRMASchedJobsRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[GetUserPermissions] TO [IRMAReportsRole]
    AS [dbo];

