if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[EIM_UploadSession_NewItems]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[EIM_UploadSession_NewItems]
GO

CREATE PROCEDURE [dbo].[EIM_UploadSession_NewItems]
	@UploadSession_ID int,
	@UploadRow_ID int,
	@RetryCount int,
	@FourLevelHierarchyFlag bit,
	@UseStoreJurisdictions bit,
	@LoggingLevel varchar(10),
	@Item_key int OUTPUT		
AS
	
	-- ***********************************************************************************************************************
	-- Called by EIM to create new items.
	--
	-- First calls InsertItem proc to create the item
	-- and then executes dynamic update statements to
	-- update uploaded data that is not a param of the
	-- InsertItem proc.
	--
	-- David Marine
	-------------------------------------------------------------------------------------------------------------------------
	-- Revision history
	-------------------------------------------------------------------------------------------------------------------------
	-- MZ   2015-08-19  16352 (10976)   Do NOT allow retail sail flag to be set to true if the item to be updated is linked
    --                                  to an ingredient identifier (defined by ranges)
	-- KM	2015-09-15	11338			Insert sign attribute values if present;
	-- KM	2015-09-23	11338			More sign attributes;
	-- MU/MZ	
	--		2016-03-24		TFS18686	
	--						PBI13711	Adding Sold By WFM and Sold By 365 to accommodate setting via EIM

	-- ***********************************************************************************************************************
	
	set nocount on

	DECLARE
		@Tablename varchar(200),
		@ColumnName varchar(200),
		@ColumnValue varchar(4200),
		@ColumnDbDataType varchar(200),
		@UploadValue_ID int,		
		@IsDefaultJurisdiction bit,
		@StoreJurisdictionID int
	
	DECLARE
		-- for params for inserting a new item
		@POS_Description varchar(26),
		@Item_Description varchar(60),
		@SubTeam_No int,
		@Category_ID int,
		@Retail_Unit_ID int,
		@Package_Unit_ID int,
		@Package_Desc1 decimal(9,4),
		@Package_Desc2 decimal(9,4),
		@IdentifierType char(1),
		@Identifier varchar(13),
		@CheckDigit char(1),
		@Retail_Sale bit,
		@ClassID int,
		@CostedByWeight bit,
		@Vendor_Unit_ID int,
		@Distribution_Unit_ID int,   
		@TaxClassID int,
		@LabelType_ID int,
		@Brand_ID int,
		@National_Identifier tinyint,
		@NumPluDigitsSentToScale int,
		@Scale_Identifier bit,
		@ProdHierarchyLevel4_ID int,
		@InsertApplication varchar(30),
		@WFM_Item bit,
		@HFM_Item tinyint,

		-- sign attributes
		@Locality varchar(50),
		@ShortSignRomance varchar(140),
		@LongSignRomance varchar(300),
		@ChicagoBaby varchar(50),
		@TagUom int,
		@Exclusive date,
		@ColorAdded bit
		
	EXEC dbo.EIM_Log @LoggingLevel, 'TRACE', @UploadSession_ID, @UploadRow_ID, @RetryCount, @Item_key, NULL, '2.0 New Item Creation - [Begin]'
	
	-- default values
	SELECT @CostedByWeight = dbo.fn_IsScaleIdentifier(@Identifier)
	SELECT @Scale_Identifier = dbo.fn_IsScaleIdentifier(@Identifier)

	-- for looping through the row's upload values
	DECLARE UploadValue_cursor CURSOR FOR
		SELECT uv.UploadValue_ID, uv.Value As ColumnValue, LOWER(ua.TableName) As TableName, LOWER(ua.ColumnNameorKey) As ColumnName,
			ua.DbDataType
		FROM UploadValue (NOLOCK) uv
			INNER JOIN UploadAttribute (NOLOCK) ua
			ON uv.UploadAttribute_ID = ua.UploadAttribute_ID
			INNER JOIN UploadTypeAttribute (NOLOCK) uta
			ON ua.UploadAttribute_ID = uta.UploadAttribute_ID
		WHERE uv.UploadRow_ID = @UploadRow_ID
			-- only for item maintenance
			AND uta.uploadtype_code = 'ITEM_MAINTENANCE'

	OPEN UploadValue_cursor
	
	FETCH NEXT FROM UploadValue_cursor INTO @UploadValue_ID, @ColumnValue, @TableName, @ColumnName, @columnDbDataType

	-- extract the uploaded values
	WHILE @@FETCH_STATUS = 0
	BEGIN
	
		-- extract all the upload values needed as parameters
		-- for the InsertItem proc
		IF @TableName = 'item'
		BEGIN						
			if @ColumnName = LOWER('POS_Description')
				select @POS_Description = CAST(@ColumnValue AS varchar(26))
			else
			if @ColumnName = LOWER('Item_Description')
				select @Item_Description = CAST(@ColumnValue AS varchar(60))
			else
			if @ColumnName = LOWER('SubTeam_No')
				select @SubTeam_No = CAST(@ColumnValue AS int)
			else
			if @ColumnName = LOWER('Category_ID')
				select @Category_ID = CAST(@ColumnValue AS int)
			else
			if @ColumnName = LOWER('Retail_Unit_ID')
				select @Retail_Unit_ID = CAST(@ColumnValue AS int)
			else
			if @ColumnName = LOWER('Package_Unit_ID')
				select @Package_Unit_ID = CAST(@ColumnValue AS int)
			else
			if @ColumnName = LOWER('Package_Desc1')
				select @Package_Desc1 = CAST(@ColumnValue AS decimal(9,4))
			else
			if @ColumnName = LOWER('Package_Desc2')
				select @Package_Desc2 = CAST(@ColumnValue AS decimal(9,4))
			else
			if @ColumnName = LOWER('Retail_Sale')
				if ((CONVERT(FLOAT, @Identifier) >= 46000000000 And CONVERT(FLOAT, @Identifier)  <= 46999999999) Or (CONVERT(FLOAT, @Identifier) >= 48000000000 And CONVERT(FLOAT, @Identifier) <= 48999999999))
					select @Retail_Sale = 0
				else
					select @Retail_Sale = CAST(@ColumnValue AS bit)
			else
			if @ColumnName = LOWER('ClassID')
				select @ClassID = CAST(@ColumnValue AS int)
			else
			if @ColumnName = LOWER('CostedByWeight')
				select @CostedByWeight = CAST(@ColumnValue AS bit)
			else
			if @ColumnName = LOWER('Vendor_Unit_ID')
				select @Vendor_Unit_ID = CAST(@ColumnValue AS int)
			else
			if @ColumnName = LOWER('Distribution_Unit_ID')
				select @Distribution_Unit_ID = CAST(@ColumnValue AS int)
			else
			if @ColumnName = LOWER('TaxClassID')
				select @TaxClassID = CAST(@ColumnValue AS int)
			else
			if @ColumnName = LOWER('LabelType_ID')
				select @LabelType_ID = CAST(@ColumnValue AS int)
			else
			if @ColumnName = LOWER('Brand_ID')
				select @Brand_ID = CAST(@ColumnValue AS int)				
			else
			if @ColumnName = LOWER('ProdHierarchyLevel4_ID') AND @FourLevelHierarchyFlag = 1
				select @ProdHierarchyLevel4_ID = CAST(@ColumnValue AS int)
				
			-- store jurisdiction values
			ELSE IF @ColumnName = LOWER('IsDefaultJurisdiction') 
				SELECT  @IsDefaultJurisdiction = CAST(@ColumnValue AS bit)
			ELSE IF @ColumnName = LOWER('StoreJurisdictionID') 
				SELECT  @StoreJurisdictionID = CAST(@ColumnValue AS int)
			else			
			if @ColumnName = LOWER('WFM_Item')
				select @WFM_Item = CAST(ISNULL(@ColumnValue,1) AS bit)
			else			
			if @ColumnName = LOWER('HFM_Item')
				select @HFM_Item = CAST(CAST(ISNULL(@ColumnValue,0) AS bit) AS tinyint)
 					
		END
		ELSE 
			IF @TableName = 'itemsignattribute'
				BEGIN
					IF @ColumnName = LOWER('Locality') 
						SELECT  @Locality = CAST(@ColumnValue AS varchar(50))
					ELSE IF @ColumnName = LOWER('SignRomanceTextShort')
						SELECT  @ShortSignRomance = CAST(@ColumnValue AS varchar(140))	
					ELSE IF @ColumnName = LOWER('SignRomanceTextLong')
						SELECT  @LongSignRomance = CAST(@ColumnValue AS varchar(300))	
					ELSE IF @ColumnName = LOWER('UomRegulationChicagoBaby')
						SELECT  @ChicagoBaby = CAST(@ColumnValue AS varchar(50))	
					ELSE IF @ColumnName = LOWER('UomRegulationTagUom')
						SELECT  @TagUom = CAST(@ColumnValue AS int)	
					ELSE IF @ColumnName = LOWER('Exclusive')
						SELECT  @Exclusive = CAST(@ColumnValue AS date)	
					ELSE IF @ColumnName = LOWER('ColorAdded')
						SELECT  @ColorAdded = NULLIF(CAST(@ColumnValue AS bit), 0)
				END
		ELSE IF @TableName = 'itemidentifier'
		BEGIN
			if @ColumnName = LOWER('IdentifierType')
				select @IdentifierType = CAST(@ColumnValue AS char(1))
			else
			if @ColumnName = LOWER('National_Identifier')
				select @National_Identifier = CASE WHEN LOWER(@ColumnValue) = 'true' THEN 1 ELSE 0 END
			ELSE
			IF @ColumnName = LOWER('Identifier')
				select @Identifier = CAST(@ColumnValue AS varchar(13))
			else
			if @ColumnName = LOWER('Scale_Identifier')
				select @Scale_Identifier = CAST(@ColumnValue AS bit)
			else
			if @ColumnName = LOWER('NumPluDigitsSentToScale')
				select @NumPluDigitsSentToScale = CAST(@ColumnValue AS int)
			else
			if @ColumnName = LOWER('CheckDigit')
				select @CheckDigit = CAST(@ColumnValue AS char(1))
		END
		
		-- get the row's next upload value
		FETCH NEXT FROM UploadValue_cursor INTO
			@UploadValue_ID, @ColumnValue, @TableName,
			@ColumnName, @columnDbDataType
			
	END -- looping through the row's upload values

	CLOSE UploadValue_cursor
	DEALLOCATE UploadValue_cursor

	IF @UseStoreJurisdictions = 0
		BEGIN
			SELECT @StoreJurisdictionID = (SELECT TOP 1 StoreJurisdictionID
				FROM StoreJurisdiction)
		END

	-- only create a new item if the region is not using store jurisdictions
	-- or they are and the upload row is not for the item's
	-- default jurisdiction
	If @UseStoreJurisdictions = 0 OR
			(@UseStoreJurisdictions = 1 AND @IsDefaultJurisdiction = 1)
	BEGIN

	  BEGIN TRY
		  SET @WFM_Item = (ISNULL(@WFM_Item,1))
		  SET @HFM_Item = (ISNULL(@HFM_Item,0))
		  -- create the new item
		  EXEC @Item_Key = dbo.InsertItem
			  @POS_Description,
			  @Item_Description,
			  @SubTeam_No,
			  @Category_ID,
			  @Retail_Unit_ID,
			  @Package_Unit_ID,
			  @Package_Desc1,
			  @Package_Desc2,
			  @IdentifierType,
			  @Identifier,
			  @CheckDigit,
			  null, --@User_ID
			  @Retail_Sale,
			  @ClassID,
			  @CostedByWeight,
			  @Vendor_Unit_ID,
			  @Distribution_Unit_ID,   
			  @TaxClassID,
			  @LabelType_ID,
			  @Brand_ID,
			  @National_Identifier,
			  @NumPluDigitsSentToScale,
			  @Scale_Identifier,
			  @StoreJurisdictionID,
			  @ProdHierarchyLevel4_ID,
			  'EIM', -- InsertApplication,
			  0, --Food_Stamps
			  0, --Organic
			  NULL, --Manager_ID
			  @WFM_Item,
			  @HFM_Item
		
		  EXEC dbo.EIM_Log @LoggingLevel, 'TRACE', @UploadSession_ID, @UploadRow_ID, @RetryCount, @Item_key, NULL, '2.1 New Item Creation - [InsertItem]'
  
	  END TRY
	  BEGIN CATCH
  
			  EXEC dbo.EIM_LogAndRethrowException @UploadSession_ID, @UploadRow_ID, @RetryCount, @Item_key, NULL, '2.1 New Item Creation Error - [InsertItem]'
	  END CATCH
			
	END	
	ELSE
	BEGIN
		-- even if we are not creating a new item because the upload row is
		-- for a new alternate jurisdiction we still need to go get the item_key
		-- from the Item table
		-- note that this requires that default jurisdiction item upload rows
		-- must be processed first in a session upload
		SELECT @Item_Key = Item.Item_Key
		FROM dbo.Item (NOLOCK)
			JOIN dbo.ItemIdentifier(NOLOCK) ON ItemIdentifier.Item_Key = Item.Item_Key
		WHERE Identifier = @Identifier
	
	END

	-- Update sign attributes (if present)
	IF ((@Locality is not null) OR (@ShortSignRomance is not null) OR (@LongSignRomance is not null) OR
		(@ChicagoBaby is not null) OR (@TagUom is not null) OR (@Exclusive is not null) OR (@ColorAdded is not null))
		BEGIN
			BEGIN TRY
				EXEC dbo.InsertOrUpdateItemSignAttribute
					@Item_key,
					@Locality,
					@LongSignRomance,
					@ShortSignRomance,
					null,
					null,
					null,
					null,
					null,
					null,
					null,
					null,
					null,
					null,
					null,
					null,
					null,
					null,
					null,
					null,
					null,
					@ChicagoBaby,
					@TagUom,
					@Exclusive,
					@ColorAdded
			  
				EXEC dbo.EIM_Log @LoggingLevel, 'TRACE', @UploadSession_ID, @UploadRow_ID, @RetryCount, @Item_key, NULL, '2.2 New Item Creation - [InsertOrUpdateItemSignAttribute]'
			END TRY
			BEGIN CATCH
				EXEC dbo.EIM_LogAndRethrowException @UploadSession_ID, @UploadRow_ID, @RetryCount, @Item_key, NULL, '2.2 New Item Creation Error - [InsertOrUpdateItemSignAttribute]'
			END CATCH
		END

	
	-- update the item_key in the upload rows for that identifier
	-- note, we have to search by identifier and not UploadRow_ID
	-- since we didn't create a new item for each row, but instead,
	-- for each unique identifier
	UPDATE UploadRow SET Item_Key = @Item_Key WHERE Identifier = @Identifier
	
	EXEC dbo.EIM_Log @LoggingLevel, 'TRACE', @UploadSession_ID, @UploadRow_ID, @RetryCount, @Item_key, NULL, '2.3 New Item Creation - [End]'

GO