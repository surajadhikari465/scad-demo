param (
		[string]$ServerHost ,
		[string]$adminUserName,
		[string]$adminPassword,
		[string]$SQLServer ,
		[string]$WindowsAuthentication ,
		[string]$Database ,
		[string]$Login ,
		[string]$Password ,
        [string]$ScriptFile,
		[string]$Timeout
    )
Write-Verbose "Server Host = $ServerHost"
Write-Verbose "SQLServer = $SQLServer"
Write-Verbose "adminUserName = $adminUserName"
Write-Verbose "WindowsAuthentication = $WindowsAuthentication"
Write-Verbose "Database = $Database"
Write-Verbose "ScriptFile = $ScriptFile"
Write-Verbose "Login  = $Login"
Write-Verbose "Timeout = $Timeout"

$env:CURRENT_TASK_ROOTDIR = Split-Path -Parent $MyInvocation.MyCommand.Path
. $env:CURRENT_TASK_ROOTDIR\SQLServerScriptRunner.ps1

(RunSqlScript -ServerHost $ServerHost -adminUserName $adminUserName -adminPassword $adminPassword -SQLServer $SQLServer -WindowsAuthentication $WindowsAuthentication -Database $Database -Login $Login -Password $Password -ScriptFile $ScriptFile -Timeout $Timeout)
