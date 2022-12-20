param (
    [string]$machineList,
	[string]$serviceList,
    [string]$adminUserName,
    [string]$adminPassword
    )

$env:CURRENT_TASK_ROOTDIR = Split-Path -Parent $MyInvocation.MyCommand.Path
. $env:CURRENT_TASK_ROOTDIR\StopWindowsServices.ps1

(RunStopWindowsServices -machineList $machineList -adminUserName $adminUserName -adminPassword $adminPassword -serviceList $serviceList)