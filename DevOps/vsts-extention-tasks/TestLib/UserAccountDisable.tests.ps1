$here = Split-Path -Parent $MyInvocation.MyCommand.Path
$script = "$here\..\UserAccountDisabler\Main.ps1"
Describe "UserAccountDisabler" {
	Context "Runs_with_passthru_Auth" {
		It "Runs" {
		#test
		$args = '-AccountToDisable wfm\icon.deploy.nonprod'
		Write-Host "$script $args"
		invoke-expression "$script $args"
		}
	}
}