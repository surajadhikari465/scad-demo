
# Reminder breakpoint.
"
******************************************
*** GET LATEST from TFS FIRST!!! ***
** AND SET VERSION # in this script **
******************************************
"
read-host -prompt "<press enter when ready>"

# Sometimes these fail; when they do, run each line separately then rerun full script.
Add-PSSnapin Microsoft.TeamFoundation.PowerShell
Get-PSSnapin Microsoft.TeamFoundation.PowerShell

# Unique identifier added to output filename.
$releaseName = "ICON v4.8"

# IRMA or ICON
$targetTfs = "ICON"

# Example with time: “D06/30/2014 23:00:00~”
$changesetDateRange = “D04/13/2015~D04/24/2015”


# IRMA --> http://cewp1605:8080/tfs
# Icon --> https://irma.visualstudio.com/defaultcollection
if($targetTfs -eq "ICON"){
    $tfsUrl = "https://irma.visualstudio.com/defaultcollection"
    $localTfsFolder = "/visualstudio.com"
    # proj path needs to end with "/" so it only recurses below that folder.
    $tfsProjectPath = "$/Item Consolidation/Icon/"
} else {
    $tfsUrl = "http://cewp1605:8080/tfs"
    # proj path needs to end with "/" so it only recurses below that folder.
    $localTfsFolder = "/tfs2010"
    $tfsProjectPath = "$/IRMA/DEV/"
    $tfsProjectPathIrmaDb = "$/IRMA/ItemCatalog/IRMA Scripts - DEV/Staging"
}


$tfsSvr = Get-TfsServer -Name $tfsUrl

# If you get an error on this line, try separately running the two commands on lines 12-13.
$workItemStore = $tfsSvr.GetService([Microsoft.TeamFoundation.WorkItemTracking.Client.WorkItemStore])

$baseDevPath = "C:\visualstudio.com"
$baseLogPath = "C:\Users\kyle.milner.WFM\Documents\deployment"
$tfsLocalBasePath = $baseDevPath + $localTfsFolder


# =======================================================================
# =======================================================================
# =======================================================================
# =======================================================================


function GetTFSFileList (
    [string]$tfsPath,
    [string]$tfsVersion,
    [ref]$pureFileList,
    [ref]$fileListByChangeset,
    [ref]$changesetList,
    [ref]$noWorkItemFileList,
    [ref]$uniqueTaskNameList
)
{

    $changes = Get-TfsItemHistory $tfsPath -server $tfsSvr -Version $tfsVersion -Recurse 
    $numProcessed = 1
    $changes | foreach {
        write-host ("<< " + $numProcessed + " of " + $changes.Count + " >>")
        $numProcessed++;
        $changesetID = $_.changesetid
        $changeset = Get-TfsChangeset -ChangesetNumber $_.changesetid -server $tfsSvr
        $changeDateTime = $changeset.CreationDate.ToShortDateString() + " " + $changeset.CreationDate.ToShortTimeString()

        "Changeset " + $changesetID + ": " + $changeDateTime

        $taskList = $changeset.WorkItems

        # Get list of changes in the changeset.
        $changelist = $changeset.changes
        # Loop through list of changes in changeset.
        $changeCount = 0
        $changelist | foreach {
            $changeCount = $changelist.Count
            # Get specific TFS artifact that was changed (file).
            $artifact = $_.Item.ServerItem
            # Add base/local-system path.
            $file = $artifact.replace("$", $tfsLocalBasePath)
            if($pureFileList.value -notcontains $file){
                $pureFileList.value += $file
            }
            if($fileListByChangeset.value -notcontains $file){
                $fileListByChangeset.value += $file + " [CS " + [string]$changesetID + "]" + "[Chg=" + $_.ChangeType + "]" + "[DT=" + $changeDateTime + "][Dev=" + $changeset.Owner + "]"
            }
            # Track files not associated to work items.
            if($taskList.Count -eq 0){
                if($noWorkItemFileList.value -notcontains $file){
                    $noWorkItemFileList.value += $file
                }
            }
        }



        $taskList | foreach { 
            $wiType = $_.Type.Name
            $taskParentTitle = ""
            if($wiType -eq "Task"){
                $taskParentTitle = " [PBI=" + (GetWorkItemParentTitle $_.id) + "]"
            }

            $detailedTaskName = "[" + $_.Type.Name + "] TFS " + [string]$_.id + " - " + $_.title + $taskParentTitle

            # Build changeset/task entry.
            $cstEntry = (
                "CS " + [string]$changesetID + 
                " :: Changes=" + $changeCount + 
                ", Tasks=" + $taskList.Count + 
                " :: [" + $_.Type.Name + "] TFS " + [string]$_.id + 
                " - " + $_.title + $taskParentTitle + " " +
                "[DT=" + $changeDateTime + "]" + 
                "[Dev=" + $changeset.Owner + "]"
            ) 
            if($changesetList.value -notcontains $cstEntry){
                $changesetList.value += $cstEntry
            }

            # Unique list of PBIs/Tasks/Parents
            if($uniqueTaskNameList.value -notcontains $detailedTaskName){
                $uniqueTaskNameList.value += $detailedTaskName
            }
        }
    }
}

