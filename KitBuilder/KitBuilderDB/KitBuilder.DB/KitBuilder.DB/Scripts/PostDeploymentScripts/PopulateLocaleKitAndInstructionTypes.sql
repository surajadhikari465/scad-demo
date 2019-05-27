DECLARE @scriptKey AS NVARCHAR(200)

 SET @scriptKey = N'PopulateLocaleKitAndInstructionTypes'

 IF(NOT exists(Select * from app.PostDeploymentScriptHistory where ScriptKey = @scriptKey))
BEGIN
	print 'Executing [ ' + @scriptKey + ' ]'

	SET IDENTITY_INSERT [dbo].[InstructionType] ON 
	
	IF NOT EXISTS (Select * from InstructionType where InstructionTypeId = 1)
	INSERT [dbo].[InstructionType] ([InstructionTypeId], [Name]) VALUES (1, N'Cooking')
	
	IF NOT EXISTS (Select * from InstructionType where InstructionTypeId = 2)
	INSERT [dbo].[InstructionType] ([InstructionTypeId], [Name]) VALUES (2, N'Generic')
	
	SET IDENTITY_INSERT [dbo].[InstructionType] OFF
	

	IF NOT EXISTS (Select * from KitTypes where KitType = 1)
	INSERT [dbo].[KitTypes] ([KitType], [Type]) VALUES (1, N'Simple')
	
	IF NOT EXISTS (Select * from KitTypes where KitType = 2)
	INSERT [dbo].[KitTypes] ([KitType], [Type]) VALUES (2, N'Fixed')
	
	IF NOT EXISTS (Select * from KitTypes where KitType = 3)
	INSERT [dbo].[KitTypes] ([KitType], [Type]) VALUES (3, N'Customizable')
	


	IF NOT EXISTS (Select * from LocaleType where localeTypeId = 1)
	INSERT [dbo].[LocaleType] ([localeTypeId], [localeTypeCode], [localeTypeDesc]) VALUES (1, N'CH', N'Chain')
	
	IF NOT EXISTS (Select * from LocaleType where localeTypeId = 2)
	INSERT [dbo].[LocaleType] ([localeTypeId], [localeTypeCode], [localeTypeDesc]) VALUES (2, N'RG', N'Region')
	
	IF NOT EXISTS (Select * from LocaleType where localeTypeId = 3)
	INSERT [dbo].[LocaleType] ([localeTypeId], [localeTypeCode], [localeTypeDesc]) VALUES (3, N'MT', N'Metro')
	
	IF NOT EXISTS (Select * from LocaleType where localeTypeId = 4)
	INSERT [dbo].[LocaleType] ([localeTypeId], [localeTypeCode], [localeTypeDesc]) VALUES (4, N'ST', N'Store')
	
	IF NOT EXISTS (Select * from LocaleType where localeTypeId = 5)
	INSERT [dbo].[LocaleType] ([localeTypeId], [localeTypeCode], [localeTypeDesc]) VALUES (5, N'VE', N'Venue')
	
	INSERT INTO app.PostDeploymentScriptHistory (ScriptKey, RunTime) VALUES (@scriptKey, GETDATE())

END
GO
