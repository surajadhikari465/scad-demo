CREATE PROCEDURE dbo.GetConfigurationData
AS 

BEGIN
	SELECT *, 
		sql_variant_property(ConfigValue, 'BaseType') as BaseType,
		sql_variant_property(ConfigValue, 'Precision') as Precision,
		sql_variant_property(ConfigValue, 'Scale') as Scale
	FROM 
		dbo.ConfigurationData
	ORDER BY 
		ConfigKey
END
GO
GRANT EXECUTE
    ON OBJECT::[dbo].[GetConfigurationData] TO [IRMAAdminRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[GetConfigurationData] TO [IRMAClientRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[GetConfigurationData] TO [IRMASchedJobsRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[GetConfigurationData] TO [IRMAReportsRole]
    AS [dbo];

