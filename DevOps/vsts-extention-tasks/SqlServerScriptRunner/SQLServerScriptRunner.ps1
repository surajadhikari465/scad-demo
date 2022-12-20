
function RunRemoteSqlScript
{
    [CmdletBinding()]
    param (
		[string]$SQLServer ,
		[string]$Database ,
		[bool]$WindowsAuthentication ,
		[string]$Login ,
		[string]$Password ,
        [string]$ScriptFile,
		[int]$Timeout,
		[string]$ServerHost,
		$credential
    )
	$sqlScript = Get-Content $ScriptFile -Raw 

	$hostName = ([System.Net.Dns]::GetHostByName($serverHost)).HostName
	Write-Host "Invoking command on to $hostName to run sql job $JobName"
    $result = Invoke-Command -ComputerName $hostName -Credential $credential  -ErrorVariable errmsg 2>$null -ScriptBlock {
		param (
			[string]$SQLServer ,
			[string]$Database ,
			[bool]$WindowsAuthentication ,
			[string]$Login ,
			[string]$Password ,
			[string]$sqlScript,
			[int]$Timeout
		)
		$version = 16
			while(("Microsoft.SqlServer.Management.Smo.Server" -as [type]) -eq $null){
				try{
					Add-Type -AssemblyName "Microsoft.SqlServer.Smo, Version=$Version.0.0.0, Culture=neutral, PublicKeyToken=89845dcd8080cc91" -ErrorVariable errorResult -ErrorAction Stop
				}catch{
				}
				$version = $version -1
				if($version -eq 0)
				{
					Write-Host "SMO not found";
					break;
				}
			}
  
		# Load the SQLPS module
		Push-Location; Import-Module SQLPS -DisableNameChecking; Pop-Location

		
		if($WindowsAuthentication){
			Invoke-Sqlcmd -Query $sqlScript -ServerInstance $SQLServer -ErrorAction 'Stop' -Database $Database -Verbose -QueryTimeout $Timeout
		}else{
			Invoke-Sqlcmd -Query $sqlScript -ServerInstance $SQLServer -ErrorAction 'Stop' -Database $Database -Username $Login -Password $Password -Verbose -QueryTimeout $Timeout
		}
	} -ArgumentList $SQLServer, $Database, $WindowsAuthentication, $Login, $Password, $sqlScript, $Timeout
		Write-Host $errmsg;
}


function RunSqlScript{
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

	Write-Verbose "Entering script SqlServerJobRunner.ps1" -Verbose
	Write-Verbose "SQLServer = $SQLServer" -Verbose
	Write-Verbose "Database = $Database" -Verbose
	Write-Verbose "WindowsAuthentication = $WindowsAuthentication" -Verbose
	Write-Verbose "Login  = $Login" -Verbose
	Write-Verbose "ScriptFile = $ScriptFile" -Verbose
	Write-Verbose "Timeout (in seconds) = $Timeout" -Verbose

	$ErrorActionPreference = 'Stop'

	[bool]$WindowsAuthenticationBool= ($WindowsAuthentication -eq 'true')
	[int]$TimeoutInt = [System.Convert]::ToInt32($Timeout)

	$secpasswd = ConvertTo-SecureString $adminPassword -AsPlainText -Force
	$credentials = New-Object System.Management.Automation.PSCredential ($adminUserName, $secpasswd)

	
	RunRemoteSqlScript -SQLServer $SQLServer -WindowsAuthentication $WindowsAuthenticationBool -Login $Login -Password $Password -ScriptFile $ScriptFile -Database $Database -Timeout $TimeoutInt -credential $credentials -serverHost $ServerHost
}