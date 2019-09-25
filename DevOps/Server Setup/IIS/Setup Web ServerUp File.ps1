$drop = $false
$svrAliasList = @("IRMAQAWeb01", "IRMAQAWeb02", "IRMAQAWeb03", "IRMAQAWeb04", "IRMAQAWeb05", "IRMAQAWeb06")

$serverupFilePath = "c:\inetpub\wwwroot\serverup.html"

foreach($svrAlias in $svrAliasList){
    $svrHostname = ([System.Net.Dns]::GetHostByName($svrAlias)).HostName

    if($drop){
        "[$svrAlias] Remove Server-Up File"
        Invoke-Command -ComputerName $svrHostname -ScriptBlock {
        param($supFilePath)

            Remove-Item -Verbose $supFilePath

    
        } -ArgumentList $serverupFilePath
    } else{

        "[$svrAlias] Create Server-Up File"
        Invoke-Command -ComputerName $svrHostname -ScriptBlock {
        param($supFilePath, $alias, $hostname)

            Out-File -FilePath $supFilePath -InputObject ("<h1>$alias - $hostname</h1>")
    
        } -ArgumentList $serverupFilePath, $svrAlias, $svrHostname
    }

    dir "\\$svrAlias\c$\inetpub\wwwroot\serverup.html"

}