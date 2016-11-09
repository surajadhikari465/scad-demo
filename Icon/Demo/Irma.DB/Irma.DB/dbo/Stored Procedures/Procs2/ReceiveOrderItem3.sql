CREATE PROCEDURE [dbo].[ReceiveOrderItem3] 
	@OrderItem_ID				int,  
    @DateReceived				DateTime,  
    @Quantity					decimal(18,4),  
    @Package_Desc1				decimal(9,4),  
    @Weight						decimal(18,4),  
    @RecvDiscrepancyReasonID	int,
	@User_ID					int,
	@Correction                 int = 0,
    @Cost						decimal(18,4) OUTPUT,   
    @Freight					decimal(18,4) OUTPUT,  
    @LineItemCost				decimal(18,4) OUTPUT,  
    @LineItemFreight			decimal(18,4) OUTPUT,   
    @ReceivedItemCost			decimal(18,4) OUTPUT,  
    @ReceivedItemFreight		decimal(18,4) OUTPUT,
	@AlreadyClosed				int = NULL	  OUTPUT,
	@ReceivedViaGun             bit = 0

AS   

-- **************************************************************************
-- Procedure: ReceiveOrderItem3()
--    Author: n/a
--      Date: n/a
--
-- Description:
-- This procedure is called from numerous places to update OrderItem values.
--
-- Modification History:
-- Date       	Init  	TFS   	Comment
-- 03/28/2011	BBB   	1732	Updated for code standards and readability
-- 07/25/2011   MD      2459    Added Receiving Discrepancy Reason Code ID
-- 2011/12/22	KM		3744	Extension change; code formatting
-- 2011/12/30	KM		3744	Remove local variable @Correction - all calls to this SP
--								pass zero as the argument, so the logic is unneeded;
-- 02/06/2012   FA      4652    Added @Correction parameter again to make it 
--                              compatible to PSCClient
-- 12/05/2012   TD      9324    4.7 - Added @ReceivedViaGun to determine if Item
--                              was scanned in via Mobile
-- 01/30/2013	BS		9975	Added @AlreadyClosed Output Parameter for handheld. And added
--								section that checks close date to see if order is closed already.
-- 02/01/2013	BS		10028	Populate @AlreadyClosed with rest of parameters instead of trying
--								to use a default.
-- 03/05/2013   FA      8325    Added code to insert record into OrderItemRefused table
-- 03/11/2013	FA		8325	Added code to calculate QuantitiyReceived correctly when there are refused items.
-- 03/12/2013	FA		8325	Added code to update TotalRefused on OrderHeader table
-- 2013/03/26	KM		11682	Make @AlreadyClosed optional; Move @ReceivedViaGun to the end of
--								the parameter listing;
-- 2013/04/13	KM		11974	Disable receiving refusal enhancement;
-- ***************************************************************************************

