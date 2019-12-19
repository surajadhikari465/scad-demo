export function validateNumberOfDecimals(value, fieldOptions) {
    if (value.includes('.')) {
        if (value.substring(value.indexOf('.') + 1).length === 0) {
            //Infragistics allows a value to be a digit followed by a decimal point with no
            //digits after the decimal point so adding extra validation to check that there are
            //digits after the decimal point. For Example: 1. would fall into this category
            return false;
        }
        else {
            let numberOfDecimals = $(this.element).attr('data-number-of-decimals');
            return isValidNumberOfDecimals(value, numberOfDecimals);
        }
    } else {
        return true;
    }
}

export function validateNumberOfDecimalsForGrid(value, attribute) {
    if (value === undefined || value === null) {
        return true;
    }
    else {
        let valueString = value.toString();
        if (valueString.includes('.')) {
            if (valueString.substring(valueString.indexOf('.') + 1).length === 0) {
                return false;
            }
            else {
                return isValidNumberOfDecimals(valueString, attribute.NumberOfDecimals);
            }
        }
        else {
            return true;
        }
    }
}

export function isValidNumberOfDecimals(value, maxNumberOfDecimals) {
    return value.substring(value.indexOf('.') + 1).length <= maxNumberOfDecimals;
}

export default validateNumberOfDecimals