$here = Split-Path -Parent $MyInvocation.MyCommand.Path
$script = "$here\..\StartWindowsScheduledTasks\StartScheduleTasks.ps1"
$testUser = '' #enter test credentials
$testPassword = '' #enter test credentials
Import-Module $script
Describe "Start Windows Schedule Tasks" {
	Context "CanStartWindowsScheduledTask" {
		it "Can start a job" {
			RunTaskStarts -machineList "cewd1643" -taskList "TestJob1" -adminUserName $testUser -adminPassword $testPassword
		}
	}
}