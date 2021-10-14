'use strict';

$(document).ready(async function () {


    $("#btnCreate").click(function ()
    {
        var careName = $("#care-name").val();
        console.log(careName);

        if (careName != '' && careName != null)
        {
            //$(".ovelay, .popup").fadeIn();

            CallActionCreate(careName);
        }
        else
        {
            alert('debe asignar un valor');
        }


      
    });

    function LoadingBar()
    {
        $("#fade-area").removeAttr("style");
        $("#myModal").removeAttr("style");
    }


    function CallActionCreate(careName)
    {
       
            $.ajax({
                type: "POST",
                url: "/Cares/Create",
                data: { care: careName },
                success: function (msg)
                {
                    console.log("Suceess operation");
                    console.log(msg);

                    if (msg == 'Success')
                    {
                        toastr.success("Cuidado guardado correctamente", "Exíto");
                    }
                    if (msg == 'Duplicate')
                    {
                        toastr.warning("Success", "Success");
                    }
                    else
                    {
                        toastr.error("Success", "Success");
                    }
                },
                error: function (req, status, error)
                {

                    console.log("Error operation");
                    alert(error);
                }
            });
       

    }

});