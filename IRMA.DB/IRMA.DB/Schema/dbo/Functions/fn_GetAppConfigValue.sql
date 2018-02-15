
CREATE FUNCTION [dbo].[fn_GetAppConfigValue] 
(
	@ConfigurationKeyName varchar(150),
	@ApplicationName varchar(20)
)
RETURNS varchar(350)
AS
BEGIN
	DECLARE @configurationValue varchar(350)

	SELECT  
		@configurationValue = acv.Value
	FROM 
		AppConfigValue acv
		INNER JOIN AppConfigEnv ace ON acv.EnvironmentID = ace.EnvironmentID 
		INNER JOIN AppConfigApp aca ON acv.ApplicationID = aca.ApplicationID 
		INNER JOIN AppConfigKey ack ON acv.KeyID = ack.KeyID 
	WHERE 
		aca.Name = @ApplicationName
		AND ack.Name = @ConfigurationKeyName
		and SUBSTRING(ace.Name, 1, 1) = SUBSTRING((SELECT top(1) Environment FROM Version), 1, 1)

	RETURN @configurationValue
END

GO
GRANT EXECUTE
    ON OBJECT::[dbo].[fn_GetAppConfigValue] TO [IRMAClientRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[fn_GetAppConfigValue] TO [MammothRole]
    AS [dbo];

