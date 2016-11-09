-- 7/11/2014, Sprint 18, TFS 3790
-- Populates the dbo.AddressType and dbo.AddressUsage tables

SET IDENTITY_INSERT [dbo].[AddressType] ON 

IF NOT EXISTS (select * from AddressType at where at.addressTypeDesc = 'Physical Address')
BEGIN

	INSERT [dbo].[AddressType] ([addressTypeID], [addressTypeCode], [addressTypeDesc]) 
	VALUES (1, N'PHY', N'Physical Address')
END

SET IDENTITY_INSERT [dbo].[AddressType] OFF
SET IDENTITY_INSERT [dbo].[AddressUsage] ON 

IF NOT EXISTS (select * from AddressUsage au where au.addressUsageDesc = 'Shipping')
BEGIN
	INSERT [dbo].[AddressUsage] ([addressUsageID], [addressUsageCode], [addressUsageDesc]) 
	VALUES (1, N'SHP', N'Shipping')
END

SET IDENTITY_INSERT [dbo].[AddressUsage] OFF