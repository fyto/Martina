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
            { "data": "userType", "autoWidth": true },
            { "data": null, "autoWidth": true },

            //{
            //    "defaultContent": "<div class='tr-operation'> <button type='button' class='edit-button btn btn-warning'><i class='fa fa-pencil-square-o'></i></button> 	<button type='button' class='delete-button btn btn-danger'><i class='fa fa-trash-o'></i></button> </div>"
            //}
        ],
        columnDefs: [{
            // adiciona los botones en la última columna
            targets: [-1], render: function (a, b, data, d)
            {
                var buttons = "";

                //if (data.userType == 0)
                //{
                //    return "<button type='button'>Go</button>";
                //}

                buttons = "<div class='tr-operation'> <button type='button' class='edit-button btn btn-warning'><i class='fa fa-pencil-square-o'></i></button>" +
                    "<button type = 'button' class='delete-button btn btn-danger'> <i class='fa fa-trash-o'></i></button > </div> ";
               

                return buttons;
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

    $("#btnCallModal").click(function ()
    {
        var validoPhone = document.getElementById('phone-number-ok');
        var validoEmail = document.getElementById('email-ok');

        validoPhone.innerHTML = '';
        validoEmail.innerHTML = '';

        $("#create-modal").modal();
    });

    $("#btnCreateModal").click(function ()
    {
        var selector = document.getElementsByName('selector');
        var valorRadio = '';

        var validator = false;
        var idTypeUser;

        for (var i = 0; i < selector.length; i++)
        {
            if (selector[i].checked == true)
            {
               valorRadio = selector[i].value;
            }
        }        

        var typeUser = valorRadio;
        var email = $("#email").val();
        var name = $("#name").val();
        var lastName = $("#last-name").val();
        var address = $("#address").val();
        var phoneNumber = $("#phone-number").val();

        if (email == null || email == '' || email == undefined)
        {
            toastr.error('Debe introducir un email', "Error");

            $("#email").addClass("input-red");
            setTimeout(function () {
                $("#email").removeClass('input-red');
            }, 2000)

            validator = true;
        }

        if (name == null || name == '' || name == undefined)
        {
            toastr.error('Debe introducir un nombre', "Error");

            $("#name").addClass("input-red");
            setTimeout(function () {
                $("#name").removeClass('input-red');
            }, 2000)

            validator = true;
        }

        if (lastName == null || lastName == '' || lastName == undefined)
        {
            toastr.error('Debe introducir un apellido', "Error");

            $("#last-name").addClass("input-red");
            setTimeout(function () {
                $("#last-name").removeClass('input-red');
            }, 2000)

            validator = true;
        }

        if (validator == false)
        {  
            ActionCreateUser(typeUser, email, name, lastName, address, phoneNumber);
        }
       
    });

    function ActionCreateUser(typeUser, email, name, lastName, address, phoneNumber)
    {
        var userObject = {};

        userObject.userType = typeUser;
        userObject.email = email;
        userObject.firstName = name;
        userObject.lastName = lastName;
        userObject.address = address;
        userObject.phoneNumber = phoneNumber;     

        $.ajax({
            type: "POST",
            url: "/Users/Create",
            data: JSON.stringify(userObject),
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            success: function (response)
            {
                if (response == 'Success')
                {
                    toastr.success('se ha creado el usuario ' + name +' '+ lastName + ' .' , "Guardado");

                    $("#create-modal").modal('hide');
                    $('#TableUsers').DataTable().ajax.reload();
                }
                //if (response == 'Duplicate') {
                //    toastr.warning(careName + ' ya existe como un cuidado.', "Advertencia");

                //    $("#edit-modal").modal('hide');
                //}
                //if (response.includes('Cannot insert the value NULL into column')) {
                //    toastr.error('Ha ocurrido un problema, intentelo más tarde', "Error");

                //    $("#edit-modal").modal('hide');
                //}
                //if (response == 'Failed') {
                //    toastr.error('Ha ocurrido un problema, intente más tarde', "Error");

                //    $("#edit-modal").modal('hide');
                //}
            },
            error: function (xhr, status, error) {
                toastr.error(error, "Error");
            }
        });
    }

    document.getElementById('email').addEventListener('input', function ()
    {
        var campo = event.target;
        var valido = document.getElementById('email-ok');

        $("#email-ok").removeClass("span-green");
        $("#email-ok").removeClass("span-red");
     
        var emailRegex = /^[-\w.%+]{1,64}@(?:[A-Z0-9-]{1,63}\.){1,125}[A-Z]{2,63}$/i;
        //Se muestra un texto a modo de ejemplo, luego va a ser un icono
        if (emailRegex.test(campo.value))
        {
            $("#email-ok").addClass("span-green");
          
            valido.innerText = "Email válido";
        }
        else
        {
            $("#email-ok").addClass("span-red");
        
            valido.innerText = "Email inválido";
        }
    });

    jQuery("#phone-number").on('input', function (evt)
    {
        var valido = document.getElementById('phone-number-ok');

        $("#phone-number-ok").removeClass("span-green");
        $("#phone-number-ok").removeClass("span-red");

        // Permite solo números
        jQuery(this).val(jQuery(this).val().replace(/[^0-9]/g, ''));

        if (jQuery(this).val().length > 8)
        {
            this.value = this.value.slice(0, 8);

            $("#phone-number-ok").addClass("span-green");

            valido.innerText = "Número válido";
        }
        else
        {
            $("#phone-number-ok").addClass("span-red");

            valido.innerText = "Número no válido";
        }
    });

});
