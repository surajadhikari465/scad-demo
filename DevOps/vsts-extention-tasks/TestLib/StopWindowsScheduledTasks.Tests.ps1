$here = Split-Path -Parent $MyInvocation.MyCommand.Path
$script = "$here\..\StopWindowsScheduledTasks\StopScheduleTasks.ps1"
$testUser = '' #enter test credentials
$testPassword = '' #enter test credentials
Import-Module $script
Describe "Stop Windows Schedule Tasks" {
	Context "CanStopWindowsScheduledTask" {
		it "Can stop a job" {
			RunTaskStops -machineList "cewd1643" -taskList "TestJob1" -adminUserName $testUser -adminPassword $testPassword
		}
	}
}