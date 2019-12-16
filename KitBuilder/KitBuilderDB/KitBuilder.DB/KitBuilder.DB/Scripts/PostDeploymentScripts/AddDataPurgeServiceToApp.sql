IF NOT EXISTS (SELECT * FROM [app].[App] WHERE AppName = 'Data Purge Service')
INSERT INTO [app].[App]
           ([AppName])
     VALUES
           ('Data Purge Service')
GO