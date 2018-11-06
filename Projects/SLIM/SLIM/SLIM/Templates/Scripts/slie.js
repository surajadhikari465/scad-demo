//slie.js javascript function used in SLIE
//Authored By: Anthony Marsella
//Whole Foods Market Southern Pacific Region


//-----------------------------------------------------------------------------------------------------

//determines which type of entry form to provide reg or random weight
function itemInfoAction()
	{
		if (document.index2.rw.checked == true)
		{
			if (document.index2.dept.value == "0031")
			{	
			document.index2.action = "iteminforw.asp";
			}
		}
		if (document.index2.rw.checked == true)
		{
			if (document.index2.dept.value == "0001")
			{	
			document.index2.action = "iteminforw.asp";
			}
		}
		if (document.index2.rw.checked == true)
		{
			if (document.index2.dept.value == "0028")
			{	
			document.index2.action = "iteminforw.asp";
			}
		}
		if (document.index2.rw.checked == true)
		{
			if (document.index2.dept.value == "0035")
			{	
			document.index2.action = "iteminforw.asp";
			}
		}
		if (document.index2.rw.checked == false)
			{	
			document.index2.action = "iteminfo.asp";
			}
	}
	
//determines zone based on store/dept info provided through logon
function getZone()
{
			if (iteminfo.realstore.value == "115" || iteminfo.realstore.value == "122")
			{
				if (iteminfo.dept.value == "0001" || iteminfo.dept.value == "0002" || iteminfo.dept.value == "0005" || iteminfo.dept.value == "0006" || iteminfo.dept.value == "0007" || iteminfo.dept.value == "0015" || iteminfo.dept.value == "0013" || iteminfo.dept.value == "0016" || iteminfo.dept.value == "0017" || iteminfo.dept.value == "0023" || iteminfo.dept.value == "0024" || iteminfo.dept.value == "0025" || iteminfo.dept.value == "0028" || iteminfo.dept.value == "0031" || iteminfo.dept.value == "0035")	
			
			{
		document.iteminfo.pzone.value = "2";
			}	
		}

	
			if (iteminfo.realstore.value == "112" || iteminfo.realstore.value == "113" || iteminfo.realstore.value == "109")
			{
				if (iteminfo.dept.value == "0007" || iteminfo.dept.value == "0017" )	
			
			{
		document.iteminfo.pzone.value = "3";
			}
			}
					
			if (iteminfo.realstore.value == "125")
			{
				if (iteminfo.dept.value == "0002" || iteminfo.dept.value == "0006" || iteminfo.dept.value == "0007" || iteminfo.dept.value == "0013" || iteminfo.dept.value == "0015" || iteminfo.dept.value == "0015" || iteminfo.dept.value == "0016" || iteminfo.dept.value == "0017" || iteminfo.dept.value == "0019" || iteminfo.dept.value == "0020" || iteminfo.dept.value == "0023" || iteminfo.dept.value == "0024" || iteminfo.dept.value == "0025")	
			
			{
		document.iteminfo.pzone.value = "5";
			}
			}	

}


//determines agecode based on dept info from logon
function getAgeCode()
{
			if (iteminfo.dept.value == "0015" || iteminfo.dept.value == "0025")
	
			{
		document.iteminfo.agecode.value = "2";
			}	


}

//determines tax, foodstamp flag base values based on store/dept info provided through logon

