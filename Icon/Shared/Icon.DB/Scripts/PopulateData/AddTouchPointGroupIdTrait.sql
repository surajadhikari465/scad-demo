DECLARE @scriptKey VARCHAR(128) = 'PopulateTouchPointGroupIdTrait';

IF(NOT EXISTS(SELECT 1 FROM app.PostDeploymentScriptHistory WHERE ScriptKey = @scriptKey))
BEGIN

	SET IDENTITY_INSERT [dbo].[Trait] ON
	
	IF NOT EXISTS(SELECT 1 FROM dbo.Trait WHERE traitCode= 'TPG')
		INSERT [dbo].[Trait] ([traitID], [traitCode], [traitPattern], [traitDesc], [traitGroupID]) VALUES (215, N'TPG', N'', N'TouchPoint Group Id', 5)

	SET IDENTITY_INSERT [dbo].[Trait] OFF

	INSERT INTO app.PostDeploymentScriptHistory (ScriptKey, RunTime) values (@scriptKey, GETDATE())

END
ELSE
BEGIN
	print '[' + convert(nvarchar, getdate(), 121) + '] ' + 'Pop-data already applied: ' + @scriptKey
END
GO