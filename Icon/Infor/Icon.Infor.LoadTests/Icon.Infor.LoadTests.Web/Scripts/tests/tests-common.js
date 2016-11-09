$(function () {
    console.log('starting');
    $('#RunBtn').click(function () {
        $.ajax({
            url: $('#action')
        }).done(function (data) {
            setInterval(function () {
                $.ajax({
                    method: 'POST',
                    url: 'Status',
                    data: { TestName: $('#TestName') }
                }).done(function (data) {
                    console.log('in the done');
                    console.log(data);
                    for (var property in data) {
                        $('#' + property.toString()).html(data[property]);
                    }
                }).fail(function (a1, a2, a3, a4) {
                    console.log('failed');
                }).always(function () {
                    console.log('stuff');
                });
            }, 3000);
        }).fail(function (data) {
            console.log('Failed ')
        });
    });
});