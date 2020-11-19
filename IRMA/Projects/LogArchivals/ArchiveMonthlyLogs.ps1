<#
Created by: Ahmed Ilyas
Date: Sept 2020
Purpose: This script has functions which are executed on a remote computer that archives files for a given date filter and path
and zips up those files, finally deleting the loose files that were placed in the zip from the parent folder (optional).
It cleverly checks to see if the file it has found as part of a filter, is currently locked or in use by an application
so it will not fall over when zipping - so at least we are zipping as many of the files as possible without it being an
"all or nothing" scenario.
#>


Set-ExecutionPolicy -ExecutionPolicy Unrestricted

$currentDateStr = ([DateTime]::Today).ToString("yyyy_MM_dd")
 
$InitVars = {
    $filesToAddToZip = @()
    $filesUnableToCompress = @()
}

$GenerateValidFiles = {
    function GenValidFiles($thePathAndFilter) 
    {
        $files = Get-ChildItem -Path $thePathAndFilter
        foreach ($item in $files)
        {
            Try 
            {
                Write-Host $item
                # Checking File Mode and Access
                $FileStream = [System.IO.File]::Open($item.FullName,'Open','Read')
                if ($null -ne $FileStream)
                {
                    $FileStream.Close()
                    $FileStream.Dispose()
                    $global:filesToAddToZip += $item
                }
            }
            Catch 
            {
                $global:filesUnableToCompress += $item
            }
        }            
    }    
}

$HelperGetFirstOfMonth = {
    function GetFirstOfTheMonthDate ([DateTime]$theDateOfInterest)
    {
        $year = $theDateOfInterest.Year
        $month = $theDateOfInterest.Month

        # create a new DateTime object set to the first day of a given month and year
        $startOfMonth = Get-Date -Year $year -Month $month -Day 1 -Hour 0 -Minute 0 -Second 0 -Millisecond 0

        return $startOfMonth
    }
}

$HelperGetLastOfMonth = {
    function GetLastOfTheMonthDate([DateTime]$theDateOfInterest)
    {
    
        # add a month and subtract the smallest possible time unit
        $endOfMonth = ($theDateOfInterest).AddMonths(1).AddTicks(-1)

        return $endOfMonth
    }
}

function GetFirstOfTheMonthDate { Param([DateTime]$theDateOfInterest)

    $year = $theDateOfInterest.Year
    $month = $theDateOfInterest.Month

    # create a new DateTime object set to the first day of a given month and year
    $startOfMonth = Get-Date -Year $year -Month $month -Day 1 -Hour 0 -Minute 0 -Second 0 -Millisecond 0

    return $startOfMonth
}

function GetLastOfTheMonthDate { Param([DateTime]$theDateOfInterest)
    
    # add a month and subtract the smallest possible time unit
    $endOfMonth = ($theDateOfInterest).AddMonths(1).AddTicks(-1)

    return $endOfMonth
}

function WriteLog { param($line)
    try
    {
        $currentLogDateTime = ([DateTime]::Now).ToString("yyyy-MM-dd HH:mm:ss")
        Add-Content "c:\JobOutput_$($currentDateStr).log" "$($currentLogDateTime) $($line)" -Force
    }
    catch
    {
        Write-Warning "Unable to write log to file"
        Write-Warning $_
    }
}

