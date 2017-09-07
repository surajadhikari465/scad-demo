CREATE USER [TibcoDataReader] 
FOR LOGIN [TibcoDataReader] WITH DEFAULT_SCHEMA=[dbo]
GO

EXEC sp_addrolemember N'db_datareader', N'TibcoDataReader'
GO
