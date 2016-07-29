
$(window).load(function () {

    //mirror image position & size from editor
    var $imgs = $('.cuento figure img');
    var errorText = typeof contactMeByErrorText != 'undefined' ? contactMeByErrorText : '';
    var divisor = typeof homeDivisor != 'undefined' ? homeDivisor : 1.3;

    $imgs.each(function (i, e) {

        var $element = $(e);
        var width = $element[0].width;
        var newWidth = width / divisor;

        $element.css({ 'width': newWidth, 'margin': '0 auto' })
            .closest('.center-inner').siblings('.shade').fadeOut(200);

    });

})