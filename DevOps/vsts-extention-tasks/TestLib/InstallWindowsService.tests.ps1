$here = Split-Path -Parent $MyInvocation.MyCommand.Path
$script = "$here\..\InstallWindowsService\Main.ps1"
Describe "InstallWindowsService" {
	Context "Runs_with_passthru_Auth" {
		It "Runs" {
		$user = ''
		$password = ''
		$args = "-machineList vm-icon-dev1"
		$args += " -adminUserName '$user'"
		$args += " -adminPassword '$password'"
		$args += " -specialUser 'custom'"
		$args += " -BinaryPath 'E:\Icon\CCH Tax Listener\Icon.Esb.CchTaxListener.WindowsService.exe'"
		$args += " -serviceUsername 'wfm\iconinterfaceuserdev'"
		$args += " -servicePassword 'Ic0n&irmaDEV'"
		$args += " -ServiceName 'Icon.Esb.CchTaxListener.WindowsService'"
		$args += " -DisplayName 'Icon CCH Tax Listener'"
		$args += " -Description ''"
		$args += " -StartUpType 'Automatic'"
		Write-Host "$script $args"
		invoke-expression "$script $args"
		}
	}
}