

function Start-SetAppSettings
{
    param (
		[string]$serverHost,
		[string]$configName,
        [string]$replacementValues,
		[string]$installPath,
		$iteration,
		$credential
    )

	

	$hostName = ([System.Net.Dns]::GetHostByName($serverHost)).HostName
	Write-Host "Invoking command on to $hostName to replace $configName with $transformToUse"
	Write-Host $iteration;
    $result = Invoke-Command -ComputerName $hostName -Credential $credential  -ErrorVariable errmsg 2>$null -ScriptBlock {
			param (
				[string]$configName,
				[string]$replacementValues,
				[string]$installPath,
				$iteration
			)

			$transformation = @()
			Write-Host $replacementValues
			$replacementValues.split("[`r`n]") |`
				Foreach { if( ![string]::IsNullOrWhiteSpace($_)) {$transformation += $_}}
			
			$appConfigFullPath = Join-Path $installPath $configName
			if(Test-Path $appConfigFullPath){
				Write-Host "Applying Transformations $appConfigFullPath"
				$appConfig = New-Object XML
				# load the config file as an xml object
				$appConfig.Load($appConfigFullPath)
				foreach ($transform in $transformation){
					$keypair = @()
					$transform.split('|', [System.StringSplitOptions]::RemoveEmptyEntries) |`
					Foreach { if( ![string]::IsNullOrWhiteSpace($_) -and ![string]::Equals('\n', $_)) {$keypair += $_}}
					$key = $keypair[0]
					$value = $keypair[1]
					if($value -like "*~iteration~*"){
						$value = $value.replace('~iteration~',$iteration.ToString())
					}
					$node = $appConfig.SelectSingleNode('configuration/appSettings/add[@key="' + $key + '"]')
					$node.Attributes['value'].Value = $value
				}
				# save the updated config file
				$appConfig.Save($appConfigFullPath)
			}
		} -ArgumentList $configName, $replacementValues, $installPath, $iteration
		Write-Host $result
		Write-Host $errmsg;
}


function RunSetAppSettings{
param (
		[string]$configName,
        [string]$replacementValues,
		[string]$installPath,
		[string]$adminUserName,
		[string]$adminPassword,
		[string]$machineList
    )


	$secpasswd = ConvertTo-SecureString $adminPassword -AsPlainText -Force
	$credential = New-Object System.Management.Automation.PSCredential ($adminUserName, $secpasswd)

	$machines = @()
	$machineList.split(',', [System.StringSplitOptions]::RemoveEmptyEntries) |`
    Foreach { if( ![string]::IsNullOrWhiteSpace($_) -and ![string]::Equals('\n', $_)) {$machines += $_}}
	$i = 0;
	foreach ($machine in $machines){
		$i++
		Start-SetAppSettings -serverHost $machine -configName $configName -replacementValues $replacementValues -installPath $installPath -credential $credential -iteration $i
	}
}
