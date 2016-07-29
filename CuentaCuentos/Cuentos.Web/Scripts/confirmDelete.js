jQuery(document).ready(function () {
    $('div#miscuento').on('click', 'a.delete-btn', function (e) {
        e.preventDefault();
        
        var row = $(this).closest('div.cuento');
        $('#confirmDelete .toDelete').text(row.find('h2.confirmName').text());
        $('#confirmDelete form')
            .attr('action', row.find('a.delete-btn').data('url'))
            .attr('data-id', row.find('a.delete-btn').data('id'));
    });

    $('#confirmDelete form').on('submit', function (e) {
        e.preventDefault();
        var deleteId = $(this).data('id');

        $.ajax({
            type: "POST",
            url: $(this).attr('action'),
        })
        .done(function () {
            $('#flash-messages').flashMessage({ message: "Cuento ha sido elimiinado, satisfactoriamente.", alert: 'success' });
            location.reload();
        })
        .error(function () {
            $('#flash-messages').flashMessage({ message: "Something went wrong, please try again later.", alert: 'error' });
        })
        .always(function () {
            $('#confirmDelete').modal('hide');
        });
    });
});