--Change Owner and Recovery Mode to SIMPLE
USE iCON
GO
exec sp_changedbowner 'sa'

USE master
GO
ALTER DATABASE iCON SET RECOVERY SIMPLE WITH NO_WAIT
GO