function getTaxFoodstampInfo()
{
		if (iteminfo.dept.value == "0065" ||iteminfo.dept.value == "0001" || iteminfo.dept.value == "0002" || iteminfo.dept.value == "0003" || iteminfo.dept.value == "0004" || iteminfo.dept.value == "0006" || iteminfo.dept.value == "0017" || iteminfo.dept.value == "0013" || iteminfo.dept.value == "0019" || iteminfo.dept.value == "0020" || iteminfo.dept.value == "0021" || iteminfo.dept.value == "0023" || iteminfo.dept.value == "0024" || iteminfo.dept.value == "0028" || iteminfo.dept.value == "0030" || iteminfo.dept.value == "0031" || iteminfo.dept.value == "0035" || iteminfo.dept.value == "0043" || iteminfo.dept.value == "0037")	
			{
		document.iteminfo.foodstamp.value = "Y";
			}
			
			if (iteminfo.dept.value == "0065" ||iteminfo.dept.value == "0001" || iteminfo.dept.value == "0002" || iteminfo.dept.value == "0003" || iteminfo.dept.value == "0004" || iteminfo.dept.value == "0006" || iteminfo.dept.value == "0017" || iteminfo.dept.value == "0011"|| iteminfo.dept.value == "0019" || iteminfo.dept.value == "0020" || iteminfo.dept.value == "0021" || iteminfo.dept.value == "0023" || iteminfo.dept.value == "0024" || iteminfo.dept.value == "0028" || iteminfo.dept.value == "0030" || iteminfo.dept.value == "0031" || iteminfo.dept.value == "0035" || iteminfo.dept.value == "0043" || iteminfo.dept.value == "0037")	
			{
		document.iteminfo.tax1.value = "N";
			}
			
			if (iteminfo.dept.value == "0064" ||iteminfo.dept.value == "0007" || iteminfo.dept.value == "0005" || iteminfo.dept.value == "0012" || iteminfo.dept.value == "0015" || iteminfo.dept.value == "0016" || iteminfo.dept.value == "0018" || iteminfo.dept.value == "0025" || iteminfo.dept.value == "0011" || iteminfo.dept.value == "0027" ||iteminfo.dept.value == "0066" || iteminfo.dept.value == "0029" || iteminfo.dept.value == "0041")	
			{
		document.iteminfo.foodstamp.value = "N";
			}
			
			if (iteminfo.dept.value == "0064" ||iteminfo.dept.value == "0007" || iteminfo.dept.value == "0005" || iteminfo.dept.value == "0012" || iteminfo.dept.value == "0015" || iteminfo.dept.value == "0016" || iteminfo.dept.value == "0018" || iteminfo.dept.value == "0025" || iteminfo.dept.value == "0013"|| iteminfo.dept.value == "0027" ||iteminfo.dept.value == "0066" || iteminfo.dept.value == "0029" || iteminfo.dept.value == "0041")	
			{
		document.iteminfo.tax1.value = "Y";
			}
			
			if (iteminfo.realstore.value == "115" || iteminfo.realstore.value == "122")
			{
				if (iteminfo.dept.value == "0013")	
			
			{
		document.iteminfo.tax1.value = "N";
			}
			}
			
			if (iteminfo.pzone.value == "5")
			{
				if (iteminfo.dept.value == "0001")	
			
			{
		document.iteminfo.tax1.value = "Y";
		document.iteminfo.sioverride.value = "Y";
		document.iteminfo.foodstamp.value = "Y";
			}
			}	
			
			if (iteminfo.pzone.value == "5")
			{
				if (iteminfo.dept.value == "0013")	
			
			{
		document.iteminfo.tax1.value = "N";
		document.iteminfo.sioverride.value = "Y";
		document.iteminfo.foodstamp.value = "Y";
			}
			}
			if (iteminfo.pzone.value == "5")
			{
				if (iteminfo.dept.value == "0064")	
			
			{
		document.iteminfo.tax1.value = "N";
		document.iteminfo.sioverride.value = "Y";
		document.iteminfo.foodstamp.value = "Y";
			}
			}
			if (iteminfo.pzone.value == "5")
			{
					if (iteminfo.dept.value == "0019")	
			
			{
		document.iteminfo.tax1.value = "Y";
		document.iteminfo.sioverride.value = "Y";
		document.iteminfo.foodstamp.value = "N";
			}
			}	
			
							
		}

//freight + casecost
function addFreight()
{
var freight = iteminfo.freight.value;
var casecost = iteminfo.casecost.value;
var newcost
newcost = ((1*casecost) + (1*freight));
{
iteminfo.casecost.value = newcost;
}
}


//-----------------------------------------------------------------------------------------------------

//margin calculator
function calcwindow()
{
window.open("http://sprco-web/slie/margincalc.asp","my_new_window","toolbar=no, location=no, directories=no, status=no, menubar=no, scrollbars=no, resizable=no, copyhistory=no, width=400, height=400")
}


function  marginCalc()
{
var retail = margincalc.retail.value;
var casecost = margincalc.casecost.value;
var casesize = margincalc.casesize.value;
var unitcost = (casecost/casesize);
margin = (((retail - unitcost)/retail)*100.00);
margin.toFixed(2);
{
margincalc.margin.value = margin;
}
}

function  retailCalc()
{
var margin = margincalc.margin.value;
var casecost = margincalc.casecost.value;
var casesize = margincalc.casesize.value;
var unitcost = (casecost/casesize);
retail = (unitcost/(100-margin))*100;
retail.toFixed(2);
{
margincalc.retail.value = retail;
}
}




//sets focus on appropriate field for item info pages
function focus()
{
	iteminfo.upcno.focus();
}

function focus1()
{
	iteminfo.miscalphaSelect.focus();
}



//verifies that user is requesting a posplu



function posplucheck()
{
	if (iteminfo.upcno.value == "0000000000000")
				{
				
		if (iteminfo.posplu.checked == false)
				{		
					alert('You have entered all zeros as the UPC indicating a POS PLU request. If that is correct please check the POS PLU box. If not then please enter a UPC.');
					iteminfo.upcno.focus();
					return (false);
					
				}
			}
}


