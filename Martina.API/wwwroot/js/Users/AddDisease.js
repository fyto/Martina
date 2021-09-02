'use strict';

$(document).ready(async function () {

    $('#span-disease-types').hide();

    $('#disease-types').on('change', function (e) {
        // Valor no valido
        if ($('#disease-types').val() == 0)
        {
            $('#first-name-input').val('');
            $('#first-name').hide();
            $('#div-button').hide();
            $('#span-disease-types').show();
           
        }
        else
        {
            $('#first-name').show();
            $('#div-button').show();
            $('#span-disease-types').hide();

          
        }
      
    })

});
