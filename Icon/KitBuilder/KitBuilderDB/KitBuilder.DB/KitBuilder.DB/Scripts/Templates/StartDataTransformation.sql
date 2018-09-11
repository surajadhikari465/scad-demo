DECLARE 
 @ERR_MSG AS NVARCHAR(4000)
 ,@ERR_SEV AS SMALLINT
 ,@ERR_STA AS SMALLINT
 ,@scriptKey AS NVARCHAR(200)

SET @scriptKey = '<INSERTSCRIPTKEY>'

 IF(NOT exists(Select * from app.PostDeploymentScriptHistory where ScriptKey = @scriptKey))
BEGIN
	print 'Executing [ ' + @scriptKey + ' ]'
	BEGIN TRY
		BEGIN TRANSACTION
