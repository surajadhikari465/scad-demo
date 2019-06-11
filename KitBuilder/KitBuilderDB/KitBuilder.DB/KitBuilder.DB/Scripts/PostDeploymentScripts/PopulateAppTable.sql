DECLARE 
 @ERR_MSG AS NVARCHAR(4000)
 ,@ERR_SEV AS SMALLINT
 ,@ERR_STA AS SMALLINT
 ,@scriptKey AS NVARCHAR(200)

 SET @scriptKey = 'PopulateAppTable'

 IF(NOT exists(Select * from app.PostDeploymentScriptHistory where ScriptKey = @scriptKey))
BEGIN
	print 'Executing [ ' + @scriptKey + ' ]'
	BEGIN TRY
		BEGIN TRANSACTION

		set identity_insert App.App on

		insert into App.App (appid, appname) values (1,'Publish Instructions')
		insert into App.App (appid, appname) values (2,'Publish Kits'        )
		insert into App.App (appid, appname) values (3,'Web'             )
		insert into App.App (appid, appname) values (4,'WebApi'          )
		insert into App.App (appid, appname) values (5,'Locale Listener' )
		insert into App.App (appid, appname) values (6,'Item Listener'   )

		set identity_insert App.App off
		
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