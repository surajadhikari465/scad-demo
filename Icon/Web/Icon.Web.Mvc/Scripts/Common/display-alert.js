function displayAlertMessage(message, isError) {
    var alertClass = null;
    if (isError) {
        alertClass = "alert-danger";
    } else {
        alertClass = "alert-success";
    }
    var alertMessage = '<div class="alert ' + alertClass + ' alert-dismissable page-subsection">' +
                            '<button type="button" class="close" data-dismiss="alert" aria-hidden="true">&times;</button>' +
                            '<strong class="alertMessage">' + message + '</strong>' +
                        '</div>';
    $('.search-alert').html(alertMessage);
}

function displayWarning(message) {
    var alertClass = "alert-warning";
    
    var alertMessage = '<div class="alert ' + alertClass + ' alert-dismissable page-subsection">' +
                            '<button type="button" class="close" data-dismiss="alert" aria-hidden="true">&times;</button>' +
                            '<strong class="alertMessage">' + message + '</strong>' +
                        '</div>';

    $('#alert').html(alertMessage);
}