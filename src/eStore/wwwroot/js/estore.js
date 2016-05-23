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
});

function CopyToModal(id) {
    $("#qty").val("0");
    $("#description").text($("#descr" + id).data("description"));
    $("#brand").text($("#pbrand").data("brand"));
    $("#name").text($("#pname" + id).data("name"));
    $("#price").text($("#pprice" + id).val());
    $("#detailsId").val(id);
}