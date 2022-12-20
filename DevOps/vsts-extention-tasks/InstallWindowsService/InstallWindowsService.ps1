function Start-RemoteInstallWindowsService {

	param (
		[string]$serviceName, 
		[string]$displayName,
		[string]$binaryPath, 
		[string]$description, 
		[string]$serviceUsername, 
		[string]$servicePassword, 
		[string]$startUpType, 
		[string]$serverName, 
		$credential
	)

	$hostName = ([System.Net.Dns]::GetHostByName($serverName)).HostName
	Write-Host "Invoking command on to $hostName to install service $serviceName"
    $result = Invoke-Command -ComputerName $hostName -Credential $credential  -ErrorVariable errmsg 2>$null -ScriptBlock {
		param (
		[string]$serviceName,
		[string]$displayName, 
		[string]$binaryPath, 
		[string]$description, 
		[string]$serviceUsername, 
		[string]$servicePassword, 
		[string]$startUpType
		)
        Write-Host "Trying to create service: $serviceName"

        #Check Parameters
        if ((Test-Path $binaryPath)-eq $false)
        {
            Write-Host "BinaryPath to service not found: $binaryPath"
            Write-Host "Service was NOT installed."
            return
        }

        if (("Automatic", "Manual", "Disabled") -notcontains $startUpType)
        {
            Write-Host "Value for startUpType parameter should be (Automatic or Manual or Disabled) and it was $startUpType"
            Write-Host "Service was NOT installed."
            return
        }

        # Verify if the service already exists, and if yes remove it first
        if (Get-Service $serviceName -ErrorAction SilentlyContinue)
        {
			Stop-Service $serviceName
            # using WMI to remove Windows service because PowerShell does not have CmdLet for this
            $serviceToRemove = Get-WmiObject -Class Win32_Service -Filter "name='$serviceName'"

            $serviceToRemove.delete()
            Write-Host "Service removed: $serviceName"
        }

        # if password is empty, create a dummy one to allow have credentias for system accounts: 
        #NT AUTHORITY\LOCAL SERVICE
        #NT AUTHORITY\NETWORK SERVICE
        if ($servicePassword -eq "") 
        {
            $servicePassword = "dummy"
        }
        $secpasswd = ConvertTo-SecureString $servicePassword -AsPlainText -Force
        $mycreds = New-Object System.Management.Automation.PSCredential ($serviceUsername, $secpasswd)

	    if ($description -eq "") 
        {
            $description = $displayName
        }

        # Creating Windows Service using all provided parameters
        Write-Host "Installing service: $serviceName"
		
        New-Service -name $serviceName -binaryPathName $binaryPath -Description $description -displayName $displayName -startupType $startUpType -credential $mycreds

        Write-Host "Installation completed: $serviceName"

		# skipping the startup perminately because we don't want to start services at the install stage so we can coordinate when they do start.
  #      # Trying to start new service
		#if (("Disabled") -notcontains $startUpType)
  #      {
		#	Write-Host "Trying to start new service: $serviceName"
		#	$serviceToStart = Get-WmiObject -Class Win32_Service -Filter "name='$serviceName'"
		#	$serviceToStart.startservice()
		#	Write-Host "Service started: $serviceName"
		#}

	} -ArgumentList $serviceName, $displayName, $binaryPath, $description, $serviceUsername, $servicePassword, $startUpType
	Write-Host $result
	Write-Host $errmsg
}


function RunInstallServices{
param (
		[string]$machineList,
		[string]$serviceName, 
		[string]$displayName,
		[string]$binaryPath, 
		[string]$description, 
		[string]$serviceUsername, 
		[string]$servicePassword, 
		[string]$startUpType, 
		[string]$adminUserName,
		[string]$adminPassword
    )


	$secpasswd = ConvertTo-SecureString $adminPassword -AsPlainText -Force
	$credential = New-Object System.Management.Automation.PSCredential ($adminUserName, $secpasswd)

	$machines = @()
	$machineList.split(',', [System.StringSplitOptions]::RemoveEmptyEntries) |`
    Foreach { if( ![string]::IsNullOrWhiteSpace($_) -and ![string]::Equals('\n', $_)) {$machines += $_}}

	foreach ($machine in $machines){
		Start-RemoteInstallWindowsService -serviceName $serviceName -displayName $displayName -binaryPath $binaryPath -description $description -serviceUsername $serviceUsername -servicePassword $servicePassword -startUpType $startUpType -credential $credential -serverName $machine
	}
}