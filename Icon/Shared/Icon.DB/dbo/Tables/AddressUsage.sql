CREATE TABLE [dbo].[AddressUsage] 
(
	[addressUsageID] INT NOT NULL IDENTITY,
	[addressUsageCode] NVARCHAR(3) NOT NULL,
	[addressUsageDesc] NVARCHAR(255) NULL  
)
GO
ALTER TABLE [dbo].[AddressUsage] ADD CONSTRAINT [AddressUsage_PK] PRIMARY KEY CLUSTERED ([addressUsageID])