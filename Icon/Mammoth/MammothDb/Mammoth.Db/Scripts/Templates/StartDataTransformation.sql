DECLARE 
 @ERR_MSG AS NVARCHAR(4000)
 ,@ERR_SEV AS SMALLINT
 ,@ERR_STA AS SMALLINT

 IF(NOT exists(Select * from PostDeploymentScriptHistory where ScriptKey = @scriptKey))
BEGIN
	print 'Executing :' + @scriptKey
	BEGIN TRY
		BEGIN TRANSACTION
