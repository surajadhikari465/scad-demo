$here = Split-Path -Parent $MyInvocation.MyCommand.Path
$script = "$here\..\StopWindowsServices\StopWindowsServices.ps1"
$testUser = '' #enter test credentials
$testPassword = '' #enter test credentials
Import-Module $script
Describe "Stop Windows Services" {
	Context "CanStopWindowsServices" {
		it "Can stop a Windows Service" {
			RunStopWindowsServices -machineList "cewd1643" -serviceList "SQL Server Browser" -adminUserName $testUser -adminPassword $testPassword
		}
	}
}