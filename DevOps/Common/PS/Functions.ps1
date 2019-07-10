# Function declarations.
########################

function LogAndWrite() {
    param([string] $LogOutput)
    $__logText = "[" + (Get-Date -Format "yyyy-MM-dd HH:mm:ss") + "] " + $LogOutput
    Write-Host $__logText
    Out-File -InputObject $__logText -FilePath $logFilePath -Append
}

function UserWantsToContinue() {
    $continue = Read-Host "Do you want to continue? (y/n)"

    While (($continue -ne 'n') -and ($continue -ne 'N') -and ($continue -ne 'y') -and ($continue -ne 'Y'))
    {
        Write-Host "Invalid input.  Please enter y or n."
        $continue = Read-Host "Do you want to continue? (y/n)"
    }

    If (($continue -eq 'n') -or ($continue -eq 'N'))
    {
        Return $false
    }
    Else 
    {
        Return $true
    }
}

function AskYesNo() {
    param([string] $MainPrompt)
    $prompt = "$MainPrompt --> Yes or No? (y/n)"
    $response = Read-Host $prompt

    While ((-not ($response -like 'n')) -and (-not ($response -like 'y')))
    {
        Write-Host "Invalid input.  Please enter y or n."
        $response = Read-Host $prompt
    }

    return ($response -like 'y')
}

function RecycleAppPool() {
    [CmdletBinding(SupportsShouldProcess=$True)]
    param([string] $ServerName, [string] $AppPoolName)

    $hostName = ([System.Net.Dns]::GetHostByName($ServerName)).HostName

    $shouldProcessMessage = "Invoke-Command on computer $hostName ($ServerName) Restart-WebAppPool -Name $AppPoolName"

    If ($PSCmdlet.ShouldProcess($shouldProcessMessage)) {
            Invoke-Command -ComputerName $hostName -ScriptBlock {
                param($appPool)
                Add-PSSnapin WebAdministration
                Restart-WebAppPool -Name $appPool
            } -ArgumentList $AppPoolName
    }
}

function SetSitePhysicalPath() {
    [CmdletBinding(SupportsShouldProcess=$True)]
    param(
        [string] $ServerName,
        [string] $SiteName,
        [string] $NewPath,
        [string] $AppPoolName
    )

    $hostName = ([System.Net.Dns]::GetHostByName($ServerName)).HostName

    $shouldProcessMessage = "Invoke-Command on computer $hostName ($ServerName) for site [$SiteName] and app pool [$AppPoolName] Set-ItemProperty -name physicalPath -value $NewPath"

    If ($PSCmdlet.ShouldProcess($shouldProcessMessage)) {
            Invoke-Command -ComputerName $hostName -ScriptBlock {
            param($site, $sitePath, $appPool)
                Add-PSSnapin WebAdministration
                $siteRef = "IIS:\Sites\" + $site
                "Updating [" + $site + "] site's physical path to [" + $sitePath + "]..."
                Set-ItemProperty $siteRef -name physicalPath -value $sitePath
                $updatedPath = Get-ItemProperty $siteRef -name physicalPath
                "Value after update: " + $updatedPath
                "Recycling app pool [" + $appPool + "]"
                Restart-WebAppPool -Name $appPool
            } -ArgumentList $SiteName, $NewPath, $AppPoolName
    }
}

function DeleteContentsOfFolderRemotely() {
    [CmdletBinding(SupportsShouldProcess=$True)]

    param([string] $ServerName, [string] $Path)
    $hostName = ([System.Net.Dns]::GetHostByName($serverName)).HostName

    $contentsOfFolderPath = $path + "\*"

    $shouldProcessMessage = "Invoke-Command on computer $hostName ($ServerName): Remove-Item -Path $contentsOfFolderPath -Recurse"

    If ($PSCmdlet.ShouldProcess($shouldProcessMessage)) {
        Invoke-Command -ComputerName $hostName -ScriptBlock {
            param($directory)
            Remove-Item -Path $directory -Recurse
        } -ArgumentList $contentsOfFolderPath
    }
}