//verifies all required fields are complteted
function check(iteminfo)
	{
			if (iteminfo.upcno.value == "")
			
			{	
						
					alert('Please enter a UPC');
					iteminfo.upcno.focus();
					return (false);
			}
			
			
			

			if (iteminfo.longdesc.value == "")
			
			{	
						
					alert('Please enter a Long Description');
					iteminfo.longdesc.focus();
					return (false);
			}
			
				if (iteminfo.posdesc.value == "")
			
			{	
						
					alert('Please enter a POS Description');
					iteminfo.posdesc.focus();
					return (false);
			}
			
				if (iteminfo.itemsize.value == "")
			
			{	
						
					alert('Please enter an Item Size');
					iteminfo.itemsize.focus();
					return (false);
			}	
			
				if (iteminfo.itemuom.value == "")
			
			{	
						
					alert('Please enter a UOM');
					iteminfo.itemuom.focus();
					return (false);
			}
			
			if (iteminfo.retail.value == "")
			
			{	
						
					alert('Please enter a Retail');
					iteminfo.retail.focus();
					return (false);
			}
			if (iteminfo.casesize.value == "")
			
			{	
						
					alert('Please enter a Case Size');
					iteminfo.casesize.focus();
					return (false);
			}
			if (iteminfo.casecost.value == "")
			
			{	
						
					alert('Please enter a Case Cost');
					iteminfo.casecost.focus();
					return (false);
			}
			
			if (iteminfo.warehouse.value == "")
			
			{	
						
					alert('Please enter a Warehouse#');
					iteminfo.warehouse.focus();
					return (false);
			}
			
					if (iteminfo.insurance.checked == false)
	
			{	
					alert('It must be verified that his item carries Liability Insurance.  Do not check this box without that verification. If you cannot verify insurance then please exit the program and return when you have verification.');
					iteminfo.insurance.focus();
					return (false);
			}
			
				if (iteminfo.policyno.value == "")
			
			{	
						
					alert('Please enter the Policy Number');
					iteminfo.policyno.focus();
					return (false);
			}
			
			if (iteminfo.qualstand.checked == false)
	
			{	
					alert('It must be verified that his item meet all Whole Foods Market quality standards.  Do not check this box without verifying all ingredients for this item are acceptable.');
					iteminfo.qualstand.focus();
					return (false);
			}
			
					if (iteminfo.scaledesc.value == "")
			
			{	
						
					alert('Please enter a Scale Description');
					iteminfo.scaledesc.focus();
					return (false);
			}
			
					if (iteminfo.ingredients.value == "")
			
			{	
						
					alert('Please enter Scale Ingredients');
					iteminfo.ingredients.focus();
					return (false);
			}

		}
		
	
//formats upc		
function pad(string) {
var UPC_LEN = 13;
var tmpString = string;
var len = tmpString.length;
if ( len >= UPC_LEN )
 return new String (tmpString); // unmodified text
else {
 var iter = UPC_LEN-1-len;
 var zeroes = "";
 for (i = 0; i < iter; i++)
  zeroes += "0";  
 return new String(zeroes + tmpString + "0");  // trailing zero append
 }
}

function renderMultiSkus(inputStr){
var tmpString = inputStr;
var tmpArray = tmpString.split(",");
var len = tmpArray.length;
var build = "";
for (j = 0; j < len; j++)
  build += pad(tmpArray[j]) + ",";
// return without trailing comma.
return new String ( build.substring(0,build.length-1) );
}
//verifies correct field length for Long Description
function descLen(string){
var descLen 
var tmpString = string;
var descLen = tmpString.length;
if ( descLen > 30 )
{	
	alert('Description may only be 30 characters in length.');
	iteminfo.longdesc.focus();
	return (false);
}
}
//verifies correct field length for scale description

function scaleDescLen(string){
var scaledescLen 
var tmpString = string;
var scaledescLen = tmpString.length;
if ( scaledescLen > 32 )
{	
	alert('Scale Description may only be 32 characters in length.');
	iteminfo.scaledesc.focus();
	return (false);
}
}
//verifies correct field length for scale ingredients
function ingLen(string){
var ingredLen 
var tmpString = string;
var ingredLen = tmpString.length;
if ( ingredLen > 972 )
{	
	alert('Ingredients may only be 972 characters in length.');
	iteminfo.ingredients.focus();
	return (false);
}
}
//verifies correct field length for upcno
function upcLen(string){
var upcLen 
var tmpString = string;
var upcLen = tmpString.length;
if ( upcLen > 13 )
{	
	alert('UPC may only be 13 characters in length.');
	iteminfo.upcno.focus();
	return (false);
}
}


//sets longdesc to upper case
function bigDesc(string) {

var tmpString = string;
var desc = tmpString;
{

document.iteminfo.longdesc.value = desc.toUpperCase();
}
}
//checks for illegal characters in description
function descCharChk(string) {
var tmpString = string;
var pos=tmpString.indexOf("@")
var pos1=tmpString.indexOf(",")
var pos2=tmpString.indexOf(":")
var pos3=tmpString.indexOf(";")
var pos4=tmpString.indexOf("/")
var pos5=tmpString.indexOf("&")
var pos6=tmpString.indexOf("-")
if (pos>0)
{ 
	alert('Descriptions may not include the @ symbol.');
	iteminfo.longdesc.focus();
	return (false);
}
if (pos1>0)
{ 
	alert('Descriptions may not include commas.');
	iteminfo.longdesc.focus();
	return (false);
}
if (pos2>0)
{ 
	alert('Descriptions may not include colons.');
	iteminfo.longdesc.focus();
	return (false);
}
if (pos3>0)
{ 
	alert('Descriptions may not include semi-colons.');
	iteminfo.longdesc.focus();
	return (false);
}
if (pos4>0)
{ 
	alert('Descriptions may not include forward slashes.');
	iteminfo.longdesc.focus();
	return (false);
}
if (pos5>0)
{ 
	alert('Descriptions may not include ampersands.');
	iteminfo.longdesc.focus();
	return (false);
}
if (pos6>0)
{ 
	alert('Descriptions may not include dashes.');
	iteminfo.longdesc.focus();
	return (false);
}
}


