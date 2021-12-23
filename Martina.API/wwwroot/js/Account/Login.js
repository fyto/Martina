'use strict';

$(document).ready(async function () {

    document.getElementById('option-1').addEventListener('click', function ()
    {
        $("#certificado").show();
    });

    document.getElementById('option-2').addEventListener('click', function ()
    {
        $("#certificado").hide();
    });



    document.getElementById('email').addEventListener('input', function () {
        var campo = event.target;
        var valido = document.getElementById('email-ok');

        $("#email-ok").removeClass("span-green");
        $("#email-ok").removeClass("span-red");

        var emailRegex = /^[-\w.%+]{1,64}@(?:[A-Z0-9-]{1,63}\.){1,125}[A-Z]{2,63}$/i;
        //Se muestra un texto a modo de ejemplo, luego va a ser un icono
        if (emailRegex.test(campo.value)) {
            $("#email-ok").addClass("span-green");

            valido.innerText = "Email válido";
        }
        else {
            $("#email-ok").addClass("span-red");

            valido.innerText = "Email inválido";
        }
    });

    document.getElementById('photo').addEventListener('input', function () {
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

        if (campo.value) {
            $("#photo-ok").addClass("span-green");

            valido.innerText = "Imagen cargada";
        }
        else {
            $("#photo-ok").addClass("span-red");

            valido.innerText = "Imagen no cargada";
        }


    });

    jQuery("#phone-number").on('input', function (evt) {
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

    $("#btnRegisterModal").click(function ()
    {
        $("#register-modal").modal();

        $("#email").val('');
        $("#name").val('');
        $("#last-name").val('');
        $("#address").val('');
        $("#phone-number").val('');       
        $("#password-register").val('');
        $("#password-confirm-register").val('');
        $("#photo").val('');
    });

    /* SUBMIT REGISTER USER */
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
        var password = $("#password-register").val();
        var passwordConfirm = $("#password-confirm-register").val();

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

        if (password != passwordConfirm)
        {
            toastr.error('La contraseña y su confirmación no coinciden', "Error");

            $("#password-register").addClass("input-red");
            $("#password-confirm-register").addClass("input-red");

            setTimeout(function () {
                $("#password-register").removeClass('input-red');
                $("#password-confirm-register").removeClass('input-red');
            }, 2000)

            validator = true;
        }

        if (password.length < 6) {
            toastr.error('La contraseña y la confirmación deben tener 6 caracteres como mínimo', "Error");

            $("#password-register").addClass("input-red");
            $("#password-confirm-register").addClass("input-red");

            setTimeout(function () {
                $("#password-register").removeClass('input-red');
                $("#password-confirm-register").removeClass('input-red');
            }, 2000)

            validator = true;
        }

        if (validator == false) {
            /* ActionCreateUser(typeUser, email, name, lastName, address, phoneNumber, photo);*/

            formData.append('ImageFile', photo.files[0]);
            formData.append('FirstName', name);
            formData.append('LastName', lastName);
            formData.append('Email', email);
            formData.append('UserType', typeUser);
            formData.append('Address', address);
            formData.append('PhoneNumber', phoneNumber);
            formData.append('Password', password);
            formData.append('PasswordConfirm', passwordConfirm);
            formData.append('UserName', email);

            $.ajax({
                type: 'POST',
                url: "/Account/Register",
                data: formData,
                contentType: false,
                processData: false,
                success: function (data)
                {
                    console.log(data);
                    if (data == 'Email send')
                    {
                        toastr.success('se ha enviado un correo de confirmación a: ' + email + '.', "Correo enviado");

                        $("#register-modal").modal('hide');
                    }
                    if (data == 'Email repeat')
                    {
                        toastr.error('el correo ' + email + ' esta siendo usado por otro usuario', "Email inválido");
                    }
                    if (data == 'Model invalid') {
                        toastr.error('Ha ocurrido un problema, intente más tarde.', "Error");
                    }                   
                },
                error: function (xhr, status, error) {
                    toastr.error(error, "Error");
                }
            });

        }

    }));

    $('#login').on('submit', (function (e)
    {      
        e.preventDefault();

        var validator = false;

        var username = $("#username").val();
        var password = $("#password").val();

        if (username == null || username == '' || username == undefined)
        {
            toastr.error('Debe introducir un usuario', "Error");

            $("#username").addClass("input-red");
            setTimeout(function () {
                $("#username").removeClass('input-red');
            }, 2000)

            validator = true;
        }

        if (password == null || password == '' || password == undefined)
        {
            toastr.error('Debe introducir una contraseña', "Error");

            $("#password").addClass("input-red");
            setTimeout(function () {
                $("#password").removeClass('input-red');
            }, 2000)

            validator = true;
        }

        if (password.length < 6)
        {
            toastr.error('Debe introducir una contraseña de mínimo 6 caracteres', "Error");

            $("#password").addClass("input-red");
            setTimeout(function () {
                $("#password").removeClass('input-red');
            }, 2000)

            validator = true;
        }

        if (!validator)
        {
            var selector = document.getElementsByName('remember');
            var valorCheckbox = '';

            for (var i = 0; i < selector.length; i++) {
                if (selector[i].checked == true) {
                    valorCheckbox = selector[i].value;
                }
            }

            if (valorCheckbox == 'remember') {
                valorCheckbox = true;
            }
            else {
                valorCheckbox = false;
            }

            var formData = new FormData(this);

            formData.append('Username', username);
            formData.append('Password', password);
            formData.append('RememberMe', valorCheckbox);

            $.ajax({
                type: 'POST',
                url: "/Account/Login",
                data: formData,
                contentType: false,
                processData: false,
                success: function (data)
                {  
                    console.log(data);
                   
                    if (data == 'Index/Home')
                    {  
                        toastr.success(
                            'Inicio de sesión correcto',
                            'Éxito',
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
                    if (data == 'Email o contraseña incorrectos')
                    {
                        toastr.error('Email o contraseña incorrectos.', "Error");
                    }
                    if (data == 'Model invalid')
                    {
                        toastr.error('Ocurrio un problema, intente más tarde.', "Error");
                    }
                },
                error: function (xhr, status, error)
                {
                    toastr.error(error, "Error");
                }
            });
        }
       
        
              
      
        
    }));

    $("#btnCallModalRecoverPassword").click(function ()
    {
        $("#recover-password-modal").modal();
    });

    /* SUBMIT RECOVER PASS USER */
    $('#recover-password').on('submit', (function (e)
    {
        e.preventDefault();

        var validator = false;
        var formData = new FormData(this);
      
        var email = $("#email-recover").val();
    
      
        if (email == null || email == '' || email == undefined) {
            toastr.error('Debe introducir un email', "Error");

            $("#email-recover").addClass("input-red");
            setTimeout(function () {
                $("#email-recover").removeClass('input-red');
            }, 2000)

            validator = true;
        }

      
        if (validator == false)
        {
            formData.append('Email', email);

            $.ajax({
                type: 'POST',
                url: "/Account/RecoverPassword",
                data: formData,
                contentType: false,
                processData: false,
                success: function (data)
                {
                    console.log(data);
                    if (data == "Success")
                    {
                        toastr.success('Las instrucciones para el cambio de contraseña han sido enviadas al email:  ' + email + '.', "Correo enviado");

                        $("#recover-password-modal").modal('hide');
                    }
                    if (data == "Email invalid")
                    {
                        toastr.error('El correo ingresado no corresponde a ningún usuario.', "Email inválido");
                    }
                    if (data == 'Model invalid')
                    {
                        toastr.error('Ha ocurrido un problema, intente más tarde.', "Error");
                    }
                  
                },
                error: function (xhr, status, error) {
                    toastr.error(error, "Error");
                }
            });

        }

    }));

});