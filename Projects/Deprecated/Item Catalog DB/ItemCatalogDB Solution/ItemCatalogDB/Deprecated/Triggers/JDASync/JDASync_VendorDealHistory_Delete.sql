

IF EXISTS (SELECT * FROM sysobjects WHERE type = 'TR' AND name = 'JDASync_VendorDealHistory_Delete')
	BEGIN
		PRINT 'Dropping Trigger JDASync_VendorDealHistory_Delete'
		DROP  Trigger JDASync_VendorDealHistory_Delete
	END
GO


PRINT 'Creating Trigger JDASync_VendorDealHistory_Delete'
GO
CREATE Trigger dbo.JDASync_VendorDealHistory_Delete 
ON VendorDealHistory
FOR DELETE
AS
BEGIN
    DECLARE @Error_No int
    SELECT @Error_No = 0

    DECLARE @SyncJDA bit
    
    SELECT @SyncJDA = dbo.fn_InstanceDataValue('SyncJDA', NULL)

	-- only if the instance data flag is set
	If @SyncJDA = 1
	BEGIN
			
		DECLARE @DeletedStartDate DATETIME
		
		SELECT @DeletedStartDate = Deleted.StartDate FROM Deleted

		INSERT INTO JDA_CostSync
		(
			ApplyDate,
			Store_No,
			Item_Key,
			Vendor_Id,
			Promotional,
			NetCost,
			Package_Desc1,
			StartDate,
			EndDate
		)
		SELECT
			GetDate(),
			siv.Store_No,
			siv.Item_Key,
			siv.Vendor_Id,
			1,
			cost.NetCost,
			Deleted.Package_Desc1,
			Deleted.StartDate,
			Deleted.EndDate	
		FROM Deleted
			JOIN StoreItemVendor siv (NOLOCK)
				ON siv.StoreItemVendorID = Deleted.StoreItemVendorID
			JOIN
			dbo.fn_VendorCostAll(CONVERT(datetime, CONVERT(varchar(255), @DeletedStartDate, 101))) cost 
				ON cost.Item_Key = siv.Item_Key
					and cost.Store_No = siv.Store_No						
					and cost.Vendor_Id = siv.Vendor_Id						
	END

    SELECT @Error_No = @@ERROR

    IF @Error_No <> 0
    BEGIN
        DECLARE @Severity smallint
        SELECT @Severity = ISNULL((SELECT severity FROM master.dbo.sysmessages WHERE error = @Error_No), 16)
        RAISERROR ('JDASync_VendorDealHistory_Delete trigger failed with @@ERROR: %d', @Severity, 1, @Error_No)
    END
END

GO
