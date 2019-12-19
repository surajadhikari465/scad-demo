EXECUTE sp_addlinkedsrvlogin @rmtsrvname = N'VIM_DP', @useself = N'FALSE', @rmtuser = N'vim_user_oos';


GO
EXECUTE sp_addlinkedsrvlogin @rmtsrvname = N'STELLA_DP', @useself = N'FALSE', @rmtuser = N'stella_user';


GO
EXECUTE sp_addlinkedsrvlogin @rmtsrvname = N'VIMORACLE', @useself = N'FALSE', @rmtuser = N'vim';


GO
EXECUTE sp_addlinkedsrvlogin @rmtsrvname = N'VIM_DQ', @useself = N'FALSE', @rmtuser = N'vim';


GO
EXECUTE sp_addlinkedsrvlogin @rmtsrvname = N'STELLA_DT', @useself = N'FALSE', @rmtuser = N'stella_user';


GO
EXECUTE sp_addlinkedsrvlogin @rmtsrvname = N'STELLA_DEV', @useself = N'FALSE', @rmtuser = N'stella_user';

