USE [icon]
GO
SET IDENTITY_INSERT [dbo].[Trait] ON
GO
IF NOT EXISTS(SELECT 1 FROM dbo.Trait WHERE traitCode= 'TPG')
	INSERT [dbo].[Trait] ([traitID], [traitCode], [traitPattern], [traitDesc], [traitGroupID]) VALUES (215, N'TPG', N'', N'TouchPoint Group Id', 5)
GO
SET IDENTITY_INSERT [dbo].[Trait] OFF
GO