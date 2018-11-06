

IF EXISTS (SELECT * FROM sysobjects WHERE type = 'TR' AND name = 'JDASync_Vendor_Insert')
	BEGIN
		PRINT 'Dropping Trigger JDASync_Vendor_Insert'
		DROP  Trigger JDASync_Vendor_Insert
	END
GO


PRINT 'Creating Trigger JDASync_Vendor_Insert'
GO
CREATE Trigger dbo.JDASync_Vendor_Insert 
ON Vendor
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
	
		DECLARE @ConvertedPhone BIGINT, @ConvertedFax BIGINT
		
		-- try to convert the phone and fax to numeric
		-- default to null if conversion fails
		BEGIN TRY
		
			SELECT @ConvertedPhone = CAST(Replace(Replace(Replace(Replace(Inserted.Phone, '(', ''), ')', ''), '/', ''), '-', '') AS BIGINT) FROM Inserted
			SELECT @ConvertedFax = CAST(Replace(Replace(Replace(Replace(Inserted.Fax, '(', ''), ')', ''), '/', ''), '-', '') AS BIGINT) FROM Inserted
			
		END TRY
		BEGIN CATCH
		
			SET @ConvertedPhone = NULL
			SET @ConvertedFax = NULL

		END CATCH

		INSERT INTO JDA_VendorSync
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
			Non_Product_Vendor,
			Po_Note,
			Receiving_Authorization_Note,
			Other_Name
		)
		SELECT
			'A',
			GetDate(),
			Inserted.Vendor_ID,
			SUBSTRING(Inserted.CompanyName, 1, 50),
			SUBSTRING(Inserted.Address_Line_1, 1, 35),
			SUBSTRING(Inserted.Address_Line_2, 1, 35),
			Inserted.City,
			Inserted.State,
			Inserted.Zip_Code,
			SUBSTRING(Inserted.Country, 1, 3),
			Inserted.Phone,
			Inserted.Fax,
			Inserted.PS_Vendor_ID,
			CASE WHEN Inserted.Non_Product_Vendor = 1 THEN 2 ELSE Inserted.Non_Product_Vendor END,
			Inserted.Po_Note,
			Inserted.Receiving_Authorization_Note,
			Inserted.Other_Name
		FROM Inserted
	END
	
    SELECT @Error_No = @@ERROR

    IF @Error_No <> 0
    BEGIN
        DECLARE @Severity smallint
        SELECT @Severity = ISNULL((SELECT severity FROM master.dbo.sysmessages WHERE error = @Error_No), 16)
        RAISERROR ('JDASync_Vendor_Insert trigger failed with @@ERROR: %d', @Severity, 1, @Error_No)
    END
END

GO
