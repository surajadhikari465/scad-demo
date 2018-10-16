$gitRootFolder = "c:\tlux\dev\git\IRMA"

$relTmpl = "$gitRootFolder\DevOps\Release Documentation\IRMA Apps Release Template.htm"
if(!(Test-Path $relTmpl)){
    Write-Host -ForegroundColor Yellow "**ERROR** -- Doc template not found: [$relTmpl]"
    Read-Host "Press <ENTER> to exit..."
    return
}
$relDocTxt = Get-Content $relTmpl

$relDocValuesFile = "$gitRootFolder\DevOps\Release Documentation\ReleaseDoc.in.tsv"
if(!(Test-Path $relDocValuesFile)){
    Write-Host -ForegroundColor Yellow "**ERROR** -- Doc template not found: [$relDocValuesFile]"
    Read-Host "Press <ENTER> to exit..."
    return
}
$relDocValuesTxt = Get-Content $relDocValuesFile
$relDocValues = $relDocValuesTxt.split("`n") # Each line in rel-doc input file.

##################### TODO: GET THIS FROM INPUT FILE
$targetEnv = "QA"
$targetEnvShort = "QA"
$relDoc = "$gitRootFolder\DevOps\Release Documentation\IRMA Apps Release 2018-11.$targetEnvShort.htm"

$relDocHash = @{}
foreach($valueDef in $relDocValues){
    $values = $valueDef.split("`t") # Each field in a line.
    $varName = $values[0]
    $varDesc = $values[1]
    $value = $values[2]
    $relDocHash.add($varName, $value)
    $relDocTxt = $relDocTxt.Replace(("__" + $varName + "__"), $value)
    <#
    $relDocTxt = $relDocTxt.Replace("__IrmaVer__", "10.6")
    $relDocTxt = $relDocTxt.Replace("__MammothVer__", "3.7")
    $relDocTxt = $relDocTxt.Replace("__IconVer__", "9.8")
    $relDocTxt = $relDocTxt.Replace("__TibcoVer__", "1.9")
    $relDocTxt = $relDocTxt.Replace("__IconReleaseBranchName__", "RELEASE_Icon98_Mammoth37_Sprints127-128")
    $relDocTxt = $relDocTxt.Replace("__IconBuildNum__", "??")
    $relDocTxt = $relDocTxt.Replace("__IconBuildId__", "??")
    $relDocTxt = $relDocTxt.Replace("__IrmaReleaseBranchName__", "RELEASE_IRMA106_Sprints127-128")
    $relDocTxt = $relDocTxt.Replace("__IrmaBuildNum__", "??")
    $relDocTxt = $relDocTxt.Replace("__IrmaBuildId__", "??")
    #>
}



#function GetHtmlTemplate(
$tibcoPropsHtmTmpl =
"
<h2>__TargetEnv__</h2>

<h2>List __TargetEnv__: Deployable Artifact Links</h2>

<ol style='font-family:Calibri'>
__DeployPropFileList__
</ol>
"

function BuildHtmlListItems(
    [string] $items
){
    $itemList = $items.Split("`n")
    $htm = ""
    foreach($item in $itemList){
        $htm += "<li>" + $item + "`n"
    }
    return $htm
}

$appListTxt = $relDocHash.TibcoAppsUpdated
if(-not $appListTxt.tolower().Contains("none")){
    $propsListTxt = ""
    $appList = $appListTxt.Split("`n")
    foreach($app in $appList){
        $app = $app.trim()
        if($app -like "IrmaPriceListener"){
            $propsListTxt += "http://irmaqaapp1/tibco/__TargetEnv__/" + $app + "_FL___TargetEnv__.properties`n"
            $propsListTxt += "http://irmaqaapp1/tibco/__TargetEnv__/" + $app + "_MA___TargetEnv__.properties`n"
            $propsListTxt += "http://irmaqaapp1/tibco/__TargetEnv__/" + $app + "_MW___TargetEnv__.properties`n"
            $propsListTxt += "http://irmaqaapp1/tibco/__TargetEnv__/" + $app + "_NA___TargetEnv__.properties`n"
            $propsListTxt += "http://irmaqaapp1/tibco/__TargetEnv__/" + $app + "_NC___TargetEnv__.properties`n"
            $propsListTxt += "http://irmaqaapp1/tibco/__TargetEnv__/" + $app + "_NE___TargetEnv__.properties`n"
            $propsListTxt += "http://irmaqaapp1/tibco/__TargetEnv__/" + $app + "_PN___TargetEnv__.properties`n"
            $propsListTxt += "http://irmaqaapp1/tibco/__TargetEnv__/" + $app + "_RM___TargetEnv__.properties`n"
            $propsListTxt += "http://irmaqaapp1/tibco/__TargetEnv__/" + $app + "_SP___TargetEnv__.properties`n"
            $propsListTxt += "http://irmaqaapp1/tibco/__TargetEnv__/" + $app + "_SO___TargetEnv__.properties`n"
            $propsListTxt += "http://irmaqaapp1/tibco/__TargetEnv__/" + $app + "_SW___TargetEnv__.properties`n"
        } else {
            $propsListTxt += "http://irmaqaapp1/tibco/__TargetEnv__/" + $app + "___TargetEnv__.properties`n"
        }
    }
    $propsListTxt = $propsListTxt.trim()

    $propsHtm = BuildHtmlListItems $propsListTxt # Remove any extra line so we dont create empty HTML list-item tags.
    $propFilesHtm = ""
    if($targetEnv -like "QA"){
        $tibcoEnv = "QAF"
        $propFilesHtm = $tibcoPropsHtmTmpl.Replace("__DeployPropFileList__", $propsHtm).Replace("__TargetEnv__", $tibcoEnv) # Apply properties-files list to tibco-props template, then replace env refs.
        $tibcoEnv = "QAP"
    } else {
        $tibcoEnv = $targetEnvShort
    }
    $propFilesHtm += "`n`n" + $tibcoPropsHtmTmpl.Replace("__DeployPropFileList__", $propsHtm).Replace("__TargetEnv__", $tibcoEnv)
} else {
    $propFilesHtm = "N/A"
}

    $relDocTxt = $relDocTxt.Replace("__PropFilesListSection__", $propFilesHtm)

##################################################################################################################
##################################################################################################################

Set-Content $relDoc -Value $relDocTxt

Copy-Item $relDoc -Destination "\\irmaqaapp1\e$\inetpub\wwwroot\reldoc"
