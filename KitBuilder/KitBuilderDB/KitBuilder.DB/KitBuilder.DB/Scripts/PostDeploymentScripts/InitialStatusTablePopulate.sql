﻿DECLARE 
 @ERR_MSG AS NVARCHAR(4000)
 ,@ERR_SEV AS SMALLINT
 ,@ERR_STA AS SMALLINT
 ,@scriptKey AS NVARCHAR(200)

 SET @scriptKey = 'InitialStatusTablePopulate'

 IF(NOT exists(Select * from app.PostDeploymentScriptHistory where ScriptKey = @scriptKey))
BEGIN
	print 'Executing [ ' + @scriptKey + ' ]'
	BEGIN TRY
		BEGIN TRANSACTION


			-- Populate Status Table.
			SET IDENTITY_INSERT dbo.Status ON;
			MERGE INTO Status AS Target
			USING(VALUES(1, N'DIS', N'Disabled'),
						(2, N'B', N'Building'),
						(3, N'RP', N'ReadyToPublish'),
						(4, N'PQ', N'Publish Queued'),
						(5, N'P', N'Published'),
						(6, N'M', N'Modifying'),
						(7, N'PF', N'PublishFailed'),
						(8, N'PRQ', N'Publish ReQueued'),
						(9, N'PP', N'Partially Published'),
						(10, N'UA', N'Unauthorized'),
						(11, N'PR', N'Processed'),
						(12, N'U', N'UnProcessed')
					) 
			AS Source(StatusID, StatusCode, StatusDescription)
			ON Target.StatusID=Source.StatusID
			--update matched rows
			WHEN MATCHED THEN UPDATE SET
								  Target.StatusCode=Source.StatusCode,
								  Target.StatusDescription=Source.StatusDescription
			--insert new rows
			WHEN NOT MATCHED BY TARGET THEN INSERT(StatusID, StatusCode, StatusDescription)
											VALUES(StatusID, StatusCode, StatusDescription);
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