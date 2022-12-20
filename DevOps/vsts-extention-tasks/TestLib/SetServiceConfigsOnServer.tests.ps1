$here = Split-Path -Parent $MyInvocation.MyCommand.Path
$script = "$here\..\SetServiceConfigsOnServer\Main.ps1"
Describe "SetServiceConfigsOnServer" {
	Context "Runs_with_passthru_Auth" {
		It "Runs" {
		$user = ''
		$password = ''
		$args = "-machineList vm-icon-dev1"
		$args += " -adminUserName '$user'"
		$args += " -adminPassword '$password'"
		$args += " -configName 'Icon.ApiController.Controller.exe.config'"
		$args += " -replacementValues 'ControllerInstanceId|1~iteration~'"
		$args += " -installPath 'E:\Icon\API Controller Phase 2\Hierarchy'"
		Write-Host "$script $args"
		invoke-expression "$script $args"
		}
	}
}