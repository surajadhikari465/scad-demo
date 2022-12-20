param (
        [string]$SQLServer ,
		[string]$ServerHost ,
		[string]$WindowsAuthentication ,
		[string]$Login ,
		[string]$Password ,
        [string]$JobNames,
		[string]$adminUserName,
		[string]$adminPassword
    )
Write-Verbose "Server Host = $ServerHost"
Write-Verbose "SQLServer = $SQLServer"
Write-Verbose "adminUserName = $adminUserName"
Write-Verbose "WindowsAuthentication = $WindowsAuthentication"
Write-Verbose "Login  = $Login"
Write-Verbose "JobNames = $JobNames"

$env:CURRENT_TASK_ROOTDIR = Split-Path -Parent $MyInvocation.MyCommand.Path
. $env:CURRENT_TASK_ROOTDIR\SqlJobWaiter.ps1

(WaitForSqlServerJobs -ServerHost $ServerHost -adminUserName $adminUserName -adminPassword $adminPassword -SQLServer $SQLServer -WindowsAuthentication $WindowsAuthentication -Login $Login -Password $Password -JobNames $JobNames)
