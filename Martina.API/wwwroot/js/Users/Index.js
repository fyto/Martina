'use strict';

$(document).ready(async function () {

    // Error: DataTable is not a function
    $.noConflict();

    // Inicializar Tooltips
    $('[data-toggle="tooltip"]').tooltip()

    var table = $('#TableUsers').DataTable({
        "processing": true,
        "responsive": true,
        "destroy": true,
        "ajax": {
            "contentType": "application/json; charset=utf-8",
            "url": "/Users/GetUsers",
            "type": "POST",
            "datatype": "json",
            "dataSrc": ""
        },
        "columns": [          
            { "data": "fullName", "autoWidth": true },
            { "data": "email", "autoWidth": true },
            { "data": "phoneNumber", "autoWidth": true },
            { "data": "nameTypeUser", "autoWidth": true },
            { "data": null, "autoWidth": true },

            //{
            //    "defaultContent": "<div class='tr-operation'> <button type='button' class='edit-button btn btn-warning'><i class='fa fa-pencil-square-o'></i></button> 	<button type='button' class='delete-button btn btn-danger'><i class='fa fa-trash-o'></i></button> </div>"
            //}
        ],
        columnDefs: [{
            // adiciona los botones en la última columna
            targets: [-1], render: function (a, b, data, d)
            {
                //if (data.userType == 0)
                //{
                //    return "<button type='button'>Go</button>";
                //}
               

                return "<div class='tr-operation'> <button type='button' class='edit-button btn btn-warning'><i class='fa fa-pencil-square-o'></i></button>" + 
                                                  "<button type = 'button' class='delete-button btn btn-danger' > <i class='fa fa-trash-o'></i></button > </div> ";
            }
        }],
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

    $("#btnCreateModal").click(function ()
    {
        //var careName = $("#care-name-modal").val();
        //var idCare = $("#care-id-modal").val();
        //var creationDate = $("#care-creation-date-modal").val();
        //var firstName = $("#care-first-name-modal").val();
        //var lastName = $("#care-last-name-modal").val();

        //if (careName != '' && careName != null)
        //{
        //    ActionEditModal(idCare, careName, creationDate, firstName, lastName);
        //}
        //else
        //{
        //    toastr.error('Debe introducir un nombre de cuidado', "Error");
        //    $("#care-name-modal").focus();
        //}

        $("#create-modal").modal();
    });

});
