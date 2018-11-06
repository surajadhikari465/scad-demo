#######################################################################################
#######################################################################################
# IRMA Apps IIS Setup
# Tom Lux
# 2018.10.08
#######################################################################################
#######################################################################################

# Creates a set of app pools and sites based on input files.


#######################################################################################

$PreviewMode = $false

$appPoolDefinitionList = Get-Content "\\irmaqafile\e$\IRMA\Staging\docs\Projects\IIS Web App Setup\App Pool Defs.Dev.tsv"
$appPoolDefs = $appPoolDefinitionList.Split("`n")

$appDefinitionList = Get-Content "\\irmaqafile\e$\IRMA\Staging\docs\Projects\IIS Web App Setup\IIS Web App Defs.Dev.tsv"
$webAppDefs = $appDefinitionList.Split("`n")

$legacyAppDefinitionList = Get-Content "\\irmaqafile\e$\IRMA\Staging\docs\Projects\IIS Web App Setup\Legacy IIS Web App Defs.tsv"
$legacyWebAppDefs = $legacyAppDefinitionList.Split("`n")

$webServersText = Get-Content "\\irmaqafile\e$\IRMA\Staging\docs\Projects\IIS Web App Setup\Web Server List.Dev"
$webServers = @("IRMADevWeb01")#$webServersText.Split("`n")

#######################################################################################

if($PreviewMode){
    Write-Host -ForegroundColor DarkYellow "<< PREVIEW MODE >>`n<< PREVIEW MODE >>`n<< PREVIEW MODE >>`n<< PREVIEW MODE >>`n<< PREVIEW MODE >>`n<< PREVIEW MODE >>`n<< PREVIEW MODE >>`n<< PREVIEW MODE >>`n<< PREVIEW MODE >>`n<< PREVIEW MODE >>`n"
}

$output = "Starting pool and app setup in IIS..."
$output

"
-----------------------------"
"Loaded " + $appPoolDefs.count + " app-pool definitions."
"Loaded " + $webAppDefs.count + " web-app definitions."
"Loaded " + $legacyWebAppDefs.count + " legacy-app definitions."
"Target Servers:"
$webServers
"-----------------------------"

Read-Host "Press <ENTER> to continue..."

Foreach ($webServer in $webServers)
{ 
    "`n========================================================="
    $webSvr = ([System.Net.Dns]::GetHostByName($webServer)).HostName

    $output = "Starting IIS setup on server " + $webServer
    $output


    Invoke-Command -ComputerName $webSvr -ScriptBlock {
        param($poolDefs, $appDefs, $legAppDefs, $r_previewMode)
    
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

            $poolAttributeList = $poolDef.Split("`t")
            $poolName = $poolAttributeList[0]
            $poolDotNetVersion = $poolAttributeList[1]
            $username = $poolAttributeList[2]
            $password = $poolAttributeList[3]

            "Creating app pool '$poolName' for user '$username' and .NET '$poolDotNetVersion'..."
        
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
        }
        
        ################################################
        # Navigate to the sites root
        ################################################
        cd IIS:\Sites\

        foreach($appDef in $appDefs){

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

    } -ArgumentList $appPoolDefs, $webAppDefs, $legacyWebAppDefs, $previewMode


    $output = "IIS setup on server " + $webServer + " is complete."
    $output
}


