
#------------------------Script Detials---------BEGIN---------------------------------------------------
#Removes content of mapped directory ($directoryPath) if exists
#Creates the mapped direcotry ($directoryPath) if does not exist
#Copies the source files from stage folder to mapped directory
#Creates IIS app pool ($iisAppPoolName) if does not exist
#Created webb app or standalone website ($iisAppName) if does not exist
#------------------------Script Detials---------END-----------------------------------------------------


#Uncommnet below  importing WebAdministration if doing on local machine and commnet out importin in ScriptBlock  
#Import-Module WebAdministration

#---------------Varible values ----------------BEGIN------------------------------------

#Flag to inidcate to set up as standalone web site or application inder default website
$setUpAsAppUnderDefault = $false
$iisAppPoolName = "my-test-app"
$iisAppPoolDotNetVersion = "v4.0"
$iisAppName = "my-default-app"
$directoryPath = "E:\WebApps\testapp"
#$stagingPath = "\\irmaqafile\e$\IRMA\Staging\Nutrition\" + $version + "\" + $environment
$stagingPath = "E:\WebApps\Nutrition\Test"
$dnsName = "icon-nutrition-test"
$username = "WFM\NutriconServiceDev"  
$password = "wAb1TI*M81" 
$appServers = @('irmatestapp1', 'irmatestapp2', 'irmatestapp3', 'irmatestapp4')
#---------------varible values ----------------END------------------------------------


$sourceContent = $stagingPath + '\*'
$destination = $directoryPath


Foreach ($webServer in $appServers.GetEnumerator())
{ 

    $webSvr = ([System.Net.Dns]::GetHostByName($webServer)).HostName

    Invoke-Command -ComputerName $webSvr -ScriptBlock {
        param($directory)

        $contentsToDelete = $directory + "\*"

        If ((Test-Path $directory))
        {
            Remove-Item -Path $contentsToDelete -Recurse
        }
    } -ArgumentList $directoryPath


    Invoke-Command -ComputerName $webSvr -ScriptBlock {
        param($sourceDirectory, $destDirectory)

        # Create the dest folder, if it does not exist
        If (-not (Test-Path $destDirectory))
        {
            New-Item -ItemType Directory -Path $destDirectory
        }
        #Copy items from source to destination
        Copy-Item -Path $sourceDirectory -Destination $destDirectory -Recurse
        
    } -ArgumentList $sourceContent, $destination
   



    Invoke-Command -ComputerName $webSvr -ScriptBlock {
        param($iisAppPoolName, $iisAppPoolDotNetVersion, $iisAppName, $directoryPath, $setUpAsAppUnderDefault, $dnsName, $username, $password)
    
        #Import WebAdministration to use IIS commandlets
        Add-PSSnapin WebAdministration;
    
   
        #navigate to the app pools root
        cd IIS:\AppPools\

        if (!(Test-Path $iisAppPoolName -pathType container))
        {
            #create the app pool       

            $pool = New-WebAppPool -Name $iisAppPoolName -Force
            $pool.processModel.username = $username
            $pool.processModel.password = $password
            $pool.processModel.identityType = 3
            $pool | Set-Item 

            Set-ItemProperty ("IIS:\AppPools\$iisAppPoolName") -Name managedRuntimeVersion -Value "v4.0"

            Start-WebAppPool -Name $iisAppPoolName 
        }

        #Below will be used when we set up usign DNS name
        # -------------DNS set up as web site --- Begin -----------------

        if($setUpAsAppUnderDefault -eq $false)
        {
            #navigate to the sites root
            cd IIS:\Sites\

            #check if the site exists
            if (Test-Path $iisAppName -pathType container)
            {
                return
            }

            #create the site
        
            #$iisApp = New-Item $iisAppName -bindings @{protocol="http";bindingInformation=":8080:" + $iisAppName} -physicalPath $directoryPath

            #Use below one if using DNS instead of above line
            $iisApp = New-Item $iisAppName -bindings @{protocol="http";bindingInformation=":80:" + $dnsName} -physicalPath $directoryPath

            $iisApp | Set-ItemProperty -Name "applicationPool" -Value $iisAppPoolName
        }
        # -------------DNS set up as web site --- End -----------------


        #---  Wihtout DNS name add application to default web site --- BEGIN---------------------------
        else
        {
            $defaultAppPath = "IIS:\Sites\Default Web Site\";
            
            if((Test-Path $defaultAppPath$iisAppName) -eq 0 -and (Get-WebApplication -Name $iisAppName) -eq $null)
            {
                If (-not (Test-Path $destDirectory))
                {
                    New-Item -ItemType directory -Path $directoryPath; 
                }

                New-WebApplication -Name $iisAppName -ApplicationPool $iisAppPoolName -Site "Default Web Site" -PhysicalPath $directoryPath;
            }
            elseif((Get-WebApplication -Name $iisAppName) -eq $null -and (Test-Path $defaultAppPath$iisAppName) -eq $true)
            {
                ConvertTo-WebApplication -ApplicationPool $iisAppName $appPath$appName;
            }
            else
            {
                echo "$iisAppName already exists";
            }
        }
        #---  Wihtout DNS name add application to default web site --- END---------------------------

    } -ArgumentList $iisAppPoolName, $iisAppPoolDotNetVersion, $iisAppName, $directoryPath, $setUpAsAppUnderDefault, $dnsName, $username, $password
}

    