export const isNumber = function isNumber(o:any) {
    return !isNaN(o - 0) && o.replace(/^\s\s*/, '') !== "" && o !== null  && o !== false;
}

export const checkValueLessThanOrEqualToMaxValue =  function checkMaxValue(o:number, max:number)
{
  if(o > max)
  {
      return false;
  }
  return true;

}

export const overFlow = function (input:number)
{
  if(input> 32767)
  {
    return true;
  }
   return false;
}
export const checkDuplicateInObject =  function  checkDuplicateInObject(propertyName:any, inputArray:any) {
  var seenDuplicate = false,
      testObject = {};

  inputArray.map(function(item:any) {
    var itemPropertyName = item[propertyName];    
    if (itemPropertyName in testObject) {
      testObject[itemPropertyName].duplicate = true;
      item.duplicate = true;
      seenDuplicate = true;
    }
    else {
      testObject[itemPropertyName] = item;
      delete item.duplicate;
    }
  });

  return seenDuplicate;
}
export const checkValueMoreThanOrEqualToMinValue =  function checkMinValue(o:number, min:number)
{
  if(o < min)
  {
      return false;
  }
  return true;

}