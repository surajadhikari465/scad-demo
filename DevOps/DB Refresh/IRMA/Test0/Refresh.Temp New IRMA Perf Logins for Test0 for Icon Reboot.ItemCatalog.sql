If NOT EXISTS (SELECT loginname FROM MASTER.dbo.syslogins WHERE name = 'WFM\IconInterfaceUserQP' )  BEGIN CREATE LOGIN [WFM\IconInterfaceUserQP] FROM WINDOWS WITH DEFAULT_DATABASE = [master] END  ELSE  BEGIN  Print' Login WFM\IconInterfaceUserQP Already Exists.' END 
If NOT EXISTS (SELECT loginname FROM MASTER.dbo.syslogins WHERE name = 'WFM\IconWebQP' )  BEGIN CREATE LOGIN [WFM\IconWebQP] FROM WINDOWS WITH DEFAULT_DATABASE = [master] END  ELSE  BEGIN  Print' Login WFM\IconWebQP Already Exists.' END 
If NOT EXISTS (SELECT loginname FROM MASTER.dbo.syslogins WHERE name = 'WFM\MammothQP' )  BEGIN CREATE LOGIN [WFM\MammothQP] FROM WINDOWS WITH DEFAULT_DATABASE = [master] END  ELSE  BEGIN  Print' Login WFM\MammothQP Already Exists.' END 
If NOT EXISTS (SELECT loginname FROM MASTER.dbo.syslogins WHERE name = 'WFM\PDXExtractUserQP' )  BEGIN CREATE LOGIN [WFM\PDXExtractUserQP] FROM WINDOWS WITH DEFAULT_DATABASE = [master] END  ELSE  BEGIN  Print' Login WFM\PDXExtractUserQP Already Exists.' END 
