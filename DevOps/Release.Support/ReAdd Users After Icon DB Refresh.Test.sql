USE [Icon]
GO
IF NOT EXISTS
    (SELECT name
     FROM sys.database_principals
     WHERE name = 'WFM\IRMA.bsa')
BEGIN
    CREATE USER [WFM\IRMA.bsa] FOR LOGIN [WFM\IRMA.bsa]
END
GO
USE [Icon]
GO
EXEC sp_addrolemember N'db_datareader', N'WFM\IRMA.bsa'
GO


USE [Icon]
GO
IF NOT EXISTS
    (SELECT name
     FROM sys.database_principals
     WHERE name = 'WFM\IconWebTest')
BEGIN
    CREATE USER [WFM\IconWebTest] FOR LOGIN [WFM\IconWebTest]
END
GO
USE [Icon]
GO
EXEC sp_addrolemember N'db_datareader', N'WFM\IconWebTest'
EXEC sp_addrolemember N'db_datawriter', N'WFM\IconWebTest'
EXEC sp_addrolemember N'db_owner', N'WFM\IconWebTest'
GO

---------------------------------------------------------------------
---------------------------------------------------------------------

USE [Icon]
GO
IF NOT EXISTS
    (SELECT name
     FROM sys.database_principals
     WHERE name = 'WFM\IconInterfaceUserTes')
BEGIN
    CREATE USER [WFM\IconInterfaceUserTes] FOR LOGIN [WFM\IconInterfaceUserTes]
END
GO
USE [Icon]
GO
EXEC sp_addrolemember N'db_datareader', N'WFM\IconInterfaceUserTes'
EXEC sp_addrolemember N'db_datawriter', N'WFM\IconInterfaceUserTes'
EXEC sp_addrolemember N'db_owner', N'WFM\IconInterfaceUserTes'
GO

---------------------------------------------------------------------
---------------------------------------------------------------------

USE [Icon]
GO
IF NOT EXISTS
    (SELECT name
     FROM sys.database_principals
     WHERE name = 'WFM\MammothTest')
BEGIN
    CREATE USER [WFM\MammothTest] FOR LOGIN [WFM\MammothTest]
END
GO
USE [Icon]
GO
EXEC sp_addrolemember N'db_datareader', N'WFM\MammothTest'
EXEC sp_addrolemember N'db_datawriter', N'WFM\MammothTest'
EXEC sp_addrolemember N'db_owner', N'WFM\MammothTest'
GO

---------------------------------------------------------------------
---------------------------------------------------------------------


USE [Icon]
GO
IF NOT EXISTS
    (SELECT name
     FROM sys.database_principals
     WHERE name = 'WFM\NutriconServiceTest')
BEGIN
    CREATE USER [WFM\NutriconServiceTest] FOR LOGIN [WFM\NutriconServiceTest]
END
GO
USE [Icon]
GO
EXEC sp_addrolemember N'db_datareader', N'WFM\NutriconServiceTest'
EXEC sp_addrolemember N'db_datawriter', N'WFM\NutriconServiceTest'
EXEC sp_addrolemember N'db_owner', N'WFM\NutriconServiceTest'
EXEC sp_addrolemember N'NutritionRole', N'WFM\NutriconServiceTest'
GO

---------------------------------------------------------------------
---------------------------------------------------------------------

USE [Icon]
GO
IF NOT EXISTS
    (SELECT name
     FROM sys.database_principals
     WHERE name = 'WFM\PDXExtractUserDev')
BEGIN
    CREATE USER [WFM\PDXExtractUserDev] FOR LOGIN [WFM\PDXExtractUserDev]
END
GO
USE [Icon]
GO
EXEC sp_addrolemember N'IconPDXExtractRole', N'WFM\PDXExtractUserDev'
GO

---------------------------------------------------------------------
---------------------------------------------------------------------