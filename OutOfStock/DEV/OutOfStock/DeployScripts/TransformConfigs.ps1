param
(
  [string]$Environment
)

if ($Environment -eq "") {
  Write-Output "No Environment provided. skipping config transform.";
  exit;
}

$transformFile = "..\Web.$Environment.config.transformed"
$configToOverwrite = "..\web.config";
if (Test-Path -path $transformFile) {
  Write-Output "Found $transformFile";
  Copy-Item -Path $transformFile -Destination $configToOverwrite -Force -Verbose
  Get-ChildItem -Path "..\web.*.config.transformed" | ForEach-Object { write-output "Removing $_"; $_.Delete(); }
}
else {
  Write-Host "Tranform file not found: $transformFile"
  exit;
}
