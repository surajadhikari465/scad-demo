DECLARE 
 @ERR_MSG AS NVARCHAR(4000)
 ,@ERR_SEV AS SMALLINT
 ,@ERR_STA AS SMALLINT
 ,@scriptKey AS NVARCHAR(200)

 SET @scriptKey = 'PopulateInstructionMemberPluNumbers'

 IF(NOT exists(Select * from app.PostDeploymentScriptHistory where ScriptKey = @scriptKey))
BEGIN
	print 'Executing [ ' + @scriptKey + ' ]'
	BEGIN TRY
		BEGIN TRANSACTION


			-- Populate [InstructionMemberPluNumbers] Table.
		DECLARE @startingNumber int = 472900 
DECLARE @endingNumber int = 473099

DECLARE @currentNumber int = @startingNumber
DECLARE @checkforExistingValues bit = 0

WHILE ( @currentNumber<=@endingNumber)
BEGIN

 IF(@checkforExistingValues = 1)
 BEGIN
	IF( NOT EXISTS(SELECT  1 FROM [dbo].[AvailablePluNumber] WHERE  PluNumber = @currentNumber) )
	BEGIN
		INSERT INTO [dbo].[AvailablePluNumber](PluNumber,InUse)
		VALUES( @currentNumber,0)
	END
 END
 ELSE
 BEGIN

	INSERT INTO [dbo].[AvailablePluNumber](PluNumber,InUse)
	VALUES( @currentNumber,0)
END

SET @currentNumber = @currentNumber+ 1;
END


		
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