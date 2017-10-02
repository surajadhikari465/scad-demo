CREATE PROCEDURE [dbo].[Administration_UserAdmin_UpdateUser]
	--User Info
	@AccountEnabled bit,
	@CoverPage varchar(30),
	@EMail varchar(50),
	@Fax_Number varchar(15),
	@FullName varchar(50),
	@Pager_Email varchar(50),
	@Phone_Number varchar(25),
	@Printer varchar(50),
	@RecvLog_Store_Limit int,
	@Telxon_Store_Limit int,
	@Title int,
	@User_Id int,
	@UserName varchar(25),

	--Client Roles
	@Accountant bit,
	@ApplicationConfigAdmin bit,
	@BatchBuildOnly bit,
	@Buyer bit,
	@Coordinator bit,
	@CostAdmin bit,
	@FacilityCreditProcessor bit,
	@DataAdministrator bit,
	@DCAdmin bit,
	@Distributor bit,
	@DeletePO bit,
	@EInvoicingAdmin bit,
	@Inventory_Administrator bit,
	@Item_Administrator bit,
	@JobAdministrator bit,
	@Lock_Administrator bit,
	@SuperUser bit,
	@PO_Accountant bit,
	@POApprovalAdmin bit,
	@POEditor bit,
	@POSInterfaceAdministrator bit,
	@PriceBatchProcessor bit,
	@PromoAccessLevel smallint,
	@SecurityAdministrator bit,
	@Shrink bit,
	@ShrinkAdmin bit,
	@StoreAdministrator bit,
	@SystemConfigurationAdministrator bit,
	@TaxAdministrator bit,
	@UserMaintenance bit,
	@Vendor_Administrator bit,
	@VendorCostDiscrepancyAdmin bit,
	@Warehouse bit,

	--SLIM --optional paramters for user audit Export
	@Authorizations bit = Null,
	@IRMAPush bit = Null,
	@ItemRequest bit = Null,
	@RetailCost bit = Null,
	@ScaleInfo BIT= Null,
	@StoreSpecials bit= Null,
	@UserAdmin bit= Null,
	@VendorRequest bit= Null,
	@WebQuery bit= Null,
	@ECommerce bit = Null

