<#
Created by: Tom Lux and based on archive functions by Ahmed Ilyas
Dec, 2020
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
    [switch]$addFolderToArchiveFileName,
    [switch]$deleteFilesAfterZip,
    $targetDate = [DateTime]::Today,
    [switch]$recurse,
    [switch]$whatIfMode
)
    $archiveName = "Archive-FilesByMonth"

    "[" + (get-date) + "] " + "Beginning $archiveName, params: computerName=$computerName, targetPath=$targetPath, leftFilter=$filenameFilterLeftOfDate, rightFilter=$filenameFilterRightOfDate, filenameDateFormat=$filenameDateFormat, archiveID=$archiveID, delFiles=$deleteFilesAfterZip, targetDate=$targetDate, whatIf=$whatIfMode"

    $filenameDateFilter = $targetDate.ToString($filenameDateFormat)
    "[" + (get-date) + "] " + "Archiving for filename month filter: $($filenameDateFilter)"

    # Build filename-filter and zip paths.
    $fullFileFilter = "$targetPath\$filenameFilterLeftOfDate" + $filenameDateFilter + $filenameFilterRightOfDate
    $destZipFile = "$targetPath\$archiveID $filenameDateFilter.zip"
    
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
        $destZipFile = $args[3]
        $addFolderToArchiveFileName = $args[4]
        $deleteFilesAfterZip = $args[5]
        $recurse = $args[6]
        $whatIfMode = $args[7]

        GenValidFiles -thePathAndFilter $fullFileFilter -recurse:$recurse
        # Make sure we have something to archive.
        if ($global:filesToAddToZip.Count -gt 0){
            "Total files to archive: " + $global:filesToAddToZip.Count
            # Whether recursing or not, we build mapping of all folders with list of files to zip in each.
            $zipFolders=@{}
            # Loop through each file
            foreach($file in $global:filesToAddToZip){
                if($zipFolders.ContainsKey($file.DirectoryName)){
                    # Get file-list for folder key and add file.
                    $zipFolders.($file.DirectoryName) += $file
                } else {
                    # Add new folder ref and initial file list of current file.
                    $zipFolders.add($file.DirectoryName, @($file))
                }
            }

            # Loop through each folder and zip each set of files.
            foreach($folder in ($zipFolders.Keys | sort)){
                # Build path to target zip file.
                if($addFolderToArchiveFileName){ $folderNameRef = $zipFolders.$folder[0].directory.name + " " } # Grab dir name ONLY from first file in list for this folder.
                else { $folderNameRef = "" }
                $destZipFile = "$folder\$archiveID $folderNameRef$filenameDateFilter.zip"
                "[" + (Get-Date) + "] " + "Zipping " + $zipFolders.$folder.Count + " files to [" + $destZipFile + "]"
                $zipFolders.$folder | Compress-Archive -DestinationPath $destZipFile -Update -Verbose -WhatIf:$whatIfMode
                        
                if ($deleteFilesAfterZip -eq $true){
                    # Only delete raw source files if expected-list/filtered file count matches the number of files in the dest zip/archive file.
                    $zipFile = [io.compression.zipfile]::OpenRead($destZipFile)
                    if($zipFolders.$folder.Count -eq $zipFile.Entries.Count){
                        "[" + (Get-Date) + "] " + "Archived and expected file counts MATCH. Deleting source files after zip."
                        # Delete source files
                        $zipFolders.$folder | Remove-Item -Force -Verbose -WhatIf:$whatIfMode
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
    Invoke-Command -Session $remoteSession -ArgumentList $archiveID, $filenameDateFilter, $fullFileFilter, $destZipFile, $addFolderToArchiveFileName, $deleteFilesAfterZip, $recurse, $whatIfMode -ScriptBlock $remoteScriptToExecute
    
    Remove-PSSession $remoteSession
    "[" + (get-date) + "] " + "Finished $archiveName - $archiveID, " + $hostDisplayInfo
}
