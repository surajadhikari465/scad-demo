$(function () {
    console.log('starting');
    setInterval(function () {
        $.ajax({
            url: "ProductStatus"
        }).done(function (data) {
            console.log('in the done');
            console.log(data);
            for (var property in data) {
                console.log(property);
            }
        }).fail(function (a1, a2, a3, a4) {
            console.log('failed');
        }).always(function () {
            console.log('stuff');
        });
    }, 2000);
});