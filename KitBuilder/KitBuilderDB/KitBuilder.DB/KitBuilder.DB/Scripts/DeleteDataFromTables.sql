﻿IF (@@SERVERNAME  = 'EC2AMAZ-SRCJJ6Q')
BEGIN
	DELETE FROM  [app].[AppLog]
	DELETE FROM  [dbo].[InstructionListQueue]
	DELETE FROM  [dbo].[KitQueue]
	DELETE FROM  [dbo].[InstructionListMember]
	DELETE FROM  [dbo].[KitInstructionList]
	DELETE FROM  [dbo].[KitLinkGroupItemLocale]
	DELETE FROM  [dbo].[KitLinkGroupItem]
	DELETE FROM  [dbo].[LinkGroupItem]
	DELETE FROM  [dbo].[KitLinkGroup]
	DELETE FROM  [dbo].[LinkGroup]
	DELETE FROM  [dbo].[KitLinkGroupLocale]
	DELETE FROM  [dbo].[KitLocale]
	DELETE FROM  [dbo].[Kit]
	DELETE FROM  [dbo].[Items]
	DELETE FROM  Locale where localeTypeid in ( SELECT localeTypeId from localeType Where localeTypeId=5)
	DELETE FROM  [dbo].[InstructionList]
END