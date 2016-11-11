declare @scriptKey varchar(128)

-- Product Backlog Item 17157: As IRMA I need to purge NoTagThreshold table automatically
set @scriptKey = 'InsertInforErrors'

IF(NOT exists(Select * from app.PostDeploymentScriptHistory where ScriptKey = @scriptKey))
BEGIN
	print '[' + convert(nvarchar, getdate(), 121) + '] ' + @scriptKey

	declare @inforItemListenerAppId int = (select AppID from app.App where AppName = 'Infor Item Listener')

	insert into infor.Errors(AppId, ErrorCode, ErrorDetails)
	values (@inforItemListenerAppId, 'NonExistentBrand', 'No Brand exists in Icon with a hierarchy class ID ''{PropertyValue}''.'),
		(@inforItemListenerAppId, 'NonExistentSubTeam', 'No Sub Team exists in Icon with an sub team code ''{PropertyValue}''.'),
		(@inforItemListenerAppId, 'NonExistentSubBrick', 'No Sub Brick exists in Icon with a hierarchy class ID ''{PropertyValue}''.'),
		(@inforItemListenerAppId, 'NonExistentNationalClass', 'No National Class exists in Icon with a hierarchy class ID ''{PropertyValue}''.'),
		(@inforItemListenerAppId, 'NonExistentTax', 'No Tax Class exists in Icon with a tax code ''{PropertyValue}''.')

	insert into app.PostDeploymentScriptHistory (ScriptKey, RunTime) values (@scriptKey, GETDATE())

END
ELSE
BEGIN
	print '[' + convert(nvarchar, getdate(), 121) + '] ' + 'Pop-data already applied: ' + @scriptKey
END
GO