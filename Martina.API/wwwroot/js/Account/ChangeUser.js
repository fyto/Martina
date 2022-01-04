'use strict';

$(document).ready(async function () {

   /* GET DATA USER */
    $.ajax({
        type: 'POST',
        url: '/Account/EditUser',
        contentType: 'application/json; charset=utf-8',
        dataType: 'json',
        success: function (data)
        {
            console.log(data.userType);

            if (data.userType == 'Administrador')
            {
                $("#f-option").attr('checked', 'checked');
                $("#f-option").prop('disabled', true);
                $("#s-option").prop('disabled', true);
                $("#t-option").prop('disabled', true);
            }
            if (data.userType == 'Cuidador')
            {
                $("#s-option").attr('checked', 'checked');
                $("#f-option").prop('disabled', true);
                $("#s-option").prop('disabled', true);
                $("#t-option").prop('disabled', true);

                $("#label-visible").hide();
            }
            if (data.userType == 'Cuidado')
            {
                $("#t-option").attr('checked', 'checked');
                $("#f-option").prop('disabled', true);
                $("#s-option").prop('disabled', true);
                $("#t-option").prop('disabled', true);

                $("#label-visible").hide();
            }

            $('#name').val(data.firstName);
            $('#last-name').val(data.lastName);
            $('#address').val(data.address);
            $('#phone-number').val(data.phoneNumber);
            $('#email').val(data.email);
            $('#id').val(data.id);

            var imagetarget = document.getElementById('target-image');
            var userName = document.getElementById('strong-title');         

            imagetarget.src = data.imageFullPath;
            userName.innerText = data.email;
        },
        error: function (xhr, status, error)
        {
            toastr.error(error, "Error");
        }
    });

    /* MANEJO DE FOTOGRAFIA*/
    document.getElementById('photo').addEventListener('input', function ()
    {
        var campo = event.target;
        var valido = document.getElementById('photo-ok');

        var imagetarget = document.getElementById('target-image');

        $("#photo-ok").removeClass("span-green");
        $("#photo-ok").removeClass("span-red");

        // Carga la imagen en el src
        const [file] = campo.files
        if (file) {
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

    /* MANEJO DE NUMERO DE TELEFONO */
    jQuery("#phone-number").on('input', function (evt)
    {
        var valido = document.getElementById('phone-number-ok');

        $("#phone-number-ok").removeClass("span-green");
        $("#phone-number-ok").removeClass("span-red");

        // Permite solo números
        jQuery(this).val(jQuery(this).val().replace(/[^0-9]/g, ''));

        if (jQuery(this).val().length > 8) {
            this.value = this.value.slice(0, 8);

            $("#phone-number-ok").addClass("span-green");

            valido.innerText = "Número válido";
        }
        else {
            $("#phone-number-ok").addClass("span-red");

            valido.innerText = "Número no válido";
        }
    });

    /* SUBMIT FORM EDIT USER */
    $('#register-user').on('submit', (function (e)
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
        var imagetarget = document.getElementById('target-image');

            
        if (email == null || email == '' || email == undefined)
        {
            toastr.error('Debe introducir un email', "Error");

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

        if (validator == false) {


            if (photo.files[0] == null)
            {
                formData.append('ImageFile', imagetarget.src);
            }
            else
            {
                formData.append('ImageFile', photo.files[0]);
            }
         
            formData.append('FirstName', name);
            formData.append('LastName', lastName);
            formData.append('Email', email);
            formData.append('UserType', typeUser);
            formData.append('Address', address);
            formData.append('PhoneNumber', phoneNumber);

            $.ajax({
                type: 'POST',
                url: "/Account/ChangeUser",
                data: formData,
                contentType: false,
                processData: false,
                success: function (data)
                {
                    if (data == "Success")
                    {  
                        toastr.success(
                            'Se han guardado los cambios correctamente',
                            'Guardado',
                            {
                                timeOut: 1000,
                                fadeOut: 1000,
                                onHidden: function ()
                                {
                                    window.location.href = '/Home/Index/';
                                }
                            }
                        );
                    }
                    if (data == "Model invalid")
                    {
                        toastr.error('Ha ocurrido un problema, intente más tarde', "Error");
                    }
                },
                error: function (xhr, status, error)
                {
                    toastr.error(error, "Error");
                }
            });          
        }
     

    }));

    /* MODAL CHANGE PASSWORD */
    $("#btnCallModal").click(function ()
    {
        $("#old-password").val('');
        $("#new-password").val('');
        $("#confirm-password").val('');

        $("#change-password-modal").modal();
    });


    /* SUBMIT FORM CHANGE PASSWORD */
    $('#change-password').on('submit', (function (e)
    {
        e.preventDefault();
       
        var formData = new FormData(this);
        var validator = false;

        var oldPassword = $("#old-password").val();
        var newPassword = $("#new-password").val();
        var confirmPassword = $("#confirm-password").val();

        if (oldPassword == null || oldPassword == '' || email == undefined)
        {
            toastr.error('Debe introducir el actual password', "Error");

            $("#old-password").addClass("input-red");
            setTimeout(function () {
                $("#old-password").removeClass('input-red');
            }, 2000)

            validator = true;
        }

        if (newPassword == null || newPassword == '' || newPassword == undefined)
        {
            toastr.error('Debe introducir el nuevo password', "Error");

            $("#new-password").addClass("input-red");
            setTimeout(function () {
                $("#new-password").removeClass('input-red');
            }, 2000)

            validator = true;
        }

        if (confirmPassword == null || confirmPassword == '' || confirmPassword == undefined)
        {
            toastr.error('Debe introducir la confirmación del password', "Error");

            $("#confirm-password").addClass("input-red");
            setTimeout(function () {
                $("#confirm-password").removeClass('input-red');
            }, 2000)

            validator = true;
        }

        if (confirmPassword != newPassword)
        {
            toastr.error('La nueva contraseña y su confirmación deben coincidir', "Error");

            $("#confirm-password").addClass("input-red");
            setTimeout(function () {
                $("#confirm-password").removeClass('input-red');
            }, 2000)

            $("#new-password").addClass("input-red");
            setTimeout(function () {
                $("#new-password").removeClass('input-red');
            }, 2000)

            validator = true;
        }

     
        if (validator == false)
        {
            formData.append('OldPassword', oldPassword);
            formData.append('NewPassword', newPassword);
            formData.append('Confirm', confirmPassword);

            $.ajax({
                type: 'POST',
                url: "/Account/ChangePassword",
                data: formData,
                contentType: false,
                processData: false,
                success: function (data)
                {
                    if (data == 'Success')
                    {
                        toastr.success(
                            'Se han guardado los cambios correctamente',
                            'Guardado',
                            {
                                timeOut: 1000,
                                fadeOut: 1000,
                                onHidden: function ()
                                {
                                    $("#change-password-modal").modal('hide');
                                }
                            }
                        );
                    }
                    if (data == 'Incorrect password')
                    {
                        toastr.error('Contraseña actual incorrecta', "Error");
                    }
                    if (data == 'User not found')
                    {
                        toastr.error('Usuario no existe', "Error");
                    }
                    if (data == 'Model invalid')
                    {
                        toastr.error('Ha ocurrido un problema, intente más tarde', "Error");
                    }
                  
                },
                error: function (xhr, status, error)
                {
                    toastr.error(error, "Error");
                }
            });


        }

    }));
});
