'use strict';

$(document).ready(async function () {


    $('#form-adulto-mayor').hide();

    // Selección/Cambio de tipo de usuario
    $('#UserType').on('change', function (e)
    {
        // Valor no valido
        if ($('#UserType').val() == 0)
        {
            $('#form-adulto-mayor').hide();

            $('#email').val('');
            $('#first-name').val('');
            $('#last-name').val('');
            $('#phone-number').val('');
            $('#address').val('');
            $('#image-file').val('');
        }
        else
        {
            $('#form-adulto-mayor').show();
        }

      
    })

});
