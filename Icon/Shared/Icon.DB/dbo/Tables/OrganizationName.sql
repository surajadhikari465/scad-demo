CREATE TABLE [dbo].[OrganizationName] (
[orgPartyID] INT  NOT NULL  
, [nameTypeCode] INT  NOT NULL  
, [orgName] NVARCHAR(255)  NULL  
, [orgNameDescription] NVARCHAR(255)  NULL  
)
GO
ALTER TABLE [dbo].[OrganizationName] WITH CHECK ADD CONSTRAINT [Organization_OrganizationName_FK1] FOREIGN KEY (
[orgPartyID]
)
REFERENCES [dbo].[Organization] (
[orgPartyID]
)
GO
ALTER TABLE [dbo].[OrganizationName] WITH CHECK ADD CONSTRAINT [NameType_OrganizationName_FK1] FOREIGN KEY (
[nameTypeCode]
)
REFERENCES [dbo].[NameType] (
[nameTypeCode]
)
GO
ALTER TABLE [dbo].[OrganizationName] ADD CONSTRAINT [OrganizationName_PK] PRIMARY KEY CLUSTERED (
[orgPartyID]
, [nameTypeCode]
)