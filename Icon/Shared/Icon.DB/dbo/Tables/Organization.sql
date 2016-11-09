CREATE TABLE [dbo].[Organization] (
[orgPartyID] INT  NOT NULL IDENTITY  
, [orgTypeID] INT  NULL  
, [parentOrgPartyID] INT  NULL  
, [orgDesc] NVARCHAR(255)  NULL  
)
GO
ALTER TABLE [dbo].[Organization] WITH CHECK ADD CONSTRAINT [Party_Organization_FK1] FOREIGN KEY (
[orgPartyID]
)
REFERENCES [dbo].[Party] (
[partyID]
)
GO
ALTER TABLE [dbo].[Organization] WITH CHECK ADD CONSTRAINT [Organization_Organization_FK1] FOREIGN KEY (
[parentOrgPartyID]
)
REFERENCES [dbo].[Organization] (
[orgPartyID]
)
GO
ALTER TABLE [dbo].[Organization] WITH CHECK ADD CONSTRAINT [OrganizationType_Organization_FK1] FOREIGN KEY (
[orgTypeID]
)
REFERENCES [dbo].[OrganizationType] (
[orgTypeID]
)
GO
ALTER TABLE [dbo].[Organization] ADD CONSTRAINT [Organization_PK] PRIMARY KEY CLUSTERED (
[orgPartyID]
)