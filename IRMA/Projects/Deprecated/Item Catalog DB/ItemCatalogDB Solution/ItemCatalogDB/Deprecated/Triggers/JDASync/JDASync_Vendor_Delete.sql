

IF EXISTS (SELECT * FROM sysobjects WHERE type = 'TR' AND name = 'JDASync_Vendor_Delete')
	BEGIN
		PRINT 'Dropping Trigger JDASync_Vendor_Delete'
		DROP  Trigger JDASync_Vendor_Delete
	END
GO


PRINT 'Creating Trigger JDASync_Vendor_Delete'
GO
CREATE Trigger dbo.JDASync_Vendor_Delete 
ON Vendor
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
	
		DECLARE @ConvertedPhone BIGINT, @ConvertedFax BIGINT
		
		-- try to convert the phone and fax to numeric
		-- default to null if conversion fails
		BEGIN TRY
		
			SELECT @ConvertedPhone = CAST(Replace(Replace(Replace(Replace(Deleted.Phone, '(', ''), ')', ''), '/', ''), '-', '') AS BIGINT) FROM Deleted
			SELECT @ConvertedFax = CAST(Replace(Replace(Replace(Replace(Deleted.Fax, '(', ''), ')', ''), '/', ''), '-', '') AS BIGINT) FROM Deleted
			
		END TRY
		BEGIN CATCH
		
			SET @ConvertedPhone = NULL
			SET @ConvertedFax = NULL

		END CATCH

		Insert INTO JDA_VendorSync
		(
			ActionCode,
			ApplyDate,
			Vendor_ID,
			CompanyName,
			Address_Line_1,
			Address_Line_2,
			City,
			State,
			Zip_Code,
			Country,
			Phone,
			Fax,
			PS_Vendor_ID,
			Non_Product_Vendor
		)
		SELECT
			'D',
			GetDate(),
			Deleted.Vendor_ID,
			SUBSTRING(Deleted.CompanyName, 1, 50),
			SUBSTRING(Deleted.Address_Line_1, 1, 35),
			SUBSTRING(Deleted.Address_Line_2, 1, 35),
			Deleted.City,
			Deleted.State,
			Deleted.Zip_Code,
			SUBSTRING(Deleted.Country, 1, 3),
			@ConvertedPhone,
			@ConvertedFax,
			Deleted.PS_Vendor_ID,
			CASE WHEN Deleted.Non_Product_Vendor = 1 THEN 2 ELSE Deleted.Non_Product_Vendor END
		FROM Deleted
	END
	
    SELECT @Error_No = @@ERROR

    IF @Error_No <> 0
    BEGIN
        DECLARE @Severity smallint
        SELECT @Severity = ISNULL((SELECT severity FROM master.dbo.sysmessages WHERE error = @Error_No), 16)
        RAISERROR ('JDASync_Vendor_Delete trigger failed with @@ERROR: %d', @Severity, 1, @Error_No)
    END
END

GO
