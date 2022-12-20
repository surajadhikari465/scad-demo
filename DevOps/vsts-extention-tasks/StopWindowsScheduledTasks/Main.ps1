param (
    [string]$machineList,
	[string]$taskList,
    [string]$adminUserName,
    [string]$adminPassword
    )

$env:CURRENT_TASK_ROOTDIR = Split-Path -Parent $MyInvocation.MyCommand.Path
. $env:CURRENT_TASK_ROOTDIR\StopScheduleTasks.ps1

(RunTaskStops -machineList $machineList -adminUserName $adminUserName -adminPassword $adminPassword -taskList $taskList)
