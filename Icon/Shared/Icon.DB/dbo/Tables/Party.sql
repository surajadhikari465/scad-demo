CREATE TABLE [dbo].[Party] (
[partyID] INT  NOT NULL IDENTITY  
, [partyTypeID] INT  NULL  
)
GO
ALTER TABLE [dbo].[Party] WITH CHECK ADD CONSTRAINT [PartyType_Party_FK1] FOREIGN KEY (
[partyTypeID]
)
REFERENCES [dbo].[PartyType] (
[partyTypeID]
)
GO
ALTER TABLE [dbo].[Party] ADD CONSTRAINT [Party_PK] PRIMARY KEY CLUSTERED (
[partyID]
)