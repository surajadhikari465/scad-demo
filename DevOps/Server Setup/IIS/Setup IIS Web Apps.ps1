#######################################################################################
#######################################################################################
# IRMA Apps IIS Setup
# Tom Lux
# 2018.10.08
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

$appPoolDefinitionList = Get-Content "$scriptFolder\App Pool Defs.Dev.tsv"
$appPoolDefs = $appPoolDefinitionList.Split("`n")

$appDefinitionList = Get-Content "$scriptFolder\IIS Web App Defs.Dev.tsv"
$webAppDefs = $appDefinitionList.Split("`n")

$legacyAppDefinitionList = Get-Content "$scriptFolder\Legacy IIS Web App Defs.tsv"
$legacyWebAppDefs = $legacyAppDefinitionList.Split("`n")

$virtualDirDefinitionList = Get-Content "$scriptFolder\IIS Virtual Directory Defs.tsv"
$virtualDirDefs = $virtualDirDefinitionList.Split("`n")

$webServersText = Get-Content "$scriptFolder\Web Server List.Dev"
$webServers = @("IRMADevWeb02")#$webServersText.Split("`n")

#######################################################################################


"[Starting pool and app setup in IIS]"

"
-----------------------------"
"Loaded " + $appPoolDefs.count + " app-pool definitions."
"Loaded " + $webAppDefs.count + " web-app definitions."
"Loaded " + $legacyWebAppDefs.count + " legacy-app definitions."
"Loaded " + $virtualDirDefs.count + " virtual-directory definitions."
"Target Servers:"
$webServers
"-----------------------------"

$outputBreak
Write-Host -ForegroundColor Yellow "Next: All Web Setup"
If (-not (UserWantsToContinue))
{ ExitProcess }
$outputBreak

