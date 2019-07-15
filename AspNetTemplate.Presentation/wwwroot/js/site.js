/* Private Functions */

function getColumns(type) {
    if (type == "teamlead") {
        return [
            { data: 'Id' },
            { data: 'Description' },
            { data: 'UploadDate' },
            {
                data: 'State',
                render: function (data) {
                    return renderState(data);
                }
            },
            { data: 'StateDescription' },
            {
                data: 'Link',
                render: function (data, type, row, meta) {
                    return renderViewFile(data, row);
                }
            },
            {
                data: 'Id',
                render: function (data, type, row, meta) {
                    return renderManageButtons(data, row);
                }
            }
        ];
    }
    if (type == "finance") {
        return [
            { data: 'Id' },
            { data: 'Description' },
            { data: 'UploadDate' },
            {
                data: 'State',
                render: function (data) {
                    return renderState(data);
                }
            },
            { data: 'StateDescription' },
            {
                data: 'Link',
                render: function (data, type, row, meta) {
                    return renderViewFile(data, row);
                }
            }
        ]
    }
}

function renderViewFile(data, row) {
    return '<a class="btn btn-small" href="' + data + '" data-lightbox="data" data-title="">'+ row.FileName +'</a>';
}

function renderManageButtons(data, row) {
    var html = '';

    if(row.State != "Approved")
        html = '<a class="btn btn-small accept-expense" href="#" data-id="' + data + '">Accept</a>';

    if (row.State != "Declined")
        html += '<a class="btn btn-small decline-expense" href="#" data-id="' + data + '">Decline</a>';

    return html;
}

function renderState(data) {
    var cssclass = "text-primary";

    if (data == "UnApproved") {
        cssclass = "text-warning";
    }

    if (data == "Declined") {
        cssclass = "text-danger";
    }

    if (data == "Approved") {
        cssclass = "text-success";
    }
    return '<span class="' + cssclass + '">' + data + '</sapn>';
}

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
                if (result && result.Status == 1)
                    location.reload();
                else
                    AppManager.Notification.Error(result.Messages[0]);
            });
        }
    },
    Expense: {
        ManageExpenseDatatable: {},
        RefreshManageDatatable: function (callback, restPaging) {
            if (AppManager.Expense.ManageExpenseDatatable &&
                AppManager.Expense.ManageExpenseDatatable.ajax) {
                    AppManager.Expense.ManageExpenseDatatable.ajax.reload(callback, restPaging);
            }
        },
        Accept: function (id) {
            AppManager.Ajax.Post("/account/acceptexpense", { id: id }, function (result) {
                if (result.Status == 1) {
                    AppManager.Notification.Success(result.Messages[0]);
                    AppManager.Expense.RefreshManageDatatable(null, false);
                    return;
                }

                AppManager.Notification.Error(result.messages[0]);
            });
        },
        Decline: function (id, stateDescription, callback) {
            AppManager.Ajax.Post("/account/declineexpense", { id: id, stateDescription: stateDescription}, function (result) {
                if (result.Status == 1) {
                    AppManager.Notification.Success(result.Messages[0]);
                    AppManager.Expense.RefreshManageDatatable(null, false);

                    if (callback && typeof (callback) == "function") {
                        callback(result);
                    }
                    return;
                }

                AppManager.Notification.Error(result.messages[0]);
            });
        },
        LoadAllExpenses: function (tableSelector, type) {
            AppManager.Expense.ManageExpenseDatatable  = $(tableSelector).DataTable({
                searching: false,
                dom: 'Bfrtip',
                buttons: [
                    'excelHtml5'
                ],
                ajax: {
                    url: '/account/LoadAllExpenses',
                    dataSrc: ''
                },
                "columns": getColumns(type) 
            });
        },
        LoadAllUserExpense: function (tableSelector) {
            $(tableSelector).DataTable({
                searching: false,
                ajax: {
                    url: '/account/LoadAllUserExpenses',
                    dataSrc : ''
                },
                "columns": [
                    { data: 'Id' },
                    { data: 'Description' },
                    { data: 'UploadDate' },
                    {
                        data: 'State',
                        render: function (data) {
                            return renderState(data);
                        }},
                    { data: 'StateDescription' },
                    {
                        data: 'Link',
                        render: function (data, type, row, meta) {
                            return renderViewFile(data, row);
                        }
                    }
                ]
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