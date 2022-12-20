

function Start-RemoteInstallTopShelfService
{
    param (
		[string]$serverHost,
		[string]$topshelfExe,
        [string]$specialUser,
		[string]$serviceUsername,
		[string]$servicePassword,
		[string]$instanceName,
		[string]$uninstallFirst,
		$credential
    )
	$hostName = ([System.Net.Dns]::GetHostByName($serverHost)).HostName
	
	Write-Host "Installing TopShelf service $topshelfExe on $hostName"

	#$servicePassword = $servicePassword.Replace('`', '``').Replace('"', '`"').Replace('$', '`$').Replace('&', '`&')
	#$instanceName = $instanceName.Replace('`', '``').Replace('"', '`"').Replace('$', '`$').Replace('&', '`&')

	$additionalArguments = ""
	if (-Not [string]::IsNullOrWhiteSpace($serviceUsername))
	{
		$additionalArguments += " -username:$serviceUsername -password:$servicePassword"

	}
	else
	{
		$additionalArguments += " --$specialUser"
	}

	if (-Not [string]::IsNullOrWhiteSpace($instanceName))
	{
		$additionalArguments += " -instance:$instanceName"
	}

	$script = [Scriptblock]::Create($cmd)  
	$result = Invoke-Command -ComputerName $hostName -Credential $credential  -ScriptBlock {
		param (
				$topShelfExe,
				$uninstallFirst,
				$additionalArguments
			)
		$cmdOutput = ""
		if(Test-Path $topShelfExe){
			Write-Host "File Found"
			if ($uninstallFirst -eq "true")
			{
				Write-Host "Uninstalling Service"
				$action = "uninstall $additionalArguments"
				$cmdOut = & $topShelfExe $action.Split(" ") 2>&1
				Write-Host $cmdOut
			}
			Write-Host "Installing Service"
			$action = "install $additionalArguments"
			$cmdOuput = & $topShelfExe $action.Split(" ") 2>&1
			Write-Host $cmdOuput
		}else{
			Write-Host "File Not Found"
		}
	} -ArgumentList $topShelfExe, $uninstallFirst, $additionalArguments

	Write-Host $result

}


function RunInstallTopShelfService{
param (
    [string] $topshelfExe,
    [string] $machineList,
    [string] $adminUserName,
    [string] $adminPassword,
	[string] $specialUser,
	[string] $serviceUsername,
    [string] $servicePassword,
    [string] $instanceName,
	[string] $uninstallFirst
    )

	$secpasswd = ConvertTo-SecureString $adminPassword -AsPlainText -Force
	$credential = New-Object System.Management.Automation.PSCredential ($adminUserName, $secpasswd)

	$machines = @()
	$machineList.split(',', [System.StringSplitOptions]::RemoveEmptyEntries) |`
    Foreach { if( ![string]::IsNullOrWhiteSpace($_) -and ![string]::Equals('\n', $_)) {$machines += $_}}

	foreach ($machine in $machines){
		Start-RemoteInstallTopShelfService -serverHost $machine -credential $credential -topshelfExe $topshelfExe -specialUser $specialUser -serviceUsername $serviceUsername -servicePassword $servicePassword -instanceName $instanceName -uninstallFirst $uninstallFirst
	}
}
