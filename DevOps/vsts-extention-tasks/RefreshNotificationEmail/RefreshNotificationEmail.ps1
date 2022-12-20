#
# RefreshNotificationEmail.ps1
#

function RefreshEmail {
	param(
		[string]$paramTo,
		[string]$paramEnv,
		[string]$paramDbInstanceId,
		[string]$paramStatus
	)

	$mailTo = @()

	$paramTo.split(',', [System.StringSplitOptions]::RemoveEmptyEntries) |`
    Foreach { if( ![string]::IsNullOrWhiteSpace($_) -and ![string]::Equals('\n', $_)) {$mailTo += $_}}

	$mailFrom = "DB Refresh " + $paramEnv + " <noreply.db.refresh." + $paramEnv + "@wholefoods.com>"
	$mailSubject = "DB Refresh"
	if($paramStatus -like "start"){
		$mailSubject += " Starting:"
	} elseif($paramStatus -like "finish"){
		$mailSubject += " Complete:"
	} elseif($paramStatus -like "fail"){
		$mailSubject += " *Failed*:"
	} else {
		$mailSubject += " **Unknown**:"
	}
	$mailSubject += " " + $paramDbInstanceId + " " + $paramEnv
	$mailBodyHtml = "<h1>See details here... **link**</h1>"
	send-mailmessage -SmtpServer smtp.wholefoods.com -to $mailTo -from $mailFrom -subject $mailSubject -BodyAsHtml $mailBodyHtml


}
