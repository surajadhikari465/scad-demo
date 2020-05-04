<#
------------------------------------------------------
Script-Update History
------------------------------------------------------
Tom Lux, 2019.10.23:
1) Script will add an entry to the update list if the base app.config or web.config file has an edit.
2) Change-log-commit-history file and change-log file so they reference IRMAPrdFile2 now.

Tom Lux, Spring 2020:
Fixed app list entries (reg-ex) for a few apps that were not being missed.
Added Perf configs.

Tom Lux, 2020.05.04:
Added new Icon Bulk Brand Upload.



#>

$lineBreak = "------------------------------------------------------"
# Get target repository.
$repoName = "SCAD"

# Show commits
#$commitLogFile = "c:\tlux\dev\Change Control\$repoName Release Commits.log"
$commitLogFile = "\\irmaprdfile2\e$\devops\ps\Git Change Log\$repoName Release Commits.log"
$lineBreak
$commitLogFile
$lineBreak
if(Test-Path $commitLogFile){ Get-Content $commitLogFile } else { Write-Host -ForegroundColor Yellow "**No change-log-commit-history file found.**" }
$lineBreak

# Switch to base local GIT folder
$baseGit = read-host "Enter local SCAD repo location, (ex: c:\tlux\dev\git\scad\)"
cd $baseGit

# Validate we're in the right spot.
$gitStatus = git status
$gitStatus
read-host "Using correct branch?"

$start = read-host "Start commit (not included)"
$gitStartCommitDetails = git show --summary $start
$gitStartCommitDetails
$end = read-host "End commit (included)"
$gitEndCommitDetails = git show --summary $end
$gitEndCommitDetails
$version = read-host "Enter $repoName release version"

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

$releaseLogFile = "\\irmaprdfile2\e$\devops\ps\Git Change Log\$repoName $version Release - Changes.log"
if(Test-Path $releaseLogFile){
    Read-Host ("** FILE EXISTS (and will be overwritten): " + $releaseLogFile)
}

#####################################################################################################

$appList = @(
    "icon/api controller",
    "cch tax", #sunset, then reactivated with icon-reboot
    "icon/.*item move",
    "icon/.*r10 listen",
    "ewic apl",
    "ewic error response",
    "icon/iconwebapi",
#    "icon/infor/listeners/.*hier", #sunset with IR
#    "icon/infor/listeners/.*item", #sunset with IR
#    "icon/infor/listeners/.*locale", #not used
#    "icon/infor/listeners/.*price", #not used
#    "icon/infor/services/.*newitem", #sunset with IR
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
    # new with Icon Reboot:
    "icon/AttributePublisher",
    "icon/ItemPublisher",
    "Services.BulkItemUpload",
    "Services.Extract",
    "Services.NewItem",
    # later addition:
    "Services.BrandUpload",
    ############################
    "/MammothAuditService",
    "mammoth/esb/.*hier",
    "mammoth/esb/.*locale",
    "mammoth/esb/.*product",
    "mammoth.apicon",
    "mammoth.itemlocale",
    "mammoth.price",
    "mammothwebapi",
    "mammoth/primeaffinity",
    "mammoth/websupport",
    "KitBuilder.ESB.Listeners.Item",
    "KitBuilder.Esb.LocaleListener",
    "KitBuilderWeb/",
    "KitBuilderWebApi/"
)

$appsUpdatedList = @()
foreach($app in $appList){
    # Class-files
    if($releaseChanges -match "$app.*\.cs$"){
        $appsUpdatedList += $app
    }

    # For the next set of files, entries are only added to the updated-apps list if they aren't already in the list for a class-file (functional) update. 

    # Project files
    if($releaseChanges -match "$app.*\.csproj$"){
        if(-not $appsUpdatedList.Contains($app)){
            $appsUpdatedList += ($app + " (PROJ)")
        }
    }

    # Base App-Config files
    if($releaseChanges -match "$app.*/app.config$"){
        if(-not $appsUpdatedList.Contains($app)){
            $appsUpdatedList += ($app + " (BASE.APP.CFG)")
        }
    }

    # Base Web-Config files
    if($releaseChanges -match "$app.*/web.config$"){
        if(-not $appsUpdatedList.Contains($app)){
            $appsUpdatedList += ($app + " (BASE.WEB.CFG)")
        }
    }

    # QA Config files
    if($releaseChanges -match "$app.*\.QA\.config$"){
        if(-not $appsUpdatedList.Contains($app)){
            $appsUpdatedList += ($app + " (QA.CFG)")
        }
    }

    # Perf Config files
    if($releaseChanges -match "$app.*\.Perf\.config$"){
        if(-not $appsUpdatedList.Contains($app)){
            $appsUpdatedList += ($app + " (PERF.CFG)")
        }
    }

    # Prod Config files
    if($releaseChanges -match "$app.*\.Release\.config$"){
        if(-not $appsUpdatedList.Contains($app)){
            $appsUpdatedList += ($app + " (REL.CFG)")
        }
    }
}


# TIBCO EARs
if($releaseChanges -match ".*\.ear$"){
    $ears = $releaseChanges -match ".*\.ear$"
    foreach($ear in $ears){
        # Lines will have change type and file, separated by tab, so we just grab the file.
        $appsUpdatedList += $ear.split("`t")[1]
    }
}

#####################################################################################################

Out-File -FilePath $releaseLogFile -InputObject "Apps With Class/Proj/Cgf-File Updates:"
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


read-host "Press <ENTER> to exit..."
