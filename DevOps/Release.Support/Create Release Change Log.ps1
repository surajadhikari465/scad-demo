$scriptFullPath = $MyInvocation.MyCommand.Definition
$scriptFileObj = new-object system.io.fileinfo $MyInvocation.MyCommand.Definition
$scriptFolder = $scriptFileObj.directoryname

# Make sure we're in the location of this script, since it should be in Git.
cd $scriptFolder

$lineBreak = "------------------------------------------------------"
# Get target repository.
$repoName = "SCAD"
$tempDevOpsFolder = "\temp\devops\"

if(-not (Test-Path $tempDevOpsFolder)){
    new-item -Verbose -ItemType Directory $tempDevOpsFolder
}

# Show commits
$commitLogFile = "$tempDevOpsFolder\$repoName Release Commits.log"
$lineBreak
$commitLogFile
$lineBreak
if(-not (Test-Path $tempDevOpsFolder)){
    Get-Content $commitLogFile
} else {
    Write-Host -ForegroundColor Yellow "Release change log commit history file will be created."
}
$lineBreak

# Switch to base local GIT folder (should be 2 folders up, if this script stays in '...\%REPO_NAME%\DevOps\Release.Support\' folder.
cd "..\.."

# Validate we're in the right spot.
$gitStatus = git status
$gitStatus
$lineBreak
Write-Host -ForegroundColor Yellow "** Review the local-branch information output above, to ensure change log is taken from appropriate branch. **"
$lineBreak
read-host "If using correct branch, press ENTER to continue..."

$start = read-host "Start commit (not included)"
$gitStartCommitDetails = git show --summary $start
$gitStartCommitDetails
$end = read-host "End commit (included)"
$gitEndCommitDetails = git show --summary $end
$gitEndCommitDetails
$version = read-host "Enter $repoName release version (format: YYYY.II -- 4-digit year and 2-digit release ID)"

[string]$commitLogEntry = $repoName + " " + $version + "`t" + $start + "`t" + $end
$lineBreak
"New commit-log entry:"
$commitLogEntry
$lineBreak
Out-File -append -FilePath $commitLogFile -InputObject $commitLogEntry


#"SUMMARY"
#git diff --summary --relative $start $end
"Getting NAME-STATUS details from Git..."
$releaseChanges = git diff --name-status --relative $start $end
#cd ..\IRMA
#$releaseChanges += git diff --name-status --relative $start $end
#"---------------------------------------
#NAME-ONLY"
#git diff --name-only --relative $start $end

$releaseLogFile = "$tempDevOpsFolder\$repoName $version Release - Changes.log"
if(Test-Path $releaseLogFile){
    Read-Host ("** FILE EXISTS (and will be overwritten): " + $releaseLogFile)
}

#####################################################################################################

$appList = @(
    "icon/api controller",
#    "cch tax", #sunset
    "icon/item move",
    "icon/r10 listen",
    "ewic apl",
    "ewic error response",
    "icon/iconwebapi",
    "icon/infor/listeners/.*hier",
    "icon/infor/listeners/.*item",
#    "icon/infor/listeners/.*locale", #not used
#    "icon/infor/listeners/.*price", #not used
    "icon/infor/services/.*newitem",
    "icon/interface controller/global event",
    "icon/interface controller/pos push",
#    "icon/interface controller/regional event", #sunset
#    "icon/interface controller/subteamevent", #sunset
    "icon/interface controller/tlog",
    "icon/web/",
    "icon/monitoring/icon.mon",
    "icon/monitoring/icon.dash",
    "icon/nutrition/",
    "icon/vim/",
    "/MammothAuditService",
    "mammoth/esb/.*hier",
    "mammoth/esb/.*locale",
    "mammoth/esb/.*product",
    "mammoth.apicon",
    "mammoth.itemlocale",
    "mammoth.price",
    "mammothwebapi",
    "mammoth/primeaffinity",
    "mammoth/websupport"
)

$appsUpdatedList = @()
foreach($app in $appList){
    if($releaseChanges -match "$app.*\.cs$"){
        $appsUpdatedList += $app
    }
}

#####################################################################################################

Out-File -FilePath $releaseLogFile -InputObject "Apps With Class-File Updates:"
Out-File -Append -FilePath $releaseLogFile -InputObject $appsUpdatedList
Out-File -Append -FilePath $releaseLogFile -InputObject "`r`n$lineBreak"
Out-File -Append -FilePath $releaseLogFile -InputObject "[Full App List Used For Class-Update Search]"
Out-File -Append -FilePath $releaseLogFile -InputObject $appList
Out-File -Append -FilePath $releaseLogFile -InputObject "`r`n$lineBreak`r`n`r`n"
Out-File -Append -FilePath $releaseLogFile -InputObject $gitStatus
Out-File -Append -FilePath $releaseLogFile -InputObject $lineBreak
Out-File -Append -FilePath $releaseLogFile -InputObject $commitLogEntry
Out-File -Append -FilePath $releaseLogFile -InputObject $lineBreak
Out-File -Append -FilePath $releaseLogFile -InputObject $gitStartCommitDetails
Out-File -Append -FilePath $releaseLogFile -InputObject $lineBreak
Out-File -Append -FilePath $releaseLogFile -InputObject $gitEndCommitDetails
Out-File -Append -FilePath $releaseLogFile -InputObject "`r`n$lineBreak`r`n`r`n"

Out-File -Append -FilePath $releaseLogFile -InputObject $releaseChanges

#$releaseChanges = ("Apps With Class-File Updates:" + $appsUpdatedList + "`r`n$lineBreak`r`n`r`n") + $releaseChanges

"--> " + $releaseLogFile
Invoke-Item $releaseLogFile

