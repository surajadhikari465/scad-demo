param (
        [string]$AccountToEnable
    )
Write-Verbose "Enable Account = $AccountToEnable" -verbose

import-module ActiveDirectory
if($AccountToEnable.Contains("\")){
	$AccountToEnable = $AccountToEnable.Split("\")[1]
}
$userCN = "not found"

try{
	$user = Get-ADUser -Identity $AccountToEnable
	$userCN = $user.DistinguishedName
}catch{
	Write-Host "User account not found"
}
Write-Host "User account found : $userCN"
if($userCN -ne "not found"){
	Enable-ADAccount -Identity $userCN
}
