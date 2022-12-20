param (
		[string]$configName,
        [string]$replacementValues,
		[string]$installPath,
		[string]$adminUserName,
		[string]$adminPassword,
		[string]$machineList
    )
Write-Verbose "Config Name = $configName" -verbose
Write-Verbose "replacementValues = $replacementValues" -verbose
Write-Verbose "Iinstall Path = $installPath" -verbose
Write-Verbose "Machines = $machineList" -verbose

$env:CURRENT_TASK_ROOTDIR = Split-Path -Parent $MyInvocation.MyCommand.Path
. $env:CURRENT_TASK_ROOTDIR\SetServiceConfigsOnServer.ps1

(RunSetAppSettings -configName $configName -replacementValues $replacementValues -installPath $installPath -adminUserName $adminUserName -adminPassword $adminPassword -machineList $machineList)
