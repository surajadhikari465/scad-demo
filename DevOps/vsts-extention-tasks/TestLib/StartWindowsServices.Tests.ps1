$here = Split-Path -Parent $MyInvocation.MyCommand.Path
$script = "$here\..\StartWindowsServices\StartWindowsServices.ps1"
$testUser = '' #enter test credentials
$testPassword = '' #enter test credentials
Import-Module $script
Describe "Start Windows Service" {
	Context "CanStartWindowsService" {
		it "Can start a service" {
			RunStartWindowsServices -machineList "cewd1643" -serviceList "SQL Server Browser" -adminUserName $testUser -adminPassword $testPassword
		}
	}
}