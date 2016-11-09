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




# Pre-deployment configuration.
###############################

# Array of services that will need to be stopped on each server.
$preDeploymentServices = @('Icon R10 Listener', 'Icon CCH Tax Listener', 'Icon Item Movement Listener', 'Icon eWIC APL Listener', 'Icon eWIC Error Response Listener', 'POS Push Controller', 'Regional Controller')

# Array of tasks that will need to be stopped on each server.
$preDeploymentScheduledTasks = @('API Controller*', 'Global Controller*', 'SubTeam Controller*', 'Tlog Controller*')

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


# Stop services and scheduled tasks.
$output = $newline + "Stopping services and scheduled tasks on application servers:"
LogAndWrite -LogOutput $output

Foreach ($appServer in $appServers.GetEnumerator())
{
    $output = $appServer
    LogAndWrite -LogOutput $output
}
If ((UserWantsToContinue) -eq $false)
{
    Exit
}

Foreach ($appServer in $appServers.GetEnumerator())
{
    $output = $newline + "Stopping services and scheduled tasks on server: " + $appServer
    LogAndWrite -LogOutput $output

    If ($previewMode -eq $true)
    {
        Foreach ($service in $preDeploymentServices.GetEnumerator())
        {
            StopServiceRemotely -ServerName $appServer -ServiceDisplayName $service -WhatIf
        }

        Foreach ($task in $preDeploymentScheduledTasks.GetEnumerator())
        {
            DisableScheduledTasksRemotely -ServerName $appServer -ScheduledTaskNameWildCard $task -WhatIf
        }
    }

    if ($previewMode -eq $false)
    {
        Foreach ($service in $preDeploymentServices.GetEnumerator())
        {
            StopServiceRemotely -ServerName $appServer -ServiceDisplayName $service -Verbose:$true *>> $logFilePath
        }

        Foreach ($task in $preDeploymentScheduledTasks.GetEnumerator())
        {
            DisableScheduledTasksRemotely -ServerName $appServer -ScheduledTaskNameWildCard $task -Verbose:$true *>> $logFilePath
        }
    }

    $output = $newline + "Finished pre-deployment for $appServer."
    LogAndWrite -LogOutput $output
}


$output = $newline + "Pre-deployment script execution complete."
LogAndWrite -LogOutput $output
