IF EXISTS (SELECT * FROM sysobjects WHERE type = 'P' AND name = 'CalculateOrderItemCosts')
	BEGIN
		DROP  Procedure  dbo.CalculateOrderItemCosts
	END
GO

CREATE Procedure dbo.CalculateOrderItemCosts
	@QuantityOrdered		decimal(18,4),  
	@QuantityUnit			int,  
	@QuantityReceived		decimal(18,4),  
	@Total_Weight			decimal(18,4),  
	@Cost					money,  
	@CostUnit				int,  
	@OrderQuantityDiscount	decimal(18,4),	-- OrderHeader  
	@OrderDiscountType		int,			-- OrderHeader  
	@ItemQuantityDiscount	decimal(18,4),	-- OrderItem  
	@ItemDiscountType		int,			-- OrderItem  
	@Freight				decimal(18,4),  
	@FreightUnit			int,			-- This is not used currently since it is always the same as CostUnit per the UI rules  
	@MarkupPercent			decimal(18,4),  
	@Package_Desc1			decimal(9,4),  
	@Package_Desc2			decimal(9,4),  
	@Package_Unit_ID		int,  
	@IsCostedByWeight		bit,  
	@CatchWeightRequired	bit,
	@LandedCost				money OUTPUT,  
	@MarkupCost				money OUTPUT,  
	@LineItemCost			money OUTPUT,  
	@LineItemFreight		money OUTPUT,  
	@ReceivedItemCost		money OUTPUT,  
	@ReceivedItemFreight	money OUTPUT,  
	@UnitCost				money OUTPUT,  
	@UnitExtCost			money OUTPUT,  
	@MarkupDollars			smallmoney,  
	@Freight3Party			money  
AS  
-- **************************************************************************
-- Procedure: CalculateOrderItemCosts()
--    Author: n/a
--      Date: n/a
--
-- Description:
-- Called by UpdateOrderRefreshCost SP to provide line item cost calculation
--
-- Modification History:
-- Date       	Init  	TFS   	Comment
-- 11/29/2010	BBB   	13604	Added conditional call to CalculateOrderDiscountForLineItem
--								in the instance of OrderType 1; added condition
--								to treat value based upon case or unit cost; added 
--								new variable CashDiscountAmount;
-- 2011/12/27	KM		3744	Code formatting;
-- 2013/01/25   MZ      9822    Updated the Unit Cost conversion for random-weight items
-- **************************************************************************
  
SET NOCOUNT ON  
  
DECLARE 
	@Unit					int,   
	@WorkingCost			decimal(38,28),   
	@WorkingFreight			decimal(38,28),   
	@CCCost					decimal(38,28),   
	@UnitsReceived			decimal(18,4),  
	@WorkingUnitCost		decimal(38,28),  
	@WorkingUnitExtCost		decimal(38,28),
	@CashDiscountAmount		decimal(18,4),
	@IsCostUnitWeight       bit, 
	@IsPackageUnitWeight    bit
 
SELECT @IsCostUnitWeight = Weight_Unit
  FROM ItemUnit WITH (nolock)
 WHERE Unit_ID = @CostUnit

SELECT @IsPackageUnitWeight = Weight_Unit
  FROM ItemUnit WITH (nolock)
 WHERE Unit_ID = @Package_Unit_ID
	          
IF @Total_Weight IS NULL  
    SET @Total_Weight = 0  
 
IF @ItemDiscountType = 1  
	SELECT @CashDiscountAmount = @ItemQuantityDiscount, @ItemQuantityDiscount = @ItemQuantityDiscount * ISNULL(@QuantityReceived, @QuantityOrdered) 
      
IF @QuantityReceived IS NULL  
    SET @QuantityReceived = 0  
     
IF (@Total_Weight > 0) AND (@QuantityReceived = 0)  
    SET @Total_Weight = 0  
  
SELECT @Unit =	CASE 
					WHEN @IsCostedByWeight = 1 AND @IsCostUnitWeight = 1
						THEN @CostUnit		
					WHEN @IsCostedByWeight = 1 AND @IsCostUnitWeight = 0 AND @IsPackageUnitWeight = 1		
						THEN  @Package_Unit_ID
					WHEN @IsCostedByWeight = 1 AND @IsCostUnitWeight = 0 AND @IsPackageUnitWeight = 0	
						THEN (SELECT Unit_ID FROM ItemUnit (nolock) WHERE EDISysCode = 'LB') 
					ELSE 
						(SELECT Unit_ID FROM ItemUnit (nolock) WHERE EDISysCode = 'EA') 
				END  
  
SET @WorkingCost	= @Cost  
SET @WorkingFreight = @Freight  
  
  
-- Markup  
--  Convert to quantity unit first in case we are using markup dollars, which applies to the quantity unit  
SET @CCCost = @WorkingCost  
EXEC CostConversion @CCCost, @CostUnit, @QuantityUnit, @Package_Desc1, @Package_Desc2, @Package_Unit_ID, 0, 0, @NewAmount = @WorkingCost OUTPUT  
  
-- Mark up  
IF @MarkupDollars IS NULL  
    -- Apply the markup percent to the working cost  
	SET @WorkingCost = @WorkingCost * (100 + ISNULL(@MarkupPercent, 0)) / 100  
  
