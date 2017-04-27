IF NOT EXISTS (	SELECT * FROM dbo.sysobjects WHERE OBJECTPROPERTY(id, N'IsProcedure') = 1 AND
	id = OBJECT_ID(N'[dbo].[InsertItem]') 
)
BEGIN
	EXEC ('CREATE PROCEDURE dbo.InsertItem AS SELECT 1')
END
GO

SET QUOTED_IDENTIFIER ON 
GO
SET ANSI_NULLS ON 
GO

ALTER PROCEDURE [dbo].[InsertItem]
    @POS_Description			varchar(26),
    @Item_Description			varchar(60),
    @SubTeam_No					int,
    @Category_ID				int,
    @Retail_Unit_ID				int,
    @Package_Unit_ID			int,
    @Package_Desc1				decimal(9,4),
    @Package_Desc2				decimal(9,4),
    @IdentifierType				char(1),
    @Identifier					varchar(13),
    @CheckDigit					char(1),
    @User_ID					int,
    @Retail_Sale				bit,
    @ClassID					int,
    @CostedByWeight				bit,
    @Vendor_Unit_ID				int,
    @Distribution_Unit_ID		int,   
    @TaxClassID					int,
	@LabelType_ID				int,
	@Brand_ID					int,
	@National_Identifier		tinyint,
	@NumPluDigitsSentToScale	int,
	@Scale_Identifier			bit,
	@StoreJurisdictionID		int,
	
	-- new params placed at the end so they can be optional
	-- to not break existing calls that do not pass in a value for them
    @ProdHierarchyLevel4_ID		int = NULL,
    @InsertApplication			varchar(30) = NULL,
	@Food_Stamps				bit = 0,
	@Organic                    bit = 0,
    @Manager_ID					tinyint = NULL,
	@WFM_Item					bit = 1,
	@HFM_Item					tinyint = 0
AS

-- ****************************************************************************************************************
-- Procedure: [InsertItem]
--    Author: unknown
--      Date: unknown
--
-- Description:
-- Called from ItemAdd.vb.  Creates a new Item record.
--
-- Modification History:
-- Date       	Init  	TFS   	Comment
-- 2012-12-14	KM		8747	Add update history template; Set StoreJurisdictionID to 1 for all new items in the Item insert
--								and the ItemChangeHistory insert; Insert into ItemOverride if an alternate jurisdiction is chosen at item creation;
-- 2012-04-01   MZ      14921   Added Retail_Sale flag check when inserting a new item into iConItemChangeQueue. 
--                              Only Retail Sale items need to be sent to iCon.      
-- 2012-04-21	KM		14989	For now, don't add new item events to the queue for PLUs.   
-- 2014-05-01	DN		15014	Added a new parameter @Food_Stamps. INSERT statements for tables Item, ItemOverride, and ItemChangeHistory will 
--								include the value of @Food_Stamps.   
-- 2012-06-16   MZ      15157   Checked the two app config keys (EnableUPCIRMAToIConFlow and EnablePLUIRMAIConFlow). Depending on the config keys' seeting, 
--                              insert new item events into iConItemChangeQueue only for UPCs or PLUs or both.   
-- 2015-08-24   MZ      16385   Also generates new item event for Icon for non-retail items in specified ranges.
-- 2015-10-15	MZ		    	Added a new parameter @Organic. INSERT statements for tables Item, and ItemChangeHistory will 
--								include the value of @Organic.  
-- 2016-03-18	MU/MZ	TFS18686	
--						PBI13711	Adding Sold By WFM and Sold By 365 to accommodate setting via EIM
-- ****************************************************************************************************************

