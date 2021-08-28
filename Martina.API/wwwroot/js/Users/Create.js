'use strict';

$(document).ready(async function () {


    $('#form-adulto-mayor').hide();


    // Selección de tipo de usuario
    $('#UserType').on('change', function (e)
    {
        if ($('#UserType').val() == 0)
        {
            $('#form-adulto-mayor').hide();
        }
        else
        {
            $('#form-adulto-mayor').show();
        }

      
    })

});
