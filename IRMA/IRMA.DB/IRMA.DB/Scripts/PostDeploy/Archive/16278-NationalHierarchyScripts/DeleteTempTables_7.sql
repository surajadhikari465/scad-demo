IF EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[NatItemCatBackUp]')
			 AND TYPE IN (N'U'))
DROP TABLE [dbo].[NatItemCatBackUp]
IF EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[NatItemClassBackUp]')
			 AND TYPE IN (N'U'))
DROP TABLE [dbo].[NatItemClassBackUp]
IF EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[NatItemfamilyBackUp]')
			 AND TYPE IN (N'U'))
DROP TABLE [NatItemfamilyBackUp]