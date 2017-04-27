SET QUOTED_IDENTIFIER ON 
GO
SET ANSI_NULLS ON 
GO

if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[CopyExistingPO]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
	drop procedure [dbo].[CopyExistingPO]
GO

CREATE PROCEDURE dbo.CopyExistingPO
	@OrderHeader_ID			int,
	@InvalidCopyPOItems_ID	int,
	@ExpectedDate			smalldatetime,
	@User_Id				int,
	@CopyToStoreNo			int,
	@IsDeleted				bit

AS 

	-- **************************************************************************
	-- Procedure: CopyExistingPO()
	--    Author: n/a
	--      Date: n/a
	--
	-- Description:
	-- This procedure is called from Orders.vb
	--
	-- Modification History:
	-- Date			Init	TFS		Comment
	-- 12/05/2011	BBB		3744	applied coding standards; removed deprecated fn call;
	-- 03/14/2013	DN		8870	Enable copy PO for deleted POs.
	-- 2013/05/17	KM		12355	Use ISNULLs for nullable columns in DeletedOrder;
	-- **************************************************************************

BEGIN

	--**************************************************************************
	--Order Header variables
	--**************************************************************************	
	DECLARE @Vendor_ID					int
	DECLARE @OrderType_ID				tinyint
	DECLARE @ProductType_ID				tinyint
	DECLARE @PurchaseLocation_ID		int
	DECLARE @ReceiveLocation_ID			int
	DECLARE @Transfer_SubTeam			int
	DECLARE @Transfer_To_SubTeam		int
	DECLARE @Fax_Order					bit
	DECLARE @CreatedBy					int
	DECLARE @Return_Order				tinyint
	DECLARE @FromQueue					bit
	DECLARE @CurrDate					datetime
	DECLARE @SupplyTransferToSubTeam	int
	DECLARE @CurrencyID					int
	DECLARE @OrderHeaderDesc			varchar(4000)
	
	--**************************************************************************
	--Order Item Variables
	--**************************************************************************	
	DECLARE @NewOrderHeader_ID int

	--**************************************************************************	
	--Create Order Header
	--**************************************************************************	
	IF @IsDeleted != 1 
		BEGIN
			SELECT 
				@Vendor_ID					= OrderHeader.Vendor_ID,
				@OrderType_ID				= OrderType_ID,
				@ProductType_ID				= ProductType_ID,
				@Transfer_SubTeam			= Transfer_SubTeam,
				@Transfer_To_SubTeam		= Transfer_To_SubTeam,
				@Fax_Order					= Fax_Order,
				@CreatedBy					= @User_ID,
				@Return_Order				= Return_Order,
				@SupplyTransferToSubTeam	= SupplyTransferToSubTeam,
				@FromQueue					= FromQueue,
				@CurrencyID					= CurrencyID,
				@OrderHeaderDesc			= 'Copied from PO: ' + convert(varchar(20),OrderHeader_ID)
			FROM 
				OrderHeader
			WHERE 
				OrderHeader_ID = @OrderHeader_ID
	
			SELECT @PurchaseLocation_ID = Vendor_ID FROM [Vendor] WHERE Store_No = @CopyToStoreNo
			SELECT @ReceiveLocation_ID  = Vendor_ID FROM [Vendor] WHERE Store_No = @CopyToStoreNo

			INSERT INTO OrderHeader
						(Vendor_ID, OrderType_ID, ProductType_ID, PurchaseLocation_ID, ReceiveLocation_ID, Transfer_SubTeam, Transfer_To_SubTeam, 
						 Fax_Order, Expected_Date, CreatedBy, Return_Order, FromQueue, SupplyTransferToSubTeam, CurrencyId, OrderHeaderDesc)
					VALUES 
						(@Vendor_ID, @OrderType_ID, @ProductType_ID, @PurchaseLocation_ID, @ReceiveLocation_ID, @Transfer_SubTeam, @Transfer_To_SubTeam, 
						@Fax_Order, @ExpectedDate, @CreatedBy, @Return_Order, @FromQueue, @SupplyTransferToSubTeam, @CurrencyID, @OrderHeaderDesc)
						
			--**************************************************************************	
			--Create Order Items
			--**************************************************************************	
			SELECT @NewOrderHeader_ID = IDENT_CURRENT('OrderHeader') + IDENT_INCR('OrderHeader')
    
			SELECT @CurrDate = CONVERT(datetime, CONVERT(varchar(255), GETDATE(), 101))         
                              
			SELECT 
				@NewOrderHeader_ID, 
				OI.Item_Key, 
				OI.Units_Per_Pallet, 
				OI.QuantityUnit, 
				OI.QuantityOrdered, 
				OI.Cost, 
				OI.CostUnit, 
				0, 
				OI.HandlingUnit, 
				OI.Freight, 
				OI.FreightUnit, 
				0, 
				0, 
				0, 
				OI.LandedCost, 
				OI.LineItemCost, 
				OI.LineItemFreight, 
				OI.LineItemHandling, 
				OI.UnitCost, 
				OI.UnitExtCost, 
				OI.Package_Desc1, 
				OI.Package_Desc2, 
				OI.Package_Unit_ID, 
				OI.MarkupPercent,
				OI.MarkupCost, 
				OI.Retail_Unit_ID, 
				OI.CostAdjustmentReason_ID, 
				OI.Origin_ID, 
				OI.CountryProc_ID, 
				OI.CreditReason_ID 
			FROM
				OrderItem OI
				JOIN OrderHeader OH ON OH.OrderHeader_ID = OI.OrderHeader_ID
			WHERE 
				OI.OrderHeader_ID = @OrderHeader_ID AND OI.Item_Key NOT IN (SELECT Item_Key FROM InvalidCopyPOItems WHERE InvalidCopyPOItems_ID = @InvalidCopyPOItems_ID AND Item_Key IS NOT NULL)
		END
	ELSE
		BEGIN
			SELECT 
				@Vendor_ID					= DeletedOrder.Vendor_ID,
				@OrderType_ID				= OrderType_ID,
				@ProductType_ID				= ProductType_ID,
				@Transfer_SubTeam			= Transfer_SubTeam,
				@Transfer_To_SubTeam		= Transfer_To_SubTeam,
				@Fax_Order					= ISNULL(Fax_Order, 0),
				@CreatedBy					= @User_ID,
				@Return_Order				= ISNULL(Return_Order, 0),
				@SupplyTransferToSubTeam	= SupplyTransferToSubTeam,
				@FromQueue					= ISNULL(FromQueue, 0),
				@CurrencyID					= CurrencyID,
				@OrderHeaderDesc			= 'Copied from Deleted PO: ' + convert(varchar(20),OrderHeader_ID)
			FROM 
				DeletedOrder
			WHERE 
				OrderHeader_ID = @OrderHeader_ID
	
			SELECT @PurchaseLocation_ID = Vendor_ID FROM [Vendor] WHERE Store_No = @CopyToStoreNo
			SELECT @ReceiveLocation_ID  = Vendor_ID FROM [Vendor] WHERE Store_No = @CopyToStoreNo

			INSERT INTO OrderHeader
						(Vendor_ID, OrderType_ID, ProductType_ID, PurchaseLocation_ID, ReceiveLocation_ID, Transfer_SubTeam, Transfer_To_SubTeam, 
						 Fax_Order, Expected_Date, CreatedBy, Return_Order, FromQueue, SupplyTransferToSubTeam, CurrencyId, OrderHeaderDesc)
					VALUES 
						(@Vendor_ID, @OrderType_ID, @ProductType_ID, @PurchaseLocation_ID, @ReceiveLocation_ID, @Transfer_SubTeam, @Transfer_To_SubTeam, 
						@Fax_Order, @ExpectedDate, @CreatedBy, @Return_Order, @FromQueue, @SupplyTransferToSubTeam, @CurrencyID, @OrderHeaderDesc)
						
			--**************************************************************************	
			--Create Order Items
			--**************************************************************************	
			SELECT @NewOrderHeader_ID = IDENT_CURRENT('OrderHeader') + IDENT_INCR('OrderHeader')
    
			SELECT @CurrDate = CONVERT(datetime, CONVERT(varchar(255), GETDATE(), 101))      
	
			UPDATE DeletedOrder SET CopyDeletedPODate		= @CurrDate,
									CopyDeletedPOBy			= @User_Id,
									CopiedOrderHeader_ID	= @NewOrderHeader_ID - 1
			WHERE OrderHeader_ID		= @OrderHeader_ID	AND
				  CopyDeletedPOBy		IS NULL				AND
				  CopiedOrderHeader_ID	IS NULL
                              
			SELECT 
				@NewOrderHeader_ID, 
				OI.Item_Key, 
				OI.Units_Per_Pallet, 
				OI.QuantityUnit, 
				OI.QuantityOrdered, 
				OI.Cost, 
				OI.CostUnit, 
				0, 
				OI.HandlingUnit, 
				OI.Freight, 
				OI.FreightUnit, 
				0, 
				0, 
				0, 
				OI.LandedCost, 
				OI.LineItemCost, 
				OI.LineItemFreight, 
				OI.LineItemHandling, 
				OI.UnitCost, 
				OI.UnitExtCost, 
				OI.Package_Desc1, 
				OI.Package_Desc2, 
				OI.Package_Unit_ID, 
				OI.MarkupPercent,
				OI.MarkupCost, 
				OI.Retail_Unit_ID, 
				OI.CostAdjustmentReason_ID, 
				OI.Origin_ID, 
				OI.CountryProc_ID, 
				OI.CreditReason_ID 
			FROM
				DeletedOrderItem OI
				JOIN DeletedOrder OH ON OH.OrderHeader_ID = OI.OrderHeader_ID
			WHERE 
				OI.OrderHeader_ID = @OrderHeader_ID AND OI.Item_Key NOT IN (SELECT Item_Key FROM InvalidCopyPOItems WHERE InvalidCopyPOItems_ID = @InvalidCopyPOItems_ID AND Item_Key IS NOT NULL)

		END
END
GO
SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO