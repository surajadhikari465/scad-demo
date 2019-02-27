$drop = $true
$svrAlias = "IRMADevWeb01"
$svrHostname = ([System.Net.Dns]::GetHostByName($svrAlias)).HostName

$serverupFilePath = "c:\inetpub\wwwroot\serverup.html"

if($drop){
    Invoke-Command -ComputerName $svrHostname -ScriptBlock {
    param($supFilePath)

        Remove-Item -Verbose $supFilePath

    
    } -ArgumentList $serverupFilePath
} else{

    Invoke-Command -ComputerName $svrHostname -ScriptBlock {
    param($supFilePath, $alias, $hostname)

        Out-File -FilePath $supFilePath -InputObject ("<h1>$alias - $hostname</h1>")
    
    } -ArgumentList $serverupFilePath, $svrAlias, $svrHostname
}

dir "\\$svrAlias\c$\inetpub\wwwroot\serverup.html"

