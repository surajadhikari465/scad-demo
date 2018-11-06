


		
	
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

