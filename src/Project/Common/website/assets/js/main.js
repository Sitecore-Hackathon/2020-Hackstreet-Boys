$(function () {

    $(".choice-submit").click(function (e) {

        var $form = $(e.target).closest(".choice-form");

        $form.submit(); 

    });

});