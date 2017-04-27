if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[GetUserPermissions]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[GetUserPermissions]
GO

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
SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO




