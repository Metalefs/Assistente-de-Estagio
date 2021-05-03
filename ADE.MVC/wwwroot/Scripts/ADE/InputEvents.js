
export function FilterTableWithInput(inputId, tableId) {
    $("#" + inputId).on("change", function () {
        var value = $(this).val().toLowerCase();
        $("#" + tableId + " tr").filter(function () {
            $(this).toggle($(this).text().toLowerCase().indexOf(value) > -1);
        });
    });
}
