# Function declarations.
########################

function LogAndWrite() {
    param([string] $LogOutput)
    Write-Host $LogOutput
    Out-File -InputObject $LogOutput -FilePath $logFilePath -Append
}

function UserWantsToContinue() {
    $continue = Read-Host "Do you want to continue? (y/n)"

    While (($continue -ne 'n') -and ($continue -ne 'N') -and ($continue -ne 'y') -and ($continue -ne 'Y'))
    {
        Write-Host "Invalid input.  Please enter y or n."
        $continue = Read-Host "Do you want to continue? (y/n)"
    }

    If (($continue -eq 'n') -or ($continue -eq 'N'))
    {
        Return $false
    }
    Else 
    {
        Return $true
    }
}

function RecycleAppPool() {
    [CmdletBinding(SupportsShouldProcess=$True)]
    param([string] $ServerName, [string] $AppPoolName)

    $hostName = ([System.Net.Dns]::GetHostByName($ServerName)).HostName

    $shouldProcessMessage = "Invoke-Command on computer $hostName ($ServerName) Restart-WebAppPool -Name $AppPoolName"

    If ($PSCmdlet.ShouldProcess($shouldProcessMessage)) {
            Invoke-Command -ComputerName $hostName -ScriptBlock {
                param($appPool)
                Add-PSSnapin WebAdministration
                Restart-WebAppPool -Name $appPool
            } -ArgumentList $AppPoolName
    }
}

function DeleteContentsOfFolderRemotely() {
    [CmdletBinding(SupportsShouldProcess=$True)]

    param([string] $ServerName, [string] $Path)
    $hostName = ([System.Net.Dns]::GetHostByName($serverName)).HostName

    $contentsOfFolderPath = $path + "\*"

    $shouldProcessMessage = "Invoke-Command on computer $hostName ($ServerName): Remove-Item -Path $contentsOfFolderPath -Recurse"

    If ($PSCmdlet.ShouldProcess($shouldProcessMessage)) {
        Invoke-Command -ComputerName $hostName -ScriptBlock {
            param($directory)
            Remove-Item -Path $directory -Recurse
        } -ArgumentList $contentsOfFolderPath
    }
}

function CopyContentsOfFolderOnRemoteHost() {
    [CmdletBinding(SupportsShouldProcess=$True)]

    param([string] $ServerName, [string] $Source, [string] $Destination)

    $hostName = ([System.Net.Dns]::GetHostByName($serverName)).HostName
    $sourceContent = $Source + '\*'

    $shouldProcessMessage = "Invoke-Command on computer $hostName ($ServerName): Copy-Item -Path $sourceContent -Destination $destination -Recurse"

    If ($PSCmdlet.ShouldProcess($shouldProcessMessage)) 
    {
        Invoke-Command -ComputerName $hostName -ScriptBlock {
            param($sourceDirectory, $destDirectory)
            # We must create the dest folder, if it does not exist, because if the dest folder does NOT exist, 
			# the copy command will inappropriately copy each source file to a *FILE* with the dest folder name.
            If (-not (Test-Path $destDirectory)){
                New-Item -ItemType Directory -Path $destDirectory
            }
            Copy-Item -Path $sourceDirectory -Destination $destDirectory -Recurse
        
        } -ArgumentList $sourceContent, $destination
    }
}

function DisableScheduledTasksRemotely() {
    [cmdletBinding(SupportsShouldProcess=$True)]

    param([string] $ServerName, [string] $ScheduledTaskNameWildCard)

    $hostName = ([System.Net.Dns]::GetHostByName($serverName)).HostName

    $shouldProcessMessage = "Invoke-Command on computer $hostName ($ServerName): Get-ScheduledTask | Where-Object {`$_.TaskName -like '$ScheduledTaskNameWildCard'} | Disable-ScheduledTask"

    If ($PSCmdlet.ShouldProcess($shouldProcessMessage))
    {
        Invoke-Command -ComputerName $hostName -ScriptBlock {
            param($task)
            Get-ScheduledTask | ? {$_.TaskName -like $task} | Disable-ScheduledTask
        } -ArgumentList $ScheduledTaskNameWildCard
    }
}

