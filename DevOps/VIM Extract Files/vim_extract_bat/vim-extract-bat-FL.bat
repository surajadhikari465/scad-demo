@ECHO OFF
CLS
ECHO You are about to execute the TestPackage SSIS package
"C:\Program Files\Microsoft SQL Server\140\DTS\Binn\DTEXEC.exe" /File "E:\inetpub\ftproot\vimfiles\vim_extract.dtsx" /config "E:\inetpub\ftproot\vimfiles\vim_config\VIMConfigFile_FL.dtsConfig"
