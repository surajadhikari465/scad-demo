$gitRootFolder = "c:\tlux\dev\git\IRMA"

########################################################################################################################################

$relDocInValuesFile = "$gitRootFolder\DevOps\Release Documentation\ReleaseDoc.in.tsv"
if(!(Test-Path $relDocInValuesFile)){
    Write-Host -ForegroundColor Magenta "**ERROR** -- Doc template not found: [$relDocInValuesFile]"
    Read-Host "Press <ENTER> to exit..."
    return
}
$relDocInValuesTxt = Get-Content -Encoding String $relDocInValuesFile
$relDocInValues = $relDocInValuesTxt.split("`n") # Each line in rel-doc input file.

########################################################################################################################################
########################################################################################################################################
########################################################################################################################################

$relTmpl = "$gitRootFolder\DevOps\Release Documentation\IRMA Apps Release Template.htm"
if(!(Test-Path $relTmpl)){
    Write-Host -ForegroundColor Magenta "**ERROR** -- Doc template not found: [$relTmpl]"
    Read-Host "Press <ENTER> to exit..."
    return
}
$relDocTxt = Get-Content -Encoding String $relTmpl

# Check MS-Word breaking up placeholder strings.
$mswordSpellTag = "<span class=SpellE>"
if($relDocTxt.Contains("__$mswordSpellTag")){
    Write-Host -ForegroundColor Magenta "**ERROR** -- Doc template contains MS-Word spelling formatting, which corrupts placeholders: [$mswordSpellTag].  Edit & save in Word, then retry."
    Read-Host "Press <ENTER> to exit..."
    return
}

# Replace var refs inside input data.  We could add a loop here that could run this replacement a few times to allow more complex/iterative replacement.
foreach($valueDef in $relDocInValues){
    $values = $valueDef.split("`t") # Each field in a line.
    $varName = $values[0]
    $varDesc = $values[1]
    $value = $values[2]
    $relDocInValuesTxt = $relDocInValuesTxt.Replace(("__" + $varName + "__"), $value)
}

# Reset input-values array to updated value that were just replaced.
$relDocInValues = $relDocInValuesTxt.split("`n") # Each line in rel-doc input file.

# Build input-values hash so we can use values inside this script (for output filename, etc.).
$relValuesInHash = @{}
foreach($valueDef in $relDocInValues){
    $values = $valueDef.split("`t") # Each field in a line.
    $varName = $values[0]
    $varDesc = $values[1]
    $value = $values[2]
    $relValuesInHash.add($varName, $value)
}

$targetEnv = $relValuesInHash.TargetEnv
$targetEnvShort = $relValuesInHash.TargetEnvShort
$relYear = $relValuesInHash.ReleaseYear
$relId = $relValuesInHash.ReleaseId

# Create local release-doc folder, if needed.
$relDocFolder = "c:\temp\reldoc\"
if(-not (Test-Path $relDocFolder)){
    New-Item -Verbose -ItemType Directory $relDocFolder
}
$relDoc = "$relDocFolder\IRMA Apps Release $relYear-$relId.$targetEnvShort.htm"

# Replace values in HTML file with input values.
foreach($keyName in $relValuesInHash.keys){
    $relDocTxt = $relDocTxt.Replace(("__" + $keyName + "__"), $relValuesInHash.Item($keyName))
}

########################################################################################################################################

#function GetHtmlTemplate(
# Hard-coded here, but we could get from input-variables file or other source.
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

$appListTxt = $relValuesInHash.TibcoAppsUpdated
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

    $propsHtm = BuildHtmlListItems $propsListTxt # Remove any extra lines so we dont create empty HTML list-item tags.
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
$relDocFileObj = Get-ChildItem $relDoc
$docDest = "\\irmaqaapp1\e$\inetpub\wwwroot\reldoc"
Copy-Item $relDoc -Destination $docDest

"URL --> " + "http://irmaqaapp1/reldoc/" + $relDocFileObj.name