function EnableScheduledTasksRemotely() {
    [cmdletBinding(SupportsShouldProcess=$True)]

    param([string] $ServerName, [string] $ScheduledTaskNameWildCard)

    $hostName = ([System.Net.Dns]::GetHostByName($serverName)).HostName

    $shouldProcessMessage = "Invoke-Command on computer $hostName ($ServerName): Get-ScheduledTask | Where-Object {`$_.TaskName -like '$ScheduledTaskNameWildCard'} | Enable-ScheduledTask"

    If ($PSCmdlet.ShouldProcess($shouldProcessMessage))
    {
        Invoke-Command -ComputerName $hostName -ScriptBlock {
            param($task)
            Get-ScheduledTask | ? {$_.TaskName -like $task} | Enable-ScheduledTask
        } -ArgumentList $ScheduledTaskNameWildCard
    }
}

function StartServiceRemotely() {
    [cmdletBinding(SupportsShouldProcess=$True)]

    param($ServerName, $ServiceDisplayName)

    $hostName = ([System.Net.Dns]::GetHostByName($serverName)).HostName

    $shouldProcessMessage = "Invoke-Command on computer $hostName ($ServerName): Get-Service -DisplayName $ServiceDisplayName | Where-Object {`$_.Status -ne 'Running'} | Start-Service"

    If ($PSCmdlet.ShouldProcess($shouldProcessMessage))
    {
        Invoke-Command -ComputerName $hostName -ScriptBlock {
            param($serviceName)

            Get-Service -DisplayName $serviceName | ? {$_.Status -ne "Running"} | Start-Service
        } -ArgumentList $ServiceDisplayName
    }
}

function StopServiceRemotely() {
    [cmdletBinding(SupportsShouldProcess=$True)]

    param($ServerName, $ServiceDisplayName)

    $hostName = ([System.Net.Dns]::GetHostByName($serverName)).HostName

    $shouldProcessMessage = "Invoke-Command on computer $hostName ($ServerName): Get-Service -DisplayName $ServiceDisplayName | Where-Object {`$_.Status -eq 'Running'} | Stop-Service"

    If ($PSCmdlet.ShouldProcess($shouldProcessMessage))
    {
        Invoke-Command -ComputerName $hostName -ScriptBlock {
            param($serviceName)

            Get-Service -DisplayName $serviceName | ? {$_.Status -eq "Running"} | Stop-Service
        } -ArgumentList $ServiceDisplayName
    }
}

function RemoveService() {
	[cmdletBinding(SupportsShouldProcess=$True)]

	param($ServiceName, $RemoteSvrName)
	
	$shouldProcessMessage = "ServiceName=[$ServiceName], RemoteSvrName=[$RemoteSvrName]"

	If ($PSCmdlet.ShouldProcess($shouldProcessMessage))
    {
        
        if ($RemoteSvrName -ne $null -and $RemoteSvrName.length -gt 0){
            $dnsHostname = [System.Net.Dns]::GetHostByName($RemoteSvrName).HostName
            "[[RemoveService.Log]] Locating service '" + $ServiceName + "' on remote machine '" + $RemoteSvrName + "' (DNS=" + $dnsHostname + ")..."
            Invoke-Command -ComputerName $dnsHostname -ScriptBlock {
                param($r_SvcName)

                $service = Get-Service -Name $r_SvcName -ErrorAction SilentlyContinue
                if ($service) {
                    $serviceName = $service.Name
                    $serviceObject = Get-WmiObject -Class Win32_Service -Filter "Name='$serviceName'"
                    "[[RemoveService.Log]] Deleting service..."
                    if ($serviceObject -ne $null){
                        $serviceObject.delete()
                    }
                } else {
                    "[[RemoveService.Log]] *Service-Not-Found"
                }

            } -ArgumentList $ServiceName

        } else {
            "[[RemoveService.Log]] Locating service '" + $ServiceName + "'..."
            $service = Get-Service -Name $ServiceName -ErrorAction SilentlyContinue
            if ($service) {
                $serviceName = $service.Name
                $serviceObject = Get-WmiObject -Class Win32_Service -Filter "Name='$serviceName'"
                "[[RemoveService.Log]] Deleting service..."
                if ($serviceObject -ne $null){
                    $serviceObject.delete()
                }
            } else {
                "[[RemoveService.Log]] *Service-Not-Found"
            }
        }
    }
}

