<#

-------------------------------------------------------------------
PeopleSoftFileHashGenerator.ps1
-------------------------------------------------------------------

This script generates hash values for Peoplesoft data files and stores the value in a separate file with the same name
as the Peoplesoft file but with a ".hash" file extention.

The env and region parameters are required.

An optional data parameter can be used, in case a day is missed or a run attempt fails.

IRMA[QA|Prd]File have old version of powershell (v2) and do not meet the requirements for installing powershell 4 or 5,
therefore these servers do not have the get-filehash command.  To work around this, we can copy the PeopleSoft file to 
an alternate server in the environment (QA=VM-Icon-QA1, Prod=IRMAPrdFile2), for example, since these are 2012 servers running 
powershell v4, and then run the hash command from that server against the local file (you cannot invoke a command on a 
remote server that then references a UNC/remote path, since that's a "double hop").

We use an input file to store the name of the server where we'll execute the hash-generation command.  The env param is used
to identify the file containing the hash server name.

Because this process could run simultaneously for different regions (it's intended to be triggered by the completion of an IRMA 
PeopleSoft job that generates a data file), the log file is region-specific.

Not all log entries are written to the log file, but all log entries are output to the console, so additional runtime information
could be viewed in the job's output tab in Tidal.

[2016-09-28] Added job name as arg.
We expect the hash job's name (string) passed in to contain a keyword so that we can determine for which PeopleSoft data file we
are generating a hash value.  VALID KEYWORDS, MAPPING:
"PeopleSoftUpload"   -- This job-name keyword identifies the normal upload file (ex: PSFile_FL_20160925.EDI)
"FINTEC"             -- This job-name keyword identifies the FINTECH upload file (ex: PSFINTECH_FL_20160925.txt) **NOTE: Sometimes the last "H" is missing from "FINTECH" in filename, so we exclude it from file refs, just in case.
"Transfer"           -- This job-name keyword identifies the GL upload file (ex: PSGL_FL_20160925.txt)

The PeopleSoft-Upload jobs are named with one of the three keywords above, so the hash-gen job for each must have the same keyword.

See [e:\ScheduledJobs\PeopleSoft File Hash Generator\Prd\Logs] for output examples.

==================================================================



***************
DEBUG from cmd.exe
PowerShell.exe -Command "& 'e:\ScheduledJobs\PeopleSoft File Hash Generator\Test\PeopleSoftFileHashGenerator.ps1' Env=Test Region=FL Job='FL - Dev - PeopleSoft Transfer Hash'"
PowerShell.exe -Command "& 'e:\ScheduledJobs\PeopleSoft File Hash Generator\Test\PeopleSoftFileHashGenerator.ps1' Env=Test Region=FL Job='FL - Dev - PeopleSoft Transfer Hash' Date=20160718"
***************

#>

####################################################################################################################################
####################################################################################################################################


"PeopleSoft File Hash Generator"


# Build script-execution variables.
$scriptFullPath = $MyInvocation.MyCommand.Definition
$scriptFileObj = new-object system.io.fileinfo $MyInvocation.MyCommand.Definition
$scriptFolder = $scriptFileObj.directoryname

">>> Loading functions..."
">>> Trying script location..."
$fnScript = $scriptFolder + "\Functions.ps1"

if (-not (Test-Path $fnScript)) {
"
>>> Local functions script not found...
--> '" + $fnScript + "'

>>> Trying change-doc site...
"
# Try to load functions script from Sharepoint Change Docs List.
$fnScript = "\\tmn\DavWWWRoot\global\it\Change Control Documentation\Functions.ps1"
    
}

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


####################################################################################################################################
function IdentifyJobType() {
    param(
        [string] $JobName,
        [System.Collections.Hashtable] $JobTypeHash
    )
    
    foreach ($key in $JobTypeHash.Keys){
        if($JobName.Contains($key)){
            return $key
        }
    }

    return $null
}
####################################################################################################################################
# Grab params needed to create unique log file name.

$todayYmd = (Get-Date).year.ToString() + (Get-Date).month.ToString().padleft(2, "0") + (Get-Date).day.ToString().padleft(2, "0")
$logFolder = $scriptFolder + "\Logs"
if(-not (Test-Path $logFolder)){
    "Creating log folder [" + $logFolder + "]..."
    New-Item -ItemType Directory -Path $logFolder
}

# The target region.
$regionParam = GetDeployParam -ArgList $args -ParamName "region"
if(-not $regionParam){
    $msg = "ERROR [CANNOT CONTINUE]: The 'Region' parameter is required."
    Write-Host -ForegroundColor Magenta $msg
    exit
}

# Job name and target data file identifier mapping.
# In this hashtable: the key is the keyword found in the job's name
# In this hashtable: the value is the corresponding unique string present in the filename generated by that specific PeopleSoft Upload job.
$jobTypeHash = @{}
$jobTypeHash.Add("PeopleSoftUpload", "PSFile")
$jobTypeHash.Add("FINTEC", "PSFINTEC")
$jobTypeHash.Add("Transfer", "PSGL")

# Job.
$jobParam = GetDeployParam -ArgList $args -ParamName "job"
if(-not $jobParam){
    $msg = "ERROR [CANNOT CONTINUE]: The 'Job' parameter is required."
    Write-Host -ForegroundColor Magenta $msg
    exit
}

# Parse job name to identify the unique string needed to target the specific PeopleSoft data file.
$jobTypeKeyword = IdentifyJobType -JobName $jobParam -JobTypeHash $jobTypeHash
$jobType = $jobTypeHash.Item($jobTypeKeyword)

if(-not $jobType){
    $msg = "ERROR [CANNOT CONTINUE]: The specified job '" + $jobParam + "' could not be mapped to a valid PeopleSoft data file identifier: [" + $jobTypeHash.Values + "]"
    Write-Host -ForegroundColor Magenta $msg
    exit
}


####################################################################################################################################
# Setup logging.

$logFilePath = $logFolder + "\" + $scriptFileObj.name + "." + $todayYmd + "." + $regionParam + "." + $jobType + ".log"
"LOG FILE: [" + $logFilePath + "]"

$msg = "Script Executing: [" + $scriptFileObj.name + "] by [" + $env:USERNAME + "] on [" + $env:COMPUTERNAME + "] at [" + (Get-Date) + "]"
LogAndWrite $msg

####################################################################################################################################


"Parsing args..."

if(-not $args){
    $msg = "ERROR [CANNOT CONTINUE]: No command-line args provided."
    Write-Host -ForegroundColor Magenta $msg
    LogAndWrite $msg
    exit
}

"Parameters Found:"

# Target environment.
$envParam = GetDeployParam -ArgList $args -ParamName "env"
if(-not $envParam){
    $msg = "ERROR [CANNOT CONTINUE]: The 'Env' parameter is required."
    Write-Host -ForegroundColor Magenta $msg
    LogAndWrite $msg
    exit
}
"Environment: " + $envParam

"Region: " + $regionParam

"Job Name (Type): " + $jobParam + " (" + $jobType + ")"

# The target date, format=YYYYMMDD (optional, default is today's date).
$dateParam = GetDeployParam -ArgList $args -ParamName "date"
if(-not $dateParam){
    $dateParam = (Get-Date).year.ToString() + (Get-Date).month.ToString().padleft(2, "0") + (Get-Date).day.ToString().padleft(2, "0")
    "No date param specified; using today: " + $dateParam + "."
} else {
    $msg = "Date parameter received."
    LogAndWrite $msg
}
"Date: " + $dateParam

$msg = "Environment=" + $envParam + ", Region=" + $regionParam + ", Date=" + $dateParam + ", FileType=" + $jobType
LogAndWrite $msg

#########################################
# Command-Line Arg Validation
#########################################

# Valid environment list.
$envHash = @{}
$envHash.Add("test", "test")
$envHash.Add("qa", "qa")
$envHash.Add("prd", "prd")

# Validate env param passed in.
if(-not ($envHash.Values -like $envParam)){
    $msg = "ERROR [CANNOT CONTINUE]: The specified environment '" + $envParam + "' is not one of the following valid options: [" + $envHash.Values + "]"
    Write-Host -ForegroundColor Magenta $msg
    LogAndWrite $msg
    exit
}

# Valid region list.
$regionHash = @{}
$regionHash.Add("EU", "EU")
$regionHash.Add("FL", "FL")
$regionHash.Add("MA", "MA")
$regionHash.Add("MW", "MW")
$regionHash.Add("NA", "NA")
$regionHash.Add("NC", "NC")
$regionHash.Add("NE", "NE")
$regionHash.Add("PN", "PN")
$regionHash.Add("RM", "RM")
$regionHash.Add("SO", "SO")
$regionHash.Add("SP", "SP")
$regionHash.Add("SW", "SW")
$regionHash.Add("TS", "TS")

# Validate region.
if(-not ($regionHash.Values -like $regionParam)){
    $msg = "ERROR [CANNOT CONTINUE]: The specified region '" + $regionParam + "' is not one of the following valid options: [" + ($regionHash.Values | sort) + "]"
    Write-Host -ForegroundColor Magenta $msg
    LogAndWrite $msg
    exit
}

# Validate date.
# YEAR
if(-not ([int]$dateParam.Substring(0, 4) -gt 2000 -and [int]$dateParam.Substring(0, 4) -lt 2100)){
    $msg = "ERROR [CANNOT CONTINUE]: The year (YYYY) value '" + $dateParam.Substring(0, 4) + "' in the specified date parameter '" + $dateParam + "' must be in the range 2000-2099.  Date format: YYYYMMDD"
    Write-Host -ForegroundColor Magenta $msg
    LogAndWrite $msg
    exit
}
# MONTH
if(-not ([int]$dateParam.Substring(4, 2) -gt 0 -and [int]$dateParam.Substring(4, 2) -lt 13)){
    $msg = "ERROR [CANNOT CONTINUE]: The month (MM) value '" + $dateParam.Substring(4, 2) + "' in the specified date parameter '" + $dateParam + "' must be in the range 1-12.  Date format: YYYYMMDD"
    Write-Host -ForegroundColor Magenta $msg
    LogAndWrite $msg
    exit
}
# YEAR
if(-not ([int]$dateParam.Substring(6, 2) -gt 0 -and [int]$dateParam.Substring(6, 2) -lt 32)){
    $msg = "ERROR [CANNOT CONTINUE]: The day (DD) value '" + $dateParam.Substring(6, 2) + "' in the specified date parameter '" + $dateParam + "' must be in the range 1-31.  Date format: YYYYMMDD"
    Write-Host -ForegroundColor Magenta $msg
    LogAndWrite $msg
    exit
}



####################################################################################################################################
####################################################################################################################################


"Identifying hash server..."

$hashSvrFile = "e:\data\psfiles\hash." + $envParam + ".svr"
if(-not (Test-Path $hashSvrFile)){
    $msg = "**ERROR**: Hash server file not found [" + $hashSvrFile + "]."
    LogAndWrite $msg
    exit
}

$hashSvrAlias = get-content -LiteralPath ($hashSvrFile)
$hashSvrName = ([System.Net.Dns]::GetHostByName($hashSvrAlias)).HostName

$msg = "Hash Server: " + $hashSvrAlias + " (" + $hashSvrName + ")"
LogAndWrite $msg

# Source PeopleSoft files against which a hash needs to be generated.
$psoftPath = "e:\Data\PSFiles\" + $regionParam + "\"
$hashFileExtention = ".hash"

$psoftFileDateFilter = "*" + $dateParam + "*"
$psoftFileLocalTempFolder = "e:\temp\PsFileHash"
$psoftFileTempFolder = "PsFileHash"
# Processing folder needs to include the region we are processing so that when this script runs for different regions, it does not reprocess/overlap files.
$psoftFileLocalTempPath = $psoftFileLocalTempFolder + "\" + $regionParam
$psoftFileTempPath = $psoftFileTempFolder + "\" + $regionParam
# We are running from the job server, which contains the data files for which we need to generate a hash value,
# so this filter represents the local/source files we will be processing.
$psoftLocalFileFilter = $psoftPath + $jobType + $psoftFileDateFilter

# Destination folder local to the hash server where hash input files are copied.
$psoftDestFolder = "\\" + $hashSvrName + "\" + $psoftFileTempPath + "\"
if(-not (Test-Path $psoftDestFolder)){
    "Creating temp folder on hash server [" + $psoftDestFolder + "]..."
    New-Item -ItemType Directory -Path $psoftDestFolder
}

"Checking for local data files to process [" + $psoftLocalFileFilter + "]..."
if(-not (Test-Path $psoftLocalFileFilter -Exclude ("*" + $hashFileExtention))){
    $msg = "**WARNING**: No data files found [" + $psoftLocalFileFilter + "]."
    LogAndWrite $msg
    exit
}

# Identify target files (make sure to exclude any hash-value files themselves, in case this is run multiple times in one day).
"Identifying data files for which hash values have already been generated..."
$psoftLocalFileListCheck = Get-ChildItem -Path $psoftLocalFileFilter -Exclude ("*" + $hashFileExtention)
$psoftLocalFileListOk = @()
foreach($psoftFile in @($psoftLocalFileListCheck)){
    $existingHashFile = $psoftFile.FullName + $hashFileExtention
    # If hash file is found for the current data file, we do not add that file to the processing (Ok) list.
    if(Test-Path ($existingHashFile)){
        # Get contents of file so that we can decide to still generate the hash if the file is empty.
        $existingHashFileText = Get-Content -LiteralPath $existingHashFile
        if($existingHashFileText){
            $msg = "FILE SKIPPED: Hash exists for [" + $psoftFile.Name + "], value: " + $existingHashFileText
            LogAndWrite $msg
        } else {
            $psoftLocalFileListOk += $psoftFile
            $msg = "Hash file exists for [" + $psoftFile.Name + "] but is empty so hash will be written."
            LogAndWrite $msg
        }
    } else {
        $psoftLocalFileListOk += $psoftFile
        $msg = "Hash needed for [" + $psoftFile.Name + "]"
        LogAndWrite $msg
    }
}

$hashSvrPsoftFileFilter = $psoftDestFolder + $jobType + $psoftFileDateFilter
"Removing any data files for target date from hash server..."
Remove-Item -Path ($hashSvrPsoftFileFilter) -Verbose

if($psoftLocalFileListOk.Count -eq 0){
    $msg = "**WARNING**: All data files already have a hash."
    LogAndWrite $msg
    exit
}

"Copying file(s) to hash server:"
$psoftLocalFileListOk
foreach($psoftFile in $psoftLocalFileListOk){
    Copy-Item -LiteralPath ($psoftFile) -Destination $psoftDestFolder -Verbose
}

# These are the files we've copied to our helper/hash server, which we now need to process.
$psoftFileList = Get-ChildItem -Path $hashSvrPsoftFileFilter
$msg = "Generating hash files for (" + @($psoftFileList).Count + ") target file(s) [" + $hashSvrPsoftFileFilter + "]..."
LogAndWrite $msg

# Loop through files and pass each file name to hash server to generate hash value.
$hashesCreatedCount = 0
foreach ($psoftFile in @($psoftFileList)){
    $hashSvrLocalPsoftFile = $psoftFileLocalTempPath + "\" + $psoftFile.name
    # Perform hash calculation on remote server.
    $hash = Invoke-Command -ComputerName $hashSvrName -ScriptBlock {
        param($targetPsoftFile)
        get-filehash -Algorithm MD5 -LiteralPath $targetPsoftFile
    } -ArgumentList $hashSvrLocalPsoftFile

    # Setup hash output file.
    $destHashFileName = $psoftFile.name + $hashFileExtention
    $destHashFilePath = $psoftPath + $destHashFileName
    $msg = "Writing hash file [" + $destHashFilePath + "], details: " + $hash
    LogAndWrite $msg
    # Write hash.
    Out-File -FilePath $destHashFilePath -InputObject $hash.hash
    if(Test-Path $destHashFilePath){
        $hashesCreatedCount++
    } else {
        $msg = "**WARNING**: Destination hash file not create/found [" + $destHashFilePath + "]."
        LogAndWrite $msg
    }
}

$psoftHashFileList = Get-ChildItem -Path ($psoftLocalFileFilter + $hashFileExtention)
$msg = "Created " + $hashesCreatedCount + " total hash-value file(s)."
LogAndWrite $msg
