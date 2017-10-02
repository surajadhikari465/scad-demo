DECLARE @scriptKey VARCHAR(128)

SET @scriptKey = 'AddCustomerFriendlyDescription'

IF(NOT EXISTS(SELECT 1 FROM app.PostDeploymentScriptHistory WHERE ScriptKey = @scriptKey))
BEGIN
	print '[' + convert(nvarchar, getdate(), 121) + '] ' + @scriptKey
	SET IDENTITY_INSERT [dbo].[Trait] ON

	DECLARE @TraitID INT
	DECLARE @TraitGroupID INT

	SET @TraitID = 151

	SET @TraitGroupID = 1

	IF NOT EXISTS (SELECT 1 FROM [dbo].[Trait] WHERE traitCode= 'CFD')
	INSERT [dbo].[Trait] (
							[traitID], 
							[traitCode], 
							[traitPattern], 
							[traitDesc], 
							[traitGroupID]) 
							VALUES 
							(
							  @TraitID, 
							  N'CFD',
							  N'^[\x20-\x21\x23-\x2A\x2C-\x5A\x61-\x7A\x9C]{0,60}$',
							  N'Customer Friendly Description', 
							  @TraitGroupID
							 )
	INSERT INTO app.PostDeploymentScriptHistory (ScriptKey, RunTime) VALUES (@scriptKey, GETDATE())
	SET IDENTITY_INSERT [dbo].[Trait] OFF
END
ELSE
BEGIN
	print '[' + convert(nvarchar, getdate(), 121) + '] ' + 'Pop-data already applied: ' + @scriptKey
END