function NewService() {	
	[cmdletBinding(SupportsShouldProcess=$True)]

	param($NewSvcName, $NewSvcDisplayName, $NewSvcDesc, $NewSvcExePath, $NewSvcUser, $IsNewSvcEnabled, $IsDeleteExisting, $RemoteSvrName)
	
	$shouldProcessMessage = "ServiceName=[$NewSvcName], NewSvcDisplayName=[$NewSvcDisplayName], NewSvcDesc=[$NewSvcDesc], NewSvcExePath=[$NewSvcExePath], NewSvcUser=[$NewSvcUser], IsNewSvcEnabled=[$IsNewSvcEnabled], IsDeleteExisting=[$IsDeleteExisting], RemoteSvrName=[$RemoteSvrName]"

    If ($PSCmdlet.ShouldProcess($shouldProcessMessage))
    {
        # Create on remove server, if a server name was passed.
        if ($RemoteSvrName -ne $null -and $RemoteSvrName.length -gt 0){

            $dnsHostname = [System.Net.Dns]::GetHostByName($RemoteSvrName).HostName
            "[[NewService.Log]] Creating service '" + $NewSvcName + "' on remote machine '" + $RemoteSvrName + "' (DNS=" + $dnsHostname + ")..."
            # Delete remote service first, if specified.
            if ($IsDeleteExisting){
                RemoveService -ServiceName $NewSvcName -RemoteSvrName $RemoteSvrName
            }

            Invoke-Command -ComputerName $dnsHostname -ScriptBlock {
                param($r_NewSvcName, $r_newSvcDisplayName, $r_newSvcDesc, $r_newSvcExePath, $r_newSvcUser, $r_isNewSvcEnabled)

                if ($r_isNewSvcEnabled){
		            New-Service -Name $r_NewSvcName -DisplayName $r_newSvcDisplayName -Description $r_newSvcDesc -BinaryPathName $r_newSvcExePath -Credential $r_newSvcUser -StartupType Automatic -Verbose
                } else {
		            New-Service -Name $r_NewSvcName -DisplayName $r_newSvcDisplayName -Description $r_newSvcDesc -BinaryPathName $r_newSvcExePath -Credential $r_newSvcUser -StartupType Disabled -Verbose
                }
            } -ArgumentList $NewSvcName, $newSvcDisplayName, $newSvcDesc, $newSvcExePath, $newSvcUser, $isNewSvcEnabled

        } else {
            # Create on local machine.
            "[[NewService.Log]] Creating service '" + $NewSvcName + "'..."
            # Delete local service first, if specified.
            if ($IsDeleteExisting){
                RemoveService -ServiceName $NewSvcName
            }
            if ($isNewSvcEnabled){
		        New-Service -Name $NewSvcName -DisplayName $newSvcDisplayName -Description $newSvcDesc -BinaryPathName $newSvcExePath -Credential $newSvcUser -StartupType Automatic -Verbose
            } else {
		        New-Service -Name $NewSvcName -DisplayName $newSvcDisplayName -Description $newSvcDesc -BinaryPathName $newSvcExePath -Credential $newSvcUser -StartupType Disabled -Verbose
            }
        }

    }
}

