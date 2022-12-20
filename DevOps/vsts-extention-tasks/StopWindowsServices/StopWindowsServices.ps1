function StopServiceRemotely() {

    param($ServerName, $serviceName, $credential)

    $hostName = ([System.Net.Dns]::GetHostByName($serverName)).HostName
	Write-Host "Invoking command on to $hostName to stop $serviceName"
    $result = Invoke-Command -ComputerName $hostName -Credential $credential  -ErrorVariable errmsg 2>$null -ScriptBlock {
		param($service) 
		try{
		Get-Service -DisplayName $service | ? {$_.Status -eq "Running"} | Stop-Service
		}
		catch
		{
			return $_
		}
	} -ArgumentList $serviceName 
	Write-Host $result;
}

function StopServices{
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
			Write-Host "Stopping $service on machine $machine if available"
			StopServiceRemotely -ServerName $machine -serviceName $service -credential $credentials

		}
	}


}

function RunStopWindowsServices{
	param (
		[string]$machineList,
		[string]$serviceList,
		[string]$adminUserName,
		[string]$adminPassword
		)
		$secpasswd = ConvertTo-SecureString $adminPassword -AsPlainText -Force
		$credentials = New-Object System.Management.Automation.PSCredential ($adminUserName, $secpasswd)
		StopServices -machineList $machineList -serviceList $serviceList -credentials $credentials
}