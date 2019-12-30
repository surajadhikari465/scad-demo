CREATE TYPE app.ContactInputType AS TABLE(
	[ContactId] [int] NOT NULL,
	[HierarchyClassId] [int] NOT NULL,
	[ContactTypeId] [int] NOT NULL,
	[ContactName] [nvarchar](255) NOT NULL,
	[Email] [nvarchar](255) NOT NULL,
	[Title] [nvarchar](255) NULL,
	[AddressLine1] [nvarchar](255) NULL,
	[AddressLine2] [nvarchar](255) NULL,
	[City] [nvarchar](255) NULL,
	[State] [nvarchar](255) NULL,
	[ZipCode] [nvarchar](15) NULL,
	[Country] [nvarchar](255) NULL,
	[PhoneNumber1] [nvarchar](30) NULL,
	[PhoneNumber2] [nvarchar](30) NULL,
	[WebsiteURL] [nvarchar](255) NULL
)
GO