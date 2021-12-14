'use strict';

$(document).ready(async function () {

    /* SUBMIT REGISTER USER */
    $('#reset-password').on('submit', (function (e)
    {
        e.preventDefault();
       
        var validator = false;
        var formData = new FormData(this);

        var email = $("#email-recover").val();
        var password = $("#password").val();
        var confirmPassword = $("#confirm-password").val();
        var token = $("#token").val();


        if (email == null || email == '' || email == undefined) {
            toastr.error('Debe introducir un email', "Error");

            $("#email-recover").addClass("input-red");
            setTimeout(function () {
                $("#email-recover").removeClass('input-red');
            }, 2000)

            validator = true;
        }

        if (password == null || password == '' || password == undefined) {
            toastr.error('Debe introducir un password', "Error");

            $("#password").addClass("input-red");
            setTimeout(function () {
                $("#password").removeClass('input-red');
            }, 2000)

            validator = true;
        }

        if (confirmPassword == null || confirmPassword == '' || confirmPassword == undefined) {
            toastr.error('Debe introducir una confirmación de password', "Error");

            $("#confirm-password").addClass("input-red");
            setTimeout(function () {
                $("#confirm-password").removeClass('input-red');
            }, 2000)

            validator = true;
        }

        if (confirmPassword != password)
        {
            toastr.error('La contraseña y su confirmación no coinciden', "Error");

            $("#password").addClass("input-red");
            $("#confirm-password").addClass("input-red");

            setTimeout(function () {
                $("#password").removeClass('input-red');
                $("#confirm-password").removeClass('input-red');
            }, 2000)

            validator = true;
        }

        if (password.length < 6)
        {
            toastr.error('La contraseña y la confirmación deben tener 6 caracteres como mínimo', "Error");

            $("#password").addClass("input-red");
            $("#confirm-password").addClass("input-red");

            setTimeout(function () {
                $("#password").removeClass('input-red');
                $("#confirm-password").removeClass('input-red');
            }, 2000)

            validator = true;
        }


        if (validator == false)
        {
            formData.append('UserName', email);
            formData.append('Password', password);
            formData.append('ConfirmPassword', confirmPassword);
            formData.append('Token', token);

            $.ajax({
                type: 'POST',
                url: "/Account/ResetPassword",
                data: formData,
                contentType: false,
                processData: false,
                success: function (data)
                {
                    if (data == 'Success')
                    {
                        toastr.success('La contraseña ha sido actualizada correctamente.', "Contraseña actualizada");

                        $("#email-recover").prop('disabled', true);
                        $("#password").prop('disabled', true);
                        $("#confirm-password").prop('disabled', true);
                        $("#change-password").prop('disabled', true);
                    }
                    if (data == 'Error')
                    {
                        toastr.error('La contraseña no pudo ser actualizada.', "Error");
                    }
                    if (data == 'User invalid')
                    {
                        toastr.error('No se encuentra el usuario.', "Usuario inválido");
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