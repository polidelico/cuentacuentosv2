jQuery(document).ready(function () {
    $('div#miscuento').on('click', 'a.publish-btn', function (e) {
        e.preventDefault();

        var row = $(this).closest('div.cuento');
        $('#confirmPublish .toPublish').text(row.find('h2.confirmName').text());
        $('#confirmPublish form')
            .attr('action', row.find('a.publish-btn').data('url'))
            .attr('data-id', row.find('a.publish-btn').data('id'));
    });

    $('#confirmPublish form').on('submit', function (e) {
        e.preventDefault();
        var deleteId = $(this).data('id');

        console.log("Id: " + deleteId);
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
            $('#confirmPublish').modal('hide');
        });
    });
});