SET @CCCost = @WorkingFreight  
EXEC CostConversion @CCCost, @CostUnit, @QuantityUnit, @Package_Desc1, @Package_Desc2, @Package_Unit_ID, 0, 0, @NewAmount = @WorkingFreight OUTPUT  
  
  
-- Ordered Line Item totals  
SET @LineItemCost = (@WorkingCost + ISNULL(@MarkupDollars, 0)) * (@QuantityOrdered - CASE WHEN @ItemDiscountType = 3 THEN @ItemQuantityDiscount ELSE 0 END)  
IF @LineItemCost < 0  
    SET @LineItemCost = 0  
  
SET @LineItemFreight = @WorkingFreight * @QuantityOrdered  
  
-- Apply Percent/Cash Discounts to the line item totals  
IF @ItemDiscountType = 1  
	BEGIN 
		IF (SELECT IsPackageUnit FROM ItemUnit WHERE Unit_ID = @QuantityUnit) = 0
			BEGIN 
				SET @LineItemCost = (((@WorkingCost * @Package_Desc1) - @CashDiscountAmount) / @Package_Desc1) * @QuantityOrdered
			END
		ELSE
			SET @LineItemCost = (@WorkingCost - @CashDiscountAmount) * @QuantityOrdered
	END
ELSE
	BEGIN
		EXEC CalculateOrderDiscountForLineItem @OrderDiscountType, @OrderQuantityDiscount, @ItemDiscountType, @ItemQuantityDiscount, @LineItemCost OUTPUT, @LineItemFreight OUTPUT  
	END
  
-- Unit costs  
EXEC CostConversion @WorkingCost, @QuantityUnit, @Unit, @Package_Desc1, @Package_Desc2, @Package_Unit_ID, @Total_Weight, @QuantityReceived, @NewAmount = @WorkingUnitCost OUTPUT  
SET @UnitCost = @WorkingUnitCost  
  
IF @WorkingFreight > 0  
	BEGIN  
		SET @CCCost = @WorkingCost + @WorkingFreight                      
		EXEC CostConversion @CCCost, @QuantityUnit, @Unit, @Package_Desc1, @Package_Desc2, @Package_Unit_ID, @Total_Weight, @QuantityReceived, @NewAmount = @WorkingUnitExtCost OUTPUT  
		SET @UnitExtCost = @WorkingUnitExtCost  
	END  
ELSE
	SET @UnitExtCost = @UnitCost  

-- 3rd Party Freight should be included in Landed Cost.  
print '# Calculate Landed Cost'
print '  CatchWeightRequired: ' + cast(@CatchWeightRequired as varchar(100))
print '  WorkingCost:		  ' + cast(@WorkingCost as varchar(100))
print '  Total_Weight:        ' + cast(@Total_Weight as varchar(100))
print '  QuantityReceived:    ' + cast(@QuantityReceived as varchar(100))
print '  WorkingFreight:      ' + cast(@WorkingFreight as varchar(100))
print '  Freight3Party:       ' + cast(@Freight3Party as varchar(100))
print '  UnitExtcost:		  ' + cast(@UnitExtCost as varchar(100))

-- Landed Cost  

--  Convert to cost what is on the cost record first
IF @CatchWeightRequired=1
	SET @CCCost =	CASE 
						WHEN @QuantityReceived = 0 THEN 0 
						ELSE (ISNULL(@UnitExtCost,0) * ISNULL(@Total_Weight,0)) / ISNULL(@QuantityReceived,1) 
					END 
ELSE
	EXEC CostConversion @WorkingCost, @QuantityUnit, @CostUnit, @Package_Desc1, @Package_Desc2, @Package_Unit_ID, 0, 0, @NewAmount = @CCCost OUTPUT
	
print '  Pre-Conversion:      ' + cast(@CCCost AS varchar(100))
EXEC CostConversion @CCCost, @CostUnit, @QuantityUnit, @Package_Desc1, @Package_Desc2, @Package_Unit_ID, 0, 0, @NewAmount = @LandedCost OUTPUT  
print '  Post-Conversion:     ' + cast(@LandedCost AS varchar(100))


SET @MarkupCost = @LandedCost + @WorkingFreight + ISNULL(@MarkupDollars, 0)  
print '  Markup Cost:     ' + cast(@markupCost AS varchar(100))


-- Units received  
IF @Total_Weight > 0  
    SET @UnitsReceived = @Total_Weight  
ELSE                    
    EXEC CostConversion @QuantityReceived, @Unit, @QuantityUnit, @Package_Desc1, @Package_Desc2, @Package_Unit_ID, 0, 0, @UnitsReceived OUTPUT  
      
-- Received Line Item Totals  
SELECT @ReceivedItemCost = @WorkingUnitCost * @UnitsReceived -	(CASE 
																			WHEN @ItemDiscountType = 3 THEN @ItemQuantityDiscount 
																			ELSE 0 
																		END * @WorkingCost) + (@QuantityReceived * ISNULL(@MarkupDollars, 0))
IF @ReceivedItemCost < 0  
    SET @ReceivedItemCost = 0  
  
SET @ReceivedItemFreight = (ISNULL(@WorkingUnitExtCost, 0) - @WorkingUnitCost) * @UnitsReceived  
  
-- Apply Percent/Cash Discounts to the received line item totals  
IF @QuantityReceived > 0  
    EXEC CalculateOrderDiscountForLineItem @OrderDiscountType, @OrderQuantityDiscount, @ItemDiscountType, @ItemQuantityDiscount, @ReceivedItemCost OUTPUT, @ReceivedItemFreight OUTPUT  
  
SET NOCOUNT OFF
GO