function GetWorkItemParentTitle (
    $workItemId
)
{
    $workItem = $workItemStore.GetWorkItem($workItemId)
    $parentLink = $workItem.Links | Where {$_.LinkTypeEnd.Name -eq "Parent"}
    # If workitem has a parent, get it.
    if($parentLink.Count -gt 0){
        $parentWorkItem = $workItemStore.GetWorkItem($parentLink.RelatedWorkItemId)
        [string]$parentWorkItem.Id + " - " + $parentWorkItem.Title
    }
}

###############################
# Build component list.
###############################
function BuildReleaseComponents (
    [ref]$pureFileList, 
    [ref]$componentList,
    [ref]$componentTypeHash)
{

    $pureFileList.value | foreach {
        #if($_.ToString().Trim().Length -eq 0){
        #    continue;
        #}

        $folders = $_.Split("/")
        $component = $folders[3] + "/" + $folders[4]
        # Associate file with component type in hashtable.
        $componentTypeHash.value.Add($_, $component)
        if($componentList.value -notcontains $component){
            $componentList.value += $component
        }
    }
}# END [BuildReleaseComponents]


# =======================================================================
# =======================================================================
# =======================================================================
# =======================================================================


# Clear lists to house updated files and components.
$fileListByChangeset = @()
$pureFileList = @()
$changesetList = @()
$componentList = @()
$componentTypeHash = @{}
$noWorkItemFileList = @()
$uniqueTaskNameList = @()


GetTFSFileList $tfsProjectPath $changesetDateRange  ([ref]$pureFileList) ([ref]$fileListByChangeset) ([ref]$changesetList) ([ref]$noWorkItemFileList) ([ref]$uniqueTaskNameList)


# Get IRMA DB scripts (schema & pop-data) from diff project path.
if($targetTfs -eq "IRMA"){
    GetTFSFileList $tfsProjectPathIrmaDb $changesetDateRange  ([ref]$pureFileList) ([ref]$fileListByChangeset) ([ref]$changesetList) ([ref]$noWorkItemFileList) ([ref]$uniqueTaskNameList)
}

$fileListByChangeset = ($fileListByChangeset | Sort-Object)

BuildReleaseComponents ([ref]$pureFileList) ([ref]$componentList) ([ref]$componentTypeHash)

$releaseDetails = ""

$releaseDetails += "

******* PURE UPDATED FILES (" + $pureFileList.Count + ") ********
"
$releaseDetails += ($componentTypeHash.keys | Sort-Object | foreach { $_ + " (" + $componentTypeHash[$_] + ")" }) -join "`n"

$releaseDetails += "

******* UPDATED FILES BY CHANGESET# (" + $fileListByChangeset.Count + ") ********
"
$releaseDetails += $fileListByChangeset -join "`n"

$releaseDetails += "

******* UPDATES WITHOUT ASSOCIATED WORK ITEMS (" + $noWorkItemFileList.Count + ") ********
"
$releaseDetails += $noWorkItemFileList -join "`n"

$releaseDetails += "

******* CHANGESETS & TASKS (" + $changesetList.Count + ") ********
"
$releaseDetails += ($changesetList | Sort-Object) -join "`n"


$releaseDetails += "

******* UNIQUE PBIs/TASKS (" + $uniqueTaskNameList.Count + ") ********
"
$releaseDetails += ($uniqueTaskNameList | Sort-Object) -join "`n"


$releaseDetails += "

******* COMPONENT LIST ********
"
$releaseDetails += ($componentList | Sort-Object) -join "`n"

$releaseDetails += "

------------------
"

# Show details to PS console.
$releaseDetails


$releaseDetailsFile = $baseLogPath + "\change control\" + $releaseName + ".ReleaseDetails."
$releaseDetailsFile += $targetTfs + "." + $changesetDateRange.Replace("/", "-").Replace(":", "") + "." + (Get-Date -Format "yyyyMMddHHmmss") + ".log"
Out-File -FilePath $releaseDetailsFile -InputObject $releaseDetails -Append


