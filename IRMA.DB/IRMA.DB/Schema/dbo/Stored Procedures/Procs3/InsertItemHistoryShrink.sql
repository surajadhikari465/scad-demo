CREATE PROCEDURE [dbo].[InsertItemHistoryShrink] 
    @Store_No int, 
    @Item_Key int, 
    @Quantity decimal(18,4), 
    @Weight decimal(18,4), 
    @Adjustment_ID int, 
    @AdjustmentReason varchar(100), 
    @CreatedBy int, 
    @SubTeam_No int,
	@InventoryAdjustmentCode varchar(3),
	@Username varchar(25)
AS 
-- **************************************************************************
-- Procedure: InsertItemHistoryShrink()
--    Author: n/a
--      Date: n/a
--
-- Description:
-- 
--
-- Modification History:
-- Date       	Init  	TFS   	Comment
-- 09/22/2010	BBB   	13534	Removed hard-coded value treatment
-- 10/03/2012	AlexB		Removed all references to ItemCaseHistory
-- **************************************************************************
BEGIN
    SET NOCOUNT ON

    DECLARE @error_no int
	DECLARE @ItemHistoryId int
	DECLARE @InventoryAdjustmentCode_ID int

	DECLARE @PackSize decimal(9,4)
    
	SELECT @error_no = 0

	Select @PackSize = dbo.fn_GetCurrentVendorPackage_Desc1(@Item_Key, @Store_No)

	Select @InventoryAdjustmentCode_ID = InventoryAdjustmentCode_ID from InventoryAdjustmentCode where Abbreviation = @InventoryAdjustmentCode

    BEGIN TRAN
    
	IF @CreatedBy IS NULL
	BEGIN
		SELECT @CreatedBy = User_ID FROM Users WHERE Username = @Username
		
		SELECT @error_no = @@ERROR
	END
	
    IF @SubTeam_No IS NULL
    BEGIN
        SELECT @SubTeam_No = SubTeam_No FROM Item (nolock) WHERE Item_Key = @Item_Key

        SELECT @error_no = @@ERROR
    END

    IF (@error_no = 0) AND (EXISTS (SELECT * FROM Shipper (nolock) WHERE Shipper_Key = @Item_Key))
    BEGIN
        INSERT INTO ItemHistory (Store_No, Item_Key, DateStamp, Quantity, Weight, ExtCost, Retail, 
                                 Adjustment_ID, AdjustmentReason, CreatedBy, SubTeam_No, OrderItem_ID, InventoryAdjustmentCode_ID)
        SELECT @Store_No, Shipper.Item_Key, GetDate(), @Quantity * Shipper.Quantity, 0, 
               0, 
               (SELECT TOP 1 dbo.fn_Price(PriceChgTypeId, Multiple, Price, PricingMethod_ID, Sale_Multiple, Sale_Price)
                FROM PriceHistory (nolock)
                WHERE Item_Key = Shipper.Item_Key AND Store_No = @Store_No AND  Effective_Date <= GetDate()
                ORDER BY PriceHistory.Effective_Date DESC), 
               @Adjustment_ID, @AdjustmentReason, @CreatedBy, @SubTeam_No, null, @InventoryAdjustmentCode_ID
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
              Effective_Date <= GetDate()
        ORDER BY PriceHistory.Effective_Date DESC

        SELECT @error_no = @@ERROR

        IF @error_no = 0
        BEGIN
            INSERT INTO ItemHistory (Store_No, Item_Key, DateStamp, Quantity, Weight, ExtCost, Retail,
                                     Adjustment_ID, AdjustmentReason, CreatedBy, SubTeam_No, OrderItem_ID, 
										InventoryAdjustmentCode_ID)
            VALUES (@Store_No, @Item_Key, GetDate(), @Quantity, @Weight, 0, 
                    @Price, @Adjustment_ID, @AdjustmentReason, @CreatedBy, @SubTeam_No, null, @InventoryAdjustmentCode_ID)
    
            SELECT @error_no = @@ERROR, @ItemHistoryId = SCOPE_IDENTITY()
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
        RAISERROR ('InsertItemHistoryShrink failed with @@ERROR: %d', @Severity, 1, @error_no)
    END
END
GO
GRANT EXECUTE
    ON OBJECT::[dbo].[InsertItemHistoryShrink] TO [IRMAAdminRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[InsertItemHistoryShrink] TO [IRMAClientRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[InsertItemHistoryShrink] TO [IRMASchedJobsRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[InsertItemHistoryShrink] TO [IRMAReportsRole]
    AS [dbo];

