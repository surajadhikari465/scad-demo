function setEsbEnvironmentSuccess(result) {
    $('#message').html(
        '<div class="alert alert-success fade in">' 
            + '<a href="#" class="close" data-dismiss="alert" aria-label="close">&times;</a>'
            + '<div>' + result.message + '</div>'
        + '</div>');
    $('.set-esb-btn').removeClass('disable-link').removeClass('disabled');
    var env = result.message.slice(result.message.lastIndexOf(' ') + 1);
    $('#currentEsbEnvironmentDiv').html('<div class="fade in"><h4>' + env + '</h4></div>');
    $('#spinSpan').removeClass('spinning');
    $('#btnGetCurrentEsbEnvironment').removeClass('disabled');
}

function setEsbEnvironmentBegin() {
    $('.set-esb-btn').addClass('disable-link').addClass('disabled');
    $('#btnGetCurrentEsbEnvironment').addClass('disabled');
    $('#spinSpan').addClass('spinning');
}

function setEsbEnvironmentFailure(result) {
    if (typeof console !== 'undefined') {
        console.log('setEsbEnvironmentFailure ajax call error');
        console.log(result);
    }
}

function getEsbEnvironmentSuccess(result) {
    $('#currentEsbEnvironmentDiv').html('<div class="fade in"><h4>' + result + '</h4></div>');
    $('#spinSpan').removeClass('spinning');
    $('#btnGetCurrentEsbEnvironment').removeClass('disabled');
}

function getEsbEnvironmentBegin() {
    $('#btnGetCurrentEsbEnvironment').addClass('disabled');
    $('#spinSpan').addClass('spinning');
}

function getEsbEnvironmentFailure(result) {
    if (typeof console !== 'undefined') {
        console.log('getEsbEnvironmentFailure ajax call error');
        console.log(result);
    }
    $('#spinSpan').removeClass('spinning');
    $('#currentEsbEnvironmentDiv').html('<h4>Indeterminate</h4>');
    $('#btnGetCurrentEsbEnvironment').removeClass('disabled');
}