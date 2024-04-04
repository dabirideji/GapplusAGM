
$(document).ready(function () {
    $("#AgmDisplay").click(function () {
        $.ajax({
            url: '/Controllers/ShareHolderController',
            type: 'GET',
            success: function (result) {
                $("#dashBody").html(result);
            }
        });
    });

});