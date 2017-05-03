CREATE PROCEDURE dbo.InsertItemHistory 
    @Store_No int, 
    @Item_Key int, 
    @DateStamp datetime, 
    @Quantity decimal(18,4), 
    @Weight decimal(18,4), 
    @ExtCost smallmoney,
    @Adjustment_ID int, 
    @AdjustmentReason varchar(100), 
    @CreatedBy int, 
    @SubTeam_No int,
    @OrderItem_ID int
AS 

--Update History

--7407			10/01/2012	Alex B		Addedd logic fir Queuing Receiving during sales load

BEGIN
    SET NOCOUNT ON

    DECLARE @error_no int
    SELECT @error_no = 0
	DECLARE @JobStatus varchar(50)
	select @JobStatus= dbo.fn_GetJobStatus('TLOGSalesLoadJob')

    BEGIN TRAN

    IF @SubTeam_No IS NULL
    BEGIN
        SELECT @SubTeam_No = SubTeam_No FROM Item (nolock) WHERE Item_Key = @Item_Key

        SELECT @error_no = @@ERROR
    END

    IF (@error_no = 0) AND (EXISTS (SELECT * FROM Shipper (nolock) WHERE Shipper_Key = @Item_Key))
		-- If Jobstatus is Queuing(Sales load is running) and Adjustment_ID = 5(Receiving) inserts will go to ItemHistoryQueue otherwise - to ItemHistory
    BEGIN
	 IF (@JobStatus='Queueing') and (@Adjustment_ID=5)
        INSERT INTO ItemHistoryQueue (Store_No, Item_Key, DateStamp, Quantity, Weight, ExtCost, Retail, 
                                 Adjustment_ID, AdjustmentReason, CreatedBy, SubTeam_No, OrderItem_ID)
        SELECT @Store_No, Shipper.Item_Key, @DateStamp, @Quantity * Shipper.Quantity, 0, 
               @ExtCost, 
               (SELECT TOP 1 dbo.fn_Price(PriceChgTypeId, Multiple, Price, PricingMethod_ID, Sale_Multiple, Sale_Price)
                FROM PriceHistory (nolock)
                WHERE Item_Key = Shipper.Item_Key AND Store_No = @Store_No AND  Effective_Date <= @DateStamp
                ORDER BY PriceHistory.Effective_Date DESC), 
               @Adjustment_ID, @AdjustmentReason, @CreatedBy, @SubTeam_No, @OrderItem_ID
        FROM Shipper (nolock)
        WHERE Shipper.Shipper_Key = @Item_Key
   ELSE
        INSERT INTO ItemHistory (Store_No, Item_Key, DateStamp, Quantity, Weight, ExtCost, Retail, 
                                 Adjustment_ID, AdjustmentReason, CreatedBy, SubTeam_No, OrderItem_ID)
        SELECT @Store_No, Shipper.Item_Key, @DateStamp, @Quantity * Shipper.Quantity, 0, 
               @ExtCost, 
               (SELECT TOP 1 dbo.fn_Price(PriceChgTypeId, Multiple, Price, PricingMethod_ID, Sale_Multiple, Sale_Price)
                FROM PriceHistory (nolock)
                WHERE Item_Key = Shipper.Item_Key AND Store_No = @Store_No AND  Effective_Date <= @DateStamp
                ORDER BY PriceHistory.Effective_Date DESC), 
               @Adjustment_ID, @AdjustmentReason, @CreatedBy, @SubTeam_No, @OrderItem_ID
        FROM Shipper (nolock)
        WHERE Shipper.Shipper_Key = @Item_Key

        SELECT @error_no = @@ERROR
    END
    ELSE
    BEGIN
        DECLARE @Price money

        SELECT TOP 1 @Price = dbo.fn_Price(PriceChgTypeId, Multiple, Price, PricingMethod_ID, Sale_Multiple, Sale_Price)                
        FROM PriceHistory (nolock)
        WHERE Item_Key = @Item_Key AND Store_No = @Store_No AND 
              Effective_Date <= @DateStamp
        ORDER BY PriceHistory.Effective_Date DESC

        SELECT @error_no = @@ERROR

        IF @error_no = 0
			-- If Jobstatus is Queuing(Sales load is running) and Adjustment_ID = 5(Receiving) inserts will go to ItemHistoryQueue otherwise - to ItemHistory
        BEGIN
		IF (@JobStatus='Queueing') and (@Adjustment_ID=5)
            INSERT INTO ItemHistoryQueue (Store_No, Item_Key, DateStamp, Quantity, Weight, ExtCost, Retail,
                                     Adjustment_ID, AdjustmentReason, CreatedBy, SubTeam_No, OrderItem_ID)
            VALUES (@Store_No, @Item_Key, @DateStamp, @Quantity, @Weight, @ExtCost, 
                    @Price, @Adjustment_ID, @AdjustmentReason, @CreatedBy, @SubTeam_No, @OrderItem_ID)
		ELSE
            INSERT INTO ItemHistory (Store_No, Item_Key, DateStamp, Quantity, Weight, ExtCost, Retail,
                                     Adjustment_ID, AdjustmentReason, CreatedBy, SubTeam_No, OrderItem_ID)
            VALUES (@Store_No, @Item_Key, @DateStamp, @Quantity, @Weight, @ExtCost, 
                    @Price, @Adjustment_ID, @AdjustmentReason, @CreatedBy, @SubTeam_No, @OrderItem_ID)
    
            SELECT @error_no = @@ERROR
        END
    
    -- 6.30.08 - RS - Added two UOM fields to ItemCaseHistory to support new Current On Hand panel and
    -- catchwreight requirements
      
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
        RAISERROR ('InsertItemHistory failed with @@ERROR: %d', @Severity, 1, @error_no)
    END
END
GO
GRANT EXECUTE
    ON OBJECT::[dbo].[InsertItemHistory] TO [IRMAClientRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[InsertItemHistory] TO [IRMASchedJobsRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[InsertItemHistory] TO [IRMAReportsRole]
    AS [dbo];

