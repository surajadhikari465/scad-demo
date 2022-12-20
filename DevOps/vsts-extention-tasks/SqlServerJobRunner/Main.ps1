param (
        [string]$SQLServer ,
		[string]$ServerHost ,
		[string]$WindowsAuthentication ,
		[string]$Login ,
		[string]$Password ,
        [string]$JobName,
		[string]$adminUserName,
		[string]$adminPassword
    )
Write-Verbose "Server Host = $ServerHost"
Write-Verbose "SQLServer = $SQLServer"
Write-Verbose "adminUserName = $adminUserName"
Write-Verbose "WindowsAuthentication = $WindowsAuthentication"
Write-Verbose "Login  = $Login"
Write-Verbose "JobName = $JobName"

$env:CURRENT_TASK_ROOTDIR = Split-Path -Parent $MyInvocation.MyCommand.Path
. $env:CURRENT_TASK_ROOTDIR\SqlServerJobRunner.ps1

(RunSqlServerJob -ServerHost $ServerHost -adminUserName $adminUserName -adminPassword $adminPassword -SQLServer $SQLServer -WindowsAuthentication $WindowsAuthentication -Login $Login -Password $Password -JobName $JobName)