function CopyAndArchive_Data_DVO{ Param($remoteComputerToConnectTo, $srcServer, $destServer, $region, $deleteFilesAfterZip) 

    WriteLog -line "CopyAndArchive_Data_DVO with params: $($srcServer), $($destServer), $($region)"

    $interestedInDateFrom = GetFirstOfTheMonthDate -theDateOfInterest ([DateTime]::Today).AddMonths(-1)
    $strMonthFilter = $interestedInDateFrom.ToString("yyyyMM")
    WriteLog -line "Month filter: $($strMonthFilter)"
     
    $srcPath = "$srcServer\Data\DVO_Orders\$region\backup" 
    $destPath = "$destServer\Data\DVO_Orders\$region\backup"
         
    #get me files for all of last month. Filter on the filename where the yyyyMM matches. We need to execute remotely on that computer too
    
    WriteLog -line "Remote invoke cmd to: $($remoteComputerToConnectTo)"
    
    $remoteSession = New-PSSession -ComputerName $remoteComputerToConnectTo    
    Invoke-Command -Session $remoteSession -ScriptBlock $InitVars
    Invoke-Command -Session $remoteSession -ScriptBlock $GenerateValidFiles
    
    $remoteScriptToExecute = { 
        $region = $args[0]
        $srcPath = $args[1]
        $destPath = $args[2]
        $interestedInDateFrom = $args[3]
        $strMonthFilter = $args[4] 
        $deleteFilesAfterZip = $args[5]
        New-Item -ItemType Directory -Force -Path $destPath 

        GenValidFiles -thePathAndFilter "$srcPath\$($region)OrderExport_$($strMonthFilter)*.txt"
        
        if ($($global:filesToAddToZip).Count -gt 0)
        {            
            $global:filesToAddToZip | Compress-Archive -DestinationPath "$destPath\$($region)OrderExport_$($strMonthFilter).zip" -Update -Verbose 
            if ($deleteFilesAfterZip -eq $true)
            {
                #Finally delete the files:
                $global:filesToAddToZip | Remove-Item -Force  
            }               
        }
        
        $global:filesToAddToZip = @()
        $global:filesUnableToCompress = @()
    }
    Invoke-Command -Session $remoteSession -ArgumentList $region, $srcPath, $destPath, $interestedInDateFrom, $strMonthFilter, $deleteFilesAfterZip -ScriptBlock $remoteScriptToExecute

    WriteLog -line "Finished Remote invoke cmd to: $($remoteComputerToConnectTo)" 
    Disconnect-PSSession $remoteSession   
} 

function CopyAndArchive_IIS_Logs{ param($remoteComputerToConnectTo, $srcPath, $deleteFilesAfterZip)
    
    <#Go through each W3SVC folder found in srcPath and archive previous months files#>
    
    WriteLog -line "CopyAndArchive_IIS_Logs with params: $($srcPath)"
        
    WriteLog -line "Remote invoke cmd to: $($remoteComputerToConnectTo)"
    $remoteSession = New-PSSession -ComputerName $remoteComputerToConnectTo    
    Invoke-Command -Session $remoteSession -ScriptBlock $InitVars
    Invoke-Command -Session $remoteSession -ScriptBlock $GenerateValidFiles
    Invoke-Command -Session $remoteSession -ScriptBlock $HelperGetFirstOfMonth
    Invoke-Command -Session $remoteSession -ScriptBlock $HelperGetLastOfMonth

    $remoteScriptToExecute = { 
        $srcPath = $args[0]
        $strMonthFilter = $args[1]
        $deleteFilesAfterZip = $args[2]
        $baseFolder = "InetPub\logs\LogFiles"
        $fullPath = "$src\$baseFolder"
        $interestedDateTo = Get-Date
        $firstOfPreviousMonth = GetFirstOfTheMonthDate ([DateTime]::Today).AddMonths(-1)
        $lastOfPreviousMonth = GetLastOfTheMonthDate $firstOfPreviousMonth

        # get a list of these folders since there could be more than 1 website in IIS and each folder is a website for logs.
        $folders = Get-ChildItem -Directory -Path $fullPath
        foreach($currentFolderPath in $folders)
        {
            $currentFolderName = $($currentFolderPath.FullName | Split-Path -Leaf)
       
            #Filter for previous month only.
            $filteredFiles = Get-ChildItem -Path "$currentFolderPath\*.txt" | Where-Object { $_.LastWriteTime.Date -ge $($firstOfPreviousMonth).Date -AND $_.LastWriteTime.Date -le $($lastOfPreviousMonth)  } | Sort LastWriteTime -Descending | Select-Object LastWriteTime,Name,FullName  
            foreach($currentFilteredFile in $filteredFiles)
            {
                #Test that the file is valid in that there is no process holding the file/in use otherwise the zip will fail.
                GenValidFiles -thePathAndFilter $currentFilteredFile
                if ($($global:filesToAddToZip).Count -gt 0)
                {                    
                    $global:filesToAddToZip | Compress-Archive -DestinationPath "$currentFolderPath\$currentFolderName_($strMonthFilter).zip" -Update -Verbose 
                    
                    if ($deleteFilesAfterZip -eq $true)
                    {
                        #Finally delete the files:
                        $global:filesToAddToZip | Remove-Item -Force                      
                    }
        
                    $global:filesToAddToZip = @()
                    $global:filesUnableToCompress = @()
                }

            }                           
        }

        $global:filesToAddToZip = @()
        $global:filesUnableToCompress = @()
    }
    
    Invoke-Command -Session $remoteSession -ArgumentList $srcPath, $strMonthFilter, $deleteFilesAfterZip -ScriptBlock $remoteScriptToExecute
    
    WriteLog -line "Finished Remote invoke cmd to: $($remoteComputerToConnectTo)" 
    Disconnect-PSSession $remoteSession     
}

