$(window).ready(function () {
    $("#SearchFrontEnd").keyup(filterSearch);
    $("#InactiveItemsCb").click(filterSearch);
    $("#search_dummy").click(function () {
        $("#search-result-modal").modal("show");
        $("#search_dummy").blur();
        if (!$("#SearchFrontEnd").is(":focus") || $("#search-result-modal").hasClass("in")) {
            console.log("focus");
            $("#SearchFrontEnd").focus();
        }
    });
});

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