BEGIN
    SET NOCOUNT ON
 
    DECLARE @Error_No int
    SELECT @Error_No = 0
    
    DECLARE @newItemChgTypeID tinyint
	SELECT @newItemChgTypeID = itemchgtypeid FROM itemchgtype WHERE itemchgtypedesc like 'new'

    DECLARE	@PriceChgTypeId int
    SELECT	@PriceChgTypeId = PriceChgTypeId
    FROM	PriceChgType
    WHERE	On_Sale = 0

	DECLARE @EnableUPCIRMAToIConFlow bit
	SELECT  @EnableUPCIRMAToIConFlow = acv.Value
			FROM AppConfigValue acv INNER JOIN AppConfigEnv ace
			ON acv.EnvironmentID = ace.EnvironmentID 
			INNER JOIN AppConfigApp aca
			ON acv.ApplicationID = aca.ApplicationID 
			INNER JOIN AppConfigKey ack
			ON acv.KeyID = ack.KeyID 
			WHERE aca.Name = 'IRMA Client' AND
			ack.Name = 'EnableUPCIRMAToIConFlow' and
			SUBSTRING(ace.Name,1,1) = SUBSTRING((SELECT Environment FROM Version WHERE ApplicationName = 'IRMA CLIENT'),1,1)
	
	DECLARE @EnablePLUIRMAIConFlow bit
	SELECT @EnablePLUIRMAIConFlow = acv.Value
			FROM AppConfigValue acv INNER JOIN AppConfigEnv ace
			ON acv.EnvironmentID = ace.EnvironmentID 
			INNER JOIN AppConfigApp aca
			ON acv.ApplicationID = aca.ApplicationID 
			INNER JOIN AppConfigKey ack
			ON acv.KeyID = ack.KeyID 
			WHERE aca.Name = 'IRMA Client' AND
			ack.Name = 'EnablePLUIRMAIConFlow' and
			SUBSTRING(ace.Name,1,1) = SUBSTRING((SELECT Environment FROM Version WHERE ApplicationName = 'IRMA CLIENT'),1,1)

    BEGIN TRANSACTION ItemInsert

    -- Insert a new record into the item table.  Set the default StoreJurisdictionID to the region's primary jurisdiction (ID = 1).
	INSERT INTO Item 
	(
		POS_Description, 
		Item_Description, 
		Sign_Description, 
		SubTeam_No, 
		Category_ID, 
		ProdHierarchyLevel4_ID, 
		Retail_Unit_ID, 
		Package_Unit_ID, 
		Package_Desc1, 
		Package_Desc2, 
		Retail_Sale, 
		ClassID, 
		CostedByWeight, 
		Vendor_Unit_ID, 
		Distribution_Unit_ID, 
		TaxClassID, 
		LabelType_ID, 
		Brand_ID, 
		Manager_ID,
		Food_Stamps,
		Organic,
		StoreJurisdictionID,
		WFM_Item,
		HFM_Item
	)

    VALUES 
	(
		@POS_Description, 
		@Item_Description, 
		@Item_Description, 
		@SubTeam_No, 
		@Category_ID, 
		@ProdHierarchyLevel4_ID, 
		@Retail_Unit_ID, 
		@Package_Unit_ID, 
		@Package_Desc1, 
		@Package_Desc2, 
		@Retail_Sale, 
		@ClassID, 
		@CostedByWeight, 
		@Vendor_Unit_ID, 
		@Distribution_Unit_ID, 
		@TaxClassID, 
		@LabelType_ID, 
		@Brand_ID, 
		@Manager_ID,
		@Food_Stamps, 
		@Organic,
		1,
		@WFM_Item,
		@HFM_Item
	)

	SELECT @Error_No = @@ERROR

	-- Grab the item key for the new item record so that it can be used in the rest of the updates.
	IF @Error_No = 0
		BEGIN
			DECLARE @LastItem int
			SELECT @LastItem = SCOPE_IDENTITY()
			SELECT @Error_No = @@ERROR
		END

	IF @StoreJurisdictionID > 1
		-- An alternate jurisdiction was chosen for initial item creation.  Populate ItemOverride.
		BEGIN
			INSERT INTO ItemOverride 
			(
				Item_Key,
				POS_Description, 
				Item_Description, 
				Sign_Description, 
				Retail_Unit_ID, 
				Package_Unit_ID, 
				Package_Desc1, 
				Package_Desc2, 
				CostedByWeight, 
				Vendor_Unit_ID, 
				Distribution_Unit_ID, 
				LabelType_ID,
				Brand_ID, 
				Food_Stamps,
				StoreJurisdictionID
			)

			VALUES
			(
				@LastItem,
				@POS_Description, 
				@Item_Description, 
				@Item_Description, 
				@Retail_Unit_ID, 
				@Package_Unit_ID, 
				@Package_Desc1, 
				@Package_Desc2, 
				@CostedByWeight, 
				@Vendor_Unit_ID, 
				@Distribution_Unit_ID, 
				@LabelType_ID, 
				@Brand_ID, 
				@Food_Stamps,
				@StoreJurisdictionID
			)
		END

	IF @Error_No = 0
    BEGIN
		-- Create the PriceBatchDetail record for the New item record.  This will prevent any of the insert/update triggers from
		-- inserting a second PriceBatchDetail record.
		INSERT INTO PriceBatchDetail (Store_No, Item_Key, ItemChgTypeID, User_ID, User_ID_Date, InsertApplication)
		SELECT Store_No, @LastItem, @newItemChgTypeID, @User_ID, GetDate(), @InsertApplication
		FROM Store
		WHERE WFM_Store = 1 OR Mega_Store = 1
	    
		SELECT @Error_No = @@ERROR
	END

	IF @Error_No = 0
	BEGIN
		-- IF SCALE ITEM THEN DEFAULT ScaleDesc1 = ITEM DESCRIPTION
		-- Note: This update no longer happens since the ItemScale table now holds the scale descriptions instead of the Item table
		-- UPDATE Item SET ScaleDesc1 = @Item_Description WHERE Item_Key = @LastItem AND dbo.fn_IsScaleItem(@Identifier) = 1
	
		SELECT @Error_No = @@ERROR
	END

    IF @Error_No = 0
    BEGIN
        INSERT INTO ItemChangeHistory (Item_Key, POS_Description, Item_Description, SubTeam_No, Category_ID, Retail_Unit_ID, Package_Unit_ID, Package_Desc1, Package_Desc2, ClassID, CostedByWeight, Vendor_Unit_ID, Distribution_Unit_ID, TaxClassID, LabelType_ID, Brand_ID, Manager_ID, Food_Stamps, Organic, StoreJurisdictionID)
        VALUES (@LastItem, @POS_Description, @Item_Description, @SubTeam_No, @Category_ID, @Retail_Unit_ID, @Package_Unit_ID, @Package_Desc1, @Package_Desc2, @ClassID, @CostedByWeight, @Vendor_Unit_ID, @Distribution_Unit_ID, @TaxClassID, @LabelType_ID, @Brand_ID, @Manager_ID, @Food_Stamps, @Organic, 1)

        SELECT @Error_No = @@ERROR
    END

    IF @Error_No = 0
    BEGIN
        -- Make sure the input Identifier is an integer
        DECLARE @X bigint
        SELECT @X = CAST(@Identifier as bigint)
        SELECT @Error_No = @@ERROR

        IF @Error_No = 0
        BEGIN
        
			DECLARE @NewItemAutoSkuFlag bit
						    
		    -- check the flag
			SELECT @NewItemAutoSkuFlag = dbo.fn_InstanceDataValue('NewItemAutoSku', NULL)

			If @NewItemAutoSkuFlag = 1
			BEGIN
			-- if the "NewItemAutoSku" instance data flag is set
			
				-- "auto-gen" a non-default sku using the item key as the
				-- identifier value
				INSERT INTO ItemIdentifier
					(Item_Key, Identifier, Add_Identifier, Default_Identifier, IdentifierType, CheckDigit, National_Identifier, NumPluDigitsSentToScale, Scale_Identifier)
				VALUES
					(@LastItem, @LastItem, 0, 1, 'S', 0, 0, 0, 0)

				-- [iCon] Not doing new-item event queue here because this would not be a "real" identifier that should go to iCon.

				IF @InsertApplication = 'EIM' AND
						@IdentifierType <> 'S' AND
						(@Identifier <> '' AND  @Identifier IS NOT NULL)
				BEGIN
				-- if the inserting application is "EIM" and the 
				-- passed in identifier type is not "S" and the
				-- identifier value is not empty or null				
											
					-- always create the user-defined identifier as the default
					INSERT INTO ItemIdentifier
						(Item_Key, Identifier, Add_Identifier, Default_Identifier, IdentifierType, CheckDigit, National_Identifier, NumPluDigitsSentToScale, Scale_Identifier)
					VALUES
						(@LastItem, @Identifier, 0, 1, @IdentifierType, CASE WHEN RTRIM(ISNULL(@CheckDigit, '')) = '' THEN NULL ELSE @CheckDigit END, @National_Identifier, @NumPluDigitsSentToScale, @Scale_Identifier)
					
					-- [iCon] Add identifier-add to event queue only if the item is marked as retail_sale item.
					-- Additionally, depending on the app config keys' value, add an event to the queue only for UPC or PLU or both.
					IF @Retail_Sale = 1 
					BEGIN
						IF	(@EnableUPCIRMAToIConFlow = 1 AND @EnablePLUIRMAIConFlow = 1) OR
							(@EnableUPCIRMAToIConFlow = 1 AND NOT (LEN(@Identifier) < 7 OR @Identifier LIKE '2%00000')) OR
							(@EnablePLUIRMAIConFlow = 1 AND (LEN(@Identifier) < 7 OR @Identifier LIKE '2%00000'))
						BEGIN
							insert into iConItemChangeQueue (
								Item_Key, 
								Identifier, 
								ItemChgTypeID
							)
							SELECT
								Item_Key = @LastItem,
								Identifier = @Identifier,
								ItemChgTypeID = @newItemChgTypeID

						END
					END
					ELSE
					BEGIN
						IF ((CONVERT(FLOAT, @Identifier) >= 46000000000 And CONVERT(FLOAT, @Identifier)  <= 46999999999) OR 
							(CONVERT(FLOAT, @Identifier) >= 48000000000 And CONVERT(FLOAT, @Identifier) <= 48999999999))
						BEGIN
							insert into iConItemChangeQueue (
								Item_Key, 
								Identifier, 
								ItemChgTypeID
							)
							SELECT
								Item_Key = @LastItem,
								Identifier = @Identifier,
								ItemChgTypeID = @newItemChgTypeID
						END
					END
					--RESET THE 'S' SKU VALUE TO *NOT* BE THE DEFAULT ANY MORE
					UPDATE ItemIdentifier SET Default_Identifier = 0 WHERE Item_Key = @LastItem AND Identifier = CAST(@LastItem AS varchar(200))
					
				END
			END
			ELSE
			BEGIN
			-- if the "NewItemAutoSku" instance data flag is *not* set
				INSERT INTO ItemIdentifier
					(Item_Key, Identifier, Add_Identifier, Default_Identifier, IdentifierType, CheckDigit, National_Identifier, NumPluDigitsSentToScale, Scale_Identifier)
				VALUES
					(@LastItem, @Identifier, 0, 1, @IdentifierType, CASE WHEN RTRIM(ISNULL(@CheckDigit, '')) = '' THEN NULL ELSE @CheckDigit END, @National_Identifier, @NumPluDigitsSentToScale, @Scale_Identifier)

				-- [iCon] Add identifier-add to event queue only if the item is marked as retail_sale item.
				-- Additionally, depending on the app config keys' value, add an event to the queue only for UPCs or PLUs or both.
				IF @Retail_Sale = 1 
				BEGIN
					IF	(@EnableUPCIRMAToIConFlow = 1 AND @EnablePLUIRMAIConFlow = 1) OR
						(@EnableUPCIRMAToIConFlow = 1 AND NOT (LEN(@Identifier) < 7 OR @Identifier LIKE '2%00000')) OR
						(@EnablePLUIRMAIConFlow = 1 AND (LEN(@Identifier) < 7 OR @Identifier LIKE '2%00000'))
					BEGIN
						insert into iConItemChangeQueue (
							Item_Key, 
							Identifier, 
							ItemChgTypeID
						)
						SELECT
							Item_Key = @LastItem,
							Identifier = @Identifier,
							ItemChgTypeID = @newItemChgTypeID
					END
				END
				ELSE
				BEGIN
					IF ((CONVERT(FLOAT, @Identifier) >= 46000000000 And CONVERT(FLOAT, @Identifier)  <= 46999999999) OR 
						(CONVERT(FLOAT, @Identifier) >= 48000000000 And CONVERT(FLOAT, @Identifier) <= 48999999999))
					BEGIN
						insert into iConItemChangeQueue (
							Item_Key, 
							Identifier, 
							ItemChgTypeID
							)
						SELECT
							Item_Key = @LastItem,
							Identifier = @Identifier,
							ItemChgTypeID = @newItemChgTypeID
					END
				END
			END

            SELECT @Error_No = @@ERROR 
        END   
    END

	--SETUP DEFAULT PRICE VALUES FOR ALL RETAIL STORES and DC's
    IF @Error_No = 0
    BEGIN
        INSERT Price (Item_Key, Store_No, PriceChgTypeId) 
        SELECT @LastItem, Store_No, @PriceChgTypeId
        FROM Store
        WHERE WFM_Store = 1 OR Mega_Store = 1 OR Distribution_Center = 1 OR Manufacturer = 1      

        SELECT @Error_No = @@ERROR
    END
    
    --SETUP DEFAULT STORE-ITEM RELATIONSHIP DATA FOR ALL RETAIL STORES and DC's; DEFAULT TO NOT-AUTHORIZED
    IF @Error_No = 0
    BEGIN
        INSERT StoreItem (Item_Key, Store_No, Authorized) 
        SELECT @LastItem, Store_No, 0
        FROM Store
        WHERE WFM_Store = 1 OR Mega_Store = 1 or Distribution_Center = 1 OR Manufacturer = 1     

        SELECT @Error_No = @@ERROR
    END

    IF @Error_No = 0
    BEGIN

		-- set up values for any configured default attributes
		EXEC @Error_No = SetItemDefaultValues
				@LastItem,
				@InsertApplication

    END

    IF @Error_No = 0
        SELECT @LastItem AS Item_Key
	
    IF @Error_No = 0
    BEGIN
        COMMIT TRANSACTION ItemInsert
        RETURN @LastItem
    END
    ELSE
    BEGIN
        ROLLBACK TRAN
        DECLARE @Severity smallint
        SELECT @Severity = ISNULL((SELECT severity FROM master.dbo.sysmessages WHERE error = @Error_No), 16)
        SET NOCOUNT OFF
        RAISERROR ('InsertItem failed with @@ERROR: %d', @Severity, 1, @Error_No)
        RETURN
    END
    
    SET NOCOUNT OFF
END

GO