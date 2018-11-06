SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE FUNCTION dbo.fn_GetAppConfigValue 
(
	@ConfigurationKeyName varchar(150),
	@ApplicationName varchar(20)
)
RETURNS varchar(100)
AS
BEGIN
	DECLARE @configurationValue varchar(100);

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
		and SUBSTRING(ace.Name,1,1) = SUBSTRING((SELECT Environment FROM Version WHERE ApplicationName = @ApplicationName),1,1);

	RETURN @configurationValue;
END
GO

