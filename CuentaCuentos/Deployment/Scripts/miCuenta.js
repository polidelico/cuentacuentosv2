$(function () {
    var hash = window.location.hash;
    hash && $('ul.nav a[href="' + hash + '"]').tab('show');

    $('.nav-tabs a').click(function (e) {
        $(this).tab('show');
        var scrollmem = $('body').scrollTop();
        window.location.hash = this.hash;
        $('html,body').scrollTop(scrollmem);
    });

    $("#clickMainImage").on("click", function () {
        $("#uploadMainImage").click();
    })

    $('.story-options a.show-menu').on('click', toggleStoryOptions)

    $('.story-options .popover-options a.disabled').on('click', function (e) {
        e.preventDefault();
    })
});

function toggleStoryOptions(e) {

    e.preventDefault();   
    var $this = $(this);
    var $options = $this.siblings('div.popover-options');
    var $allOptionsPanels = $('.story-options .popover-options');

    $allOptionsPanels.fadeOut(200);    

    if (!$options.is(':visible')) {
        $options.fadeIn(200);
    }

}