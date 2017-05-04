CREATE PROCEDURE [dbo].[UpdateVendorInfo]
	@Vendor_ID INT,
	@Vendor_Key VARCHAR(10),
	@CompanyName VARCHAR(50),
	@Address_Line_1 VARCHAR(50),
	@Address_Line_2 VARCHAR(50),
	@City VARCHAR(30),
	@State VARCHAR(2),
	@Zip_Code VARCHAR(10),
	@Country VARCHAR(10),
	@County VARCHAR(20),
	@Phone VARCHAR(20),
	@Phone_Ext VARCHAR(5),
	@Fax VARCHAR(20),
	@PayTo_CompanyName VARCHAR(50),
	@PayTo_Attention VARCHAR(50),
	@PayTo_Address_Line_1 VARCHAR(50),
	@PayTo_Address_Line_2 VARCHAR(50),
	@PayTo_City VARCHAR(30),
	@PayTo_State VARCHAR(2),
	@PayTo_Zip_Code VARCHAR(10),
	@PayTo_Country VARCHAR(10),
	@PayTo_County VARCHAR(20),
	@PayTo_Phone VARCHAR(20),
	@PayTo_Phone_Ext VARCHAR(5),
	@PayTo_Fax VARCHAR(20),
	@PS_Vendor_ID VARCHAR(10),
	@PS_Export_Vendor_ID VARCHAR(10),
	@PS_Location_Code VARCHAR(10),
	@PS_Address_Sequence VARCHAR(2),
	@Comment VARCHAR(255),
	@Customer BIT,
	@InternalCustomer BIT,
	@WFM BIT,
	@Default_GLNumber VARCHAR(10),
	@Non_Product_Vendor TINYINT,
	@Email VARCHAR(50),
	@EFT BIT,
	@Po_Note VARCHAR(150),
	@Receiving_Authorization_Note VARCHAR(150),
	@Other_Name VARCHAR(35),
	@Culture VARCHAR(5),
	@CaseDistHandlingCharge SMALLMONEY,
	@POTransmissionTypeID TINYINT,
	@Einvoicing BIT,
	@EinvoiceRequired BIT,
	@CurrencyID INT,
	@LeadTimeDays int,
	@LeadTimeDayOfWeek int,
	@ChangedByUserID int,
	@AccountingContactEmail varchar(50) = NULL,
	@PaymentTermID int,
	@AllowReceiveAll BIT,
	@ShortpayProhibited BIT,
	@ActiveVendor BIT,
   @AllowBarcodePOReport BIT

AS
   -- **************************************************************************
   -- Procedure: UpdateVendorInfo
   --    Author: n/a
   --      Date: n/a
   --
   -- Description:
   --
   -- Modification History:
   -- Date        Init			Comment
   -- 11/11/2009  BBB			Update existing SP to update BusinessUnit_ID in Vendor table 
   -- 11/24/2009  BBB			Added trap to Store_No to bypass FK constraint
   -- 04/02/2010  BBB			Added trap for blank BusinessUnit_ID value
   -- 04/06/2010  BBB			Removed BusinessUnit_ID from query
   -- 01/17/2010  Tom Lux		Added two params for vendor table: lead-time days and dayofweek.
   --							Added changed-by-user param for new insert into new VendorHistory table.
   --							Added transaction.
   -- 08/03/2011  MD            Added EinvoiceRequired field TFS 2455
   -- 2012-10-09  KM			Added AllowReceiveAll field to Vendor update and VendorHistory insert
   -- 11/05/2012  DN			Added ShortpayProhibited field to Vendor update and VendorHistory insert
   -- 03/26/2013  MZ			Added ActiveVendor field to Vendor Update
   -- **************************************************************************

BEGIN

