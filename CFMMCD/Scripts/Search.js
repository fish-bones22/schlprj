$("#SearchFrontEnd").keyup(filterSearch);
$("#InactiveItemsCb").click(filterSearch);
function filterSearch() {
    // Hide items not relevant to the search term
    if ($("#SearchFrontEnd").val() != "") {
        var searchTerm = $("#SearchFrontEnd").val().toLowerCase();
        $(".tr-collapse").addClass("hidden");
        $(".tr-collapse").each(function () {
            if ($(this).attr("class").toLowerCase().includes(searchTerm))
                if ($(this).hasClass("inactive") && ($("#InactiveItemsCb").prop("checked")))
                    $(this).removeClass("hidden");
                else if ($(this).hasClass("active"))
                    $(this).removeClass("hidden");
        });
    } else {
        $(".tr-collapse").each(function () {
            if ($(this).hasClass("inactive") && $("#InactiveItemsCb").prop("checked")) {
                $(this).removeClass("hidden");
            }
            else if ($(this).hasClass("active"))
                $(this).removeClass("hidden");
            else if (!$(this).hasClass("active") && !$(this).hasClass("inactive"))
                $(this).removeClass("hidden");
            else
                $(this).addClass("hidden");
        });

    }
}