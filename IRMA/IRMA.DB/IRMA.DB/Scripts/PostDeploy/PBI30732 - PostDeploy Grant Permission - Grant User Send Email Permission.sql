USE msdb;
 
--give this GROUP rights to use dbmail

IF RIGHT(@@SERVERNAME, 1) = 'P' 
	exec sp_addrolemember 'DatabaseMailUserRole', 'WFM\PDXExtractUserPrd'
ELSE IF RIGHT(@@SERVERNAME, 1) = 'Q'
	exec sp_addrolemember 'DatabaseMailUserRole', 'WFM\PDXExtractUserQA'
ELSE 
	exec sp_addrolemember 'DatabaseMailUserRole', 'WFM\PDXExtractUserDev'