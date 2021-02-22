<#
Created by: Tom Lux and based on archive functions by Ahmed Ilyas
Dec, 2020

Update History:
[Tom Lux, 2021-01-29]: Added handling archiveLocationProvided and destArchiveFolder input parameters in Archive-FilesByMonth.
[Tom Lux, 2021-02-12]: Added retry of deleting raw files (after successful archive) if any are left behind (tracked using $delOk var).
[Tom Lux, 2021-02-18]: Changed use of Compress-Archive command to be called separately for each file, rather than receiving the full target file list
    via the pipeline "|" method.  This saves significant RAM if the target files are large (such as for PAM Prod web servers).  This may have also, somehow,
    resolved the PAM IIS log issue where some of the files could not be deleted after being archived successfully.  This wasn't completely tested in Prod because
    I do not know if DataDog re-uploads full IIS logs if I restore them to the IIS log folder, so we will wait until next month's run to see if the delete-failure issue is resolved.
    Also added code but commented-out for restarting the DataDog agent, as it seems to keep file handles open (blocking delete of the files), but insufficient perms means we will postpone such actions.
[NOTE] A design opportunity is to first move the target file to a temp location before zipping it, which would address the issue on PAM web servers
    (large IIS log files that cannot be deleted because "in use by another process"), because it would skip the archiving until it could move the file.
    What has happened on the PAM servers is the archiver process can zip the files, but then the delete fails after successful zip, which then means 
    the next pass will re-zip the file, adding it to the archive twice.  If we move the file first, this will only add file to the archive once and
    reruns should be immune to duplicates-in-zip-file issues.  If the previous update solves the delete issue, this move-before-archiving method is not needed.
#>

$InitVars = {
    $filesToAddToZip = @()
    $filesUnableToCompress = @()
}

$GenerateValidFiles = {
    function GenValidFiles($thePathAndFilter, [switch]$recurse, [switch]$readCheckEachFile)
    {
        "[" + (Get-Date) + "] " + "Selecting target files for filter [" + $thePathAndFilter + "], recurse=$recurse, readCheckEachFile=$readCheckEachFile"
        $files = Get-ChildItem -Path $thePathAndFilter -Recurse:$recurse
        foreach ($item in $files)
        {
            Try 
            {
                # Checking File Mode and Access
                if($readCheckEachFile){
                    $FileStream = [System.IO.File]::Open($item.FullName,'Open','Read')
                    if ($null -ne $FileStream)
                    {
                        $FileStream.Close()
                        $FileStream.Dispose()
                    }
                }
                $global:filesToAddToZip += $item
                "OK: " + $item.FullName
            }
            Catch 
            {
                $global:filesUnableToCompress += $item
                "BLOCKED: " + $item.FullName
            }
        }            
    }    
}


