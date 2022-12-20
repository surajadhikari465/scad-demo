param (
		[string]$configName,
        [string]$transform,
		[string]$installPath,
		[string]$adminUserName,
		[string]$adminPassword,
		[string]$machineList
    )
Write-Verbose "Config Name = $configName" -verbose
Write-Verbose "Transform = $transform" -verbose
Write-Verbose "Iinstall Path = $installPath" -verbose
Write-Verbose "Machines = $machineList" -verbose

$env:CURRENT_TASK_ROOTDIR = Split-Path -Parent $MyInvocation.MyCommand.Path
. $env:CURRENT_TASK_ROOTDIR\CodeAssassinConfigTransformPicker.ps1

(RunPickConfigs -configName $configName -transform $transform -installPath $installPath -adminUserName $adminUserName -adminPassword $adminPassword -machineList $machineList)