function CopyContentsOfFolderOnRemoteHost() {
    [CmdletBinding(SupportsShouldProcess=$True)]

    param([string] $ServerName, [string] $Source, [string] $Destination)

    $hostName = ([System.Net.Dns]::GetHostByName($serverName)).HostName
    $sourceContent = $Source + '\*'

    $shouldProcessMessage = "Invoke-Command on computer $hostName ($ServerName): Copy-Item -Path $sourceContent -Destination $destination -Recurse"

    If ($PSCmdlet.ShouldProcess($shouldProcessMessage)) 
    {
        Invoke-Command -ComputerName $hostName -ScriptBlock {
            param($sourceDirectory, $destDirectory)
            # We must create the dest folder, if it does not exist, because if the dest folder does NOT exist, 
			# the copy command will inappropriately copy each source file to a *FILE* with the dest folder name.
            If (-not (Test-Path $destDirectory)){
                New-Item -ItemType Directory -Path $destDirectory
            }
            Copy-Item -Path $sourceDirectory -Destination $destDirectory -Recurse
        
        } -ArgumentList $sourceContent, $destination
    }
}

function DisableScheduledTasksRemotely() {
    [cmdletBinding(SupportsShouldProcess=$True)]

    param([string] $ServerName, [string] $ScheduledTaskNameWildCard)

    $hostName = ([System.Net.Dns]::GetHostByName($serverName)).HostName

    $shouldProcessMessage = "Invoke-Command on computer $hostName ($ServerName): Get-ScheduledTask | Where-Object {`$_.TaskName -like '$ScheduledTaskNameWildCard'} | Disable-ScheduledTask"

    If ($PSCmdlet.ShouldProcess($shouldProcessMessage))
    {
        Invoke-Command -ComputerName $hostName -ScriptBlock {
            param($task)
            Get-ScheduledTask | ? {$_.TaskName -like $task} | Disable-ScheduledTask
        } -ArgumentList $ScheduledTaskNameWildCard
    }
    
}

function StopServiceRemotely() {
    [cmdletBinding(SupportsShouldProcess=$True)]

    param($ServerName, $ServiceDisplayName)

    $hostName = ([System.Net.Dns]::GetHostByName($serverName)).HostName

    $shouldProcessMessage = "Invoke-Command on computer $hostName ($ServerName): Get-Service -DisplayName $ServiceDisplayName | Where-Object {`$_.Status -eq 'Running'} | Stop-Service"

    If ($PSCmdlet.ShouldProcess($shouldProcessMessage))
    {
        Invoke-Command -ComputerName $hostName -ScriptBlock {
            param($serviceName)

            Get-Service -DisplayName $serviceName | ? {$_.Status -eq "Running"} | Stop-Service
        } -ArgumentList $ServiceDisplayName
    }
}

function EnableScheduledTasksRemotely() {
    [cmdletBinding(SupportsShouldProcess=$True)]

    param([string] $ServerName, [string] $ScheduledTaskNameWildCard)

    $hostName = ([System.Net.Dns]::GetHostByName($serverName)).HostName

    $shouldProcessMessage = "Invoke-Command on computer $hostName ($ServerName): Get-ScheduledTask | Where-Object {`$_.TaskName -like '$ScheduledTaskNameWildCard'} | Enable-ScheduledTask"

    If ($PSCmdlet.ShouldProcess($shouldProcessMessage))
    {
        Invoke-Command -ComputerName $hostName -ScriptBlock {
            param($task)
            Get-ScheduledTask | ? {$_.TaskName -like $task} | Enable-ScheduledTask
        } -ArgumentList $ScheduledTaskNameWildCard
    }
    
}

function StartServiceRemotely() {
    [cmdletBinding(SupportsShouldProcess=$True)]

    param($ServerName, $ServiceDisplayName)

    $hostName = ([System.Net.Dns]::GetHostByName($serverName)).HostName

    $shouldProcessMessage = "Invoke-Command on computer $hostName ($ServerName): Get-Service -DisplayName $ServiceDisplayName | Where-Object {`$_.Status -ne 'Running'} | Start-Service"

    If ($PSCmdlet.ShouldProcess($shouldProcessMessage))
    {
        Invoke-Command -ComputerName $hostName -ScriptBlock {
            param($serviceName)

            Get-Service -DisplayName $serviceName | ? {$_.Status -ne "Running"} | Start-Service
        } -ArgumentList $ServiceDisplayName
    }
}

