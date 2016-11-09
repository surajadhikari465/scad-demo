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
$logFilePath = $scriptFullPath + ".log"


# Test: @('vm-icon-test1', 'vm-icon-test2')
# QA: @('vm-icon-qa1', 'vm-icon-qa2', 'vm-icon-qa3', 'vm-icon-qa4')
# Production: @('vm-icon-prd1', 'vm-icon-prd2', 'vm-icon-prd3', 'vm-icon-prd4', 'vm-icon-prd5', 'vm-icon-prd6')

$appServers = @('vm-icon-prd1', 'vm-icon-prd2', 'vm-icon-prd3', 'vm-icon-prd4', 'vm-icon-prd5', 'vm-icon-prd6')




# Post-deployment configuration.
###############################

# Array of services that will need to be started back up.  There's no corresponding array for scheduled tasks because their schedule is not
# uniform across the servers.
# ----------------------------
# Icon CCH Tax Listener
# Icon eWIC Error Response Listener
# Icon eWIC APL Listener
# Icon Item Movement Listener
# Icon R10 Listener
$postDeploymentServices = @('Icon R10 Listener', 'Icon CCH Tax Listener', 'Icon Item Movement Listener', 'POS Push Controller', 'Regional Controller')




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




# Post-deployment.
##################

# Restart the Windows Services.
$output = $newline + "Starting services on the following application servers:"
LogAndWrite -LogOutput $output

# Services may not run on all six production servers.
# Prod --> Item Movement Listener added Aug 2015 to 5 & 6.
# ------------------------------
# Test: @('vm-icon-test1', 'vm-icon-test2')
# QA: @('vm-icon-qa1', 'vm-icon-qa2', 'vm-icon-qa3', 'vm-icon-qa4')
# Production: @('vm-icon-prd1', 'vm-icon-prd2', 'vm-icon-prd5', 'vm-icon-prd6')
# ------------------------------

$appServersForActiveServices = @('vm-icon-prd1', 'vm-icon-prd2', 'vm-icon-prd5', 'vm-icon-prd6')

Foreach ($appServer in $appServersForActiveServices.GetEnumerator())
{
    $output = $appServer
    LogAndWrite -LogOutput $output
}

If ((UserWantsToContinue) -eq $false)
{
    Exit
}

Foreach ($appServer in $appServersForActiveServices.GetEnumerator())
{
    $output = $newline + "Starting services on server: " + $appServer
    LogAndWrite -LogOutput $output

    If ($previewMode -eq $true)
    {
        Foreach ($service in $postDeploymentServices.GetEnumerator())
        {
            StartServiceRemotely -ServerName $appServer -ServiceDisplayName $service -WhatIf
        }
    }

    if ($previewMode -eq $false)
    {
        Foreach ($service in $postDeploymentServices.GetEnumerator())
        {
            StartServiceRemotely -ServerName $appServer -ServiceDisplayName $service -Verbose:$true *>> $logFilePath
        }
    }

    $output = $newline + "Finished post-deployment for Windows Services on $appServer."
    LogAndWrite -LogOutput $output
}


