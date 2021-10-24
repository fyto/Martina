'use strict';

$(document).ready(async function ()
{
    // Error: DataTable is not a function
    $.noConflict();

    var table = $('#TableCares').DataTable({
        "processing": true,
        "responsive": true,
        "destroy": true,
        "ajax": {
            "contentType": "application/json; charset=utf-8",
            "url": "/Cares/GetCares",
            "type": "POST",
            "datatype": "json",
            "dataSrc": ""
        },
        "columns": [
            { "data": "description", "autoWidth": true },
            { "data": "fullName", "autoWidth": true },
            { "data": "creationDate", "autoWidth": true },
            {
                "defaultContent": "<div class='tr-operation'> <button type='button' class='edit-button btn btn-warning'><i class='fa fa-pencil-square-o'></i></button>	<button type='button' class='delete-button btn btn-danger' data-toggle='modal' data-target='#modalEliminar' ><i class='fa fa-trash-o'></i></button> </div>"
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

    

    $("#care-name").focus();

    var getDataEdit = function (tbody, table)
    {
        $(tbody).on("click", "button.edit-button", function ()
        {
            var data = table.row($(this).parents("tr")).data();

            var careName = data.description;
            var id = data.id;

            $("#care-name-modal").val(careName);
            $("#care-id-modal").val(id);

            $("#edit-modal").modal();
        });
    }

    getDataEdit("#TableCares tbody", table);

    $("#btnCreate").click(function ()
    {
        var careName = $("#care-name").val();
        console.log(careName);

        if (careName != '' && careName != null)
        {
            ActionCreate(careName);
        }
        else
        {
            toastr.error('Debe introducir un nombre de cuidado', "Error");
            $("#care-name").focus();
        }      
    });

    $("#btnEditModal").click(function ()
    {
        var careName = $("#care-name-modal").val();
        var idCare = $("#care-id-modal").val();
    
        if (careName != '' && careName != null)
        {
            ActionEditModal(idCare, careName);
        }
        else {
            toastr.error('Debe introducir un nombre de cuidado', "Error");
            $("#care-name-modal").focus();
        }
    });

    function ActionCreate(careName)
    {       
            $.ajax({
                type: "POST",
                url: "/Cares/Create",
                data: { care: careName },
                success: function (msg)
                {
                    if (msg == 'Success')
                    {
                        toastr.success(careName + ' se ha creado como un cuidado.', "Guardado");
                        $("#care-name").val('');

                        $('#TableCares').DataTable().ajax.reload()

                    }
                    if (msg == 'Duplicate')
                    {
                        toastr.warning(careName + ' ya existe como un cuidado.', "Advertencia");
                    }
                    if (msg.includes('Cannot insert the value NULL into column'))
                    {
                        toastr.error('Ha ocurrido un problem zorrona, intente más tarde', "Error");
                    }
                    if (msg == 'Failed')
                    {
                        toastr.error('Ha ocurrido un problema, intente más tarde', "Error");
                    }
                },
                error: function (req, status, error)
                {
                    toastr.error(error, "Error");
                }     
            });
    }


    function ActionEditModal(id, careName) {
        $.ajax({
            type: "POST",
            url: "/Cares/Create",
            data: { care: careName },
            success: function (msg) {
                if (msg == 'Success') {
                    toastr.success(careName + ' se ha creado como un cuidado.', "Guardado");
                    $("#care-name").val('');

                    $('#TableCares').DataTable().ajax.reload()

                }
                if (msg == 'Duplicate') {
                    toastr.warning(careName + ' ya existe como un cuidado.', "Advertencia");
                }
                if (msg.includes('Cannot insert the value NULL into column')) {
                    toastr.error('Ha ocurrido un problem zorrona, intente más tarde', "Error");
                }
                if (msg == 'Failed') {
                    toastr.error('Ha ocurrido un problema, intente más tarde', "Error");
                }
            },
            error: function (req, status, error) {
                toastr.error(error, "Error");
            }

        });

    }

});