function RemoveService() {
	[cmdletBinding(SupportsShouldProcess=$True)]

	param($ServiceName, $RemoteSvrName)
	
	$shouldProcessMessage = "ServiceName=[$ServiceName], RemoteSvrName=[$RemoteSvrName]"

	If ($PSCmdlet.ShouldProcess($shouldProcessMessage))
    {
        
        if ($RemoteSvrName -ne $null -and $RemoteSvrName.length -gt 0){
            $dnsHostname = [System.Net.Dns]::GetHostByName($RemoteSvrName).HostName
            "[[RemoveService.Log]] Locating service '" + $ServiceName + "' on remote machine '" + $RemoteSvrName + "' (DNS=" + $dnsHostname + ")..."
            Invoke-Command -ComputerName $dnsHostname -ScriptBlock {
                param($r_SvcName)

                $service = Get-Service -Name $r_SvcName -ErrorAction SilentlyContinue
                if ($service) {
                    $serviceName = $service.Name
                    $serviceObject = Get-WmiObject -Class Win32_Service -Filter "Name='$serviceName'"
                    "[[RemoveService.Log]] Deleting service..."
                    if ($serviceObject -ne $null){
                        $serviceObject.delete()
                    }
                } else {
                    Write-Host -ForegroundColor Yellow "[[RemoveService.Log]] *Service-Not-Found"
                }

            } -ArgumentList $ServiceName

        } else {
            "[[RemoveService.Log]] Locating service '" + $ServiceName + "'..."
            $service = Get-Service -Name $ServiceName -ErrorAction SilentlyContinue
            if ($service) {
                $serviceName = $service.Name
                $serviceObject = Get-WmiObject -Class Win32_Service -Filter "Name='$serviceName'"
                "[[RemoveService.Log]] Deleting service..."
                if ($serviceObject -ne $null){
                    $serviceObject.delete()
                }
            } else {
                Write-Host -ForegroundColor Yellow "[[RemoveService.Log]] *Service-Not-Found"
            }
        }
    }
}

function NewService() {	
	[cmdletBinding(SupportsShouldProcess=$True)]

	param($NewSvcName, $NewSvcDisplayName, $NewSvcDesc, $NewSvcExePath, $NewSvcUser, $IsNewSvcEnabled, $IsDeleteExisting, $RemoteSvrName)
	
	$shouldProcessMessage = "ServiceName=[$NewSvcName], NewSvcDisplayName=[$NewSvcDisplayName], NewSvcDesc=[$NewSvcDesc], NewSvcExePath=[$NewSvcExePath], NewSvcUser=[$NewSvcUser], IsNewSvcEnabled=[$IsNewSvcEnabled], IsDeleteExisting=[$IsDeleteExisting], RemoteSvrName=[$RemoteSvrName]"

    If ($PSCmdlet.ShouldProcess($shouldProcessMessage))
    {
        # Create on remote server, if a server name was passed.
        if ($RemoteSvrName -ne $null -and $RemoteSvrName.length -gt 0){

            $dnsHostname = [System.Net.Dns]::GetHostByName($RemoteSvrName).HostName
            "[[NewService.Log]] Creating service '" + $NewSvcName + "' on remote machine '" + $RemoteSvrName + "' (DNS=" + $dnsHostname + ")..."
            # Delete remote service first, if specified.
            if ($IsDeleteExisting){
                RemoveService -ServiceName $NewSvcName -RemoteSvrName $RemoteSvrName
            }

            Invoke-Command -ComputerName $dnsHostname -ScriptBlock {
                param($r_NewSvcName, $r_newSvcDisplayName, $r_newSvcDesc, $r_newSvcExePath, $r_newSvcUser, $r_isNewSvcEnabled)

                if ($r_isNewSvcEnabled){
		            New-Service -Name $r_NewSvcName -DisplayName $r_newSvcDisplayName -Description $r_newSvcDesc -BinaryPathName $r_newSvcExePath -Credential $r_newSvcUser -StartupType Automatic -Verbose
                } else {
		            New-Service -Name $r_NewSvcName -DisplayName $r_newSvcDisplayName -Description $r_newSvcDesc -BinaryPathName $r_newSvcExePath -Credential $r_newSvcUser -StartupType Disabled -Verbose
                }
            } -ArgumentList $NewSvcName, $newSvcDisplayName, $newSvcDesc, $newSvcExePath, $newSvcUser, $isNewSvcEnabled

        } else {
            # Create on local machine.
            "[[NewService.Log]] Creating service '" + $NewSvcName + "'..."
            # Delete local service first, if specified.
            if ($IsDeleteExisting){
                RemoveService -ServiceName $NewSvcName
            }
            if ($isNewSvcEnabled){
		        New-Service -Name $NewSvcName -DisplayName $newSvcDisplayName -Description $newSvcDesc -BinaryPathName $newSvcExePath -Credential $newSvcUser -StartupType Automatic -Verbose
            } else {
		        New-Service -Name $NewSvcName -DisplayName $newSvcDisplayName -Description $newSvcDesc -BinaryPathName $newSvcExePath -Credential $newSvcUser -StartupType Disabled -Verbose
            }
        }

    }
}

