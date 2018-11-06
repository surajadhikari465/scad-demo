IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[dbo].[Scale_GetItemScaleOverride]') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
	DROP PROCEDURE [dbo].[Scale_GetItemScaleOverride]
GO

CREATE PROCEDURE dbo.Scale_GetItemScaleOverride
    @Item_Key				int,
    @StoreJurisdictionID	int

AS 

-- ****************************************************************************************************************
-- Procedure: Scale_GetItemScaleOverride()
--    Author: unknown
--      Date: unknown
--
-- Description:
-- Called from StoreJurisdictionDAO.vb.
--
-- Modification History:
-- Date       	Init  	TFS   	Comment
-- 2013-01-21	KM		9394	Add this update history template; Add the new 4.8 values ForceTare and Scale_Alternate_Tare_ID to the selection;
-- 2013-01-25	KM		9382	Add Scale_EatBy_ID and Scale_Grade_ID to the selection;
-- 2013-01-25	KM		9382	Add all of the PrintBlank columns to the selection;
-- 2013-01-29	KM		9393	Add Nutrifact_ID to the selection;
-- ****************************************************************************************************************

BEGIN
	SELECT
		ItemScaleOverride.Scale_Description1,
		ItemScaleOverride.Scale_Description2,
		ItemScaleOverride.Scale_Description3,
		ItemScaleOverride.Scale_Description4,
		ItemScaleOverride.Scale_ExtraText_ID,
		Scale_ExtraText.Description AS Scale_ExtraText,
		ItemScaleOverride.Scale_Tare_ID,
		ItemScaleOverride.Scale_LabelStyle_ID,
		ItemScaleOverride.Scale_ScaleUOMUnit_ID,
		ItemScaleOverride.Scale_RandomWeightType_ID,
		ItemScaleOverride.Scale_FixedWeight,
		ItemScaleOverride.Scale_ByCount,
		ItemScaleOverride.ShelfLife_Length,
		ItemScaleOverride.ForceTare,
		ItemScaleOverride.Scale_Alternate_Tare_ID,
		ItemScaleOverride.Scale_EatBy_ID,
		ItemScaleOverride.Scale_Grade_ID,
		ItemScaleOverride.PrintBlankEatBy,
		ItemScaleOverride.PrintBlankPackDate,
		ItemScaleOverride.PrintBlankShelfLife,
		ItemScaleOverride.PrintBlankTotalPrice,
		ItemScaleOverride.PrintBlankUnitPrice,
		ItemScaleOverride.PrintBlankWeight,
		ItemScaleOverride.Nutrifact_ID

	FROM 
		ItemScaleOverride
		LEFT OUTER JOIN Scale_ExtraText ON ItemScaleOverride.Scale_ExtraText_ID = Scale_ExtraText.Scale_ExtraText_ID
	
	WHERE 
		Item_Key = @Item_Key AND StoreJurisdictionID = @StoreJurisdictionID
END
GO