

function Start-RemoteUninstallTopShelfService
{
    param (
		[string]$serverHost,
		[string]$topshelfExe,
		$credential
    )
	$hostName = ([System.Net.Dns]::GetHostByName($serverHost)).HostName
	
	Write-Output "Uninstalling TopShelf service $topshelfExe on $hostName"

	$cmd = ""
	$cmd += "& '$topShelfExe' uninstall $additionalArguments`n"

	Write-Output "CMD: $cmd"
	$script = [Scriptblock]::Create($cmd)  
	$result = Invoke-Command -ComputerName $hostName -Credential $credential  -ErrorVariable errmsg 2>$null -ScriptBlock $script

	Write-Output $result
	Write-Output $errmsg	
	

}


function RunUninstallTopShelfService{
param (
    [string] $topshelfExe,
    [string] $machineList,
    [string] $adminUserName,
    [string] $adminPassword
    )

	$secpasswd = ConvertTo-SecureString $adminPassword -AsPlainText -Force
	$credential = New-Object System.Management.Automation.PSCredential ($adminUserName, $secpasswd)

	$machines = @()
	$machineList.split(',', [System.StringSplitOptions]::RemoveEmptyEntries) |`
    Foreach { if( ![string]::IsNullOrWhiteSpace($_) -and ![string]::Equals('\n', $_)) {$machines += $_}}

	foreach ($machine in $machines){
		Start-RemoteUninstallTopShelfService -serverHost $machine -credential $credential -topshelfExe $topshelfExe
	}
}