//checks for illegal characters in warehouse/order #
function whouseCharChk(string) {
var tmpString = string;
var pos=tmpString.indexOf("@")
var pos1=tmpString.indexOf(",")
var pos2=tmpString.indexOf(":")
var pos3=tmpString.indexOf(";")
var pos4=tmpString.indexOf("/")
var pos5=tmpString.indexOf("&")
var pos6=tmpString.indexOf("-")
if (pos>0)
{ 
	alert('Warehouse # may not include the @ symbol.');
	iteminfo.warehouse.focus();
	return (false);
}
if (pos1>0)
{ 
	alert('Warehouse # may not include commas.');
	iteminfo.warehouse.focus();
	return (false);
}
if (pos2>0)
{ 
	alert('Warehouse # may not include colons.');
	iteminfo.warehouse.focus();
	return (false);
}
if (pos3>0)
{ 
	alert('Warehouse # may not include semi-colons.');
	iteminfo.warehouse.focus();
	return (false);
}
if (pos4>0)
{ 
	alert('Warehouse # may not include forward slashes.');
	iteminfo.warehouse.focus();
	return (false);
}
if (pos5>0)
{ 
	alert('Warehouse # may not include ampersands.');
	iteminfo.warehouse.focus();
	return (false);
}
if (pos6>0)
{ 
	alert('Warehouse # may not include dashes.');
	iteminfo.warehouse.focus();
	return (false);
}
}
//verifies correct field length for pos description
function posdescLen(string){
var posdescLen 
var tmpString = string;
var posdescLen = tmpString.length;
if ( posdescLen > 18 )
{	
	alert('Description may only be 18 characters in length.');
	iteminfo.posdesc.focus();
	return (false);
}
}
//sets pos description to upper case
function bigPOSDesc(string) {

var tmpString = string;
var posdesc = tmpString;
{

document.iteminfo.posdesc.value = posdesc.toUpperCase();
}
}
//verifies correct field length for brand
function brandLen(string){
var brandLen 
var tmpString = string;
var brandLen = tmpString.length;
if ( brandLen > 10 )
{	
	alert('Manufacturer may only be 10 characters in length.');
	iteminfo.miscalphaText.focus();
	return (false);
}
}
//sets brand to upper case
function bigBRAND(string) {

var tmpString = string;
var brand = tmpString;
{

document.iteminfo.miscalphaText.value = brand.toUpperCase();
}
}
//verifies correct field length and formats warehouse number
function warehouseLen(string) 
{
var tmpString = string;
var len = tmpString.length;

if ( len == 1 ) 
{
document.iteminfo.warehouse.value = "00000000000" + tmpString;
}

if ( len == 2 )
{
document.iteminfo.warehouse.value = "0000000000" + tmpString;
}

if ( len == 3)
{
document.iteminfo.warehouse.value = "000000000" + tmpString;
}

if ( len == 4 )
{
document.iteminfo.warehouse.value = "00000000" + tmpString;
}
if ( len == 5 )
{
document.iteminfo.warehouse.value = "0000000" + tmpString;
}
if ( len == 6 )
{
document.iteminfo.warehouse.value = "000000" + tmpString;
}
if ( len == 7 )
{
document.iteminfo.warehouse.value = "00000" + tmpString;
}
if ( len == 8 )
{
document.iteminfo.warehouse.value = "0000" + tmpString;
}

if ( len == 9 )
{
document.iteminfo.warehouse.value = "000" + tmpString;
}

if ( len == 10 )
{
document.iteminfo.warehouse.value = "00" + tmpString;
}

if ( len == 11 )
{
document.iteminfo.warehouse.value = "0" + tmpString;
}

if ( len == 12 )
{
document.iteminfo.warehouse.value = tmpString;
}
if ( len > 12 )
{
	alert('Warehouse# may only have 12 characters.');
	iteminfo.warehouse.focus();
	return (false);
}
}

//sets warehouse number to upper case
function BIGwarehouse(string)
{
var tmpString = string;
var warehouse = tmpString;
{

document.iteminfo.warehouse.value = warehouse.toUpperCase();
}
}
//sets policay no to upper case
function BIGpolicyno(string)
{
var tmpString = string;
var policyno = tmpString;
{

document.iteminfo.policyno.value = policyno.toUpperCase();
}
}
//verifies correct field length for policy number
function policynoLen(string){
var policynoLen 
var tmpString = string;
var policynoLen = tmpString.length;
if ( policynoLen > 16 )
{	
	alert('Policy Number may only be 16 characters in length.');
	iteminfo.policyno.focus();
	return (false);
}
}