function RemoveScheduledTask() {
	[cmdletBinding(SupportsShouldProcess=$True)]

	param($TaskName, $ServerName)
	
	$shouldProcessMessage = "TaskName=[$TaskName], ServerName=[$ServerName]"

	If ($PSCmdlet.ShouldProcess($shouldProcessMessage))
    {
        
        if ($ServerName -ne $null -and $ServerName.length -gt 0){
            $dnsHostname = [System.Net.Dns]::GetHostByName($ServerName).HostName
            # Try to get sched task before remove is attempted.
            "[[RemoveSchedTask.Log]] Locating sched task '" + $TaskName + "' on remote machine '" + $ServerName + "' (DNS=" + $dnsHostname + ")..."
            $task = Get-ScheduledTask -TaskName $TaskName -CimSession $dnsHostname -ErrorAction SilentlyContinue
            if ($task) {
                "[[RemoveSchedTask.Log]] Removing sched task..."
                Unregister-ScheduledTask -TaskName $TaskName -CimSession $dnsHostname -Confirm:$false -Verbose
            } else {
                Write-Host -ForegroundColor Yellow "[[RemoveSchedTask.Log]] *Sched-Task-Not-Found"
            }

        } else {

            "[[RemoveSchedTask.Log]] Locating sched task '" + $TaskName + "'..."
            $task = Get-ScheduledTask -TaskName $TaskName -ErrorAction SilentlyContinue
            if ($task) {
                "[[RemoveSchedTask.Log]] Removing sched task..."
                Unregister-ScheduledTask -TaskName $TaskName -Confirm:$false -Verbose
            } else {
                Write-Host -ForegroundColor Yellow "[[RemoveSchedTask.Log]] *Sched-Task-Not-Found"
            }
        }
    }
}

