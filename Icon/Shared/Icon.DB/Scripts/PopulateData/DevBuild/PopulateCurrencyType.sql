print N'Populating CurrencyType...'

SET IDENTITY_INSERT [dbo].[CurrencyType] ON 

GO
INSERT [dbo].[CurrencyType] ([currencyTypeID], [currencyTypeCode], [currencyTypeDesc]) VALUES (1, N'USD', N'US Dollar')
GO
SET IDENTITY_INSERT [dbo].[CurrencyType] OFF
GO