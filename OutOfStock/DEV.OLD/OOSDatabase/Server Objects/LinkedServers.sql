EXECUTE sp_addlinkedserver @server = N'VIM_DP', @srvproduct = N'OraOLEDB.Oracle', @provider = N'OraOLEDB.Oracle', @datasrc = N'VIM_DP', @provstr = N'(DESCRIPTION=(ADDRESS=(PROTOCOL=tcp)(HOST=odxp-scan.wfm.pvt)(PORT=1521))(CONNECT_DATA=(SERVICE_NAME=VIM_DP)))';


GO
EXECUTE sp_addlinkedserver @server = N'STELLA_DP', @srvproduct = N'OraOLEDB.Oracle', @provider = N'OraOLEDB.Oracle', @datasrc = N'STELLA_DP', @provstr = N'(DESCRIPTION=(ADDRESS=(PROTOCOL=TCP)(HOST=odxp-scan.wfm.pvt)(PORT=1521))(CONNECT_DATA=(SERVICE_NAME=STELLA_DP)))';


GO
EXECUTE sp_addlinkedserver @server = N'VIMORACLE', @srvproduct = N'OraOLEDB.Oracle', @provider = N'OraOLEDB.Oracle', @datasrc = N'vim_dt', @provstr = N'(DESCRIPTION=(ADDRESS=(PROTOCOL=tcp)(HOST=gemini_nonprd.wfm.pvt)(PORT=1521))(CONNECT_DATA=(SERVICE_NAME=vim_dt)))';


GO
EXECUTE sp_addlinkedserver @server = N'VIM_DQ', @srvproduct = N'OraOLEDB.Oracle', @provider = N'OraOLEDB.Oracle', @datasrc = N'vim_dq', @provstr = N'(DESCRIPTION=(ADDRESS=(PROTOCOL=tcp)(HOST=gemini_nonprd.wfm.pvt)(PORT=1521))(CONNECT_DATA=(SERVICE_NAME=vim_dq)))';


GO
EXECUTE sp_addlinkedserver @server = N'STELLA_DT', @srvproduct = N'OraOLEDB.Oracle', @provider = N'OraOLEDB.Oracle', @datasrc = N'stella_dt', @provstr = N'(DESCRIPTION=(ADDRESS=(PROTOCOL=TCP)(HOST=cexd-scan.wfm.pvt)(PORT=1521))(CONNECT_DATA=(SERVICE_NAME=stella_dt)))';


GO
EXECUTE sp_addlinkedserver @server = N'STELLA_DEV', @srvproduct = N'OraOLEDB.Oracle', @provider = N'OraOLEDB.Oracle', @datasrc = N'stella_dt', @provstr = N'(DESCRIPTION=(ADDRESS=(PROTOCOL=TCP)(HOST=cexd-scan.wfm.pvt)(PORT=1521))(CONNECT_DATA=(SERVICE_NAME=stella_dt)))';