function CopyAndArchive_PSOrder{ Param($remoteComputerToConnectTo, $srcServer, $destServer, $region, $deleteFilesAfterZip) 

    WriteLog -line "CopyAndArchive_PSOrder with params: $($srcServer), $($destServer), $($region)"

    $interestedInDateFrom = GetFirstOfTheMonthDate -theDateOfInterest ([DateTime]::Today).AddMonths(-1)
    $strMonthFilter = $interestedInDateFrom.ToString("yyyyMM")
    WriteLog -line "Month filter: $($strMonthFilter)"
     
    $srcPath = "$srcServer\Data\PSFiles\$region" 
    $destPath = "$destServer\Data\PSFiles\$region"  
         
    #get me files for all of last month. Filter on the filename where the yyyyMM matches. We need to execute remotely on that computer too
    
    WriteLog -line "Remote invoke cmd to: $($remoteComputerToConnectTo)"
    $remoteSession = New-PSSession -ComputerName $remoteComputerToConnectTo    
    Invoke-Command -Session $remoteSession -ScriptBlock $InitVars
    Invoke-Command -Session $remoteSession -ScriptBlock $GenerateValidFiles

    $remoteScriptToExecute = { 
        $region = $args[0]
        $srcPath = $args[1]
        $destPath = $args[2]
        $interestedInDateFrom = $args[3]
        $strMonthFilter = $args[4]
        $deleteFilesAfterZip = $args[5]
        New-Item -ItemType Directory -Force -Path $destPath 

        GenValidFiles -thePathAndFilter "$srcPath\PSFile_$($region)_$($strMonthFilter)*.EDI"
        
        if ($($global:filesToAddToZip).Count -gt 0)
        {            
            $global:filesToAddToZip | Compress-Archive -DestinationPath "$destPath\PSFile_$($region)_($strMonthFilter).zip" -Update -Verbose 
            
            if ($deleteFilesAfterZip -eq $true)
            {
                #Finally delete the files:
                $global:filesToAddToZip | Remove-Item -Force    
            }             
        }
        
        $global:filesToAddToZip = @()
        $global:filesUnableToCompress = @()
    }
    Invoke-Command -Session $remoteSession -ArgumentList $region, $srcPath, $destPath, $interestedInDateFrom, $strMonthFilter, $deleteFilesAfterZip -ScriptBlock $remoteScriptToExecute
    
    WriteLog -line "Finished Remote invoke cmd to: $($remoteComputerToConnectTo)" 
    Disconnect-PSSession $remoteSession      
} 

