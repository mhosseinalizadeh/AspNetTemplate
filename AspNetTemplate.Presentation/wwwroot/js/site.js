var AppManager = {
    Notification: {
        Info: function (msg) {
            toastr.info(msg);
        },
        Warning: function (msg) {
            toastr.warning(msg);
        },
        Success: function (msg) {
            toastr.success(msg);
        },
        Error: function (msg) {
            toastr.error(msg);
        }
    },
    User: {
        Login : function (formData) {
            AppManager.Ajax.Post("/account/login", formData, function (result) {
                if (result && result.status == 1)
                    location.reload();
            });
        }
    },

    Ajax: {
        Post: function (url, dataObj, then, always) {
            if (!url) {
                console.log("Please enter valid url for post data.");
                return;
            }

            var defaults = { "data": null, "method": "POST" };
            var options = { "data": dataObj, "url": url };

            // Merge defaults and options, without modifying defaults
            var settings = $.extend({}, defaults, options);
            return $.ajax(settings).then(function (data) {
                if (data) {
                    if (typeof (then) == "function") {
                        if (data.status == 1) {
                            if (data.messages != null && data.messages.length > 0) {
                                for (var i = 0; i < data.messages.length; i++) {
                                    AppManager.Notification.Success(data.messages[i])
                                }
                            }   
                        }

                        if (data.status == 2) {
                            if (data.messages != null && data.messages.length > 0) {
                                for (var i = 0; i < data.messages.length; i++) {
                                    AppManager.Notification.Error(data.messages[i])
                                }
                            }  
                        }
                        then(data);
                    }
                }
            }).always(function (data) {
                if (data.status == 500) {
                    AppManager.Notification.Error(data.statusText);
                }

                if (data && typeof (always) == "function") {
                    always(data);
                }
            });
        },
        Get: function (url, dataObj, then, always) {
            if (!url) {
                console.log("Please enter valid url for post data.");
                return;
            }

            var defaults = { "data": null, "method": "GET" };
            var options = { "data": dataObj, "url": url, "dataType": "json" };

            // Merge defaults and options, without modifying defaults
            var settings = $.extend({}, defaults, options);
            return $.ajax(settings)
                .then(function (data) {
                    if (data) {
                        if (typeof (then) == "function") {
                            then(data, this.url);
                        }
                    }
                }).always(function (data) {
                    if (data.status == 500) {
                        AppManager.Notification.Error(data.statusText);
                    }
                    if (data && typeof (always) == "function") {
                        always(data);
                    }
                });
        }
    },
}

toastr.options.preventDuplicates = true;
toastr.options.closeButton = true;
toastr.options.positionClass = "toast-bottom-right";