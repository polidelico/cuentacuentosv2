jQuery(document).ready(function () {
    $('tbody').on('click', 'a.approve-btn', function (e) {
        e.preventDefault();
        var row = $(this).closest('tr');
        $('#confirmApprove .toApprove').text(row.find('td.confirmName').text());
        $('#confirmApprove form')
            .attr('action', row.find('a.approve-btn').data('url'))
            .attr('data-id', row.data('id'));
    });

    $('#confirmApprove form').on('submit', function (e) {
        var approveId = $(this).attr('data-id');
        e.preventDefault();

        $.ajax({
            type: "POST",
            url: $(this).attr('action'),
        })
        .done(function () {
            $('#flash-messages').flashMessage({ message: "Aprobación realizada satisfactoriamente.", alert: 'success' });
            $('#item-' + approveId).find('.approve-col').empty().text('Si');
        })
        .error(function () {
            $('#flash-messages').flashMessage({ message: "Ha ocurrido un error. Intente mas tarde.", alert: 'error' });
        })
        .always(function () {
            $('#confirmApprove').modal('hide');
        });
    });
});