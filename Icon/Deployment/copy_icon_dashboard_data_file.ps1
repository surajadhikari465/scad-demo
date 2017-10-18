# copies file on remote web server
# for use when deploying/releasing the icon dashboard
# The dashboard uses an xml data file to store data
#   and the normal website deploy release step overwrites this file
# This script copies the existing data file to a backup folder
#   and can also be used to copy the file back to the target folder later

# expected arguments:
# args[0]: name of data file, including extension, e.g. "DashboardApplicationsTEST.xml"
# args[1]: source folder on the remote server, e.g. "E:\WebApps\IconDashboard\App_Data"
# args[2]: destination folder on the remote server, e.g. "E:\WebApps\IconDashboardBackup"
# args[3]: webServer(s), eg. "cewd6589,cewd6590"
# args[4]: user name with permissions to read/write on the server, e.g. "wfm\icon.deploy.nonprod"
# args[5]: password for user, e.g. "1234*****"

#set up script variables from expected arguments
$ext = $args[0].Split(".")[1]
$srcpath = $args[1]
$dstpath = $args[2]
$webServers = $args[3]
$username = $args[4]
$password = ConvertTo-SecureString -AsPlainText $args[5] -Force

$creds = new-object -typename System.Management.Automation.PSCredential -argumentlist $username, $password

Foreach ($webServer in $webServers.GetEnumerator())
{ 
    Invoke-Command -ComputerName $webServer -Credential $creds  -ScriptBlock {
    param($file, $ext, $srcPath, $destPath)

    #create backup folder in case it does not exist
    New-Item -ItemType Directory -Force -Path $destPath
    
    $srcFile = "${srcPath}\${file}.${ext}"
    $destFile = "${destPath}\${file}.${ext}"
   
    Write-Host "Looking for existing data file '${srcFile}'"
    if (Test-Path ($srcFile))
    {
        #check if another copy is already there
        if (Test-Path ($destFile))
        {
            #rename the existing
            $lastMod = (Get-Item $destFile).LastWriteTime.ToString("yyyyMMddHHmm")
            $newName = "${destPath}\${file}${lastMod}.${ext}"
            if (Test-Path ($newName))
            {
                #Write-Host "not backing up data file because copy '${newName}' already exists"
                # copy over the new file, there is already a backup of the current copy
                Write-Host "Overwriting existing backup file ${destFile}"
                Copy-Item $srcFile -Destination $destPath
            }
            else 
            {      
                Write-Host "Backing up existing backup file as ${newName}"
                Copy-Item $destFile -Destination $newName
                Write-Host " and copying current data file to ${destPath}"
                Copy-Item $srcFile -Destination $destPath
            }
        }
        else
        {
            Write-Host "...and backing up data file to ${destPath}"
            Copy-Item $srcFile -Destination $destPath
        }
    } 
    else
    {
        Write-Host "... but source data not found"
    }
    
    } -ArgumentList ($datafile, $ext,$srcpath, $dstpath)
}