# Enable the scheduled tasks.  Here is the current list of tasks to enable by server.
# ********* PHASE 1
# vm-icon-prd1: All Phase 1 API Controllers, Global Controller, Regional Controller, POS Push Controller, SubTeam Controller
# vm-icon-prd2: Phase 1 API Controllers of type [ItemLocale, Price, Product], Global Controller, Regional Controller, POS Push Controller
# vm-icon-prd3: Phase 1 API Controllers of type [ItemLocale, Price, Product], POS Push Controller
# vm-icon-prd4: n/a
# vm-icon-prd5: n/a
# vm-icon-prd6: n/a
#
# ********* PHASE 2
# vm-icon-prd1	API Controller Phase 2 - Hierarchy - Instance ID=1
# vm-icon-prd1	API Controller Phase 2 - Item Locale - Instance ID=1
# vm-icon-prd1	API Controller Phase 2 - Locale - Instance ID=1
# vm-icon-prd1	API Controller Phase 2 - Price - Instance ID=1
# vm-icon-prd1	API Controller Phase 2 - Product - Instance ID=1
# vm-icon-prd1	API Controller Phase 2 - Product Selection Group - Instance ID=1
# vm-icon-prd1	Global Controller - Instance ID=1
# vm-icon-prd1	POS Push Controller - Instance ID=1
# vm-icon-prd1	Regional Controller - Instance ID=1
# vm-icon-prd1	SubTeam Controller – Instance ID=1
# vm-icon-prd2	API Controller Phase 2 - Hierarchy - Instance ID=2
# vm-icon-prd2	API Controller Phase 2 - Item Locale - Instance ID=2
# vm-icon-prd2	API Controller Phase 2 - Price - Instance ID=2
# vm-icon-prd2	API Controller Phase 2 - Product - Instance ID=2
# vm-icon-prd2	API Controller Phase 2 - Product Selection Group - Instance ID=2
# vm-icon-prd2	Global Controller - Instance ID=2
# vm-icon-prd2	POS Push Controller - Instance ID=2
# vm-icon-prd2	Regional Controller - Instance ID=2
# vm-icon-prd3	API Controller Phase 2 - Item Locale
# vm-icon-prd3	API Controller Phase 2 - Price
# vm-icon-prd3	API Controller Phase 2 - Product
# vm-icon-prd3	API Controller Phase 2 - Product Selection Group
# vm-icon-prd3	POS Push Controller - Instance ID=3

$output = $newline + "Enable tasks on the following application servers:"
LogAndWrite -LogOutput $output

# Tasks only run on three of the six production servers (for now).

# Test: @('vm-icon-test1', 'vm-icon-test2')
# QA: @('vm-icon-qa1', 'vm-icon-qa2', 'vm-icon-qa3', 'vm-icon-qa4')
# Production: @('vm-icon-prd1', 'vm-icon-prd2', 'vm-icon-prd3')

$appServersForActiveTasks =@('vm-icon-prd1', 'vm-icon-prd2', 'vm-icon-prd3')

Foreach ($appServer in $appServersForActiveTasks.GetEnumerator())
{
    $output = $appServer
    LogAndWrite -LogOutput $output
}

If ((UserWantsToContinue) -eq $false)
{
    Exit
}

