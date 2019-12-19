

let toggleHistoryButton = (byDate) => {
    if (byDate) {
        document.getElementById('ItemHistoryByAttribute').style.display = 'none';
        document.getElementById('ItemHistoryByDate').style.display = '';
        document.getElementById('ItemHistoryToggleAttribute').style.textDecoration = '';
        document.getElementById('ItemHistoryToggleDate').style.textDecoration = 'underline';
    }
    else {
        document.getElementById('ItemHistoryByAttribute').style.display = '';
        document.getElementById('ItemHistoryByDate').style.display = 'none';
        document.getElementById('ItemHistoryToggleAttribute').style.textDecoration = 'underline';
        document.getElementById('ItemHistoryToggleDate').style.textDecoration = '';
    }
}
$(function () {
    $('#ItemHistoryToggleDate').click(() => {
        toggleHistoryButton(true);
    });

    $('#ItemHistoryToggleAttribute').click(() => {
        toggleHistoryButton(false);
    });

    toggleHistoryButton(true);
});
