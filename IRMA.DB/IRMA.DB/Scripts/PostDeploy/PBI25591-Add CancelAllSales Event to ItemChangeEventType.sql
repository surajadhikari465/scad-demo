IF NOT EXISTS(SELECT 1 FROM [mammoth].[ItemChangeEventType] WHERE EventTypeName = 'CancelAllSales' )
BEGIN
INSERT INTO [mammoth].[ItemChangeEventType]
           ([EventTypeName])
	VALUES('CancelAllSales')
END
go