function Archive-FilesByMonth {
Param(
    $computerName,
    $targetPath,
    $filenameFilterLeftOfDate,
    $filenameFilterRightOfDate,
    $filenameDateFormat = "yyyyMM",
    $archiveID,
    [switch]$archiveLocationProvided,
    $destArchiveFolder,
    [switch]$addFolderToArchiveFileName,
    [switch]$deleteFilesAfterZip,
    $targetDate = [DateTime]::Today,
    [switch]$recurse,
    [switch]$whatIfMode
)
    $archiveName = "Archive-FilesByMonth"

    "[" + (get-date) + "] " + "Beginning $archiveName, params: computerName=$computerName, targetPath=$targetPath, leftFilter=$filenameFilterLeftOfDate, rightFilter=$filenameFilterRightOfDate, filenameDateFormat=$filenameDateFormat, archiveID=$archiveID, archiveLoc=$archiveLocationProvided, destArchiveFolder=$destArchiveFolder, delFiles=$deleteFilesAfterZip, targetDate=$targetDate, whatIf=$whatIfMode"

    $filenameDateFilter = $targetDate.ToString($filenameDateFormat)
    "[" + (get-date) + "] " + "Archiving for filename month filter: $($filenameDateFilter)"

    # Build filename-filter and zip paths.
    $fullFileFilter = "$targetPath\$filenameFilterLeftOfDate" + $filenameDateFilter + $filenameFilterRightOfDate
    
    # Lookup hostname because PS may not like DNS alias for remote command session.
    $remoteHost = [System.Net.Dns]::GetHostByName($computerName).HostName
    $hostDisplayInfo = "target server: $computerName - $remoteHost"
    "[" + (get-date) + "] " + "Opening remote session, " + $hostDisplayInfo

    $remoteSession = New-PSSession -ComputerName $remoteHost
    Invoke-Command -Session $remoteSession -ScriptBlock $InitVars
    Invoke-Command -Session $remoteSession -ScriptBlock $GenerateValidFiles

    $remoteScriptToExecute = {
        $archiveID = $args[0]
        $filenameDateFilter = $args[1]
        $fullFileFilter = $args[2]
        $archiveLocationProvided = $args[3]
        $destArchiveFolder = $args[4]
        $addFolderToArchiveFileName = $args[5]
        $deleteFilesAfterZip = $args[6]
        $recurse = $args[7]
        $whatIfMode = $args[8]

        GenValidFiles -thePathAndFilter $fullFileFilter -recurse:$recurse
        # Make sure we have something to archive.
        if ($global:filesToAddToZip.Count -gt 0){
            "Total files to archive: " + $global:filesToAddToZip.Count
            # Whether recursing or not, we build mapping of all folders with list of files to zip in each.
            $zipFolders=@{}
            # Loop through each file.
            foreach($file in $global:filesToAddToZip){
                if($zipFolders.ContainsKey($file.DirectoryName)){
                    # Get file-list for folder key and add file.
                    $zipFolders.($file.DirectoryName) += $file
                } else {
                    # Add new folder ref and initial file list of current file.
                    $zipFolders.add($file.DirectoryName, @($file))
                }
            }

            <# 
            ************* The runtime user isn't an Admin on the PAM servers, which is why this DataDog-Agent restart was written, but it cannot be used at this point, due to lack of permissions to perform this service restart.
            if(-not $whatIfMode){
                $ddAgentExe = "$env:ProgramFiles\Datadog\Datadog Agent\embedded\agent.exe"
                if(Test-Path $ddAgentExe){ # Make sure agent executable exists.
                    # The DataDog agent can keep file handles open, which prevents the deletion of some files after they are successfully zipped, so we restart the DD-agent service.
                    "[" + (Get-Date) + "] " + "Restarting DataDog agent to clear hung file handles."
                    .$ddAgentExe restart-service
                } else {
                    "[" + (Get-Date) + "] " + "DataDog agent not found: [$ddAgentExe]."
                }
            }
            #>

            # Loop through each folder and zip each set of files.
            foreach($folder in ($zipFolders.Keys | sort)){
                if($addFolderToArchiveFileName){ $folderNameRef = $zipFolders.$folder[0].directory.name + " " } # Grab dir name ONLY from first file in list for this folder.
                else { $folderNameRef = "" }
                # Dest zip-file path/folder will be target folder, unless an alternate dest was provided, which will be set below.
                $zipPath = $folder
                # Build path to target zip file, either to current folder or the destination passed in.
                if($archiveLocationProvided){ $zipPath = $destArchiveFolder }
                $destZipFile = "$zipPath\$archiveID $folderNameRef$filenameDateFilter.zip"
                "[" + (Get-Date) + "] " + "Zipping " + $zipFolders.$folder.Count + " files to [" + $destZipFile + "]"
                foreach($file in $zipFolders.$folder){
                    # Does it consume less memory to not pipeline all files to the compress-archive command?  YES!  Tom confirmed this.
                    # For example: A run on a PAM web server, with over 7GB of IIS logs for a month, took ~9GB of RAM while it was running with full list of files passed via pipeline method, but took <1GB after this code change.
                    Compress-Archive -Path $file -DestinationPath $destZipFile -Update -Verbose -WhatIf:$whatIfMode
                }
                if ($deleteFilesAfterZip -eq $true){
                    # Only delete raw source files if expected-list/filtered file count matches the number of files in the dest zip/archive file.
                    $zipFile = [io.compression.zipfile]::OpenRead($destZipFile)
                    if($zipFolders.$folder.Count -eq $zipFile.Entries.Count){
                        "[" + (Get-Date) + "] " + "Archived and expected file counts MATCH. Deleting source files after zip."
                        # Delete source files.
                        $delOk = $true
                        foreach($file in $zipFolders.$folder){
                            # We make the full pass through all files to try to delete, so we'll just set a flag and retry later.
                            Remove-Item -LiteralPath $file -Force -Verbose -WhatIf:$whatIfMode
                            if(-not $whatIfMode){
                                if(Test-Path $file){ $delOk = $false } # If file still exists, the DEL failed.
                            }
                        }
                        if(-not $delOk){
                            $delaySeconds = 10
                            "[" + (Get-Date) + "] " + "[WARNING] Delete of source file(s) failed.  Pausing $delaySeconds seconds, then retrying delete."
                            Start-Sleep -s $delaySeconds # Short pause before delete retry of current set of files.
                            foreach($file in $zipFolders.$folder){
                                if(Test-Path $file){ Remove-Item -LiteralPath $file -Force -Verbose -WhatIf:$whatIfMode }
                            }
                        }
                    } else {
                        "[" + (Get-Date) + "] " + "**ERROR**: Source files will not be deleted. The filtered file count [" + $zipFolders.$folder.Count + "] does not match file count [" + $zipFile.Entries.Count + "] in archive file [$destZipFile]"
                    }
                } else { "[Delete files after zip DISABLED]" }
            } # foreach folder
        } else {
            "[" + (Get-Date) + "] " + "-- NO FILES FOUND TO ARCHIVE --"
        }
        $global:filesToAddToZip = @()
        $global:filesUnableToCompress = @()
    }
    Invoke-Command -Session $remoteSession -ArgumentList $archiveID, $filenameDateFilter, $fullFileFilter, $archiveLocationProvided, $destArchiveFolder, $addFolderToArchiveFileName, $deleteFilesAfterZip, $recurse, $whatIfMode -ScriptBlock $remoteScriptToExecute
    
    Remove-PSSession $remoteSession
    "[" + (get-date) + "] " + "Finished $archiveName - $archiveID, " + $hostDisplayInfo
}