function NewScheduledTaskFromXML() {
	[cmdletBinding(SupportsShouldProcess=$True)]

	param($NewTaskName, $NewTaskUser, $NewTaskPwd, $NewTaskXmlFilePath, $IsDeleteExisting, $ServerName, $CopyXmlFileToFolder, $XmlFileRemoteServerLocalPath)
	
	$shouldProcessMessage = "TaskName=[$NewTaskName], NewTaskUser=[$NewTaskUser], NewTaskPwd=[*****], NewTaskXmlFilePath=[$NewTaskXmlFilePath], IsDeleteExisting=[$IsDeleteExisting], ServerName=[$ServerName], CopyXmlFileToFolder=[$CopyXmlFileToFolder], XmlFileRemoteServerLocalPath=[$XmlFileRemoteServerLocalPath]"

    If ($PSCmdlet.ShouldProcess($shouldProcessMessage))
    {
        # Create on remote server, if a server name was passed.
        if ($ServerName -ne $null -and $ServerName.length -gt 0){

            $dnsHostname = [System.Net.Dns]::GetHostByName($ServerName).HostName
            # Get XML definition file obj.
            "[[NewSchedTask.Log]] Locating sched task xml definition file '" + $NewTaskXmlFilePath + "'..."
            $localXmlFile = Get-ChildItem -Path $NewTaskXmlFilePath
            if (-not($localXmlFile)){
                Write-Host -ForegroundColor Magenta "[[NewSchedTask.Log]] Source task definition file '" + $NewTaskXmlFilePath + "' not found."
                Write-Host -ForegroundColor Yellow "[[NewSchedTask.Log]] *Cannot-Continue"
                return $null;
            }
            # Copy XML file to remote server first, if needed.  If not specified, we assume file on remote server exists (is local or full network path).
            $xmlDeleteOk = $false
            if ($CopyXmlFileToFolder){
                $xmlDeleteOk = $true
                $destXmlFilePath = $CopyXmlFileToFolder + "\" + $localXmlFile.Name
                "[[NewSchedTask.Log]] Deploying xml definition file..."
                Copy-Item -Path $NewTaskXmlFilePath -Destination $destXmlFilePath -Verbose
                # Verify dest file.
                if(-not(Test-Path $destXmlFilePath)){
                    Write-Host -ForegroundColor Magenta "[[NewSchedTask.Log]] Source task definition file '" + $NewTaskXmlFilePath + "' could not be copied to destination folder '" + $destXmlFilePath + "' on remote server '" + $ServerName + "'."
                    Write-Host -ForegroundColor Yellow "[[NewSchedTask.Log]] *Sched-Task-Not-Created"
                    return $null;
                }
                # Build path to XML file from remote server's perspective (it was just copied there).
                $xmlImportPath = $XmlFileRemoteServerLocalPath + "\" + $localXmlFile.Name
            } else {
                # If no definition file deployed, source and dest should be the same.
                $xmlImportPath = $NewTaskXmlFilePath
            }

            "[[NewSchedTask.Log]] Creating sched task '" + $NewTaskName + "' on remote machine '" + $ServerName + "' (DNS=" + $dnsHostname + ")..."
            # Delete remote instance first, if specified.
            if ($IsDeleteExisting){
                RemoveScheduledTask -TaskName $NewTaskName -ServerName $ServerName
            }

            Invoke-Command -ComputerName $dnsHostname -ScriptBlock {
                param($r_NewTaskName, $r_NewTaskUser, $r_NewTaskPwd, $r_xmlImportPath, $r_xmlDeleteOk)

                "[[NewSchedTask.Log]] Registering sched task '" + $r_NewTaskName + "' from XML definition '" + $r_xmlImportPath + "'..."
                schtasks.exe /create /RU $r_NewTaskUser /RP $r_NewTaskPwd /TN $r_NewTaskName /XML $r_xmlImportPath

                # Clean up only if file was copied to remote server.
                if ($r_xmlDeleteOk){
                    "[[NewSchedTask.Log]] Deleting remote XML definition file '" + $r_xmlImportPath + "'..."
                    Remove-Item -Path $r_xmlImportPath
                }

            } -ArgumentList $NewTaskName, $NewTaskUser, $NewTaskPwd, $xmlImportPath, $xmlDeleteOk

        } else {
            # Create on local machine.
            "[[NewSchedTask.Log]] Creating sched task '" + $NewSvcName + "'..."
            # Delete local instance first, if specified.
            if ($IsDeleteExisting){
                RemoveScheduledTask -TaskName $NewTaskName
            }

            "[[NewSchedTask.Log]] Registering sched task '" + $NewTaskName + "' from XML definition '" + $xmlImportPath + "'..."
            schtasks.exe /create /RU $NewTaskUser /RP $NewTaskPwd /TN $NewTaskName /XML $xmlImportPath
        }

    }
}




<#

.SYNOPSIS

Builds tab-separated values (TSV) list of attributes for targeted Scheduled Tasks.

.DESCRIPTION

Returns a list of details for scheduled tasks on the local machine or a specific remote server.
Each row/entry in the list contains attributes of a Windows Scheduled Task, separated by tabs.

.PARAMETER TaskNameFilter
Filter applied to task names used to target one or more Scheduled Tasks.

.PARAMETER GroupName
Optional group identifier that can be added as an attribute for each row returned.
If excluded/empty/null, the sched-task attribute "Group Name" will not be included in the results list.

.PARAMETER ServerName
Optional remote server from which to pull sched-task info.

.EXAMPLE
GetSchedTaskAsTSV -TaskNameFilter "*control*" 


