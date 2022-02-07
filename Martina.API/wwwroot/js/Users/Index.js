'use strict';

$(document).ready(async function ()
{
    // Error: DataTable is not a function
    $.noConflict();

    // Inicializar Tooltips
    $('[data-toggle="tooltip"]').tooltip()

    // Oculta el modal de carga
    $('#loading').hide();

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
            { "data": "imageFullPath", render: getImg },
            { "data": "fullName", "autoWidth": true },
            { "data": "email", "autoWidth": true },
            { "data": "phoneNumber", "autoWidth": true },
            { "data": "userType", "autoWidth": true },
            { "data": "userStatus", "autoWidth": true, render: getAlert },
            { "data": null, "autoWidth": true },

            //{
            //    "defaultContent": "<div class='tr-operation'> <button type='button' class='edit-button btn btn-warning'><i class='fa fa-pencil-square-o'></i></button> 	<button type='button' class='delete-button btn btn-danger'><i class='fa fa-trash-o'></i></button> </div>"
            //}
        ],
        columnDefs: [{
            // adiciona los botones en la última columna
            targets: [-1], render: function (a, b, data, d) {

                var buttons = "";

                buttons = "<div class='tr-operation'> <button type='button' class= 'disease-button btn btn-info' btn-column> <i class='fa fa-heartbeat'></i></button>" +
                                                     "<button type='button' class= 'flow-user-button btn btn-primary btn-column'> <i class='fas fa-sitemap'></i></button>" +
                           "</div>";
                            
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

    function selectUserStatus(userStatusId) {

        var userStatus = $("#user-status");
       
        var param = { "userStatusId": userStatusId };

        $.ajax({
            type: "POST",
            url: "/UserStatus/GetPossibleStatusByStatus",
            contentType: 'application/x-www-form-urlencoded',
            data: param,
            success: function (data) {

                console.log(data);

                var placeholderSelect = {};

                placeholderSelect.id = 0;
                placeholderSelect.name = '[Seleccione un estado...]';

                data.push(placeholderSelect);

                data.sort(function (a, b) {
                    return a.id - b.id || a.name.localeCompare(b.description);
                });

                userStatus.html('');

                for (var i = 0; i < data.length; i++) {
                    userStatus.append('<option id="' + data[i].id + '">' + data[i].name + '</option>');
                }

            },
            error: function (req, status, error)
            {
                toastr.error(error, "Error");
            }
        });
    }

    

    var getDataDiseases = function (tbody, table)
    {
        $(tbody).on("click", "button.disease-button", function ()
        {
            var data = table.row($(this).parents("tr")).data();
           
            var id = data.id;
            var fullName = data.fullName;

            if (id != null)
            {
                GetDiseasesByUser(id);

                var nombreUser = document.getElementById('title-strong-diseases');
                nombreUser.innerHTML = fullName

                $("#diseases-modal").modal();            
            }
            else
            {
                toastr.error('Ha ocurrido un problema, intente más tarde', "Error");
            }
        });

     
    }

    var getDataFlow = function (tbody, table)
    {
        $(tbody).on("click", "button.flow-user-button", function ()
        {
            var data = table.row($(this).parents("tr")).data();

            console.log(data);

            // Input type hidden in modal
            $("#user-id").val(data.id)
            $("#user-name").val(data.email)
            $("#old-status").val(data.userStatus)
            $("#old-status-id").val(data.userStatusId)

            var userStrong = document.getElementById("strong-user");
            userStrong.innerHTML = data.email;

            var status = data.userStatus;

            var element = document.getElementById("status-div");

            var alert = '';
            element.innerHTML = '';

            if (status == 'Registrado'){
                alert = '<span class="badge bg-secondary alert-column"> Registrado </span>';
                element.innerHTML += alert;

                selectUserStatus(data.userStatusId);
            }
            if (status == 'Revision') {
                alert = '<span class="badge bg-warning text-dark alert-column"> En Revisión </span>';
                element.innerHTML += alert;

                selectUserStatus(data.userStatusId);
            }
            if (status == 'Aprobado') {
                alert = '<span class="badge bg-success alert-column"> Aprobado </span>';
                element.innerHTML += alert;

                selectUserStatus(data.userStatusId);
            }
            if (status == 'Rechazado') {
                alert = '<span class="badge bg-danger alert-column"> Rechazado </span>';
                element.innerHTML += alert;

                selectUserStatus(data.userStatusId);
            }

            $("#flow-modal").modal();
           
        });


    }

    getDataFlow("#TableUsers tbody", table);

    getDataDiseases("#TableUsers tbody", table);

    function getImg(data, type, full, meta)
    {
        var imagen = data;

        return '<img src="' + imagen + '" style="width: 100px; height: 100px; border-radius: 150px;" />';
    }

    function getAlert(data, type, full, meta)
    {
        var status = data;

        if (status == 'Registrado'){
            return '<span class="badge bg-secondary alert-column"> Registrado </span>';
        }
        if (status == 'Revision') {
            return '<span class="badge bg-warning text-dark alert-column"> En Revisión </span>';
        }
        if (status == 'Aprobado') {
            return '<span class="badge bg-success alert-column"> Aprobado </span>';
        }
        if (status == 'Rechazado') {
            return '<span class="badge bg-danger alert-column"> Rechazado </span>';
        }
    }

    function getToggleSwitch(data, type, full, meta)
    {
        var dataToggle = data;

        if (dataToggle)
        {
            return "<label class='switch'><input type ='checkbox' class='toggle-disease' checked><span class='slider round'></span></label>";
        }
        else
        {
            return "<label class='switch'><input type='checkbox' class='toggle-disease'><span class='slider round'></span></label >";

        }
    }

    function GetDiseasesByUser(id)
    {
        $("#TableDiseasesUser").dataTable().fnDestroy();      

        var tableModal = $('#TableDiseasesUser').DataTable({
            "processing": true,
            "responsive": true,
            "ajax": {
                method: "POST",
                url: "/DiseaseTypes/GetDiseasesByUser",
                data: { id: id },
                "datatype": "json",
                "dataSrc": ""
            },
            "columns": [
                    { "data": "diseaseId", "autoWidth": true },
                    { "data": "diseaseName", "autoWidth": true },
                    { "data": "diseasedStatus", render: getToggleSwitch }, 
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

        tableModal.off('click').on('click', function () {

        });

        tableModal.on('click', 'input.toggle-disease', function ()
        {       
            var dataModal = tableModal.row($(this).closest('tr')).data();

            var userDiseaseObject = {};

            userDiseaseObject.userId = dataModal.userId;
            userDiseaseObject.firstName = dataModal.firstName;
            userDiseaseObject.lastName = dataModal.lastName;
            userDiseaseObject.diseaseId = dataModal.diseaseId;
            userDiseaseObject.diseaseName = dataModal.diseaseName;
            userDiseaseObject.diseasedStatus = dataModal.diseasedStatus;

            $.ajax({
                type: "POST",
                url: "/Users/AddDiseaseToUser",
                data: JSON.stringify(userDiseaseObject),
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                success: function (response)
                {
                    if (response)
                    {                       
                        ReloadDataTableModal(dataModal.userId, tableModal)

                        toastr.success('Se ha actualizado la enfermedad ' + dataModal.diseaseName + ' al usuario ' + dataModal.firstName + ' ' + dataModal.lastName, "Guardado");
                    }
                },
                error: function (xhr, status, error) {
                    toastr.error(error, "Error");
                }
            });

          

         

        });
    }  
   
    $("#btnCallModal").click(function ()
    {
        var validoPhone = document.getElementById('phone-number-ok');
        var validoEmail = document.getElementById('email-ok');

        validoPhone.innerHTML = '';
        validoEmail.innerHTML = '';

     
        $("#create-modal").modal();
    });

    $('#change-user-status').on('submit', (function (e)
    {
        e.preventDefault();

        var formData = new FormData(this);

        var userStatusId = $("#user-status").find('option:selected').attr('id');
        var userStatus = $("#user-status").val()
        var comment = $("#comment").val();
        var userId = $("#user-id").val();
        var userName = $("#user-name").val();
        var oldStatus = $("#old-status").val();
        var oldStatusId = $("#old-status-id").val();

        formData.append('UserStatusId', userStatusId);
        formData.append('UserStatus', userStatus);
        formData.append('UserId', userId);
        formData.append('Comment', comment);
        formData.append('OldStatus', oldStatus);
        formData.append('OldStatusId', oldStatusId);

        var validator = false;

        if (userStatusId == 0 || userStatusId == undefined)
        {
            $("#user-status").addClass("input-red");
            setTimeout(function () {
                $("#user-status").removeClass('input-red');
            }, 2000)

            validator = true;
        }

        if (validator == false)
        {

            $.ajax({
                type: "POST",
                url: "/UserStatus/ChangeUserStatus",
                data: formData,
                contentType: false,
                processData: false,
                success: function (response)
                {
                    if (response == 'Not found' || response != 'Success')
                    {
                        toastr.error('Ha ocurrido un problema, intente más tarde', "Error");

                        $("#flow-modal").modal('hide');
                    }

                    if (response == 'Success')
                    {
                        toastr.success('se ha actualizado el estado del usuario ' + userName + '.', "Actualizado");

                        $("#flow-modal").modal('hide');
                        $('#TableUsers').DataTable().ajax.reload(); 
                    }
                },
                error: function (xhr, status, error) {
                    toastr.error(error, "Error");
                }
            });

        }

    }));

    $('#create-user').on('submit', (function (e)
    {
        e.preventDefault();

        var formData = new FormData(this);
        var selector = document.getElementsByName('selector');
        var valorRadio = '';

        var validator = false;

        for (var i = 0; i < selector.length; i++) {
            if (selector[i].checked == true) {
                valorRadio = selector[i].value;
            }
        }

        var typeUser = valorRadio;
        var email = $("#email").val();
        var name = $("#name").val();
        var lastName = $("#last-name").val();
        var address = $("#address").val();
        var phoneNumber = $("#phone-number").val();
        var photo = $("#photo")[0];

        if (email == null || email == '' || email == undefined) {
            toastr.error('Debe introducir un email', "Error");

            $("#email").addClass("input-red");
            setTimeout(function () {
                $("#email").removeClass('input-red');
            }, 2000)

            validator = true;
        }

        if (name == null || name == '' || name == undefined) {
            toastr.error('Debe introducir un nombre', "Error");

            $("#name").addClass("input-red");
            setTimeout(function () {
                $("#name").removeClass('input-red');
            }, 2000)

            validator = true;
        }

        if (lastName == null || lastName == '' || lastName == undefined) {
            toastr.error('Debe introducir un apellido', "Error");

            $("#last-name").addClass("input-red");
            setTimeout(function () {
                $("#last-name").removeClass('input-red');
            }, 2000)

            validator = true;
        }

        if (validator == false)
        {
           /* ActionCreateUser(typeUser, email, name, lastName, address, phoneNumber, photo);*/
               
            formData.append('ImageFile', photo.files[0]);
            formData.append('FirstName', name);
            formData.append('LastName', lastName);
            formData.append('Email', email);
            formData.append('UserType', typeUser);
            formData.append('Address', address);
            formData.append('PhoneNumber', phoneNumber);
            
            $.ajax({
                type: 'POST',
                url: "/Users/Create",
                data: formData,
                contentType: false,
                processData: false,
                success: function (data)
                {
                    if (data == 'Success')
                    {
                        toastr.success('se ha creado el usuario ' + name + ' ' + lastName + ' Porfavor verifique su email de confirmación.', "Guardado");

                        $("#create-modal").modal('hide');
                        $('#TableUsers').DataTable().ajax.reload();
                    }
                    if (data == 'Duplicate')
                    {
                        toastr.warning(careName + 'Ya existe el usuario ' + email + '.', "Advertencia");

                        $("#create-modal").modal('hide');
                    }
                    if (data == 'Error')
                    {
                        toastr.error('Ha ocurrido un problema, intente más tarde', "Error");
                    }
                },
                error: function (xhr, status, error)
                {
                    toastr.error(error, "Error");
                }
            });

          /*  $('#loading').hide();*/
        }
    
    }));

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

    /* MANEJO DE FOTOGRAFIA */
    document.getElementById('photo').addEventListener('input', function ()
    {
        var campo = event.target;
        var valido = document.getElementById('photo-ok');
      
        var imagetarget = document.getElementById('target-image');    

        $("#photo-ok").removeClass("span-green");
        $("#photo-ok").removeClass("span-red");

        // Carga la imagen en el src
        const [file] = campo.files
        if (file)
        {
            imagetarget.src = URL.createObjectURL(file)
        }

        if (campo.value)
        {
            $("#photo-ok").addClass("span-green");

            valido.innerText = "Imagen cargada";
        }
        else
        {
            $("#photo-ok").addClass("span-red");

            valido.innerText = "Imagen no cargada";
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

    function ActionAddDiseaseToUser(dataModal, tableModal)
    {
        var userDiseaseObject = {};

        userDiseaseObject.userId = dataModal.userId;
        userDiseaseObject.firstName = dataModal.firstName;
        userDiseaseObject.lastName = dataModal.lastName;
        userDiseaseObject.diseaseId = dataModal.diseaseId;
        userDiseaseObject.diseaseName = dataModal.diseaseName;
        userDiseaseObject.diseasedStatus = dataModal.diseasedStatus;

        $.ajax({
            type: "POST",
            url: "/Users/AddDiseaseToUser",
            data: JSON.stringify(userDiseaseObject),
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            success: function (response)
            {
                if (response)
                {
                    ReloadDataTableModal(dataModal.userId, tableModal);

                    toastr.success('Se ha actualizado la enfermedad ' + dataModal.diseaseName + ' al usuario ' + dataModal.firstName + ' ' + dataModal.lastName, "Guardado");
                }
            },
            error: function (xhr, status, error)
            {
                toastr.error(error, "Error");
            }
        });
    
    }

    function ReloadDataTableModal(userId, tableModal)
    {
        tableModal.ajax.data = { id: userId };
        tableModal.ajax.reload();
    }

});
