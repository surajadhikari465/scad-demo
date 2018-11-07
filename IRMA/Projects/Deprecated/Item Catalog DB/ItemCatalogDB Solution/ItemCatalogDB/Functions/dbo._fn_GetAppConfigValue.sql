IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[fn_GetAppConfigValue]') AND type in (N'FN', N'IF', N'TF', N'FS', N'FT'))
	DROP FUNCTION [dbo].[fn_GetAppConfigValue]
GO

CREATE FUNCTION [dbo].[fn_GetAppConfigValue] 
(
	@ConfigurationKeyName varchar(150),
	@ApplicationName varchar(20)
)
RETURNS varchar(100)
AS
BEGIN
	DECLARE @configurationValue varchar(100)

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