Foreach ($appServer in $appServersForActiveTasks.GetEnumerator())
{
    $output = "Starting tasks on server: " + $appServer + $newline
    LogAndWrite -LogOutput $output

    If ($previewMode -eq $true)
    {
        If ($appServer -eq 'vm-icon-prd1')
        {
            EnableScheduledTasksRemotely -ServerName $appServer -ScheduledTaskNameWildCard 'API Controller Phase 2*' -WhatIf
            EnableScheduledTasksRemotely -ServerName $appServer -ScheduledTaskNameWildCard 'Global Controller*' -WhatIf
            EnableScheduledTasksRemotely -ServerName $appServer -ScheduledTaskNameWildCard 'SubTeam Controller*' -WhatIf
        }

        If ($appServer -eq 'vm-icon-prd2')
        {
            EnableScheduledTasksRemotely -ServerName $appServer -ScheduledTaskNameWildCard 'API Controller Phase 2 - Hierarchy*' -WhatIf
            EnableScheduledTasksRemotely -ServerName $appServer -ScheduledTaskNameWildCard 'API Controller Phase 2 - Item Locale*' -WhatIf
            EnableScheduledTasksRemotely -ServerName $appServer -ScheduledTaskNameWildCard 'API Controller Phase 2 - Price*' -WhatIf
            EnableScheduledTasksRemotely -ServerName $appServer -ScheduledTaskNameWildCard 'API Controller Phase 2 - Product*' -WhatIf
            EnableScheduledTasksRemotely -ServerName $appServer -ScheduledTaskNameWildCard 'API Controller Phase 2 - Product Selection Group*' -WhatIf
            EnableScheduledTasksRemotely -ServerName $appServer -ScheduledTaskNameWildCard 'Global Controller*' -WhatIf
        }

        If ($appServer -eq 'vm-icon-prd3')
        {
            EnableScheduledTasksRemotely -ServerName $appServer -ScheduledTaskNameWildCard 'API Controller Phase 2 - Item Locale*' -WhatIf
            EnableScheduledTasksRemotely -ServerName $appServer -ScheduledTaskNameWildCard 'API Controller Phase 2 - Price*' -WhatIf
            EnableScheduledTasksRemotely -ServerName $appServer -ScheduledTaskNameWildCard 'API Controller Phase 2 - Product*' -WhatIf
            EnableScheduledTasksRemotely -ServerName $appServer -ScheduledTaskNameWildCard 'API Controller Phase 2 - Product Selection Group*' -WhatIf
        }        
    }

    if ($previewMode -eq $false)
    {
        If ($appServer -eq 'vm-icon-prd1')
        {
            EnableScheduledTasksRemotely -ServerName $appServer -ScheduledTaskNameWildCard 'API Controller Phase 2*' -Verbose:$true *>> $logFilePath
            EnableScheduledTasksRemotely -ServerName $appServer -ScheduledTaskNameWildCard 'Global Controller*' -Verbose:$true *>> $logFilePath
            EnableScheduledTasksRemotely -ServerName $appServer -ScheduledTaskNameWildCard 'SubTeam Controller*' -Verbose:$true *>> $logFilePath
        }

        If ($appServer -eq 'vm-icon-prd2')
        {
            EnableScheduledTasksRemotely -ServerName $appServer -ScheduledTaskNameWildCard 'API Controller Phase 2 - Hierarchy*' -Verbose:$true *>> $logFilePath
            EnableScheduledTasksRemotely -ServerName $appServer -ScheduledTaskNameWildCard 'API Controller Phase 2 - Item Locale*' -Verbose:$true *>> $logFilePath
            EnableScheduledTasksRemotely -ServerName $appServer -ScheduledTaskNameWildCard 'API Controller Phase 2 - Price*' -Verbose:$true *>> $logFilePath
            EnableScheduledTasksRemotely -ServerName $appServer -ScheduledTaskNameWildCard 'API Controller Phase 2 - Product*' -Verbose:$true *>> $logFilePath
            EnableScheduledTasksRemotely -ServerName $appServer -ScheduledTaskNameWildCard 'API Controller Phase 2 - Product Selection Group*' -Verbose:$true *>> $logFilePath
            EnableScheduledTasksRemotely -ServerName $appServer -ScheduledTaskNameWildCard 'Global Controller*' -Verbose:$true *>> $logFilePath
        }

        If ($appServer -eq 'vm-icon-prd3')
        {
            EnableScheduledTasksRemotely -ServerName $appServer -ScheduledTaskNameWildCard 'API Controller Phase 2 - Item Locale*' -Verbose:$true *>> $logFilePath
            EnableScheduledTasksRemotely -ServerName $appServer -ScheduledTaskNameWildCard 'API Controller Phase 2 - Price*' -Verbose:$true *>> $logFilePath
            EnableScheduledTasksRemotely -ServerName $appServer -ScheduledTaskNameWildCard 'API Controller Phase 2 - Product*' -Verbose:$true *>> $logFilePath
            EnableScheduledTasksRemotely -ServerName $appServer -ScheduledTaskNameWildCard 'API Controller Phase 2 - Product Selection Group*' -Verbose:$true *>> $logFilePath
        }        
    }

    $output = "Finished post-deployment for Scheduled Tasks on $appServer." + $newline
    LogAndWrite -LogOutput $output
}


$output = "Post-deployment script execution complete."
LogAndWrite -LogOutput $output
