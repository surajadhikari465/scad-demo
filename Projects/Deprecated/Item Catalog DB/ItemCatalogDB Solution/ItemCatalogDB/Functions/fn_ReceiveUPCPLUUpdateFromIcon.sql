IF EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[fn_ReceiveUPCPLUUpdateFromIcon]') AND type in (N'FN', N'IF', N'TF', N'FS', N'FT'))
	DROP FUNCTION [dbo].[fn_ReceiveUPCPLUUpdateFromIcon]
GO

CREATE FUNCTION [dbo].[fn_ReceiveUPCPLUUpdateFromIcon]
()
RETURNS SMALLINT
AS
/*
[ Modification History ]
--------------------------------------------
Date		Developer		TFS		Comment
--------------------------------------------
2014-05-23	Denis Ng		15008	Function creation. 	
2014-06-23	Denis Ng		15220	Updated function to use 
									two new config keys		
2014-07-29	Denis Ng		15330	Updated name of the config key to retrieve
									from EnableUPCIConToIRMAFlow to EnableUPCIRMAToIConFlow
2014-08-15	Denis Ng		15377	Added two new config keys: AllowNonValidatedUPCToBatch & AllowNonValidatedPLUToBatch
*/
BEGIN
--	ReceiveUPCPLUFromIcon Values:
--	0: Not receiving updates
--	1: Only UPC 
--	2: Only PLU
--	3: Both UPC and PLU	

DECLARE	@Status							SMALLINT
DECLARE @EnableUPCIRMAToIConFlow		SMALLINT = 0
DECLARE @EnablePLUIRMAIconFlow			SMALLINT = 0
DECLARE @AllowNonValidatedUPCToBatch	SMALLINT = 0
DECLARE @AllowNonValidatedPLUToBatch	SMALLINT = 0

	
SELECT  @EnableUPCIRMAToIConFlow = CASE WHEN ISNULL(acv.Value,0) > 0 THEN 1 ELSE 0 END
		FROM AppConfigValue acv INNER JOIN AppConfigEnv ace
		ON acv.EnvironmentID = ace.EnvironmentID 
		INNER JOIN AppConfigApp aca
		ON acv.ApplicationID = aca.ApplicationID 
		INNER JOIN AppConfigKey ack
		ON acv.KeyID = ack.KeyID 
		WHERE aca.Name = 'IRMA Client' AND
		ack.Name = 'EnableUPCIRMAToIConFlow' and
		SUBSTRING(ace.Name,1,1) = SUBSTRING((SELECT Environment FROM Version WHERE ApplicationName = 'IRMA CLIENT'),1,1)

SELECT  @EnablePLUIRMAIconFlow = CASE WHEN ISNULL(acv.Value,0) > 0 THEN 1 ELSE 0 END
		FROM AppConfigValue acv INNER JOIN AppConfigEnv ace
		ON acv.EnvironmentID = ace.EnvironmentID 
		INNER JOIN AppConfigApp aca
		ON acv.ApplicationID = aca.ApplicationID 
		INNER JOIN AppConfigKey ack
		ON acv.KeyID = ack.KeyID 
		WHERE aca.Name = 'IRMA Client' AND
		ack.Name = 'EnablePLUIRMAIConFlow' and
		SUBSTRING(ace.Name,1,1) = SUBSTRING((SELECT Environment FROM Version WHERE ApplicationName = 'IRMA CLIENT'),1,1)

SELECT  @AllowNonValidatedUPCToBatch = CASE WHEN ISNULL(acv.Value,0) > 0 THEN 1 ELSE 0 END
		FROM AppConfigValue acv INNER JOIN AppConfigEnv ace
		ON acv.EnvironmentID = ace.EnvironmentID 
		INNER JOIN AppConfigApp aca
		ON acv.ApplicationID = aca.ApplicationID 
		INNER JOIN AppConfigKey ack
		ON acv.KeyID = ack.KeyID 
		WHERE aca.Name = 'IRMA Client' AND
		ack.Name = 'AllowNonValidatedUPCToBatch' and
		SUBSTRING(ace.Name,1,1) = SUBSTRING((SELECT Environment FROM Version WHERE ApplicationName = 'IRMA CLIENT'),1,1)

SELECT  @AllowNonValidatedPLUToBatch = CASE WHEN ISNULL(acv.Value,0) > 0 THEN 1 ELSE 0 END
		FROM AppConfigValue acv INNER JOIN AppConfigEnv ace
		ON acv.EnvironmentID = ace.EnvironmentID 
		INNER JOIN AppConfigApp aca
		ON acv.ApplicationID = aca.ApplicationID 
		INNER JOIN AppConfigKey ack
		ON acv.KeyID = ack.KeyID 
		WHERE aca.Name = 'IRMA Client' AND
		ack.Name = 'AllowNonValidatedPLUToBatch' and
		SUBSTRING(ace.Name,1,1) = SUBSTRING((SELECT Environment FROM Version WHERE ApplicationName = 'IRMA CLIENT'),1,1)

IF @AllowNonValidatedUPCToBatch = 1
	SET @EnableUPCIRMAToIConFlow = 0

IF @AllowNonValidatedPLUToBatch = 1
	SET @EnablePLUIRMAIconFlow = 0

IF @EnableUPCIRMAToIConFlow = 0 AND @EnablePLUIRMAIconFlow = 0
	SET @Status = 0

IF @EnableUPCIRMAToIConFlow = 1 AND @EnablePLUIRMAIconFlow = 0
	SET @Status = 1

IF @EnableUPCIRMAToIConFlow = 0 AND @EnablePLUIRMAIconFlow = 1
	SET @Status = 2

IF @EnableUPCIRMAToIConFlow = 1 AND @EnablePLUIRMAIconFlow = 1
	SET @Status = 3

RETURN @Status
END
GO