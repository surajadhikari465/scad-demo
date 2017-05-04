IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[dbo].[Scale_InsertUpdateItemScaleOverride]') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
	DROP PROCEDURE [dbo].[Scale_InsertUpdateItemScaleOverride]
GO

CREATE PROCEDURE dbo.Scale_InsertUpdateItemScaleOverride
    @Item_Key					int,
    @StoreJurisdictionID		int,
	@Scale_Description1			varchar(64),
	@Scale_Description2			varchar(64),
	@Scale_Description3			varchar(64),
	@Scale_Description4			varchar(64),
	@Scale_ExtraText_ID			int,
	@Scale_Tare_ID				int,
	@Scale_LabelStyle_ID		int,
	@Scale_ScaleUOMUnit_ID		int,
	@Scale_RandomWeightType_ID	int,
	@Scale_FixedWeight			varchar(25),
	@Scale_ByCount				int,
	@ShelfLife_Length			smallint,
	@ForceTare					bit,
	@Scale_Alternate_Tare_ID	int,
	@Scale_EatBy_ID				int,
	@Scale_Grade_ID				int,
	@PrintBlankEatBy			bit,
	@PrintBlankPackDate			bit,
	@PrintBlankShelfLife		bit,
	@PrintBlankTotalPrice		bit,
	@PrintBlankUnitPrice		bit,
	@PrintBlankWeight			bit,
	@Nutrifact_ID				int = NULL

AS 

-- ****************************************************************************************************************
-- Procedure: Scale_InsertUpdateItemScaleOverride()
--    Author: unknown
--      Date: unknown
--
-- Description:
-- Called from StoreJurisdictionDAO.vb.
--
-- Modification History:
-- Date       	Init  	TFS   	Comment
-- 2013-01-21	KM		9394	Add update history template; Add ForceTare and Scale_Alternate_Tare_ID to the insert/update operations;
-- 2013-01-25	KM		9382	Add Scale_EatBy_ID and Scale_Grade_ID to the insert/update operations;
-- 2013-01-25	KM		9382	Add of the PrintBlank columns to the insert/update operations;
-- 2013-01-29	KM		9393	Add Nutrifact_ID column;
-- 2013-04-15   MZ      11918   Made @Nutrifact_ID optional by default it to NULL so that it won't break EIM Item Upload.
-- 2015-10-05   MZ      16646   Stage IRMA Item Locale Events for Alt Jurisdiction Changes 
-- ****************************************************************************************************************
DECLARE @OriginalScale_ExtraText_ID int

DECLARE @Error_No int

BEGIN
	-- Create the ItemScaleOverride record if it does not already exist
    IF NOT EXISTS(SELECT * FROM ItemScaleOverride WHERE Item_Key = @Item_Key AND StoreJurisdictionID = @StoreJurisdictionID)
		BEGIN
			INSERT INTO ItemScaleOverride (
				Item_Key,
				StoreJurisdictionID,
				Scale_Description1,
				Scale_Description2,
				Scale_Description3,
				Scale_Description4,
				Scale_ExtraText_ID,
				Scale_Tare_ID,
				Scale_LabelStyle_ID,
				Scale_ScaleUOMUnit_ID,
				Scale_RandomWeightType_ID,
				Scale_FixedWeight,
				Scale_ByCount,
				ShelfLife_Length,
				ForceTare,
				Scale_Alternate_Tare_ID,
				Scale_EatBy_ID,
				Scale_Grade_ID,
				PrintBlankEatBy,
				PrintBlankPackDate,
				PrintBlankShelfLife,
				PrintBlankTotalPrice,
				PrintBlankUnitPrice,
				PrintBlankWeight,
				Nutrifact_ID
			) VALUES (
				@Item_Key,
				@StoreJurisdictionID,
				@Scale_Description1,
				@Scale_Description2,
				@Scale_Description3,
				@Scale_Description4,
				@Scale_ExtraText_ID,
				@Scale_Tare_ID,
				@Scale_LabelStyle_ID,
				@Scale_ScaleUOMUnit_ID,
				@Scale_RandomWeightType_ID,
				@Scale_FixedWeight,
				@Scale_ByCount,
				@ShelfLife_Length,
				@ForceTare,
				@Scale_Alternate_Tare_ID,
				@Scale_EatBy_ID,
				@Scale_Grade_ID,
				@PrintBlankEatBy,
				@PrintBlankPackDate,
				@PrintBlankShelfLife,
				@PrintBlankTotalPrice,
				@PrintBlankUnitPrice,
				@PrintBlankWeight,
				@Nutrifact_ID
			)

			SELECT @Error_No = @@ERROR

			IF @Error_No = 0 
				IF @Scale_ExtraText_ID IS NOT NULL
					EXEC [mammoth].[InsertItemLocaleChangeQueue] @Item_Key, NULL, 'ItemLocaleAddOrUpdate', NULL, @StoreJurisdictionID
		END
	
	-- Otherwise, update the ItemScaleOverride data
	ELSE
		BEGIN
			SELECT
				@OriginalScale_ExtraText_ID = iso.Scale_ExtraText_ID
			  FROM
				ItemScaleOverride iso
			 WHERE   
				iso.Item_Key = @Item_Key
				AND iso.StoreJurisdictionID = @StoreJurisdictionID

			UPDATE ItemScaleOverride SET
				Scale_Description1			= @Scale_Description1,
				Scale_Description2			= @Scale_Description2,
				Scale_Description3			= @Scale_Description3,
				Scale_Description4			= @Scale_Description4,
				Scale_ExtraText_ID			= @Scale_ExtraText_ID,
				Scale_Tare_ID				= @Scale_Tare_ID,
				Scale_LabelStyle_ID			= @Scale_LabelStyle_ID,
				Scale_ScaleUOMUnit_ID		= @Scale_ScaleUOMUnit_ID,
				Scale_RandomWeightType_ID	= @Scale_RandomWeightType_ID,
				Scale_FixedWeight			= @Scale_FixedWeight,
				Scale_ByCount				= @Scale_ByCount,
				ShelfLife_Length			= @ShelfLife_Length,
				ForceTare					= @ForceTare,
				Scale_Alternate_Tare_ID		= @Scale_Alternate_Tare_ID,
				Scale_EatBy_ID				= @Scale_EatBy_ID,
				Scale_Grade_ID				= @Scale_Grade_ID,
				PrintBlankEatBy				= @PrintBlankEatBy,
				PrintBlankPackDate			= @PrintBlankPackDate,
				PrintBlankShelfLife			= @PrintBlankShelfLife,
				PrintBlankTotalPrice		= @PrintBlankTotalPrice,
				PrintBlankUnitPrice			= @PrintBlankUnitPrice,
				PrintBlankWeight			= @PrintBlankWeight,
				Nutrifact_ID				= @Nutrifact_ID
			WHERE 
				Item_Key = @Item_Key AND StoreJurisdictionID = @StoreJurisdictionID

			SELECT @Error_No = @@ERROR

			IF @Error_No = 0
				BEGIN
					IF (@OriginalScale_ExtraText_ID is not NULL AND @OriginalScale_ExtraText_ID <> @Scale_ExtraText_ID)
					 OR (@OriginalScale_ExtraText_ID is NULL AND @Scale_ExtraText_ID is not NULL)
						EXEC [mammoth].[InsertItemLocaleChangeQueue] @Item_Key, NULL, 'ItemLocaleAddOrUpdate', NULL, @StoreJurisdictionID
				END
		END
END
GO