 
if exists (select * from dbo.sysobjects where id = object_id(N'dbo.GetConfigurationData') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
	drop procedure dbo.GetConfigurationData
GO

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
go
