'use strict';

$(document).ready(async function ()
{
    // Error: DataTable is not a function
    $.noConflict();

    // Inicializar Tooltips
    $('[data-toggle="tooltip"]').tooltip()

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
            { "data": "id", "autoWidth": true },
            { "data": "description", "autoWidth": true },
            { "data": "fullName", "autoWidth": true },
            { "data": "creationDate", "autoWidth": true, render: formatDate },
            {
                "defaultContent": "<div class='tr-operation'> <button type='button' class='edit-button btn btn-warning'><i class='fa fa-pencil-square-o'></i></button> 	<button type='button' class='delete-button btn btn-danger'><i class='fa fa-trash-o'></i></button> </div>"
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

    var getDataEdit = function (tbody, table)
    {
        $(tbody).on("click", "button.edit-button", function ()
        {
            var data = table.row($(this).parents("tr")).data();

            var careName = data.description;
            var id = data.id;

            $("#care-name-modal").val(careName);
            $("#care-id-modal").val(id);

            $("#care-creation-date-modal").val(data.creationDate);
            $("#care-first-name-modal").val(data.firstName);
            $("#care-last-name-modal").val(data.lastName);

            $("#edit-modal").modal();
        });
    }

    var getDataDelete = function (tbody, table)
    {
        $(tbody).on("click", "button.delete-button", function ()
        {
            var data = table.row($(this).parents("tr")).data();
                
            var careName = data.description;
            var id = data.id;

            $("#care-name-modal-delete").val(careName);
            $("#care-id-modal-delete").val(id);

            $("#label-delete").html('¿Desea borrar el cuidado <strong>' + careName + '</strong> ?');

            $("#delete-modal").modal();
        });
    }

    getDataEdit("#TableCares tbody", table);

    getDataDelete("#TableCares tbody", table);

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
        var creationDate = $("#care-creation-date-modal").val();
        var firstName = $("#care-first-name-modal").val();
        var lastName = $("#care-last-name-modal").val();
    
        if (careName != '' && careName != null)
        {
            ActionEditModal(idCare, careName, creationDate, firstName, lastName);
        }
        else
        {
            toastr.error('Debe introducir un nombre de cuidado', "Error");
            $("#care-name-modal").focus();
        }
    });

    $("#btnDeleteModal").click(function ()
    {
        var careName = $("#care-name-modal-delete").val();
        var idCare = $("#care-id-modal-delete").val();

        if (idCare != '' && idCare != null && idCare != undefined)
        {           
            ActionDeleteModal(idCare, careName);
        }
        else
        {
            toastr.error('A ocurrido un error, intente más tarde', "Error");    
        }
    });

    function formatDate(data, type, full, meta)
    {
        var date = data;

        if (date) {

            return moment(date).format('DD-MM-YYYY');
        }
    }

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
                        $('#TableCares').DataTable().ajax.reload();
                    }
                    if (msg == 'Duplicate')
                    {
                        toastr.warning(careName + ' ya existe como un cuidado.', "Advertencia");
                    }
                    if (msg.includes('Cannot insert the value NULL into column'))
                    {
                        toastr.error('Ha ocurrido un problema, intentelo más tarde', "Error");
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

    function ActionEditModal(id, careName, creationDate, firstName, lastName) {

        var careObject = {};

        careObject.id = id;
        careObject.description = careName;
        careObject.creationDate = creationDate;
        careObject.firstName = firstName;
        careObject.lastName = lastName;

        $.ajax({
            type: "POST",
            url: "/Cares/Edit",
            data: JSON.stringify(careObject),
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            success: function (response)
            {
                if (response == 'Success')
                {
                    toastr.success(careName + ' se ha guardado como un cuidado.', "Guardado");

                    $("#edit-modal").modal('hide');
                    $('#TableCares').DataTable().ajax.reload();
                }
                if (response == 'Duplicate')
                {
                    toastr.warning(careName + ' ya existe como un cuidado.', "Advertencia");

                    $("#edit-modal").modal('hide');
                }
                if (response.includes('Cannot insert the value NULL into column'))
                {
                    toastr.error('Ha ocurrido un problema, intentelo más tarde', "Error");

                    $("#edit-modal").modal('hide');
                }
                if (response == 'Failed')
                {
                    toastr.error('Ha ocurrido un problema, intente más tarde', "Error");

                    $("#edit-modal").modal('hide');
                }
            },
            error: function (xhr, status, error) 
            {
                toastr.error(error, "Error");
            }
        });

    }

    function ActionDeleteModal(id, careName)
    {

        $.ajax({
            type: "POST",
            url: "/Cares/Delete",
            data: { id: id },
            success: function (response)
            {
                if (response == 'Success')
                {
                    toastr.success(careName + ' se ha eliminado como un cuidado.', "Eliminado");

                    $("#delete-modal").modal('hide');
                    $('#TableCares').DataTable().ajax.reload();
                }
                if (response == 'NoExist')
                {
                    toastr.warning(careName + ' no existe como un cuidado.', "Advertencia");

                    $("#delete-modal").modal('hide');
                }
                if (response == 'Error')
                {
                    toastr.error('Ha ocurrido un problema, intente más tarde', "Error");

                    $("#delete-modal").modal('hide');
                }
            },
            error: function (xhr, status, error)
            {
                console.log("error");
                toastr.error(error, "Error");
            }
        });

    }

});