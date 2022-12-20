function StopTaskRemotely() {

    param($ServerName, $task, $credential)

    $hostName = ([System.Net.Dns]::GetHostByName($serverName)).HostName
	Write-Host "Invoking command on to $hostName to disable Task $task"
    $result = Invoke-Command -ComputerName $hostName -Credential $credential  -ErrorVariable errmsg 2>$null -ScriptBlock {
		param($taskQuery) 
		try{
			Get-ScheduledTask | ? {$_.TaskName -like $taskQuery} | Stop-ScheduledTask
			Get-ScheduledTask | ? {$_.TaskName -like $taskQuery} | Disable-ScheduledTask
		}
		catch
		{
			return $_
		}
	} -ArgumentList $task 
	Write-Host $errmsg;
}

function StopScheduleTasks{
	param(
		[string]$machineList,
		[string]$taskList,
		$credentials
	)
	$machines = @()
	$tasks = @()

	$machineList.split(',', [System.StringSplitOptions]::RemoveEmptyEntries) |`
    Foreach { if( ![string]::IsNullOrWhiteSpace($_) -and ![string]::Equals('\n', $_)) {$machines += $_}}

	$taskList.split(',', [System.StringSplitOptions]::RemoveEmptyEntries) |`
    Foreach { if( ![string]::IsNullOrWhiteSpace($_) -and ![string]::Equals('\n', $_)) {$tasks += $_}}

	foreach ($machine in $machines){
		foreach($task in $tasks){
			Write-Host "Stopping job $task on machine $machine if available"
			StopTaskRemotely -ServerName $machine -task $task -credential $credentials

		}
	}


}


function RunTaskStops{
	param (
		[string]$machineList,
		[string]$taskList,
		[string]$adminUserName,
		[string]$adminPassword
		)
		$secpasswd = ConvertTo-SecureString $adminPassword -AsPlainText -Force
		$credentials = New-Object System.Management.Automation.PSCredential ($adminUserName, $secpasswd)
		StopScheduleTasks -machineList $machineList -taskList $taskList -credentials $credentials
}
