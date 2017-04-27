

IF EXISTS (SELECT * FROM sysobjects WHERE type = 'TR' AND name = 'JDASync_VendorCostHistory_Insert')
	BEGIN
		PRINT 'Dropping Trigger JDASync_VendorCostHistory_Insert'
		DROP  Trigger JDASync_VendorCostHistory_Insert
	END
GO


PRINT 'Creating Trigger JDASync_VendorCostHistory_Insert'
GO
CREATE Trigger dbo.JDASync_VendorCostHistory_Insert 
ON VendorCostHistory
FOR INSERT
AS
BEGIN
    DECLARE @Error_No int
    SELECT @Error_No = 0

    DECLARE @SyncJDA bit
    
    SELECT @SyncJDA = dbo.fn_InstanceDataValue('SyncJDA', NULL)

	-- only if the instance data flag is set
	If @SyncJDA = 1
	BEGIN
	
		DECLARE @InsertedStartDate DATETIME
		
		SELECT @InsertedStartDate = Inserted.StartDate FROM Inserted
		
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
			Inserted.Promotional,
			cost.NetCost,
			Inserted.Package_Desc1,
			Inserted.StartDate,
			CASE WHEN Inserted.Promotional = 1 THEN Inserted.EndDate ELSE NULL END
		FROM Inserted
			JOIN StoreItemVendor siv (NOLOCK)
				ON siv.StoreItemVendorID = Inserted.StoreItemVendorID
			JOIN
			dbo.fn_VendorCostAll(CONVERT(datetime, CONVERT(varchar(255), @InsertedStartDate, 101))) cost 
				ON cost.Item_Key = siv.Item_Key
					and cost.Store_No = siv.Store_No						
					and cost.Vendor_Id = siv.Vendor_Id		
		WHERE Inserted.IsFromJDASync = 0 -- only if the cost did not come from the loaded cost sync
						
	END

    SELECT @Error_No = @@ERROR

    IF @Error_No <> 0
    BEGIN
        DECLARE @Severity smallint
        SELECT @Severity = ISNULL((SELECT severity FROM master.dbo.sysmessages WHERE error = @Error_No), 16)
        RAISERROR ('JDASync_VendorCostHistory_Insert trigger failed with @@ERROR: %d', @Severity, 1, @Error_No)
    END
END

GO
