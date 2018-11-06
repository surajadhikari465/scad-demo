 
if exists (select * from dbo.sysobjects where id = object_id(N'dbo.GetConfigurationValue') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
	drop procedure dbo.GetConfigurationValue
GO

CREATE PROCEDURE dbo.GetConfigurationValue (@iConfigKey	varchar(50)) 
AS 

-- EXEC GetConfigurationValue ''

BEGIN
	DECLARE @iConfigValue	sql_variant
	SELECT 
		@iConfigValue = ConfigValue
	FROM 
		dbo.ConfigurationData
	 WHERE ConfigKey = @iConfigKey

	select @iConfigValue
END
go
