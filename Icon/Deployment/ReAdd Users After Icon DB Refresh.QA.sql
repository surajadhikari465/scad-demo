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
     WHERE name = 'WFM\IconWebQA')
BEGIN
    CREATE USER [WFM\IconWebQA] FOR LOGIN [WFM\IconWebQA]
END
GO
USE [Icon]
GO
EXEC sp_addrolemember N'db_datareader', N'WFM\IconWebQA'
EXEC sp_addrolemember N'db_datawriter', N'WFM\IconWebQA'
EXEC sp_addrolemember N'db_owner', N'WFM\IconWebQA'
GO

---------------------------------------------------------------------
---------------------------------------------------------------------

USE [Icon]
GO
IF NOT EXISTS
    (SELECT name
     FROM sys.database_principals
     WHERE name = 'WFM\IconInterfaceUserQA')
BEGIN
    CREATE USER [WFM\IconInterfaceUserQA] FOR LOGIN [WFM\IconInterfaceUserQA]
END
GO
USE [Icon]
GO
EXEC sp_addrolemember N'db_datareader', N'WFM\IconInterfaceUserQA'
EXEC sp_addrolemember N'db_datawriter', N'WFM\IconInterfaceUserQA'
EXEC sp_addrolemember N'db_owner', N'WFM\IconInterfaceUserQA'
GO

---------------------------------------------------------------------
---------------------------------------------------------------------

USE [Icon]
GO
IF NOT EXISTS
    (SELECT name
     FROM sys.database_principals
     WHERE name = 'WFM\MammothQA')
BEGIN
    CREATE USER [WFM\MammothQA] FOR LOGIN [WFM\MammothQA]
END
GO
USE [Icon]
GO
EXEC sp_addrolemember N'db_datareader', N'WFM\MammothQA'
EXEC sp_addrolemember N'db_datawriter', N'WFM\MammothQA'
EXEC sp_addrolemember N'db_owner', N'WFM\MammothQA'
GO

---------------------------------------------------------------------
---------------------------------------------------------------------


USE [Icon]
GO
IF NOT EXISTS
    (SELECT name
     FROM sys.database_principals
     WHERE name = 'WFM\NutriconServiceQA')
BEGIN
    CREATE USER [WFM\NutriconServiceQA] FOR LOGIN [WFM\NutriconServiceQA]
END
GO
USE [Icon]
GO
EXEC sp_addrolemember N'db_datareader', N'WFM\NutriconServiceQA'
EXEC sp_addrolemember N'db_datawriter', N'WFM\NutriconServiceQA'
EXEC sp_addrolemember N'db_owner', N'WFM\NutriconServiceQA'
EXEC sp_addrolemember N'NutritionRole', N'WFM\NutriconServiceQA'
GO

---------------------------------------------------------------------
---------------------------------------------------------------------

USE [Icon]
GO
IF NOT EXISTS
    (SELECT name
     FROM sys.database_principals
     WHERE name = 'WFM\PDXExtractUserQA')
BEGIN
    CREATE USER [WFM\PDXExtractUserQA] FOR LOGIN [WFM\PDXExtractUserQA]
END
GO
USE [Icon]
GO
EXEC sp_addrolemember N'IconPDXExtractRole', N'WFM\PDXExtractUserQA'
GO

---------------------------------------------------------------------
---------------------------------------------------------------------