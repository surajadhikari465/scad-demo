
function Start-RemoteFileDelete
{
    param (
		[string]$serverHost,
		[string]$fileToDelete,
		[string]$isDirectory,
		$credential
    )

	$hostName = ([System.Net.Dns]::GetHostByName($serverHost)).HostName
	Write-Host "Invoking command on to $hostName to delete $fileToDelete"
    $result = Invoke-Command -ComputerName $hostName -Credential $credential  -ErrorVariable errmsg 2>$null -ScriptBlock {
			param (
				[string]$fileToDelete
			)
			Write-Host "Deleting $fileToDelete"
			if($isDirectory -eq "false"){
				Write-Host "Deleting $fileToDelete"
				Remove-Item $fileToDelete
			}else{
				$folderToDelete = "$fileToDelete\*"
				Write-Host "Deleting $folderToDelete contents"
				Remove-Item $folderToDelete -Recurse
				Write-Host "Deleting folder $fileToDelete"
				Remove-Item $fileToDelete
			}
		} -ArgumentList $fileToDelete,$isDirectory
		Write-Host $result
		Write-Host $errmsg;
}

function DeleteFile{
	param (
		[string]$fileToDelete,
		[string]$isDirectory,
		[string]$adminUserName,
		[string]$adminPassword,
		[string]$machineList
    )

	$secpasswd = ConvertTo-SecureString $adminPassword -AsPlainText -Force
	$credential = New-Object System.Management.Automation.PSCredential ($adminUserName, $secpasswd)

	$machines = @()
	$machineList.split(',', [System.StringSplitOptions]::RemoveEmptyEntries) |`
    Foreach { if( ![string]::IsNullOrWhiteSpace($_) -and ![string]::Equals('\n', $_)) {$machines += $_}}

	foreach ($machine in $machines){
		Start-RemoteFileDelete -serverHost $machine -fileToDelete $fileToDelete -credential $credential -isDirectory $isDirectory
	}
}
