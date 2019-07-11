var AppManager = {
    User: {
        Login : function (formData) {
            AppManager.Ajax.Post("/account/login", formData, function (result) {
                
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
                        then(data);
                    }
                }
            }).always(function (data) {
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
                    if (data && typeof (always) == "function") {
                        always(data);
                    }
                });
        }
    },
}
