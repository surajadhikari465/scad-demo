

function Start-TokenReplacement
{
    param (
		[string]$serverHost,
        [string]$replacementValues,
		$files,
		$iteration,
		$credential
    )

	

	$hostName = ([System.Net.Dns]::GetHostByName($serverHost)).HostName
	Write-Host "Invoking command on to $hostName to replace $configName with $transformToUse"
	Write-Host $iteration;
    $result = Invoke-Command -ComputerName $hostName -Credential $credential  -ErrorVariable errmsg 2>$null -ScriptBlock {
			param (
				$files,
				[string]$replacementValues,
				$iteration
			)

			$transformation = @()
			Write-Host $replacementValues
			$replacementValues.split("[`r`n]") |`
				Foreach { if( ![string]::IsNullOrWhiteSpace($_)) {$transformation += $_}}
			foreach ($file in $files){
				
				if(Test-Path $file){
					Write-Host "Replacing Tokens in $file"
					$fileContent = (Get-Content $file)
					foreach ($transform in $transformation){
						$keypair = @()
						$transform.split('|', [System.StringSplitOptions]::RemoveEmptyEntries) |`
						Foreach { if( ![string]::IsNullOrWhiteSpace($_) -and ![string]::Equals('\n', $_)) {$keypair += $_}}
						$key = '${' + $keypair[0] + '}'
						$value = $keypair[1]
						$fileContent = $fileContent.replace($key,$value)
					}
					# save the updated config file
					Set-Content -Path $file -Value $fileContent
				}else{
					Write-Host "$file not found"
				}
			}
		} -ArgumentList $files, $replacementValues, $iteration
		Write-Host $result
		Write-Host $errmsg;
}


function RunTokenReplacement{
param (
        [string]$replacementValues,
		[string]$fileList,
		[string]$adminUserName,
		[string]$adminPassword,
		[string]$machineList
    )


	$secpasswd = ConvertTo-SecureString $adminPassword -AsPlainText -Force
	$credential = New-Object System.Management.Automation.PSCredential ($adminUserName, $secpasswd)

	$machines = @()
	$machineList.split(',', [System.StringSplitOptions]::RemoveEmptyEntries) |`
    Foreach { if( ![string]::IsNullOrWhiteSpace($_) -and ![string]::Equals('\n', $_)) {$machines += $_}}
	
	$files = @()
	$fileList.split("[`r`n]", [System.StringSplitOptions]::RemoveEmptyEntries) |`
				Foreach { if( ![string]::IsNullOrWhiteSpace($_) -and ![string]::Equals('\n', $_)) {$files += $_.Replace("`r","").Replace("`n","")}}
	
	$i = 0;
	foreach ($machine in $machines){
		$i++
		Start-TokenReplacement -serverHost $machine -replacementValues $replacementValues -files $files -credential $credential -iteration $i
	}
}
