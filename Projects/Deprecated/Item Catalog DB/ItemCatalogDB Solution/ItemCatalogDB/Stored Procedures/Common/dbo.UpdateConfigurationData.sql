 
if exists (select * from dbo.sysobjects where id = object_id(N'dbo.UpdateConfigurationData') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
	drop procedure dbo.UpdateConfigurationData
GO

CREATE PROCEDURE dbo.UpdateConfigurationData (
		@iConfigKey	varchar(50),
		@iConfigValue	sql_variant
		)
AS 

BEGIN
	UPDATE dbo.ConfigurationData
	   SET ConfigValue = @iConfigValue
	 WHERE ConfigKey = @iConfigKey
END
go
