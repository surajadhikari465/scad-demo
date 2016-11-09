function boolFormatter(value) {
    var formattedBool;

    if (value === true) {
        formattedBool = "Y";
    } else if (value === false) {
        formattedBool = "N";
    } else {
        formattedBool = '';
    }

    return formattedBool;
}