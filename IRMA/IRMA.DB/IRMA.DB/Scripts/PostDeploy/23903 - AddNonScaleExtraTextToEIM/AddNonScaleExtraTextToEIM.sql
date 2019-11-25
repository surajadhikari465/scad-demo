-- ******************* EIM Populate Data ************************
DECLARE @UploadAttribute_Id int
DECLARE @UploadTypeAttribute_Id int
DECLARE @ChangeDataId int
DECLARE @UploadTypeTemplate_ID int
DECLARE @MaxSpreadSheetPosition int
DECLARE @GridPosition int
		
SELECT  @MaxSpreadSheetPosition = max(SpreadSheetPosition) + 1 FROM UploadAttribute

IF NOT EXISTS (SELECT * FROM UploadAttribute WHERE Name = 'NonScale Extra Text')
INSERT INTO [UploadAttribute]([Name], [TableName], [ColumnNameOrKey], [ControlType], [DbDataType], [Size], [DisplayFormatString], [IsRequiredValue], [IsCalculated], [DefaultValue], [OptionalMinValue], [OptionalMaxValue], [IsActive], [PopulateProcedure], [PopulateIndexField], [PopulateDescriptionField], [SpreadsheetPosition],[ValueListStaticData]) 
VALUES('NonScale Extra Text', 'item_extratext', 'extratext', 'TextBox', 'varchar', 4200, NULL, 0, 0, NULL, NULL, NULL, 1,  NULL,  NULL,  NULL, @MaxSpreadSheetPosition, NULL)

-- ***** Get inserted UploadAttribute_ID ******
SELECT  @UploadAttribute_Id = UploadAttribute_ID FROM UploadAttribute WHERE Name = 'NonScale Extra Text'

-- ************ UPLOADTYPEATTRIBUTE **********
-- ******* Determine the next Grid position *********
SELECT  @GridPosition = max(GridPosition) + 1 FROM UploadTypeAttribute where UploadType_Code = 'ITEM_MAINTENANCE'

IF NOT EXISTS (SELECT * FROM UploadTypeAttribute WHERE UploadType_Code = 'ITEM_MAINTENANCE' AND UploadAttribute_ID = @UploadAttribute_ID)
INSERT INTO UploadTypeAttribute (UploadType_Code, UploadAttribute_ID, IsRequiredForUploadTypeForExistingItems, IsReadOnlyForExistingItems, IsHidden, GridPosition, IsRequiredForUploadTypeForNewItems, IsReadOnlyForNewItems, GroupName)
VALUES ('ITEM_MAINTENANCE', @UploadAttribute_ID, 0, 1, 0, @GridPosition, 0, 1, 'Shelf Label Data')

-- ************ UPLOADTYPETEMPLATEATTRIBUTE *****************
SELECT  @UploadTypeAttribute_Id = UploadTypeAttribute_ID FROM UploadTypeAttribute WHERE UploadType_Code = 'ITEM_MAINTENANCE' AND UploadAttribute_ID = @UploadAttribute_ID
SELECT  @UploadTypeTemplate_Id = UploadTypeTemplate_ID FROM UploadTypeTemplate WHERE UploadType_Code = 'ITEM_MAINTENANCE'AND Name = '- All Attributes -'

IF NOT EXISTS 
(SELECT * FROM UploadTypeTemplateAttribute WHERE UploadTypeTemplate_ID = @UploadTypeTemplate_Id AND UploadTypeAttribute_ID = @UploadTypeAttribute_ID)
INSERT INTO UploadTypeTemplateAttribute (UploadTypeTemplate_ID, UploadTypeAttribute_ID)
VALUES (@UploadTypeTemplate_Id, @UploadTypeAttribute_Id)
GO