foreach ($webServer in $webServers)
{ 
    $outputBreak
    $webSvr = ([System.Net.Dns]::GetHostByName($webServer)).HostName

    "Starting IIS setup on server [" + $webServer + "]"

    Invoke-Command -ComputerName $webSvr -ScriptBlock {
        param($poolDefs, $appDefs, $legAppDefs, $vdirDefs, $r_previewMode)
        # Hard-coding the output-break here on the remote server (don't remove this line).
        $outputBreak = "---------------------------------------------------------------------"

        # Import WebAdministration to use IIS commandlets.
        # Depending on the server, one of the following module loads should work.
        # **Disabling ps-snapin cmd because import-module should work on our new web servers.
        #"Loading Web-Admin module... (it's okay if one of the following load attempts fail, but not both)"
        #"Attempting to load Web-Admin module via Add-PSSnapin cmd..."
        #Add-PSSnapin WebAdministration
        "Attempting to load Web-Admin module via Import-Module cmd..."
        Import-Module WebAdministration
    
        ################################################
        # Navigate to the app pools root.
        ################################################
        cd IIS:\AppPools\

        foreach($poolDef in $poolDefs){
            $outputBreak
            $poolAttributeList = $poolDef.Split("`t")
            $poolName = $poolAttributeList[0]
            $poolDotNetVersion = $poolAttributeList[1]
            $username = $poolAttributeList[2]
            $password = $poolAttributeList[3]
            $extProperty = $poolAttributeList[4]

            $hasExtProp = $false
            $extPropMsg = ""
            # Check if field has equals sign, which means there's an extended attribute defined.
            if($extProperty -like "*=*"){
                $hasExtProp = $true
                $extProp = $extProperty.Split("=")
                $extPropName = $extProp[0]
                $extPropValue = $extProp[1]
                $extPropMsg = " and extended property '$extPropName=$extPropValue'"
            }

            "Creating app pool '$poolName' for user '$username' and .NET '$poolDotNetVersion'$extPropMsg..."
        
            if(!(Test-Path $poolName -pathType container))
            {
                # Create the app pool.
                if($r_previewMode){
                    "[[PREVIEW.MODE]] New-WebAppPool, update, and start skipped"
                } else {
                    $pool = New-WebAppPool -Name $poolName -Force
                    $identitySet = $false
                    # https://docs.microsoft.com/en-us/iis/configuration/system.applicationhost/applicationpools/add/processmodel
                    if($username -like "LocalSystem"){ $pool.processModel.identityType = 0; $identitySet = $true }
                    if($username -like "LocalService"){ $pool.processModel.identityType = 1; $identitySet = $true }
                    if($username -like "NetworkService"){ $pool.processModel.identityType = 2; $identitySet = $true }
                    if($username -like "ApplicationPoolIdentity"){ $pool.processModel.identityType = 4; $identitySet = $true }
                    # If one of the generic users wasn't specified, we assume a specified user is specified.
                    if(!$identitySet){
                        $pool.processModel.username = $username
                        $pool.processModel.password = $password
                        $pool.processModel.identityType = 3
                    }
                    # Apply updates saved in pool object.
                    $pool | Set-Item
                    Set-ItemProperty ("IIS:\AppPools\$poolName") -Name managedRuntimeVersion -Value $poolDotNetVersion
                    "~~ Taking a slight pause (4 seconds) to allow IIS to process..."
                    Start-Sleep -s 4
                    "Starting App Pool..."
                    Start-WebAppPool -Name $poolName 
                }

            } else {
                Write-Host -ForegroundColor yellow ("**App pool [" + $poolName + "] already exists.")
            }

            if($hasExtProp){
                "Setting pool extended property: $extProp"
                Set-ItemProperty ("IIS:\AppPools\$poolName") -Name $extPropName -Value $extPropValue
            }
        }
        
        ################################################
        # Navigate to the sites root
        ################################################
        cd IIS:\Sites\

        foreach($appDef in $appDefs){
            $outputBreak
            $appAttributeList = $appDef.Split("`t")
            $appName = $appAttributeList[0]
            $dnsName = $appAttributeList[1]
            $poolName = $appAttributeList[2]
            $directoryPath = $appAttributeList[3]

            "Creating site '$appName' for DNS '$dnsName' and pool '$poolName' with path '$directoryPath'..."

            # Check if the site exists
            if(!(Test-Path $appName -pathType container)){
                # Create the site
                if($r_previewMode){
                    $iisApp = New-Item $appName -bindings @{protocol="http";bindingInformation=":80:" + $dnsName} -physicalPath $directoryPath -WhatIf
                    $iisApp | Set-ItemProperty -Name "applicationPool" -Value $poolName -WhatIf
                    # The set-web-config-prop call below fails if app doesn't exist, so we just print cmd.
                    "**Manual WHAT-IF** --> Set-WebConfigurationProperty -Filter ""/system.webServer/security/authentication/windowsAuthentication"" -Name Enabled -Value True -PSPath IIS:\ -location $appName"
            
                } else {
                    $iisApp = New-Item $appName -bindings @{protocol="http";bindingInformation=":80:" + $dnsName} -physicalPath $directoryPath
                    $iisApp | Set-ItemProperty -Name "applicationPool" -Value $poolName
                    Set-WebConfigurationProperty -Filter "/system.webServer/security/authentication/windowsAuthentication" -Name Enabled -Value True -PSPath IIS:\ -location $appName
                }
            } else {
                Write-Host -ForegroundColor yellow ("**Site [" + $appName + "] already exists.")
            }

        }

        ################################################
        # Setup legacy apps under sites
        ################################################
        foreach($legDef in $legAppDefs){
            $outputBreak
            $legacyAppDefinitionList = $legDef.Split("`t")
            $appName = $legacyAppDefinitionList[0]
            $siteName = $legacyAppDefinitionList[1]
            $poolName = $legacyAppDefinitionList[2]
            $directoryPath = $legacyAppDefinitionList[3]

            "Creating app '$appName' under site '$siteName' and pool '$poolName' with path '$directoryPath'..."
            # Check if the site exists
            if(!(Test-Path "$siteName\$appName")){
                if($r_previewMode){
                    "**Manual WHAT-IF** --> New-WebApplication -Name $appName -Site $siteName -PhysicalPath $directoryPath -ApplicationPool $poolName"
                } else {
                    New-WebApplication -Name $appName -Site $siteName -PhysicalPath $directoryPath -ApplicationPool $poolName
                }
            } else {
                Write-Host -ForegroundColor yellow ("**Legacy app [" + $appName + "] under site [" + $siteName + "] already exists.")
            }
        }

        ################################################
        # Setup virtual directories for sites
        ################################################
        foreach($vdirDef in $vdirDefs){
            $outputBreak
            $vdirAttributeList = $vdirDef.Split("`t")
            $siteName = $vdirAttributeList[0]
            $vdirName = $vdirAttributeList[1]
            $vdirPath = $vdirAttributeList[2]

            "Creating virtual directory '$vdirName' under site '$siteName' with path '$vdirPath'..."
            # Check if the site exists
            if(!(Get-WebVirtualDirectory -Site $siteName -Name $vdirName)){
                if($r_previewMode){
                    "**Manual WHAT-IF** --> New-WebVirtualDirectory -Site $siteName -Name $vdirName -PhysicalPath $vdirPath"
                } else {
                    New-WebVirtualDirectory -Site $siteName -Name $vdirName -PhysicalPath $vdirPath
                }
            } else {
                Write-Host -ForegroundColor yellow ("**Virtual directory [" + $vdirName + "] under site [" + $siteName + "] already exists.")
            }
        }
    } -ArgumentList $appPoolDefs, $webAppDefs, $legacyWebAppDefs, $virtualDirDefs, $previewMode


    "IIS setup on server [" + $webServer + "] is complete."
}


$outputBreak
"**SCRIPT COMPLETE**"
$outputBreak
Read-Host "Press <ENTER> to close log exit..."
ExitProcess
