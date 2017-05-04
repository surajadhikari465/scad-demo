CREATE PROCEDURE [dbo].[POSDeleteItem]
    @Item_Key int
AS 

BEGIN
/******************************************************************************************************************************
CHANGE LOG
DEV				DATE					TASK				Description
----------------------------------------------------------------------------------------------
DBS				20110210				13835				Speed up delete process by removing locks 
BAS				20130114				8755				Removed Discontinue_Item in Item table update since StoreItemVendor
															record is being removed which would also remove Discontinue flag
********************************************************************************************************************************/

    DECLARE @Error_No int
    SELECT @Error_No = 0

    BEGIN TRAN

		DELETE VendorDealHistory
		FROM dbo.VendorDealHistory VDH
		INNER JOIN dbo.StoreItemVendor SIV (NOLOCK) ON SIV.StoreItemVendorID = VDH.StoreItemVendorID
		WHERE Item_Key = @Item_Key
    
    SELECT @Error_No = @@ERROR

    IF @Error_No = 0
    BEGIN
		DELETE VendorCostHistory
		FROM dbo.VendorCostHistory VCH
		INNER JOIN dbo.StoreItemVendor SIV (NOLOCK) ON SIV.StoreItemVendorID = VCH.StoreItemVendorID
		WHERE Item_Key = @Item_Key
        
        SELECT @Error_No = @@ERROR
    END

    IF @Error_No = 0
    BEGIN
		DELETE FROM dbo.StoreItemVendor WHERE Item_Key = @Item_Key
        SELECT @Error_No = @@ERROR
    END

    IF @Error_No = 0
    BEGIN
        DELETE FROM dbo.ItemVendor WHERE Item_Key = @Item_Key
        SELECT @Error_No = @@ERROR
    END

    IF @Error_No = 0
    BEGIN
        DELETE FROM dbo.Price WHERE Item_Key = @Item_Key
        SELECT @Error_No = @@ERROR
    END

    IF @Error_No = 0
    BEGIN
        DELETE FROM dbo.ItemIdentifier WHERE Item_Key = @Item_Key AND Default_Identifier = 0
        SELECT @Error_No = @@ERROR
    END

    IF @Error_No = 0
    BEGIN
        UPDATE dbo.ItemIdentifier 
        SET Deleted_Identifier = 1, Add_Identifier = 0, Remove_Identifier = 0
        WHERE Item_Key = @Item_Key
        
        SELECT @Error_No = @@ERROR
    END
        
    IF @Error_No = 0
    BEGIN
        UPDATE dbo.Item 
        SET Deleted_Item = 1, 
            Remove_Item = 0, 
            Not_Available = 0,
            Category_ID = NULL -- Allows categories to be deleted
        WHERE Item_Key = @Item_Key

        SELECT @Error_No = @@ERROR
    END

    IF @Error_No = 0
        COMMIT TRAN
    ELSE
    BEGIN
        ROLLBACK TRAN
        DECLARE @Severity smallint
        SELECT @Severity = ISNULL((SELECT severity FROM master.dbo.sysmessages WHERE error = @Error_No), 16)
        RAISERROR ('POSDeleteItem failed with @@ERROR: %d', @Severity, 1, @Error_No)
    END

END
GO
GRANT EXECUTE
    ON OBJECT::[dbo].[POSDeleteItem] TO [IRMAClientRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[POSDeleteItem] TO [IRMASchedJobsRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[POSDeleteItem] TO [IRMAReportsRole]
    AS [dbo];

