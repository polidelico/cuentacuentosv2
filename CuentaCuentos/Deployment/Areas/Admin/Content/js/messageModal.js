jQuery(document).ready(function () {
    $('tbody').on('click', 'a.read-mess-btn', function (e) {
        e.preventDefault();
        var row = $(this).closest('tr');
        $('#messageModal .subject').text(row.find('td.Subject').text());
        $('#messageModal .name').text(row.find('td.Name').text());
        $('#messageModal .from').html('<a href="mailto:' + row.find('td.From').text() + '" class="btn-link">' + row.find('td.From').text() + '</a>');
        $('#messageModal .commentsText').text(row.find('td.commentsText').text());
        $('#messageModal form')
            .attr('action', row.find('a.read-mess-btn').attr('data-url'))
            .attr('data-id', row.attr('data-id'));
    });

    $('#messageModal form').on('submit', function (e) {
        var messageId = $(this).attr('data-id');
        e.preventDefault();

        $.ajax({
            type: "POST",
            url: $(this).attr('action'),
        })
        .done(function () {

            var $readTbody = $('#read table tbody');
            var $row = $('#item-' + messageId);

            if ($readTbody.find('.dataTables_empty').length) {
                $readTbody.empty();
            }

            $row.remove();
            $readTbody.append($row);
            $row.find('.approve-col').empty().text('Si');
            $('#flash-messages').flashMessage({ message: "Mensaje marcado como leido satisfactoriamente.", alert: 'success' });
        })
        .error(function () {
            $('#flash-messages').flashMessage({ message: "Ha ocurrido un error, intente mas tarde.", alert: 'error' });
        })
        .always(function () {
            $('#messageModal').modal('hide');
        });
    });
});