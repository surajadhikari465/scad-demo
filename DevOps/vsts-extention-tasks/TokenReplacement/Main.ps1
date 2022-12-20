param (
        [string]$replacementValues,
		[string]$fileList,
		[string]$adminUserName,
		[string]$adminPassword,
		[string]$machineList
    )
Write-Verbose "Config Name = $configName" -verbose
Write-Verbose "replacementValues = $replacementValues" -verbose
Write-Verbose "fileList = $fileList" -verbose
Write-Verbose "Machines = $machineList" -verbose

$env:CURRENT_TASK_ROOTDIR = Split-Path -Parent $MyInvocation.MyCommand.Path
. $env:CURRENT_TASK_ROOTDIR\TokenReplacement.ps1

(RunTokenReplacement -replacementValues $replacementValues -fileList $fileList -adminUserName $adminUserName -adminPassword $adminPassword -machineList $machineList)