//determines replink value based on link code chosen
function getLinkInfo()
{
	if (iteminfo.linkcode.value == "110")
	
			{
		document.iteminfo.linkdept.value = "0084";
		document.iteminfo.replink.value = "Y";
			}
		if 	(iteminfo.linkcode.value == "103" || iteminfo.linkcode.value == "104" || iteminfo.linkcode.value == "105" || iteminfo.linkcode.value == "106" || iteminfo.linkcode.value == "107" || iteminfo.linkcode.value == "108" || iteminfo.linkcode.value == "109" || iteminfo.linkcode.value == "136" || iteminfo.linkcode.value == "137" || iteminfo.linkcode.value == "138" || iteminfo.linkcode.value == "139" || iteminfo.linkcode.value == "140" || iteminfo.linkcode.value == "152" || iteminfo.linkcode.value == "153" || iteminfo.linkcode.value == "154" || iteminfo.linkcode.value == "155" || iteminfo.linkcode.value == "156" || iteminfo.linkcode.value == "157" || iteminfo.linkcode.value == "162" || iteminfo.linkcode.value == "163" || iteminfo.linkcode.value == "241" || iteminfo.linkcode.value == "268" || iteminfo.linkcode.value == "269" || iteminfo.linkcode.value == "276" || iteminfo.linkcode.value == "281" || iteminfo.linkcode.value == "805" || iteminfo.linkcode.value == "806" || iteminfo.linkcode.value == "807" || iteminfo.linkcode.value == "809" || iteminfo.linkcode.value == "814" || iteminfo.linkcode.value == "817" || iteminfo.linkcode.value == "818" || iteminfo.linkcode.value == "819" || iteminfo.linkcode.value == "820" || iteminfo.linkcode.value == "821" || iteminfo.linkcode.value == "822" || iteminfo.linkcode.value == "823" || iteminfo.linkcode.value == "825" || iteminfo.linkcode.value == "842" || iteminfo.linkcode.value == "859" || iteminfo.linkcode.value == "862" || iteminfo.linkcode.value == "870" || iteminfo.linkcode.value == "873" || iteminfo.linkcode.value == "879")
			{
		document.iteminfo.linkdept.value = "0010";
		document.iteminfo.replink.value = "Y";
			}	
		

}


//get brand value from textbox pr selectbox
function getMiscalpha1()
{

	if (iteminfo.miscalphaText.value == "")
		{
		iteminfo.miscalpha1.value = iteminfo.miscalphaSelect.value;
		} 
		else
		{
		iteminfo.miscalpha1.value = iteminfo.miscalphaText.value;
		}
}


//opens windows for rules popups
function openwindow()
{
window.open("http://sprco-web/slie/descrules.asp","my_new_window","toolbar=no, location=no, directories=no, status=no, menubar=no, scrollbars=no, resizable=no, copyhistory=no, width=400, height=100")
}

function openwindow1()
{
window.open("http://sprco-web/slie/brandrules.asp","my_new_window","toolbar=no, location=no, directories=no, status=no, menubar=no, scrollbars=no, resizable=no, copyhistory=no, width=400, height=100")
}

function openwindow2()
{
window.open("http://sprco-web/slie/botdeprules.asp","my_new_window","toolbar=no, location=no, directories=no, status=no, menubar=no, scrollbars=no, resizable=no, copyhistory=no, width=400, height=100")
}

function openwindow3()
{
window.open("http://sprco-web/slie/warehouserules.asp","my_new_window","toolbar=no, location=no, directories=no, status=no, menubar=no, scrollbars=no, resizable=no, copyhistory=no, width=400, height=100")
}

function openwindow4()
{
window.open("http://sprco-web/slie/vendorrules.asp","my_new_window","toolbar=no, location=no, directories=no, status=no, menubar=no, scrollbars=no, resizable=no, copyhistory=no, width=400, height=100")
}

function openwindow5()
{
window.open("http://sprco-web/slie/insurancerules.asp","my_new_window","toolbar=no, location=no, directories=no, status=no, menubar=no, scrollbars=no, resizable=no, copyhistory=no, width=400, height=100")
}

function openwindow6()
{
window.open("http://sprco-web/slie/flagsrules.asp","my_new_window","toolbar=no, location=no, directories=no, status=no, menubar=no, scrollbars=yes, resizable=no, copyhistory=no, width=400, height=400")
}