function CopyAndArchive_PSCurrency{ Param($remoteComputerToConnectTo, $srcServer, $destServer, $region, $deleteFilesAfterZip) 

    WriteLog -line "CopyAndArchive_PSCurrency with params: $($srcServer), $($destServer), $($region)"

    $interestedInDateFrom = GetFirstOfTheMonthDate -theDateOfInterest ([DateTime]::Today).AddMonths(-1)
    $strMonth = "$($interestedInDateFrom.ToString('MM'))"
    $strYear = "$($interestedInDateFrom.ToString('yyyy'))"
    $strMonthFilter = "$($interestedInDateFrom.ToString('MM'))*$($interestedInDateFrom.ToString('yyyy'))"
    WriteLog -line "Month filter: $($strMonthFilter)"
     
    $srcPath = "$srcServer\Data\PSFiles\$region\CurrencyFeed\Archive" 
    $destPath = "$destServer\Data\PSFiles\$region\CurrencyFeed\Archive"
         
    #get me files for all of last month. Filter on the filename where the yyyyMM matches. We need to execute remotely on that computer too
    
    WriteLog -line "Remote invoke cmd to: $($remoteComputerToConnectTo)"
    
    $remoteSession = New-PSSession -ComputerName $remoteComputerToConnectTo
    Invoke-Command -Session $remoteSession -ScriptBlock $InitVars
    Invoke-Command -Session $remoteSession -ScriptBlock $GenerateValidFiles
    $remoteScriptToExecute = { 
        $region = $args[0]
        $srcPath = $args[1]
        $destPath = $args[2]
        $interestedInDateFrom = $args[3]
        $strMonthFilter = $args[4]
        $strMonth = $args[5]
        $strYear = $args[6] 
        $deleteFilesAfterZip = $args[7]
        New-Item -ItemType Directory -Force -Path $destPath 
        GenValidFiles -thePathAndFilter "$srcPath\rates$($strMonthFilter).txt"
        
        if ($($global:filesToAddToZip).Count -gt 0)
        {            
            $global:filesToAddToZip | Compress-Archive -DestinationPath "$destPath\$($region)rates_$($strYear)$($strMonth).zip" -Update -Verbose 
            if ($deleteFilesAfterZip -eq $true)
            {
                #Finally delete the files:
                $global:filesToAddToZip | Remove-Item -Force
            }                 
        }
        
        $global:filesToAddToZip = @()
        $global:filesUnableToCompress = @()
    }
    Invoke-Command -Session $remoteSession -ArgumentList $region, $srcPath, $destPath, $interestedInDateFrom, $strMonthFilter, $strMonth, $strYear, $deleteFilesAfterZip -ScriptBlock $remoteScriptToExecute


    WriteLog -line "Finished Remote invoke cmd to: $($remoteComputerToConnectTo)"  
    Disconnect-PSSession $remoteSession  
} 

function CopyAndArchive_HashLogs{ Param($remoteComputerToConnectTo, $srcServer, $destServer, $region, $deleteFilesAfterZip) 

    WriteLog -line "CopyAndArchive_HashLogs with params: $($srcServer), $($destServer)"

    $interestedInDateFrom = GetFirstOfTheMonthDate -theDateOfInterest ([DateTime]::Today).AddMonths(-1)
    $strMonthFilter = $interestedInDateFrom.ToString("MM")
    WriteLog -line "Month filter: $($strMonthFilter)"
     
    $srcPath = "$srcServer\ScheduledJobs\PeopleSoft File Hash Generator\Logs" 
    $destPath = "$destServer\ScheduledJobs\PeopleSoft File Hash Generator\Logs" 
         
    #get me files for all of last month. Filter on the filename where the yyyyMM matches. We need to execute remotely on that computer too
    
    WriteLog -line "Remote invoke cmd to: $($remoteComputerToConnectTo)"
    
    $remoteSession = New-PSSession -ComputerName $remoteComputerToConnectTo
    Invoke-Command -Session $remoteSession -ScriptBlock $InitVars
    Invoke-Command -Session $remoteSession -ScriptBlock $GenerateValidFiles
    $remoteScriptToExecute = { 
        $region = $args[0]
        $srcPath = $args[1]
        $destPath = $args[2]
        $interestedInDateFrom = $args[3]
        $strMonthFilter = $args[4]
        $deleteFilesAfterZip = $args[5]
        New-Item -ItemType Directory -Force -Path $destPath 

        GenValidFiles -thePathAndFilter "$srcPath\PeopleSoftFileHashGenerator.ps1.$($interestedInDateFrom.Year)$($strMonthFilter)*.PSFile.log"
        
        if ($($global:filesToAddToZip).Count -gt 0)
        {            
            $global:filesToAddToZip | Compress-Archive -DestinationPath "$destPath\PeopleSoftFileHashGenerator_PSFile_$($interestedInDateFrom.Year)$($strMonthFilter).zip" -Update -Verbose 
            if ($deleteFilesAfterZip -eq $true)
            {
                #Finally delete the files:
                $global:filesToAddToZip | Remove-Item -Force    
            }             
        }
        
        $global:filesToAddToZip = @()
        $global:filesUnableToCompress = @()
    }
    
    Invoke-Command -Session $remoteSession -ArgumentList $region, $srcPath, $destPath, $interestedInDateFrom, $strMonthFilter, $deleteFilesAfterZip -ScriptBlock $remoteScriptToExecute

    WriteLog -line "Finished Remote invoke cmd to: $($remoteComputerToConnectTo)" 
    Disconnect-PSSession $remoteSession     
}


