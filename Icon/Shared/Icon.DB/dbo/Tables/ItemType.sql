﻿CREATE TABLE [dbo].[ItemType] (
[itemTypeID] INT  NOT NULL IDENTITY,
[itemTypeCode] NVARCHAR(3)  NOT NULL,  
[itemTypeDesc] NVARCHAR(255)  NULL,
CONSTRAINT [AK_itemTypeCode_itemTypeCode] UNIQUE NONCLUSTERED ([itemTypeCode] ASC) WITH (FILLFACTOR = 80)

)
GO
ALTER TABLE [dbo].[ItemType] ADD CONSTRAINT [ItemType_PK] PRIMARY KEY CLUSTERED (
[itemTypeID]
)