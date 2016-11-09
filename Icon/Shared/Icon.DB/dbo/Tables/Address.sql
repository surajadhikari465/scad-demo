CREATE TABLE [dbo].[Address] 
(
	[addressID] INT  NOT NULL IDENTITY,
	[addressTypeID] INT  NOT NULL  
)
GO
ALTER TABLE [dbo].[Address] WITH CHECK ADD CONSTRAINT [AddressType_Address_FK1] FOREIGN KEY ([addressTypeID])
REFERENCES [dbo].[AddressType] ([addressTypeID])
GO
ALTER TABLE [dbo].[Address] ADD CONSTRAINT [Address_PK] PRIMARY KEY CLUSTERED ([addressID])