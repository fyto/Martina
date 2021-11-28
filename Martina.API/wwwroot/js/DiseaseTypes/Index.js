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
                "defaultContent": "<div class='tr-operation'> " +                
                                  "<button type='button' class= 'disease-button btn btn-info' > <i class='fa fa-heartbeat'></i></button>  </div>"
                 
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

    $("#btnCreate").click(function ()
    {
        var diseaseTypeName = $("#disease-type-name").val();

        if (diseaseTypeName != '' && diseaseTypeName != null)
        {           
            ActionCreate(diseaseTypeName);
        }
        else
        {
            toastr.error('Debe introducir un nombre para el tipo de enfermedad', "Error");
            $("#disease-type-name").focus();
        }
    });

    function ActionCreate(diseaseTypeName) {
        $.ajax({
            type: "POST",
            url: "/DiseaseTypes/CreateDiseaseType",
            data: { nameDiseaseType: diseaseTypeName },
            success: function (msg)
            {
                if (msg == 'Success')
                {
                    toastr.success(diseaseTypeName + ' se ha creado como un tipo de enfermedad.', "Guardado");

                    $("#disease-type-name").val('');
                    $('#TableDiseaseTypes').DataTable().ajax.reload();
                }
                if (msg == 'Duplicate')
                {
                    toastr.warning(diseaseTypeName + ' ya existe como un tipo de enfermedad.', "Advertencia");
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
            error: function (req, status, error) {
                toastr.error(error, "Error");
            }
        });
    }

    var getDataDisease = function (tbody, table)
    {
        $(tbody).on("click", "button.disease-button", function ()
        {
            var data = table.row($(this).parents("tr")).data();

            var request = $.ajax({
                type: "POST",
                url: "/DiseaseTypes/GetDiseases",
                data: { idDiseaseType: data.id },
                success: function (msg)
                { 
                    if (msg.length > 0)
                    {                       
                       var tableModal = $('#TableDiseases').DataTable({
                            "processing": true,
                            "responsive": true,
                            "destroy": true,
                            "ajax": {
                                "url": "/DiseaseTypes/GetDiseases",
                                "type": "POST",
                                data: { idDiseaseType: data.id },
                                "dataSrc": "",
                            },
                            "columns": [
                                { "data": "id", "autoWidth": true },
                                { "data": "description", "autoWidth": true }
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

                        var titleModal = document.getElementById('title-strong');

                        titleModal.innerHTML = data.description;
                        $("#diseases-modal").modal();
                    }
                    else
                    {
                        toastr.error('No existen enfermedades para la categoria ' + "'" + data.description + "'" ,"Error");
                    }
                },
                error: function (req, status, error)
                {
                    toastr.error(error, "Error");
                }
            });

          
          
        });
    }

    getDataDisease("#TableDiseaseTypes tbody", table);


});