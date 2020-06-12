DECLARE @key VARCHAR(128) = 'PopulateItemGroupTypes';

IF(Not Exists(SELECT 1 FROM app.PostDeploymentScriptHistory WHERE ScriptKey = @key))
BEGIN

  IF NOT EXISTS( SELECT 1 FROM dbo.ItemGroupType  WHERE ItemGroupTypeName ='SKU')
  BEGIN
	INSERT INTO dbo.ItemGroupType  (ItemGroupTypeName)
	VALUES('SKU')
  END

   IF NOT EXISTS( SELECT 1 FROM dbo.ItemGroupType  WHERE ItemGroupTypeName ='Price Line')
   BEGIN
	INSERT INTO dbo.ItemGroupType  (ItemGroupTypeName)
	VALUES('Price Line')
   END

	INSERT INTO app.PostDeploymentScriptHistory (ScriptKey, RunTime) values (@key, GetDate());

END
ELSE
BEGIN
	print '[' + Convert(nvarchar, getdate(), 121) + '] ' + 'Pop-data already applied: ' + @key;
END
GO