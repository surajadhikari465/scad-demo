#######################################################################################
#######################################################################################
# IRMA Apps Server-Security Setup
# Tom Lux
# Late 2018
#######################################################################################
#######################################################################################

# Creates a set of app pools and sites based on input files.

<#
HISTORY:
2019.02.25 - Tom Lux - Standardized to determine its own script folder, load functions script, and create transcript log.
#>


#######################################################################################

$scriptFullPath = $MyInvocation.MyCommand.Definition
$scriptFileObj = new-object system.io.fileinfo $MyInvocation.MyCommand.Definition
$scriptFolder = $scriptFileObj.directoryname
$outputBreak = "---------------------------------------------------------------------"

########################################################################################################################################

">>> Loading functions..."
">>> Trying common location...

"
$fnScript = "$scriptFolder\..\..\common\ps\Functions.ps1"

if (-not (Test-Path $fnScript)) {

"
******************************************************************
Cannot Continue -- Expected Functions Script Not Found
--> '" + $fnScript + "'

Ensure this script exists in the relative common folder and try again.
******************************************************************
"
    Read-Host "Press enter to exit..."
    return
}

# Functions file must be in the assumed 'common' directory.
. $fnScript

########################################################################################################################################

$outputBreak
$logFile = "\\irmaprdfile2\e$\devops\server setup\logs\" + $scriptFileObj.Name + "." + (Get-Date -Format "yyyyMMdd.HHmmss") + ".log"
Start-Transcript -Path $logFile -Append

$prompt = "Enable preview mode?"
"
[[ PROMPT ]]
----------------------------
$prompt
----------------------------
"
$previewMode = AskYesNo -MainPrompt $prompt
# Force preview mode, if variable is null or not boolean (so IF statements don't evaluate to TRUE just because the variable has some value).
if($previewMode -eq $null){ $previewMode = $true }
if($previewMode.GetType() -ne [bool]){ $previewMode = $true }

$previewMsg = "
*********************************
>>>>>>>>>>>> PREVIEW MODE
*********************************
"

if($previewMode){
    Write-Host -ForegroundColor Cyan $previewMsg
} else {
    Write-Host -ForegroundColor Magenta "** Running in UPDATE Mode ** -- Changes will be made to target servers!"
}

########################################################################################################################################

$svrListInFile = "$scriptFolder\ServerList.in.tsv"
if(!(Test-Path $svrListInFile)){
    Write-Host -ForegroundColor Magenta "**ERROR** -- Server list file not found: [$svrListInFile]"
    Read-Host "Press <ENTER> to exit..."
    return
}
$svrListInTxt = Get-Content -Encoding String $svrListInFile
$svrList = $svrListInTxt.split("`n") # Each line in input file.


$securityInFile = "$scriptFolder\ServerSec.in.tsv"
if(!(Test-Path $securityInFile)){
    Write-Host -ForegroundColor Magenta "**ERROR** -- Server security file not found: [$securityInFile]"
    Read-Host "Press <ENTER> to exit..."
    return
}
$securityInTxt = Get-Content -Encoding String $securityInFile
$securityList = $securityInTxt.split("`n") # Each line in input file.


$fileOrFolderPermsInFile = "$scriptFolder\FileOrFolderPerms.in.tsv"
if(!(Test-Path $fileOrFolderPermsInFile)){
    Write-Host -ForegroundColor Magenta "**ERROR** -- File/folder perms file not found: [$fileOrFolderPermsInFile]"
    Read-Host "Press <ENTER> to exit..."
    return
}
$fileOrFolderPermsInTxt = Get-Content -Encoding String $fileOrFolderPermsInFile
$fileOrFolderPerms = $fileOrFolderPermsInTxt.split("`n") # Each line in input file.

#######################################################################################

# Parse server list into groups.
$svrTypeHash = @{}
foreach($svr in $svrList){
    $svrAttr = $svr.split("`t")
    $svrType = $svrAttr[0].toupper()
    $svrName = $svrAttr[1]
    if($svrTypeHash.ContainsKey($svrType)){
        $svrTypeHash.$svrType += $svrName
    } else {
        $svrTypeHash.Add($svrType, @($svrName))
    }
}


$outputBreak
"Loaded " + $svrList.count + " server definitions."
"Loaded " + $securityList.count + " security definitions."
"Loaded " + $fileOrFolderPerms.count + " file/folder-perms definitions."
"Target Servers:"
$svrTypeHash