function openwindow7()
{
window.open("http://sprco-web/slie/qualitystandards.asp","my_new_window","toolbar=no, location=no, directories=no, status=no, menubar=no, scrollbars=yes, resizable=no, copyhistory=no, width=400, height=400")
}
//verify that crv is supposed to be set
function chkCRV()
 {
			if (iteminfo.realstore.value == "122" || iteminfo.realstore.value == "115" || iteminfo.realstore.value == "125")
				{
				if (iteminfo.linkcode.value == "103" || iteminfo.linkcode.value == "104" || iteminfo.linkcode.value == "105" || iteminfo.linkcode.value == "106" || iteminfo.linkcode.value == "107" || iteminfo.linkcode.value == "108" || iteminfo.linkcode.value == "109" || iteminfo.linkcode.value == "136" || iteminfo.linkcode.value == "137" || iteminfo.linkcode.value == "138" || iteminfo.linkcode.value == "139" || iteminfo.linkcode.value == "140" || iteminfo.linkcode.value == "152" || iteminfo.linkcode.value == "153" || iteminfo.linkcode.value == "154" || iteminfo.linkcode.value == "155" || iteminfo.linkcode.value == "156" || iteminfo.linkcode.value == "157" || iteminfo.linkcode.value == "162" || iteminfo.linkcode.value == "163" || iteminfo.linkcode.value == "241" || iteminfo.linkcode.value == "268" || iteminfo.linkcode.value == "269" || iteminfo.linkcode.value == "276" || iteminfo.linkcode.value == "281" || iteminfo.linkcode.value == "805" || iteminfo.linkcode.value == "806" || iteminfo.linkcode.value == "807" || iteminfo.linkcode.value == "809" || iteminfo.linkcode.value == "814" || iteminfo.linkcode.value == "817" || iteminfo.linkcode.value == "818" || iteminfo.linkcode.value == "819" || iteminfo.linkcode.value == "820" || iteminfo.linkcode.value == "821" || iteminfo.linkcode.value == "822" || iteminfo.linkcode.value == "823" || iteminfo.linkcode.value == "825" || iteminfo.linkcode.value == "842" || iteminfo.linkcode.value == "859" || iteminfo.linkcode.value == "862" || iteminfo.linkcode.value == "870" || iteminfo.linkcode.value == "873" || iteminfo.linkcode.value == "879" )

				{
					alert('CRV is California Only please select another setting from the dropdown.');
					iteminfo.linkcode.focus(); 
					return(false);
				}
			}
	}
	
//check for alphanumerics in numeric only fields	
function IsNumeric(sText)
{
   var ValidChars = "0123456789.";
   var IsNumber=true;
   var Char;

 
   for (i = 0; i < sText.length && IsNumber == true; i++) 
      { 
      Char = sText.charAt(i); 
      if (ValidChars.indexOf(Char) == -1) 
         {
         IsNumber = false;
    
    alert('This is a number only field alpha characters are not allowed.');
	iteminfo.itemsize.focus();
	return IsNumber;
	
        }
      }
   }
   
   function IsNumericMinWeight(sText)
{
   var ValidChars = "0123456789.";
   var IsNumber=true;
   var Char;

 
   for (i = 0; i < sText.length && IsNumber == true; i++) 
      { 
      Char = sText.charAt(i); 
      if (ValidChars.indexOf(Char) == -1) 
         {
         IsNumber = false;
    
    alert('This is a number only field alpha characters are not allowed.');
	iteminfo.minwght.focus();
	return IsNumber;
	
        }
      }
   }
   
 function IsNumericUPC(sText)
{
   var ValidChars = "0123456789.";
   var IsNumber=true;
   var Char;

 
   for (i = 0; i < sText.length && IsNumber == true; i++) 
      { 
      Char = sText.charAt(i); 
      if (ValidChars.indexOf(Char) == -1) 
         {
         IsNumber = false;
    
    alert('This is a number only field alpha characters are not allowed.');
	iteminfo.upcno.focus();
	return IsNumber;
	
        }
      }
      }  
      
 
function IsNumericPM(sText)
{
   var ValidChars = "0123456789.";
   var IsNumber=true;
   var Char;

 
   for (i = 0; i < sText.length && IsNumber == true; i++) 
      { 
      Char = sText.charAt(i); 
      if (ValidChars.indexOf(Char) == -1) 
         {
         IsNumber = false;
    
    alert('This is a number only field alpha characters are not allowed.');
	iteminfo.pm.focus();
	return IsNumber;
	
        }
      }
      }
      
      function IsNumericRetail(sText)
{
   var ValidChars = "0123456789.";
   var IsNumber=true;
   var Char;

 
   for (i = 0; i < sText.length && IsNumber == true; i++) 
      { 
      Char = sText.charAt(i); 
      if (ValidChars.indexOf(Char) == -1) 
         {
         IsNumber = false;
    
    alert('This is a number only field alpha characters are not allowed.');
	iteminfo.retail.focus();
	return IsNumber;
	
        }
      }
     }
     
           function IsNumericShelfLife(sText)
{
   var ValidChars = "0123456789.";
   var IsNumber=true;
   var Char;

 
   for (i = 0; i < sText.length && IsNumber == true; i++) 
      { 
      Char = sText.charAt(i); 
      if (ValidChars.indexOf(Char) == -1) 
         {
         IsNumber = false;
    
    alert('This is a number only field alpha characters are not allowed.');
	iteminfo.shelflife.focus();
	return IsNumber;
	
        }
      }
     }
      
