DECLARE @PosDataTypeKey INT = (SELECT POSDataTypeKey FROM POSDataTypes WHERE DataTypeDesc = 'Corp Scale Data')

IF NOT EXISTS (SELECT * FROM POSDataElement WHERE POSDataTypeKey = @PosDataTypeKey AND DataElement = 'StorageText')
	INSERT INTO POSDataElement (POSDataTypeKey, DataElement, Description, IsBoolean)
	VALUES (@PosDataTypeKey, 'StorageText', 'Scale_StorageData.StorageData', 0)