function StartServiceRemotely() {

    param($ServerName, $serviceName, $credential)

    $hostName = ([System.Net.Dns]::GetHostByName($serverName)).HostName
	Write-Host "Invoking command on to $hostName to enable Task $serviceName"
    $result = Invoke-Command -ComputerName $hostName -Credential $credential  -ErrorVariable errmsg 2>$null -ScriptBlock {
		param($service) 
		try{
		Get-Service -DisplayName $service | ? {$_.Status -ne "Running"} | Start-Service
		}
		catch
		{
			return $_
		}
	} -ArgumentList $serviceName 
	Write-Host $result;
}

function StartServices{
	param(
		[string]$machineList,
		[string]$serviceList,
		$credentials
	)
	$machines = @()
	$services = @()

	$machineList.split(',', [System.StringSplitOptions]::RemoveEmptyEntries) |`
    Foreach { if( ![string]::IsNullOrWhiteSpace($_) -and ![string]::Equals('\n', $_)) {$machines += $_}}

	$serviceList.split(',', [System.StringSplitOptions]::RemoveEmptyEntries) |`
    Foreach { if( ![string]::IsNullOrWhiteSpace($_) -and ![string]::Equals('\n', $_)) {$services += $_}}

	foreach ($machine in $machines){
		foreach($service in $services){
			Write-Host "Starting $service on machine $machine if available"
			StartServiceRemotely -ServerName $machine -serviceName $service -credential $credentials

		}
	}


}


function RunStartWindowsServices{
	param (
		[string]$machineList,
		[string]$serviceList,
		[string]$adminUserName,
		[string]$adminPassword
		)
		$secpasswd = ConvertTo-SecureString $adminPassword -AsPlainText -Force
		$credentials = New-Object System.Management.Automation.PSCredential ($adminUserName, $secpasswd)
		StartServices -machineList $machineList -serviceList $serviceList -credentials $credentials
}
