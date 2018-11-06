@echo off
rem ************************************************************************
rem   Script: IRMA_Cleanup_logs.cmd
rem   Author: Min Zhao
rem     Date: 12/20/2013
rem
rem  Description:
rem  This cmd will kick off  
rem  file folders in preparation for adding FilePurge commands to the various
rem  DM jobs to keep their data files cleaned up.
rem
rem Change History:
rem Date        Init. Description
rem 05/07/2012  RS    Copied from imha_update.cmd and modified.
rem ************************************************************************

set cleanup_folder=%1
set file_age=%2

@echo *********************************************************************
@echo                  IRMA Clean up log files
@echo                  Script: IRMA_Cleanup_logs.cmd
@echo        Target Directory: %cleanup_folder%
@echo        File Age in Days: %file_age%
@echo *********************************************************************
@echo If the information above is correct, press [enter] otherwise press Ctrl-C to exit.
rem pause


rem ************************************************************************
rem  Map the DNS directory to a local drive first and delete files (including 
rem  the ones in the sub-folder under the directory) if the files are older than
rem  what's specified in %file_age%
rem ************************************************************************
PushD %cleanup_folder% &&(
    forfiles -s -m *.log -d -%file_age% -c "cmd /c del /q @path" 
	) & PopD

pause
goto exit

:exit
@echo all done, exiting.
