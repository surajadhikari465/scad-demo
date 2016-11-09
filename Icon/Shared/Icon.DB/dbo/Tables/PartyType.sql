CREATE TABLE [dbo].[PartyType] (
[partyTypeID] INT  NOT NULL IDENTITY,
[partyTypeCode] NVARCHAR(3)  NOT NULL,
[partyTypeDesc] NVARCHAR(255)  NULL,
CONSTRAINT [AK_partyTypeCode_partyTypeCode] UNIQUE NONCLUSTERED ([partyTypeCode] ASC) WITH (FILLFACTOR = 80)

)
GO
ALTER TABLE [dbo].[PartyType] ADD CONSTRAINT [PartyType_PK] PRIMARY KEY CLUSTERED (
[partyTypeID]
)