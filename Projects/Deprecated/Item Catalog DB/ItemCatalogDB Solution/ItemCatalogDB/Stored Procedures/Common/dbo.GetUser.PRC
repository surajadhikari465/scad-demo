SET QUOTED_IDENTIFIER ON 
GO
SET ANSI_NULLS ON 
GO

if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].GetUser') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].GetUser
GO

-- 08.25.08 V3.2 Robert Shurbet
-- Added new SlimAccess join for SLIM access attributes.

CREATE PROCEDURE dbo.GetUser
    @User_ID int
AS

BEGIN
    SET NOCOUNT ON

    SELECT Users.User_ID,
        Accountant,
        AccountEnabled, 
        Buyer,
        Coordinator,
        FacilityCreditProcessor,
        Distributor,
        DeletePO,
        ISNULL(FullName, '') As FullName,
        Inventory_Administrator,
        Item_Administrator,
        Lock_Administrator,
        PO_Accountant, 
        PriceBatchProcessor,
        EInvoicing_Administrator,
        ISNULL(RecvLog_Store_Limit, 0) As RecvLog_Store_Limit,
        SuperUser,
        ISNULL(Telxon_Store_Limit, 0) As Telxon_Store_Limit,
        Vendor_Administrator,
        ISNULL((SELECT MAX(Vendor_ID) FROM Vendor WHERE Store_No = Telxon_Store_Limit), 0) AS Vendor_Limit,
        UserName,
		Printer,
		CoverPage,
		Email,
		Pager_Email,
		Fax_Number,
		Phone_Number,
		ISNULL(Title, -1) As Title,
		Warehouse,
		BatchBuildOnly,
		DCAdmin,
		PromoAccessLevel,
		CostAdmin,
		VendorCostDiscrepancyAdmin,
		POApprovalAdmin,
		POEditor,
		TaxAdministrator,
-- ADMIN ROLES
		ApplicationConfigAdmin,
		SystemConfigurationAdministrator,
		DataAdministrator,
		JobAdministrator,
		POSInterfaceAdministrator,
		StoreAdministrator,
		SecurityAdministrator,
		UserMaintenance,	
		Shrink,
		ShrinkAdmin,	
-- SLIM ACCESS ATTRIBUTES
		ISNULL(SlimAccess.UserAdmin, 0) As UserAdmin,
		ISNULL(SlimAccess.ItemRequest, 0) As ItemRequest,
		ISNULL(SlimAccess.VendorRequest, 0) As VendorRequest,
		ISNULL(SlimAccess.IRMAPush, 0) As IRMAPush,
		ISNULL(SlimAccess.StoreSpecials, 0) As StoreSpecials,
		ISNULL(SlimAccess.RetailCost, 0) As RetailCost,
		ISNULL(SlimAccess.Authorizations, 0) As Authorizations,
		ISNULL(SlimAccess.WebQuery, 0) As WebQuery,
		ISNULL(SlimAccess.ScaleInfo, 0) As ScaleInfo,
		ISNULL(SlimAccess.ECommerce, 0) As ECommerce

    FROM Users
		LEFT JOIN SlimAccess ON Users.User_ID = SlimAccess.User_ID
    WHERE Users.User_ID = @User_ID

    SET NOCOUNT OFF
END

GO
SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO