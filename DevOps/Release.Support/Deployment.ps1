$scriptFullPath = $MyInvocation.MyCommand.Definition

$scriptFileObj = new-object system.io.fileinfo $MyInvocation.MyCommand.Definition
$scriptFolder = $scriptFileObj.directoryname
$fnScript = $scriptFolder + "\Functions.ps1"

if (-not (Test-Path $fnScript)) {

"
******************************************************************
Cannot Continue -- Expected Functions Script Not Found
--> '" + $fnScript + "'

Ensure this script exists in the same folder from which you
ran the current script and try again.
******************************************************************
"
    Read-Host "Press enter to exit..."
    return
}

# Functions file must be in the same directory.
. $fnScript




# Script variables.
###################

$previewMode = $true
$newline = [Environment]::NewLine
$environment = "PRD"
$version = "4.8"
$logFilePath = $scriptFullPath + "." + $environment + "." + $version + ".log"

# Test: "\\irmadevfile\e$\IRMA\Staging\Icon\"
# QA: "\\irmaqafile\e$\IRMA\Staging\iCon\"

$stagingPath = "\\irmaqafile\e$\IRMA\Staging\iCon\" + $version + "\" + $environment


# Test: @('vm-icon-test1', 'vm-icon-test2')
# QA: @('vm-icon-qa1', 'vm-icon-qa2', 'vm-icon-qa3', 'vm-icon-qa4')
# Production: @('vm-icon-prd1', 'vm-icon-prd2', 'vm-icon-prd3', 'vm-icon-prd4', 'vm-icon-prd5', 'vm-icon-prd6')

$appServers = @('vm-icon-prd1', 'vm-icon-prd2', 'vm-icon-prd3', 'vm-icon-prd4', 'vm-icon-prd5', 'vm-icon-prd6')


# Test: @('irmatestapp1', 'irmatestapp2', 'irmatestapp3', 'irmatestapp4')
# QA: @('irmaqaapp1', 'irmaqaapp2', 'irmaqaapp3', 'irmaqaapp4')
# Production: @('irmaprdapp1', 'irmaprdapp2', 'irmaprdapp3', 'irmaprdapp4', 'irmaprdapp5', 'irmaprdapp6', 'irmaprdapp7', 'irmaprdapp8')

$webServers = @('irmaprdapp1', 'irmaprdapp2', 'irmaprdapp3', 'irmaprdapp4', 'irmaprdapp5', 'irmaprdapp6', 'irmaprdapp7', 'irmaprdapp8')




# IconWeb deployment configuration.
###################################

$deployIconWeb = $true
$iconWebRelativePath = "\WebApps\Icon"
$iconWebLocalServerPath = "E:" + $iconWebRelativePath
$iconWebNetworkServerPath = "\e$" + $iconWebRelativePath
$iconWebApplicationPoolName = "Icon"
$iconWebNewVersion = "4.8"




# IconService deployment configuration.
#######################################

$deployIconService = $true
$iconServiceRelativePath = "\WebApps\IconService"
$iconServiceLocalServerPath = "E:" + $iconServiceRelativePath
$iconServiceNetworkServerPath = "\e$" + $iconServiceRelativePath
$iconServiceApplicationPoolName = "IconService"
$iconServiceNewVersion = "4.5.3"




# Controller deployment configuration.
######################################

# This dictionary should contain each controller name paired with the new version number being deployed.  Possible values:
#
#     APIController
#     API Controller Phase 2
#     Global Controller     
#     Regional Controller
#     POS Push Controller
#     SubTeam Controller

$controllersToDeploy = @{
    "APIController" = "4.1.7";
    "API Controller Phase 2" = "1.4.6";
}       

$deployControllers = $true
$controllerRelativePath = "\Icon"
$controllerLocalPath = "E:" + $controllerRelativePath
$controllerNetworkPath = "\e$" + $controllerRelativePath




# Windows Service deployment configuration.
###########################################

# This dictionary should contain each service name paired with the new version number being deployed.  Possible values:
#
#		CCH Tax Listener
#		R10 Listener
#		Item Movement Listener
#		eWIC APL Listener
#		eWIC Error Response Listener

$windowsServicesToDeploy = @{
    "CCH Tax Listener" = "1.4.4";
    "R10 Listener" = "1.3.4";    
	"Regional Controller" = "1.7.5";
    "POS Push Controller" = "1.4.4";
}

