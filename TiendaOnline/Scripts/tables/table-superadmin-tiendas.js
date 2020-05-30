//Activar data table

var tabla;
if (sessionStorage.getItem('select_value_superadmin_tienda') == null) {
    sessionStorage.setItem('select_value_superadmin_tienda', 10);
}

function InicializarDataTable() {

    tabla = $('#tablalista').DataTable({
        searching: false,
        "pageLength": sessionStorage.getItem('select_value_superadmin_tienda'),
        "aaSorting": [],
        "language": {
            "sProcessing": "Procesando...",
            "sLengthMenu": "Mostrar _MENU_ registros",
            "sZeroRecords": "No se encontraron resultados",
            "sEmptyTable": "Ningún dato disponible en esta tabla",
            "sInfo": "Mostrando registros del _START_ al _END_ de un total de _TOTAL_ registros",
            "sInfoEmpty": "Mostrando registros del 0 al 0 de un total de 0 registros",
            "sInfoFiltered": "(filtrado de un total de _MAX_ registros)",
            "sInfoPostFix": "",
            "sSearch": "Buscar:",
            "sUrl": "",
            "sInfoThousands": ",",
            "sLoadingRecords": "Cargando...",
            "oPaginate": {
                "sFirst": "Primero",
                "sLast": "Último",
                "sNext": "Siguiente",
                "sPrevious": "Anterior"
            },
            "oAria": {
                "sSortAscending": ": Activar para ordenar la columna de manera ascendente",
                "sSortDescending": ": Activar para ordenar la columna de manera descendente"
            }
        }
    });

}

InicializarDataTable();

//Cambio Select Tabla
$(function () {
    $("select").change(function () {
        var val = this.value;
        sessionStorage.setItem('select_value_superadmin_tienda', val);
    });
});