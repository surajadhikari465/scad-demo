
$(document).ready(function () {
    // selects all text in the config file textbox when click occurs
    //  using jQuery
    $("#ConfigFilePath").click(function () {
        $("#ConfigFilePath").focus();
        $("#ConfigFilePath").select();
    });
});