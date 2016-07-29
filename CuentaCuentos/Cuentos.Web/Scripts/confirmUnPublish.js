jQuery(document).ready(function () {
    $('div#miscuento').on('click', 'a.un-publish-btn', function (e) {
        e.preventDefault();

        var row = $(this).closest('div.cuento');
        $('#confirmUnPublish .toUnPublish').text(row.find('h2.confirmName').text());
        $('#confirmUnPublish form')
            .attr('action', row.find('a.un-publish-btn').data('url'))
            .attr('data-id', row.find('a.un-publish-btn').data('id'));
    });

    $('#confirmUnPublish form').on('submit', function (e) {
        e.preventDefault();
        var deleteId = $(this).data('id');

        $.ajax({
            type: "POST",
            url: $(this).attr('action'),
        })
        .done(function () {
            $('#flash-messages').flashMessage({ message: "Cuento enviado para aprobación.", alert: 'success' });
            
            location.reload();

        })
        .error(function () {
            $('#flash-messages').flashMessage({ message: "Something went wrong, please try again later.", alert: 'error' });
        })
        .always(function () {
            $('#confirmUnPublish').modal('hide');
        });
    });
});