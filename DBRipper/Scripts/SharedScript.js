$(document).ready(function () {

    $('.tab').click(function () {
        $('.active-tab').removeClass('.active-tab');
        $('.active-panel').removeClass('.active-panel');
        $(this).addClass('.active-tab');
        $('.content-' + $(this).attr('id')).addClass('.active-panel');
    });

});