function IsNumericPackage(sText)
{
   var ValidChars = "0123456789.";
   var IsNumber=true;
   var Char;

 
   for (i = 0; i < sText.length && IsNumber == true; i++) 
      { 
      Char = sText.charAt(i); 
      if (ValidChars.indexOf(Char) == -1) 
         {
         IsNumber = false;
    
    alert('This is a number only field alpha characters are not allowed.');
	iteminfo.package.focus();
	return IsNumber;
	
        }
      }
      }
   
   
   function IsNumericFreight(sText)
{
   var ValidChars = "0123456789.";
   var IsNumber=true;
   var Char;

 
   for (i = 0; i < sText.length && IsNumber == true; i++) 
      { 
      Char = sText.charAt(i); 
      if (ValidChars.indexOf(Char) == -1) 
         {
         IsNumber = false;
    
    alert('This is a number only field alpha characters are not allowed.');
	iteminfo.freight.focus();
	return IsNumber;
	
        }
      }
     } 
function IsNumericCaseCost(sText)
{
   var ValidChars = "0123456789.";
   var IsNumber=true;
   var Char;

 
   for (i = 0; i < sText.length && IsNumber == true; i++) 
      { 
      Char = sText.charAt(i); 
      if (ValidChars.indexOf(Char) == -1) 
         {
         IsNumber = false;
    
    alert('This is a number only field alpha characters are not allowed.');
	iteminfo.casecost.focus();
	return IsNumber;
	
        }
      }
     }
      
 function IsNumericCaseSize(sText)
{
   var ValidChars = "0123456789.";
   var IsNumber=true;
   var Char;

 
   for (i = 0; i < sText.length && IsNumber == true; i++) 
      { 
      Char = sText.charAt(i); 
      if (ValidChars.indexOf(Char) == -1) 
         {
         IsNumber = false;
    
    alert('This is a number only field alpha characters are not allowed.');
	iteminfo.casesize.focus();
	return IsNumber;
	
        }
      }
}
//sets bycount info based on dropdown
function getByCount()
	{
		if (iteminfo.scaleuom.value == "BC")
		{
			iteminfo.bycount.value = "1";
		}
		
		if (iteminfo.scaleuom.value == "FW")
		{
			iteminfo.bycount.value = "0";
		}
		if (iteminfo.scaleuom.value == "LB")
		{
			iteminfo.bycount.value = "0";
		}
	}


	
//formats plu field	
function padPLU(string) 
{
var tmpString = string;
var len = tmpString.length;

if ( len == 1 ) 
{
document.rwitems.plu.value = "0000" + tmpString;
}

if ( len == 2 )
{
document.rwitems.plu.value = "000" + tmpString;
}

if ( len == 3)
{
document.rwitems.plu.value = "00" + tmpString;
}

if ( len == 4 )
{
document.rwitems.plu.value = "0" + tmpString;
}

if ( len == 5 )
{
document.rwitems.plu.value = tmpString;
}
}


//formats scale upc based on plu
function makeUPC()
{
	var plu = document.rwitems.plu.value;
		{
	document.rwitems.upcno.value = "02" + plu + "000000";
		}
}

//loads current date
function loadDate()

	{
	
		var month;
		var tDate;
		var day;                                
		var wholeDate;
		var year;
		tDate = new Date();
		month = tDate.getMonth() + 1;
		day = tDate.getDate();
		year = tDate.getYear();
							
				
		wholeDate = month + "/" + day + "/" + year;
		fthing.txtDate.value = wholeDate;
	}
				
	//verifies all required fields are provided for vendor request form
	function checkit(fthing)
	{
			if (fthing.upcno.value == "")
			
			{	
						
					alert('Please enter a UPC');
					fthing.upcno.focus();
					return (false);
			}
		 	
				
		   
				if (fthing.vendor.value == "")
			
			{	
						
					alert('Please enter a Vendor Name');
					fthing.vendor.focus();
					return (false);
			}
					
				
			
				if (fthing.address1.value == "")
			
			{	
						
					alert('Please enter an address');
					fthing.address1.focus();
					return (false);
			}	
			
				if (fthing.city.value == "")
			
			{	
						
					alert('Please enter a city');
					fthing.city.focus();
					return (false);
			}
			
			if (fthing.state.value == "")
			
			{	
						
					alert('Please enter a state');
					fthing.state.focus();
					return (false);
			}
			if (fthing.zip.value == "")
			
			{	
						
					alert('Please enter a zip');
					fthing.zip.focus();
					return (false);
			}
			
						if (fthing.phone.value == "")
			
			{	
						
					alert('Please enter a phone #');
					fthing.phone.focus();
					return (false);
			}
			
						if (fthing.fax.value == "")
			
			{	
						
					alert('Please enter a fax #.');
					fthing.fax.focus();
					return (false);
			}
			
						if (fthing.netterms.value == "")
			
			{	
						
					alert('Please enter payment terms.');
					fthing.netterms.focus();
					return (false);
			}
					if (fthing.policyno.value == "")
			
			{	
						
					alert('Please enter a policy number.');
					fthing.policyno.focus();
					return (false);
			}
			
					
				
		}
//verifies length for vendor name		
function vendorLen(string){
var vendorLen 
var tmpString = string;
var vendorLen = tmpString.length;
if ( vendorLen > 30 )
{	
	alert('Vendor may only be 30 characters in length.');
	fthing.vendor.focus();
	return (false);
}
}

