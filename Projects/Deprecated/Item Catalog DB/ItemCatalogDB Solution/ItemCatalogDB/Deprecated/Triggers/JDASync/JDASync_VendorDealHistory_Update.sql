

IF EXISTS (SELECT * FROM sysobjects WHERE type = 'TR' AND name = 'JDASync_VendorDealHistory_Update')
	BEGIN
		PRINT 'Dropping Trigger JDASync_VendorDealHistory_Update'
		DROP  Trigger JDASync_VendorDealHistory_Update
	END
GO


PRINT 'Creating Trigger JDASync_VendorDealHistory_Update'
GO
CREATE Trigger dbo.JDASync_VendorDealHistory_Update 
ON VendorDealHistory
FOR UPDATE
AS
BEGIN
	-- this is critical to the functioning of the audit
	-- it allows us to compare null to null
	SET ANSI_NULLS OFF

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
			1,
			cost.NetCost,
			Inserted.Package_Desc1,
			Inserted.StartDate,
			Inserted.EndDate	
		FROM Inserted
			JOIN StoreItemVendor siv (NOLOCK)
				ON siv.StoreItemVendorID = Inserted.StoreItemVendorID
			JOIN
			dbo.fn_VendorCostAll(CONVERT(datetime, CONVERT(varchar(255), @InsertedStartDate, 101))) cost 
				ON cost.Item_Key = siv.Item_Key
					and cost.Store_No = siv.Store_No						
					and cost.Vendor_Id = siv.Vendor_Id						
	END

    SELECT @Error_No = @@ERROR

    IF @Error_No <> 0
    BEGIN
        DECLARE @Severity smallint
        SELECT @Severity = ISNULL((SELECT severity FROM master.dbo.sysmessages WHERE error = @Error_No), 16)
        RAISERROR ('JDASync_VendorDealHistory_Update trigger failed with @@ERROR: %d', @Severity, 1, @Error_No)
    END
	
	-- reset it
	SET ANSI_NULLS ON

END

GO
