########################################################################################################################################
########################################################################################################################################
# Functions
########################################################################################################################################
########################################################################################################################################

function AskYesNo() {
    param([string] $MainPrompt)
    $prompt = "$MainPrompt --> Yes or No? (y/n)"
    $response = Read-Host $prompt

    While ((-not ($response -like 'n')) -and (-not ($response -like 'y')))
    {
        Write-Host "Invalid input.  Please enter y or n."
        $response = Read-Host $prompt
    }

    return ($response -like 'y')
}

########################################################################################################################################

function BuildHtmlListItems(
    [string] $List,
    [string] $Delim
){
    $itemList = $List.Split($Delim)
    $htm = ""
    foreach($item in $itemList){
        $htm += "<li>" + $item + "`n"
    }
    return $htm
}

########################################################################################################################################

function StampAndArchive(){

Param($rel_doc)

    if(test-path $rel_doc){
    
    $rel_docObj = Get-Item $rel_doc
    
    [System.IO.FileInfo]$ext = Get-ChildItem ($rel_docObj) | select -ExpandProperty Extension
    [DateTime]$modifiedDate = Get-ChildItem ($rel_docObj) | select -ExpandProperty lastwritetime

    Rename-Item ($rel_docObj) ($rel_docObj.Basename + "." + (get-date $modifiedDate -f "yyyyMMdd.HHmmss") + $ext)
    Write-Host ("Earlier Release Doc version archived: " + $rel_docObj.Basename + "." + (get-date $modifiedDate -f "yyyyMMdd.HHmmss") + $ext) -ForegroundColor Green

    }
    else{  
    #do nothing    
    }

}

########################################################################################################################################
########################################################################################################################################
########################################################################################################################################
########################################################################################################################################
########################################################################################################################################


$gitRootFolder = "c:\tlux\dev\git\scad"

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

-split "1 2 3" | foreach {
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
}

# Build input-values hash so we can use values inside this script (for output filename, etc.).
$relValuesInHash = @{}
foreach($valueDef in $relDocInValues){
    $values = $valueDef.split("`t") # Each field in a line.
    $varName = $values[0]
    $varDesc = $values[1]
    $value = $values[2]
    #DEBUG -->
    #"[RelValuesInHash] Adding to hash: [$varName]=[$value]"
    $relValuesInHash.add($varName, $value)
}

$targetEnv = $relValuesInHash.TargetEnv
$targetEnvShort = $relValuesInHash.TargetEnvShort
$relId = $relValuesInHash.ReleaseId

# Create local release-doc folder, if needed.
$relDocFolder = "c:\temp\reldoc\"
if(-not (Test-Path $relDocFolder)){
    New-Item -Verbose -ItemType Directory $relDocFolder
}
$relDoc = ($relDocFolder + "IRMA Apps Release $relId.$targetEnvShort.htm")

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

$tibcoAppsUpdated = $relValuesInHash.TibcoAppsUpdated
$tibcoAppsUpdatedOL = "<ol style='font-family:Calibri'>" + (BuildHtmlListItems -List $tibcoAppsUpdated -Delim ",") + "</ol>"


if(-not $tibcoAppsUpdated.tolower().Contains("none")){
    $propsListTxt = ""
    $appList = $tibcoAppsUpdated.Split(",")
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
        } elseif (
            # ** HoneyCrisp Apps!
                $app -like "PublishInventorySpoilageService" `
            -or $app -like "PublishPurchaseOrderService" `
            -or $app -like "PublishReceivedOrderService" `
            -or $app -like "PublishTransferOrderService" `
            -or $app -like "RePublishInventoryMessagesService" `
            ) {
            if($targetEnv -like "QA"){
                # Generate only regions specified for HoneyCrisp TIBCO.
                $hcTibcoRegionsList = ($relValuesInHash.HcTibcoRegions).Split(",")
                foreach($region in $hcTibcoRegionsList){
                    $propsListTxt += "http://irmaqaapp1/tibco/__TargetEnv__/" + $app + "_" + $region + "___TargetEnv__.properties`n"
                }
            }
        }
        else {
            $propsListTxt += "http://irmaqaapp1/tibco/__TargetEnv__/" + $app + "___TargetEnv__.properties`n"
        }
    }
    $propsListTxt = $propsListTxt.trim()

    $propsHtm = BuildHtmlListItems -List $propsListTxt -Delim "`n" # Remove any extra lines so we dont create empty HTML list-item tags.
    $propFilesHtm = ""

    $hcGenQAFLinks = $false
    if($relValuesInHash.HcGenerateQafLinks -like "1"){ $hcGenQAFLinks = $true }
    if($targetEnv -like "QA"){
        $tibcoEnv = "QAF"
        # Only generate QAF links if we've deployed HC apps there, based on input setting.
        if(-not $hcGenQAFLinks){
            $propFilesHtmWithoutHC = ""
            $propsList = $propsHtm.Split("`n")
            foreach($propLine in $propsList){
                if($propLine.contains("PublishInventorySpoilageService") `
                   -or $propLine.contains("PublishPurchaseOrderService") `
                   -or $propLine.contains("PublishReceivedOrderService") `
                   -or $propLine.contains("PublishTransferOrderService") `
                   -or $propLine.contains("RePublishInventoryMessagesService") `
                ){
                    # We skip this app line, so we don't add to app list.
                    continue
                } else {
                    $propFilesHtmWithoutHC += $propLine + "`n"
                }
            }
            # Remove any trailing new-line chars from our rebuild of properties list.
            $propFilesHtmWithoutHC = $propFilesHtmWithoutHC.trim()
            # Now, we use the list we just generated (which will NOT have the HC apps) and we generate our tibco-properties-files HTML block.
            $propFilesHtm = $tibcoPropsHtmTmpl.Replace("__DeployPropFileList__", $propFilesHtmWithoutHC).Replace("__TargetEnv__", $tibcoEnv) # Apply properties-files list to tibco-props template, then replace env refs.
        } else {
            $propFilesHtm = $tibcoPropsHtmTmpl.Replace("__DeployPropFileList__", $propsHtm).Replace("__TargetEnv__", $tibcoEnv) # Apply properties-files list to tibco-props template, then replace env refs.
        }
        # After generating the QAF links above, we change the tibco env var to be our next QA env, which will be generated below.
        $tibcoEnv = "QAP"
    } else {
        # Not targeting QA env.
        $tibcoEnv = $targetEnvShort
    }
    $propFilesHtm += "`n`n" + $tibcoPropsHtmTmpl.Replace("__DeployPropFileList__", $propsHtm).Replace("__TargetEnv__", $tibcoEnv)
} else {
    $propFilesHtm = "N/A"
}

# Additional replacements for var values that were generated herein.
$relDocTxt = $relDocTxt.Replace("__TibcoAppsUpdatedOL__", $tibcoAppsUpdatedOL)
$relDocTxt = $relDocTxt.Replace("__PropFilesListSection__", $propFilesHtm)

##################################################################################################################
##################################################################################################################

Set-Content $relDoc -Value $relDocTxt
"Local File --> " + $relDoc

$relDocFileObj = Get-ChildItem $relDoc
$docDest = "\\irmaqaapp1\e$\inetpub\wwwroot\reldoc"
$archive = $docDest+"\"+$relDocFileObj.PSChildName

if(AskYesNo "`n-----`nCopy doc to web server: $docDest ?"){

    StampAndArchive $archive
    Copy-Item $relDoc -Destination $docDest

    "URL --> " + "http://irmaqaapp1/reldoc/" + $relDocFileObj.name
}