$outputBreak
Write-Host -ForegroundColor Yellow "Next: Server Security"
If (-not (UserWantsToContinue))
{ ExitProcess }
$outputBreak


#######################################################################################
# Server Security
#######################################################################################
foreach($securityDef in $securityList){
    $values = $securityDef.split("`t") # Each field in a line.
    $groupName = $values[0]
    $userName = $values[1]
    $svrTypeList = $values[2].split(" ")

    $outputBreak
    "User [$userName] to Group [$groupName]"
    foreach($svrType in $svrTypeList) {
        "-- $svrType Servers --"
        foreach($svr in $svrTypeHash.$svrType){
            "[Applying security updates to server: $svr]"

            Invoke-Command -ComputerName ([System.Net.Dns]::GetHostByName($svr)).HostName -ScriptBlock {
            param($rGroupName, $rUserName, $rPreviewMode) # "r" prefix identifies "remote" variables.
                if($rPreviewMode){
                    Add-LocalGroupMember -Verbose -Group $rGroupName -Member $rUserName -WhatIf
                } else {
                    Add-LocalGroupMember -Verbose -Group $rGroupName -Member $rUserName
                }

            } -ArgumentList $groupName, $userName, $previewMode
        }
    }

}

$outputBreak
Write-Host -ForegroundColor Yellow "Next: File & Folder Perms"
If (-not (UserWantsToContinue))
{ ExitProcess }
$outputBreak

#######################################################################################
# File/Folder Perms
#######################################################################################
foreach($fileOrFolderPerm in $fileOrFolderPerms) {
    $values = $fileOrFolderPerm.split("`t") # Each field in a line.
    $svrTypeList = $values[0].split(" ")
    $fileOrFolder = $values[1]
    $user = $values[2]
    $perms = $values[3]

    $inherit = $values[4]
    $inheritSettings = "none"
    if($inherit -like "y"){
        $inheritSettings = "Containerinherit, ObjectInherit"
    }

    $outputBreak
    "File/Folder [$fileOrFolder] to User [$user]"
    foreach($svrType in $svrTypeList) {
        "-- $svrType Servers --"
        foreach($svr in $svrTypeHash.$svrType){
            "[Applying file/folder perms to server: $svr]"

            Invoke-Command -ComputerName ([System.Net.Dns]::GetHostByName($svr)).HostName -ScriptBlock {
            param($rFileOrFolder, $rUser, $rPerms, $rInheritSettings, $rPreviewMode) # "r" prefix identifies "remote" variables.
                if(-not(Test-Path $rFileOrFolder)){
                    # File or Directory
                    $objType = "File"
                    # If it ends with backslash, we'll make folder.
                    if($rFileOrFolder -like "*\"){
                        $objType = "Directory"
                    }
                    "Creating $objType [$rFileOrFolder] to apply permissions: $rPerms" 
                    if($rPreviewMode){
                        New-Item -Verbose -ItemType $objType -Path $rFileOrFolder -WhatIf
                    } else {
                        New-Item -Verbose -ItemType $objType -Path $rFileOrFolder
                    }
                }

                if($rPerms -like "new-folder"){
                    "Returning after create-only for [$rFileOrFolder]"
                    return
                }

                "[Setting ACL]"
                # https://win32.io/posts/How-To-Set-Perms-With-Powershell
                # Hard-coding to none.
                $propogationSettings = "None" #Usually set to none but can setup rules that only apply to children.
                # Hard-coding to allow.
                $ruleType = "Allow" #Allow or Deny.

                $perm = $rUser, $rPerms, $rInheritSettings, $propogationSettings, $ruleType
                if($rPreviewMode){
                    "CMD --> Get-Acl $rFileOrFolder"
                    "CMD --> New-Object -TypeName System.Security.AccessControl.FileSystemAccessRule -ArgumentList $perm"
                    "CMD --> acl.SetAccessRule($rule)"
                } else {
                    $acl = Get-Acl $rFileOrFolder
                    $rule = New-Object -TypeName System.Security.AccessControl.FileSystemAccessRule -ArgumentList $perm
                    $acl.SetAccessRule($rule)
                    $acl | Set-Acl -Path $rFileOrFolder
                }

            } -ArgumentList $fileOrFolder, $user, $perms, $inheritSettings, $previewMode
        }
    }

}


$outputBreak
"**SCRIPT COMPLETE**"
$outputBreak
Read-Host "Press <ENTER> to close log exit..."
ExitProcess
