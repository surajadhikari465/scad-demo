param (
    [string][Parameter(Mandatory=$true)][ValidateNotNullOrEmpty()] $topshelfExe,
    [string][Parameter(Mandatory=$true)][ValidateNotNullOrEmpty()] $machineList,
    [string][Parameter(Mandatory=$true)][ValidateNotNullOrEmpty()] $adminUserName,
    [string][Parameter(Mandatory=$true)][ValidateNotNullOrEmpty()] $adminPassword,
	[string][Parameter(Mandatory=$true)][ValidateSet("custom", "localsystem", "localservice", "networkservice")] $specialUser,
	[string] $serviceUsername,
    [string] $servicePassword,
    [string] $instanceName,
	[string] $uninstallFirst
    )

$env:CURRENT_TASK_ROOTDIR = Split-Path -Parent $MyInvocation.MyCommand.Path
. $env:CURRENT_TASK_ROOTDIR\InstallTopShelfService.ps1

(RunInstallTopShelfService -topshelfExe $topshelfExe -specialUser $specialUser -serviceUsername $serviceUsername -servicePassword $servicePassword -instanceName $instanceName -uninstallFirst $uninstallFirst -adminUserName $adminUserName -adminPassword $adminPassword -machineList $machineList)
