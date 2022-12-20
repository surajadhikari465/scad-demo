declare @scriptKey varchar(128)

-- Product Backlog Item 17488: Purge AppLog on ItemCatalog
set @scriptKey = 'TestSuccess Transaction'
:r .\..\Templates\StartDataTransformation.sql
 Delete Users
:r .\..\Templates\EndDataTransformation.sql