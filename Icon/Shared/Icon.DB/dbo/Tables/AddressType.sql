CREATE TABLE [dbo].[AddressType] 
(
	[addressTypeID] INT NOT NULL IDENTITY,
	[addressTypeCode] NVARCHAR(3) NOT NULL,
	[addressTypeDesc] NVARCHAR(255) NULL  
)
GO
ALTER TABLE [dbo].[AddressType] ADD CONSTRAINT [AddressType_PK] PRIMARY KEY CLUSTERED ([addressTypeID])