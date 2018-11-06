	-- ******************* EIM Populate Data ************************
	DECLARE @UploadAttribute_Id int
	DECLARE @UploadTypeAttribute_Id int
	DECLARE @ChangeDataId int
	DECLARE @UploadTypeTemplate_ID int
	DECLARE @MaxSpreadSheetPosition int
	DECLARE @GridPosition int
		
	SELECT  @MaxSpreadSheetPosition = max(SpreadSheetPosition) + 1 FROM UploadAttribute

	IF NOT EXISTS 
	(SELECT * FROM UploadAttribute WHERE Name = 'Ordered By Infor')
	INSERT INTO 
	[UploadAttribute]
	([Name], [TableName], [ColumnNameOrKey], [ControlType], [DbDataType], [Size], [DisplayFormatString], [IsRequiredValue], [IsCalculated], [DefaultValue], [OptionalMinValue], [OptionalMaxValue], [IsActive], [PopulateProcedure], [PopulateIndexField], [PopulateDescriptionField], [SpreadsheetPosition],[ValueListStaticData]) 
	VALUES('Ordered By Infor', 'storeitemextended', 'orderedbyinfor', 'CheckBox', 'bit', NULL, NULL, 0, 0, 0, NULL, NULL, 1,  NULL,  NULL,  NULL, @MaxSpreadSheetPosition, NULL)

	-- ***** Get inserted UploadAttribute_ID ******
	SELECT  @UploadAttribute_Id = UploadAttribute_ID FROM UploadAttribute WHERE Name = 'Ordered By Infor'


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

