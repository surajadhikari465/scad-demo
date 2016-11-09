CREATE TABLE [dbo].[UOM] (
[uomID] int  NOT NULL,
[uomCode] nvarchar(4)  NOT NULL,  
[uomName] nvarchar(100)  NULL,
CONSTRAINT [AK_uomCode_uomCode] UNIQUE NONCLUSTERED ([uomCode] ASC) WITH (FILLFACTOR = 80)

)
GO
ALTER TABLE [dbo].[UOM] ADD CONSTRAINT [UOM_PK] PRIMARY KEY CLUSTERED (
[uomID]
)