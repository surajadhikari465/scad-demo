<#

This script relies on release-component entries in the idt-ce\ced.ReleasePlan database.
See Tom Lux for details.

#>


$suite = read-host -prompt "Enter application suite name (IRMA or ICON)"

$version = read-host -prompt "Enter release version (ex: 1.1)"
$environment = read-host -prompt "Enter environment (Test, QA, Prd)"
$releaseName = $suite + " v" + $version

$stagingSvrAlias = "irmaqafile"

# Default staging location.
$stagingLocalBaseFolder = "e:\irma\staging\$suite\"
$stagingBaseFolder = "\\" + $stagingSvrAlias + "\e$\irma\staging\$suite\"
# Special staging location for IRMA.
if ($suite -like "irma"){
    $stagingLocalBaseFolder = "e:\irma\staging\release\"
    $stagingBaseFolder = "\\" + $stagingSvrAlias + "\e$\irma\staging\release\"
}

# IRMA or ICON
$targetTfs = $suite.ToUpper()

$versionExt = $version
if ($version.Split(".").Count -eq 2){
    $versionExt += ".0"
}

"Getting release instance details..."
$releaseQuery = "exec dbo.GetReleaseInstanceDetails @suiteName = '" + $suite + "', @version = '" + $version + "'"
$componentsQuery = "exec dbo.GetReleaseComponents @suiteName = '" + $suite + "', @version = '" + $version + "'"

$releaseSqlResults = Invoke-Sqlcmd -ServerInstance idt-ce\ced -Database releaseplan -Query $releaseQuery

$components = Invoke-Sqlcmd -ServerInstance idt-ce\ced -Database releaseplan -Query $componentsQuery

# Make sure we aren't in the SQLSERVER provider/prompt, so switch to default drive.
c:

foreach ($component in $components){
    "----------------------------------------------------"
    "Name: " + $component.ComponentName

    # For use with remote command, but not working...
    #$appStagingFolder = $stagingLocalBaseFolder + "$version\$environment\" + $component.ComponentStagingFolder + "\"

    $appStagingFolder = $stagingBaseFolder + "$version\$environment\" + $component.ComponentStagingFolder + "\"
    # Slightly different staging folder structure and version refs for IRMA components:
    if ($suite -like "irma"){
        # Staging folders are version refs for IRMA.
        $appStagingFolder = $stagingBaseFolder + "$versionExt\" + $component.ComponentStagingFolder + "\$environment\"
        if($environment -like "prd"){
            $appStagingFolder = $stagingBaseFolder + "$versionExt\" + $component.ComponentStagingFolder + "\$versionExt\"
        }
    }

    "Staging local folder, server=" + $stagingSvrAlias + ": " + $appStagingFolder

    $stagingSvrHostname = ([System.Net.Dns]::GetHostByName($stagingSvrAlias)).HostName

    $componentStagedFiles = Get-ChildItem -Recurse -Name ($appStagingFolder + "*")
    # Remote command not working... something wrong with WinRM on IRMAQAFile...
    #$componentStagedFiles = Invoke-Command -ComputerName $stagingSvrHostname -ScriptBlock {
    #    param($folder)
    #    Get-ChildItem -Recurse -Name ($folder + "*")
    #} -ArgumentList $appStagingFolder
    "Staged File Count: " + $componentStagedFiles.count

    #Get-ChildItem -Recurse "\\irmaqafile\e$\irma\staging\icon\6.1\prd\Item Movement Listener\*.dll" | Sort-Object -Property LastWriteTime -Descending | Select-Object -First 1
    $stagedLatestModFile = Get-ChildItem -Recurse ($appStagingFolder + "*.dll") | Sort-Object -Property LastWriteTime -Descending | Select-Object -First 1
    "Staged Latest Mod: " + $stagedLatestModFile.name + " @ " + $stagedLatestModFile.lastwritetime


    # Web apps go to different servers.
    if($component.ComponentType.ToLower() -eq "web"){
        $deploySvrAlias = "irma" + $environment + "app1"
        #$appDeployFolder = "\\" + $svrHostname + "\e$\webapps\" + $component.ComponentDeployFolder + "\" + $environment + "\"
        $appDeployFolder = "e:\webapps\" + $component.ComponentDeployFolder + "\" + $environment + "\"
        # Prod deploy folders for IRMA are version refs.
        if ($suite -like "irma" -and $environment -like "prd"){
            $appDeployFolder = "e:\webapps\" + $component.ComponentDeployFolder + "\" + $versionExt + "\"
        }

    } else {
        # All other apps go to "job" servers.
        $deploySvrAlias = "vm-icon-" + $environment + "1"
        #$appDeployFolder = "\\" + $svrHostname + "\e$\icon\" + $component.ComponentDeployFolder + "\" + $environment + "\"
        $appDeployFolder = "e:\icon\" + $component.ComponentDeployFolder + "\" + $environment + "\"
        # Prod deploy folders for IRMA are version refs.
        if ($suite -like "irma" -and $environment -like "prd"){
            $deploySvrAlias = "irma" + $environment + "file"
            $appDeployFolder = "e:\scheduledjobs\" + $component.ComponentDeployFolder + "\" + $versionExt + "\"
        }
    }

    $deploySvrHostname = ([System.Net.Dns]::GetHostByName($deploySvrAlias)).HostName
    "Deploy local folder, server=" + $deploySvrAlias + ": " + $appDeployFolder
    
    #$componentDeployedFiles = Get-ChildItem -Recurse -Name ($appDeployFolder + "*")
    $componentDeployedFiles = Invoke-Command -ComputerName $deploySvrHostname -ScriptBlock {
        param($folder)
        Get-ChildItem -Recurse -Name ($folder + "*")
    } -ArgumentList $appDeployFolder
    "Deploy File Count: " + $componentDeployedFiles.count

    $deployLatestModFile = Invoke-Command -ComputerName $deploySvrHostname -ScriptBlock {
        param($folder)
        Get-ChildItem -Recurse -Path $folder -Filter "*.dll" | Sort-Object -Property LastWriteTime -Descending | Select-Object -First 1
    } -ArgumentList $appDeployFolder
    "Deploy Latest Mod: " + $deployLatestModFile.name + " @ " + $deployLatestModFile.lastwritetime
}

