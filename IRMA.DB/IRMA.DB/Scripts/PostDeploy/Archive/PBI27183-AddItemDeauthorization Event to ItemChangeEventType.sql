IF NOT EXISTS(SELECT 1 FROM [mammoth].[ItemChangeEventType] WHERE EventTypeName = 'ItemDeauthorization' )
BEGIN
INSERT INTO [mammoth].[ItemChangeEventType]
           ([EventTypeName])
	VALUES('ItemDeauthorization')
END
go