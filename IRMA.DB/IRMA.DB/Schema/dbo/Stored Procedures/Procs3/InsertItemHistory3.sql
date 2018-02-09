CREATE PROCEDURE dbo.InsertItemHistory3 
    @Store_No int, 
    @Item_Key int, 
    @DateStamp datetime, 
    @Quantity decimal(18,4), 
    @Weight decimal(18,4), 
    @InventoryAdjustmentCode_Id int, 
    @CreatedBy int, 
    @SubTeam_No int,
    @PackSize decimal(9,4),
    @ShrinkSubtype_Id int
AS 

   -- Modification History:
   -- Date			Init		Comment
   -- 10/03/2012	AlexB		Removed all references to ItemCaseHistory
   --  2/05/2018    EM          Added ShrinkSubtype_Id parameter
BEGIN
    SET NOCOUNT ON

	DECLARE @Adjustment_Id int,
		@AdjustmentReason varchar(100)

	DECLARE @Abbreviation varchar(3) 

    DECLARE @error_no int, @ItemHistoryId int
    SELECT @error_no = 0

	BEGIN TRAN

	SELECT @Adjustment_Id = Adjustment_Id,
		@Abbreviation = Abbreviation,
		@AdjustmentReason = AdjustmentDescription
	FROM InventoryAdjustmentCode 
	WHERE InventoryAdjustmentCode_Id = @InventoryAdjustmentCode_Id

    IF @SubTeam_No IS NULL
    BEGIN
        SELECT @SubTeam_No = SubTeam_No FROM Item (nolock) WHERE Item_Key = @Item_Key
        SELECT @error_no = @@ERROR
    END

    IF (@error_no = 0) AND (EXISTS (SELECT * FROM Shipper (nolock) WHERE Shipper_Key = @Item_Key))
    BEGIN
        INSERT INTO ItemHistory (Store_No, Item_Key, DateStamp, Quantity, Weight, ExtCost, Retail, 
                                 Adjustment_ID, AdjustmentReason, CreatedBy, SubTeam_No, InventoryAdjustmentCode_Id)
        SELECT @Store_No, Shipper.Item_Key, @DateStamp, @Quantity * Shipper.Quantity, 0, 
		ISNULL(dbo.fn_AvgCostHistory(Shipper.Item_Key, @Store_No, @SubTeam_No, @DateStamp), 0), 
		ISNULL(dbo.fn_PriceHistoryPrice(Shipper.Item_Key, @Store_No, @DateStamp), 0),
               @Adjustment_ID, @AdjustmentReason, @CreatedBy, @SubTeam_No, @InventoryAdjustmentCode_Id
        FROM Shipper (nolock)
        WHERE Shipper.Shipper_Key = @Item_Key

        SELECT @error_no = @@ERROR, @ItemHistoryId = SCOPE_IDENTITY()
    END
    ELSE
    BEGIN
        DECLARE @AvgCost money, @Price money
    
        SELECT TOP 1 @AvgCost = ISNULL(AH.AvgCost, 0),
                     @Price = dbo.fn_Price(PriceChgTypeID, Multiple, Price, PricingMethod_ID, Sale_Multiple, Sale_Price)
        FROM PriceHistory PH (nolock)
		LEFT OUTER JOIN AvgCostHistory AH (nolock) 
		ON AH.Item_Key = @Item_Key AND AH.Store_No = @Store_No AND AH.SubTeam_No = @SubTeam_No
        WHERE PH.Item_Key = @Item_Key AND PH.Store_No = @Store_No AND PH.Effective_Date <= @DateStamp
        ORDER BY PH.Effective_Date DESC

        SELECT @error_no = @@ERROR

        IF @error_no = 0
        BEGIN
            INSERT INTO ItemHistory (Store_No, Item_Key, DateStamp, Quantity, Weight, ExtCost, Retail, Adjustment_ID, AdjustmentReason, CreatedBy, SubTeam_No, InventoryAdjustmentCode_Id)
            VALUES (@Store_No, @Item_Key, @DateStamp, @Quantity, @Weight, @AvgCost, @Price, @Adjustment_ID, @AdjustmentReason, @CreatedBy, @SubTeam_No, @InventoryAdjustmentCode_Id)
        
			SELECT @error_no = @@ERROR, @ItemHistoryId = SCOPE_IDENTITY()
        END
    END

	IF (@Abbreviation <> 'SP' and @Abbreviation <> 'SM' and @Abbreviation <> 'FB')
	BEGIN
		INSERT INTO ItemHistoryUpload (ItemHistoryId, AccountingUploadDate)
		VALUES (@ItemHistoryId, NULL)
	END
	
	-- if a shrink subtype is specified in addition to the main Adjustment Code, record it
	IF (@error_no = 0 AND @ItemHistoryId > 0 AND @ShrinkSubtype_Id > 0) 
	BEGIN
		INSERT INTO dbo.ItemHistoryShrinkSubType (ShrinkSubType_ID, ItemHistoryID, AddedDate)
			VALUES ( @ShrinkSubtype_Id, @ItemHistoryId, GETDATE())
		SELECT @error_no = @@ERROR
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
        RAISERROR ('InsertItemHistory3 failed with @@ERROR: %d', @Severity, 1, @error_no)
    END
END
GO
GRANT EXECUTE
    ON OBJECT::dbo.InsertItemHistory3 TO IRMAClientRole
    AS dbo;


GO
GRANT EXECUTE
    ON OBJECT::dbo.InsertItemHistory3 TO IRMASchedJobsRole
    AS dbo;


GO
GRANT EXECUTE
    ON OBJECT::dbo.InsertItemHistory3 TO IRMAReportsRole
    AS dbo;

