
/****** Object:  StoredProcedure [dbo].[InsertItemHistory4]    Script Date: 10/04/2012 15:27:49 ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[InsertItemHistory4]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[InsertItemHistory4]
GO


/****** Object:  StoredProcedure [dbo].[InsertItemHistory4]    Script Date: 10/04/2012 15:27:49 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE PROCEDURE [dbo].[InsertItemHistory4] 
    @Store_No int, 
    @Identifier varchar(255), 
    @DateStamp datetime, 
    @Quantity decimal(18,4), 
    @Weight decimal(18,4), 
    @InventoryAdjustmentCode_Id int, 
    @CreatedBy int, 
    @PackSize decimal(9,4)
AS 

   -- Modification History:
   -- Date			Init		Comment
   -- 10/03/2012	AlexB		Removed all references to ItemCaseHistory
BEGIN  
    SET NOCOUNT ON    
    
    DECLARE @error_no int    
    SELECT @error_no = 0    
    
    DECLARE @Adjustment_Id int,
	@AdjustmentReason varchar(100)
		
   SELECT @Adjustment_Id = Adjustment_Id,
		@AdjustmentReason = AdjustmentDescription
	FROM InventoryAdjustmentCode 
	WHERE InventoryAdjustmentCode_Id = @InventoryAdjustmentCode_Id
    
    BEGIN TRAN    
    
 DECLARE @Item_Key int, @SubTeam_No int    
    
    SELECT @Item_Key = II.Item_Key,    
           @SubTeam_No = SubTeam_No    
    FROM ItemIdentifier II (nolock)    
    INNER JOIN    
        Item (nolock)    
        ON Item.Item_Key = II.Item_Key    
    WHERE Deleted_Identifier = 0 AND Identifier = @Identifier    
    
 SELECT @error_no = @@ERROR    
    
    IF (@error_no = 0) AND (EXISTS (SELECT * FROM Shipper (nolock) WHERE Shipper_Key = @Item_Key))    
    BEGIN    
        INSERT INTO ItemHistory (Store_No, Item_Key, DateStamp, Quantity, Weight, ExtCost, Retail,     
                                 Adjustment_ID, AdjustmentReason, CreatedBy, SubTeam_No, InventoryAdjustmentCode_Id)    
        SELECT @Store_No, Shipper.Item_Key, @DateStamp, @Quantity * Shipper.Quantity, 0,     
  ISNULL(dbo.fn_AvgCostHistory(Shipper.Item_Key, @Store_No, @SubTeam_No, @DateStamp), 0),     
  ISNULL(dbo.fn_PriceHistoryPrice(Shipper.Item_Key, @Store_No, @DateStamp), 0),    
               @Adjustment_ID, @AdjustmentReason, @CreatedBy, @SubTeam_No,  @InventoryAdjustmentCode_Id
        FROM Shipper (nolock)    
        WHERE Shipper.Shipper_Key = @Item_Key    
    
        SELECT @error_no = @@ERROR    
    END    
    ELSE    
    BEGIN    
        DECLARE @AvgCost money, @Price money    
    
        SELECT TOP 1 @AvgCost = ISNULL(AH.AvgCost, 0),    
                     @Price = dbo.fn_Price(PriceChgTypeId, Multiple, Price, PricingMethod_ID, Sale_Multiple, Sale_Price)                    
        FROM PriceHistory PH (nolock)    
  LEFT OUTER JOIN AvgCostHistory AH (nolock)     
  ON AH.Item_Key = @Item_Key AND AH.Store_No = @Store_No AND AH.SubTeam_No = @SubTeam_No    
        WHERE PH.Item_Key = @Item_Key AND PH.Store_No = @Store_No AND     
              PH.Effective_Date <= @DateStamp    
        ORDER BY PH.Effective_Date DESC    
    
        SELECT @error_no = @@ERROR    
    
  
  Declare  @CalcWeight as decimal(18,4)  
  Declare  @CalcQty as decimal(18,4)  
  Declare @CostedByWeight as int  
  
  set @CostedByWeight=(select CostedByWeight from item where item_key = @Item_Key)  
  
  if @CostedByWeight = 1 and @Weight <> 0 
  begin  
   set @CalcWeight = @Quantity * @Weight   
   set @CalcQty = 0  
  end  
  else if @CostedByWeight = 1 and @Weight = 0 
  begin  
   set @CalcWeight = @Quantity * @Packsize   
   set @CalcQty = 0  
  end  
  else
  begin  
   set @CalcWeight = 0  
   set @CalcQty = @Quantity * @PackSize   
  end  
  
    
  
        IF @error_no = 0    
        BEGIN    
            INSERT INTO ItemHistory (Store_No, Item_Key, DateStamp, Quantity, Weight, ExtCost, Retail,     
                                     Adjustment_ID, AdjustmentReason, CreatedBy, SubTeam_No, InventoryAdjustmentCode_Id)    
            VALUES (@Store_No, @Item_Key, @DateStamp, @CalcQty, @CalcWeight,     
             @AvgCost, @Price, @Adjustment_ID, @AdjustmentReason, @CreatedBy, @SubTeam_No, @InventoryAdjustmentCode_Id)    
--- ############ changed on 10/12/08 bug 7490  
--            VALUES (@Store_No, @Item_Key, @DateStamp, @Quantity * CASE WHEN @PackSize > 0 THEN @PackSize ELSE 1 END, @Weight,     
--             @AvgCost, @Price,     
--                   @Adjustment_ID, @AdjustmentReason, @CreatedBy, @SubTeam_No, @OrderItem_ID)    
            
            SELECT @error_no = @@ERROR    
        END    
        
       
    END    
        
    SET NOCOUNT OFF    
    
    IF @error_no = 0    
     COMMIT TRAN    
    ELSE    
    BEGIN    
        IF @@TRANCOUNT <> 0    
            ROLLBACK TRAN    
        DECLARE @Severity smallint    
        SELECT @Severity = ISNULL((SELECT severity FROM master.dbo.sysmessages WHERE error = @error_no), 16)    
        RAISERROR ('InsertItemHistory2 failed with @@ERROR: %d', @Severity, 1, @error_no)    
    END    
END
GO


