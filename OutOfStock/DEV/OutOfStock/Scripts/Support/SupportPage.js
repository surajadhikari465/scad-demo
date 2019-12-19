$(document).ready(function () {
    //Switches Navbar from fixed so it goes away when you scroll down
    $("div.navbar").toggleClass('navbar-fixed-top');
    $("div.navbar").toggleClass('navbar-static-top');


    //$("#showContactDiv").click(function () {
    //    $("#contactform").fadeToggle(300);
    //});

    //$("#showGlossaryDiv").click(function () {
    //    $("#glossarycontent").fadeToggle(300);
    //});

    $("#btnRequestAccess").click(function() {
        window.open(
            "http://orchard/ServiceCatalog/RequestOffering/c56852ac-9601-fc40-3ad9-84f8474f8f64,e389e45b-5e5e-04e6-fd70-7a8196a98368",
            "_blank" // <- This is what makes it open in a new window.
        );
    });

    $("#btnReportProblemRegional").click(function () {
        window.open(
            "http://orchard/ServiceCatalog/RequestOffering/192cad90-e445-b07b-22cf-b5cae0ff7c18,3cf7429d-f551-f959-2352-e4b982c4ad98",
            "_blank" // <- This is what makes it open in a new window.
        );
    });

    $("#btnReportProblemStore").click(function () {
        window.open(
            "http://orchard/ServiceCatalog/RequestOffering/192cad90-e445-b07b-22cf-b5cae0ff7c18,c28c378b-5064-41b8-13dc-f43fdd65c711",
            "_blank" // <- This is what makes it open in a new window.
        );
    });

    $("#cancelEmail").click(function () {
        $("#contactform").fadeOut(300);
    });

    $('#sendEmail').click(function () {
        sendemail();
    });

    $('a').click(function () {
        $('html, body').animate({
            scrollTop: $($.attr(this, 'href')).offset().top
        }, 500);
        return false;
    });

});

function toggleAnswer(answer) {
    var elem = "#" + answer;
    $(elem).fadeToggle(300);
    adjustHeights();
}


function sendemail() {
    var name = $("#name").val();
    var details = $("#details").val();
    var validData = validate(details);

    if (validData) {
        $.ajax({
            url: "/Support/SendEmail",
            cache: false,
            data: {
                name: name,
                details: details
            }
        }).success(function (html) {
            toastr.success('Email Sent Successfully');
            $("#contactform").fadeOut(300);
        }).error(function (html) {
            toastr.error('Email did not send, please try again');
        });
    }
    else {
        if (details === '' || details === undefined) {
            toastr.error('Please fill in details');
        }
        else {
            toastr.error('Please fill in required information');
        }
    }
}
function validate(requestDetails) {
    if (requestDetails === '' || requestDetails === undefined) {
        return false;
    }
    return true;
}