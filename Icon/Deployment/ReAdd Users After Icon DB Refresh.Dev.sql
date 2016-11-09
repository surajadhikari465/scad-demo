USE [IconDev]
GO
IF NOT EXISTS
    (SELECT name
     FROM sys.database_principals
     WHERE name = 'WFM\IRMA.bsa')
BEGIN
    CREATE USER [WFM\IRMA.bsa] FOR LOGIN [WFM\IRMA.bsa]
END
GO
USE [IconDev]
GO
EXEC sp_addrolemember N'db_datareader', N'WFM\IRMA.bsa'
GO


USE [IconDev]
GO
IF NOT EXISTS
    (SELECT name
     FROM sys.database_principals
     WHERE name = 'WFM\IconWebDev')
BEGIN
    CREATE USER [WFM\IconWebDev] FOR LOGIN [WFM\IconWebDev]
END
GO
USE [IconDev]
GO
EXEC sp_addrolemember N'db_datareader', N'WFM\IconWebDev'
EXEC sp_addrolemember N'db_datawriter', N'WFM\IconWebDev'
EXEC sp_addrolemember N'db_owner', N'WFM\IconWebDev'
GO

---------------------------------------------------------------------
---------------------------------------------------------------------

USE [IconDev]
GO
IF NOT EXISTS
    (SELECT name
     FROM sys.database_principals
     WHERE name = 'WFM\IConInterfaceUserDev')
BEGIN
    CREATE USER [WFM\IConInterfaceUserDev] FOR LOGIN [WFM\IConInterfaceUserDev]
END
GO
USE [IconDev]
GO
EXEC sp_addrolemember N'db_datareader', N'WFM\IConInterfaceUserDev'
EXEC sp_addrolemember N'db_datawriter', N'WFM\IConInterfaceUserDev'
EXEC sp_addrolemember N'db_owner', N'WFM\IConInterfaceUserDev'
GO

---------------------------------------------------------------------
---------------------------------------------------------------------

USE [IconDev]
GO
IF NOT EXISTS
    (SELECT name
     FROM sys.database_principals
     WHERE name = 'WFM\MammothDev')
BEGIN
    CREATE USER [WFM\MammothDev] FOR LOGIN [WFM\MammothDev]
END
GO
USE [IconDev]
GO
EXEC sp_addrolemember N'db_datareader', N'WFM\MammothDev'
EXEC sp_addrolemember N'db_datawriter', N'WFM\MammothDev'
EXEC sp_addrolemember N'db_owner', N'WFM\MammothDev'
GO

---------------------------------------------------------------------
---------------------------------------------------------------------


USE [IconDev]
GO
IF NOT EXISTS
    (SELECT name
     FROM sys.database_principals
     WHERE name = 'WFM\NutriconServiceDev')
BEGIN
    CREATE USER [WFM\NutriconServiceDev] FOR LOGIN [WFM\NutriconServiceDev]
END
GO
USE [IconDev]
GO
EXEC sp_addrolemember N'db_datareader', N'WFM\NutriconServiceDev'
EXEC sp_addrolemember N'db_datawriter', N'WFM\NutriconServiceDev'
EXEC sp_addrolemember N'db_owner', N'WFM\NutriconServiceDev'
EXEC sp_addrolemember N'NutritionRole', N'WFM\NutriconServiceDev'
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