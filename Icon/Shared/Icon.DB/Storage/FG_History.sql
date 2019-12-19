ALTER DATABASE [$(DatabaseName)] ADD FILEGROUP [FG_History]
GO
ALTER DATABASE [$(DatabaseName)] ADD FILE ( NAME = N'IconHistory', FILENAME = N'e:\sql_data_01\files\IconHistory.mdf' , SIZE = 1048576KB , FILEGROWTH = 1048576KB ) TO FILEGROUP [FG_History]
GO