function StartTaskRemotely() {

    param($ServerName, $task, $credential)

    $hostName = ([System.Net.Dns]::GetHostByName($serverName)).HostName
	Write-Host "Invoking command on to $hostName to enable Task $task"
    $result = Invoke-Command -ComputerName $hostName -Credential $credential  -ErrorVariable errmsg 2>$null -ScriptBlock {
		param($taskQuery) 
		try{
		Get-ScheduledTask | ? {$_.TaskName -like $taskQuery} | Enable-ScheduledTask
		}
		catch
		{
			return $_
		}
	} -ArgumentList $task 
	Write-Host $errmsg;
}

function StartScheduleTasks{
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
			Write-Host "Starting job $task on machine $machine if available"
			StartTaskRemotely -ServerName $machine -task $task -credential $credentials

		}
	}


}


function RunTaskStarts{
	param (
		[string]$machineList,
		[string]$taskList,
		[string]$adminUserName,
		[string]$adminPassword
		)
		$secpasswd = ConvertTo-SecureString $adminPassword -AsPlainText -Force
		$credentials = New-Object System.Management.Automation.PSCredential ($adminUserName, $secpasswd)
		StartScheduleTasks -machineList $machineList -taskList $taskList -credentials $credentials
}
