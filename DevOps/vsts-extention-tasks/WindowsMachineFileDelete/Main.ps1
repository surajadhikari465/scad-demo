param (
		[string]$fileToDelete,
		[string]$isDirectory,
		[string]$adminUserName,
		[string]$adminPassword,
		[string]$machineList
    )
Write-Verbose "File To Delete = $fileToDelete" -verbose
Write-Verbose "IsDirectory = $isDirectory" -verbose
Write-Verbose "Machines = $machineList" -verbose

$env:CURRENT_TASK_ROOTDIR = Split-Path -Parent $MyInvocation.MyCommand.Path
. $env:CURRENT_TASK_ROOTDIR\WindowsMachineFileDelete.ps1

(DeleteFile -fileToDelete $fileToDelete -adminUserName $adminUserName -adminPassword $adminPassword -machineList $machineList -isDirectory $isDirectory)
