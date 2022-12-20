param (
    [string]$paramTo,
    [string]$paramEnv,
    [string]$paramDbInstanceId,
	[string]$paramStatus
    )

$env:CURRENT_TASK_ROOTDIR = Split-Path -Parent $MyInvocation.MyCommand.Path
. $env:CURRENT_TASK_ROOTDIR\RefreshNotificationEmail.ps1

(RefreshEmail -paramTo $paramTo -paramEnv $paramEnv -paramInstanceId $paramDbInstanceId -paramStatus $paramStatus)