BEGIN      
    SET NOCOUNT ON        

    BEGIN TRY      

		--**************************************************************************
		--Exit stored proc and Return output if the PO is already closed
		--@AlreadyClosed = 0 means it's an open PO
		--@AlreadyClosed = 1 means it's closed and Update cannot happen
		--**************************************************************************
		DECLARE @CloseDate datetime
		
		SELECT
			@CloseDate = oh.CloseDate
		FROM
			OrderHeader				oh (nolock)
			INNER JOIN OrderItem	oi (nolock) ON oh.OrderHeader_ID = oi.OrderHeader_ID
		WHERE
			oi.OrderItem_ID = @OrderItem_ID

		IF @CloseDate IS NOT NULL
		BEGIN
			SET @AlreadyClosed			= 1
			SET @Cost					= 0	   
			SET @Freight				= 0	  
			SET @LineItemCost			= 0	  
			SET @LineItemFreight		= 0	   
			SET @ReceivedItemCost		= 0	  
			SET @ReceivedItemFreight	= 0	
			RETURN
		END

		--**************************************************************************
		--Declare internal variables
		--**************************************************************************
		DECLARE
			@Vendor_ID				int, 
			@ReceiveLocation_ID		int, 
			@QuantityOrdered		decimal(18,4), 
			@QuantityAllocated		decimal(18,4), 
			@QuantityReceived		decimal(18,4),
			--@QuantityRefused		decimal(18,4),   
			@Total_Weight			decimal(18,4), 
			@DiscountType			int, 
			@QuantityDiscount		decimal(18,4), 
			@OrderDiscountType		int,   
			@OrderQuantityDiscount	decimal(18,4), 
			@CostUnit				int, 
			@FreightUnit			int, 
			@QuantityUnit			int,
			@PD1					decimal(9,4), 
			@PD2					decimal(9,4),   
			@PDU					int, 
			@MarkupPercent			decimal(18,4), 
			@LandedCost				decimal(18,4), 
			@UnitCost				decimal(18,4), 
			@UnitFreight			decimal(18,4),   
			@UnitExtCost			decimal(18,4), 
			@MUCost					decimal(18,4), 
			@MUFreight				decimal(18,4), 
			@IsCostUnitPackage		bit, 
			@IsCostedByWeight		bit,   
			@AdjustedCost			decimal(18,4), 
			@HandlingCharge			smallmoney,  
			@Freight3Party			money, 
			@Item_Key				int, 
			@OrderType_ID			int,               
			@UnitsReceived			decimal(18,4), 
			@OrderHeader_Id			int, 
			@Unit					int, 
			@CatchWeightRequired	bit

		--**************************************************************************
		--Set internal variables
		--**************************************************************************
		SELECT 
			@Vendor_ID				= OH.Vendor_ID, 
			@ReceiveLocation_ID		= ReceiveLocation_ID, 
			@Cost					= Cost, 
			@Freight				= Freight, 
			@DiscountType			= OI.DiscountType,      
			@QuantityDiscount		= OI.QuantityDiscount, 
			@CostUnit				= CostUnit, 
			@FreightUnit			= FreightUnit, 
			@QuantityUnit			= QuantityUnit,      
			@PD1					= OI.Package_Desc1, 
			@PD2					= OI.Package_Desc2, 
			@PDU					= OI.Package_Unit_ID, 
			@MarkupPercent			= MarkupPercent,      
			@QuantityOrdered		= QuantityOrdered, 
			@QuantityAllocated		= QuantityAllocated, 
			@QuantityReceived		= QuantityReceived, 
			@Total_Weight			= Total_Weight,      
			@UnitCost				= UnitCost, 
			@UnitFreight			= UnitExtCost - UnitCost,   
			@HandlingCharge			= OI.HandlingCharge,   
			@IsCostUnitPackage		= ISNULL(CU.IsPackageUnit, 0),
			@IsCostedByWeight		= CostedByWeight, 
			@AdjustedCost			= AdjustedCost,      
			@OrderDiscountType		= OH.DiscountType, 
			@OrderQuantityDiscount	= OH.QuantityDiscount, 
			@OrderType_ID			= OH.OrderType_ID,      
			@OrderHeader_Id			= OH.OrderHeader_ID, 
			@Freight3Party			= OI.Freight3Party, 
			@Item_Key				= OI.Item_Key,
			@CatchWeightRequired	= i.CatchWeightRequired,
			@AlreadyClosed			= 0 
		FROM 
			OrderHeader					(nolock) oh 
			INNER JOIN	OrderItem		(nolock) oi		ON OI.OrderHeader_ID	= OH.OrderHeader_ID      
			INNER JOIN  Item			(nolock) i		ON i.Item_Key			= OI.Item_Key      
			LEFT JOIN   ItemUnit		(nolock) cu		ON CU.Unit_ID			= OI.CostUnit      
		WHERE 
			OI.OrderItem_ID = @OrderItem_ID      
      
		--**************************************************************************
		--Set @QuantityReceived and @Total_Weight
		--**************************************************************************
		SELECT @QuantityReceived = @Quantity, @Total_Weight = @Weight         

		-- 4.8 - Refusal functionality is disabled until final requirements are delivered.
		--SELECT @QuantityRefused = 0
		--IF ISNULL(@Quantity, 0) > 1 AND @Quantity > @QuantityOrdered
		--	BEGIN
		--		SELECT @QuantityRefused = @Quantity - @QuantityOrdered 
		--		SELECT @QuantityReceived = @QuantityOrdered, @Total_Weight = @Weight
		--	END
		--ELSE
		--	SELECT @QuantityReceived = @Quantity, @Total_Weight = @Weight         
		
		--**************************************************************************
		--Set @PD1 based upon whether @Package_Desc1 is equal to @PD1
		--**************************************************************************
		IF (ISNULL(@Package_Desc1, @PD1) <> @PD1)      
			BEGIN      
				SELECT @PD1 = @Package_Desc1    
			END  

		--**************************************************************************
		--Declare and Set @WorkingQuantityOrdered
		--**************************************************************************
		DECLARE @WorkingQuantityOrdered decimal(18,4), @CCCost money  
		SET		@WorkingQuantityOrdered = ISNULL(@QuantityAllocated, @QuantityOrdered)  

		--**************************************************************************
		--Set @CCCost based upon @AdjustedCost value
		--**************************************************************************
		SET @CCCost =	CASE 
							WHEN ISNULL(@AdjustedCost, 0) > 0 THEN 
								@AdjustedCost 
							ELSE 
								@Cost 
						END  

		--**************************************************************************
		--Set @HandlingCharge based @OrderType_ID value
		--**************************************************************************
		IF @OrderType_ID = 2    
			BEGIN    
				SELECT @HandlingCharge = ISNULL(dbo.fn_GetCurrentHandlingCharge(@Item_Key, @Vendor_ID), @HandlingCharge)  
			END  

		--**************************************************************************
		--Call CalculateOrderItemCosts
		--**************************************************************************
		EXEC CalculateOrderItemCosts  
			@WorkingQuantityOrdered,  
			@QuantityUnit,  
			@QuantityReceived,  
			@Total_Weight,  
			@CCCost,  
			@CostUnit,  
			@OrderQuantityDiscount,  
			@OrderDiscountType,  
			@QuantityDiscount,  
			@DiscountType,  
			@Freight,  
			@FreightUnit,  
			@MarkupPercent,  
			@PD1,  
			@PD2,  
			@PDU,  
			@IsCostedByWeight, 
			@CatchWeightRequired, 
			@LandedCost				OUTPUT,  
			@MUCost					OUTPUT,  
			@LineItemCost			OUTPUT,  
			@LineItemFreight		OUTPUT,  
			@ReceivedItemCost		OUTPUT,  
			@ReceivedItemFreight	OUTPUT,  
			@UnitCost				OUTPUT,  
			@UnitExtCost			OUTPUT,  
			@HandlingCharge,  
			@Freight3Party  

		--**************************************************************************
		--Units received logic
		--**************************************************************************
		IF (@Total_Weight > 0) AND (@QuantityReceived > 0)  
			BEGIN  
				SET @UnitsReceived = @Total_Weight  
			END  
		ELSE  
			BEGIN  
				SET @Total_Weight = 0  
				SELECT @Unit =	CASE 
									WHEN @IsCostedByWeight = 1 THEN  
										(SELECT Unit_ID FROM ItemUnit (nolock) WHERE EDISysCode = 'LB') 
									ELSE 
										(SELECT Unit_ID FROM ItemUnit (nolock) WHERE EDISysCode = 'EA')
								END  
            
				EXEC CostConversion @QuantityReceived, @Unit, @QuantityUnit, @PD1, @PD2, @PDU, 0, 0, @UnitsReceived OUTPUT  
			END  

		--**************************************************************************
		--Main SQL
		--**************************************************************************
		UPDATE 
			OrderItem       
		SET 
			QuantityReceived	= @QuantityReceived,      
			Package_Desc1		= @PD1,      
			Total_Weight		= @Total_Weight,      
            ReceivedViaGun      = @ReceivedViaGun,
            Freight				= ISNULL(@Freight, 0),      
			LineItemCost		= ISNULL(@LineItemCost, 0),      
			LineItemFreight		= ISNULL(@LineItemFreight, 0),      
			ReceivedItemCost	= ISNULL(@ReceivedItemCost, 0),      
			ReceivedItemFreight = ISNULL(@ReceivedItemFreight, 0),      
			DateReceived		= ISNULL(DateReceived, @DateReceived),      
			UnitExtCost			= ISNULL(@UnitExtCost, 0),      
			UnitCost			= ISNULL(@UnitCost, 0),  
			UnitsReceived		= ISNULL(@UnitsReceived, 0) ,  
			HandlingCharge		= ISNULL(@HandlingCharge, 0),  
			LandedCost			= ISNULL(@LandedCost, 0),  
			MarkupCost			= ISNULL(@MUCost, 0), 
			ReceivingDiscrepancyReasonCodeID = @RecvDiscrepancyReasonID
	    FROM 
			OrderItem (rowlock)      
	    WHERE 
			OrderItem_ID = @OrderItem_ID                   
    
		--**************************************************************************
		--Receiving Item History
		--**************************************************************************
		EXEC InsertReceivingItemHistory @OrderItem_ID, @User_ID      

		--************************************************************************************
		--Delete any entry in OrderItemRefused table 
		--************************************************************************************
		-- 4.8 - Refusal functionality is disabled until final requirements are delivered.
		-- DELETE FROM OrderItemRefused WHERE OrderHeader_ID = @OrderHeader_Id AND OrderItem_ID = @OrderItem_ID 

		--************************************************************************************
		--Add an entry to OrderItemRefused table when @QuantityReceived > @QuantityOrdered
		--************************************************************************************
		-- 4.8 - Refusal functionality is disabled until final requirements are delivered.
		--IF @QuantityRefused > 0
		--	BEGIN
		--		DECLARE @Identifier varchar(13)
		--		DECLARE @VendorItemNumber varchar(255)
		--		DECLARE @EInvoice_ID int
		--		DECLARE @Description varchar(60)
		--		DECLARE @InvoiceCost money
		--		DECLARE @InvoiceQuantity decimal(18,4)
		--		DECLARE @UnitName varchar(25)
				
		--		SELECT 
		--			@Identifier = ii.Identifier 
		--		FROM 
		--			ItemIdentifier ii 
		--		WHERE ii.Item_Key = @Item_Key AND ii.Default_Identifier = 1
				
		--		SELECT 
		--			@UnitName = ii.Unit_Name
		--		FROM 
		--			ItemUnit ii
		--		WHERE ii.Unit_ID = @Unit
				
		--		SELECT @EInvoice_ID = oh.eInvoice_ID FROM OrderHeader oh WHERE oh.OrderHeader_ID = @OrderHeader_ID
				
		--		SELECT	
		--			@VendorItemNumber = ISNULL(ei.Vendor_Item_Num, '') 
		--		FROM 
		--			EInvoicing_Item ei 
		--		WHERE
		--			ei.EInvoice_ID = @EInvoice_ID AND ei.Item_Key = @Item_Key 
				 
		--		SELECT @Description = i.Item_Description FROM Item i WHERE i.Item_Key = @Item_Key

		--		SELECT 
		--			@InvoiceCost = oi.InvoiceCost,
		--			@InvoiceQuantity = oi.eInvoiceQuantity
		--		FROM
		--			OrderItem oi
		--		WHERE oi.OrderItem_ID = @OrderItem_ID
												
		--		EXEC dbo.InsertOrderItemRefused 
		--			@OrderHeader_ID, 
		--			@OrderItem_ID,
		--			@Identifier,
		--			@VendorItemNumber,
		--			@Description,
		--			@UnitName,
		--			@InvoiceQuantity,
		--			@InvoiceCost,
		--			@QuantityRefused,
		--			@RecvDiscrepancyReasonID,
		--			0,
		--			@EInvoice_ID						
					
		--	END	
		
		--EXEC dbo.UpdateOrderHeaderCosts @OrderHeader_ID				     
		
	END TRY  

	--**************************************************************************
	--SQL Error Catch
	--**************************************************************************
	BEGIN CATCH  
        DECLARE @err_no int, @err_sev int, @err_msg nvarchar(4000)  
        SELECT @err_no = ERROR_NUMBER(), @err_sev = ERROR_SEVERITY(), @err_msg = ERROR_MESSAGE()  
        RAISERROR ('ReceiveOrderItem3 failed with @@ERROR: %d - %s', @err_sev, 1, @err_no, @err_msg)  
    END CATCH  
      
    SET NOCOUNT OFF  
END
GO
GRANT EXECUTE
    ON OBJECT::[dbo].[ReceiveOrderItem3] TO [IRMASupportRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[ReceiveOrderItem3] TO [IRMAClientRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[ReceiveOrderItem3] TO [IRMASchedJobsRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[ReceiveOrderItem3] TO [IRMAReportsRole]
    AS [dbo];

