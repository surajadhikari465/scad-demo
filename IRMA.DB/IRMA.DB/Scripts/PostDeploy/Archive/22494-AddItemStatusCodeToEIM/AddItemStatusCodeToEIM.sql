		-- ******************* EIM Populate Data ************************
	DECLARE @UploadAttribute_Id int
	DECLARE @UploadTypeAttribute_Id int
	DECLARE @ChangeDataId int
	DECLARE @UploadTypeTemplate_ID int
	DECLARE @MaxSpreadSheetPosition int
	DECLARE @GridPosition int
		
	SELECT  @MaxSpreadSheetPosition = max(SpreadSheetPosition) + 1 FROM UploadAttribute

	IF NOT EXISTS 
	(SELECT * FROM UploadAttribute WHERE Name = 'Item Status Code')
	INSERT INTO 
	[UploadAttribute]
	([Name], [TableName], [ColumnNameOrKey], [ControlType], [DbDataType], [Size], [DisplayFormatString], [IsRequiredValue], [IsCalculated], [DefaultValue], [OptionalMinValue], [OptionalMaxValue], [IsActive], [PopulateProcedure], [PopulateIndexField], [PopulateDescriptionField], [SpreadsheetPosition],[ValueListStaticData]) 
	VALUES('Item Status Code', 'storeitemextended', 'itemstatuscode', 'ValueList', 'int', NULL, NULL, 0, 0, NULL, NULL, NULL, 1,  NULL,  NULL,  NULL, @MaxSpreadSheetPosition, '10|10,11|11,12|12,13|13,14|14,15|15,16|16,17|17,18|18,19|19,20|20,21|21,22|22,23|23,24|24,25|25,26|26,27|27,28|28,29|29,30|30,31|31,32|32,33|33,34|34,35|35,36|36,37|37,38|38,39|39,40|40,41|41,42|42,43|43,44|44,45|45,46|46,47|47,48|48,49|49,50|50,51|51,52|52,53|53,54|54,55|55,56|56,57|57,58|58,59|59')

	-- ***** Get inserted UploadAttribute_ID ******
	SELECT  @UploadAttribute_Id = UploadAttribute_ID FROM UploadAttribute WHERE Name = 'Item Status Code'


	-- ************ UPLOADTYPEATTRIBUTE **********
	-- ******* Determine the next Grid position *********
	SELECT  @GridPosition = max(GridPosition) + 1 FROM UploadTypeAttribute where UploadType_Code = 'PRICE_UPLOAD'

	IF NOT EXISTS 
	(SELECT * FROM UploadTypeAttribute WHERE UploadType_Code = 'PRICE_UPLOAD' AND UploadAttribute_ID = @UploadAttribute_ID)
	INSERT INTO 
	UploadTypeAttribute 
	(UploadType_Code, UploadAttribute_ID, IsRequiredForUploadTypeForExistingItems, IsReadOnlyForExistingItems, IsHidden, GridPosition, IsRequiredForUploadTypeForNewItems, IsReadOnlyForNewItems, GroupName)
	VALUES 
	('PRICE_UPLOAD', @UploadAttribute_ID, 0, 0, 0, @GridPosition, 0, 0, 'Item / Store Data')


	-- ************ UPLOADTYPETEMPLATEATTRIBUTE *****************
	SELECT  @UploadTypeAttribute_Id = UploadTypeAttribute_ID FROM UploadTypeAttribute WHERE UploadType_Code = 'PRICE_UPLOAD' AND UploadAttribute_ID = @UploadAttribute_ID
	SELECT  @UploadTypeTemplate_Id = UploadTypeTemplate_ID FROM UploadTypeTemplate WHERE UploadType_Code = 'PRICE_UPLOAD'AND Name = '- All Attributes -'

	IF NOT EXISTS 
	(SELECT * FROM UploadTypeTemplateAttribute WHERE UploadTypeTemplate_ID = @UploadTypeTemplate_Id AND UploadTypeAttribute_ID = @UploadTypeAttribute_ID)
	INSERT INTO UploadTypeTemplateAttribute (UploadTypeTemplate_ID, UploadTypeAttribute_ID)
	VALUES (@UploadTypeTemplate_Id, @UploadTypeAttribute_Id)
GO

