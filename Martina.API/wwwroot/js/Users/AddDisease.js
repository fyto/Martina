'use strict';

$(document).ready(async function ()
{
    // Error: DataTable is not a function
    $.noConflict();

    var table = $('#TableDiseases').DataTable({
        "processing": true,
        "responsive": true,
        "destroy": true,
        "ajax": {
            "contentType": "application/json; charset=utf-8",
            "url": "/DiseaseTypes/GetDiseases",
            "type": "POST",
            "datatype": "json",
            "dataSrc": ""
        },
        "columns": [
            { "data": "id", "autoWidth": true },
            { "data": "description", "autoWidth": true },
            { "data": "descriptionDeseaseType", "autoWidth": true },
            {
                "defaultContent": "<div class='tr-operation'> <button type='button' class='edit-button btn btn-warning'><i class='fa fa-pencil-square-o'></i></button> " + 
                                                             "<button type='button' class= 'user-desease-button btn btn-info' > <i class='fa fa-users'></i></button> "  +
                                                             "<button type='button' class='delete-button btn btn-danger'><i class='fa fa-trash-o'></i></button>    </div >"
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

    var diseaseTypes = $("#disease-types");

    function selectDiseaseTypes()
    {
        $.ajax({
            type: "POST",
            url: "/DiseaseTypes/GetDiseaseTypes",
            success: function (data)
            {
                var placeholderSelect = {};

                placeholderSelect.id = 0;
                placeholderSelect.description = '[Seleccione una enfermedad...]';

                data.push(placeholderSelect);

                data.sort(function (a, b)
                {
                    return a.id - b.id || a.name.localeCompare(b.description);
                });

                diseaseTypes.html('');

                for (var i = 0; i < data.length; i++)
                {
                    diseaseTypes.append('<option id="' + data[i].id + '">' + data[i].description + '</option>');
                }              
             
            },
            error: function (req, status, error)
            {
                diseaseTypes.prop("disabled", true);
                $("#disease-name").prop("disabled", true);
                $("#btnCreate").prop("disabled", true);

                toastr.error(error, "Error");
            }
        });
    }

    selectDiseaseTypes();  

    $("#btnCreate").click(function ()
    {
        $("#disease-name").removeClass("input-red");
        $("#disease-types").removeClass("input-red");

        var idDiseaseTypes = $('#disease-types option:selected').attr('id');
        var diseaseTypes = $("#disease-types").val();
        var diseaseName = $("#disease-name").val();

        if (diseaseName != null && diseaseName != ''
            && idDiseaseTypes != 0 && diseaseTypes != '')
        {       
            ActionCreate(idDiseaseTypes, diseaseName);
         
        }
        else 
        {          
            if (diseaseName === '')
            {
                toastr.error('Debe introducir un nombre de enfermedad', "Error");
             
                $("#disease-name").addClass("input-red");
                setTimeout(function () {
                    $("#disease-name").removeClass('input-red');
                }, 2000)               
            }

            if (idDiseaseTypes == 0)
            {
                toastr.error('Debe introducir un tipo de enfermedad', "Error");

                $("#disease-types").addClass("input-red");
                setTimeout(function () {
                    $("#disease-types").removeClass('input-red');
                }, 2000)
            }
        }

        
    });

    function ActionCreate(idDiseaseTypes, diseaseName)
    {
        $.ajax({
            type: "POST",
            url: "/DiseaseTypes/CreateDisease",
            data: { id: idDiseaseTypes, nameDisease: diseaseName },
            success: function (msg) {
                if (msg == 'Success')
                {
                    toastr.success(diseaseName + ' se ha creado como una enfermedad.', "Guardado");

                    $("#disease-name").val('');
                    selectDiseaseTypes();
                   /* $('#TableCares').DataTable().ajax.reload();*/
                }
                if (msg == 'Duplicate')
                {
                    toastr.warning(diseaseName + ' ya existe como un cuidado.', "Advertencia");
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

    var getDataDisease = function (tbody, table)
    {
        $(tbody).on("click", "button.user-desease-button", function ()
        {
            var data = table.row($(this).parents("tr")).data();

            console.log(data);

            //var careName = data.description;
            //var id = data.id;

            //$("#care-name-modal").val(careName);
            //$("#care-id-modal").val(id);

            //$("#care-creation-date-modal").val(data.creationDate);
            //$("#care-first-name-modal").val(data.firstName);
            //$("#care-last-name-modal").val(data.lastName);

            //$("#edit-modal").modal();
        });
    }

    getDataDisease("#TableDiseases tbody", table);
    
});
