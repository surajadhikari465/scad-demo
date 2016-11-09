-- This script will be run against IRMA by an IRMA developer right before ReCon is turned back on
-- Expected results:
-- AllowNonValidatedPLUToBatch	0
-- AllowNonValidatedUPCToBatch	0
-- EnableUPCIConToIRMAFlow	1
-- ConvertMultiple	1

select  
A.Name as [App Name]
, E.ShortName as Environment
--, E.EnvironmentID??
, K.Name as [Key Name]
, V.Value as [Key Value]
--, V.*
from AppConfigValue V
inner join AppConfigApp A on V.ApplicationID = A.ApplicationID
inner join AppConfigEnv E on V.EnvironmentID = E.EnvironmentID 
inner join AppConfigKey K on V.KeyID = K.KeyID
where 1=1
and E.ShortName in ('PRD')
and V.Deleted = 0
and K.Name in ('EnableUPCIConToIRMAFlow ','AllowNonValidatedPLUToBatch', 'AllowNonValidatedUPCToBatch', 'ConvertMultiple')
and A.Name in ('IRMA CLIENT', 'POS PUSH JOB')
order by A.Name, K.Name, E.ShortName