AS
	BEGIN
	
		DECLARE @tblRoleValues TABLE (User_Id int, RoleName varchar(50), RoleValue bit)

		UPDATE Users
		   SET 
			  [AccountEnabled] = @AccountEnabled,
			  [CoverPage] = @CoverPage,
			  [EMail] = @EMail,
			  [Fax_Number] = @Fax_Number,
			  [FullName] = @FullName,
			  [Pager_Email] = @Pager_Email,
			  [Phone_Number] = @Phone_Number,
			  [Printer] = @Printer,
			  [PromoAccessLevel] = @PromoAccessLevel,
			  [RecvLog_Store_Limit] = @RecvLog_Store_Limit,
			  [Telxon_Store_Limit] = @Telxon_Store_Limit,
			  [Title] = @Title,
			  [UserName] = @UserName,
		      
			  [Accountant] = @Accountant,
			  [ApplicationConfigAdmin] = @ApplicationConfigAdmin,
			  [BatchBuildOnly] = @BatchBuildOnly,
			  [Buyer] = @Buyer,
			  [Coordinator] = @Coordinator,
			  [CostAdmin] = @CostAdmin,
			  [FacilityCreditProcessor] = @FacilityCreditProcessor,
			  [DataAdministrator] = @DataAdministrator,
			  [DCAdmin] = @DCAdmin,
			  [Distributor] = @Distributor,
			  [DeletePO] = @DeletePO,
			  [EInvoicing_Administrator] = @EInvoicingAdmin,
			  [Inventory_Administrator] = @Inventory_Administrator,
			  [Item_Administrator] = @Item_Administrator,
			  [JobAdministrator] = @JobAdministrator,
			  [Lock_Administrator] = @Lock_Administrator,
			  [POSInterfaceAdministrator] = @POSInterfaceAdministrator,
			  [PriceBatchProcessor] = @PriceBatchProcessor,
			  [SuperUser] = @SuperUser,
			  [PO_Accountant] = @PO_Accountant,
			  [POApprovalAdmin] = @POApprovalAdmin,
			  [POEditor] = @POEditor,
			  [SecurityAdministrator] = @SecurityAdministrator,
			  [Shrink] = @Shrink,
			  [ShrinkAdmin] = @ShrinkAdmin,
			  [StoreAdministrator] = @StoreAdministrator,
			  [SystemConfigurationAdministrator] = @SystemConfigurationAdministrator,
			  [TaxAdministrator] = @TaxAdministrator,
			  [UserMaintenance] = @UserMaintenance,
			  [Vendor_Administrator] = @Vendor_Administrator,
			  [VendorCostDiscrepancyAdmin] = @VendorCostDiscrepancyAdmin,
			  [Warehouse] = @Warehouse
		 	  
		 WHERE  User_ID = @User_Id

		-- 8.25.08 V3.2 Robert Shurbet
		-- Added second update statement to support SLIM access flags
		-- if the account is enabled then update it with the parameters passed in
		-- otherwise set all SlimAccess attributes to FALSE
 IF( @ScaleInfo IS NOT NULL)
 BEGIN
		DECLARE @IsSlimUser bit

		SET @IsSlimUser = dbo.fn_IsUserInSLIM(@User_ID)

		IF @AccountEnabled = 1
		BEGIN

		-- if the user doesn't exist, add them, otherwise update the existing record
			IF @IsSlimUser = 1
				BEGIN
					UPDATE SlimAccess
						SET 
							Authorizations = @Authorizations,
							IRMAPush = @IRMAPush,
							ItemRequest = @ItemRequest,
							RetailCost = @RetailCost,
							ScaleInfo = @ScaleInfo,	
							StoreSpecials = @StoreSpecials,
							UserAdmin = @UserAdmin,
							VendorRequest = @VendorRequest,
							WebQuery = @WebQuery,
							ECommerce = @ECommerce
					WHERE User_ID = @User_ID
				END
			ELSE
				BEGIN
					INSERT INTO SlimAccess 
								(Authorizations
								 ,IRMAPush
								 ,ItemRequest
								 ,RetailCost
								 ,ScaleInfo
								 ,StoreSpecials
								 ,UserAdmin
								 ,User_ID
								 ,VendorRequest
								 ,WebQuery
								 ,ECommerce)
					VALUES
							   (@Authorizations,
								@IRMAPush,
								@ItemRequest,
								@RetailCost,
								@ScaleInfo,
								@StoreSpecials,												
								@UserAdmin,
								@User_ID,
								@VendorRequest,
								@WebQuery,
								@ECommerce
								)
				END
		END
		ELSE
			BEGIN
				UPDATE SlimAccess
					SET Authorizations = 0,
						IRMAPush = 0,
						ItemRequest = 0,
						RetailCost = 0,
						ScaleInfo = 0,
						StoreSpecials = 0,
						UserAdmin = 0,
						VendorRequest = 0,
						WebQuery = 0,
						ECommerce = 0
				WHERE User_ID = @User_ID
			END
	END
