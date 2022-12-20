param (
    [string][Parameter(Mandatory=$true)][ValidateNotNullOrEmpty()] $topshelfExe,
    [string][Parameter(Mandatory=$true)][ValidateNotNullOrEmpty()] $machineList,
    [string][Parameter(Mandatory=$true)][ValidateNotNullOrEmpty()] $adminUserName,
    [string][Parameter(Mandatory=$true)][ValidateNotNullOrEmpty()] $adminPassword
    )

$env:CURRENT_TASK_ROOTDIR = Split-Path -Parent $MyInvocation.MyCommand.Path
. $env:CURRENT_TASK_ROOTDIR\UninstallTopShelfService.ps1

(RunUninstallTopShelfService -topshelfExe $topshelfExe -adminUserName $adminUserName -adminPassword $adminPassword -machineList $machineList)
