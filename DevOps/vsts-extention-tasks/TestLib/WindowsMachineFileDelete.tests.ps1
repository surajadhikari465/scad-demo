$here = Split-Path -Parent $MyInvocation.MyCommand.Path
$script = "$here\..\WindowsMachineFileDelete\Main.ps1"
Describe "WindowsMachineFileDelete" {
	Context "Runs_with_passthru_Auth" {
		It "Runs" {
		$user = ''
		$password = ''
		$args = "-machineList vm-icon-dev1"
		$args += " -isDirectory 'false'"
		$args += " -adminUserName '$user'"
		$args += " -adminPassword '$password'"
		$args += " -fileToDelete 'E:\Icon\API Controller Phase 2\Dev\nlog.txt'"
		Write-Host "$script $args"
		invoke-expression "$script $args"
		}
	}
}