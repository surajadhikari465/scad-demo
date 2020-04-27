CREATE TYPE [dbo].[AddUpdateBrandType] AS TABLE
(
	BrandId int, 
	BrandName nvarchar(255), 
	BrandAbbreviation nvarchar(255), 
	Designation nvarchar(255), 
	ParentCompany nvarchar(255), 
	ZipCode nvarchar(255), 
	Locality nvarchar(255)
)
