-- 8.25.08 V3.2 Robert Shurbet
-- Added second insert statement to support SLIM access flags

CREATE PROCEDURE [dbo].[Administration_UserAdmin_InsertUser]
	@User_ID int OUTPUT,
	
	@AccountEnabled bit,
	@CoverPage varchar(30),
	@EMail varchar(50),
	@Fax_Number varchar(15),
	@FullName varchar(50),
	@Pager_Email varchar(50),
	@Phone_Number varchar(25),
	@Printer varchar(50),
	@PromoAccessLevel smallint,
	@RecvLog_Store_Limit int,
	@Telxon_Store_Limit int,
	@Title varchar(60),
	@UserName varchar(25),
				
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
	@CancelAllSales bit = NULL,
		
	@Authorizations bit,
	@IRMAPush bit,
	@ItemRequest bit,
	@RetailCost bit,
	@ScaleInfo BIT,
	@StoreSpecials bit,
	@UserAdmin bit,
	@VendorRequest bit,
	@WebQuery bit,
	@ECommerce bit
AS
	BEGIN

		DECLARE @UserID int

		INSERT INTO Users
				   ([AccountEnabled],
				    [CoverPage],
				    [EMail],
				    [Fax_Number],
				    [FullName],
				    [Pager_Email],
				    [Phone_Number],
				    [Printer],
				    [PromoAccessLevel],
				    [RecvLog_Store_Limit],
				    [Telxon_Store_Limit],
				    [Title],   
				    [UserName],
				    			   
					[Accountant],
					[ApplicationConfigAdmin],
					[BatchBuildOnly],
					[Buyer],
					[Coordinator],
					[CostAdmin],
					[FacilityCreditProcessor],
					[DataAdministrator],
					[DCAdmin],
					[Distributor],
					[DeletePO],
					[EInvoicing_Administrator],
					[Inventory_Administrator],
					[Item_Administrator],
					[JobAdministrator],
					[Lock_Administrator],
					[SuperUser],
					[PO_Accountant],
					[POApprovalAdmin],
					[POEditor],
					[POSInterfaceAdministrator],
					[PriceBatchProcessor],
					[SecurityAdministrator],
					[Shrink],
					[ShrinkAdmin],
					[StoreAdministrator],
					[SystemConfigurationAdministrator],
					[TaxAdministrator],
					[UserMaintenance],
					[Vendor_Administrator],
					[VendorCostDiscrepancyAdmin],
					[Warehouse],
					[CancelAllSales]
				   )
			 VALUES
				   (@AccountEnabled,
					@CoverPage,
					@EMail,
					@Fax_Number,
					@FullName,
					@Pager_Email,
					@Phone_Number,
					@Printer,
					@PromoAccessLevel,
					@RecvLog_Store_Limit,
					@Telxon_Store_Limit,
					@Title,
					@UserName,
									
					@Accountant,
					@ApplicationConfigAdmin,
					@BatchBuildOnly,
					@Buyer,
					@Coordinator,
					@CostAdmin,
					@FacilityCreditProcessor,
					@DataAdministrator,
					@DCAdmin,
					@Distributor,
					@DeletePO,
					@EInvoicingAdmin,
					@Inventory_Administrator,
					@Item_Administrator,
					@JobAdministrator,
					@Lock_Administrator,
					@SuperUser,
					@PO_Accountant,
					@POApprovalAdmin,
					@POEditor,
					@POSInterfaceAdministrator,
					@PriceBatchProcessor,
					@SecurityAdministrator,
					@Shrink,
					@ShrinkAdmin,
					@StoreAdministrator,
					@SystemConfigurationAdministrator,
					@TaxAdministrator,
					@UserMaintenance,
					@Vendor_Administrator,
					@VendorCostDiscrepancyAdmin,
					@Warehouse,
					ISNULL(@CancelAllSales, 0)
					)
					
		-- get the new User_ID from Users for the SlimAccess insert			

		SELECT @User_ID = SCOPE_IDENTITY()

		INSERT INTO SlimAccess 
					(Authorizations,
					 IRMAPush,
					 ItemRequest,
					 RetailCost,
					 ScaleInfo,
					 StoreSpecials,
					 UserAdmin,
					 [User_ID],
					 VendorRequest,
					 WebQuery,
					 ECommerce
					 )
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

GO
GRANT EXECUTE
    ON OBJECT::[dbo].[Administration_UserAdmin_InsertUser] TO [IRMAAdminRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[Administration_UserAdmin_InsertUser] TO [IRMAClientRole]
    AS [dbo];

