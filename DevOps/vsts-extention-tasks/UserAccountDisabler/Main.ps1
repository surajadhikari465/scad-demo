param (
        [string]$AccountToDisable
    )
Write-Verbose "Disable Account = $AccountToDisable" -verbose

import-module ActiveDirectory
if($AccountToDisable.Contains("\")){
	$AccountToDisable = $AccountToDisable.Split("\")[1]
}
$userCN = "not found"

try{
	$user = Get-ADUser -Identity $AccountToDisable
	$userCN = $user.DistinguishedName
}catch{
	Write-Host "User account not found"
}
Write-Host "User account found : $userCN"
if($userCN -ne "not found"){
	Disable-ADAccount -Identity $userCN
}
