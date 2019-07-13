$("document").ready(function () {
    AppManager.Expense.LoadAllExpenses("#expenses");

    $("#expenses").on("click", ".accept-expense", function (event) {
        var id = $(event.currentTarget).data("id");
        AppManager.Expense.Accept(id);
    })

    $("#expenses").on("click", ".decline-expense", function (event) {
        var id = $(event.currentTarget).data("id");
        $(".btn-decline-submit").data("id", id);
        $("textarea.decline-description").html("").val("");
        $(".modal").modal();
        
    })

    $(document).on("click", ".btn-decline-submit", function () {
        var id = $(".btn-decline-submit").data("id");
        var stateDescription = $("textarea.decline-description").val();
        AppManager.Expense.Decline(id, stateDescription, function (data) {
            $(".modal").modal("hide");
        });
    });
});

