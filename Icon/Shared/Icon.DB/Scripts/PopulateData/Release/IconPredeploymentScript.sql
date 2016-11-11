/*
All pre deploymnet op-data updates for each release go here.

Do not check in separate files to the '.../Scripts/PopulateData/Release/' folder,
just add your updates directly to IconPredeployment.sql

Please add prints/logging for each statement/block of code appropriately, to allow for tracking, debugging, and troublshooting;
Example/Format:
print '[' + convert(nvarchar, getdate(), 121) + '] [PreDeployData] TFS ?????: PBI Desc -- Action details...'

*/

go
print '[' + convert(nvarchar, getdate(), 121) + '] [PreDeployData] TFS ?????: PBI Desc -- Action details...'

