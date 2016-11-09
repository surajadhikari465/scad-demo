		INSERT INTO App.PostDeploymentScriptHistory (ScriptKey, RunTime) VALUES (@scriptKey, GETDATE())
		COMMIT TRANSACTION;
		PRINT 'Finished :' + @scriptKey
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
	print 'Skipping :' + @scriptKey
END
GO