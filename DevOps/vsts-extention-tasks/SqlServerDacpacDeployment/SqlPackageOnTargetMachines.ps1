﻿function RunCommand
{
    param(
        [string]$command,
        [bool] $failOnErr = $true
    )

    $ErrorActionPreference = 'Continue'

    if( $psversiontable.PSVersion.Major -le 4)
    {
        $result = cmd.exe /c "`"$command`""
    }
    else
    {
        $result = cmd.exe /c "$command"
    }

    $ErrorActionPreference = 'Stop'

    if($failOnErr -and $LASTEXITCODE -ne 0)
    {
        throw $result
    }

    return $result
}

function Get-SqlPackageOnTargetMachine
{
    try
    {
        $sqlDacPath, $sqlVersion = LocateHighestVersionSqlPackageWithSql
        $sqlVersionNumber = [decimal] $sqlVersion
    }
    catch [System.Exception]
    {
        Write-Verbose ("Failed to get Dac Framework (installed with SQL Server) location with exception: " + $_.Exception.Message)
        $sqlVersionNumber = 0
    }

    try
    {
        $sqlMsiDacPath, $sqlMsiVersion = LocateHighestVersionSqlPackageWithDacMsi
        $sqlMsiVersionNumber = [decimal] $sqlMsiVersion
    }
    catch [System.Exception]
    {
        Write-Verbose ("Failed to get Dac Framework (installed with DAC Framework) location with exception: " + $_.Exception.Message)
        $sqlMsiVersionNumber = 0
    }

    try
    {
        $vsDacPath, $vsVersion = LocateHighestVersionSqlPackageInVS
        $vsVersionNumber = [decimal] $vsVersion
    }
    catch [System.Exception]
    {
        Write-Verbose ("Failed to get Dac Framework (installed with Visual Studio) location with exception: " + $_.Exception.Message)
        $vsVersionNumber = 0
    }

    if (($vsVersionNumber -ge $sqlVersionNumber) -and ($vsVersionNumber -ge $sqlMsiVersionNumber))
    {
        $dacPath = $vsDacPath
    }
    elseif ($sqlVersionNumber -ge $sqlMsiVersionNumber)
    {
        $dacPath = $sqlDacPath
    }
    else
    {
        $dacPath = $sqlMsiDacPath
    }

    if ($dacPath -eq $null)
    {
        throw  "Unable to find the location of Dac Framework (SqlPackage.exe) from registry on machine $env:COMPUTERNAME"
    }
    else
    {
        return $dacPath
    }
}

function Get-RegistryValueIgnoreError
{
    param
    (
        [parameter(Mandatory = $true)]
        [Microsoft.Win32.RegistryHive]
        $RegistryHive,

        [parameter(Mandatory = $true)]
        [System.String]
        $Key,

        [parameter(Mandatory = $true)]
        [System.String]
        $Value,

        [parameter(Mandatory = $true)]
        [Microsoft.Win32.RegistryView]
        $RegistryView
    )

    try
    {
        $baseKey = [Microsoft.Win32.RegistryKey]::OpenBaseKey($RegistryHive, $RegistryView)
        $subKey =  $baseKey.OpenSubKey($Key)
        if($subKey -ne $null)
        {
            return $subKey.GetValue($Value)
        }
    }
    catch
    {
    }
    return $null
}

function Get-SubKeysInFloatFormat($keys)
{
    $targetKeys = @()
        foreach ($key in $keys)
        {
            try {
                $targetKeys += [decimal] $key
            }
            catch {}
        }

    $targetKeys
}

function Get-SqlPackageForSqlVersion([int] $majorVersion, [bool] $wow6432Node)
{
    $sqlInstallRootRegKey = "SOFTWARE", "Microsoft", "Microsoft SQL Server", "$majorVersion" -join [System.IO.Path]::DirectorySeparatorChar

    if ($wow6432Node -eq $true)
    {
        $sqlInstallRootPath = Get-RegistryValueIgnoreError LocalMachine "$sqlInstallRootRegKey" "VerSpecificRootDir" Registry64
    }
    else
    {
        $sqlInstallRootPath = Get-RegistryValueIgnoreError LocalMachine "$sqlInstallRootRegKey" "VerSpecificRootDir" Registry32
    }

    if ($sqlInstallRootPath -eq $null)
    {
        return $null
    }

    Write-Verbose "Sql Version Specific Root Dir for version $majorVersion as read from registry: $sqlInstallRootPath"

    $DacInstallPath = [System.IO.Path]::Combine($sqlInstallRootPath, "Dac", "bin", "SqlPackage.exe")

    if (Test-Path $DacInstallPath)
    {
        Write-Verbose "Dac Framework installed with SQL Version $majorVersion found at $DacInstallPath on machine $env:COMPUTERNAME"
        return $DacInstallPath
    }
    else
    {
        return $null
    }
}

function LocateHighestVersionSqlPackageWithSql()
{
    $sqlRegKey = "HKLM:", "SOFTWARE", "Wow6432Node", "Microsoft", "Microsoft SQL Server"-join [System.IO.Path]::DirectorySeparatorChar
    $sqlRegKey64 = "HKLM:", "SOFTWARE", "Microsoft", "Microsoft SQL Server"-join [System.IO.Path]::DirectorySeparatorChar

    if (-not (Test-Path $sqlRegKey))
    {
        $sqlRegKey = $sqlRegKey64
    }

    if (-not (Test-Path $sqlRegKey))
    {
        return $null, 0
    }

    $keys = Get-Item $sqlRegKey | %{$_.GetSubKeyNames()} 
    $versions = Get-SubKeysInFloatFormat $keys | Sort-Object -Descending

    Write-Verbose "Sql Versions installed on machine $env:COMPUTERNAME as read from registry: $versions"

    foreach ($majorVersion in $versions)
    {
        $DacInstallPathWow6432Node = Get-SqlPackageForSqlVersion $majorVersion $true
        $DacInstallPath = Get-SqlPackageForSqlVersion $majorVersion $false

        if ($DacInstallPathWow6432Node -ne $null)
        {
            return $DacInstallPathWow6432Node, $majorVersion
        }
        elseif ($DacInstallPath -ne $null)
        {
            return $DacInstallPath, $majorVersion
        }
    }

    Write-Verbose "Dac Framework (installed with SQL) not found on machine $env:COMPUTERNAME"      

    return $null, 0
}

function LocateHighestVersionSqlPackageWithDacMsi()
{
    $sqlRegKeyWow = "HKLM:", "SOFTWARE", "Wow6432Node", "Microsoft", "Microsoft SQL Server", "DACFramework", "CurrentVersion" -join [System.IO.Path]::DirectorySeparatorChar
    $sqlRegKey = "HKLM:", "SOFTWARE", "Microsoft", "Microsoft SQL Server", "DACFramework", "CurrentVersion" -join [System.IO.Path]::DirectorySeparatorChar

    $sqlKey = "SOFTWARE", "Microsoft", "Microsoft SQL Server", "DACFramework", "CurrentVersion" -join [System.IO.Path]::DirectorySeparatorChar

    if (Test-Path $sqlRegKey)
    {
        $dacVersion = Get-RegistryValueIgnoreError LocalMachine "$sqlKey" "Version" Registry64
        $majorVersion = $dacVersion.Substring(0, $dacVersion.IndexOf(".")) + "0"
    }

    if (Test-Path $sqlRegKeyWow)
    {
        $dacVersionX86 = Get-RegistryValueIgnoreError LocalMachine "$sqlKey" "Version" Registry32
        $majorVersionX86 = $dacVersionX86.Substring(0, $dacVersionX86.IndexOf(".")) + "0"
    }

    if ((-not($dacVersion)) -and (-not($dacVersionX86)))
    {
        Write-Verbose "Dac Framework (installed with DAC Framework) not found on machine $env:COMPUTERNAME"
        return $null, 0
    }

    if ($majorVersionX86 -gt $majorVersion)
    {
        $majorVersion = $majorVersionX86
    }

    $dacRelativePath = "Microsoft SQL Server", "$majorVersion", "DAC", "bin", "SqlPackage.exe" -join [System.IO.Path]::DirectorySeparatorChar
    $programFiles = $env:ProgramFiles
    $programFilesX86 = "${env:ProgramFiles(x86)}"

    if (-not ($programFilesX86 -eq $null))
    {
        $dacPath = $programFilesX86, $dacRelativePath -join [System.IO.Path]::DirectorySeparatorChar

        if (Test-Path("$dacPath"))
        {
            Write-Verbose "Dac Framework (installed with DAC Framework Msi) found on machine $env:COMPUTERNAME at $dacPath"
            return $dacPath, $majorVersion
        }
    }

    if (-not ($programFiles -eq $null))
    {
        $dacPath = $programFiles, $dacRelativePath -join [System.IO.Path]::DirectorySeparatorChar

        if (Test-Path($dacPath))
        {
            Write-Verbose "Dac Framework (installed with DAC Framework Msi) found on machine $env:COMPUTERNAME at $dacPath"
            return $dacPath, $majorVersion
        }
    }

    return $null, 0
}

function LocateSqlPackageInVS([string] $version)
{
    $vsRegKeyForVersion = "SOFTWARE", "Microsoft", "VisualStudio", $version -join [System.IO.Path]::DirectorySeparatorChar

    $vsInstallDir = Get-RegistryValueIgnoreError LocalMachine "$vsRegKeyForVersion" "InstallDir" Registry64

    if ($vsInstallDir -eq $null)
    {
        $vsInstallDir = Get-RegistryValueIgnoreError LocalMachine "$vsRegKeyForVersion"  "InstallDir" Registry32
    }

    if ($vsInstallDir)
    {
        Write-Verbose "Visual Studio install location: $vsInstallDir" 

        $dacExtensionPath = [System.IO.Path]::Combine("Extensions", "Microsoft", "SQLDB", "DAC")
        $dacParentDir = [System.IO.Path]::Combine($vsInstallDir, $dacExtensionPath)

        if (Test-Path $dacParentDir)
        {
            $dacVersionDirs = Get-ChildItem $dacParentDir | Sort-Object @{e={$_.Name -as [int]}} -Descending

            foreach ($dacVersionDir in $dacVersionDirs)
            {
                $dacVersion = $dacVersionDir.Name
                $dacFullPath = [System.IO.Path]::Combine($dacVersionDir.FullName, "SqlPackage.exe")

                if(Test-Path $dacFullPath -pathtype leaf)
                {
                    Write-Verbose "Dac Framework installed with Visual Studio found at $dacFullPath on machine $env:COMPUTERNAME"
                    return $dacFullPath, $dacVersion
                }
                else
                {
                    Write-Verbose "Unable to find Dac framework installed with Visual Studio at $($dacVersionDir.FullName) on machine $env:COMPUTERNAME"
                }
            }
        }
    }

    return $null, 0
}

function LocateHighestVersionSqlPackageInVS()
{
    $vsRegKey = "HKLM:", "SOFTWARE", "Wow6432Node", "Microsoft", "VisualStudio" -join [System.IO.Path]::DirectorySeparatorChar
    $vsRegKey64 = "HKLM:", "SOFTWARE", "Microsoft", "VisualStudio" -join [System.IO.Path]::DirectorySeparatorChar

    if (-not (Test-Path $vsRegKey))
    {
        $vsRegKey = $vsRegKey64
    }

    if (-not (Test-Path $vsRegKey))
    {
        Write-Verbose "Visual Studio not found on machine $env:COMPUTERNAME"
        return $null, 0
    }

    $keys = Get-Item $vsRegKey | %{$_.GetSubKeyNames()}
    $versions = Get-SubKeysInFloatFormat $keys | Sort-Object -Descending 

    Write-Verbose "Visual Studio versions found on machine $env:COMPUTERNAME as read from registry: $versions"

    foreach ($majorVersion in $versions)
    {
        $dacFullPath, $dacVersion = LocateSqlPackageInVS $majorVersion

        if ($dacFullPath -ne $null)
        {
            return $dacFullPath, $dacVersion
        }
    }

    Write-Verbose "Dac Framework (installed with Visual Studio) not found on machine $env:COMPUTERNAME"

    return $null, 0
}

function Get-SqlPackageCmdArgs
{
    param (
    [string]$dacpacFile,
    [string]$targetMethod,
    [string]$serverName,
    [string]$databaseName,
    [string]$authscheme,
    [string]$sqlUsername,
    [string]$sqlPassword,
    [string]$connectionString,
    [string]$publishProfile,
    [string]$additionalArguments,
	[string]$action,
	[string]$outputPath
    )

    # validate dacpac file
    if ([System.IO.Path]::GetExtension($dacpacFile) -ne ".dacpac")
    {
        throw "Invalid Dacpac file [ $dacpacFile ] provided"
    }

    $sqlPkgCmdArgs = [string]::Format(' /SourceFile:"{0}" /Action:{1}', $dacpacFile, $action)

    if($targetMethod -eq "server")
    {
        $sqlPkgCmdArgs = [string]::Format('{0} /TargetServerName:"{1}"', $sqlPkgCmdArgs, $serverName)
        if ($databaseName)
        {
            $sqlPkgCmdArgs = [string]::Format('{0} /TargetDatabaseName:"{1}"', $sqlPkgCmdArgs, $databaseName)
        }

        if($authscheme -eq "SQL Server Authentication")
        {
            $sqlPkgCmdArgs = [string]::Format('{0} /TargetUser:"{1}" /TargetPassword:"{2}"', $sqlPkgCmdArgs, $sqlUsername, $sqlPassword)
        }
    }
    elseif($targetMethod -eq "connectionString")
    {
        $sqlPkgCmdArgs = [string]::Format('{0} /TargetConnectionString:"{1}"', $sqlPkgCmdArgs, $connectionString)
    }

    if( ![string]::IsNullOrWhiteSpace($publishProfile) )
    {
        # validate publish profile
        if ([System.IO.Path]::GetExtension($publishProfile) -ne ".xml")
        {
            throw "Invalid Publish Profile [ $publishProfile ] provided"
        }
        $sqlPkgCmdArgs = [string]::Format('{0} /Profile:"{1}"', $sqlPkgCmdArgs, $publishProfile)
    }

	if((-not [string]::IsNullOrEmpty($outputPath)) -and ($action -eq 'script')){
		$sqlPkgCmdArgs = [string]::Format('{0} /OutputPath:"{1}"', $sqlPkgCmdArgs, $outputPath)
	}

    $sqlPkgCmdArgs = [string]::Format('{0} {1}', $sqlPkgCmdArgs, $additionalArguments)
    Write-Verbose "Sqlpackage.exe arguments : $sqlPkgCmdArgs"
    return $sqlPkgCmdArgs
}

function ExecuteMain
{
    param (
     [string]$dacpacFile,
     [string]$targetMethod,
     [string]$serverName,
     [string]$databaseName,
     [string]$authscheme,
     [string]$sqlUsername,
     [string]$sqlPassword,
     [string]$connectionString,
     [string]$publishProfile,
     [string]$additionalArguments,
	 [string]$action,
	 [string]$outputPath
    )

    Write-Verbose "Entering script SqlPackageOnTargetMachines.ps1"
    $sqlPackage = Get-SqlPackageOnTargetMachine
    $sqlPackageArguments = Get-SqlPackageCmdArgs -dacpacFile $dacpacFile -targetMethod $targetMethod -serverName $serverName -databaseName $databaseName -authscheme $authscheme -sqlUsername $sqlUsername -sqlPassword $sqlPassword -connectionString $connectionString -publishProfile $publishProfile -additionalArguments $additionalArguments -action $action -outputPath $outputPath

    $command = "`"$sqlPackage`" $sqlPackageArguments"
    Write-Verbose "Executing command: $command" -Verbose
    $result = RunCommand -command $command -failOnErr $true
	return $result
}