$deployWindowsServices = $true
$windowsServiceRelativePath = "\Icon"
$windowsServiceLocalPath = "E:" + $windowsServiceRelativePath
$windowsServiceNetworkPath = "\e$" + $windowsServiceRelativePath

# Main script.
##############

# Clear the logfile before each execution.
If ((Test-Path -Path $logFilePath) -eq $true)
{
    Remove-Item -Path $logFilePath
}


# Notify user of the script execution mode.
If ($previewMode -eq $true)
{
    $output = "The script is running in preview mode.  The only output will be log statements.  No changes will be made."
    LogAndWrite -LogOutput $output
        
    If ((UserWantsToContinue) -eq $false)
    {
        Exit
    }
}
Else
{
    $output = "The script is running in production mode.  Continuing may impact production systems."
    LogAndWrite -LogOutput $output
        
    If ((UserWantsToContinue) -eq $false)
    {
        Exit
    }
}


# Deploy IconWeb if it is part of the release.
If ($deployIconWeb -eq $true)
{
    $output = $newline + "IconWeb has been marked for inclusion with this deployment."
    LogAndWrite -LogOutput $output

    If ((UserWantsToContinue) -eq $false)
    {
        Exit
    }
    
    $output = $newline + "Starting deployment of IconWeb to the following servers:"
    LogAndWrite -LogOutput $output

    Foreach ($webServer in $webServers.GetEnumerator())
    {
        $output = $webServer
        LogAndWrite -LogOutput $output
    }

    Foreach ($webServer in $webServers.GetEnumerator())
    {
        $iconWebLocalDeploymentPath = $iconWebLocalServerPath + "\" + $environment
        $newVersionNetworkPath = "\\" + $webServer + $iconWebNetworkServerPath + "\" + $iconWebNewVersion
        $newVersionLocalPath = $iconWebLocalServerPath + "\" + $iconWebNewVersion
        $iconWebStagingPath = $stagingPath + "\IconWeb"
        $iconWebStagingPathWithFiles = $iconWebStagingPath + "\*"
        
        if ($previewMode -eq $true)
        {
            New-Item -Path $newVersionNetworkPath -ItemType directory -WhatIf
            Copy-Item -Path $iconWebStagingPathWithFiles -Destination $newVersionNetworkPath -Recurse -WhatIf            
            DeleteContentsOfFolderRemotely -ServerName $webServer -Path $iconWebLocalDeploymentPath -WhatIf
            CopyContentsOfFolderOnRemoteHost -ServerName $webServer -Source $newVersionLocalPath -Destination $iconWebLocalDeploymentPath  -WhatIf

            RecycleAppPool -serverName $webServer -appPoolName $iconWebApplicationPoolName -WhatIf
        }

        if ($previewMode -eq $false)
        {
            New-Item -Path $newVersionNetworkPath -ItemType directory -Verbose:$true *>> $logFilePath

            $output = "Copying files from $iconWebStagingPath to destination path $newVersionNetworkPath."
            LogAndWrite -LogOutput $output

            Copy-Item -Path $iconWebStagingPathWithFiles -Destination $newVersionNetworkPath -Recurse
            DeleteContentsOfFolderRemotely -ServerName $webServer -Path $iconWebLocalDeploymentPath -Verbose:$true *>> $logFilePath
            CopyContentsOfFolderOnRemoteHost -ServerName $webServer -Source $newVersionLocalPath -Destination $iconWebLocalDeploymentPath -Verbose:$true *>> $logFilePath
            
            RecycleAppPool -serverName $webServer -appPoolName $iconWebApplicationPoolName -Verbose *>> $logFilePath
        }

        $output = $newline + "Deployment complete for server $webServer."
        LogAndWrite -LogOutput $output
    }

    $output = $newline + "IconWeb deployment complete."
    LogAndWrite -LogOutput $output
}


