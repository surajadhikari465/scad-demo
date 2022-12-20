$here = Split-Path -Parent $MyInvocation.MyCommand.Path
$script = "$here\..\UserAccountDisabler\Main.ps1"
Describe "UserAccountEnabler" {
	Context "Runs_with_passthru_Auth" {
		It "Runs" {
		$args = '-AccountToEnable wfm\icon.deploy.nonprod'
		Write-Host "$script $args"
		invoke-expression "$script $args"
		}
	}
}