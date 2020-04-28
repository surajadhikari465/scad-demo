DECLARE @region varchar(30) = (SELECT REGIONCODE FROM REGION)
DECLARE @MaxPosition integer
IF @region = 'MA'
BEGIN
	SELECT  @MaxPosition = max(SpreadSheetPosition) + 1 
	  FROM UploadAttribute;

	INSERT INTO dbo.UploadAttribute (Name, TableName, columnNameOrKey, ControlType, DbDataType, Size, IsRequiredValue, IsCalculated, OptionalMinValue, OptionalMaxValue, IsActive, DisplayFormatString, PopulateProcedure, PopulateIndexField, PopulateDescriptionField, SpreadsheetPosition, ValueListStaticData, DefaultValue)
	VALUES ( 'Ingredient', 'item', 'ingredient', 'CheckBox', 'bit', null, 0, 0, null, null, 1, null, null, null, null, @MaxPosition, null, null);
	INSERT INTO dbo.UploadAttribute (Name, TableName, columnNameOrKey, ControlType, DbDataType, Size, IsRequiredValue, IsCalculated, OptionalMinValue, OptionalMaxValue, IsActive, DisplayFormatString, PopulateProcedure, PopulateIndexField, PopulateDescriptionField, SpreadsheetPosition, ValueListStaticData, DefaultValue)
	VALUES ( 'SustainabilityRankingRequired', 'item', 'sustainabilityrankingrequired', 'CheckBox', 'bit', null, 0, 0, null, null, 1, null, null, null, null, @MaxPosition+1, null, null);
	INSERT INTO dbo.UploadAttribute (Name, TableName, columnNameOrKey, ControlType, DbDataType, Size, IsRequiredValue, IsCalculated, OptionalMinValue, OptionalMaxValue, IsActive, DisplayFormatString, PopulateProcedure, PopulateIndexField, PopulateDescriptionField, SpreadsheetPosition, ValueListStaticData, DefaultValue)
	VALUES ( 'SustainabilityRankingID', 'item', 'sustainabilityrankingid', 'ValueList', 'tinyint', null, 0, 0, null, null, 1, null, 'GetSustainabilityRankings', 'ID', 'RankingDescription', @MaxPosition+2, null, null);
	
	SELECT @MaxPosition = max(GridPosition) + 1 
	  FROM UploadTypeAttribute 
	 WHERE UploadType_Code = 'ITEM_MAINTENANCE'
	   AND GroupName = 'Other Item Data';

	INSERT INTO UploadTypeAttribute (UploadType_Code, UploadAttribute_ID, IsRequiredForUploadTypeForExistingItems, IsReadOnlyForExistingItems, IsHidden, GridPosition, IsRequiredForUploadTypeForNewItems, IsReadOnlyForNewItems, GroupName) 
	VALUES ('ITEM_MAINTENANCE', (SELECT UploadAttribute_ID FROM UploadAttribute WHERE columnNameOrKey = 'ingredient'), 0, 0, 0, @MaxPosition, 0, 0, 'Other Item Data'); -- Ingredient
	INSERT INTO UploadTypeAttribute (UploadType_Code, UploadAttribute_ID, IsRequiredForUploadTypeForExistingItems, IsReadOnlyForExistingItems, IsHidden, GridPosition, IsRequiredForUploadTypeForNewItems, IsReadOnlyForNewItems, GroupName) 
	VALUES ('ITEM_MAINTENANCE', (SELECT UploadAttribute_ID FROM UploadAttribute WHERE columnNameOrKey = 'sustainabilityrankingrequired'), 0, 0, 0, @MaxPosition+1, 0, 0, 'Other Item Data'); -- SustainabilityRankingRequired
	INSERT INTO UploadTypeAttribute (UploadType_Code, UploadAttribute_ID, IsRequiredForUploadTypeForExistingItems, IsReadOnlyForExistingItems, IsHidden, GridPosition, IsRequiredForUploadTypeForNewItems, IsReadOnlyForNewItems, GroupName) 
	VALUES ('ITEM_MAINTENANCE', (SELECT UploadAttribute_ID FROM UploadAttribute WHERE columnNameOrKey = 'sustainabilityrankingid'), 0, 0, 0, @MaxPosition+2, 0, 0, 'Other Item Data'); -- SustainabilityRankingID
END;
GO
