$here = Split-Path -Parent $MyInvocation.MyCommand.Path
$script = "$here\..\CodeAssassinConfigTransformPicker\Main.ps1"
Describe "CodeAssassinConfigTransformPicker" {
	Context "Runs_with_passthru_Auth" {
		It "Runs" {
		$user = ''
		$password = ''
		$args = "-machineList vm-icon-dev1"
		$args += " -adminUserName '$user'"
		$args += " -adminPassword '$password'"
		$args += " -configName 'Icon.ApiController.Controller.exe.config'"
		$args += " -transform 'Debug'"
		$args += " -installPath 'E:\Icon\API Controller Phase 2\Dev'"
		Write-Host "$script $args"
		invoke-expression "$script $args"
		}
	}
}