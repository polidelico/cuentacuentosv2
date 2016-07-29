$(function () {
    $.extend($.fn.dataTableExt.oStdClasses, {
        "sWrapper": "dataTables_wrapper form-inline"
    });
    $(".dTable").dataTable({
        /*bPaginate: false,
        bSort: false,
        bInfo: false,*/
        bJQueryUI: false,
        bAutoWidth: false,
        sPaginationType: "full_numbers",
        sDom: "<\"table-header\"fl>t<\"table-footer\"ip>"
    });
    $(".dTable-small").dataTable({
        /*bPaginate: false,
        bSort: false,
        bInfo: false,*/
        iDisplayLength: 5,
        bJQueryUI: false,
        bAutoWidth: false,
        sPaginationType: "full_numbers",
        sDom: "<\"table-header\"fl>t<\"table-footer\"ip>"
    });
});