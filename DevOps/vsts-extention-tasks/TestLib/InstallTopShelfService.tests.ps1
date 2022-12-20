$here = Split-Path -Parent $MyInvocation.MyCommand.Path
$script = "$here\..\InstallTopShelfService\Main.ps1"
Describe "InstallTopShelfService" {
	Context "Runs_with_passthru_Auth" {
		It "Runs" {
		$user = ''
		$password = ''
		$args = "-machineList vm-icon-dev1"
		$args += " -adminUserName '$user'"
		$args += " -adminPassword '$password'"
		$args += " -specialUser 'custom'"
		$args += " -topshelfExe 'E:\Icon\API Controller Phase 2\Hierarchy\Icon.ApiController.Controller.exe'"
		$args += " -serviceUsername 'wfm\iconinterfaceuserdev'"
		$args += " -servicePassword 'Ic0n&irmaDEV'"
		$args += " -instanceName ''"
		$args += " -uninstallFirst 'true'"
		invoke-expression "$script $args"
		}
	}
}