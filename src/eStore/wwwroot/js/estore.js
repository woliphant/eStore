$(function () {
    // display message if modal still loaded i
    if ($("#detailsId").val() > 0) {
        var Id = $("#detailsId").val();
        CopyToModal(Id);
        $('#details_popup').modal('show');
    } //details
    // details anchor click - to load popup on catalogue
    $("a.btn-default").on("click", function (e) {
        var Id = $(this).attr("data-id");
        $("#results").text("");
        CopyToModal(Id);
    });

    if ($("#register_popup") != undefined) {
        $('#register_popup').modal('show');
    }

});

function CopyToModal(id) {
    $("#qty").val("0");
    $("#detailsGraphic").attr("src", "/img/" + $("#pgraphic" + id).val() + ".png");
    $("#description").text($("#pdescr" + id).val());
    $("#brand").text($("#pbrand" + id).val());
    $("#name").text($("#pname" + id).val());
    $("#price").text($("#pprice" + id).val());
    $("#detailsId").val(id);
}