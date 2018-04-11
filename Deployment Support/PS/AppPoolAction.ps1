<#

Take actions against IIS app pools.

#>

param(
    [string] $ServerList = $( throw 'ERROR [Missing ServerList Param]: At least one target server is required.' ), # List of web servers to take app-pool action against.
    [string] $PoolAction = $( throw 'ERROR [Missing PoolAction Param]: Action to perform is required.' ), # The action against the app-pools.
    [string] $PoolNameList = $( throw 'ERROR [Missing PoolNameList Param]: At least one target app pool is required.' ), # List of app-pool names that can contain a replacement string.
    [string] $PoolReplaceRef, # A string identifier that can exist in PoolNameList and be replaced with specified values.
    [string] $PoolReplaceValueList, # A list of replacement values where (in a loop) PoolReplaceRef in each PoolNameList item is replaced with each PoolReplaceValueList item.
                                    # Example:
                                    #    PoolReplaceRef=%foo%
                                    #    PoolNameList=FirstPool%foo%,SecondPool%foo%
                                    #    PoolReplaceValueList=A,B,C
                                    # Means these pools are targeted on each server: FirstPoolA,FirstPoolB,FirstPoolC,SecondPoolA,SecondPoolB,SecondPoolC
    [string] $ListDelim = ",", # Common delimiter used in all lists.
    [string] $AdmUsername = $( throw 'ERROR [Missing AdmUsername Param]: User is required.' ), # Admin user for target servers.
    [string] $AdmPassword = $( throw 'ERROR [Missing AdmPassword Param]: Password is required.' ) # Admin pwd.
)

if($PoolReplaceValueList){
    if(-not $PoolReplaceRef){
        throw "ERROR [Missing PoolReplaceRef Param]: Replacement values were provided but no target placeholder was specified."
        return
    }
    $replaceList = $PoolReplaceValueList.Split($ListDelim)
    # Do replacements and build list of target app-pools.
    $webPoolList = @()
    foreach ($replaceValue in $replaceList){
        $webPoolList += $PoolNameList -replace $PoolReplaceRef, $replaceValue
    }
} else {
    if($PoolReplaceRef){
        throw "ERROR [Missing PoolReplaceValueList Param]: A replacement target placeholder was provided but no replacement values were specified."
        return
    }
    # No replacement params were provided, so the list of target pool names is final.
    Write-Host "No app-pool list replacements performed."
    $webPoolList = $poolNameList.Split($ListDelim)
}

"Web App-Pool List:"
$webPoolList

$webSvrList = $ServerList.Split($ListDelim)

# Validate action.
$startAction = ($PoolAction -like "start")
$stopAction = ($PoolAction -like "stop")

if((-not $startAction) -and (-not $stopAction)){
    throw "**ERROR** -- App-pool action [$poolAction] is not one of the valid options 'start' or 'stop'."
    return
}

# Create credential for remote-server tasks.
$cryptPwd = ConvertTo-SecureString $AdmPassword -AsPlainText -Force
$auth = New-Object System.Management.Automation.PSCredential ($AdmUserName, $cryptPwd)

foreach ($svr in $webSvrList){
    "Current Server: $svr"
    Invoke-Command -ComputerName ([System.Net.Dns]::GetHostByName($svr)).HostName -Credential $auth -ScriptBlock {
    param($listOfAppPools, $isStop)
        Add-PSSnapin webadministration
        $pools = $listOfAppPools.split(",")
        foreach ($pool in $pools){
            if($isStop){
                "Stopping app pool: " + $pool
                stop-webapppool $pool -verbose
            } else {
                "Starting app pool: " + $pool
                start-webapppool $pool -verbose
            }
        }
    } -ArgumentList ($webPoolList -join ","), $stopAction # We force comma delim here because we control the code inside and use comma there.
}
