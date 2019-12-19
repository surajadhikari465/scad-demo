Import-Module pscx;
Import-Module psake;

#New-Item dte:/commandbars/menubar/build/pushlish-oos-dev -Value {
	$solutionPath = get-item dte:\solution |
					select -expand fullname |
					Split-Path -Parent;
#	
	$psakePath = Join-Path $solutionPath -Child "builds.ps1"
#	
	write-host $psakePath;
Invoke-psake $psakePath -task clean | Out-Host;
	write-prompt;
#}

Write-Host "solution: $solutionPath"
function unregister-OutOfStock {
	write-host "unloading..."
	Remove-Module psake;
	Remove-Item dte:/commandbars/menubar/build/pushlish-oos-dev
}