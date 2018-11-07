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
GO
GRANT EXECUTE
    ON OBJECT::[dbo].[GetConfigurationValue] TO [IRMAAdminRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[GetConfigurationValue] TO [IRMAClientRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[GetConfigurationValue] TO [IRMASchedJobsRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[GetConfigurationValue] TO [IRMAReportsRole]
    AS [dbo];