function CopyAndArchive_Logs_PSFINTECHLogs{ Param($remoteComputerToConnectTo, $srcServer, $destServer, $deleteFilesAfterZip) 
  
    WriteLog -line "CopyAndArchive_Logs_PSFINTECHLogs with params: $($srcServer), $($destServer)"
 
    $srcPath = "$srcServer\Program Files (x86)\DataMonkey 2\Logs" 
    $destPath = "$destServer\Program Files (x86)\DataMonkey 2\Logs" 
    New-Item -ItemType Directory -Force -Path $destPath 
    $interestedInDateFrom = GetFirstOfTheMonthDate -theDateOfInterest ([DateTime]::Today).AddMonths(-1)
    $strMonthFilter = $interestedInDateFrom.ToString("MM")

    WriteLog -line "Remote invoke cmd to: $($remoteComputerToConnectTo)"
    
    $remoteSession = New-PSSession -ComputerName $remoteComputerToConnectTo
    Invoke-Command -Session $remoteSession -ScriptBlock $InitVars
    Invoke-Command -Session $remoteSession -ScriptBlock $GenerateValidFiles
    $remoteScriptToExecute = { 
        $srcPath = $args[0]
        $destPath = $args[1]
        $interestedInDateFrom = $args[2]
        $strMonthFilter = $args[3]
        $deleteFilesAfterZip = $args[4]
        New-Item -ItemType Directory -Force -Path $destPath 
        $files = Get-ChildItem -Path "$srcPath\*PSFINTECH_*.log" | ?{ $_.LastWriteTime.Year -eq $($interestedInDateFrom.Year) -and $_.LastWriteTime.Month -eq $($strMonthFilter) }
        $global:filesToAddToZip += $files
    
        
        if ($($global:filesToAddToZip).Count -gt 0)
        {            
            $global:filesToAddToZip | Compress-Archive -DestinationPath "$destPath\PSFINTECH_$($interestedInDateFrom.Year)$($strMonthFilter).zip" -Update -Verbose 
            if ($deleteFilesAfterZip -eq $true)
            {
                #Finally delete the files:
                $global:filesToAddToZip | Remove-Item -Force    
            }             
        }
        
        $global:filesToAddToZip = @()
        $global:filesUnableToCompress = @()
    }
    
    Invoke-Command -Session $remoteSession -ArgumentList $srcPath, $destPath, $interestedInDateFrom, $strMonthFilter, $deleteFilesAfterZip -ScriptBlock $remoteScriptToExecute

    WriteLog -line "Finished Remote invoke cmd to: $($remoteComputerToConnectTo)" 
    Disconnect-PSSession $remoteSession   
} 
 


#\\IRMADevWeb01\C$\inetpub\logs\LogFiles

#CopyAndArchive_Data_DVO -remoteComputerToConnectTo ([System.Net.Dns]::GetHostByName("IRMATest1Tidal01")).HostName -srcServer "E:\" -destServer "E:\" -region "FL" -deleteFilesAfterZip $false
#CopyAndArchive_PSOrder -remoteComputerToConnectTo ([System.Net.Dns]::GetHostByName("IRMATest1Tidal01")).HostName -srcServer "E:\" -destServer "E:\" -region "FL" -deleteFilesAfterZip $false
#CopyAndArchive_HashLogs -remoteComputerToConnectTo ([System.Net.Dns]::GetHostByName("IRMATest1Tidal01")).HostName -srcServer "E:\" -destServer "E:\" -region "FL" -deleteFilesAfterZip $false
#CopyAndArchive_Logs_PSFINTECHLogs -remoteComputerToConnectTo ([System.Net.Dns]::GetHostByName("IRMATest1Tidal01")).HostName -srcServer "E:\" -destServer "E:\" -deleteFilesAfterZip $false
#CopyAndArchive_IIS_Logs -remoteComputerToConnectTo ([System.Net.Dns]::GetHostByName("IRMADEVTidal01")).HostName -srcPath "C:\"