END
	-- update any UserStoreTeamTitle values with the new title information.
	IF @AccountEnabled = 1
	BEGIN
		EXEC Administration_UserStoreTeamTitle @User_ID, 0, 0, @Title, 'U'
	END 

	--Delete resolved conflicts
	INSERT INTO @tblRoleValues
		SELECT User_Id, field, value
		FROM
		   (SELECT * from Users WHERE User_Id = @User_Id) d
		UNPIVOT
		   (Value FOR field IN (Accountant, ApplicationConfigAdmin, BatchBuildOnly, Buyer, Coordinator, CostAdmin, FacilityCreditProcessor, 
								DataAdministrator, DCAdmin, Distributor, DeletePO, EInvoicing_Administrator, Inventory_Administrator, 
								Item_Administrator,JobAdministrator,Lock_Administrator,POSInterfaceAdministrator, PriceBatchProcessor, POEditor,
								SuperUser, PO_Accountant, POApprovalAdmin, SecurityAdministrator, Shrink, ShrinkAdmin, 
								StoreAdministrator, SystemConfigurationAdministrator, TaxAdministrator, UserMaintenance, Vendor_Administrator, 
								VendorCostDiscrepancyAdmin, Warehouse)
		)AS unpvt;


	DELETE FROM RoleConflictReason
	WHERE RoleConflictReasonId IN (
										SELECT rcr.RoleConflictReasonId
										FROM RoleConflictReason rcr
										JOIN @tblRoleValues rv1 ON UPPER(REPLACE(REPLACE(rv1.RoleName,' ',''),'_','')) = UPPER(REPLACE(REPLACE(rcr.Role1,' ',''),'_',''))
										JOIN @tblRoleValues rv2 ON UPPER(REPLACE(REPLACE(rv2.RoleName,' ',''),'_','')) = UPPER(REPLACE(REPLACE(rcr.Role2,' ',''),'_',''))
										WHERE rv1.RoleValue <> rv2.RoleValue AND rcr.User_Id = @User_Id)	
										
	--Log Title Permission Overrides
	DELETE FROM TitlePermissionOverride WHERE UserId = @User_Id  --first delete all overrides for this user
	
	IF (SELECT Accountant FROM TitleDefaultPermission WHERE TitleId = @Title) <> @Accountant
		INSERT INTO TitlePermissionOverride (UserId, PermissionName, PermissionValue) VALUES (@User_Id, 'Accountant', @Accountant)

	IF (SELECT ApplicationConfigAdmin FROM TitleDefaultPermission WHERE TitleId = @Title) <> @ApplicationConfigAdmin
		INSERT INTO TitlePermissionOverride (UserId, PermissionName, PermissionValue) VALUES (@User_Id, 'ApplicationConfigAdmin', @ApplicationConfigAdmin)
		
	IF (SELECT BatchBuildOnly FROM TitleDefaultPermission WHERE TitleId = @Title) <> @BatchBuildOnly
		INSERT INTO TitlePermissionOverride (UserId, PermissionName, PermissionValue) VALUES (@User_Id, 'BatchBuildOnly', @BatchBuildOnly)
		
	IF (SELECT Buyer FROM TitleDefaultPermission WHERE TitleId = @Title) <> @Buyer
		INSERT INTO TitlePermissionOverride (UserId, PermissionName, PermissionValue) VALUES (@User_Id, 'Buyer', @Buyer)
		
	IF (SELECT Coordinator FROM TitleDefaultPermission WHERE TitleId = @Title) <> @Coordinator
		INSERT INTO TitlePermissionOverride (UserId, PermissionName, PermissionValue) VALUES (@User_Id, 'Coordinator', @Coordinator)
		
	IF (SELECT CostAdministrator FROM TitleDefaultPermission WHERE TitleId = @Title) <> @CostAdmin
		INSERT INTO TitlePermissionOverride (UserId, PermissionName, PermissionValue) VALUES (@User_Id, 'CostAdmin', @CostAdmin)
		
	IF (SELECT FacilityCreditProcessor FROM TitleDefaultPermission WHERE TitleId = @Title) <> @FacilityCreditProcessor
		INSERT INTO TitlePermissionOverride (UserId, PermissionName, PermissionValue) VALUES (@User_Id, 'FacilityCreditProcessor', @FacilityCreditProcessor)
		
	IF (SELECT DataAdministrator FROM TitleDefaultPermission WHERE TitleId = @Title) <> @DataAdministrator
		INSERT INTO TitlePermissionOverride (UserId, PermissionName, PermissionValue) VALUES (@User_Id, 'DataAdministrator', @DataAdministrator)
		
	IF (SELECT Distributor FROM TitleDefaultPermission WHERE TitleId = @Title) <> @Distributor
		INSERT INTO TitlePermissionOverride (UserId, PermissionName, PermissionValue) VALUES (@User_Id, 'Distributor', @Distributor)

	IF (SELECT DeletePO FROM TitleDefaultPermission WHERE TitleId = @Title) <> @DeletePO
		INSERT INTO TitlePermissionOverride (UserId, PermissionName, PermissionValue) VALUES (@User_Id, 'DeletePO', @DeletePO)		
		
	IF (SELECT EInvoicing FROM TitleDefaultPermission WHERE TitleId = @Title) <> @EInvoicingAdmin
		INSERT INTO TitlePermissionOverride (UserId, PermissionName, PermissionValue) VALUES (@User_Id, 'EInvoicingAdmin', @EInvoicingAdmin)
		
	IF (SELECT InventoryAdministrator FROM TitleDefaultPermission WHERE TitleId = @Title) <> @Item_Administrator
		INSERT INTO TitlePermissionOverride (UserId, PermissionName, PermissionValue) VALUES (@User_Id, 'ItemAdministrator', @Item_Administrator)

	IF (SELECT InventoryAdministrator FROM TitleDefaultPermission WHERE TitleId = @Title) <> @Inventory_Administrator
		INSERT INTO TitlePermissionOverride (UserId, PermissionName, PermissionValue) VALUES (@User_Id, 'InventoryAdministrator', @Inventory_Administrator)
		
	IF (SELECT JobAdministrator FROM TitleDefaultPermission WHERE TitleId = @Title) <> @JobAdministrator
		INSERT INTO TitlePermissionOverride (UserId, PermissionName, PermissionValue) VALUES (@User_Id, 'JobAdministrator', @JobAdministrator)
		
	IF (SELECT LockAdministrator FROM TitleDefaultPermission WHERE TitleId = @Title) <> @Lock_Administrator
		INSERT INTO TitlePermissionOverride (UserId, PermissionName, PermissionValue) VALUES (@User_Id, 'LockAdministrator', @Lock_Administrator)

	IF (SELECT POSInterfaceAdministrator FROM TitleDefaultPermission WHERE TitleId = @Title) <> @POSInterfaceAdministrator
		INSERT INTO TitlePermissionOverride (UserId, PermissionName, PermissionValue) VALUES (@User_Id, 'POSInterfaceAdministrator', @POSInterfaceAdministrator)
		
	IF (SELECT PriceBatchProcessor FROM TitleDefaultPermission WHERE TitleId = @Title) <> @PriceBatchProcessor
		INSERT INTO TitlePermissionOverride (UserId, PermissionName, PermissionValue) VALUES (@User_Id, 'PriceBatchProcessor', @PriceBatchProcessor)
		
	IF (SELECT POAccountant FROM TitleDefaultPermission WHERE TitleId = @Title) <> @PO_Accountant
		INSERT INTO TitlePermissionOverride (UserId, PermissionName, PermissionValue) VALUES (@User_Id, 'POAccountant', @PO_Accountant)
		
	IF (SELECT POApprovalAdministrator FROM TitleDefaultPermission WHERE TitleId = @Title) <> @POApprovalAdmin
		INSERT INTO TitlePermissionOverride (UserId, PermissionName, PermissionValue) VALUES (@User_Id, 'POApprovalAdmin', @POApprovalAdmin)

	IF (SELECT POEditor FROM TitleDefaultPermission WHERE TitleId = @Title) <> @POEditor
		INSERT INTO TitlePermissionOverride (UserId, PermissionName, PermissionValue) VALUES (@User_Id, 'POEditor', @POEditor)
		
	IF (SELECT Shrink FROM TitleDefaultPermission WHERE TitleId = @Title) <> @Shrink
		INSERT INTO TitlePermissionOverride (UserId, PermissionName, PermissionValue) VALUES (@User_Id, 'Shrink', @Shrink)
		
	IF (SELECT ShrinkAdmin FROM TitleDefaultPermission WHERE TitleId = @Title) <> @ShrinkAdmin
		INSERT INTO TitlePermissionOverride (UserId, PermissionName, PermissionValue) VALUES (@User_Id, 'ShrinkAdmin', @ShrinkAdmin)
		
	IF (SELECT StoreAdministrator FROM TitleDefaultPermission WHERE TitleId = @Title) <> @StoreAdministrator
		INSERT INTO TitlePermissionOverride (UserId, PermissionName, PermissionValue) VALUES (@User_Id, 'StoreAdministrator', @StoreAdministrator)

	IF (SELECT TaxAdministrator FROM TitleDefaultPermission WHERE TitleId = @Title) <> @TaxAdministrator
		INSERT INTO TitlePermissionOverride (UserId, PermissionName, PermissionValue) VALUES (@User_Id, 'TaxAdministrator', @TaxAdministrator)
		
	IF (SELECT UserMaintenance FROM TitleDefaultPermission WHERE TitleId = @Title) <> @UserMaintenance
		INSERT INTO TitlePermissionOverride (UserId, PermissionName, PermissionValue) VALUES (@User_Id, 'UserMaintenance', @UserMaintenance)
		
	IF (SELECT VendorAdministrator FROM TitleDefaultPermission WHERE TitleId = @Title) <> @Vendor_Administrator
		INSERT INTO TitlePermissionOverride (UserId, PermissionName, PermissionValue) VALUES (@User_Id, 'VendorAdministrator', @Vendor_Administrator)
		
	IF (SELECT VendorCostDiscrepancyAdmin FROM TitleDefaultPermission WHERE TitleId = @Title) <> @VendorCostDiscrepancyAdmin
		INSERT INTO TitlePermissionOverride (UserId, PermissionName, PermissionValue) VALUES (@User_Id, 'VendorCostDiscrepancyAdmin', @VendorCostDiscrepancyAdmin)
		
	IF (SELECT Warehouse FROM TitleDefaultPermission WHERE TitleId = @Title) <> @Warehouse
		INSERT INTO TitlePermissionOverride (UserId, PermissionName, PermissionValue) VALUES (@User_Id, 'Warehouse', @Warehouse)

GO
GRANT EXECUTE
    ON OBJECT::[dbo].[Administration_UserAdmin_UpdateUser] TO [IRMAAdminRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[Administration_UserAdmin_UpdateUser] TO [IRMAClientRole]
    AS [dbo];

