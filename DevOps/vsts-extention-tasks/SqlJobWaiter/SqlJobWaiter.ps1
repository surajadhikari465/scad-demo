

function Start-RemoteSQLAgentJobWaiter
{
    param (
        [string]$SQLServer ,
		[string]$serverHost,
		[bool]$WindowsAuthentication ,
		[string]$Login ,
		[string]$Password ,
        [string]$JobNames,
		$credential
    )
	$jobList = @()
	$JobNames.split(',', [System.StringSplitOptions]::RemoveEmptyEntries) |`
    Foreach { if( ![string]::IsNullOrWhiteSpace($_) -and ![string]::Equals('\n', $_)) {$jobList += $_}}

	$hostName = ([System.Net.Dns]::GetHostByName($serverHost)).HostName
	foreach($jobName in $jobList){
		Write-Host "Invoking command on to $hostName to wait on sql job $JobName"
		$result = Invoke-Command -ComputerName $hostName -Credential $credential  -ErrorVariable errmsg 2>$null -ScriptBlock {
			param (
				[string]$SQLServer ,
				[bool]$WindowsAuthentication ,
				[string]$Login ,
				[string]$Password ,
				[string]$JobName
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

			$ServerObj = New-Object Microsoft.SqlServer.Management.Smo.Server($SQLServer)
			if(!$WindowsAuthentication){
				 $ServerObj.ConnectionContext.LoginSecure = $FALSE
				 $ServerObj.ConnectionContext.Login = $Login
				 $ServerObj.ConnectionContext.Password = $Password
			}
			$ServerObj.ConnectionContext.Connect()
			$JobObj = $ServerObj.JobServer.Jobs | Where-Object {$_.Name -eq $JobName}
			if($JobObj){
				Write-host "$JobName Found"
				$JobObj.Refresh()
				if($JobObj.CurrentRunStatus -eq "Executing"){
					Write-host "$JobName is running"
					# Wait until the job completes. Check every second.
					do {
						Start-Sleep -Seconds 1
						# You have to run the refresh method to reread the status
						$JobObj.Refresh()
					} While ($JobObj.CurrentRunStatus -eq "Executing")
					Write-Host "$JobName finished running"
				}else{
					Write-Host "$JobName was not running"
				}
			}else{
				Write-Host "$JobName not found"
			}
		} -ArgumentList $SQLServer, $WindowsAuthentication, $Login, $Password, $jobName
		Write-Host $errmsg;
	}
}


function WaitForSqlServerJobs{
param (
        [string]$SQLServer ,
		[string]$WindowsAuthentication ,
		[string]$Login ,
		[string]$Password ,
        [string]$JobNames,
		[string]$adminUserName,
		[string]$adminPassword,
		[string]$serverHost
    )
		[bool]$WindowsAuthenticationBool= ($WindowsAuthentication -eq 'true')
		$secpasswd = ConvertTo-SecureString $adminPassword -AsPlainText -Force
		$credentials = New-Object System.Management.Automation.PSCredential ($adminUserName, $secpasswd)

		Start-RemoteSQLAgentJobWaiter -SQLServer $SQLServer -WindowsAuthentication $WindowsAuthenticationBool -Login $Login -Password $Login -JobNames $JobNames -serverHost $serverHost -credential $credentials
}
