param (
    [string]$machineList,
	[string]$serviceName, 
	[string]$displayName,
	[string]$binaryPath, 
	[string]$description, 
	[string]$serviceUsername, 
	[string]$servicePassword, 
	[string]$startUpType, 
    [string]$adminUserName,
    [string]$adminPassword
    )

$env:CURRENT_TASK_ROOTDIR = Split-Path -Parent $MyInvocation.MyCommand.Path
. $env:CURRENT_TASK_ROOTDIR\InstallWindowsService.ps1

(RunInstallServices -machineList $machineList -adminUserName $adminUserName -adminPassword $adminPassword -serviceName $serviceName -displayName $displayName -binaryPath $binaryPath -description $description -serviceUsername $serviceUsername -servicePassword $servicePassword -startUpType $startUpType)