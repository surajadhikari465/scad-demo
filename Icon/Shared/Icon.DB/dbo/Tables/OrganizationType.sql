CREATE TABLE [dbo].[OrganizationType] (
[orgTypeID] INT  NOT NULL IDENTITY,
[orgTypeCode] NVARCHAR(3)  NOT NULL,
[orgTypeDesc] NVARCHAR(255)  NULL,
CONSTRAINT [AK_orgTypeCode_orgTypeCode] UNIQUE NONCLUSTERED ([orgTypeCode] ASC) WITH (FILLFACTOR = 80)

)
GO
ALTER TABLE [dbo].[OrganizationType] ADD CONSTRAINT [OrganizationType_PK] PRIMARY KEY CLUSTERED (
[orgTypeID]
)