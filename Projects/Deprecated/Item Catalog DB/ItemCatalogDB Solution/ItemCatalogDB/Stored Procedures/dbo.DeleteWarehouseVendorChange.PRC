SET QUOTED_IDENTIFIER ON 
GO
SET ANSI_NULLS ON 
GO

if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].DeleteWarehouseVendorChange') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].DeleteWarehouseVendorChange
GO


CREATE PROCEDURE dbo.DeleteWarehouseVendorChange
    @WarehouseVendorChangeID int
AS

BEGIN
    SET NOCOUNT ON

    DECLARE @Error_No int
    SELECT @Error_No = 0

    BEGIN TRAN

    UPDATE Vendor
    SET EXEWarehouseVendSent = CASE WHEN WVC.Customer = 0 THEN 1 ELSE EXEWarehouseVendSent END,
        EXEWarehouseCustSent = CASE WHEN WVC.Customer = 1 THEN 1 ELSE EXEWarehouseCustSent END
    FROM Vendor
    INNER JOIN
        WarehouseVendorChange WVC
        ON WVC.Vendor_ID = Vendor.Vendor_ID
    WHERE WVC.WarehouseVendorChangeID = @WarehouseVendorChangeID
        AND ((EXEWarehouseVendSent <> CASE WHEN WVC.Customer = 0 THEN 1 ELSE EXEWarehouseVendSent END)
             OR
             (EXEWarehouseCustSent <> CASE WHEN WVC.Customer = 1 THEN 1 ELSE EXEWarehouseCustSent END))

    SELECT @Error_No = @@ERROR

    IF @Error_No = 0
    BEGIN
        DELETE WarehouseVendorChange WHERE WarehouseVendorChangeID = @WarehouseVendorChangeID

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
        RAISERROR ('DeleteWarehouseVendorChange failed with @@ERROR: %d', @Severity, 1, @Error_No)
    END
END

GO
SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO

