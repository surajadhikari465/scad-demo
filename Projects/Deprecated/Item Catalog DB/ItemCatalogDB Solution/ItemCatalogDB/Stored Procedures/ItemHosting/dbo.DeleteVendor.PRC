SET QUOTED_IDENTIFIER ON 
GO
SET ANSI_NULLS ON 
GO

if exists (select * from dbo.sysobjects where id = object_id(N'dbo.DeleteVendor') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure dbo.DeleteVendor
GO


CREATE PROCEDURE dbo.DeleteVendor
@VendorID int 
AS
-- **************************************************************************
	-- Procedure: DeleteVendor()
	--
	-- Description:
	-- This procedure deletes a vendor
	--
	-- Modification History:
	-- Date       	Init  	TFS   	Comment
	-- 01/30/2013	FA   	10005	deletes entries from LastVendor table
	-- **************************************************************************
BEGIN
    SET NOCOUNT ON

    DECLARE @Error_No int
    SELECT @Error_No = 0

    BEGIN TRAN
        
        DELETE 
        FROM VendorCostHistory 
        WHERE StoreItemVendorID in (SELECT SIV.StoreItemVendorID 
                                        FROM StoreItemVendor SIV
                                        WHERE SIV.Vendor_ID = @VendorID)
        SELECT @Error_No = @@ERROR
        
        IF @Error_No = 0
        BEGIN
            DELETE 
            FROM VendorDealHistory
            WHERE StoreItemVendorID in (SELECT SIV.StoreItemVendorID 
                                            FROM StoreItemVendor SIV
                                            WHERE SIV.Vendor_ID = @VendorID)
            SELECT @Error_No = @@ERROR
        END
        
        IF @Error_No = 0
        BEGIN
            DELETE 
            FROM StoreItemVendor 
            WHERE Vendor_ID = @VendorID
        
            SELECT @Error_No = @@ERROR
        END
        
        IF @Error_No = 0
        BEGIN
            DELETE
            FROM ItemVendor 
            WHERE Vendor_ID = @VendorID
            SELECT @Error_No = @@ERROR
        END
        
        IF @Error_No = 0
        BEGIN
            DELETE 
            FROM Contact 
            WHERE Vendor_ID = @VendorID
            SELECT @Error_No = @@ERROR
        END
        
        IF @Error_No = 0
        BEGIN
            DELETE 
            FROM WarehouseVendorChange 
            WHERE Vendor_ID = @VendorID
            SELECT @Error_No = @@ERROR
        END

		IF @Error_No = 0
        BEGIN
            DELETE 
            FROM LastVendor 
            WHERE Vendor_ID = @VendorID
            SELECT @Error_No = @@ERROR
        END
        
        IF @Error_No = 0
        BEGIN
            DELETE 
            FROM Vendor 
            WHERE Vendor_ID = @VendorID
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
        RAISERROR ('DeleteVendor failed with @@ERROR: %d', @Severity, 1, @Error_No)
    END         
END
GO
SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO

