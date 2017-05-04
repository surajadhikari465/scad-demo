
CREATE PROCEDURE [dbo].[Scale_InsertUpdateItemScaleDetails]
	@ItemScale_ID int,
	@Item_Key int,
	@Nutrifact_ID int,
	@Scale_ExtraText_ID int,
	@Scale_Tare_ID int,
	@Scale_Alternate_Tare_ID int,
	@Scale_LabelStyle_ID int,
	@Scale_EatBy_ID smallint,
	@Scale_Grade_ID int,	
	@Scale_RandomWeightType_ID int,
	@Scale_ScaleUOMUnit_ID int,
	@Scale_FixedWeight varchar(25),
	@Scale_ByCount int,
	@ForceTare bit,
	@PrintBlankShelfLife bit,	
	@PrintBlankEatBy bit,
	@PrintBlankPackDate bit,
	@PrintBlankWeight bit,
	@PrintBlankUnitPrice bit,
	@PrintBlankTotalPrice bit,
	@Scale_Description1 varchar(64),
	@Scale_Description2 varchar(64),
	@Scale_Description3 varchar(64),
	@Scale_Description4 varchar(64),
	@ShelfLife_Length smallint,
	@User_ID int,
	@User_ID_Date varchar(255),
	@CustomerFacingScaleDepartment bit = null,
	@SendToScale bit = null
AS

BEGIN
	IF @CustomerFacingScaleDepartment IS NOT NULL AND @SendToScale IS NOT NULL
		BEGIN
			IF EXISTS (SELECT Item_Key FROM dbo.ItemCustomerFacingScale icfs WHERE icfs.Item_Key = @Item_Key)
				begin
					UPDATE 
						ItemCustomerFacingScale
					SET 
						CustomerFacingScaleDepartment = @CustomerFacingScaleDepartment,
						SendToScale = @SendToScale
					WHERE 
						Item_Key = @Item_Key
				end
			ELSE
				begin
					INSERT INTO dbo.ItemCustomerFacingScale(Item_Key, CustomerFacingScaleDepartment, SendToScale) VALUES (@Item_Key, @CustomerFacingScaleDepartment, @SendToScale)
				end
		END

	IF @ItemScale_ID > 0
		BEGIN
		-- UPDATE SCALE DATA FOR THE ASSOCIATED ITEM_KEY
			UPDATE 
				ItemScale
			SET 
				Nutrifact_ID = @Nutrifact_ID,
				Scale_ExtraText_ID = @Scale_ExtraText_ID,
				Scale_Tare_ID = @Scale_Tare_ID,
				Scale_Alternate_Tare_ID = @Scale_Alternate_Tare_ID,
				Scale_LabelStyle_ID = @Scale_LabelStyle_ID,
				Scale_EatBy_ID = @Scale_EatBy_ID,
				Scale_Grade_ID = @Scale_Grade_ID,	
				Scale_RandomWeightType_ID = @Scale_RandomWeightType_ID,
				Scale_ScaleUOMUnit_ID = @Scale_ScaleUOMUnit_ID,
				Scale_FixedWeight = @Scale_FixedWeight,
				Scale_ByCount = @Scale_ByCount,
				ForceTare = @ForceTare,
				PrintBlankShelfLife = @PrintBlankShelfLife,	
				PrintBlankEatBy = @PrintBlankEatBy,
				PrintBlankPackDate = @PrintBlankPackDate,
				PrintBlankWeight = @PrintBlankWeight,
				PrintBlankUnitPrice = @PrintBlankUnitPrice,
				PrintBlankTotalPrice = @PrintBlankTotalPrice,
				Scale_Description1 = @Scale_Description1,
				Scale_Description2 = @Scale_Description2,
				Scale_Description3 = @Scale_Description3,
				Scale_Description4 = @Scale_Description4,
				ShelfLife_Length = @ShelfLife_Length 
			WHERE 
				ItemScale_ID = @ItemScale_ID		
		END
	ELSE
		BEGIN
			INSERT INTO  
				ItemScale
				(Item_Key, Nutrifact_ID, Scale_ExtraText_ID, Scale_Tare_ID, Scale_Alternate_Tare_ID, 
				Scale_LabelStyle_ID, Scale_EatBy_ID, Scale_Grade_ID, 
				Scale_RandomWeightType_ID, Scale_ScaleUOMUnit_ID,
				Scale_FixedWeight, Scale_ByCount, ForceTare, PrintBlankShelfLife,
				PrintBlankEatBy, PrintBlankPackDate, PrintBlankWeight, PrintBlankUnitPrice,
				PrintBlankTotalPrice, Scale_Description1, Scale_Description2, Scale_Description3,
				Scale_Description4, ShelfLife_Length )
			VALUES
				(@Item_Key,
				@Nutrifact_ID,
				@Scale_ExtraText_ID,
				@Scale_Tare_ID,
				@Scale_Alternate_Tare_ID,
				@Scale_LabelStyle_ID,
				@Scale_EatBy_ID,
				@Scale_Grade_ID,	
				@Scale_RandomWeightType_ID,
				@Scale_ScaleUOMUnit_ID,
				@Scale_FixedWeight,
				@Scale_ByCount,
				@ForceTare,
				@PrintBlankShelfLife,	
				@PrintBlankEatBy,
				@PrintBlankPackDate,
				@PrintBlankWeight,
				@PrintBlankUnitPrice,
				@PrintBlankTotalPrice,
				@Scale_Description1,
				@Scale_Description2,
				@Scale_Description3,
				@Scale_Description4,
				@ShelfLife_Length 
			)
		END
		
	UPDATE Item SET LastModifiedUser_ID = @User_ID, LastModifiedDate = @User_ID_Date WHERE Item_Key = @Item_Key
	
	--Insert Mammoth Events
	EXEC mammoth.InsertItemLocaleChangeQueue @Item_Key, NULL, 'ItemLocaleAddOrUpdate', NULL, NULL
END

GO
GRANT EXECUTE
    ON OBJECT::[dbo].[Scale_InsertUpdateItemScaleDetails] TO [IRMAClientRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[Scale_InsertUpdateItemScaleDetails] TO [IRMASLIMRole]
    AS [dbo];