#>
function GetSchedTaskAsTSV() {
	[cmdletBinding(SupportsShouldProcess=$True)]

	param($TaskNameFilter, $GroupName, $ServerName)
	
    $groupNameHeader = ""
    $groupNameValue = ""
    if ($GroupName -ne $null -and $GroupName.length -gt 0){
        $groupNameHeader = "Group Name`t"
        $groupNameValue = $GroupName + "`t"
    }
    # Setup return var.
    $tsvOutput = @()
    $tsvOutput += $groupNameHeader + "Server Name`tServer Alias`tTask Name`tExecute Action`tAction Args`tIs Enabled`tConfig File Net Path"

	$shouldProcessMessage = "TaskName=[$TaskNameFilter], ServerName=[$ServerName]"

	If ($PSCmdlet.ShouldProcess($shouldProcessMessage))
    {
        
        if ($ServerName -ne $null -and $ServerName.length -gt 0){
            $jobSvrAlias = $ServerName
            $dnsHostname = [System.Net.Dns]::GetHostByName($ServerName).HostName
            Write-Host -ForegroundColor Cyan ("[[GetSchedTaskAttributes.Log]] Pulling sched task info from " + $ServerName + "' (DNS=" + $dnsHostname + ")...")
            Remove-CimSession $dnsHostname -ErrorAction SilentlyContinue
            $svrCimSession = New-CimSession $dnsHostname
            $schedTaskList = Get-ScheduledTask -CimSession $svrCimSession -TaskName $TaskNameFilter

        } else {
            Write-Host -ForegroundColor Cyan "[[GetSchedTaskAttributes.Log]] Pulling sched task info from local machine..."
            $jobSvrAlias = "localhost"
            $dnsHostname = $env:computername
            $schedTaskList = Get-ScheduledTask -TaskName $TaskNameFilter
        }

        $schedTaskList | foreach {
            # Build config file from EXE in sched task's action to link to config files that were gathered.
            $jobConfigFilePath = ""
            if($_.Actions.Execute){
                $jobConfigFilePath = "\\" + $dnsHostname + "\" + $_.Actions.Execute.Replace("""", "").Replace(":\", "$\") + ".config"
            }
            Write-Host -ForegroundColor Cyan ("[[GetSchedTaskAttributes.Log]] Config file: " + $jobConfigFilePath)
            $tsvOutput += $groupNameValue + $dnsHostname + "`t" + $jobSvrAlias + "`t" + $_.TaskName + "`t" + $_.Actions.Execute + "`t" + $_.Actions.Arguments + "`t" + $_.Settings.Enabled + "`t" + $jobConfigFilePath
        }
        return $tsvOutput
    }
}



function ExitProcess() {
    Stop-Transcript
    Exit
}


function GetDeployParam() {
    param(
    [system.array] $ArgList, 
    [string] $ParamName)

    foreach ($param in $ArgList){
        if($param.ToLower().Contains($ParamName.ToLower())){
            # Split around "=".
            $paramKeyValue = $param.split("=")
            Write-Debug ("paramKeyValue=" + $paramKeyValue)
            return $paramKeyValue[1]
        }
    }
}


function IsComponentDeployEnabled() {
    param(
    [system.array] $ComponentList, 
    [string] $TargetComponentName)

    return $ComponentList.ToLower().Contains($TargetComponentName.tolower())

}


function GetSubfolderWithVersionName() {
    param(
    [string] $Path)

    $item = Get-ChildItem $Path
    if(-not ($items -is [System.Array])){
        $itemList = @()
        $itemList += $item
    } else {
        $itemList = $item
    }

    # We don't have rules for if there are more than one version folder, so we return the first one we find.
    foreach ($item in $itemList){
        if($item -is [System.IO.DirectoryInfo] -and $item.name -match "\."){
            return $item.name
        }
    }
}




function GetSQLObjLastModDate(){
    param(
        [string] $dbSvr,
        [string] $dbName,
        [string] $objType,
        [string] $objSchema,
        [string] $objName,
        [boolean] $extractCode,
        [string] $extractCodeRegion,
        [string] $extractCodePath,
        [string] $extractCodeInstanceRef # Ex: idq-fl or idq-icon
    )

    [System.Reflection.Assembly]::LoadWithPartialName("Microsoft.SqlServer.SMO") | Out-Null

    #$dbSvr = ("ID"  + $envId + "-" + $region + "\" + $region + $envId)
    $SMOserver = New-Object ('Microsoft.SqlServer.Management.Smo.Server') -argumentlist $dbSvr
    $db = $SMOserver.databases[$dbName]
    
    # Verify connection.
    if (-not $db){
        Write-Host -ForegroundColor Magenta ("[[GetSQLObjLastModDate.Log]] ERROR: DB instance connection to [" + $dbName + "] failed.")
        return $null
    }

    $objRef = ""
    # Grab FN or PROC reference.
    if ($objType -ieq "fn") {
        $objRef = $db.UserDefinedFunctions.Item($objName, $objSchema)
    } else {
        $objRef = $db.StoredProcedures.Item($objName, $objSchema)
    }

    # No need to continue if we didn't find the object.
    if (-not $objRef) { return $null }

    # Extract code?
    if ($extractCode) {
        if ($extractCodeRegion){
            $objSQL = $objRef.textbody
            $extractedCodeFile = $extractCodePath + "\ExtractedSQL." + $objName + "." + $extractCodeInstanceRef + ".sql"
            Out-File -FilePath $extractedCodeFile -InputObject $objSQL
            "--> Extracted SQL to: " + $extractedCodeFile
        }
    }

    if ($objRef){
        $dbSvr + ": " + $objRef.DateLastModified.DateTime
    } else {
        return $null
    }

}



function GetHashKey() {
    param(
        [System.Collections.Hashtable] $HashTable,
        [string] $HashValue
    )

    foreach ($key in $HashTable.Keys){
        if($HashTable.$key -like $HashValue){
            return $key
        }
    }

    # If we are here, we didn't find a key for the value.
    return $null
}


##############################################################################################################################
##############################################################################################################################

function Export-SqlTable {
[CmdletBinding()]
Param(
    [Parameter(Mandatory = $true, HelpMessage = "SQL server name to which you wish to connect.")]
    [string]$Server,

    [string]$Database,

    [Parameter(Mandatory = $true, HelpMessage = "Name of database table to export; schema prefix required, if database specified.")]
    [string]$Table,
    
    [switch]$CharType,
    [switch]$TrustedAuth,
    [string]$Path
)
    ##########################
    # SAVE DATA
    ##########################
    if($Database){ $dbMsg = "[$Database] DB " }
    Write-Host -ForegroundColor Green ("SAVING [$Server] Server " + $dbMsg + "[$Table] DATA...")
    $filenameSafeSvr = $Server.Replace("\", "-")

    # Build output folder.
    $outputFolder = ""
    if($Path){
        $outputFolder = $Path
        if(-not $Path.endswith("\")){ $outputFolder += "\" }
    }

    # Build output filename.
    $outputFile = $outputFolder + "$filenameSafeSvr.$Table.dat"

    # Only specify database param if a DB name was provided.
    $dbParam = ""
    # A space after the "-d" switch was causing an error because it tries to use the literal space in the DB name.
    if($Database){ $dbParam = "-d$Database" }

    # Build optional BCP command-line switches.
    $bcpCharType = ""
    if($CharType){ $bcpCharType = "-c" }

    $bcpTrustedAuth = ""
    if($TrustedAuth){ $bcpTrustedAuth = "-T" }

    bcp $Table out $outputFile -S $Server $dbParam $bcpCharType $bcpTrustedAuth
}


##############################################################################################################################
##############################################################################################################################

function Clear-SqlTable {
[CmdletBinding()]
Param(
    [Parameter(Mandatory = $true, HelpMessage = "SQL server name to which you wish to connect and export from.")]
    [string]$Server,

    [string]$Database,

    [Parameter(Mandatory = $true, HelpMessage = "Name of database table from which all data will be deleted/truncated; prefix required, if database specified.")]
    [string]$Table,
    
    [switch]$UseTruncate
)
    ##########################
    # CLEAR DATA
    ##########################

    # Only specify database param if a DB name was provided.
    $dbParam = ""
    # A space after the "-d" switch was causing an error because it tries to use the literal space in the DB name.
    if($Database){ $dbParam = "-d$Database" }

    $actionMsg = "DELETING"
    $dmlSql = "delete from"
    if($UseTruncate){
        $actionMsg = "TRUNCATING"
        $dmlSql = "truncate table"
    }

    Write-Host -ForegroundColor Yellow "$actionMsg [$Server] DB [$Table] DATA..."

    sqlcmd -S $Server -Q "$dmlSql $Database.$Table"
}


##############################################################################################################################
##############################################################################################################################

function Import-SqlTable {
[CmdletBinding()]
Param(
    [Parameter(Mandatory = $true, HelpMessage = "SQL server name to which you wish to connect and import into.")]
    [string]$Server,

    [string]$Database,

    [Parameter(Mandatory = $true, HelpMessage = "Name of database table to import into; schema prefix required, if database specified.")]
    [string]$Table,
    
    [string]$InputFile,

    [switch]$TrustedAuth,
    [switch]$CharType,
    [switch]$KeepNulls,
    [switch]$KeepIdentityValues
)
    ##########################
    # SAVE DATA
    ##########################
    Write-Host -ForegroundColor Green "RESTORING [$Server] DB [$Table] DATA..."
    $filenameSafeSvr = $Server.Replace("\", "-")

    # Input file specified?
    $bcpInFile = $InputFile
    if(-not $InputFile){
        # Build input filename.
        $bcpInFile = "$filenameSafeSvr.$Table.dat"
        Write-Verbose "No in-file specified; using ""$bcpInFile""."
    }
    if(-not (Test-Path $bcpInFile)){
        Write-Host -ForegroundColor Magenta "**ERROR** -- Input file [$bcpInFile] does not exist."
        return
    }
    $inFileObj = gci $bcpInFile
    Write-Verbose ("Input file [" + $inFileObj.FullName + "] size is [" + $inFileObj.Length + "] bytes.")
    

    # Only specify database param if a DB name was provided.
    $dbParam = ""
    # A space after the "-d" switch was causing an error because it tries to use the literal space in the DB name.
    if($Database){ $dbParam = "-d$Database" }

    # Build optional BCP command-line switches.
    $bcpTrustedAuth = ""
    if($TrustedAuth){ $bcpTrustedAuth = "-T" }

    $bcpCharType = ""
    if($CharType){ $bcpCharType = "-c" }

    $bcpKeepNulls = ""
    if($KeepNulls){ $bcpKeepNulls = "-k" }

    $bcpKeepIdentityValues = ""
    if($KeepIdentityValues){ $bcpKeepIdentityValues = "-E" }

    bcp $Table in $bcpInFile -S $Server -d $Database $bcpTrustedAuth $bcpCharType $bcpKeepNulls $bcpKeepIdentityValues
}




####################################################################################################################################
####################################################################################################################################
#
# Object-Properties Definitions Below
#
####################################################################################################################################
####################################################################################################################################



# This object represents instance-specific attributes for an instance of a controller on a server.
# An instance of a controller needs:
#  1) instance name
#  2) the server where it will be deployed
#  3) xml definition file (usually different for each server and instance, if instance ID is passed to controller)
#  4) instance ID option
#  5) instance and value (instance IDs are optional, so can be disabled).
#
# See below for example with parent controller object.
$controllerInstanceProperties = @{
    'Name' = ""; # This should match the shortname in the 'controllerBuildList' lists.
    'TaskName' = ""; # This is the unique name in Task Scheduler and will contain the instance ID, if enabled.
    'Server' = ""; # Server where task will be deployed.
    'XmlFileName' = ""; # Name of XML file from which Scheduled Task will be created.
    'UseInstanceId' = $true; # If enabled, the job's name in Task Scheduler will contain "InstanceId=#".
    'InstanceId' = ""; # Unique identifer for an instance of a type of controller to be used in the name of the job created in Task Scheduler.
}

# An instance of the below 'controllerProperties' object represents a set of instances of a specific type of controller, 
# so it contains the global attributes that apply to all copies of the job and a list of specific instances.
# Each copy of a controller deployed to a server will have an instance of the 'controllerInstanceProperties' object, and the list
# of all copies of that type of controller are stored in the 'InstanceList' array.
# EXAMPLE:
# -------------------------
# [Controller Object]
# -------------------------
# > Name = Price Controller
# > RunAsUser
# > RunAsPwd
# > CopyXmlFileToServerSubFolder = e$\mammoth
# > XmlFileRemoteServerLocalPath
# --------------------------------------------------
#     -------------------------
#     [Instance 1]
#     -------------------------
#     > Name = Price Controller
#     > TaskName = Mammoth Price Controller
#     > Server = vm-icon-qa1
#     > XmlFileName = \\irmaqafile\e$\irma\staging\Mammoth\1.0\QA\dummy.1.xml
#     > UseInstanceId = true
#     > InstanceId = 1
#     -------------------------
#     [Instance 2]
#     -------------------------
#     > Name = Price Controller
#     > TaskName = Mammoth Price Controller
#     > Server = vm-icon-qa2
#     > XmlFileName = \\irmaqafile\e$\irma\staging\Mammoth\1.0\QA\dummy.2.xml
#     > UseInstanceId = true
#     > InstanceId = 2
#     -------------------------
# --------------------------------------------------
$controllerProperties = @{
    'Name' = ""; # This should match the shortname in the 'controllerBuildList' lists.
    'RunAsUser' = "";
    'RunAsPwd' = "";
    'CopyXmlFileToServerSubFolder' = "";
    'XmlFileRemoteServerLocalPath' = "";
    'InstanceList' = @() # Start with empty list, we'll add each controller to it.
}

# An instance of the below 'winSvcProperties' object represents the attributes for a specific type of Windows Service
# and the list of servers where it will be enabled.
$winSvcProperties = @{
    'Name' = ""; # This should match the shortname in the 'windowsServicesToDeploy' and 'windowsServicesBuildList' lists.
    'InstanceName' = ""; # This will be the instance name created in Windows.
    'ExeName' = "";
    'RunAsUser' = "";
    'EnabledServerList' = @()
}

