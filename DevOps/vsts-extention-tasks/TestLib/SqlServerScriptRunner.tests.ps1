$here = Split-Path -Parent $MyInvocation.MyCommand.Path
$script = "$here\..\SqlServerScriptRunner\Main.ps1"
Describe "SqlServerScriptRunner" {
	Context "Runs_with_passthru_Auth" {
		It "Runs" {
		$user = ''
		$password = ''
		$args = "-ServerHost cewd1815"
		$args += " -adminUserName '$user'"
		$args += " -adminPassword '$password'"
		$args += " -SQLServer CEWD1815\SQLSHARED2012D"
		$args += " -WindowsAuthentication true"
		$args += " -Database iCONDev"
        $args += " -ScriptFile '\\cewd6503\buildshare\ICON_DEVOPS\Icon Dev\20160713.5\drop\Deployment\ICON_Dev_ReAdd_Users.sql'"
		$args += " -Timeout 30"
		invoke-expression "$script $args"
		}
	}
}