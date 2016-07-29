$(function () {
    var $starsContainer = $('span.stars-container');
    var $stars = $starsContainer.find('i.star');
    
    if (isLogged === 'true' && alreadyRate === 'false') {
        $stars.on('click', StarClick);
        $stars.mouseenter(StarIn);
    }

    $starsContainer.mouseleave(StarContainerOut);

});

function StarClick(e) {
    var rateUrl = $(this).closest('span.stars-container').data('url');
    var starsRate = $(this).data('star-num');
    var Id = $(this).closest('span.stars-container').data('storyid');


    console.log(Id);

    $.ajax({
        type: "POST",
        url: rateUrl,
        data: {
            storyId: Id,
            rate: starsRate
        }
    })
    .done(function (msg) {
        var $starsContainer = $('span.stars-container');
        var $stars = $starsContainer.find('i.star');

        $starsContainer.off('mouseleave');
        $stars.off('mouseenter click');
        InitStars(starsRate, true);
    });
}

function StarContainerOut(e) {
    var rating = $(this).data('average');
    InitStars(rating, false);
};

function StarIn(e) {
    var index = $(this).data('star-num');
    InitStars(index, true);

};

function InitStars(rating, isHighlight) {
    var $stars = $('span.stars-container i.star');

    $stars.each(function (i, e) {
        $(this).removeClass('iconStarHalf iconStar iconStarZero starHighlight');
        var currentPosition = $(this).data('star-num');
        var cssClass = currentPosition <= rating ? "iconStar" : rating > currentPosition - 1 ? "iconStarHalf" : "iconStarZero";
        if (isHighlight)
            cssClass += ' starHighlight';

        $(this).addClass(cssClass);
    });
}

function StarContainerIn(e) {
    var $stars = $(this).find('i.star');

    $stars.each(function (i, e) {
        var $star = $(e);

        $star.removeClass('iconStar, iconStarHalf').addClass('iconStarZero');
    });
};