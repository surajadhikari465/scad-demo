$(function () {
    var interval = null;

    $(".details-link").click(function (e) {
        e.preventDefault();
        if (interval !== null) {
            clearInterval(interval)
        }
        $.get($(this).attr("href"), function (data) {
            $(".details").html(data)
            interval = setInterval(refreshApplicationDetails, 3000, this);
        });
    });
});

function refreshApplicationDetails(that) {
    $.get(that.url, function (data) {
        console.log('setting data');
        $(".details").html(data)
    });
}