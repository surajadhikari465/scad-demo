

function Start-RemoteSQLAgentJob
{
    param (
        [string]$SQLServer ,
		[string]$serverHost,
		[bool]$WindowsAuthentication ,
		[string]$Login ,
		[string]$Password ,
        [string]$JobName,
		$credential
    )



	$hostName = ([System.Net.Dns]::GetHostByName($serverHost)).HostName
	Write-Host "Invoking command on to $hostName to run sql job $JobName"
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
			$JobObj.Refresh()

			# If the job is not currently executing start it
			if ($JobObj.CurrentRunStatus -ne "Executing") {
				$JobObj.Start()
			}

			# Wait until the job completes. Check every second.
			do {
				Start-Sleep -Seconds 1
				# You have to run the refresh method to reread the status
				$JobObj.Refresh()
			} While ($JobObj.CurrentRunStatus -eq "Executing")

			# Get the run duration by adding all of the step durations
			$RunDuration = 0
			foreach($JobStep in $JobObj.JobSteps)     {
				$RunDuration += $JobStep.LastRunDuration
			}
			$JobObj.Refresh();
			
			return $JobObj.LastRunOutcome.ToString()
		} -ArgumentList $SQLServer, $WindowsAuthentication, $Login, $Password, $JobName
		Write-Host $Result
		if($Result -eq "Succeeded"){
			Write-Host "$JobName on $SQLServer ran successfully"
		}else{
			Throw "$JobName on $SQLServer failed"
		}
		Write-Host $errmsg
}


function RunSqlServerJob{
param (
        [string]$SQLServer ,
		[string]$WindowsAuthentication ,
		[string]$Login ,
		[string]$Password ,
        [string]$JobName,
		[string]$adminUserName,
		[string]$adminPassword,
		[string]$serverHost
    )
		[bool]$WindowsAuthenticationBool= ($WindowsAuthentication -eq 'true')
		$secpasswd = ConvertTo-SecureString $adminPassword -AsPlainText -Force
		$credentials = New-Object System.Management.Automation.PSCredential ($adminUserName, $secpasswd)

		Start-RemoteSQLAgentJob -SQLServer $SQLServer -WindowsAuthentication $WindowsAuthenticationBool -Login $Login -Password $Login -JobName $JobName -serverHost $serverHost -credential $credentials
}