//sets vendor name to uppercase
function bigVendor(string) {

var tmpString = string;
var vendor = tmpString;
{

document.fthing.vendor.value = vendor.toUpperCase();
}
}
//verifies length for vendor number	
function vendornumLen(string){
var vendornumLen 
var tmpString = string;
var vendornumLen = tmpString.length;
if ( vendornumLen > 8 )
{	
	alert('VendorNum may only be 8 characters in length.');
	fthing.vendornum.focus();
	return (false);
}
}

//sets vendor number to upper case
function bigVendorNum(string) {
var tmpString = string;
var vendornum = tmpString;
{
document.fthing.vendornum.value = vendornum.toUpperCase();
}
}
//verifies length for address line 1
function address1Len(string){
var address1Len 
var tmpString = string;
var address1Len = tmpString.length;
if ( address1Len > 20 )
{	
	alert('Address Line 1  may only be 20 characters in length.');
	fthing.address1.focus();
	return (false);
}
}

//sets address line 1 to upper case
function bigAddress1(string) {
var tmpString = string;
var address1 = tmpString;
{
document.fthing.address1.value = address1.toUpperCase();
}
}
//verifies length for addressline 2
function address2Len(string){
var address2Len 
var tmpString = string;
var address2Len = tmpString.length;
if ( address2Len > 20 )
{	
	alert('Address Line 2 may only be 20 characters in length.');
	fthing.address2.focus();
	return (false);
}
}

//sets address line2 to upper case
function bigAddress2(string) {
var tmpString = string;
var address2 = tmpString;
{
document.fthing.address2.value = address2.toUpperCase();
}
}
//verifies length for for state
function stateLen(string){
var stateLen 
var tmpString = string;
var stateLen = tmpString.length;
if ( stateLen > 2 )
{	
	alert('State may only be 2 characters in length.');
	fthing.state.focus();
	return (false);
}
}

//sets state to upper case
function bigState(string) {
var tmpString = string;
var state = tmpString;
{
document.fthing.state.value = state.toUpperCase();
}
}

//verifies length for city
function cityLen(string){
var cityLen 
var tmpString = string;
var cityLen = tmpString.length;
if ( cityLen > 20 )
{	
	alert('City may only be 20 characters in length.');
	fthing.city.focus();
	return (false);
}
}

//sets city to upper case
function bigCity(string) {
var tmpString = string;
var city = tmpString;
{
document.fthing.city.value = city.toUpperCase();
}
}

//verifies length for net terms
function nettermsLen(string){
var nettermsLen 
var tmpString = string;
var nettermsLen = tmpString.length;
if ( nettermsLen > 2 )
{	
	alert('NetTerms may only be 2 characters in length.');
	fthing.netterms.focus();
	return (false);
}
}

//sets net terms to upper case
function bigNetTerms(string) {
var tmpString = string;
var netterms = tmpString;
{
document.fthing.netterms.value = netterms.toUpperCase();
}
}

function getunit()
	{
	var casecost = document.newcost.newcost.value;
	var unitcost;
	var casesize = document.values.casesize.value;
	var margin;
	var retail = document.values.holdprice.value;
	unitcost = (casecost/casesize);
	margin = (((retail-unitcost)/retail)*100.00);
	margin = margin.toFixed(2);
	unitcost = unitcost.toFixed(2);

	{
	document.newcost.newcost2.value = unitcost;
	document.values.margin.value = margin;
	}
	}
	
	function getcase()
	{
	var unitcost = document.newcost.newcost2.value;
	var casecost;
	var casesize = document.values.casesize.value;
	var margin;
	var retail = document.values.holdprice.value;
	casecost = (unitcost*casesize);
	margin = (((retail-unitcost)/retail)*100.00);
	margin = margin.toFixed(2);
	casecost = casecost.toFixed(2);

	{
	document.newcost.newcost.value = casecost;
	document.values.margin.value = margin;
	}
	}
	
	
function getmargin()
{
var retail = document.retail.newretail.value;
var unitcost = document.values.unitcost.value;
margin = (((retail-unitcost)/retail)*100.00);
margin = margin.toFixed(2);
{
	document.values.margin.value = margin;
}
}


function retailDecimal(string) 
{
var tmpString = string;
var len = tmpString.length;

if ( len == 1 ) 
{
document.retail.newretail.value = ".0" + tmpString;
}

if ( len == 2 )
{
document.retail.newretail.value = "." + tmpString;
}

if ( len == 3)
{
var str1 = tmpString.substr(0,1);
var str2 = tmpString.substr(1,2);
document.retail.newretail.value = str1 + "." + str2;
}

if ( len == 4 )
{
var str1 = tmpString.substr(0,2);
var str2 = tmpString.substr(2,4);
document.retail.newretail.value = str1 + "." + str2;
}
if ( len == 5 )
{
var str1 = tmpString.substr(0,3);
var str2 = tmpString.substr(3,5);
document.retail.newretail.value = str1 + "." + str2;
}

}