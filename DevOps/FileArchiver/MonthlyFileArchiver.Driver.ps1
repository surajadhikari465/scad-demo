<#

File Archiver Driver
Created by Tom Lux
Dec, 2020

Description
--------------------------------------------
Calls archiver functions for each server, each file type, and each region, where appropriate.

See full details here: https://wholefoods.sharepoint.com/:w:/s/PPS_Sharing_P201/Eaf70Og2ZkhEvSLqR_3bwVYB1JDgc_vYEgD0CPmEdA8HFQ?e=Z0982a
--------------------------------------------

UPDATES/NOTES
(Tom) -- Was seeing errors (file in use) and then found leftover ps-session instances and determined it was because archiver functions were using Disconnect-Session rather than Remove-Session.

#>

$scriptFullPath = $MyInvocation.MyCommand.Definition
$scriptFileObj = new-object system.io.fileinfo $MyInvocation.MyCommand.Definition
$scriptFolder = $scriptFileObj.directoryname

$fnScript = "$scriptFolder\Functions.ps1"
">>> Loading functions [$fnScript]..."

if (-not (Test-Path $fnScript)) {

"
******************************************************************
Cannot Continue -- Expected Functions Script Not Found
--> '" + $fnScript + "'

Ensure this script exists in the same folder from which you
ran the current script and try again.
******************************************************************
"
    return
}

# Functions file must be in the same directory.
. $fnScript

# Switch to program folder.
cd $scriptFolder

# Create log dest dir if it does not exist.
$logFolder = $scriptFolder + "\logs"
If (-not (Test-Path $logFolder)){
    New-Item -Path $logFolder -ItemType Directory
}

########################
# Load config.
########################
# If cmd-line arg specified, we assume it's a JSON config file, otherwise we use default config file that matches program base filename.
if($args[0]){ $cfgFile = $args[0] }
else { $cfgFile = $scriptFolder + "\" + $scriptFileObj.BaseName + ".json" }

########################
# Start log.
########################
$logFile = "$logFolder\" + $scriptFileObj.Name + ".$cfgFile" + "." + (Get-Date -Format "yyyyMMdd.HHmmss") + ".$env:UserName" + ".$env:COMPUTERNAME" + ".log"
Start-Transcript -Path $logFile -Append

"[[" + $scriptFileObj.Name + " Starting]]"
Get-Date
"Log File: " + $logFile

$cfg = $null
"Loading JSON Cfg File: " + $cfgFile
$json = Get-Content $cfgFile -Raw
$cfg = ConvertFrom-Json $json
"Archiver count in config: " + $cfg.archiverList.Count

$outputDelim = "--------------------------------------------------"

foreach($archiver in $cfg.archiverList){
    "Archiver Config:"
    $archiver

    if(-not $archiver.serverList){
        "[" + (Get-Date) + "] " + "**ERROR -- CANNOT CONTINUE CURRENT ARCHIVER** ServerList value required."
        continue
    }

    $regionList = $archiver.regionList
    if(-not $regionList){ "Using default value regionList=WFM"; $regionList = "WFM" }

    $filenameDateFormat = $archiver.filenameDateFormat
    # Apply default, if value was provided.
    if(-not $filenameDateFormat){ "Using default value filenameDateFormat=yyyyMM"; $filenameDateFormat = "yyyyMM" }

    $targetDate = $archiver.targetDate
    # Default to last month, if value was provided.
    if(-not $targetDate){ "Using default value targetDate='last-month'"; $targetDate = ([DateTime]::Today).AddMonths(-1) }
    else {
        # Ensure valid date.
        try{
            $targetDate = [DateTime]$targetDate
        }
        catch{
            "[" + (Get-Date) + "] " + "**ERROR -- CANNOT CONTINUE CURRENT ARCHIVER** TargetDate value [" + $targetDate + "] is not a date."
            continue
        }
    }

    $addFolderToArchiveFileName = $archiver.addFolderToArchiveFileName
    if($addFolderToArchiveFileName -eq $null){ "Using default value addFolderToArchiveFileName=false"; $addFolderToArchiveFileName = $false }

    $deleteFilesAfterZip = $archiver.deleteFilesAfterZip
    if($deleteFilesAfterZip -eq $null){ "Using default value deleteFilesAfterZip=true"; $deleteFilesAfterZip = $true }

    $previousMonthsCount = $archiver.previousMonthsCount
    if(-not $previousMonthsCount){ "Using default value previousMonthsCount=1"; $previousMonthsCount = 1 }
    try{
        $previousMonthsCount = [int32]$previousMonthsCount
    }
    catch{
        "[" + (Get-Date) + "] " + "**WARNING** PreviousMonthsCount value [" + $previousMonthsCount + "] is not numeric, so value of '1' will be used."
        $previousMonthsCount = 1
    }
    # Enforce prev-months limits.
    if($previousMonthsCount -gt 120){ "Value of [$previousMonthsCount] for previousMonthsCount param is too large; using max value: 120"; $previousMonthsCount = 120 }
    if($previousMonthsCount -lt 1){ "Value of [$previousMonthsCount] for previousMonthsCount param is invalid; using min value: 1"; $previousMonthsCount = 120 }

    $recurse = $archiver.recurse
    if($recurse -eq $null){ "Using default value recurse=false"; $recurse = $false }

    $whatIfMode = $archiver.whatIfMode
    if($whatIfMode -eq $null){ "Using default value whatIfMode=false"; $whatIfMode = $false }

    for(($months = 1), ($j = 0); $months -le $previousMonthsCount; ($months++), ($targetDate = $targetDate.AddMonths(-1))){
        foreach($server in $archiver.serverList){
            # If no region-list was specified, a default value is used so the "region" loop will still execute once, which is desired, but we assume no "%region%" placeholders will be specified in any input param values, so no substitution will take place.
            foreach($region in $regionList){
                # If a param is not specified in the config, it will be NULL and will result in the SWITCH param (ex: "-recurse:$recurse") being ignored by the function (effectively FALSE).
                Archive-FilesByMonth `
                    -computerName $server `
                    -targetPath $archiver.targetPath.Replace("%region%", $region) `
                    -filenameFilterLeftOfDate $archiver.filenameFilterLeftOfDate.Replace("%region%", $region) `
                    -filenameFilterRightOfDate $archiver.filenameFilterRightOfDate.Replace("%region%", $region) `
                    -filenameDateFormat $filenameDateFormat `
                    -archiveID $archiver.archiveID.Replace("%region%", $region) `
                    -addFolderToArchiveFileName:$addFolderToArchiveFileName `
                    -deleteFilesAfterZip:$deleteFilesAfterZip `
                    -targetDate $targetDate `
                    -recurse:$recurse `
                    -whatIfMode:$whatIfMode
                ################################################################
                $outputDelim
            }
        }
    }
}

Stop-Transcript
