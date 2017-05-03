/* ---------------------------------------------------------------------------
// login.js - v1.0
// ---------------------------------------------------------------------------
// Author: Bryce Bartley (SW SWC)
// Last Modified: 7/2/2013
// 
// CHANGELOG 
//    - 7/2/2013 - Files prepped for global use
//---------------------------------------------------------------------------*/

/* ---------------------------------------------------------------------------
// Purpose: Additional knockout binding handler to process enter kepresses
//---------------------------------------------------------------------------*/
ko.bindingHandlers.executeOnEnter = {
    init: function (element, valueAccessor, allBindingsAccessor, viewModel) {
        var allBindings = allBindingsAccessor();
        $(element).keypress(function (event) {
            var keyCode = (event.which ? event.which : event.keyCode);
            if(keyCode == 13){
                allBindings.executeOnEnter.call(viewModel);
                return false;
            }
            return true;
        });
    }
};

ko.bindingHandlers.button = {
    init: function (element, valueAccessor, allBindingsAccessor, viewModel) {
        $(element).button();
    },
    update: function (element, valueAccessor, allBindingsAccessor, viewModel) {
        var value = ko.utils.unwrapObservable(valueAccessor()),
            disabled = ko.utils.unwrapObservable(value.disabled);

        $(element).button("option", "disabled", disabled);
    }
};

/* ---------------------------------------------------------------------------
// Purpose: App initalization code
//---------------------------------------------------------------------------*/
$(document).ready(function () {
    // Load KnockoutJS
    ko.applyBindings(new appViewModel());

    $("button.btn_submit").button();
    setInterval(function () {
        //console.log("tick tock");
        var hue = get_random_color();
        $('#logo_background').animate({ backgroundColor: hue }, 5000);
    }, 7000);
});

function rand(min, max) {
    return parseInt(Math.random() * (max - min + 1), 10) + min;
}

function get_lighter_color(hslColor) {
    var h = hslColor.split('hsl(')[0].split(',')[0];
    var s = 40;
    var l = 57;
    return 'hsl(' + h + ',' + s + '%,' + l + '%)';
}

function get_random_color() {
    var h = rand(1, 360);
    var s = rand(40, 50);
    var l = rand(40, 50);
    return 'hsl(' + h + ',' + s + '%,' + l + '%)';
}

function get_random_fixed_color(s, l) {
    var h = rand(1, 222);
    var s = s;
    var l = l;
    return 'hsl(' + h + ',' + s + '%,' + l + '%)';
}

function get_random_blue_color() {
    var h = rand(180, 250);
    var s = 40;
    var l = 40;
    return 'hsl(' + h + ',' + s + '%,' + l + '%)';
}

function get_random_green_color() {
    var h = rand(76, 147);
    var s = 40;
    var l = 40;
    return 'hsl(' + h + ',' + s + '%,' + l + '%)';
}

/* ---------------------------------------------------------------------------
// Class : appViewModel()
// Purpose: The primary class for the login application view model
//---------------------------------------------------------------------------*/
function appViewModel() {
	var self = this;

	self.isLoading = ko.observable(false);  // Sets loading state
	self.un = ko.observable('');    // internal username
    self.pw = ko.observable('');    // internal password

    // Computed value to check if the form is valid
    self.isValid = ko.computed(function(){
        if (self.un().length == 0) { return false; }
        else if (self.pw().length == 0) { return false; }
    	else{ return true; }
    });

    /* ---------------------------------------------------------------------------
    // Function : showLoading()
    // Purpose: Shows the loading animation and centers it on the screen
    //---------------------------------------------------------------------------*/
    self.showLoading = function(){
    	// Resize mask to fit document
    	$(".mask").css({height:$(document).height() + 'px'});
    	
    	// Center loading message on screen
    	var w = $('.loader').width();
		var h = $('.loader').height();
		var t = $(window).scrollTop() + ($(window).height() / 2) - (h/2);
		var l = $(window).scrollLeft() + ($(window).width() / 2) - (w/2);
		$(".loader").css({top:t+'px', left:l+'px'});
		
		// Setup window scroll event
		$(window).bind('scroll', function(){
			var w = $('.loader').width();
			var h = $('.loader').height();
			var t = $(window).scrollTop() + ($(window).height() / 2) - (h/2);
			var l = $(window).scrollLeft() + ($(window).width() / 2) - (w/2);
			$(".loader").css({top:t+'px', left:l+'px'});
		});
    	
    	self.isLoading(true);
    };

    /* ---------------------------------------------------------------------------
    // Function : login()
    // Purpose: Send username and password to the server for validation if 
    // successful redirect to main page.
    //---------------------------------------------------------------------------*/
    self.login = function () {
        // Show loading animation
        self.showLoading();

        // Build form data object
        var data = {
            A: 'login',
            u: self.un(),
            p: self.pw()
        }

        // Send to server to validate
        $.ajax({
            url: 'ajax/GetDataLogin.aspx',
            type: 'post',
            data: data,
            dataType: 'JSON',
            success: function (result) {
                self.isLoading(false);

                if (result.msg == "OK") {
                    // Check for referring page
                    var ref = '';
                    try {
                        ref = window.location.href.slice(window.location.href.indexOf('?') + 1).split("ref=")[1];
                    } catch (err) { }

                    if (ref != undefined && ref != '') {
                        window.location = ref;
                    } else {
                        window.location = 'Default.aspx';
                    }
                } else {
                    alert(result.msg);
                }
            },
            error: function () {
                alert("ERROR: Could not login due to ajax error");
                self.isLoading(false);
            }
        });
    }; 
};