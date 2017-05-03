CREATE PROCEDURE [dbo].[AppConfig_GetAppConfigSetting]

	@ApplicationID uniqueidentifier,
	@EnvironmentID uniqueidentifier,
	@KeyName	varchar(150)
	
AS 

DECLARE @xml XML
DECLARE @Configuration TABLE (Doc XML)

INSERT @Configuration(Doc)
EXEC	[dbo].[AppConfig_GetConfigDoc]
		@ApplicationID,
		@EnvironmentID

SELECT @xml = Doc FROM @Configuration

DECLARE @Value varchar(350)

SET @Value =	(
					SELECT
						ConfigDoc.Configuration.value('@value', 'VARCHAR(350)')
					FROM
						@xml.nodes('/configuration/appSettings/add') AS ConfigDoc(Configuration)
					WHERE
						ConfigDoc.Configuration.value('@key', 'VARCHAR(150)') = @KeyName
				)

SELECT 'Value' = @Value
GO
GRANT EXECUTE
    ON OBJECT::[dbo].[AppConfig_GetAppConfigSetting] TO [IRMAAdminRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[AppConfig_GetAppConfigSetting] TO [IRMAClientRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[AppConfig_GetAppConfigSetting] TO [IRMASchedJobsRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[AppConfig_GetAppConfigSetting] TO [IRMAReportsRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[AppConfig_GetAppConfigSetting] TO [IRMASLIMRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[AppConfig_GetAppConfigSetting] TO [IRMAPromoRole]
    AS [dbo];

