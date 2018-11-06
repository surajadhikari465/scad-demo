SET QUOTED_IDENTIFIER ON 
GO
SET ANSI_NULLS ON 
GO

if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].InsertItemHistory2') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[InsertItemHistory2]
GO


CREATE PROCEDURE dbo.InsertItemHistory2 
    @Store_No int, 
    @Identifier varchar(255), 
    @DateStamp datetime, 
    @Quantity decimal(18,4),
    @PackSize decimal(9,4),         -- @PackSize > 0 implies @Quantity in Cases; otherwise, @Quantity in units
    @Weight decimal(18,4),
    @Adjustment_ID int, 
    @AdjustmentReason varchar(100), 
    @CreatedBy int, 
    @OrderItem_ID int
AS 
--Update History

--7407			10/01/2012	Alex B		Addedd logic fir Queuing Receiving during sales load
BEGIN  
    SET NOCOUNT ON    
    
    DECLARE @error_no int    
    SELECT @error_no = 0    
    
 DECLARE @AdjustmentType int   
 set @AdjustmentType = (select adjustment_type from itemadjustment (nolock) where adjustment_id = @adjustment_id)  
 DECLARE @JobStatus varchar(50)
 select @JobStatus= dbo.fn_GetJobStatus('TLOGSalesLoadJob')
  
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
		-- If Jobstatus is Queuing(Sales load is running) and Adjustment_ID = 5(Receiving) inserts will go to ItemHistoryQueue otherwise - to ItemHistory
    BEGIN    	
    IF (@JobStatus='Queueing') and (@Adjustment_ID=5)
        INSERT INTO ItemHistoryQueue (Store_No, Item_Key, DateStamp, Quantity, Weight, ExtCost, Retail,     
                                 Adjustment_ID, AdjustmentReason, CreatedBy, SubTeam_No, OrderItem_ID)    
        SELECT @Store_No, Shipper.Item_Key, @DateStamp, @Quantity * Shipper.Quantity, 0,     
  ISNULL(dbo.fn_AvgCostHistory(Shipper.Item_Key, @Store_No, @SubTeam_No, @DateStamp), 0),     
  ISNULL(dbo.fn_PriceHistoryPrice(Shipper.Item_Key, @Store_No, @DateStamp), 0),    
               @Adjustment_ID, @AdjustmentReason, @CreatedBy, @SubTeam_No, @OrderItem_ID    
        FROM Shipper (nolock)    
        WHERE Shipper.Shipper_Key = @Item_Key    
    ELSE
        INSERT INTO ItemHistory (Store_No, Item_Key, DateStamp, Quantity, Weight, ExtCost, Retail,     
                                 Adjustment_ID, AdjustmentReason, CreatedBy, SubTeam_No, OrderItem_ID)    
        SELECT @Store_No, Shipper.Item_Key, @DateStamp, @Quantity * Shipper.Quantity, 0,     
  ISNULL(dbo.fn_AvgCostHistory(Shipper.Item_Key, @Store_No, @SubTeam_No, @DateStamp), 0),     
  ISNULL(dbo.fn_PriceHistoryPrice(Shipper.Item_Key, @Store_No, @DateStamp), 0),    
               @Adjustment_ID, @AdjustmentReason, @CreatedBy, @SubTeam_No, @OrderItem_ID    
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
			-- If Jobstatus is Queuing(Sales load is running) and Adjustment_ID = 5(Receiving) inserts will go to ItemHistoryQueue otherwise - to ItemHistory
        BEGIN    
	    IF (@JobStatus='Queueing') and (@Adjustment_ID=5)
            INSERT INTO ItemHistoryQueue (Store_No, Item_Key, DateStamp, Quantity, Weight, ExtCost, Retail,     
                                     Adjustment_ID, AdjustmentReason, CreatedBy, SubTeam_No, OrderItem_ID)    
            VALUES (@Store_No, @Item_Key, @DateStamp, @CalcQty, @CalcWeight,     
             @AvgCost, @Price, @Adjustment_ID, @AdjustmentReason, @CreatedBy, @SubTeam_No, @OrderItem_ID) 
        ELSE
            INSERT INTO ItemHistory (Store_No, Item_Key, DateStamp, Quantity, Weight, ExtCost, Retail,     
                                     Adjustment_ID, AdjustmentReason, CreatedBy, SubTeam_No, OrderItem_ID)    
            VALUES (@Store_No, @Item_Key, @DateStamp, @CalcQty, @CalcWeight,     
             @AvgCost, @Price, @Adjustment_ID, @AdjustmentReason, @CreatedBy, @SubTeam_No, @OrderItem_ID)    
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
SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO

