$here = Split-Path -Parent $MyInvocation.MyCommand.Path
$script = "$here\..\SqlServerDacpacDeployment\Main.ps1"
Describe "SqlServerDacpacDeployment" {
	Context "Runs_with_passthru_Auth" {
		It "Runs" {
		$user = ''
		$password = ''

		$args = "-machinesList 'localhost'"
		$args += "-adminUserName '$user'"
		$args += "-adminPassword '$password'"
		$args += "-winrmProtocol 'HTTP'"
		$args += "-dacpacFile 'C:\Users\2067560\OneDrive\Documents\WFM\DevOpsTools\vsts-extention-tasks\SampleDb\bin\Debug'"
		$args += "-targetMethod 'Server'"
		$args += "-serverName  'CEN106103'"
		$args += "-databaseName 'sampleDb'"
		$args += "-authscheme 'Windows Authentication'"
		$args += "-sqlUsername ''"
		$args += "-sqlPassword ''"
		$args += "-connectionString ''"
		$args += "-publishProfile ''"
		$args += "-additionalArguments ''"
		$args += "-deployInParallel 'true'"	
		$args += "-action 'publish'"
		$args += "-outputPath ''"

		Write-Host "$script $args"
		invoke-expression "$script $args"
		}
	}
}