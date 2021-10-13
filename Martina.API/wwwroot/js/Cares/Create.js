'use strict';

$(document).ready(async function () {


    $("#btnCreate").click(function ()
    {
        $(".ovelay, .popup").fadeIn();

        CallActionCreate();
    });

    function LoadingBar()
    {
        $("#fade-area").removeAttr("style");
        $("#myModal").removeAttr("style");
    }


    function CallActionCreate()
    {
        $.ajax({
            type: "POST",
            url: '@Url.Action("Cares","Create")',
            success: function () {
                console.log("success modal");
                setTimeout(function () { LoadingBar() }, 5000);
            },
            error: function () {
                console.log("error modal");
                setTimeout(function () { LoadingBar() }, 5000);
            }
        });

    }

});