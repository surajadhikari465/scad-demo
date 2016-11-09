CREATE TABLE [dbo].[DeliverySystem] (
[DeliverySystemID] int IDENTITY(1,1) NOT NULL,
[DeliverySystemCode] nvarchar(4)  NOT NULL,  
[DeliverySystemName] nvarchar(100)  NULL
)
GO
ALTER TABLE [dbo].[DeliverySystem] ADD CONSTRAINT [DS_PK] PRIMARY KEY CLUSTERED (
[DeliverySystemID]
)