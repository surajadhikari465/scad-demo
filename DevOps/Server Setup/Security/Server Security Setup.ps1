$gitRootFolder = "c:\tlux\dev\git\Icon"
$securityFolder = "$gitRootFolder\DevOps\Server Setup\Security"

$outputBreak = "---------------------------------------------------------------------"

########################################################################################################################################

$svrListInFile = "$securityFolder\ServerList.in.tsv"
if(!(Test-Path $svrListInFile)){
    Write-Host -ForegroundColor Magenta "**ERROR** -- Server list file not found: [$svrListInFile]"
    Read-Host "Press <ENTER> to exit..."
    return
}
$svrListInTxt = Get-Content -Encoding String $svrListInFile
$svrList = $svrListInTxt.split("`n") # Each line in input file.


$securityInFile = "$securityFolder\ServerSec.in.tsv"
if(!(Test-Path $securityInFile)){
    Write-Host -ForegroundColor Magenta "**ERROR** -- Server security file not found: [$securityInFile]"
    Read-Host "Press <ENTER> to exit..."
    return
}
$securityInTxt = Get-Content -Encoding String $securityInFile
$securityList = $securityInTxt.split("`n") # Each line in input file.


$fileOrFolderPermsInFile = "$securityFolder\FileOrFolderPerms.in.tsv"
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

Read-Host "Press <ENTER> to continue..."


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
            param($rGroupName, $rUserName) # "r" prefix identifies "remote" variables.

                Add-LocalGroupMember -Verbose -Group $rGroupName -Member $rUserName #-whatif

            } -ArgumentList $groupName, $userName
        }
    }

}


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
            param($rFileOrFolder, $rUser, $rPerms, $rInheritSettings) # "r" prefix identifies "remote" variables.
                if(-not(Test-Path $rFileOrFolder)){
                    # File or Directory
                    $objType = "File"
                    # If it ends with backslash, we'll make folder.
                    if($rFileOrFolder -like "*\"){
                        $objType = "Directory"
                    }
                    "Creating $objType to apply permissions: $rFileOrFolder" 
                    New-Item -Verbose -ItemType $objType
                }

                # https://win32.io/posts/How-To-Set-Perms-With-Powershell
                # Hard-coding to none.
                $propogationSettings = "None" #Usually set to none but can setup rules that only apply to children.
                # Hard-coding to allow.
                $ruleType = "Allow" #Allow or Deny.

                $acl = Get-Acl $rFileOrFolder
                $perm = $rUser, $rPerms, $rInheritSettings, $propogationSettings, $ruleType
                $rule = New-Object -TypeName System.Security.AccessControl.FileSystemAccessRule -ArgumentList $perm
                $acl.SetAccessRule($rule)
                $acl | Set-Acl -Path $rFileOrFolder

            } -ArgumentList $fileOrFolder, $user, $perms, $inheritSettings
        }
    }

}