BEGIN TRY
   
    BEGIN TRANSACTION

	-- Identifies where we are in this process for inclusion in error messages.
	declare @task varchar(128)

	/*
		Update vendor enty.
	*/
	select @task = 'Update Vendor Table Entry'
	UPDATE Vendor
	SET    USER_ID = NULL,
	       Vendor_Key = @Vendor_Key,
	       CompanyName = @CompanyName,
	       Address_Line_1 = @Address_Line_1,
	       Address_Line_2 = @Address_Line_2,
	       City = @City,
	       STATE = CASE 
	                    WHEN @Culture <> 'en-GB' THEN CASE 
	                                                       WHEN LEN(@State) = 0 THEN 
	                                                            NULL
	                                                       ELSE @State
	                                                  END
	                    ELSE STATE
	               END,
	       Zip_Code = @Zip_Code,
	       Country = @Country,
	       County = CASE 
	                     WHEN @Culture = 'en-GB' THEN CASE 
	                                                       WHEN LEN(@County) = 0 THEN 
	                                                            NULL
	                                                       ELSE @County
	                                                  END
	                     ELSE County
	                END,
	       Phone = @Phone,
	       Phone_Ext = @Phone_Ext,
	       Fax = @Fax,
	       PayTo_CompanyName = @PayTo_CompanyName,
	       PayTo_Attention = @PayTo_Attention,
	       PayTo_Address_Line_1 = @PayTo_Address_Line_1,
	       PayTo_Address_Line_2 = @PayTo_Address_Line_2,
	       PayTo_City = @PayTo_City,
	       PayTo_State = CASE 
	                          WHEN @Culture <> 'en-GB' THEN CASE 
	                                                             WHEN LEN(@PayTo_State) 
	                                                                  = 0 THEN 
	                                                                  NULL
	                                                             ELSE @PayTo_State
	                                                        END
	                          ELSE PayTo_State
	                     END,
	       PayTo_Zip_Code = @PayTo_Zip_Code,
	       PayTo_Country = @PayTo_Country,
	       PayTo_County = CASE 
	                           WHEN @Culture = 'en-GB' THEN CASE 
	                                                             WHEN LEN(@PayTo_County) 
	                                                                  = 0 THEN 
	                                                                  NULL
	                                                             ELSE @PayTo_County
	                                                        END
	                           ELSE PayTo_County
	                      END,
	       PayTo_Phone = @PayTo_Phone,
	       PayTo_Phone_Ext = @PayTo_Phone_Ext,
	       PayTo_Fax = @PayTo_Fax,
	       PS_Vendor_ID = CASE 
	                           WHEN LEN(RTRIM(@PS_Vendor_ID)) = 0 THEN NULL
	                           ELSE @PS_Vendor_ID
	                      END,
	       PS_Export_Vendor_ID = CASE 
	                                  WHEN LEN(RTRIM(@PS_Export_Vendor_ID)) = 0 THEN 
	                                       NULL
	                                  ELSE @PS_Export_Vendor_ID
	                             END,
	       PS_Location_Code = CASE 
	                               WHEN LEN(RTRIM(@PS_Location_Code)) = 0 THEN CASE 
	                                                                                WHEN 
	                                                                                     LEN(RTRIM(@PS_Vendor_ID)) 
	                                                                                     > 
	                                                                                     0 THEN 
	                                                                                     'DEFAULT'
	                                                                                ELSE 
	                                                                                     NULL
	                                                                           END
	                               ELSE @PS_Location_Code
	                          END,
	       PS_Address_Sequence = CASE 
	                                  WHEN LEN(RTRIM(@PS_Address_Sequence)) = 0 THEN 
	                                       NULL
	                                  ELSE @PS_Address_Sequence
	                             END,
	       Comment = @Comment,
	       Customer = @Customer,
	       InternalCustomer = @InternalCustomer,
	       WFM = @WFM,
	       Default_GLNumber = @Default_GLNumber,
	       Non_Product_Vendor = @Non_Product_Vendor,
	       Email = CASE 
	                    WHEN LEN(RTRIM(@Email)) = 0 THEN NULL
	                    ELSE @Email
	               END,
	       EFT = @EFT,
	       Po_Note = @Po_Note,
	       Receiving_Authorization_Note = @Receiving_Authorization_Note,
	       Other_Name = @Other_Name,
	       CaseDistHandlingCharge = @CaseDistHandlingCharge,
	       POTransmissionTypeID = @POTransmissionTypeID,
	       EInvoicing = @Einvoicing,
		   EinvoiceRequired = @EinvoiceRequired,
	       CurrencyID = @CurrencyID,
		   LeadTimeDays = @LeadTimeDays,
		   LeadTimeDayOfWeek = @LeadTimeDayOfWeek,
		   AccountingContactEmail = @AccountingContactEmail,
		   PaymentTermID = @PaymentTermID,
		   AllowReceiveAll = @AllowReceiveAll,
		   ShortpayProhibited = @ShortpayProhibited, 
		   ActiveVendor = @ActiveVendor,
			AllowBarcodePOReport = @AllowBarcodePOReport

	WHERE  Vendor_ID = @Vendor_ID
	
	/*
		If the Facility Handling Charge is 0, set any Facility Handling Charge Overrides to 0.
	*/
	select @task = 'Update ItemVendor Table: Facility Handling Charge Overrides'
	IF @CaseDistHandlingCharge = 0
	BEGIN
	    UPDATE ItemVendor
	    SET    CaseDistHandlingChargeOverride = 0
	    WHERE  Vendor_ID = @Vendor_ID
	END

	/*
		Create history entry using updated info.
	*/
	select @task = 'Create VendorHistory Table Entry'

	insert into VendorHistory
	(
		[Vendor_ID]
		,[Vendor_Key]
		,[CompanyName]
		,[Address_Line_1]
		,[Address_Line_2]
		,[City]
		,[State]
		,[Zip_Code]
		,[Country]
		,[Phone]
		,[Fax]
		,[PayTo_CompanyName]
		,[PayTo_Attention]
		,[PayTo_Address_Line_1]
		,[PayTo_Address_Line_2]
		,[PayTo_City]
		,[PayTo_State]
		,[PayTo_Zip_Code]
		,[PayTo_Country]
		,[PayTo_Phone]
		,[PayTo_Fax]
		,[Comment]
		,[Customer]
		,[InternalCustomer]
		,[ActiveVendor]
		,[Store_no]
		,[Order_By_Distribution]
		,[Electronic_Transfer]
		,[User_ID]
		,[Phone_Ext]
		,[PayTo_Phone_Ext]
		,[PS_Vendor_ID]
		,[PS_Location_Code]
		,[PS_Address_Sequence]
		,[WFM]
		,[FTP_Addr]
		,[FTP_Path]
		,[FTP_User]
		,[FTP_Password]
		,[Non_Product_Vendor]
		,[Default_GLNumber]
		,[Email]
		,[EFT]
		,[InStoreManufacturedProducts]
		,[EXEWarehouseVendSent]
		,[EXEWarehouseCustSent]
		,[County]
		,[PayTo_County]
		,[AddVendor]
		,[Po_Note]
		,[Receiving_Authorization_Note]
		,[Other_Name]
		,[PS_Export_Vendor_ID]
		,[File_Type]
		,[CaseDistHandlingCharge]
		,[EInvoicing]
		,[EinvoiceRequired]
		,[POTransmissionTypeID]
		,[CurrencyID]
		,[AccountingContactEmail]
		,[LeadTimeDays]
		,[LeadTimeDayOfWeek]
		,[ChangedByUserID]
		,[ChangeDate]
		,[AllowReceiveAll]
		,[ShortpayProhibited]
	)
	select
		[Vendor_ID]
		,[Vendor_Key]
		,[CompanyName]
		,[Address_Line_1]
		,[Address_Line_2]
		,[City]
		,[State]
		,[Zip_Code]
		,[Country]
		,[Phone]
		,[Fax]
		,[PayTo_CompanyName]
		,[PayTo_Attention]
		,[PayTo_Address_Line_1]
		,[PayTo_Address_Line_2]
		,[PayTo_City]
		,[PayTo_State]
		,[PayTo_Zip_Code]
		,[PayTo_Country]
		,[PayTo_Phone]
		,[PayTo_Fax]
		,[Comment]
		,[Customer]
		,[InternalCustomer]
		,[ActiveVendor]
		,[Store_no]
		,[Order_By_Distribution]
		,[Electronic_Transfer]
		,[User_ID]
		,[Phone_Ext]
		,[PayTo_Phone_Ext]
		,[PS_Vendor_ID]
		,[PS_Location_Code]
		,[PS_Address_Sequence]
		,[WFM]
		,[FTP_Addr]
		,[FTP_Path]
		,[FTP_User]
		,[FTP_Password]
		,[Non_Product_Vendor]
		,[Default_GLNumber]
		,[Email]
		,[EFT]
		,[InStoreManufacturedProducts]
		,[EXEWarehouseVendSent]
		,[EXEWarehouseCustSent]
		,[County]
		,[PayTo_County]
		,[AddVendor]
		,[Po_Note]
		,[Receiving_Authorization_Note]
		,[Other_Name]
		,[PS_Export_Vendor_ID]
		,[File_Type]
		,[CaseDistHandlingCharge]
		,[EInvoicing]
		,[EinvoiceRequired]
		,[POTransmissionTypeID]
		,[CurrencyID]
		,[AccountingContactEmail]
		,[LeadTimeDays]
		,[LeadTimeDayOfWeek]
		,[ChangedByUserID] = @ChangedByUserID
		,[ChangeDate] = GETDATE()
		,[AllowReceiveAll]
		,[ShortpayProhibited]

	from Vendor
	where Vendor_ID = @Vendor_ID


	/*
		Commit changes.
	*/
	if @@TRANCOUNT > 0
	begin
		print '-------------------------------------------------';
		print '[' + convert(nvarchar, getdate(), 121) + '] ' + 'Committing ' + cast(@@TRANCOUNT as varchar) + ' transaction(s)...';
		COMMIT TRANSACTION
		print '[' + convert(nvarchar, getdate(), 121) + '] ' + 'Updates committed successfully.';
		print '-------------------------------------------------';
	end
	else
	begin
		PRINT '[' + convert(nvarchar, getdate(), 121) + '] ' + '**Warning** -- No updates to commit.';
	end