# Deploy IconService if it is part of the release.
If ($deployIconService -eq $true)
{
    $output = $newline + "IconService has been marked for inclusion with this deployment."
    LogAndWrite -LogOutput $output

    If ((UserWantsToContinue) -eq $false)
    {
        Exit
    }
    
    $output = $newline + "Starting deployment of IconService to the following servers:"
    LogAndWrite -LogOutput $output

    Foreach ($webServer in $webServers.GetEnumerator())
    {
        $output = $webServer
        LogAndWrite -LogOutput $output
    }

    Foreach ($webServer in $webServers.GetEnumerator())
    {
        $iconServiceLocalDeploymentPath = $iconServiceLocalServerPath + "\" + $environment
        $newVersionNetworkPath = "\\" + $webServer + $iconServiceNetworkServerPath + "\" + $iconServiceNewVersion
        $newVersionLocalPath = $iconServiceLocalServerPath + "\" + $iconServiceNewVersion
        $iconServiceStagingPath = $stagingPath + "\IconService"
        $iconServiceStagingPathWithFiles = $iconServiceStagingPath + "\*"
        
        if ($previewMode -eq $true)
        {
            New-Item -Path $newVersionNetworkPath -ItemType directory -WhatIf:$true
            Copy-Item -Path $iconServiceStagingPathWithFiles -Destination $newVersionNetworkPath -Recurse -WhatIf
            DeleteContentsOfFolderRemotely -ServerName $webServer -Path $iconServiceLocalDeploymentPath -WhatIf
            CopyContentsOfFolderOnRemoteHost -ServerName $webServer -Source $newVersionLocalPath -Destination $iconServiceLocalDeploymentPath -WhatIf
            RecycleAppPool -serverName $webServer -appPoolName $iconServiceApplicationPoolName -WhatIf
        }

        if ($previewMode -eq $false)
        {
            New-Item -Path $newVersionNetworkPath -ItemType directory -Verbose:$true *>> $logFilePath

            $output = "Copying files from $iconServiceStagingPath to destination path $newVersionNetworkPath."
            LogAndWrite -LogOutput $output

            Copy-Item -Path $iconServiceStagingPathWithFiles -Destination $newVersionNetworkPath -Recurse
            DeleteContentsOfFolderRemotely -ServerName $webServer -Path $iconServiceLocalDeploymentPath -Verbose:$true *>> $logFilePath
            CopyContentsOfFolderOnRemoteHost -ServerName $webServer -Source $newVersionLocalPath -Destination $iconServiceLocalDeploymentPath -Verbose:$true *>> $logFilePath

            RecycleAppPool -serverName $webServer -appPoolName $iconServiceApplicationPoolName -Verbose:$true *>> $logFilePath
        }

        $output = $newline + "Deployment complete for server $webServer."
        LogAndWrite -LogOutput $output
    }

    $output = $newline + "IconService deployment complete."
    LogAndWrite -LogOutput $output
}


# Deploy any controllers that are part of the release.
If (($deployControllers -eq $true) -and ($controllersToDeploy.Count -gt 0))
{
    $output = $newline + "The following controllers will be deployed:"
    LogAndWrite -LogOutput $output

    Foreach ($controller in $controllersToDeploy.GetEnumerator())
    {
        Write-Host $controller.Key
        $controller.Key | Out-File -FilePath $logFilePath -Append    
    }

    If ((UserWantsToContinue) -eq $false)
    {
        Exit
    }
    
    $output = $newline + "Starting controller deployment..."
    LogAndWrite -LogOutput $output

    Foreach ($controller in $controllersToDeploy.GetEnumerator())
    {
        $output = $newline + "Starting deployment of " + $controller.Key + " to the following servers:"
        LogAndWrite -LogOutput $output

        Foreach ($appServer in $appServers.GetEnumerator())
        {
            $output = $appServer
            LogAndWrite -LogOutput $output
        }

        Foreach ($appServer in $appServers.GetEnumerator())
        {
            $controllerDeploymentLocalPath = $controllerLocalPath + "\" + $controller.Key + "\" + $environment
            $newVersionNetworkPath = "\\" + $appServer + $controllerNetworkPath + "\" + $controller.Key + "\" + $controller.Value            
            $newVersionLocalPath = $controllerLocalPath + "\" + $controller.Key + "\" + $controller.Value            
            $controllerStagingPathWithFiles = $stagingPath + "\" + $controller.Key + "\*"

            If ($previewMode -eq $true)
            {
                New-Item -Path $newVersionNetworkPath -ItemType directory -WhatIf:$true
                Copy-Item -Path $controllerStagingPathWithFiles -Destination $newVersionNetworkPath -Recurse -WhatIf
                DeleteContentsOfFolderRemotely -ServerName $appServer -Path $controllerDeploymentLocalPath -WhatIf
                CopyContentsOfFolderOnRemoteHost -ServerName $appServer -Source $newVersionLocalPath -Destination $controllerDeploymentLocalPath -WhatIf
            }
            
            If ($previewMode -eq $false)
            {
                New-Item -Path $newVersionNetworkPath -ItemType directory -Verbose:$true *>> $logFilePath
                Copy-Item -Path $controllerStagingPathWithFiles -Destination $newVersionNetworkPath -Recurse -Verbose *>> $logFilePath
                DeleteContentsOfFolderRemotely -ServerName $appServer -Path $controllerDeploymentLocalPath -Verbose:$true *>> $logFilePath
                CopyContentsOfFolderOnRemoteHost -ServerName $appServer -Source $newVersionLocalPath -Destination $controllerDeploymentLocalPath -Verbose:$true *>> $logFilePath
            }

            $output = $newline + "Deployment complete to server " + $appServer + "."
            LogAndWrite -LogOutput $output
        }
    }        

    $output = $newline + "All controllers have been deployed."
    LogAndWrite -LogOutput $output
}


