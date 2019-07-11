$(document).on("click", "#btn-login", function (e) {
    e.preventDefault();
    var email = $("#email").val();
    var password = $("#password").val();
    var formData = {
        email: email,
        password: password
    }
    AppManager.User.Login(formData);
})