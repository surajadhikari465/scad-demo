DECLARE @PosDataTypeKey INT = (SELECT POSDataTypeKey FROM POSDataTypes WHERE DataTypeDesc = 'Corp Scale Data')

delete POSDataElement
where POSDataTypeKey = @PosDataTypeKey
	and DataElement = 'StorageText'