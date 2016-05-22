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
    var data = JSON.parse($("#menuitem" + id).val());
    $("#name").text(data.ProductName);
    $("#brand").text(data.BrandName);
    $("#price").text(data.CostPrice);
    $("#description").text(data.Description);
    $("#detailsGraphic").attr("src", "/img/" + data.GraphicName + ".png");
    $("#detailsId").val(id);
}