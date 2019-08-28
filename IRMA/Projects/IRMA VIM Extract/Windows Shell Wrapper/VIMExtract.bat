@ECHO OFF


REM Two-letter region should be passed as cmd-line arg.
set region=%~1

REM This gets our location (where this batch script lives).
set scriptDir=%~dp0
REM Initial default log file, in case valid region isn't passed.
set logFile=%scriptDir%%~n0.log

REM Ensure value was passed.
if not defined region (
	echo [%date% %time%] *** ERROR *** :: Regional abbreviation must be passed as cmd-line argument. >> "%logFile%" 2>&1
	exit 1
)

REM ---------------------------------------------------------------
REM TODO - Region value is valid 2-letter region ID?
REM ---------------------------------------------------------------

REM Update log file to region-specific name.
set logFile=%scriptDir%%~n0.%region%.log

REM Change to primary dest folder.
e:
cd E:\host\ftp\vimfiles\%region%

REM Show then run execute-SSIS command.
echo [%date% %time%] Starting VIM Extract SSIS for %region% region >> "%logFile%"
set cmd="C:\Program Files (x86)\Microsoft SQL Server\140\DTS\Binn\DTEXEC.exe" /File "E:\Apps\VIMExtract\vim_extract.dtsx" /ConfigFile "E:\Apps\VIMExtract\Cfg\VIMConfigFile_%region%.dtsConfig" /CheckPointing off
echo [%date% %time%][CMD] %cmd% >> "%logFile%"
%cmd%

REM Zip all .dat files and save output to regional log file.
echo [%date% %time%] Zipping %region% >> "%logFile%"
"C:\Program Files (x86)\gnuwin32\bin\gzip" -S .Z -f *.dat >> "%logFile%" 2>&1
REM Show any/all zipped file, which will include timestamps.
dir *.z >> "%logFile%" 2>&1
echo [%date% %time%] Finished %region% >> "%logFile%"
echo --------------------------------------- >> "%logFile%"
