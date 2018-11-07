CREATE PROCEDURE dbo.DeleteWarehouseItemChange
    @WarehouseItemChangeID int
AS

BEGIN
    SET NOCOUNT ON

    DECLARE @Error_No int
    SELECT @Error_No = 0

    BEGIN TRAN

    UPDATE SubTeam
    SET EXEWarehouseSent = 1
    FROM WarehouseItemChange WIC
    INNER JOIN 
        Item
        ON WIC.Item_Key = Item.Item_Key
    INNER JOIN
        SubTeam
        ON SubTeam.SubTeam_No = Item.SubTeam_No
    WHERE WIC.WarehouseItemChangeID = @WarehouseItemChangeID
        AND EXEWarehouseSent <> 1

    SELECT @Error_No = @@ERROR

    IF @Error_No = 0
    BEGIN
        DELETE WarehouseItemChange WHERE WarehouseItemChangeID = @WarehouseItemChangeID

        SELECT @Error_No = @@ERROR
    END


    IF @Error_No = 0
    BEGIN
        COMMIT TRAN
        SET NOCOUNT OFF
    END
    ELSE
    BEGIN
        ROLLBACK TRAN
        DECLARE @Severity smallint
        SELECT @Severity = ISNULL((SELECT severity FROM master.dbo.sysmessages WHERE error = @Error_No), 16)
        RAISERROR ('<procedure_name> failed with @@ERROR: %d', @Severity, 1, @Error_No)
    END
END
GO
GRANT EXECUTE
    ON OBJECT::[dbo].[DeleteWarehouseItemChange] TO [IRMAClientRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[DeleteWarehouseItemChange] TO [IRMASchedJobsRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[DeleteWarehouseItemChange] TO [IRMAReportsRole]
    AS [dbo];

