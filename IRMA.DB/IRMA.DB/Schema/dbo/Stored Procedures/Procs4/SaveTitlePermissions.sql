CREATE PROCEDURE dbo.SaveTitlePermissions
		@TitleId int,
		@Accountant bit,
		@BatchBuildOnly bit,
		@Buyer bit,
		@Coordinator bit,
		@CostAdministrator bit,
		@FacilityCreditProcessor bit,
		@DCAdmin bit,
		@DeletePO bit,
		@EInvoicing bit,
		@InventoryAdministrator bit,
		@ItemAdministrator bit,
		@LockAdministrator bit,
		@POAccountant bit,
		@POApprovalAdministrator bit,
		@POEditor bit,
		@PriceBatchProcessor bit,
		@Distributor bit,
		@VendorAdministrator bit,		
		@VendorCostDiscrepancyAdmin bit,
		@Warehouse bit,
		@CancelAllSales Bit = NULL,
		@ApplicationConfigAdmin bit,
		@DataAdministrator bit,
		@JobAdministrator bit,
		@POSInterfaceAdministrator bit,
		@StoreAdministrator bit,
		@UserMaintenance bit,
		@Shrink bit,
		@ShrinkAdmin bit,
		@TaxAdministrator bit
AS 

	DECLARE @tblRoleValues TABLE (TitleId int, RoleName varchar(50), RoleValue bit)

	IF EXISTS (SELECT * FROM TitleDefaultPermission WHERE TitleId = @TitleId)
		BEGIN
			UPDATE 
				TitleDefaultPermission 
			SET 
				Accountant = @Accountant,
				BatchBuildOnly = @BatchBuildOnly,
				Buyer = @Buyer,
				Coordinator = @Coordinator,
				CostAdministrator = @CostAdministrator,
				FacilityCreditProcessor = @FacilityCreditProcessor,
				DCAdmin = @DCAdmin,
				DeletePO = @DeletePO,
				EInvoicing = @EInvoicing,
				InventoryAdministrator = @InventoryAdministrator,
				ItemAdministrator = @ItemAdministrator,
				LockAdministrator = @LockAdministrator,
				POAccountant = @POAccountant,
				POApprovalAdministrator = @POApprovalAdministrator,
				POEditor = @POEditor,
				PriceBatchProcessor = @PriceBatchProcessor,
				Distributor = @Distributor,
				VendorAdministrator = @VendorAdministrator,
				VendorCostDiscrepancyAdmin = @VendorCostDiscrepancyAdmin,
				Warehouse = @Warehouse,
				CancelAllSales = ISNULL(@CancelAllSales, CancelAllSales),
				ApplicationConfigAdmin = @ApplicationConfigAdmin,
				DataAdministrator = @DataAdministrator,
				JobAdministrator = @JobAdministrator,
				POSInterfaceAdministrator = @POSInterfaceAdministrator,
				StoreAdministrator = @StoreAdministrator,
				UserMaintenance = @UserMaintenance,
				Shrink = @Shrink,
				ShrinkAdmin = @ShrinkAdmin,
				TaxAdministrator = @TaxAdministrator
			WHERE 
				TitleId = @TitleID
		END
	ELSE
		BEGIN
			INSERT INTO 
				TitleDefaultPermission (TitleId, Accountant, ApplicationConfigAdmin, BatchBuildOnly, Buyer, Coordinator, CostAdministrator, FacilityCreditProcessor, 
										DataAdministrator, DCAdmin, Distributor, DeletePO, EInvoicing, InventoryAdministrator, ItemAdministrator, 
										JobAdministrator, LockAdministrator, POAccountant, POApprovalAdministrator, POEditor, POSInterfaceAdministrator, 
										PriceBatchProcessor, Shrink, ShrinkAdmin, StoreAdministrator, TaxAdministrator, UserMaintenance, VendorAdministrator, 
										VendorCostDiscrepancyAdmin, Warehouse, CancelAllSales)
			VALUES
				(@TitleId, @Accountant, @ApplicationConfigAdmin, @BatchBuildOnly, @Buyer, @Coordinator, @CostAdministrator, @FacilityCreditProcessor, 
				 @DataAdministrator, @DCAdmin, @Distributor, @DeletePO, @EInvoicing, @InventoryAdministrator, @ItemAdministrator,
				 @JobAdministrator, @LockAdministrator, @POAccountant, @POApprovalAdministrator, @POEditor, @POSInterfaceAdministrator,
				 @PriceBatchProcessor, @Shrink, @ShrinkAdmin, @StoreAdministrator, @TaxAdministrator, @UserMaintenance, @VendorAdministrator, 
				 @VendorCostDiscrepancyAdmin, @Warehouse , ISNULL(@CancelAllSales, 0)
				 )
		END
		
		--Also update the users table for all users assigned to the specified title
		UPDATE Users SET
				Accountant = @Accountant,
				BatchBuildOnly = @BatchBuildOnly,
				Buyer = @Buyer,
				Coordinator = @Coordinator,
				CostAdmin = @CostAdministrator,
				FacilityCreditProcessor = @FacilityCreditProcessor,
				DCAdmin = @DCAdmin,
				DeletePO = @DeletePO,
				EInvoicing_Administrator = @EInvoicing,
				Inventory_Administrator = @InventoryAdministrator,
				Item_Administrator = @ItemAdministrator,
				Lock_Administrator = @LockAdministrator,
				PO_Accountant = @POAccountant,
				POApprovalAdmin = @POApprovalAdministrator,
				POEditor = @POEditor,
				PriceBatchProcessor = @PriceBatchProcessor,
				Distributor = @Distributor,
				Vendor_Administrator = @VendorAdministrator,
				VendorCostDiscrepancyAdmin = @VendorCostDiscrepancyAdmin,
				Warehouse = @Warehouse,
				CancelAllSales = ISNULL(@CancelAllSales,CancelAllSales),
				ApplicationConfigAdmin = @ApplicationConfigAdmin,
				DataAdministrator = @DataAdministrator,
				JobAdministrator = @JobAdministrator,
				POSInterfaceAdministrator = @POSInterfaceAdministrator,
				StoreAdministrator = @StoreAdministrator,
				UserMaintenance = @UserMaintenance,
				Shrink = @Shrink,
				ShrinkAdmin = @ShrinkAdmin,
				TaxAdministrator = @TaxAdministrator
		WHERE Title = @TitleID
		
		--Update any permission overrides set for that user
		UPDATE Users SET Users.Accountant = tpo.PermissionValue
		FROM TitlePermissionOverride tpo
		JOIN Users u ON u.User_Id = tpo.UserId
		JOIN Title t ON t.Title_Id = u.Title
		WHERE t.title_Id = @TitleID AND tpo.PermissionName = 'Accountant'
		
		UPDATE Users SET Users.BatchBuildOnly = tpo.PermissionValue
		FROM TitlePermissionOverride tpo
		JOIN Users u ON u.User_Id = tpo.UserId
		JOIN Title t ON t.Title_Id = u.Title
		WHERE t.title_Id = @TitleID AND tpo.PermissionName = 'BatchBuildOnly'
		
		UPDATE Users SET Users.Buyer = tpo.PermissionValue
		FROM TitlePermissionOverride tpo
		JOIN Users u ON u.User_Id = tpo.UserId
		JOIN Title t ON t.Title_Id = u.Title
		WHERE t.title_Id = @TitleID AND tpo.PermissionName = 'Buyer'
		
		UPDATE Users SET Users.Coordinator = tpo.PermissionValue
		FROM TitlePermissionOverride tpo
		JOIN Users u ON u.User_Id = tpo.UserId
		JOIN Title t ON t.Title_Id = u.Title
		WHERE t.title_Id = @TitleID AND tpo.PermissionName = 'Coordinator'
		
		UPDATE Users SET Users.CostAdmin = tpo.PermissionValue
		FROM TitlePermissionOverride tpo
		JOIN Users u ON u.User_Id = tpo.UserId
		JOIN Title t ON t.Title_Id = u.Title
		WHERE t.title_Id = @TitleID AND tpo.PermissionName = 'CostAdmin'
		
		UPDATE Users SET Users.FacilityCreditProcessor = tpo.PermissionValue
		FROM TitlePermissionOverride tpo
		JOIN Users u ON u.User_Id = tpo.UserId
		JOIN Title t ON t.Title_Id = u.Title
		WHERE t.title_Id = @TitleID AND tpo.PermissionName = 'FacilityCreditProcessor'
		
		UPDATE Users SET Users.DCAdmin = tpo.PermissionValue
		FROM TitlePermissionOverride tpo
		JOIN Users u ON u.User_Id = tpo.UserId
		JOIN Title t ON t.Title_Id = u.Title
		WHERE t.title_Id = @TitleID AND tpo.PermissionName = 'DCAdmin'
		
		UPDATE Users SET Users.DeletePO = tpo.PermissionValue
		FROM TitlePermissionOverride tpo
		JOIN Users u ON u.User_Id = tpo.UserId
		JOIN Title t ON t.Title_Id = u.Title
		WHERE t.title_Id = @TitleID AND tpo.PermissionName = 'DeletePO'
		
		UPDATE Users SET Users.EInvoicing_Administrator = tpo.PermissionValue
		FROM TitlePermissionOverride tpo
		JOIN Users u ON u.User_Id = tpo.UserId
		JOIN Title t ON t.Title_Id = u.Title
		WHERE t.title_Id = @TitleID AND tpo.PermissionName = 'EInvoicingAdmin'
		
		UPDATE Users SET Users.Inventory_Administrator = tpo.PermissionValue
		FROM TitlePermissionOverride tpo
		JOIN Users u ON u.User_Id = tpo.UserId
		JOIN Title t ON t.Title_Id = u.Title
		WHERE t.title_Id = @TitleID AND tpo.PermissionName = 'InventoryAdministrator'
		
		UPDATE Users SET Users.Item_Administrator = tpo.PermissionValue
		FROM TitlePermissionOverride tpo
		JOIN Users u ON u.User_Id = tpo.UserId
		JOIN Title t ON t.Title_Id = u.Title
		WHERE t.title_Id = @TitleID AND tpo.PermissionName = 'ItemAdministrator'
		
		UPDATE Users SET Users.Lock_Administrator = tpo.PermissionValue
		FROM TitlePermissionOverride tpo
		JOIN Users u ON u.User_Id = tpo.UserId
		JOIN Title t ON t.Title_Id = u.Title
		WHERE t.title_Id = @TitleID AND tpo.PermissionName = 'LockAdministrator'
		
		UPDATE Users SET Users.PO_Accountant = tpo.PermissionValue
		FROM TitlePermissionOverride tpo
		JOIN Users u ON u.User_Id = tpo.UserId
		JOIN Title t ON t.Title_Id = u.Title
		WHERE t.title_Id = @TitleID AND tpo.PermissionName = 'POAccountant'
		
		UPDATE Users SET Users.POApprovalAdmin = tpo.PermissionValue
		FROM TitlePermissionOverride tpo
		JOIN Users u ON u.User_Id = tpo.UserId
		JOIN Title t ON t.Title_Id = u.Title
		WHERE t.title_Id = @TitleID AND tpo.PermissionName = 'POApprovalAdmin'
		
		UPDATE Users SET Users.PriceBatchProcessor = tpo.PermissionValue
		FROM TitlePermissionOverride tpo
		JOIN Users u ON u.User_Id = tpo.UserId
		JOIN Title t ON t.Title_Id = u.Title
		WHERE t.title_Id = @TitleID AND tpo.PermissionName = 'PriceBatchProcessor'
		
		UPDATE Users SET Users.Distributor = tpo.PermissionValue
		FROM TitlePermissionOverride tpo
		JOIN Users u ON u.User_Id = tpo.UserId
		JOIN Title t ON t.Title_Id = u.Title
		WHERE t.title_Id = @TitleID AND tpo.PermissionName = 'Distributor'
		
		UPDATE Users SET Users.Vendor_Administrator = tpo.PermissionValue
		FROM TitlePermissionOverride tpo
		JOIN Users u ON u.User_Id = tpo.UserId
		JOIN Title t ON t.Title_Id = u.Title
		WHERE t.title_Id = @TitleID AND tpo.PermissionName = 'VendorAdministrator'
		
		UPDATE Users SET Users.VendorCostDiscrepancyAdmin = tpo.PermissionValue
		FROM TitlePermissionOverride tpo
		JOIN Users u ON u.User_Id = tpo.UserId
		JOIN Title t ON t.Title_Id = u.Title
		WHERE t.title_Id = @TitleID AND tpo.PermissionName = 'VendorCostDiscrepancyAdmin'
		
		UPDATE Users SET Users.Warehouse = tpo.PermissionValue
		FROM TitlePermissionOverride tpo
		JOIN Users u ON u.User_Id = tpo.UserId
		JOIN Title t ON t.Title_Id = u.Title
		WHERE t.title_Id = @TitleID AND tpo.PermissionName = 'Warehouse'
		
		UPDATE Users SET Users.ApplicationConfigAdmin = tpo.PermissionValue
		FROM TitlePermissionOverride tpo
		JOIN Users u ON u.User_Id = tpo.UserId
		JOIN Title t ON t.Title_Id = u.Title
		WHERE t.title_Id = @TitleID AND tpo.PermissionName = 'ApplicationConfigAdmin'
		
		UPDATE Users SET Users.DataAdministrator = tpo.PermissionValue
		FROM TitlePermissionOverride tpo
		JOIN Users u ON u.User_Id = tpo.UserId
		JOIN Title t ON t.Title_Id = u.Title
		WHERE t.title_Id = @TitleID AND tpo.PermissionName = 'DataAdministrator'
		
		UPDATE Users SET Users.JobAdministrator = tpo.PermissionValue
		FROM TitlePermissionOverride tpo
		JOIN Users u ON u.User_Id = tpo.UserId
		JOIN Title t ON t.Title_Id = u.Title
		WHERE t.title_Id = @TitleID AND tpo.PermissionName = 'JobAdministrator'
		
		UPDATE Users SET Users.POSInterfaceAdministrator = tpo.PermissionValue
		FROM TitlePermissionOverride tpo
		JOIN Users u ON u.User_Id = tpo.UserId
		JOIN Title t ON t.Title_Id = u.Title
		WHERE t.title_Id = @TitleID AND tpo.PermissionName = 'POSInterfaceAdministrator'
		
		UPDATE Users SET Users.StoreAdministrator = tpo.PermissionValue
		FROM TitlePermissionOverride tpo
		JOIN Users u ON u.User_Id = tpo.UserId
		JOIN Title t ON t.Title_Id = u.Title
		WHERE t.title_Id = @TitleID AND tpo.PermissionName = 'StoreAdministrator'
		
		UPDATE Users SET Users.UserMaintenance = tpo.PermissionValue
		FROM TitlePermissionOverride tpo
		JOIN Users u ON u.User_Id = tpo.UserId
		JOIN Title t ON t.Title_Id = u.Title
		WHERE t.title_Id = @TitleID AND tpo.PermissionName = 'UserMaintenance'
		
		UPDATE Users SET Users.Shrink = tpo.PermissionValue
		FROM TitlePermissionOverride tpo
		JOIN Users u ON u.User_Id = tpo.UserId
		JOIN Title t ON t.Title_Id = u.Title
		WHERE t.title_Id = @TitleID AND tpo.PermissionName = 'Shrink'
		
		UPDATE Users SET Users.ShrinkAdmin = tpo.PermissionValue
		FROM TitlePermissionOverride tpo
		JOIN Users u ON u.User_Id = tpo.UserId
		JOIN Title t ON t.Title_Id = u.Title
		WHERE t.title_Id = @TitleID AND tpo.PermissionName = 'ShrinkAdmin'

		UPDATE Users SET Users.TaxAdministrator = tpo.PermissionValue
		FROM TitlePermissionOverride tpo
		JOIN Users u ON u.User_Id = tpo.UserId
		JOIN Title t ON t.Title_Id = u.Title
		WHERE t.title_Id = @TitleID AND tpo.PermissionName = 'TaxAdministrator'

		--Delete resolved conflicts
		INSERT INTO @tblRoleValues
			SELECT TitleId, field, value
			FROM
			   (SELECT * from TitleDefaultPermission WHERE TitleId = @TitleId) d
			UNPIVOT
			   (Value FOR field IN (Accountant, BatchBuildOnly, Buyer, Coordinator, CostAdministrator, FacilityCreditProcessor, DCAdmin, DeletePO, EInvoicing, 
									InventoryAdministrator, ItemAdministrator, LockAdministrator, POAccountant, POApprovalAdministrator, 
									PriceBatchProcessor, Distributor, VendorAdministrator, VendorCostDiscrepancyAdmin, Warehouse, 
									ApplicationConfigAdmin, DataAdministrator, JobAdministrator, POSInterfaceAdministrator, StoreAdministrator, 
									UserMaintenance, Shrink, ShrinkAdmin)
			)AS unpvt;


		DELETE FROM RoleConflictReason
		WHERE RoleConflictReasonId IN (
											SELECT rcr.RoleConflictReasonId
											FROM RoleConflictReason rcr
											JOIN @tblRoleValues rv1 ON UPPER(REPLACE(REPLACE(REPLACE(rv1.RoleName,'Distributor','Receiver'),' ',''),'_','')) = UPPER(REPLACE(REPLACE(rcr.Role1,' ',''),'_',''))
											JOIN @tblRoleValues rv2 ON UPPER(REPLACE(REPLACE(REPLACE(rv2.RoleName,'Distributor','Receiver'),' ',''),'_','')) = UPPER(REPLACE(REPLACE(rcr.Role2,' ',''),'_',''))
											WHERE rv1.RoleValue <> rv2.RoleValue AND rcr.Title_Id = @TitleId)
GO
GRANT EXECUTE
    ON OBJECT::[dbo].[SaveTitlePermissions] TO [IRMAAdminRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[SaveTitlePermissions] TO [IRMAClientRole]
    AS [dbo];

