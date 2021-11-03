'use strict';

$(document).ready(async function () {

    // Error: DataTable is not a function
    $.noConflict();

    // Inicializar Tooltips
    $('[data-toggle="tooltip"]').tooltip()

    var table = $('#TableDiseaseTypes').DataTable({
        "processing": true,
        "responsive": true,
        "destroy": true,
        "ajax": {
            "contentType": "application/json; charset=utf-8",
            "url": "/DiseaseTypes/GetDiseaseTypes",
            "type": "POST",
            "datatype": "json",
            "dataSrc": ""
        },
        "columns": [
            { "data": "id", "autoWidth": true },
            { "data": "description", "autoWidth": true },
            {
                "defaultContent": "<div class='tr-operation'> <button type='button' class='edit-button btn btn-warning'><i class='fa fa-pencil-square-o'></i></button>" + 
                    "<button type='button' class= 'edit-button btn btn-info' > <i class='fa fa-heartbeat'></i></button>" +
                    "<button type='button' class= 'delete-button btn btn-danger' > <i class='fa fa-trash-o'></i></button></div>"
            }
        ],
        fixedColumns: {
            heightMatch: 'none'
        },
        "language": {
            "url": "//cdn.datatables.net/plug-ins/9dcbecd42ad/i18n/Spanish.json"
        },
        "aLengthMenu": [
            [25, 50, 100, 200, -1],
            [25, 50, 100, 200, "Todos"]
        ]
    });


});