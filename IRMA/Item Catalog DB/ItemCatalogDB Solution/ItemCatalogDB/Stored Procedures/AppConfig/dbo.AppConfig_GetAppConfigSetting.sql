 IF  EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[dbo].[AppConfig_GetAppConfigSetting]') AND OBJECTPROPERTY(id,N'IsProcedure') = 1)
DROP PROCEDURE [dbo].[AppConfig_GetAppConfigSetting]
GO

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

