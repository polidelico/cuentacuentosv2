jQuery(document).ready(function () {
    $('tbody').on('click', 'a.confirmDelete', function (e) {
        e.preventDefault();
        var row = $(this).closest('tr');
        $('#confirmDelete .toDelete').text(row.find('td.confirmName').text());
        $('#confirmDelete form')
            .attr('action', row.find('a.confirmDelete').attr('data-url'))
            .attr('data-id', row.attr('data-id'));
        //$('#confirmDelete form').attr('action', $(this).data('url')).data('id', $(this).data('id'));
    });

    $('#confirmDelete form').on('submit', function (e) {
        var deleteId = $(this).attr('data-id');
        e.preventDefault();

        $.ajax({
            type: "DELETE",
            url: $(this).attr('action')
        })
        .done(function (data) {

            console.log(data);
            switch (data) {
                case "story":
                    console.log('in')
                    var $row = $('#item-' + deleteId);
                    $row.find('.state').text("Eliminado");
                    $row.find('.approve-col').empty().text("No");
                    
                    break;
                default: 
                    $('#item-' + deleteId).fadeOut(500, function () {
                        $(this).remove();
                    });                
            }

            $('#flash-messages').flashMessage({ message: "El registro ha sido eliminado exitosamente.", alert: 'success' });
            
        })
        .fail(function (status) {

            var msg;
            if (status.responseText.indexOf("inUse") > -1) {
                msg = "La opción que intenta borrar esta en uso.";
            } else {
                msg = "Ha ocurrido un error, intente mas tarde.";
            }            
            
            $('#flash-messages').flashMessage({ message: msg, alert: 'error' });

        })
        .always(function () {
            $('#confirmDelete').modal('hide');
        });
    });
});