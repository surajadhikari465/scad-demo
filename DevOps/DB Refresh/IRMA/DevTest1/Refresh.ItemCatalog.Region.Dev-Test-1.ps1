#######################################################################################
#######################################################################################
# SCAD DB Refresh
# Tom Lux
# Summer, 2019
#######################################################################################
#######################################################################################

# Steps through SQL scripts and commands to restore an IRMA DB from Prod.

<#
HISTORY:
2019.07.01 - Tom Lux - Initial, consolidated, full-process script.

2019.07.02:
- Tom Lux - Added region and username to end of log-file name.
- Tom Lux - New var $diffRestoreRuntimeScript is unique to region, so concurrent refreshes should not accidentially use diff-restore cmd for other regions.

2019.08.21:
- Tom Lux - Expanded to prompt for dev1 or test1 env, so either could be done from this script.
- Tom Lux - Show local assumed path and switch to e: drive first.

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



######################## TO-DO: get this into source control and it should refer to ??\common\ps\Functions.ps1 like server-setup scripts do.
$fnScript = "$scriptFolder\Functions.ps1"






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
$sqlCmdApp = "C:\Program Files (x86)\Microsoft SQL Server\Client SDK\ODBC\130\Tools\Binn\sqlcmd.exe"
$outputBreak = "--------------------------------------------------------------------------------"

$region = Read-Host "Enter 2-letter region ID to be refreshed from Prod"

# Add something to log-file name to help identify who/what DB we're refreshing.
$logFile = "\\irmaprdfile2\e$\devops\db.refresh\logs\" + $scriptFileObj.Name + "." + (Get-Date -Format "yyyyMMdd.HHmmss") + ".$env:UserName" + ".$region" + ".log"
Start-Transcript -Path $logFile -Append

$dbEnv = Read-Host "Enter target env (dev or tst01)"
$dbServer = "$region-db01-$dbEnv"
$dbName = "ItemCatalog"

$diffRestoreTemplateFile = "Refresh.Dynamic Diff Restore.ItemCatalog--TEMPLATE--.sql"
# This needs to be unique to the exact restore-script instance so that multiple refreshes do not try to use the same SQL file.
$diffRestoreRuntimeScript = "Refresh.Dynamic Diff Restore.ItemCatalog.$region.sql"
$diffRestoreCmdPlaceholder = "%DiffRestoreCommandSql%"

$outputBreak
############### TODO: we should just switch to the folder where this script lives!
$localPath = "E:\devops\refresh\ItemCatalog"
"**NOTE: Assuming we're running from this e-drive location: $localPath"
e:
cd $localPath
$outputBreak

$outputBreak
"[[Refresh Script Starting]]"
Get-Date
$outputBreak




######################## TO-DO: get this from a central place (file in source control?, pass it in?) so it's not hard-coded
$orderedTablesToPreserveData = "dbo.InstanceDataFlagsStoreOverride dbo.InstanceDataFlags dbo.StoreFTPConfig dbo.AppConfigApp dbo.AppConfigValue dbo.AppConfigKey"





$sqlScriptList = @(
#"Refresh.Kill Connections.ItemCatalog.sql",
"Refresh.Set DB RestrictedUser Access.ItemCatalog.sql",
"ExportTablesToPreserve",
"Refresh.Dynamic Generate Diff Restore Command.ItemCatalog.sql",
"Refresh.Dynamic Full Restore.ItemCatalog.sql",
$diffRestoreRuntimeScript,
"Refresh.Call DBOwner Job.All.sql",
"Refresh.Set DB RestrictedUser Access.ItemCatalog.sql"
"Refresh.Set DB Recovery Simple.ItemCatalog.sql",
"Refresh.Remap Permissions.Test.ItemCatalog.sql",
"ClearTables",
"ImportTablesPreserved",
"Refresh.Set Env.Test.ItemCatalog.sql",
"Refresh.Clear Prod AppConfig Data.ItemCatalog.sql",
"Refresh.Clear Vendor Contact Info.ItemCatalog.sql",
"Refresh.Clear Closed-Store Push Config.ItemCatalog.sql",
"Refresh.Setup Testing-Access Users.Dynamic.ItemCatalog.sql",
"Refresh.Set DB MultiUser Access.ItemCatalog.sql"
)


"Running script list for SVR=$dbServer, DB=$dbName ::"
$sqlScriptList

Read-Host "Press ENTER to continue..."

foreach($sqlScript in $sqlScriptList){
    $outputBreak
    "[" + (Get-Date) + "] Step :: $sqlScript"

    if($sqlScript -like "ExportTablesToPreserve"){
        -split $orderedTablesToPreserveData | foreach { Export-SqlTable -Server $dbServer -Database $dbName -Table $_ -CharType -TrustedAuth }
        continue
    }
    if($sqlScript -like "ClearTables"){
        -split $orderedTablesToPreserveData | foreach { Clear-SqlTable -Server $dbServer -Database $dbName -Table $_ }
        continue
    }
    if($sqlScript -like "ImportTablesPreserved"){
        -split $orderedTablesToPreserveData | foreach { Import-SqlTable -Server $dbServer -Database $dbName -Table $_ -CharType -TrustedAuth -KeepNulls -KeepIdentityValues -Verbose }
        continue
    }
    if($sqlScript -like "*generate diff restore*"){
        # Run script to generate diff-restore command and capture that output.
        [string]$genSql = . $sqlCmdApp -s "`t" -w 8000 -W -S $dbServer -d master -i $sqlScript -t 14400
        # We're replacing SQL inside a string-variable in a script, so all single-quotes must be escaped to two consecutive single-quotes.
        $genSql = $genSql.Replace("'", "''")
        # Get diff-restore template SQL.
        $diffRestoreTemplateSql = gc $diffRestoreTemplateFile
        # Replace the command placeholder with the generated SQL we captured.
        $diffRestoreSql = $diffRestoreTemplateSql.Replace($diffRestoreCmdPlaceholder, $genSql)
        # Write out to upcoming script that will be executed by this runner.
        Out-File -FilePath $diffRestoreRuntimeScript -InputObject $diffRestoreSql
        # Nothing more to do for current step.
        continue
    }
    
    . $sqlCmdApp -s "`t" -w 8000 -W -S $dbServer -d master -i $sqlScript -t 14400

    if($sqlScript -like "*call*job*"){
        Write-Host -ForegroundColor Yellow "Blind wait for 10 seconds when DB-job is called..."
        Start-Sleep -s 10
    }
}

"[[Refresh Script Finished]]"
Get-Date

Stop-Transcript
