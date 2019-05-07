DECLARE 
 @ERR_MSG AS NVARCHAR(4000)
 ,@ERR_SEV AS SMALLINT
 ,@ERR_STA AS SMALLINT
 ,@scriptKey AS NVARCHAR(200)

 SET @scriptKey = 'PopulateMessageTypes'

 IF(NOT exists(Select * from app.PostDeploymentScriptHistory where ScriptKey = @scriptKey))
BEGIN
	print 'Executing [ ' + @scriptKey + ' ]'
	BEGIN TRY
		BEGIN TRANSACTION
			-- Populate Status Table.
			SET IDENTITY_INSERT dbo.Status ON;
			
			if not exists(select * from app.MessageType where MessageTypeName = 'Item')
				insert into app.MessageType (MessageTypeId, MessageTypeName) values ('1', 'Item')

			if not exists(select * from app.MessageType where MessageTypeName = 'Locale')
				insert into app.MessageType (MessageTypeId, MessageTypeName) values ('2', 'Locale')

			if not exists(select * from app.MessageType where MessageTypeName = 'Kit')
				insert into app.MessageType (MessageTypeId, MessageTypeName) values ('3', 'Kit')

			if not exists(select * from app.MessageType where MessageTypeName = 'Instructions')
				insert into app.MessageType (MessageTypeId, MessageTypeName) values ('4', 'Instructions')

			SET IDENTITY_INSERT dbo.Status OFF;
		
		INSERT INTO app.PostDeploymentScriptHistory (ScriptKey, RunTime) VALUES (@scriptKey, GETDATE())
		COMMIT TRANSACTION;
		PRINT 'Finished [ ' + @scriptKey + ' ]'
	END TRY
	BEGIN CATCH
		ROLLBACK TRANSACTION;
		SELECT 
			@ERR_MSG = ERROR_MESSAGE(),
			@ERR_SEV =ERROR_SEVERITY(),
			@ERR_STA = ERROR_STATE()
		 SET @ERR_MSG= 'Error :' + @scriptKey + ' Message - ' + @ERR_MSG
		 PRINT @ERR_MSG
		 RAISERROR (@ERR_MSG, @ERR_SEV, @ERR_STA) WITH NOWAIT
	END CATCH
	
	
END
ELSE
BEGIN
	print 'Skipping [ ' + @scriptKey + ' ]'
END
GO