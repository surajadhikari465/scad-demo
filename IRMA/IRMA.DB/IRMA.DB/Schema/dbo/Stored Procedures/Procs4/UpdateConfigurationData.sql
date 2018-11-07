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
GO
GRANT EXECUTE
    ON OBJECT::[dbo].[UpdateConfigurationData] TO [IRMAAdminRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[UpdateConfigurationData] TO [IRMAClientRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[UpdateConfigurationData] TO [IRMASchedJobsRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[UpdateConfigurationData] TO [IRMAReportsRole]
    AS [dbo];

