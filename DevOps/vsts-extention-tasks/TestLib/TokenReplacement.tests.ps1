$here = Split-Path -Parent $MyInvocation.MyCommand.Path
$script = "$here\..\TokenReplacement\Main.ps1"
Describe "TokenReplacement" {
	Context "Runs_with_passthru_Auth" {
		It "Runs" {
		$user = ''
		$password = ''
		$args = "-machineList vm-icon-dev1"
		$args += " -adminUserName '$user'"
		$args += " -adminPassword '$password'"
		$args += " -replacementValues 'aUser.Name|AValue`naUser.Password|anotherValue'"
		$args += " -fileList 'E:\Icon\testfile1.txt`re:\Icon\testfile2.txt'"
		Write-Host "$script $args"
		invoke-expression "$script $args"
		}
	}
}