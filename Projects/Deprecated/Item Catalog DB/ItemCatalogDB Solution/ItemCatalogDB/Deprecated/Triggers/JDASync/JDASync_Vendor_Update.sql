

IF EXISTS (SELECT * FROM sysobjects WHERE type = 'TR' AND name = 'JDASync_Vendor_Update')
	BEGIN
		PRINT 'Dropping Trigger JDASync_Vendor_Update'
		DROP  Trigger JDASync_Vendor_Update
	END
GO


PRINT 'Creating Trigger JDASync_Vendor_Update'
GO
CREATE Trigger dbo.JDASync_Vendor_Update 
ON Vendor
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
			'C',
			GetDate(),
			Inserted.Vendor_ID,
			SUBSTRING(Inserted.CompanyName, 1, 50),
			SUBSTRING(Inserted.Address_Line_1, 1, 35),
			SUBSTRING(Inserted.Address_Line_2, 1, 35),
			Inserted.City,
			Inserted.State,
			Inserted.Zip_Code,
			SUBSTRING(Inserted.Country, 1, 3),
			@ConvertedPhone,
			@ConvertedFax,
			Inserted.PS_Vendor_ID,
			CASE WHEN Inserted.Non_Product_Vendor = 1 THEN 2 ELSE Inserted.Non_Product_Vendor END,
			Inserted.Po_Note,
			Inserted.Receiving_Authorization_Note,
			Inserted.Other_Name
		FROM
			Inserted
			JOIN Deleted 
				ON Deleted.Vendor_ID = Inserted.Vendor_ID
		WHERE
			-- we care only if any of the columns we are tracking changes
			Inserted.Vendor_ID <> Deleted.Vendor_ID
			OR SUBSTRING(Inserted.CompanyName, 1, 50) <> ISNULL(SUBSTRING(Deleted.CompanyName, 1, 50),'')
			OR SUBSTRING(Inserted.Address_Line_1, 1, 35) <> ISNULL(SUBSTRING(Deleted.Address_Line_1, 1, 35),'')
			OR SUBSTRING(Inserted.Address_Line_2, 1, 35) <> ISNULL(SUBSTRING(Deleted.Address_Line_2, 1, 35),'')
			OR Inserted.City <> ISNULL(Deleted.City,'')
			OR Inserted.State <> ISNULL(Deleted.State,'')
			OR Inserted.Zip_Code <> ISNULL(Deleted.Zip_Code,'')
			OR Inserted.Country <> ISNULL(Deleted.Country,'')
			OR Inserted.Phone <> ISNULL(Deleted.Phone,'')
			OR Inserted.Fax <> ISNULL(Deleted.Fax,'')
			OR Inserted.PS_Vendor_ID <> ISNULL(Deleted.PS_Vendor_ID,'')
			OR Inserted.Non_Product_Vendor <> ISNULL(Deleted.Non_Product_Vendor,'')
			OR Inserted.Po_Note <> ISNULL(Deleted.Po_Note,'')
			OR Inserted.Receiving_Authorization_Note <> ISNULL(Deleted.Receiving_Authorization_Note,'')
			OR Inserted.Other_Name <> ISNULL(Deleted.Other_Name,'')

	END
	
    SELECT @Error_No = @@ERROR

    IF @Error_No <> 0
    BEGIN
        DECLARE @Severity smallint
        SELECT @Severity = ISNULL((SELECT severity FROM master.dbo.sysmessages WHERE error = @Error_No), 16)
        RAISERROR ('JDASync_Vendor_Update trigger failed with @@ERROR: %d', @Severity, 1, @Error_No)
    END
	
	-- reset it
	SET ANSI_NULLS ON

END

GO
