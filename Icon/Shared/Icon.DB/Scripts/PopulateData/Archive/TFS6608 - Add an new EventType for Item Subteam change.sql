IF NOT EXISTS (SELECT * FROM app.EventType WHERE EventName = 'Sub Team Update')
	INSERT INTO app.EventType
	VALUES ('Sub Team Update')

INSERT INTO [app].[EventType]
           ([EventName])
     VALUES
           ('Item Sub Team Update')
GO