# Deploy any Windows Services included in the release.
If (($deployWindowsServices -eq $true) -and ($windowsServicesToDeploy.Count -gt 0))
{
    $output = $newline + "The following Windows Services will be deployed:"
    LogAndWrite -LogOutput $output

    Foreach ($windowsService in $windowsServicesToDeploy.GetEnumerator())
    {
        Write-Host $windowsService.Key
        $windowsService.Key | Out-File -FilePath $logFilePath -Append    
    }

    If ((UserWantsToContinue) -eq $false)
    {
        Exit
    }
    
    $output = $newline + "Starting Windows Service deployment..."
    LogAndWrite -LogOutput -$output

    Foreach ($windowsService in $windowsServicesToDeploy.GetEnumerator())
    {
        $output = $newline + "Starting deployment of " + $windowsService.Key + " to the following servers:"
        LogAndWrite -LogOutput $output

        Foreach ($appServer in $appServers.GetEnumerator())
        {
            $output = $appServer
            LogAndWrite -LogOutput $output
        }

        Foreach ($appServer in $appServers.GetEnumerator())
        {
            $windowsServiceDeploymentLocalPath = $windowsServiceLocalPath + "\" + $windowsService.Key + "\" + $environment
            $newVersionNetworkPath = "\\" + $appServer + $windowsServiceNetworkPath + "\" + $windowsService.Key + "\" + $windowsService.Value
            $newVersionLocalPath = $windowsServiceLocalPath + "\" + $windowsService.Key + "\" + $windowsService.Value            
            $windowsServiceStagingPathWithFiles = $stagingPath + "\" + $windowsService.Key + "\*"

            If ($previewMode -eq $true)
            {
                New-Item -Path $newVersionNetworkPath -ItemType directory -WhatIf:$true
                Copy-Item -Path $windowsServiceStagingPathWithFiles -Destination $newVersionNetworkPath -Recurse -WhatIf
                DeleteContentsOfFolderRemotely -ServerName $appServer -Path $windowsServiceDeploymentLocalPath -WhatIf
                CopyContentsOfFolderOnRemoteHost -ServerName $appServer -Source $newVersionLocalPath -Destination $windowsServiceDeploymentLocalPath -WhatIf
            }
            
            If ($previewMode -eq $false)
            {
                New-Item -Path $newVersionNetworkPath -ItemType directory -Verbose:$true *>> $logFilePath
                Copy-Item -Path $windowsServiceStagingPathWithFiles -Destination $newVersionNetworkPath -Recurse -Verbose *>> $logFilePath
                DeleteContentsOfFolderRemotely -ServerName $appServer -Path $windowsServiceDeploymentLocalPath -Verbose:$true *>> $logFilePath
                CopyContentsOfFolderOnRemoteHost -ServerName $appServer -Source $newVersionLocalPath -Destination $windowsServiceDeploymentLocalPath -Verbose:$true *>> $logFilePath
            }

            $output = $newline + "Deployment complete to server " + $appServer + "."
            LogAndWrite -LogOutput $output
        }
    }

    $output = $newline + "All Windows Services have been deployed."
    LogAndWrite -LogOutput $output
}

$output = $newline + "Script execution complete."
LogAndWrite -LogOutput $output
