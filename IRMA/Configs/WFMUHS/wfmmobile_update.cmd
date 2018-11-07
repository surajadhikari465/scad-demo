@echo off
rem ************************************************************************
rem   Script: wfmmobile_update.cmd
rem   Author: Ron Savage
rem     Date: 07/28/2015
rem
rem  Description:
rem  This command file automates the update process for WFM Mobile.
rem
rem  Syntax:
rem  wfmmobile_update.cmd [AppServer(Test|QA|Prd)] [AppName] [AppVersion] [AppFileName]
rem
rem  AppServer   - Used to generate server names (i.e. IRMA[AppServer]Appx)
rem  AppName     - Used to generate the PluginUpdates folder (i.e. E:\WebApps\WFM-Mobile\Configuration\Updates\PluginUpdates\[AppName]\)
rem  AppVersion  - Used to generate the version folder (i.e. E:\WebApps\WFM-Mobile\Configuration\Updates\PluginUpdates\[AppName]\[AppVersion]\)
rem  AppFileName - Used to specify the file to go in the version folder (i.e. DVO-TST.CAB )
rem 
rem Change History:
rem Date        Init. Description
rem 07/28/2015  RS    Created (copy from App_update.cmd).
rem ************************************************************************

rem ************************************************************************
rem  Check for and get command line arguments, exit if not there
rem ************************************************************************
set AppServer=%1
set AppName=%2
set AppVersion=%3
set AppFile=%4
if /i "%AppFile%" == "" goto syntax

rem ************************************************************************
rem  Define the update folder, and check for it
rem ************************************************************************
set AppServerBase=\\IRMA%AppServer%App
set WfmMobilePath=WebApps\WFM-Mobile\Configuration\Updates\PluginUpdates

set WfmMobileCfgPath=WebApps\WFM-Mobile\Configuration
set WfmMobileCfgFile=WFMUHS.xml

if not exist %AppServerBase%1\E$\%WfmMobilePath% goto nofolder

rem ************************************************************************
rem  Define the Server names and a log file name
rem ************************************************************************
set logFile=%AppName%_%AppVersion%_update.log

@echo *********************************************************************
@echo         WFM Mobile App Installation Script
@echo        Script: WfmMobile_update.cmd
@echo    App Server: %AppServer%
@echo      App Name: %AppName%
@echo   App Version: %AppVersion%
@echo      App File: %AppFile%
@echo *********************************************************************
@echo If the information above is correct, press [enter] otherwise press Ctrl-C to exit.
pause

rem ************************************************************************
rem  Write the header info to the log
rem ************************************************************************
@echo ********************************************************************* >> %logFile% 2>&1
@echo         WFM Mobile App Installation Script >> %logFile% 2>&1
@echo        Script: WfmMobile_update.cmd >> %logFile% 2>&1
@echo    App Server: %AppServer% >> %logFile% 2>&1
@echo      App Name: %AppName% >> %logFile% 2>&1
@echo   App Version: %AppVersion% >> %logFile% 2>&1
@echo      App File: %AppFile% >> %logFile% 2>&1
@echo ********************************************************************* >> %logFile% 2>&1
@echo. >> %logFile% 2>&1

rem ************************************************************************
rem  Create App version folder and copy the file
rem ************************************************************************
if /i "%AppServer%" == "prd" ( set ServerList=1,2,3,4,5,6,7,8 ; ) else ( set ServerList=1,2,3,4 ; )

@echo Server List: %ServerList%

for %%N in ( %ServerList% ) do (
@echo Creating folder %AppServerBase%%%N\E$\%WfmMobilePath%\%AppName%\%AppVersion% ...
@echo Creating folder %AppServerBase%%%N\E$\%WfmMobilePath%\%AppName%\%AppVersion% ... >> %logFile% 2>&1
if not exist %AppServerBase%%%N\E$\%WfmMobilePath%\%AppName%\%AppVersion% mkdir %AppServerBase%%%N\E$\%WfmMobilePath%\%AppName%\%AppVersion%  >> %logFile% 2>&1
if ERRORLEVEL 1 @echo Error creating folder ...

@echo Copying %AppFile% to folder %AppServerBase%%%N\E$\%WfmMobilePath%\%AppName%\%AppVersion%\ ...
@echo Copying %AppFile% to folder %AppServerBase%%%N\E$\%WfmMobilePath%\%AppName%\%AppVersion%\ ... >> %logFile% 2>&1
xcopy /y /r %AppFile%  %AppServerBase%%%N\E$\%WfmMobilePath%\%AppName%\%AppVersion%\  >> %logFile% 2>&1
if ERRORLEVEL 1 @echo Error copying %AppFile% ...

@echo Copying %WfmMobileCfgFile% to folder %AppServerBase%%%N\E$\%WfmMobileCfgPath%\ ...
@echo Copying %WfmMobileCfgFile% to folder %AppServerBase%%%N\E$\%WfmMobileCfgPath%\ ... >> %logFile% 2>&1
xcopy /y /r %WfmMobileCfgFile%  %AppServerBase%%%N\E$\%WfmMobileCfgPath%\ >> %logFile% 2>&1
if ERRORLEVEL 1 @echo Error copying %WfmMobileCfgFile% ...

@echo.
@echo. >> %logFile% 2>&1
)

:UpdateDone

goto exit

:syntax
@echo.
@echo. Syntax: wfmmobile_update.cmd [AppServer(Test or QA or Prd)] [AppName] [AppVersion] [AppFileName]
@echo.
goto exit

:nofolder
@echo.
@echo. No server/folder [%AppServerBase%1\E$\%WfmMobilePath%] found, exiting ...
@echo.

:exit
@echo all done, exiting.
@echo all done, exiting. >> %logFile% 2>&1
