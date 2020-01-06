CREATE TYPE app.ContactTypeInputType AS TABLE(
	[ContactTypeId] INT NOT NULL,
	[ContactTypeName] NVARCHAR(255) NOT NULL,
	[Archived] BIT NOT NULL
)
GO