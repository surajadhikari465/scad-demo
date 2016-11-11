/*
All pop-data updates for each release go here.

Do not check in separate files to the '.../Scripts/PopulateData/Release/' folder,
just add your updates directly to IconMasterData.sql or IconPopulateData.sql.

Please add prints/logging for each statement/block of code appropriately, to allow for tracking, debugging, and troublshooting;
Example/Format:
print '[' + convert(nvarchar, getdate(), 121) + '] [PopulateData] TFS ?????: PBI Desc -- Action details...'


********* IMPORTANT NOTE *********
Every group/set of updates must have it's own script key and it makes the most sense for these to be grouped by PBI.

-- Copy this template and update it for each pop-data (each PBI).
set @scriptKey = '[PopulateData] TFS ?????: PBI Desc -- Action details...'

*/

declare @scriptKey varchar(128)

set @scriptKey = 'IconPopulateData'

IF(NOT exists(Select * from app.PostDeploymentScriptHistory where ScriptKey = @scriptKey))
BEGIN
	print 'Executing :' + @scriptKey
	print 'Finished :' + @scriptKey

END
ELSE
BEGIN
	print 'Skipping :' + @scriptKey
END
GO
