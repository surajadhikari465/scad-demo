

--------------------------------------------------
-- The Fuax EIM_Jurisdiction_ItemScaleView Table
--------------------------------------------------

CREATE VIEW [dbo].[EIM_Jurisdiction_ItemScaleView] As
	SELECT
		 itms.ItemScale_ID
		,itms.Item_Key
		,itms.Nutrifact_ID
		,itmso.Scale_ExtraText_ID
		,itms.Scale_StorageData_ID
		,itmso.Scale_Tare_ID
		,itms.Scale_Alternate_Tare_ID
		,itmso.Scale_LabelStyle_ID
		,itms.Scale_EatBy_ID
		,itms.Scale_Grade_ID
		,itmso.Scale_RandomWeightType_ID
		,itmso.Scale_ScaleUOMUnit_ID
		,itmso.Scale_FixedWeight
		,itmso.Scale_ByCount
		,itms.ForceTare
		,itms.PrintBlankShelfLife
		,itms.PrintBlankEatBy
		,itms.PrintBlankPackDate
		,itms.PrintBlankWeight
		,itms.PrintBlankUnitPrice
		,itms.PrintBlankTotalPrice
		,itmso.Scale_Description1
		,itmso.Scale_Description2
		,itmso.Scale_Description3
		,itmso.Scale_Description4
		,itmso.ShelfLife_Length
		,itmso.StoreJurisdictionID
	FROM dbo.ItemScale itms (NOLOCK)
		INNER JOIN dbo.ItemScaleOverride itmso (NOLOCK)
			ON itmso.Item_Key = itms.Item_Key

	UNION

	SELECT
		 itms.ItemScale_ID
		,itms.Item_Key
		,itms.Nutrifact_ID
		,itms.Scale_ExtraText_ID
		,itms.Scale_StorageData_ID
		,itms.Scale_Tare_ID
		,itms.Scale_Alternate_Tare_ID
		,itms.Scale_LabelStyle_ID
		,itms.Scale_EatBy_ID
		,itms.Scale_Grade_ID
		,itms.Scale_RandomWeightType_ID
		,itms.Scale_ScaleUOMUnit_ID
		,itms.Scale_FixedWeight
		,itms.Scale_ByCount
		,itms.ForceTare
		,itms.PrintBlankShelfLife
		,itms.PrintBlankEatBy
		,itms.PrintBlankPackDate
		,itms.PrintBlankWeight
		,itms.PrintBlankUnitPrice
		,itms.PrintBlankTotalPrice
		,itms.Scale_Description1
		,itms.Scale_Description2
		,itms.Scale_Description3
		,itms.Scale_Description4
		,itms.ShelfLife_Length
		,itm.StoreJurisdictionID
	FROM dbo.ItemScale itms (NOLOCK)
		INNER JOIN dbo.Item itm (NOLOCK)
			ON itm.Item_Key = itms.Item_Key

GO
GRANT SELECT
    ON OBJECT::[dbo].[EIM_Jurisdiction_ItemScaleView] TO [IRMAClientRole]
    AS [dbo];


GO
GRANT SELECT
    ON OBJECT::[dbo].[EIM_Jurisdiction_ItemScaleView] TO [IRMAReportsRole]
    AS [dbo];

