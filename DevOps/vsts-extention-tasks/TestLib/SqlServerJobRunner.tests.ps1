$here = Split-Path -Parent $MyInvocation.MyCommand.Path
$script = "$here\..\SqlServerJobRunner\Main.ps1"
Describe "SqlServerJobRunner" {
	Context "Runs_with_passthru_Auth" {
		It "Runs" {
		$user = ''
		$password = ''
		$args = "-ServerHost CEWD1815"
		$args += " -adminUserName '$user'"
		$args += " -adminPassword '$password'"
		$args += " -SQLServer 'CEWD1815\SQLSHARED2012D'"
		$args += " -WindowsAuthentication true"
		$args += " -JobName 'Brand Purge - IconDev'"
		Write-Host "$script $args"
		invoke-expression "$script $args"
		}
	}
}