

function Start-RemoteConfigPicking
{
    param (
		[string]$serverHost,
		[string]$configName,
        [string]$transformToUse,
		[string]$installPath,
		$credential
    )

	$hostName = ([System.Net.Dns]::GetHostByName($serverHost)).HostName
	Write-Host "Invoking command on to $hostName to replace $configName with $transformToUse"
    $result = Invoke-Command -ComputerName $hostName -Credential $credential  -ErrorVariable errmsg 2>$null -ScriptBlock {
			param (
				[string]$configName,
				[string]$transformToUse,
				[string]$installPath
			)


			$appConfigFullPath = Join-Path $installPath $configName
			if(Test-Path $appConfigFullPath){
				Write-Host "deleting $appConfigFullPath"
				Remove-Item $appConfigFullPath
			}
			$tranformFullPath = Join-Path $installPath $transformToUse
			if(Test-Path $tranformFullPath){
				Write-Host "Renaming $tranformFullPath to $configName"
				Rename-Item $tranformFullPath $configName
			}
			
			$deleteOtherTransforms = join-path $installPath *.transformed

			Write-Host "Deleting other remaining configs ($deleteOtherTransforms)"

			Remove-Item $deleteOtherTransforms
		} -ArgumentList $configName, $transformToUse, $installPath
		Write-Host $result
		Write-Host $errmsg;
}


function RunPickConfigs{
param (
		[string]$configName,
        [string]$transform,
		[string]$installPath,
		[string]$adminUserName,
		[string]$adminPassword,
		[string]$machineList
    )

	if($configName.StartsWith("web","CurrentCultureIgnoreCase")){
		  $transformToUse = "web.$transform.config.transformed"
	}else{
		  $transformToUse = "app.$transform.config.transformed"
	}
	$secpasswd = ConvertTo-SecureString $adminPassword -AsPlainText -Force
	$credential = New-Object System.Management.Automation.PSCredential ($adminUserName, $secpasswd)

	$machines = @()
	$machineList.split(',', [System.StringSplitOptions]::RemoveEmptyEntries) |`
    Foreach { if( ![string]::IsNullOrWhiteSpace($_) -and ![string]::Equals('\n', $_)) {$machines += $_}}

	foreach ($machine in $machines){
		Start-RemoteConfigPicking -serverHost $machine -configName $configName -transformToUse $transformToUse -installPath $installPath -credential $credential
	}
}
