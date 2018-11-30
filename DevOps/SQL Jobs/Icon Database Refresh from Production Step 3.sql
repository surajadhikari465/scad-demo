
--Remap Users & Other Permissions
use iCON
go
exec sp_change_users_login @Action='update_one', @UserNamePattern='iConUser', @LoginName='iConUser';
EXEC sp_change_users_login 'Auto_Fix', 'WFM\Icon DB Read-Only Access';
EXEC sp_change_users_login 'Auto_Fix', 'IconReports';
EXEC sp_change_users_login 'Auto_Fix', 'TeraData';
go
ALTER ROLE [db_owner] ADD MEMBER [WFM\IRMA.developers]
GO

IF (SELECT name FROM sys.database_principals WHERE name = 'IconPurgeUser') IS NULL
	BEGIN
		CREATE USER [IconPurgeUser] FROM LOGIN [IconPurgeUser];
		EXEC sp_addrolemember 'db_datawriter', 'IconPurgeUser';
		EXEC sp_addrolemember 'db_datareader', 'IconPurgeUser';
	END
ELSE
	BEGIN
		EXEC sp_change_users_login 'Auto_Fix', 'IconPurgeUser';
	END


IF (SELECT name FROM sys.database_principals WHERE name = 'WFM\IRMA Replacement Team DB Read-Only Access') IS NULL
CREATE USER [WFM\IRMA Replacement Team DB Read-Only Access] FROM LOGIN [WFM\IRMA Replacement Team DB Read-Only Access]

EXEC sp_addrolemember 'db_datareader', 'WFM\IRMA Replacement Team DB Read-Only Access', 
		@database_name=N'master', 
		@flags=0