end try
begin catch
	declare @tranCount int; select @tranCount = @@TRANCOUNT
	IF @@TRANCOUNT > 0
	begin
		print '-------------------------------------------------';
		print '[' + convert(nvarchar, getdate(), 121) + '] ' + 'Performing transaction rollback...';
		ROLLBACK TRANSACTION
		print '[' + convert(nvarchar, getdate(), 121) + '] ' + 'Rollback complete.';
		print '-------------------------------------------------';
	end
	else
	begin
		print '[' + convert(nvarchar, getdate(), 121) + '] ' + '**Nothing to rollback.';
	end


	DECLARE @ErrorMessage NVARCHAR(4000);
	DECLARE @ErrorSeverity INT;
	DECLARE @ErrorState INT;

	/*
		Build an error message to raise that will show original error thrown, plus additional specifics.
	*/
    SELECT 
		@ErrorMessage = 'Status: No changes committed
Tran count: ' + cast(isnull(@tranCount, 0) as varchar) + '
Task: ' + @task  + '
Error: ''' + isnull(ERROR_MESSAGE(), '[no error msg]') + '''
Procedure: ' + isnull(ERROR_PROCEDURE(), '[no procedure ref]') + '
Line: ' + cast(isnull(ERROR_LINE(), '[no error line]') as varchar)
		,@ErrorSeverity = isnull(ERROR_SEVERITY(), 0)
		,@ErrorState = isnull(ERROR_STATE(), 0)

    RAISERROR (
		@ErrorMessage -- Message text.
		,@ErrorSeverity -- Severity.
		,@ErrorState -- State.
	)

END CATCH

END
GO
GRANT EXECUTE
    ON OBJECT::[dbo].[UpdateVendorInfo] TO [IRMAClientRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[UpdateVendorInfo] TO [IRMASchedJobsRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[UpdateVendorInfo] TO [IRMAReportsRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[UpdateVendorInfo] TO [IRMASLIMRole]
    AS [dbo];

