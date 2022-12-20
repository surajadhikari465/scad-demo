param (
    [string]$machineList,
	[string]$serviceList,
    [string]$adminUserName,
    [string]$adminPassword
    )
	Write-Host "Hello World"
$env:CURRENT_TASK_ROOTDIR = Split-Path -Parent $MyInvocation.MyCommand.Path
. $env:CURRENT_TASK_ROOTDIR\StartWindowsServices.ps1

(RunStartWindowsServices -machineList $machineList -adminUserName $adminUserName -adminPassword $adminPassword -serviceList $serviceList)