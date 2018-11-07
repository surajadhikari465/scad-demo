IF EXISTS (SELECT * FROM dbo.sysobjects where id = object_id(N'[dbo].[GetAppConfigKeys]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
	DROP PROCEDURE [dbo].[GetAppConfigKeys]
GO

CREATE PROCEDURE [dbo].[GetAppConfigKeys]
(
	@ApplicationName	VARCHAR(50)
)
AS

-- **************************************************************************
-- Procedure: GetAppConfigKeys
--    Author: Denis Ng
--      Date: 10/13/2014
--
-- Description:
-- This procedure will retrive app cofig keys/values for the specified applilcation 
--
-- Modification History:
-- Date       	Init  			TFS   	Comment
-- 10/13/2014	DN   			15457	Created

BEGIN
SELECT ack.Name AS [Key], acv.Value
		FROM AppConfigValue acv INNER JOIN AppConfigEnv ace
		ON acv.EnvironmentID = ace.EnvironmentID 
		INNER JOIN AppConfigApp aca
		ON acv.ApplicationID = aca.ApplicationID 
		INNER JOIN AppConfigKey ack
		ON acv.KeyID = ack.KeyID 
		WHERE aca.Name = LTRIM(@ApplicationName) AND
		acv.Deleted = 0 AND
		SUBSTRING(ace.Name,1,1) = SUBSTRING((SELECT Environment FROM Version WHERE ApplicationName = 'IRMA CLIENT'),1,1)
END

GO