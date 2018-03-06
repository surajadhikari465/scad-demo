DECLARE @POSDataTypeKey INT

SET @POSDataTypeKey = (SELECT TOP 1 POSDataTypeKey FROM [dbo].POSDataTypes WHERE DataTypeDesc = 'Electronic Tag Data')

IF( @POSDataTypeKey IS NOT NULL AND NOT EXISTS(SELECT 1 FROM [POSDataElement] WHERE DataElement = 'Ordered_By_Infor'))
BEGIN
INSERT INTO [dbo].[POSDataElement]
           ([POSDataTypeKey]
           ,[DataElement]
           ,[Description]
           ,[IsBoolean])
     VALUES
           (@POSDataTypeKey,
           'Ordered_By_Infor',
           'Ordered by Infor',
           1)
END

GO