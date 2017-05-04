CREATE PROCEDURE dbo.InsertOrderItemQueue
    @Store_No int,
    @TransferToSubTeam_No int,
    @Item_Key int,
    @Transfer bit,
    @User_ID int,
    @Quantity decimal(18,4),
    @Unit_ID int,
    @Credit bit
AS

BEGIN
    SET NOCOUNT ON

    DECLARE @error_no int
    SELECT @error_no = 0

    IF EXISTS (SELECT *
               FROM OrderItemQueue
               WHERE Store_No = @Store_No
                   AND TransferToSubTeam_No = @TransferToSubTeam_No
                   AND Item_Key = @Item_Key
                   AND Transfer = @Transfer
                   AND Credit = @Credit)
    BEGIN
        IF @Quantity > 0
            UPDATE OrderItemQueue
            SET Quantity = @Quantity,
                Unit_ID = @Unit_ID,
                [User_ID] = @User_ID,
                Insert_Date = GETDATE()
            WHERE Store_No = @Store_No
                AND TransferToSubTeam_No = @TransferToSubTeam_No
                AND Item_Key = @Item_Key
                AND Transfer = @Transfer
                AND Credit = @Credit
                AND (Quantity <> @Quantity
                    OR Unit_ID <> @Unit_ID)
        ELSE
            DELETE OrderItemQueue
            WHERE Store_No = @Store_No
               AND TransferToSubTeam_No = @TransferToSubTeam_No
               AND Item_Key = @Item_Key
               AND Transfer = @Transfer
               AND Credit = @Credit

        SELECT @error_no = @@ERROR
    END
    ELSE
    BEGIN
        IF @Quantity > 0
        BEGIN
            INSERT INTO OrderItemQueue (Store_No, TransferToSubTeam_No, Item_Key, Transfer, [User_ID], Quantity, Unit_ID, Insert_Date, Credit)
            VALUES(@Store_No, @TransferToSubTeam_No, @Item_Key, @Transfer, @User_ID, @Quantity, @Unit_ID, GETDATE(), @Credit)
            
            SELECT @error_no = @@ERROR
        END
    END

    SET NOCOUNT OFF

    IF @error_no <> 0
    BEGIN
        IF @@TRANCOUNT <> 0
            ROLLBACK TRAN
        DECLARE @Severity smallint
        SELECT @Severity = ISNULL((SELECT severity FROM master.dbo.sysmessages WHERE error = @error_no), 16)
        RAISERROR ('InsertOrderItemQueue failed with @@ERROR: %d', @Severity, 1, @error_no)
    END
END
GO
GRANT EXECUTE
    ON OBJECT::[dbo].[InsertOrderItemQueue] TO [IRMAClientRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[InsertOrderItemQueue] TO [IRMASchedJobsRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[InsertOrderItemQueue] TO [IRMAReportsRole]
    AS [dbo];

