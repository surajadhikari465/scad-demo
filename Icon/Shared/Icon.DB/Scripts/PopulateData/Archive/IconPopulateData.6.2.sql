/*
All pop-data updates for each release go here.

Do not check in separate files to the '.../Scripts/PopulateData/Release/' folder,
just add your updates directly to IconMasterData.sql or IconPopulateData.sql.

Please add prints/logging for each statement/block of code appropriately, to allow for tracking, debugging, and troublshooting;
Example/Format:
print '[' + convert(nvarchar, getdate(), 121) + '] [PopulateData] TFS ?????: PBI Desc -- Action details...'

*/

go

IF(NOT EXISTS (SELECT * FROM app.RetentionPolicy rp WHERE rp.[Table] = 'MessageQueueNutrition'))
BEGIN
	INSERT INTO app.RetentionPolicy([Server], [Database], [Schema], [Table], DaysToKeep)
	VALUES (@@SERVERNAME, 'Icon', 'app', 'MessageQueueNutrition', 10)
END

GO

