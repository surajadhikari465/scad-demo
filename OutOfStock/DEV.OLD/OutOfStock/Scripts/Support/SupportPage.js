$(document).ready(function () {
    //Switches Navbar from fixed so it goes away when you scroll down
    $("div.navbar").toggleClass('navbar-fixed-top');
    $("div.navbar").toggleClass('navbar-static-top');


    $("#showContactDiv").click(function () {
        $("#contactform").fadeToggle(300);
    });

    $("#showGlossaryDiv").click(function () {
        $("#glossarycontent").fadeToggle(300);
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